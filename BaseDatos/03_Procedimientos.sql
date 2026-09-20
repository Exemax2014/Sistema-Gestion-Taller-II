-- ============================================================
-- PROCEDIMIENTOS ALMACENADOS
-- Sistema Hierro y Forja
-- Base de datos: SistemaGestion
-- Script 03 - Procedimientos
-- Versión con códigos de resultado estandarizados
--
-- CONVENCIÓN DE RESULTADOS PARA OPERACIONES QUE MODIFICAN DATOS
-- ------------------------------------------------------------
-- 0   = Operación correcta
-- 1   = Registro no encontrado
-- 2   = Registro duplicado
-- 3   = Datos inválidos
-- 4   = Stock insuficiente
-- 5   = Operación no permitida
-- 500 = Error interno / inesperado de base de datos
--
-- Los procedimientos de consulta devuelven filas normalmente.
-- Los procedimientos de alta/modificación/baja/inventario devuelven:
--
-- @CodigoResultado  INT OUTPUT
-- @MensajeResultado NVARCHAR(250) OUTPUT
--
-- Los procedimientos de alta también devuelven:
-- @IdGenerado INT OUTPUT
--
-- IMPORTANTE:
-- - PRODUCTO contiene datos generales.
-- - INVENTARIO contiene stock por PRODUCTO + SUCURSAL.
-- - Las bajas son lógicas mediante eliminado_en.
-- - PRODUCTO.activo permite deshabilitar temporalmente un producto.
-- ============================================================

USE SistemaGestion;
GO


-- ============================================================
-- USUARIOS
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Usuario_BuscarPorNombreUsuario
    @nombreUsuario NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        u.id_usuario,
        u.id_perfil,
        u.id_sucursal,
        u.nombre,
        u.apellido,
        u.nombre_usuario,
        u.contrasena_hash,
        p.nombre AS perfil,
        p.alcance_global,
        ISNULL(s.nombre, N'Todas las sucursales') AS sucursal

    FROM dbo.USUARIO AS u

    INNER JOIN dbo.PERFIL AS p
        ON p.id_perfil = u.id_perfil

    LEFT JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = u.id_sucursal

    WHERE
        u.nombre_usuario = @nombreUsuario
        AND u.eliminado_en IS NULL
        AND p.eliminado_en IS NULL
        AND
        (
            u.id_sucursal IS NULL
            OR s.eliminado_en IS NULL
        );
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Usuario_Listar
    @activo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.id_usuario,
        u.nombre,
        u.apellido,
        u.dni,
        u.telefono,
        u.nombre_usuario,
        u.correo,
        u.sexo,
        u.fecha_nacimiento,
        u.id_perfil,
        p.nombre AS perfil,
        u.id_sucursal,
        ISNULL(s.nombre, N'Todas las sucursales') AS sucursal,
        CAST(
            CASE
                WHEN u.eliminado_en IS NULL THEN 1
                ELSE 0
            END
            AS BIT
        ) AS activo
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.id_perfil = u.id_perfil
    LEFT JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = u.id_sucursal
    WHERE
        (
            @activo IS NULL
            OR (
                @activo = 1
                AND u.eliminado_en IS NULL
            )
            OR (
                @activo = 0
                AND u.eliminado_en IS NOT NULL
            )
        )
    ORDER BY u.apellido, u.nombre;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Usuario_ObtenerPorId
    @idUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        u.id_usuario,
        u.id_perfil,
        u.id_sucursal,
        u.id_direccion,
        u.nombre,
        u.apellido,
        u.dni,
        u.telefono,
        u.nombre_usuario,
        u.correo,
        u.sexo,
        u.fecha_nacimiento,
        p.nombre AS perfil,
        ISNULL(s.nombre, N'Todas las sucursales') AS sucursal,
        CAST(
            CASE
                WHEN u.eliminado_en IS NULL THEN 1
                ELSE 0
            END
            AS BIT
        ) AS activo
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.id_perfil = u.id_perfil
    LEFT JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = u.id_sucursal
    WHERE u.id_usuario = @idUsuario;
END;
GO

/* ============================================================
   Procedimiento: sp_Usuario_Alta

   Regla de sucursal:
   - Perfil con alcance_global = 1:
     id_sucursal se guarda en NULL.
   - Perfil con alcance_global = 0:
     debe tener una sucursal asignada.

   Códigos de resultado:
   0   = Correcto
   1   = Registro relacionado inexistente
   2   = Registro duplicado
   3   = Datos inválidos
   500 = Error interno de base de datos
   ============================================================ */

CREATE OR ALTER PROCEDURE dbo.sp_Usuario_Alta
    @idPerfil INT,
    @idSucursal INT = NULL,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,

    @nombre NVARCHAR(100),
    @apellido NVARCHAR(100),
    @dni NVARCHAR(20),
    @telefono NVARCHAR(30) = NULL,
    @nombreUsuario NVARCHAR(50),
    @contrasenaHash NVARCHAR(255),
    @correo NVARCHAR(150),
    @sexo NVARCHAR(20) = NULL,
    @fechaNacimiento DATE = NULL,
    @idDireccion INT = NULL,

    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @alcanceGlobal BIT;

    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';


    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;

        SET @nombre = LTRIM(RTRIM(ISNULL(@nombre, N'')));
        SET @apellido = LTRIM(RTRIM(ISNULL(@apellido, N'')));
        SET @dni = LTRIM(RTRIM(ISNULL(@dni, N'')));
        SET @telefono = NULLIF(LTRIM(RTRIM(@telefono)), N'');
        SET @nombreUsuario = LTRIM(RTRIM(ISNULL(@nombreUsuario, N'')));
        SET @correo = NULLIF(LTRIM(RTRIM(@correo)), N'');

        /* -----------------------------------------------------
           Validar perfil
           ----------------------------------------------------- */

        SELECT
            @alcanceGlobal = alcance_global
        FROM dbo.PERFIL
        WHERE id_perfil = @idPerfil
          AND eliminado_en IS NULL;


        IF @alcanceGlobal IS NULL
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado =
                N'El perfil indicado no existe o está inactivo.';

            RETURN;
        END;


        /* -----------------------------------------------------
           Regla de sucursal según perfil
           ----------------------------------------------------- */

        IF @alcanceGlobal = 1
        BEGIN
            /*
               Los perfiles de alcance global no pertenecen
               obligatoriamente a una sucursal específica.
            */
            SET @idSucursal = NULL;
        END
        ELSE
        BEGIN

            /*
               Los perfiles sin alcance global deben estar
               asociados a una sucursal activa.
            */
            IF @idSucursal IS NULL
            BEGIN
                SET @CodigoResultado = 3;
                SET @MensajeResultado =
                    N'El perfil seleccionado requiere una sucursal.';

                RETURN;
            END;


            IF NOT EXISTS
            (
                SELECT 1
                FROM dbo.SUCURSAL
                WHERE id_sucursal = @idSucursal
                  AND eliminado_en IS NULL
            )
            BEGIN
                SET @CodigoResultado = 1;
                SET @MensajeResultado =
                    N'La sucursal indicada no existe o está inactiva.';

                RETURN;
            END;

        END;


        /* -----------------------------------------------------
           Validar campos obligatorios
           ----------------------------------------------------- */

        IF @nombre = N''
           OR @apellido = N''
           OR @dni = N''
           OR @nombreUsuario = N''
           OR LTRIM(RTRIM(ISNULL(@contrasenaHash, N''))) = N''
           OR @correo IS NULL
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado =
                N'Faltan datos obligatorios del usuario.';

            RETURN;
        END;

        IF @dni LIKE N'%[^0-9]%'
           OR @nombre LIKE N'%[0-9]%'
           OR @apellido LIKE N'%[0-9]%'
           OR @nombre LIKE N'%' + CHAR(9) + N'%'
           OR @nombre LIKE N'%' + CHAR(10) + N'%'
           OR @nombre LIKE N'%' + CHAR(13) + N'%'
           OR @apellido LIKE N'%' + CHAR(9) + N'%'
           OR @apellido LIKE N'%' + CHAR(10) + N'%'
           OR @apellido LIKE N'%' + CHAR(13) + N'%'
           OR (@telefono IS NOT NULL AND
               (@telefono NOT LIKE N'%[0-9]%' OR @telefono LIKE N'%[^0-9+() -]%'))
           OR @nombreUsuario LIKE N'%[^A-Za-z0-9._-]%'
           OR @correo NOT LIKE N'%_@_%._%'
           OR @correo LIKE N'%@%@%'
           OR @correo LIKE N'% %'
           OR @correo LIKE N'%' + CHAR(9) + N'%'
           OR @correo LIKE N'%' + CHAR(10) + N'%'
           OR @correo LIKE N'%' + CHAR(13) + N'%'
           OR (@fechaNacimiento IS NOT NULL AND @fechaNacimiento > CAST(GETDATE() AS DATE))
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Los datos del usuario tienen un formato inválido.';
            RETURN;
        END;


        /* -----------------------------------------------------
           Validar duplicados
           ----------------------------------------------------- */

        IF EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO
            WHERE dni = @dni
        )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado =
                N'Ya existe un usuario con ese DNI.';

            RETURN;
        END;


        IF EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO
            WHERE nombre_usuario = @nombreUsuario
        )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado =
                N'Ya existe un usuario con ese nombre de usuario.';

            RETURN;
        END;


        IF EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO
            WHERE correo = @correo
        )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado =
                N'Ya existe un usuario con ese correo.';

            RETURN;
        END;


        /* -----------------------------------------------------
           Registrar usuario
           ----------------------------------------------------- */

        BEGIN TRANSACTION;
        INSERT INTO dbo.USUARIO
        (
            id_perfil,
            id_sucursal,

            nombre,
            apellido,
            dni,
            telefono,

            nombre_usuario,
            contrasena_hash,
            correo,

            sexo,
            fecha_nacimiento,
            id_direccion
        )
        VALUES
        (
            @idPerfil,
            @idSucursal,

            @nombre,
            @apellido,
            @dni,
            @telefono,

            @nombreUsuario,
            @contrasenaHash,
            @correo,

            NULLIF(LTRIM(RTRIM(@sexo)), N''),
            @fechaNacimiento,
            @idDireccion
        );


        SET @IdGenerado =
            CAST(SCOPE_IDENTITY() AS INT);

        DECLARE @detalleAuditoriaUsuario NVARCHAR(300) = CONCAT(N'Usuario registrado: ', @nombreUsuario, N'.');
        DECLARE @sucursalAuditoriaUsuario INT = COALESCE(@idSucursal, @idSucursalAuditoria);
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'ALTA', @entidad = N'USUARIO',
            @idEntidad = @IdGenerado, @detalle = @detalleAuditoriaUsuario,
            @idSucursal = @sucursalAuditoriaUsuario;
        COMMIT TRANSACTION;

        SET @CodigoResultado = 0;
        SET @MensajeResultado =
            N'Usuario registrado correctamente.';


    END TRY

    BEGIN CATCH

        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;

        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();

    END CATCH;

END;
GO

/* Retira rutas históricas para que el guardado atómico sea el único flujo de perfiles. */
DROP PROCEDURE IF EXISTS dbo.sp_Perfil_Alta;
DROP PROCEDURE IF EXISTS dbo.sp_Perfil_Modificar;
DROP PROCEDURE IF EXISTS dbo.sp_Perfil_GuardarFuncionalidades;
DROP PROCEDURE IF EXISTS dbo.sp_Perfil_AsignarFuncionalidad;
DROP PROCEDURE IF EXISTS dbo.sp_Perfil_QuitarFuncionalidad;
GO

/* ============================================================
   Procedimiento: sp_Usuario_Modificar

   Regla de sucursal:
   - Perfil con alcance_global = 1:
     id_sucursal se guarda en NULL.
   - Perfil con alcance_global = 0:
     debe tener una sucursal asignada.

   Códigos de resultado:
   0   = Correcto
   1   = Registro relacionado inexistente
   2   = Registro duplicado
   3   = Datos inválidos
   500 = Error interno de base de datos
   ============================================================ */

CREATE OR ALTER PROCEDURE dbo.sp_Usuario_Modificar
    @idUsuario INT,
    @idPerfil INT,
    @idSucursal INT = NULL,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,

    @nombre NVARCHAR(100),
    @apellido NVARCHAR(100),
    @dni NVARCHAR(20),
    @telefono NVARCHAR(30) = NULL,
    @nombreUsuario NVARCHAR(50),
    @correo NVARCHAR(150),
    @sexo NVARCHAR(20) = NULL,
    @fechaNacimiento DATE = NULL,
    @idDireccion INT = NULL,

    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @alcanceGlobal BIT;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;

        SET @nombre = LTRIM(RTRIM(ISNULL(@nombre, N'')));
        SET @apellido = LTRIM(RTRIM(ISNULL(@apellido, N'')));
        SET @dni = LTRIM(RTRIM(ISNULL(@dni, N'')));
        SET @telefono = NULLIF(LTRIM(RTRIM(@telefono)), N'');
        SET @nombreUsuario = LTRIM(RTRIM(ISNULL(@nombreUsuario, N'')));
        SET @correo = NULLIF(LTRIM(RTRIM(@correo)), N'');

        /* -----------------------------------------------------
           Validar existencia del usuario
           ----------------------------------------------------- */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO
            WHERE id_usuario = @idUsuario
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado =
                N'El usuario no existe o fue dado de baja.';

            RETURN;
        END;


        /* -----------------------------------------------------
           Validar perfil
           ----------------------------------------------------- */

        SELECT
            @alcanceGlobal = alcance_global
        FROM dbo.PERFIL
        WHERE id_perfil = @idPerfil
          AND eliminado_en IS NULL;


        IF @alcanceGlobal IS NULL
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado =
                N'El perfil indicado no existe o está inactivo.';

            RETURN;
        END;


        /* -----------------------------------------------------
           Regla de sucursal según perfil
           ----------------------------------------------------- */

        IF @alcanceGlobal = 1
        BEGIN
            SET @idSucursal = NULL;
        END
        ELSE
        BEGIN

            IF @idSucursal IS NULL
            BEGIN
                SET @CodigoResultado = 3;
                SET @MensajeResultado =
                    N'El perfil seleccionado requiere una sucursal.';

                RETURN;
            END;


            IF NOT EXISTS
            (
                SELECT 1
                FROM dbo.SUCURSAL
                WHERE id_sucursal = @idSucursal
                  AND eliminado_en IS NULL
            )
            BEGIN
                SET @CodigoResultado = 1;
                SET @MensajeResultado =
                    N'La sucursal indicada no existe o está inactiva.';

                RETURN;
            END;

        END;


        /* -----------------------------------------------------
           Validar campos obligatorios
           ----------------------------------------------------- */

        IF @nombre = N''
           OR @apellido = N''
           OR @dni = N''
           OR @nombreUsuario = N''
           OR @correo IS NULL
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado =
                N'Faltan datos obligatorios del usuario.';

            RETURN;
        END;

        IF @dni LIKE N'%[^0-9]%'
           OR @nombre LIKE N'%[0-9]%'
           OR @apellido LIKE N'%[0-9]%'
           OR @nombre LIKE N'%' + CHAR(9) + N'%'
           OR @nombre LIKE N'%' + CHAR(10) + N'%'
           OR @nombre LIKE N'%' + CHAR(13) + N'%'
           OR @apellido LIKE N'%' + CHAR(9) + N'%'
           OR @apellido LIKE N'%' + CHAR(10) + N'%'
           OR @apellido LIKE N'%' + CHAR(13) + N'%'
           OR (@telefono IS NOT NULL AND
               (@telefono NOT LIKE N'%[0-9]%' OR @telefono LIKE N'%[^0-9+() -]%'))
           OR @nombreUsuario LIKE N'%[^A-Za-z0-9._-]%'
           OR @correo NOT LIKE N'%_@_%._%'
           OR @correo LIKE N'%@%@%'
           OR @correo LIKE N'% %'
           OR @correo LIKE N'%' + CHAR(9) + N'%'
           OR @correo LIKE N'%' + CHAR(10) + N'%'
           OR @correo LIKE N'%' + CHAR(13) + N'%'
           OR (@fechaNacimiento IS NOT NULL AND @fechaNacimiento > CAST(GETDATE() AS DATE))
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Los datos del usuario tienen un formato inválido.';
            RETURN;
        END;


        /* -----------------------------------------------------
           Validar duplicados
           ----------------------------------------------------- */

        IF EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO
            WHERE dni = @dni
              AND id_usuario <> @idUsuario
        )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado =
                N'Ya existe otro usuario con ese DNI.';

            RETURN;
        END;


        IF EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO
            WHERE nombre_usuario = @nombreUsuario
              AND id_usuario <> @idUsuario
        )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado =
                N'Ya existe otro usuario con ese nombre de usuario.';

            RETURN;
        END;


        IF EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO
            WHERE correo = @correo
              AND id_usuario <> @idUsuario
        )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado =
                N'Ya existe otro usuario con ese correo.';

            RETURN;
        END;


        /* -----------------------------------------------------
           Modificar usuario
           ----------------------------------------------------- */

        BEGIN TRANSACTION;
        UPDATE dbo.USUARIO
        SET
            id_perfil = @idPerfil,
            id_sucursal = @idSucursal,
            nombre = @nombre,
            apellido = @apellido,
            dni = @dni,
            telefono = @telefono,
            nombre_usuario = @nombreUsuario,
            correo = @correo,
            sexo = NULLIF(LTRIM(RTRIM(@sexo)), N''),
            fecha_nacimiento = @fechaNacimiento,
            id_direccion = @idDireccion
        WHERE id_usuario = @idUsuario
          AND eliminado_en IS NULL;

        DECLARE @detalleAuditoriaUsuario NVARCHAR(300) = CONCAT(N'Usuario actualizado: ', @nombreUsuario, N'.');
        DECLARE @sucursalAuditoriaUsuario INT = COALESCE(@idSucursal, @idSucursalAuditoria);
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'MODIFICACION', @entidad = N'USUARIO',
            @idEntidad = @idUsuario, @detalle = @detalleAuditoriaUsuario,
            @idSucursal = @sucursalAuditoriaUsuario;
        COMMIT TRANSACTION;


        SET @CodigoResultado = 0;
        SET @MensajeResultado =
            N'Usuario modificado correctamente.';


    END TRY

    BEGIN CATCH

        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;

        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();

    END CATCH;

END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Usuario_Baja
    @idUsuario INT,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO
            WHERE id_usuario = @idUsuario
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El usuario no existe o ya fue dado de baja.';
            RETURN;
        END;

        DECLARE @idSucursalObjetivo INT;
        SELECT @idSucursalObjetivo = id_sucursal FROM dbo.USUARIO WHERE id_usuario = @idUsuario;
        DECLARE @sucursalAuditoriaUsuario INT = COALESCE(@idSucursalObjetivo, @idSucursalAuditoria);
        BEGIN TRANSACTION;
        UPDATE dbo.USUARIO
        SET eliminado_en = SYSDATETIME()
        WHERE id_usuario = @idUsuario
          AND eliminado_en IS NULL;

        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'BAJA', @entidad = N'USUARIO',
            @idEntidad = @idUsuario, @detalle = N'Usuario dado de baja.',
            @idSucursal = @sucursalAuditoriaUsuario;
        COMMIT TRANSACTION;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Usuario dado de baja correctamente.';

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


-- Reactiva un usuario dado de baja sin alterar su perfil ni sucursal asignada.
CREATE OR ALTER PROCEDURE dbo.sp_Usuario_Reactivar
    @idUsuario INT,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO
            WHERE id_usuario = @idUsuario
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El usuario indicado no existe.';
            RETURN;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO
            WHERE id_usuario = @idUsuario
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El usuario ya se encuentra activo.';
            RETURN;
        END;

        DECLARE @idSucursalObjetivo INT;
        SELECT @idSucursalObjetivo = id_sucursal FROM dbo.USUARIO WHERE id_usuario = @idUsuario;
        DECLARE @sucursalAuditoriaUsuario INT = COALESCE(@idSucursalObjetivo, @idSucursalAuditoria);
        BEGIN TRANSACTION;
        UPDATE dbo.USUARIO
        SET eliminado_en = NULL
        WHERE id_usuario = @idUsuario
          AND eliminado_en IS NOT NULL;

        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'REACTIVACION', @entidad = N'USUARIO',
            @idEntidad = @idUsuario, @detalle = N'Usuario reactivado.',
            @idSucursal = @sucursalAuditoriaUsuario;
        COMMIT TRANSACTION;

        SET @MensajeResultado = N'Usuario dado de alta correctamente.';

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


-- ============================================================
-- PERFILES Y FUNCIONALIDADES
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Perfil_ObtenerFuncionalidades
    @idPerfil INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.codigo
    FROM dbo.PERFIL_FUNCIONALIDAD AS pf
    INNER JOIN dbo.PERFIL AS p
        ON p.id_perfil = pf.id_perfil
    INNER JOIN dbo.FUNCIONALIDAD AS f
        ON f.id_funcionalidad = pf.id_funcionalidad
    WHERE pf.id_perfil = @idPerfil
      AND p.eliminado_en IS NULL
      AND f.eliminado_en IS NULL
    ORDER BY f.codigo;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Perfil_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_perfil,
        nombre,
        descripcion,
        alcance_global
    FROM dbo.PERFIL
    WHERE eliminado_en IS NULL
    ORDER BY nombre;
END;
GO



-- ============================================================
-- ADMINISTRACIÓN DE TIPOS DE USUARIO Y PERMISOS
--
-- Reglas:
-- - Administrador es el único perfil global.
-- - Los perfiles creados desde la aplicación tienen alcance_global = 0.
-- - PERMISOS_GESTIONAR es exclusivo del Administrador.
-- - El Administrador no puede darse de baja.
-- - No puede darse de baja un perfil con usuarios activos.
-- - El Administrador debe conservar todas las funcionalidades.
-- ============================================================


CREATE OR ALTER PROCEDURE dbo.sp_Funcionalidad_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.id_funcionalidad,
        f.codigo,
        f.nombre,
        f.descripcion
    FROM dbo.FUNCIONALIDAD AS f
    WHERE f.eliminado_en IS NULL
    ORDER BY f.codigo;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Perfil_ObtenerPorId
    @idPerfil INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        p.id_perfil,
        p.nombre,
        p.descripcion,
        p.alcance_global,
        CAST(p.alcance_global AS BIT) AS es_administrador
    FROM dbo.PERFIL AS p
    WHERE p.id_perfil = @idPerfil
      AND p.eliminado_en IS NULL;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Perfil_ListarFuncionalidades
    @idPerfil INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @alcanceGlobal BIT = 0;

    SELECT
        @alcanceGlobal = p.alcance_global
    FROM dbo.PERFIL AS p
    WHERE p.id_perfil = @idPerfil
      AND p.eliminado_en IS NULL;


    SELECT
        f.id_funcionalidad,
        f.codigo,
        f.nombre,
        f.descripcion,

        CAST(
            CASE
                WHEN @alcanceGlobal = 1 THEN 1
                WHEN pf.id_funcionalidad IS NOT NULL THEN 1
                ELSE 0
            END
            AS BIT
        ) AS asignada,

        CAST(
            CASE
                WHEN @alcanceGlobal = 1 THEN 1
                WHEN f.codigo = N'PERMISOS_GESTIONAR' THEN 1
                ELSE 0
            END
            AS BIT
        ) AS bloqueada

    FROM dbo.FUNCIONALIDAD AS f
    LEFT JOIN dbo.PERFIL_FUNCIONALIDAD AS pf
        ON pf.id_funcionalidad = f.id_funcionalidad
       AND pf.id_perfil = @idPerfil
    WHERE f.eliminado_en IS NULL
    ORDER BY f.codigo;
END;
GO


/* ============================================================
   Procedimiento: sp_Perfil_Guardar

   Guarda un perfil no global y su conjunto completo de
   funcionalidades en una única transacción. @idPerfil NULL crea
   un perfil; un ID existente lo modifica.
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Perfil_Guardar
    @idPerfil INT = NULL,
    @nombre NVARCHAR(50),
    @descripcion NVARCHAR(200) = NULL,
    @idsFuncionalidades NVARCHAR(MAX) = N'',
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,

    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        DECLARE @alcanceGlobal BIT;
        DECLARE @esAlta BIT = CASE WHEN @idPerfil IS NULL THEN 1 ELSE 0 END;

        DECLARE @SeleccionEntrada TABLE
        (
            id_funcionalidad INT NULL
        );

        DECLARE @Seleccion TABLE
        (
            id_funcionalidad INT NOT NULL PRIMARY KEY
        );

        SET @nombre = LTRIM(RTRIM(ISNULL(@nombre, N'')));
        SET @descripcion = NULLIF(LTRIM(RTRIM(@descripcion)), N'');
        SET @idsFuncionalidades = LTRIM(RTRIM(ISNULL(@idsFuncionalidades, N'')));

        WHILE CHARINDEX(N'  ', @nombre) > 0
            SET @nombre = REPLACE(@nombre, N'  ', N' ');

        IF @nombre LIKE N'%' + CHAR(9) + N'%'
           OR @nombre LIKE N'%' + CHAR(10) + N'%'
           OR @nombre LIKE N'%' + CHAR(13) + N'%'
           OR (@descripcion IS NOT NULL AND
               (@descripcion LIKE N'%' + CHAR(9) + N'%'
                OR @descripcion LIKE N'%' + CHAR(10) + N'%'
                OR @descripcion LIKE N'%' + CHAR(13) + N'%'))
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El nombre y la descripción no pueden contener caracteres de control.';
            RETURN;
        END;

        IF @nombre = N''
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El nombre del tipo de usuario es obligatorio.';
            RETURN;
        END;

        IF @esAlta = 0 AND @idPerfil <= 0
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El tipo de usuario indicado no es válido.';
            RETURN;
        END;

        IF @esAlta = 0
        BEGIN
            SELECT @alcanceGlobal = p.alcance_global
            FROM dbo.PERFIL AS p
            WHERE p.id_perfil = @idPerfil
              AND p.eliminado_en IS NULL;

            IF @alcanceGlobal IS NULL
            BEGIN
                SET @CodigoResultado = 1;
                SET @MensajeResultado = N'El tipo de usuario no existe o fue dado de baja.';
                RETURN;
            END;

            IF @alcanceGlobal = 1
            BEGIN
                SET @CodigoResultado = 5;
                SET @MensajeResultado = N'El perfil global del sistema no puede modificarse.';
                RETURN;
            END;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.PERFIL AS p
            WHERE UPPER(p.nombre) = UPPER(@nombre)
              AND (@esAlta = 1 OR p.id_perfil <> @idPerfil)
        )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado = N'Ya existe un tipo de usuario con ese nombre.';
            RETURN;
        END;

        IF @idsFuncionalidades LIKE N',%'
           OR @idsFuncionalidades LIKE N'%,'
           OR @idsFuncionalidades LIKE N'%,,%'
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'La lista de funcionalidades contiene valores no válidos.';
            RETURN;
        END;

        IF @idsFuncionalidades <> N''
        BEGIN
            INSERT INTO @SeleccionEntrada (id_funcionalidad)
            SELECT TRY_CONVERT(INT, LTRIM(RTRIM(value)))
            FROM STRING_SPLIT(@idsFuncionalidades, N',');

            IF EXISTS
            (
                SELECT 1
                FROM @SeleccionEntrada
                WHERE id_funcionalidad IS NULL
                   OR id_funcionalidad <= 0
            )
            BEGIN
                SET @CodigoResultado = 3;
                SET @MensajeResultado = N'La lista de funcionalidades contiene valores no válidos.';
                RETURN;
            END;

            IF (SELECT COUNT(*) FROM @SeleccionEntrada)
               <>
               (SELECT COUNT(DISTINCT id_funcionalidad) FROM @SeleccionEntrada)
            BEGIN
                SET @CodigoResultado = 3;
                SET @MensajeResultado = N'La lista de funcionalidades contiene valores duplicados.';
                RETURN;
            END;

            INSERT INTO @Seleccion (id_funcionalidad)
            SELECT id_funcionalidad
            FROM @SeleccionEntrada;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM @Seleccion AS seleccion
            LEFT JOIN dbo.FUNCIONALIDAD AS f
                ON f.id_funcionalidad = seleccion.id_funcionalidad
               AND f.eliminado_en IS NULL
            WHERE f.id_funcionalidad IS NULL
        )
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Se seleccionó una funcionalidad inexistente o inactiva.';
            RETURN;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM @Seleccion AS seleccion
            INNER JOIN dbo.FUNCIONALIDAD AS f
                ON f.id_funcionalidad = seleccion.id_funcionalidad
            WHERE f.codigo = N'PERMISOS_GESTIONAR'
              AND f.eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 5;
            SET @MensajeResultado = N'La administración de permisos es exclusiva del perfil global.';
            RETURN;
        END;

        -- Evita alcances de Reportes incompatibles incluso ante llamadas directas al procedimiento.
        IF
        (
            SELECT COUNT(*)
            FROM @Seleccion AS seleccion
            INNER JOIN dbo.FUNCIONALIDAD AS f
                ON f.id_funcionalidad = seleccion.id_funcionalidad
            WHERE f.codigo IN
            (
                N'REPORTES_ALCANCE_PROPIO',
                N'REPORTES_ALCANCE_SUCURSAL',
                N'REPORTES_ALCANCE_GLOBAL'
            )
        ) > 1
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Solo puede asignarse un alcance de Reportes por perfil.';
            RETURN;
        END;

        IF ISNULL(@alcanceGlobal, 0) = 0
           AND EXISTS
           (
               SELECT 1
               FROM @Seleccion AS seleccion
               INNER JOIN dbo.FUNCIONALIDAD AS f
                   ON f.id_funcionalidad = seleccion.id_funcionalidad
               WHERE f.codigo = N'REPORTES_ALCANCE_GLOBAL'
           )
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El alcance global de Reportes requiere un perfil global.';
            RETURN;
        END;

        BEGIN TRANSACTION;

        IF @esAlta = 1
        BEGIN
            INSERT INTO dbo.PERFIL
            (
                nombre,
                descripcion,
                alcance_global
            )
            VALUES
            (
                @nombre,
                @descripcion,
                0
            );

            SET @idPerfil = CAST(SCOPE_IDENTITY() AS INT);
        END;
        ELSE
        BEGIN
            UPDATE dbo.PERFIL
            SET
                nombre = @nombre,
                descripcion = @descripcion
            WHERE id_perfil = @idPerfil
              AND eliminado_en IS NULL;
        END;

        DELETE FROM dbo.PERFIL_FUNCIONALIDAD
        WHERE id_perfil = @idPerfil;

        INSERT INTO dbo.PERFIL_FUNCIONALIDAD
        (
            id_perfil,
            id_funcionalidad
        )
        SELECT
            @idPerfil,
            seleccion.id_funcionalidad
        FROM @Seleccion AS seleccion;

        DECLARE @accionAuditoriaPerfil NVARCHAR(50) = CASE WHEN @esAlta = 1 THEN N'ALTA' ELSE N'MODIFICACION' END;
        DECLARE @detalleAuditoriaPerfil NVARCHAR(300) = CONCAT(N'Tipo de usuario guardado: ', @nombre, N'.');
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = @accionAuditoriaPerfil, @entidad = N'PERFIL',
            @idEntidad = @idPerfil, @detalle = @detalleAuditoriaPerfil,
            @idSucursal = @idSucursalAuditoria;

        COMMIT TRANSACTION;

        SET @IdGenerado = @idPerfil;
        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'El tipo de usuario y sus permisos se guardaron correctamente.';

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        SET @IdGenerado = 0;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = LEFT(ERROR_MESSAGE(), 250);
        THROW;

    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Perfil_Baja
    @idPerfil INT,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,

    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;

        DECLARE @alcanceGlobal BIT;


        SELECT
            @alcanceGlobal = p.alcance_global
        FROM dbo.PERFIL AS p
        WHERE p.id_perfil = @idPerfil
          AND p.eliminado_en IS NULL;


        IF @alcanceGlobal IS NULL
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado =
                N'El tipo de usuario no existe o ya fue dado de baja.';
            RETURN;
        END;


        IF @alcanceGlobal = 1
        BEGIN
            SET @CodigoResultado = 5;
            SET @MensajeResultado =
                N'El perfil global del sistema no puede darse de baja.';
            RETURN;
        END;


        IF EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO AS u
            WHERE u.id_perfil = @idPerfil
              AND u.eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 5;
            SET @MensajeResultado =
                N'No se puede dar de baja este tipo de usuario porque tiene usuarios activos asignados.';
            RETURN;
        END;


        BEGIN TRANSACTION;
        UPDATE dbo.PERFIL
        SET eliminado_en = SYSDATETIME()
        WHERE id_perfil = @idPerfil
          AND eliminado_en IS NULL;

        DECLARE @nombrePerfilAuditoria NVARCHAR(50);
        SELECT @nombrePerfilAuditoria = nombre FROM dbo.PERFIL WHERE id_perfil = @idPerfil;
        DECLARE @detalleAuditoriaPerfilBaja NVARCHAR(300) = CONCAT(N'Tipo de usuario dado de baja: ', @nombrePerfilAuditoria, N'.');
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'BAJA', @entidad = N'PERFIL',
            @idEntidad = @idPerfil, @detalle = @detalleAuditoriaPerfilBaja,
            @idSucursal = @idSucursalAuditoria;
        COMMIT TRANSACTION;


        SET @CodigoResultado = 0;
        SET @MensajeResultado =
            N'Tipo de usuario dado de baja correctamente.';

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;

    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Perfil_SincronizarAdministrador
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        DECLARE @idPerfilGlobal INT;


        IF (
            SELECT COUNT(*)
            FROM dbo.PERFIL
            WHERE alcance_global = 1
              AND eliminado_en IS NULL
        ) <> 1
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado =
                N'Debe existir exactamente un perfil global activo.';
            RETURN;
        END;


        SELECT
            @idPerfilGlobal = p.id_perfil
        FROM dbo.PERFIL AS p
        WHERE p.alcance_global = 1
           AND p.eliminado_en IS NULL;

        -- El perfil global conserva un único alcance efectivo para Reportes.
        DELETE pf
        FROM dbo.PERFIL_FUNCIONALIDAD AS pf
        INNER JOIN dbo.FUNCIONALIDAD AS f
            ON f.id_funcionalidad = pf.id_funcionalidad
        WHERE pf.id_perfil = @idPerfilGlobal
          AND f.codigo IN
          (
              N'REPORTES_ALCANCE_PROPIO',
              N'REPORTES_ALCANCE_SUCURSAL'
          );


        INSERT INTO dbo.PERFIL_FUNCIONALIDAD
        (
            id_perfil,
            id_funcionalidad
        )
        SELECT
            @idPerfilGlobal,
            f.id_funcionalidad
        FROM dbo.FUNCIONALIDAD AS f
        WHERE f.eliminado_en IS NULL
          AND f.codigo NOT IN
          (
              N'REPORTES_ALCANCE_PROPIO',
              N'REPORTES_ALCANCE_SUCURSAL'
          )
          AND NOT EXISTS
          (
              SELECT 1
              FROM dbo.PERFIL_FUNCIONALIDAD AS pf
              WHERE pf.id_perfil = @idPerfilGlobal
                AND pf.id_funcionalidad = f.id_funcionalidad
          );


        SET @CodigoResultado = 0;
        SET @MensajeResultado =
            N'Perfil global sincronizado con todas las funcionalidades.';

    END TRY
    BEGIN CATCH

        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();

    END CATCH;
END;
GO


-- ============================================================
-- UBICACIONES (PROVINCIA / LOCALIDAD / DIRECCION)
--
-- Se agregan estos procedimientos porque Clientes (y a futuro
-- Usuarios/Sucursales) necesitan cargar provincia y localidad
-- dinámicamente, y crear/modificar la DIRECCION asociada antes
-- de dar de alta o modificar el registro que la usa.
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Provincia_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_provincia,
        nombre
    FROM dbo.PROVINCIA
    WHERE eliminado_en IS NULL
    ORDER BY nombre;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Localidad_ListarPorProvincia
    @idProvincia INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_localidad,
        nombre,
        codigo_postal
    FROM dbo.LOCALIDAD
    WHERE id_provincia = @idProvincia
      AND eliminado_en IS NULL
    ORDER BY nombre;
END;
GO


/* ============================================================
   Procedimiento: sp_Direccion_Alta

   Códigos de resultado:
   0   = Correcto
   1   = Registro relacionado inexistente (localidad)
   3   = Datos inválidos
   500 = Error interno de base de datos
   ============================================================ */

/* ============================================================
   Obtiene una localidad activa por provincia y nombre o la crea.
   La comparación normaliza espacios y no distingue mayúsculas.
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Localidad_ObtenerOCrear
    @idProvincia INT,
    @nombre NVARCHAR(100),
    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';
    SET @nombre = LTRIM(RTRIM(ISNULL(@nombre, N'')));
    SET @nombre = REPLACE(@nombre, CHAR(9), N' ');

    WHILE CHARINDEX(N'  ', @nombre) > 0
        SET @nombre = REPLACE(@nombre, N'  ', N' ');

    BEGIN TRY
        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.PROVINCIA
            WHERE id_provincia = @idProvincia
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La provincia indicada no existe o está inactiva.';
            RETURN;
        END;

        IF @nombre = N''
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El nombre de la localidad es obligatorio.';
            RETURN;
        END;

        IF LEN(@nombre) > 100
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El nombre de la localidad supera el máximo de 100 caracteres.';
            RETURN;
        END;

        SELECT TOP 1 @IdGenerado = id_localidad
        FROM dbo.LOCALIDAD
        WHERE id_provincia = @idProvincia
          AND UPPER(LTRIM(RTRIM(nombre))) = UPPER(@nombre);

        IF @IdGenerado > 0
        BEGIN
            UPDATE dbo.LOCALIDAD
            SET nombre = @nombre,
                eliminado_en = NULL
            WHERE id_localidad = @IdGenerado;

            SET @MensajeResultado = N'Localidad existente seleccionada correctamente.';
            RETURN;
        END;

        INSERT INTO dbo.LOCALIDAD (id_provincia, nombre)
        VALUES (@idProvincia, @nombre);

        SET @IdGenerado = CAST(SCOPE_IDENTITY() AS INT);
        SET @MensajeResultado = N'Localidad creada correctamente.';
    END TRY
    BEGIN CATCH
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Direccion_Alta
    @idLocalidad INT,
    @calle NVARCHAR(150),
    @altura NVARCHAR(20) = NULL,

    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.LOCALIDAD
            WHERE id_localidad = @idLocalidad
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La localidad indicada no existe o está inactiva.';
            RETURN;
        END;

        IF LTRIM(RTRIM(ISNULL(@calle, N''))) = N''
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'La calle es obligatoria.';
            RETURN;
        END;

        INSERT INTO dbo.DIRECCION
        (
            id_localidad,
            calle,
            altura
        )
        VALUES
        (
            @idLocalidad,
            LTRIM(RTRIM(@calle)),
            NULLIF(LTRIM(RTRIM(@altura)), N'')
        );

        SET @IdGenerado = CAST(SCOPE_IDENTITY() AS INT);
        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Dirección registrada correctamente.';

    END TRY
    BEGIN CATCH
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


/* ============================================================
   Procedimiento: sp_Direccion_Modificar

   Códigos de resultado:
   0   = Correcto
   1   = Registro relacionado inexistente (dirección o localidad)
   3   = Datos inválidos
   500 = Error interno de base de datos
   ============================================================ */

CREATE OR ALTER PROCEDURE dbo.sp_Direccion_Modificar
    @idDireccion INT,
    @idLocalidad INT,
    @calle NVARCHAR(150),
    @altura NVARCHAR(20) = NULL,

    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.DIRECCION
            WHERE id_direccion = @idDireccion
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La dirección no existe o fue dada de baja.';
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.LOCALIDAD
            WHERE id_localidad = @idLocalidad
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La localidad indicada no existe o está inactiva.';
            RETURN;
        END;

        IF LTRIM(RTRIM(ISNULL(@calle, N''))) = N''
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'La calle es obligatoria.';
            RETURN;
        END;

        UPDATE dbo.DIRECCION
        SET
            id_localidad = @idLocalidad,
            calle = LTRIM(RTRIM(@calle)),
            altura = NULLIF(LTRIM(RTRIM(@altura)), N'')
        WHERE id_direccion = @idDireccion
          AND eliminado_en IS NULL;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Dirección modificada correctamente.';

    END TRY
    BEGIN CATCH
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


-- ============================================================
-- CLIENTES
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Cliente_ObtenerPorId
    @idCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        c.id_cliente,
        c.nombre,
        c.apellido,
        c.documento,
        c.correo,
        c.telefono,
        c.id_direccion,
        d.calle,
        d.altura,
        l.id_localidad,
        l.nombre AS localidad,
        p.id_provincia,
        p.nombre AS provincia
    FROM dbo.CLIENTE AS c
    LEFT JOIN dbo.DIRECCION AS d
        ON d.id_direccion = c.id_direccion
       AND d.eliminado_en IS NULL
    LEFT JOIN dbo.LOCALIDAD AS l
        ON l.id_localidad = d.id_localidad
       AND l.eliminado_en IS NULL
    LEFT JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
       AND p.eliminado_en IS NULL
    WHERE c.id_cliente = @idCliente
      AND c.eliminado_en IS NULL;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Alta
    @nombre NVARCHAR(100),
    @apellido NVARCHAR(100),
    @documento NVARCHAR(20),
    @correo NVARCHAR(150) = NULL,
    @telefono NVARCHAR(30) = NULL,
    @idDireccion INT = NULL,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,

    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    SET @nombre = LTRIM(RTRIM(ISNULL(@nombre, N'')));
    SET @apellido = LTRIM(RTRIM(ISNULL(@apellido, N'')));
    SET @documento = LTRIM(RTRIM(ISNULL(@documento, N'')));
    SET @correo = NULLIF(LTRIM(RTRIM(@correo)), N'');
    SET @telefono = NULLIF(LTRIM(RTRIM(@telefono)), N'');

    BEGIN TRY

        IF @nombre = N''
           OR @apellido = N''
           OR @documento = N''
           OR @idDireccion IS NULL
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Nombre, apellido, documento y dirección son obligatorios.';
            RETURN;
        END;

        IF @nombre LIKE N'%[0-9]%'
           OR @apellido LIKE N'%[0-9]%'
           OR @documento LIKE N'%[^0-9]%'
           OR (@telefono IS NOT NULL AND
               (@telefono NOT LIKE N'%[0-9]%' OR @telefono LIKE N'%[^0-9+() -]%'))
           OR (@correo IS NOT NULL AND
               (@correo NOT LIKE N'%_@_%._%' OR @correo LIKE N'% %'))
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Los datos del cliente tienen un formato inválido.';
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.DIRECCION AS d
            INNER JOIN dbo.LOCALIDAD AS l
                ON l.id_localidad = d.id_localidad
               AND l.eliminado_en IS NULL
            INNER JOIN dbo.PROVINCIA AS p
                ON p.id_provincia = l.id_provincia
               AND p.eliminado_en IS NULL
            WHERE d.id_direccion = @idDireccion
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La dirección indicada no existe o no está activa.';
            RETURN;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.CLIENTE
            WHERE documento = @documento
        )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado = N'Ya existe un cliente con ese documento.';
            RETURN;
        END;

        BEGIN TRANSACTION;
        INSERT INTO dbo.CLIENTE
        (
            nombre,
            apellido,
            documento,
            correo,
            telefono,
            id_direccion
        )
        VALUES
        (
            @nombre,
            @apellido,
            @documento,
            @correo,
            @telefono,
            @idDireccion
        );

        SET @IdGenerado = CAST(SCOPE_IDENTITY() AS INT);
        DECLARE @detalleAuditoria NVARCHAR(300) = CONCAT(N'Cliente registrado: ', @nombre, N' ', @apellido);
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'ALTA', @entidad = N'CLIENTE',
            @idEntidad = @IdGenerado, @detalle = @detalleAuditoria,
            @idSucursal = @idSucursalAuditoria;
        COMMIT TRANSACTION;
        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Cliente registrado correctamente.';

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
        THROW;
    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Modificar
    @idCliente INT,
    @nombre NVARCHAR(100),
    @apellido NVARCHAR(100),
    @documento NVARCHAR(20),
    @correo NVARCHAR(150) = NULL,
    @telefono NVARCHAR(30) = NULL,
    @idDireccion INT = NULL,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,

    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    SET @nombre = LTRIM(RTRIM(ISNULL(@nombre, N'')));
    SET @apellido = LTRIM(RTRIM(ISNULL(@apellido, N'')));
    SET @documento = LTRIM(RTRIM(ISNULL(@documento, N'')));
    SET @correo = NULLIF(LTRIM(RTRIM(@correo)), N'');
    SET @telefono = NULLIF(LTRIM(RTRIM(@telefono)), N'');

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.CLIENTE
            WHERE id_cliente = @idCliente
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El cliente no existe o fue dado de baja.';
            RETURN;
        END;

        IF @nombre = N''
           OR @apellido = N''
           OR @documento = N''
           OR @idDireccion IS NULL
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Nombre, apellido, documento y dirección son obligatorios.';
            RETURN;
        END;

        IF @nombre LIKE N'%[0-9]%'
           OR @apellido LIKE N'%[0-9]%'
           OR @documento LIKE N'%[^0-9]%'
           OR (@telefono IS NOT NULL AND
               (@telefono NOT LIKE N'%[0-9]%' OR @telefono LIKE N'%[^0-9+() -]%'))
           OR (@correo IS NOT NULL AND
               (@correo NOT LIKE N'%_@_%._%' OR @correo LIKE N'% %'))
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Los datos del cliente tienen un formato inválido.';
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.DIRECCION AS d
            INNER JOIN dbo.LOCALIDAD AS l
                ON l.id_localidad = d.id_localidad
               AND l.eliminado_en IS NULL
            INNER JOIN dbo.PROVINCIA AS p
                ON p.id_provincia = l.id_provincia
               AND p.eliminado_en IS NULL
            WHERE d.id_direccion = @idDireccion
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La dirección indicada no existe o no está activa.';
            RETURN;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.CLIENTE
            WHERE documento = @documento
              AND id_cliente <> @idCliente
        )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado = N'Ya existe otro cliente con ese documento.';
            RETURN;
        END;

        BEGIN TRANSACTION;
        UPDATE dbo.CLIENTE
        SET
            nombre = @nombre,
            apellido = @apellido,
            documento = @documento,
            correo = @correo,
            telefono = @telefono,
            id_direccion = @idDireccion
        WHERE id_cliente = @idCliente
          AND eliminado_en IS NULL;

        DECLARE @detalleAuditoria NVARCHAR(300) = CONCAT(N'Cliente actualizado: ', @nombre, N' ', @apellido);
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'MODIFICACION', @entidad = N'CLIENTE',
            @idEntidad = @idCliente, @detalle = @detalleAuditoria,
            @idSucursal = @idSucursalAuditoria;
        COMMIT TRANSACTION;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Cliente modificado correctamente.';

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
        THROW;
    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Baja
    @id_cliente INT,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.CLIENTE
            WHERE id_cliente = @id_cliente
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El cliente no existe o ya fue dado de baja.';
            RETURN;
        END;

        BEGIN TRANSACTION;
        UPDATE dbo.CLIENTE
        SET eliminado_en = SYSDATETIME()
        WHERE id_cliente = @id_cliente
          AND eliminado_en IS NULL;

        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'BAJA', @entidad = N'CLIENTE',
            @idEntidad = @id_cliente, @detalle = N'Cliente dado de baja.',
            @idSucursal = @idSucursalAuditoria;
        COMMIT TRANSACTION;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Cliente dado de baja correctamente.';

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


-- ============================================================
-- CATEGORÍAS
-- ============================================================

/* ============================================================
   CLIENTES - versión vigente con filtro de estado y reactivación
   ============================================================ */

CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Buscar
    @texto NVARCHAR(100),
    @estado NVARCHAR(10) = N'ACTIVOS'
AS
BEGIN
    SET NOCOUNT ON;
    SET @texto = LTRIM(RTRIM(ISNULL(@texto, N'')));
    SET @estado = UPPER(LTRIM(RTRIM(ISNULL(@estado, N'ACTIVOS'))));

    SELECT TOP 50
        c.id_cliente, c.nombre, c.apellido, c.documento, c.correo, c.telefono,
        l.nombre AS localidad, p.nombre AS provincia, d.calle, d.altura,
        CAST(CASE WHEN c.eliminado_en IS NULL THEN 1 ELSE 0 END AS BIT) AS activo
    FROM dbo.CLIENTE AS c
    LEFT JOIN dbo.DIRECCION AS d ON d.id_direccion = c.id_direccion AND d.eliminado_en IS NULL
    LEFT JOIN dbo.LOCALIDAD AS l ON l.id_localidad = d.id_localidad AND l.eliminado_en IS NULL
    LEFT JOIN dbo.PROVINCIA AS p ON p.id_provincia = l.id_provincia AND p.eliminado_en IS NULL
    WHERE (@estado = N'TODOS'
           OR (@estado = N'ACTIVOS' AND c.eliminado_en IS NULL)
           OR (@estado = N'BAJA' AND c.eliminado_en IS NOT NULL))
      AND (@texto = N'' OR c.nombre LIKE N'%' + @texto + N'%'
           OR c.apellido LIKE N'%' + @texto + N'%'
           OR c.documento LIKE N'%' + @texto + N'%'
           OR ISNULL(c.correo, N'') LIKE N'%' + @texto + N'%'
           OR ISNULL(c.telefono, N'') LIKE N'%' + @texto + N'%')
    ORDER BY c.apellido, c.nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Listar
    @estado NVARCHAR(10) = N'ACTIVOS'
AS
BEGIN
    SET NOCOUNT ON;
    SET @estado = UPPER(LTRIM(RTRIM(ISNULL(@estado, N'ACTIVOS'))));

    SELECT
        c.id_cliente, c.nombre, c.apellido, c.documento, c.correo, c.telefono,
        c.id_direccion, l.nombre AS localidad, p.nombre AS provincia, d.calle, d.altura,
        CAST(CASE WHEN c.eliminado_en IS NULL THEN 1 ELSE 0 END AS BIT) AS activo
    FROM dbo.CLIENTE AS c
    LEFT JOIN dbo.DIRECCION AS d ON d.id_direccion = c.id_direccion AND d.eliminado_en IS NULL
    LEFT JOIN dbo.LOCALIDAD AS l ON l.id_localidad = d.id_localidad AND l.eliminado_en IS NULL
    LEFT JOIN dbo.PROVINCIA AS p ON p.id_provincia = l.id_provincia AND p.eliminado_en IS NULL
    WHERE @estado = N'TODOS'
       OR (@estado = N'ACTIVOS' AND c.eliminado_en IS NULL)
       OR (@estado = N'BAJA' AND c.eliminado_en IS NOT NULL)
    ORDER BY c.apellido, c.nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Reactivar
    @id_cliente INT,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.CLIENTE WHERE id_cliente = @id_cliente)
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El cliente indicado no existe.';
            RETURN;
        END;

        IF EXISTS (SELECT 1 FROM dbo.CLIENTE WHERE id_cliente = @id_cliente AND eliminado_en IS NULL)
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El cliente ya se encuentra activo.';
            RETURN;
        END;

        BEGIN TRANSACTION;
        UPDATE dbo.CLIENTE
        SET eliminado_en = NULL
        WHERE id_cliente = @id_cliente
          AND eliminado_en IS NOT NULL;

        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'REACTIVACION', @entidad = N'CLIENTE',
            @idEntidad = @id_cliente, @detalle = N'Cliente reactivado.',
            @idSucursal = @idSucursalAuditoria;
        COMMIT TRANSACTION;

        SET @MensajeResultado = N'Cliente dado de alta correctamente.';
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Categoria_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_categoria,
        nombre,
        descripcion
    FROM dbo.CATEGORIA
    WHERE eliminado_en IS NULL
    ORDER BY nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Categoria_ListarGestion
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.id_categoria, c.nombre, c.descripcion,
           CONVERT(BIT, CASE WHEN c.eliminado_en IS NULL THEN 1 ELSE 0 END) AS activo,
           COUNT(p.id_producto) AS cantidad_productos
    FROM dbo.CATEGORIA c
    LEFT JOIN dbo.PRODUCTO p ON p.id_categoria=c.id_categoria
    GROUP BY c.id_categoria,c.nombre,c.descripcion,c.eliminado_en
    ORDER BY c.nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Categoria_Guardar
    @idCategoria INT,
    @nombre NVARCHAR(100),
    @descripcion NVARCHAR(200)=NULL,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT=NULL,
    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    SET @CodigoResultado=0; SET @MensajeResultado=N'Categoría guardada.'; SET @IdGenerado=0;
    SET @nombre=LTRIM(RTRIM(ISNULL(@nombre,N'')));
    WHILE CHARINDEX(N'  ',@nombre)>0 SET @nombre=REPLACE(@nombre,N'  ',N' ');
    SET @descripcion=NULLIF(LTRIM(RTRIM(@descripcion)),N'');
    DECLARE @permiso NVARCHAR(50)=CASE WHEN @idCategoria=0 THEN N'CATEGORIAS_ALTA' ELSE N'CATEGORIAS_MODIFICAR' END;
    IF @nombre=N'' OR LEN(@nombre)>100 BEGIN SET @CodigoResultado=1; SET @MensajeResultado=N'El nombre de categoría es obligatorio y admite hasta 100 caracteres.'; RETURN; END;
    IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO u JOIN dbo.PERFIL p ON p.id_perfil=u.id_perfil JOIN dbo.PERFIL_FUNCIONALIDAD pf ON pf.id_perfil=p.id_perfil JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad=pf.id_funcionalidad WHERE u.id_usuario=@idUsuarioEjecutor AND u.eliminado_en IS NULL AND p.eliminado_en IS NULL AND f.codigo=@permiso AND f.eliminado_en IS NULL)
        THROW 51000,'El usuario no está activo o no tiene permiso para gestionar categorías.',1;
    IF EXISTS (SELECT 1 FROM dbo.CATEGORIA WHERE UPPER(LTRIM(RTRIM(nombre)))=UPPER(@nombre) AND id_categoria<>@idCategoria)
    BEGIN SET @CodigoResultado=2; SET @MensajeResultado=N'Ya existe una categoría con ese nombre.'; RETURN; END;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF @idCategoria=0
        BEGIN
            INSERT dbo.CATEGORIA(nombre,descripcion) VALUES(@nombre,@descripcion);
            SET @IdGenerado=CONVERT(INT,SCOPE_IDENTITY());
        END
        ELSE
        BEGIN
            UPDATE dbo.CATEGORIA SET nombre=@nombre,descripcion=@descripcion WHERE id_categoria=@idCategoria AND eliminado_en IS NULL;
            IF @@ROWCOUNT=0 THROW 51001,'La categoría no existe o está inactiva.',1;
            SET @IdGenerado=@idCategoria;
        END;
        DECLARE @accion NVARCHAR(50)=CASE WHEN @idCategoria=0 THEN N'ALTA' ELSE N'MODIFICACION' END;
        DECLARE @detalle NVARCHAR(500)=CONCAT(N'Categoría ',LOWER(@accion),N': ',@nombre,N'.');
        EXEC dbo.sp_Auditoria_Registrar @idUsuario=@idUsuarioEjecutor,@accion=@accion,@entidad=N'CATEGORIA',@idEntidad=@IdGenerado,@detalle=@detalle,@idSucursal=@idSucursalAuditoria;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE()<>0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Categoria_Baja
    @idCategoria INT,@idUsuarioEjecutor INT,@idSucursalAuditoria INT=NULL,
    @CodigoResultado INT OUTPUT,@MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON; SET @CodigoResultado=0;
    IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO u JOIN dbo.PERFIL p ON p.id_perfil=u.id_perfil JOIN dbo.PERFIL_FUNCIONALIDAD pf ON pf.id_perfil=p.id_perfil JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad=pf.id_funcionalidad WHERE u.id_usuario=@idUsuarioEjecutor AND u.eliminado_en IS NULL AND p.eliminado_en IS NULL AND f.codigo=N'CATEGORIAS_BAJA' AND f.eliminado_en IS NULL) THROW 51000,'El usuario no está activo o no tiene permiso para dar de baja categorías.',1;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE dbo.CATEGORIA SET eliminado_en=SYSDATETIME() WHERE id_categoria=@idCategoria AND eliminado_en IS NULL;
        IF @@ROWCOUNT=0 THROW 51001,'La categoría no existe o ya está inactiva.',1;
        DECLARE @nombre NVARCHAR(100); SELECT @nombre=nombre FROM dbo.CATEGORIA WHERE id_categoria=@idCategoria;
        DECLARE @detalle NVARCHAR(500)=CONCAT(N'Categoría dada de baja: ',@nombre,N'.');
        EXEC dbo.sp_Auditoria_Registrar @idUsuario=@idUsuarioEjecutor,@accion=N'BAJA',@entidad=N'CATEGORIA',@idEntidad=@idCategoria,@detalle=@detalle,@idSucursal=@idSucursalAuditoria;
        COMMIT TRANSACTION; SET @MensajeResultado=N'Categoría dada de baja.';
    END TRY BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK TRANSACTION; THROW; END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Categoria_Reactivar
    @idCategoria INT,@idUsuarioEjecutor INT,@idSucursalAuditoria INT=NULL,
    @CodigoResultado INT OUTPUT,@MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON; SET @CodigoResultado=0;
    IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO u JOIN dbo.PERFIL p ON p.id_perfil=u.id_perfil JOIN dbo.PERFIL_FUNCIONALIDAD pf ON pf.id_perfil=p.id_perfil JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad=pf.id_funcionalidad WHERE u.id_usuario=@idUsuarioEjecutor AND u.eliminado_en IS NULL AND p.eliminado_en IS NULL AND f.codigo=N'CATEGORIAS_BAJA' AND f.eliminado_en IS NULL) THROW 51000,'El usuario no está activo o no tiene permiso para reactivar categorías.',1;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE dbo.CATEGORIA SET eliminado_en=NULL WHERE id_categoria=@idCategoria AND eliminado_en IS NOT NULL;
        IF @@ROWCOUNT=0 THROW 51001,'La categoría no existe o ya está activa.',1;
        DECLARE @nombre NVARCHAR(100); SELECT @nombre=nombre FROM dbo.CATEGORIA WHERE id_categoria=@idCategoria;
        DECLARE @detalle NVARCHAR(500)=CONCAT(N'Categoría reactivada: ',@nombre,N'.');
        EXEC dbo.sp_Auditoria_Registrar @idUsuario=@idUsuarioEjecutor,@accion=N'REACTIVACION',@entidad=N'CATEGORIA',@idEntidad=@idCategoria,@detalle=@detalle,@idSucursal=@idSucursalAuditoria;
        COMMIT TRANSACTION; SET @MensajeResultado=N'Categoría reactivada.';
    END TRY BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK TRANSACTION; THROW; END CATCH;
END;
GO


-- ============================================================
-- MARCAS
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Marca_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_marca,
        nombre
    FROM dbo.MARCA
    WHERE eliminado_en IS NULL
    ORDER BY nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Marca_ListarPorCategoria @idCategoria INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.id_marca,m.nombre FROM dbo.MARCA_CATEGORIA mc
    JOIN dbo.MARCA m ON m.id_marca=mc.id_marca
    JOIN dbo.CATEGORIA c ON c.id_categoria=mc.id_categoria
    WHERE mc.id_categoria=@idCategoria AND m.eliminado_en IS NULL AND c.eliminado_en IS NULL
    ORDER BY m.nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Marca_ListarGestion
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.id_marca,m.nombre,CONVERT(BIT,CASE WHEN m.eliminado_en IS NULL THEN 1 ELSE 0 END) activo,
           COUNT(mc.id_categoria) cantidad_categorias
    FROM dbo.MARCA m LEFT JOIN dbo.MARCA_CATEGORIA mc ON mc.id_marca=m.id_marca
    GROUP BY m.id_marca,m.nombre,m.eliminado_en ORDER BY m.nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Marca_CategoriasObtener @idMarca INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_categoria FROM dbo.MARCA_CATEGORIA WHERE id_marca=@idMarca ORDER BY id_categoria;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Marca_Guardar
    @idMarca INT,@nombre NVARCHAR(100),@categorias NVARCHAR(MAX),@idUsuarioEjecutor INT,
    @idSucursalAuditoria INT=NULL,@IdGenerado INT OUTPUT,@CodigoResultado INT OUTPUT,@MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON; SET @IdGenerado=0; SET @CodigoResultado=0; SET @MensajeResultado=N'Marca guardada.';
    SET @nombre=LTRIM(RTRIM(ISNULL(@nombre,N'')));
    WHILE CHARINDEX(N'  ',@nombre)>0 SET @nombre=REPLACE(@nombre,N'  ',N' ');
    DECLARE @permiso NVARCHAR(50)=CASE WHEN @idMarca=0 THEN N'MARCAS_ALTA' ELSE N'MARCAS_MODIFICAR' END;
    IF @nombre=N'' OR LEN(@nombre)>100 BEGIN SET @CodigoResultado=1; SET @MensajeResultado=N'El nombre de marca es obligatorio y admite hasta 100 caracteres.'; RETURN; END;
    IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO u JOIN dbo.PERFIL p ON p.id_perfil=u.id_perfil JOIN dbo.PERFIL_FUNCIONALIDAD pf ON pf.id_perfil=p.id_perfil JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad=pf.id_funcionalidad WHERE u.id_usuario=@idUsuarioEjecutor AND u.eliminado_en IS NULL AND p.eliminado_en IS NULL AND f.codigo=@permiso AND f.eliminado_en IS NULL) THROW 51000,'El usuario no está activo o no tiene permiso para gestionar marcas.',1;
    IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE UPPER(LTRIM(RTRIM(nombre)))=UPPER(@nombre) AND id_marca<>@idMarca)
    BEGIN SET @CodigoResultado=2; SET @MensajeResultado=N'Ya existe una marca con ese nombre.'; RETURN; END;
    DECLARE @Ids TABLE(id_categoria INT PRIMARY KEY);
    BEGIN TRY
        INSERT @Ids(id_categoria) SELECT TRY_CONVERT(INT,value) FROM STRING_SPLIT(ISNULL(@categorias,N''),N',');
    END TRY BEGIN CATCH SET @CodigoResultado=1; SET @MensajeResultado=N'La selección de categorías contiene valores duplicados o inválidos.'; RETURN; END CATCH;
    IF NOT EXISTS(SELECT 1 FROM @Ids) OR EXISTS(SELECT 1 FROM @Ids i LEFT JOIN dbo.CATEGORIA c ON c.id_categoria=i.id_categoria AND c.eliminado_en IS NULL WHERE c.id_categoria IS NULL)
    BEGIN SET @CodigoResultado=1; SET @MensajeResultado=N'Seleccioná al menos una categoría activa válida.'; RETURN; END;
    IF @idMarca>0 AND EXISTS
    (
        SELECT 1 FROM dbo.PRODUCTO p
        WHERE p.id_marca=@idMarca
          AND NOT EXISTS(SELECT 1 FROM @Ids i WHERE i.id_categoria=p.id_categoria)
    )
    BEGIN SET @CodigoResultado=1; SET @MensajeResultado=N'No se pueden quitar categorías utilizadas por productos existentes.'; RETURN; END;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF @idMarca=0 BEGIN INSERT dbo.MARCA(nombre) VALUES(@nombre); SET @IdGenerado=CONVERT(INT,SCOPE_IDENTITY()); END
        ELSE BEGIN UPDATE dbo.MARCA SET nombre=@nombre WHERE id_marca=@idMarca AND eliminado_en IS NULL; IF @@ROWCOUNT=0 THROW 51001,'La marca no existe o está inactiva.',1; SET @IdGenerado=@idMarca; END;
        DELETE FROM dbo.MARCA_CATEGORIA WHERE id_marca=@IdGenerado;
        INSERT dbo.MARCA_CATEGORIA(id_marca,id_categoria) SELECT @IdGenerado,id_categoria FROM @Ids;
        DECLARE @accion NVARCHAR(50)=CASE WHEN @idMarca=0 THEN N'ALTA' ELSE N'MODIFICACION' END;
        DECLARE @cantidadCategorias INT=(SELECT COUNT(*) FROM @Ids);
        DECLARE @detalle NVARCHAR(500)=CONCAT(N'Marca ',LOWER(@accion),N': ',@nombre,N' (categorías vinculadas: ',@cantidadCategorias,N').');
        EXEC dbo.sp_Auditoria_Registrar @idUsuario=@idUsuarioEjecutor,@accion=@accion,@entidad=N'MARCA',@idEntidad=@IdGenerado,@detalle=@detalle,@idSucursal=@idSucursalAuditoria;
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK TRANSACTION; THROW; END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Marca_Baja
    @idMarca INT,@idUsuarioEjecutor INT,@idSucursalAuditoria INT=NULL,@CodigoResultado INT OUTPUT,@MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON; SET @CodigoResultado=0;
    IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO u JOIN dbo.PERFIL p ON p.id_perfil=u.id_perfil JOIN dbo.PERFIL_FUNCIONALIDAD pf ON pf.id_perfil=p.id_perfil JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad=pf.id_funcionalidad WHERE u.id_usuario=@idUsuarioEjecutor AND u.eliminado_en IS NULL AND p.eliminado_en IS NULL AND f.codigo=N'MARCAS_BAJA' AND f.eliminado_en IS NULL) THROW 51000,'El usuario no está activo o no tiene permiso para dar de baja marcas.',1;
    BEGIN TRY BEGIN TRANSACTION;
        UPDATE dbo.MARCA SET eliminado_en=SYSDATETIME() WHERE id_marca=@idMarca AND eliminado_en IS NULL;
        IF @@ROWCOUNT=0 THROW 51001,'La marca no existe o ya está inactiva.',1;
        DECLARE @nombre NVARCHAR(100); SELECT @nombre=nombre FROM dbo.MARCA WHERE id_marca=@idMarca;
        DECLARE @detalle NVARCHAR(500)=CONCAT(N'Marca dada de baja: ',@nombre,N'.');
        EXEC dbo.sp_Auditoria_Registrar @idUsuario=@idUsuarioEjecutor,@accion=N'BAJA',@entidad=N'MARCA',@idEntidad=@idMarca,@detalle=@detalle,@idSucursal=@idSucursalAuditoria;
        COMMIT TRANSACTION; SET @MensajeResultado=N'Marca dada de baja.';
    END TRY BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK TRANSACTION; THROW; END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Marca_Reactivar
    @idMarca INT,@idUsuarioEjecutor INT,@idSucursalAuditoria INT=NULL,@CodigoResultado INT OUTPUT,@MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON; SET @CodigoResultado=0;
    IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO u JOIN dbo.PERFIL p ON p.id_perfil=u.id_perfil JOIN dbo.PERFIL_FUNCIONALIDAD pf ON pf.id_perfil=p.id_perfil JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad=pf.id_funcionalidad WHERE u.id_usuario=@idUsuarioEjecutor AND u.eliminado_en IS NULL AND p.eliminado_en IS NULL AND f.codigo=N'MARCAS_BAJA' AND f.eliminado_en IS NULL) THROW 51000,'El usuario no está activo o no tiene permiso para reactivar marcas.',1;
    IF NOT EXISTS(SELECT 1 FROM dbo.MARCA_CATEGORIA mc JOIN dbo.CATEGORIA c ON c.id_categoria=mc.id_categoria AND c.eliminado_en IS NULL WHERE mc.id_marca=@idMarca) BEGIN SET @CodigoResultado=1; SET @MensajeResultado=N'La marca necesita al menos una categoría activa para reactivarse.'; RETURN; END;
    BEGIN TRY BEGIN TRANSACTION;
        UPDATE dbo.MARCA SET eliminado_en=NULL WHERE id_marca=@idMarca AND eliminado_en IS NOT NULL;
        IF @@ROWCOUNT=0 THROW 51001,'La marca no existe o ya está activa.',1;
        DECLARE @nombre NVARCHAR(100); SELECT @nombre=nombre FROM dbo.MARCA WHERE id_marca=@idMarca;
        DECLARE @detalle NVARCHAR(500)=CONCAT(N'Marca reactivada: ',@nombre,N'.');
        EXEC dbo.sp_Auditoria_Registrar @idUsuario=@idUsuarioEjecutor,@accion=N'REACTIVACION',@entidad=N'MARCA',@idEntidad=@idMarca,@detalle=@detalle,@idSucursal=@idSucursalAuditoria;
        COMMIT TRANSACTION; SET @MensajeResultado=N'Marca reactivada.';
    END TRY BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK TRANSACTION; THROW; END CATCH;
END;
GO


-- ============================================================
-- PRODUCTOS
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Producto_Buscar
    @texto NVARCHAR(100),
    @idSucursal INT
AS
BEGIN
    SET NOCOUNT ON;

    SET @texto = LTRIM(RTRIM(ISNULL(@texto, N'')));

    SELECT TOP 50
        p.id_producto,
        p.codigo_barra,
        p.nombre,
        p.descripcion,
        p.precio_venta,
        p.activo,
        c.nombre AS categoria,
        m.nombre AS marca,
        ISNULL(i.stock, 0) AS stock,
        ISNULL(i.stock_minimo, 0) AS stock_minimo
    FROM dbo.PRODUCTO AS p
    INNER JOIN dbo.CATEGORIA AS c
        ON c.id_categoria = p.id_categoria
       AND c.eliminado_en IS NULL
    LEFT JOIN dbo.MARCA AS m
        ON m.id_marca = p.id_marca
       AND m.eliminado_en IS NULL
    INNER JOIN dbo.INVENTARIO AS i
        ON i.id_producto = p.id_producto
       AND i.id_sucursal = @idSucursal
       AND i.eliminado_en IS NULL
       AND i.stock > 0
    INNER JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = i.id_sucursal
       AND s.eliminado_en IS NULL
    WHERE p.eliminado_en IS NULL
      AND p.activo = 1
      AND
      (
          @texto = N''
          OR ISNULL(p.codigo_barra, N'') LIKE N'%' + @texto + N'%'
          OR p.nombre LIKE N'%' + @texto + N'%'
          OR c.nombre LIKE N'%' + @texto + N'%'
          OR ISNULL(m.nombre, N'') LIKE N'%' + @texto + N'%'
      )
    ORDER BY p.nombre;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Producto_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_producto,
        p.id_categoria,
        p.id_marca,
        p.codigo_barra,
        p.nombre,
        p.descripcion,
        p.precio_costo,
        p.porcentaje_ganancia,
        p.precio_venta,
        p.activo,
        c.nombre AS categoria,
        m.nombre AS marca
    FROM dbo.PRODUCTO AS p
    INNER JOIN dbo.CATEGORIA AS c
        ON c.id_categoria = p.id_categoria
    LEFT JOIN dbo.MARCA AS m
        ON m.id_marca = p.id_marca
    WHERE p.eliminado_en IS NULL
    ORDER BY p.nombre;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Producto_ObtenerPorId
    @idProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        p.id_producto,
        p.id_categoria,
        p.id_marca,
        p.codigo_barra,
        p.nombre,
        p.descripcion,
        p.precio_costo,
        p.porcentaje_ganancia,
        p.precio_venta,
        p.activo,
        c.nombre AS categoria,
        m.nombre AS marca
    FROM dbo.PRODUCTO AS p
    INNER JOIN dbo.CATEGORIA AS c
        ON c.id_categoria = p.id_categoria
    LEFT JOIN dbo.MARCA AS m
        ON m.id_marca = p.id_marca
    WHERE p.id_producto = @idProducto
      AND p.eliminado_en IS NULL;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Producto_Alta
    @idCategoria INT,
    @idMarca INT = NULL,
    @codigoBarra NVARCHAR(50) = NULL,
    @nombre NVARCHAR(100),
    @descripcion NVARCHAR(MAX) = NULL,
    @precioCosto DECIMAL(18,2),
    @porcentajeGanancia DECIMAL(5,2),
    @activo BIT = 1,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,

    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;

        SET @codigoBarra = NULLIF(LTRIM(RTRIM(@codigoBarra)), N'');
        SET @nombre = LTRIM(RTRIM(ISNULL(@nombre, N'')));

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.CATEGORIA
            WHERE id_categoria = @idCategoria
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La categoría indicada no existe o está inactiva.';
            RETURN;
        END;

        IF @idMarca IS NOT NULL
           AND NOT EXISTS
           (
               SELECT 1 FROM dbo.MARCA_CATEGORIA mc
               INNER JOIN dbo.MARCA m ON m.id_marca=mc.id_marca AND m.eliminado_en IS NULL
               INNER JOIN dbo.CATEGORIA c ON c.id_categoria=mc.id_categoria AND c.eliminado_en IS NULL
               WHERE mc.id_marca=@idMarca AND mc.id_categoria=@idCategoria
           )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La marca indicada no existe o está inactiva.';
            RETURN;
        END;

        IF @nombre = N''
           OR @precioCosto < 0
           OR @porcentajeGanancia < 0
           OR @porcentajeGanancia > 999.99
           OR @precioCosto * (1 + @porcentajeGanancia / 100.0) > 9999999999999999.99
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Los datos del producto no son válidos.';
            RETURN;
        END;

        IF @codigoBarra IS NOT NULL
           AND EXISTS
           (
               SELECT 1
               FROM dbo.PRODUCTO
               WHERE codigo_barra = @codigoBarra
           )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado = N'Ya existe un producto con ese código de barras.';
            RETURN;
        END;

        BEGIN TRANSACTION;
        INSERT INTO dbo.PRODUCTO
        (
            id_categoria,
            id_marca,
            codigo_barra,
            nombre,
            descripcion,
            precio_costo,
            porcentaje_ganancia,
            activo
        )
        VALUES
        (
            @idCategoria,
            @idMarca,
            @codigoBarra,
            @nombre,
            NULLIF(LTRIM(RTRIM(@descripcion)), N''),
            @precioCosto,
            @porcentajeGanancia,
            @activo
        );

        SET @IdGenerado = CAST(SCOPE_IDENTITY() AS INT);
        DECLARE @detalleAuditoriaProducto NVARCHAR(300) = CONCAT(N'Producto registrado: ', @nombre, N'.');
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'ALTA', @entidad = N'PRODUCTO',
            @idEntidad = @IdGenerado, @detalle = @detalleAuditoriaProducto,
            @idSucursal = @idSucursalAuditoria;
        COMMIT TRANSACTION;
        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Producto registrado correctamente.';

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Producto_Modificar
    @idProducto INT,
    @idCategoria INT,
    @idMarca INT = NULL,
    @codigoBarra NVARCHAR(50) = NULL,
    @nombre NVARCHAR(100),
    @descripcion NVARCHAR(MAX) = NULL,
    @precioCosto DECIMAL(18,2),
    @porcentajeGanancia DECIMAL(5,2),
    @activo BIT,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,

    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @activoAnterior BIT;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;

        SET @codigoBarra = NULLIF(LTRIM(RTRIM(@codigoBarra)), N'');
        SET @nombre = LTRIM(RTRIM(ISNULL(@nombre, N'')));

        SELECT @activoAnterior = activo
        FROM dbo.PRODUCTO
        WHERE id_producto = @idProducto AND eliminado_en IS NULL;

        IF @activoAnterior IS NULL
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El producto no existe o fue dado de baja.';
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.CATEGORIA
            WHERE id_categoria = @idCategoria
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La categoría indicada no existe o está inactiva.';
            RETURN;
        END;

        IF @idMarca IS NOT NULL
           AND NOT EXISTS
           (
               SELECT 1 FROM dbo.MARCA_CATEGORIA mc
               INNER JOIN dbo.MARCA m ON m.id_marca=mc.id_marca AND m.eliminado_en IS NULL
               INNER JOIN dbo.CATEGORIA c ON c.id_categoria=mc.id_categoria AND c.eliminado_en IS NULL
               WHERE mc.id_marca=@idMarca AND mc.id_categoria=@idCategoria
           )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La marca indicada no existe o está inactiva.';
            RETURN;
        END;

        IF @nombre = N''
           OR @precioCosto < 0
           OR @porcentajeGanancia < 0
           OR @porcentajeGanancia > 999.99
           OR @precioCosto * (1 + @porcentajeGanancia / 100.0) > 9999999999999999.99
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Los datos del producto no son válidos.';
            RETURN;
        END;

        IF @codigoBarra IS NOT NULL
           AND EXISTS
           (
               SELECT 1
               FROM dbo.PRODUCTO
               WHERE codigo_barra = @codigoBarra
                 AND id_producto <> @idProducto
           )
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado = N'Ya existe otro producto con ese código de barras.';
            RETURN;
        END;

        BEGIN TRANSACTION;
        UPDATE dbo.PRODUCTO
        SET
            id_categoria = @idCategoria,
            id_marca = @idMarca,
            codigo_barra = @codigoBarra,
            nombre = @nombre,
            descripcion = NULLIF(LTRIM(RTRIM(@descripcion)), N''),
            precio_costo = @precioCosto,
            porcentaje_ganancia = @porcentajeGanancia,
            activo = @activo
        WHERE id_producto = @idProducto
          AND eliminado_en IS NULL;

        DECLARE @accionAuditoriaProducto NVARCHAR(50) =
            CASE WHEN @activoAnterior = 0 AND @activo = 1 THEN N'REACTIVACION'
                 WHEN @activoAnterior = 1 AND @activo = 0 THEN N'BAJA'
                 ELSE N'MODIFICACION' END;
        DECLARE @detalleAuditoriaProducto NVARCHAR(300) = CONCAT(N'Producto actualizado: ', @nombre, N'.');
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = @accionAuditoriaProducto, @entidad = N'PRODUCTO',
            @idEntidad = @idProducto, @detalle = @detalleAuditoriaProducto,
            @idSucursal = @idSucursalAuditoria;
        COMMIT TRANSACTION;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Producto modificado correctamente.';

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Producto_Baja
    @idProducto INT,
    @idUsuarioEjecutor INT,
    @idSucursalAuditoria INT = NULL,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.PRODUCTO
            WHERE id_producto = @idProducto
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El producto no existe o ya fue dado de baja.';
            RETURN;
        END;

        BEGIN TRANSACTION;
        UPDATE dbo.PRODUCTO
        SET eliminado_en = SYSDATETIME()
        WHERE id_producto = @idProducto
          AND eliminado_en IS NULL;

        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'BAJA', @entidad = N'PRODUCTO',
            @idEntidad = @idProducto, @detalle = N'Producto dado de baja.',
            @idSucursal = @idSucursalAuditoria;
        COMMIT TRANSACTION;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Producto dado de baja correctamente.';

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


-- ============================================================
-- INVENTARIO
-- ============================================================

-- Retira el ajuste histórico, reemplazado por el establecimiento autorizado de stock.
DROP PROCEDURE IF EXISTS dbo.sp_Inventario_AjustarStock;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Inventario_ObtenerProductoSucursal
    @idProducto INT,
    @idSucursal INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        i.id_inventario,
        i.id_producto,
        i.id_sucursal,
        i.stock,
        i.stock_minimo
    FROM dbo.INVENTARIO AS i
    WHERE i.id_producto = @idProducto
      AND i.id_sucursal = @idSucursal
      AND i.eliminado_en IS NULL;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Inventario_ListarPorSucursal
    @idSucursal INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_producto,
        p.codigo_barra,
        p.nombre,
        c.nombre AS categoria,
        m.nombre AS marca,
        p.precio_venta,
        ISNULL(i.stock, 0) AS stock,
        ISNULL(i.stock_minimo, 0) AS stock_minimo,
        p.activo
    FROM dbo.PRODUCTO AS p
    INNER JOIN dbo.CATEGORIA AS c
        ON c.id_categoria = p.id_categoria
    LEFT JOIN dbo.MARCA AS m
        ON m.id_marca = p.id_marca
    LEFT JOIN dbo.INVENTARIO AS i
        ON i.id_producto = p.id_producto
       AND i.id_sucursal = @idSucursal
       AND i.eliminado_en IS NULL
    WHERE p.eliminado_en IS NULL
    ORDER BY p.nombre;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Inventario_EstablecerStock
    @idUsuario INT,
    @idProducto INT,
    @idSucursal INT,
    @stock INT,
    @stockMinimo INT = 0,

    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        /* ----------------------------------------------------
           Usuario y perfil activos
           ---------------------------------------------------- */

        IF @idUsuario IS NULL OR @idUsuario <= 0
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El usuario es obligatorio para actualizar el stock.';
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO AS u
            INNER JOIN dbo.PERFIL AS p
                ON p.id_perfil = u.id_perfil
               AND p.eliminado_en IS NULL
            WHERE u.id_usuario = @idUsuario
              AND u.eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El usuario o su perfil no existen o están inactivos.';
            RETURN;
        END;

        /* ----------------------------------------------------
           Permiso de modificación de productos
           ---------------------------------------------------- */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO AS u
            INNER JOIN dbo.PERFIL_FUNCIONALIDAD AS pf
                ON pf.id_perfil = u.id_perfil
            INNER JOIN dbo.FUNCIONALIDAD AS f
                ON f.id_funcionalidad = pf.id_funcionalidad
            WHERE u.id_usuario = @idUsuario
              AND u.eliminado_en IS NULL
              AND f.codigo = N'PRODUCTOS_MODIFICAR'
              AND f.eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 5;
            SET @MensajeResultado = N'El usuario no tiene permiso para modificar stock.';
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.PRODUCTO
            WHERE id_producto = @idProducto
              AND eliminado_en IS NULL
              AND activo = 1
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El producto indicado no existe, está inactivo o fue dado de baja.';
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.SUCURSAL
            WHERE id_sucursal = @idSucursal
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La sucursal indicada no existe o está inactiva.';
            RETURN;
        END;

        /* ----------------------------------------------------
           Alcance del perfil

           Un perfil no global solo puede operar sobre la
           sucursal asignada al usuario.
           ---------------------------------------------------- */

        IF EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO AS u
            INNER JOIN dbo.PERFIL AS p
                ON p.id_perfil = u.id_perfil
               AND p.eliminado_en IS NULL
            WHERE u.id_usuario = @idUsuario
              AND u.eliminado_en IS NULL
              AND p.alcance_global = 0
              AND
              (
                  u.id_sucursal IS NULL
                  OR u.id_sucursal <> @idSucursal
              )
        )
        BEGIN
            SET @CodigoResultado = 5;
            SET @MensajeResultado = N'El usuario no puede modificar stock en la sucursal indicada.';
            RETURN;
        END;

        IF @stock < 0 OR @stockMinimo < 0
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El stock y el stock mínimo no pueden ser negativos.';
            RETURN;
        END;

        DECLARE @stockAnterior INT;
        SELECT @stockAnterior = stock
        FROM dbo.INVENTARIO
        WHERE id_producto = @idProducto
          AND id_sucursal = @idSucursal;

        BEGIN TRANSACTION;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.INVENTARIO
            WHERE id_producto = @idProducto
              AND id_sucursal = @idSucursal
        )
        BEGIN
            UPDATE dbo.INVENTARIO
            SET
                stock = @stock,
                stock_minimo = @stockMinimo,
                eliminado_en = NULL
            WHERE id_producto = @idProducto
              AND id_sucursal = @idSucursal;
        END
        ELSE
        BEGIN
            INSERT INTO dbo.INVENTARIO
            (
                id_producto,
                id_sucursal,
                stock,
                stock_minimo
            )
            VALUES
            (
                @idProducto,
                @idSucursal,
                @stock,
                @stockMinimo
            );
        END;

        DECLARE @detalleAuditoria NVARCHAR(300);
        SET @detalleAuditoria = CONCAT(N'Stock actualizado de ', COALESCE(CONVERT(NVARCHAR(20), @stockAnterior), N'0'), N' a ', @stock, N'.');

        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuario,
            @accion = N'MODIFICACION',
            @entidad = N'INVENTARIO',
            @idEntidad = @idProducto,
            @detalle = @detalleAuditoria,
            @idSucursal = @idSucursal;

        COMMIT TRANSACTION;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Stock actualizado correctamente para la sucursal indicada.';

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Inventario_StockBajo
    @idSucursal INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_producto,
        p.codigo_barra,
        p.nombre,
        c.nombre AS categoria,
        m.nombre AS marca,
        ISNULL(i.stock, 0) AS stock,
        ISNULL(i.stock_minimo, 0) AS stock_minimo
    FROM dbo.PRODUCTO AS p
    INNER JOIN dbo.CATEGORIA AS c
        ON c.id_categoria = p.id_categoria
    LEFT JOIN dbo.MARCA AS m
        ON m.id_marca = p.id_marca
    LEFT JOIN dbo.INVENTARIO AS i
        ON i.id_producto = p.id_producto
       AND i.id_sucursal = @idSucursal
       AND i.eliminado_en IS NULL
    WHERE p.eliminado_en IS NULL
      AND p.activo = 1
      AND ISNULL(i.stock, 0) <= ISNULL(i.stock_minimo, 0)
    ORDER BY ISNULL(i.stock, 0), p.nombre;
END;
GO


-- ============================================================
-- VENTAS - CONSULTAS
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Venta_Listar
    @desde DATE,
    @hasta DATE,
    @idSucursal INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.id_venta,
        v.fecha_hora,
        v.tipo_factura,
        v.subtotal,
        v.descuento,
        v.total,
        c.id_cliente,
        c.nombre + N' ' + c.apellido AS cliente,
        u.id_usuario,
        u.nombre + N' ' + u.apellido AS vendedor,
        s.id_sucursal,
        s.nombre AS sucursal
    FROM dbo.VENTA AS v
    INNER JOIN dbo.CLIENTE AS c
        ON c.id_cliente = v.id_cliente
    INNER JOIN dbo.USUARIO AS u
        ON u.id_usuario = v.id_usuario
    INNER JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = v.id_sucursal
    WHERE v.eliminado_en IS NULL
      AND v.fecha_hora >= @desde
      AND v.fecha_hora < DATEADD(DAY, 1, @hasta)
      AND (@idSucursal IS NULL OR v.id_sucursal = @idSucursal)
    ORDER BY v.fecha_hora DESC;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Venta_ListarPorVendedor
    @idUsuario INT,
    @desde DATE,
    @hasta DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.id_venta,
        v.fecha_hora,
        v.tipo_factura,
        v.subtotal,
        v.descuento,
        v.total,

        c.id_cliente,
        c.nombre + N' ' + c.apellido AS cliente,

        u.id_usuario,
        u.nombre + N' ' + u.apellido AS vendedor,

        s.id_sucursal,
        s.nombre AS sucursal

    FROM dbo.VENTA AS v

    INNER JOIN dbo.CLIENTE AS c
        ON c.id_cliente = v.id_cliente

    INNER JOIN dbo.USUARIO AS u
        ON u.id_usuario = v.id_usuario

    INNER JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = v.id_sucursal

    WHERE
        v.eliminado_en IS NULL
        AND v.id_usuario = @idUsuario
        AND v.fecha_hora >= @desde
        AND v.fecha_hora < DATEADD(DAY, 1, @hasta)

    ORDER BY
        v.fecha_hora DESC;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Venta_ListarPorCliente
    @idCliente INT,
    @desde DATE,
    @hasta DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.id_venta,
        v.fecha_hora,
        v.tipo_factura,
        v.subtotal,
        v.descuento,
        v.total,

        c.id_cliente,
        c.nombre + N' ' + c.apellido AS cliente,

        u.id_usuario,
        u.nombre + N' ' + u.apellido AS vendedor,

        s.id_sucursal,
        s.nombre AS sucursal

    FROM dbo.VENTA AS v

    INNER JOIN dbo.CLIENTE AS c
        ON c.id_cliente = v.id_cliente

    INNER JOIN dbo.USUARIO AS u
        ON u.id_usuario = v.id_usuario

    INNER JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = v.id_sucursal

    WHERE
        v.eliminado_en IS NULL
        AND v.id_cliente = @idCliente
        AND v.fecha_hora >= @desde
        AND v.fecha_hora < DATEADD(DAY, 1, @hasta)

    ORDER BY
        v.fecha_hora DESC;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Venta_ObtenerDetalle
    @idVenta INT
AS
BEGIN
    SET NOCOUNT ON;


    -- ========================================================
    -- RESULTADO 1: DATOS GENERALES DE LA VENTA
    -- ========================================================

    SELECT TOP 1
        v.id_venta,
        v.fecha_hora,
        v.tipo_factura,
        v.subtotal,
        v.descuento,
        v.total,

        c.id_cliente,
        c.nombre + N' ' + c.apellido AS cliente,
        c.documento AS documento_cliente,

        u.id_usuario,
        u.nombre + N' ' + u.apellido AS vendedor,
        u.nombre_usuario,

        s.id_sucursal,
        s.nombre AS sucursal

    FROM dbo.VENTA AS v

    INNER JOIN dbo.CLIENTE AS c
        ON c.id_cliente = v.id_cliente

    INNER JOIN dbo.USUARIO AS u
        ON u.id_usuario = v.id_usuario

    INNER JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = v.id_sucursal

    WHERE
        v.id_venta = @idVenta
        AND v.eliminado_en IS NULL;


    -- ========================================================
    -- RESULTADO 2: PRODUCTOS DE LA VENTA
    -- ========================================================

    SELECT
        dv.id_detalle_venta,
        dv.id_producto,
        p.codigo_barra,
        p.nombre AS producto,
        dv.cantidad,
        dv.precio_unitario,
        dv.subtotal

    FROM dbo.DETALLE_VENTA AS dv

    INNER JOIN dbo.PRODUCTO AS p
        ON p.id_producto = dv.id_producto

    WHERE
        dv.id_venta = @idVenta
        AND dv.eliminado_en IS NULL

    ORDER BY
        dv.id_detalle_venta;


    -- ========================================================
    -- RESULTADO 3: PAGOS DE LA VENTA
    -- ========================================================

    SELECT
        pg.id_pago,
        pg.id_metodo_pago,
        mp.nombre AS metodo_pago,
        pg.monto

    FROM dbo.PAGO AS pg

    INNER JOIN dbo.METODO_PAGO AS mp
        ON mp.id_metodo_pago = pg.id_metodo_pago

    WHERE
        pg.id_venta = @idVenta
        AND pg.eliminado_en IS NULL

    ORDER BY
        pg.id_pago;
END;
GO



-- ============================================================
-- VENTAS - REGISTRO
-- ============================================================

/* ============================================================
   Procedimiento: sp_Venta_Registrar

   Registra una venta completa en una única transacción:

   1. Valida usuario y permiso VENTAS_REALIZAR.
   2. Valida sucursal y alcance del perfil.
   3. Valida cliente.
   4. Valida productos activos.
   5. Valida y bloquea stock por sucursal.
   6. Obtiene el precio vigente desde PRODUCTO.precio_venta.
   7. Valida pagos.
   8. Inserta VENTA.
   9. Inserta DETALLE_VENTA.
   10. Inserta PAGO.
   11. Descuenta INVENTARIO.

   La fecha se genera en SQL Server mediante SYSDATETIME().
   El precio NO se recibe desde la Vista.
   El descuento queda en 0 hasta definir la política correspondiente.
   tipo_factura queda en NULL hasta definir la facturación real.

   Códigos de resultado:
   0   = Operación correcta
   1   = Registro relacionado inexistente/inactivo
   3   = Datos inválidos
   4   = Stock insuficiente
   5   = Operación no permitida
   500 = Error interno / inesperado
   ============================================================ */

CREATE OR ALTER PROCEDURE dbo.sp_Venta_Registrar
    @idCliente INT,
    @idUsuario INT,
    @idSucursal INT,

    @items dbo.VentaItemTipo READONLY,
    @pagos dbo.VentaPagoTipo READONLY,

    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';


    /* --------------------------------------------------------
       Validaciones básicas
       -------------------------------------------------------- */

    IF @idCliente IS NULL OR @idCliente <= 0
       OR @idUsuario IS NULL OR @idUsuario <= 0
       OR @idSucursal IS NULL OR @idSucursal <= 0
    BEGIN
        SET @CodigoResultado = 3;
        SET @MensajeResultado =
            N'Cliente, usuario y sucursal son obligatorios.';
        RETURN;
    END;


    IF NOT EXISTS
    (
        SELECT 1
        FROM @items
    )
    BEGIN
        SET @CodigoResultado = 3;
        SET @MensajeResultado =
            N'La venta debe contener al menos un producto.';
        RETURN;
    END;


    IF NOT EXISTS
    (
        SELECT 1
        FROM @pagos
    )
    BEGIN
        SET @CodigoResultado = 3;
        SET @MensajeResultado =
            N'La venta debe contener al menos un pago.';
        RETURN;
    END;


    BEGIN TRY

        BEGIN TRANSACTION;


        /* ----------------------------------------------------
           Usuario activo
           ---------------------------------------------------- */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO AS u
            INNER JOIN dbo.PERFIL AS p
                ON p.id_perfil = u.id_perfil
               AND p.eliminado_en IS NULL
            WHERE u.id_usuario = @idUsuario
              AND u.eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado =
                N'El usuario no existe o fue dado de baja.';

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        /* ----------------------------------------------------
           Permiso VENTAS_REALIZAR
           ---------------------------------------------------- */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO AS u
            INNER JOIN dbo.PERFIL_FUNCIONALIDAD AS pf
                ON pf.id_perfil = u.id_perfil
            INNER JOIN dbo.FUNCIONALIDAD AS f
                ON f.id_funcionalidad = pf.id_funcionalidad
            WHERE u.id_usuario = @idUsuario
              AND u.eliminado_en IS NULL
              AND f.codigo = N'VENTAS_REALIZAR'
              AND f.eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 5;
            SET @MensajeResultado =
                N'El usuario no tiene permiso para realizar ventas.';

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        /* ----------------------------------------------------
           Sucursal activa
           ---------------------------------------------------- */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.SUCURSAL
            WHERE id_sucursal = @idSucursal
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado =
                N'La sucursal indicada no existe o está inactiva.';

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        /* ----------------------------------------------------
           Alcance del perfil

           alcance_global = 0:
           el usuario solo puede vender en su sucursal asignada.

           alcance_global = 1:
           puede operar en la sucursal seleccionada.
           ---------------------------------------------------- */

        IF EXISTS
        (
            SELECT 1
            FROM dbo.USUARIO AS u
            INNER JOIN dbo.PERFIL AS p
                ON p.id_perfil = u.id_perfil
            WHERE u.id_usuario = @idUsuario
              AND ISNULL(p.alcance_global, 0) = 0
              AND
              (
                  u.id_sucursal IS NULL
                  OR u.id_sucursal <> @idSucursal
              )
        )
        BEGIN
            SET @CodigoResultado = 5;
            SET @MensajeResultado =
                N'El usuario no puede registrar ventas en la sucursal seleccionada.';

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        /* ----------------------------------------------------
           Cliente activo
           ---------------------------------------------------- */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.CLIENTE
            WHERE id_cliente = @idCliente
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado =
                N'El cliente seleccionado no existe o está inactivo.';

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        /* ----------------------------------------------------
           Productos válidos
           ---------------------------------------------------- */

        DECLARE @ProductoInvalido NVARCHAR(100);


        SELECT TOP 1
            @ProductoInvalido =
                COALESCE(
                    p.nombre,
                    CONCAT(
                        N'ID ',
                        CAST(i.id_producto AS NVARCHAR(20))
                    )
                )
        FROM @items AS i
        LEFT JOIN dbo.PRODUCTO AS p
            ON p.id_producto = i.id_producto
           AND p.eliminado_en IS NULL
           AND p.activo = 1
        WHERE p.id_producto IS NULL;


        IF @ProductoInvalido IS NOT NULL
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado =
                LEFT(
                    N'El producto ' +
                    @ProductoInvalido +
                    N' no existe, está inactivo o fue dado de baja.',
                    250
                );

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        /* ----------------------------------------------------
           Validar y bloquear stock

           UPDLOCK + HOLDLOCK evita que dos ventas simultáneas
           consuman las mismas unidades de inventario.
           ---------------------------------------------------- */

        DECLARE
            @ProductoStock NVARCHAR(100),
            @StockDisponible INT,
            @CantidadSolicitada INT;


        SELECT TOP 1
            @ProductoStock = p.nombre,
            @StockDisponible = ISNULL(inv.stock, 0),
            @CantidadSolicitada = i.cantidad
        FROM @items AS i
        INNER JOIN dbo.PRODUCTO AS p
            ON p.id_producto = i.id_producto
        LEFT JOIN dbo.INVENTARIO AS inv WITH (UPDLOCK, HOLDLOCK)
            ON inv.id_producto = i.id_producto
           AND inv.id_sucursal = @idSucursal
           AND inv.eliminado_en IS NULL
        WHERE inv.id_inventario IS NULL
           OR inv.stock < i.cantidad;


        IF @ProductoStock IS NOT NULL
        BEGIN
            SET @CodigoResultado = 4;
            SET @MensajeResultado =
                LEFT(
                    N'Stock insuficiente para ' +
                    @ProductoStock +
                    N'. Disponible: ' +
                    CAST(@StockDisponible AS NVARCHAR(20)) +
                    N'. Solicitado: ' +
                    CAST(@CantidadSolicitada AS NVARCHAR(20)) +
                    N'.',
                    250
                );

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        /* ----------------------------------------------------
           Calcular detalle con el precio vigente real
           ---------------------------------------------------- */

        DECLARE @DetalleCalculado TABLE
        (
            id_producto INT NOT NULL PRIMARY KEY,
            cantidad INT NOT NULL,
            precio_unitario DECIMAL(18,2) NOT NULL,
            subtotal DECIMAL(18,2) NOT NULL
        );


        INSERT INTO @DetalleCalculado
        (
            id_producto,
            cantidad,
            precio_unitario,
            subtotal
        )
        SELECT
            i.id_producto,
            i.cantidad,
            CAST(p.precio_venta AS DECIMAL(18,2)),
            CAST(
                p.precio_venta * i.cantidad
                AS DECIMAL(18,2)
            )
        FROM @items AS i
        INNER JOIN dbo.PRODUCTO AS p
            ON p.id_producto = i.id_producto
        WHERE p.eliminado_en IS NULL
          AND p.activo = 1;


        DECLARE
            @Subtotal DECIMAL(18,2),
            @Descuento DECIMAL(18,2),
            @Total DECIMAL(18,2),
            @TotalPagos DECIMAL(18,2);


        SELECT
            @Subtotal =
                CAST(
                    ISNULL(SUM(subtotal), 0)
                    AS DECIMAL(18,2)
                )
        FROM @DetalleCalculado;


        SET @Descuento = 0;
        SET @Total = @Subtotal;


        IF @Total <= 0
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado =
                N'El total de la venta debe ser mayor que cero.';

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        /* ----------------------------------------------------
           Validar métodos de pago
           ---------------------------------------------------- */

        IF EXISTS
        (
            SELECT 1
            FROM @pagos AS pg
            LEFT JOIN dbo.METODO_PAGO AS mp
                ON mp.id_metodo_pago = pg.id_metodo_pago
               AND mp.eliminado_en IS NULL
            WHERE mp.id_metodo_pago IS NULL
        )
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado =
                N'Uno de los métodos de pago no existe o está inactivo.';

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        SELECT
            @TotalPagos =
                CAST(
                    ISNULL(SUM(monto), 0)
                    AS DECIMAL(18,2)
                )
        FROM @pagos;


        IF @TotalPagos <> @Total
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado =
                LEFT(
                    N'El total de los pagos debe coincidir con el total de la venta. ' +
                    N'Total venta: $' +
                    CAST(@Total AS NVARCHAR(30)) +
                    N'. Total pagos: $' +
                    CAST(@TotalPagos AS NVARCHAR(30)) +
                    N'.',
                    250
                );

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        /* ----------------------------------------------------
           Registrar cabecera

           Fecha: automática.
           Descuento: 0.
           Tipo factura: NULL hasta implementar facturación.
           ---------------------------------------------------- */

        INSERT INTO dbo.VENTA
        (
            id_cliente,
            id_usuario,
            id_sucursal,
            fecha_hora,
            tipo_factura,
            subtotal,
            descuento,
            total
        )
        VALUES
        (
            @idCliente,
            @idUsuario,
            @idSucursal,
            SYSDATETIME(),
            NULL,
            @Subtotal,
            @Descuento,
            @Total
        );


        SET @IdGenerado =
            CAST(SCOPE_IDENTITY() AS INT);


        /* ----------------------------------------------------
           Registrar detalle
           ---------------------------------------------------- */

        INSERT INTO dbo.DETALLE_VENTA
        (
            id_venta,
            id_producto,
            cantidad,
            precio_unitario,
            subtotal
        )
        SELECT
            @IdGenerado,
            id_producto,
            cantidad,
            precio_unitario,
            subtotal
        FROM @DetalleCalculado;


        /* ----------------------------------------------------
           Registrar pagos
           ---------------------------------------------------- */

        INSERT INTO dbo.PAGO
        (
            id_venta,
            id_metodo_pago,
            monto
        )
        SELECT
            @IdGenerado,
            id_metodo_pago,
            monto
        FROM @pagos;


        /* ----------------------------------------------------
           Descontar inventario
           ---------------------------------------------------- */

        UPDATE inv
        SET
            inv.stock =
                inv.stock - i.cantidad
        FROM dbo.INVENTARIO AS inv
        INNER JOIN @items AS i
            ON i.id_producto = inv.id_producto
        WHERE inv.id_sucursal = @idSucursal
          AND inv.eliminado_en IS NULL;


        IF @@ROWCOUNT <>
        (
            SELECT COUNT(*)
            FROM @items
        )
        BEGIN
            SET @IdGenerado = 0;
            SET @CodigoResultado = 500;
            SET @MensajeResultado =
                N'No se pudo actualizar el inventario completo de la venta.';

            ROLLBACK TRANSACTION;
            RETURN;
        END;


        DECLARE @detalleAuditoria NVARCHAR(300);
        SET @detalleAuditoria = CONCAT(N'Venta registrada por $', CONVERT(NVARCHAR(30), @Total), N'.');

        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuario,
            @accion = N'ALTA',
            @entidad = N'VENTA',
            @idEntidad = @IdGenerado,
            @detalle = @detalleAuditoria,
            @idSucursal = @idSucursal;

        COMMIT TRANSACTION;


        SET @CodigoResultado = 0;
        SET @MensajeResultado =
            N'Venta registrada correctamente.';


    END TRY

    BEGIN CATCH

        IF XACT_STATE() <> 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;


        SET @IdGenerado = 0;
        SET @CodigoResultado = 500;
        SET @MensajeResultado =
            LEFT(ERROR_MESSAGE(), 250);

    END CATCH;
END;
GO


-- ============================================================
-- AUDITORÍA
-- ============================================================

-- Inserta un evento breve sin almacenar contraseñas ni valores sensibles.
CREATE OR ALTER PROCEDURE dbo.sp_Auditoria_Registrar
    @idUsuario INT,
    @accion NVARCHAR(50),
    @entidad NVARCHAR(50),
    @idEntidad INT = NULL,
    @detalle NVARCHAR(300) = NULL,
    @idSucursal INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @idUsuario IS NULL OR NOT EXISTS
    (
        SELECT 1
        FROM dbo.USUARIO AS u
        INNER JOIN dbo.PERFIL AS p ON p.id_perfil = u.id_perfil
        WHERE u.id_usuario = @idUsuario
          AND u.eliminado_en IS NULL
          AND p.eliminado_en IS NULL
    )
        THROW 51000, 'El usuario auditor no está activo.', 1;

    IF LEN(LTRIM(RTRIM(ISNULL(@accion, N'')))) = 0
       OR LEN(LTRIM(RTRIM(ISNULL(@entidad, N'')))) = 0
        THROW 51000, 'La acción y la entidad de auditoría son obligatorias.', 1;

    IF @idSucursal IS NOT NULL AND NOT EXISTS
    (
        SELECT 1 FROM dbo.SUCURSAL
        WHERE id_sucursal = @idSucursal
    )
        THROW 51000, 'La sucursal de auditoría no existe.', 1;

    INSERT INTO dbo.AUDITORIA
    (
        id_usuario, accion, entidad, id_entidad, detalle, id_sucursal
    )
    VALUES
    (
        @idUsuario,
        LEFT(LTRIM(RTRIM(@accion)), 50),
        LEFT(LTRIM(RTRIM(@entidad)), 50),
        @idEntidad,
        NULLIF(LEFT(LTRIM(RTRIM(ISNULL(@detalle, N''))), 300), N''),
        @idSucursal
    );
END;
GO


-- Lista actividad administrativa aplicando el permiso y el alcance efectivo del solicitante.
CREATE OR ALTER PROCEDURE dbo.sp_Auditoria_Listar
    @idUsuarioSolicitante INT,
    @desde DATE,
    @hasta DATE,
    @idSucursal INT = NULL,
    @idUsuario INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @alcanceGlobal BIT, @sucursalAsignada INT, @perfil INT;

    SELECT
        @perfil = u.id_perfil,
        @alcanceGlobal = p.alcance_global,
        @sucursalAsignada = u.id_sucursal
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p ON p.id_perfil = u.id_perfil
    WHERE u.id_usuario = @idUsuarioSolicitante
      AND u.eliminado_en IS NULL
      AND p.eliminado_en IS NULL;

    IF @perfil IS NULL
        THROW 51000, 'El usuario solicitante no está activo.', 1;

    IF @desde IS NULL OR @hasta IS NULL OR @desde > @hasta
        THROW 51000, 'El período indicado no es válido.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.PERFIL_FUNCIONALIDAD AS pf
        INNER JOIN dbo.FUNCIONALIDAD AS f ON f.id_funcionalidad = pf.id_funcionalidad
        WHERE pf.id_perfil = @perfil
          AND f.codigo = N'REPORTES_VER'
          AND f.eliminado_en IS NULL
    )
        THROW 51000, 'No tiene permiso para acceder a Reportes.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.PERFIL_FUNCIONALIDAD AS pf
        INNER JOIN dbo.FUNCIONALIDAD AS f ON f.id_funcionalidad = pf.id_funcionalidad
        WHERE pf.id_perfil = @perfil
          AND f.codigo = N'REPORTES_RENDIMIENTO_VENDEDORES'
          AND f.eliminado_en IS NULL
    )
        THROW 51000, 'No tiene permiso para consultar actividad por usuario.', 1;

    IF @alcanceGlobal = 1
    BEGIN
        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.PERFIL_FUNCIONALIDAD AS pf
            INNER JOIN dbo.FUNCIONALIDAD AS f ON f.id_funcionalidad = pf.id_funcionalidad
            WHERE pf.id_perfil = @perfil
              AND f.codigo = N'REPORTES_ALCANCE_GLOBAL'
              AND f.eliminado_en IS NULL
        )
            THROW 51000, 'El perfil no posee alcance global de Reportes.', 1;
    END
    ELSE IF EXISTS
    (
        SELECT 1
        FROM dbo.PERFIL_FUNCIONALIDAD AS pf
        INNER JOIN dbo.FUNCIONALIDAD AS f ON f.id_funcionalidad = pf.id_funcionalidad
        WHERE pf.id_perfil = @perfil
          AND f.codigo = N'REPORTES_ALCANCE_SUCURSAL'
          AND f.eliminado_en IS NULL
    )
    BEGIN
        IF @sucursalAsignada IS NULL
           OR NOT EXISTS
              (SELECT 1 FROM dbo.SUCURSAL WHERE id_sucursal = @sucursalAsignada AND eliminado_en IS NULL)
            THROW 51000, 'El perfil requiere una sucursal fija activa para consultar este alcance.', 1;

        SET @idSucursal = @sucursalAsignada;
        SET @idUsuario = NULL;
    END
    ELSE IF EXISTS
    (
        SELECT 1
        FROM dbo.PERFIL_FUNCIONALIDAD AS pf
        INNER JOIN dbo.FUNCIONALIDAD AS f ON f.id_funcionalidad = pf.id_funcionalidad
        WHERE pf.id_perfil = @perfil
          AND f.codigo = N'REPORTES_ALCANCE_PROPIO'
          AND f.eliminado_en IS NULL
    )
    BEGIN
        SET @idUsuario = @idUsuarioSolicitante;
        SET @idSucursal = NULL;
    END
    ELSE
        THROW 51000, 'El perfil no posee un alcance válido de Reportes.', 1;

    SELECT
        a.fecha,
        a.accion,
        a.entidad,
        a.detalle,
        s.nombre AS sucursal
    FROM dbo.AUDITORIA AS a
    LEFT JOIN dbo.SUCURSAL AS s ON s.id_sucursal = a.id_sucursal
    WHERE a.fecha >= @desde
      AND a.fecha < DATEADD(DAY, 1, @hasta)
      AND (@idUsuario IS NULL OR a.id_usuario = @idUsuario)
      AND (@idSucursal IS NULL OR a.id_sucursal = @idSucursal)
    ORDER BY a.fecha DESC, a.id_auditoria DESC;
END;
GO


-- ============================================================
-- REPORTES
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Reporte_Recaudacion
    @desde DATE,
    @hasta DATE,
    @idSucursal INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(*) AS cantidad_ventas,
        CAST(ISNULL(SUM(v.subtotal), 0) AS DECIMAL(18,2)) AS subtotal,
        CAST(ISNULL(SUM(v.descuento), 0) AS DECIMAL(18,2)) AS descuentos,
        CAST(ISNULL(SUM(v.total), 0) AS DECIMAL(18,2)) AS recaudacion_total
    FROM dbo.VENTA AS v
    WHERE v.eliminado_en IS NULL
      AND v.fecha_hora >= @desde
      AND v.fecha_hora < DATEADD(DAY, 1, @hasta)
      AND (@idSucursal IS NULL OR v.id_sucursal = @idSucursal);
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Reporte_ProductosMasVendidos
    @desde DATE,
    @hasta DATE,
    @idSucursal INT = NULL,
    @idUsuario INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_producto,
        p.codigo_barra,
        p.nombre AS producto,
        c.nombre AS categoria,
        m.nombre AS marca,
        SUM(dv.cantidad) AS unidades_vendidas,
        CAST(SUM(dv.subtotal) AS DECIMAL(18,2)) AS importe_vendido
    FROM dbo.DETALLE_VENTA AS dv
    INNER JOIN dbo.VENTA AS v
        ON v.id_venta = dv.id_venta
    INNER JOIN dbo.PRODUCTO AS p
        ON p.id_producto = dv.id_producto
    INNER JOIN dbo.CATEGORIA AS c
        ON c.id_categoria = p.id_categoria
    LEFT JOIN dbo.MARCA AS m
        ON m.id_marca = p.id_marca
    WHERE v.eliminado_en IS NULL
      AND dv.eliminado_en IS NULL
      AND v.fecha_hora >= @desde
      AND v.fecha_hora < DATEADD(DAY, 1, @hasta)
      AND (@idSucursal IS NULL OR v.id_sucursal = @idSucursal)
      AND (@idUsuario IS NULL OR v.id_usuario = @idUsuario)
    GROUP BY
        p.id_producto,
        p.codigo_barra,
        p.nombre,
        c.nombre,
        m.nombre
    ORDER BY unidades_vendidas DESC, importe_vendido DESC, p.nombre;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Reporte_VentasPorVendedor
    @idUsuario INT,
    @desde DATE,
    @hasta DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.id_usuario,
        u.nombre,
        u.apellido,
        COUNT(v.id_venta) AS cantidad_ventas,
        CAST(ISNULL(SUM(v.total), 0) AS DECIMAL(18,2)) AS total_vendido
    FROM dbo.USUARIO AS u
    LEFT JOIN dbo.VENTA AS v
        ON v.id_usuario = u.id_usuario
       AND v.eliminado_en IS NULL
       AND v.fecha_hora >= @desde
       AND v.fecha_hora < DATEADD(DAY, 1, @hasta)
    WHERE u.id_usuario = @idUsuario
      AND u.eliminado_en IS NULL
    GROUP BY
        u.id_usuario,
        u.nombre,
        u.apellido;
END;
GO

-- Consultas reutilizables del módulo Reportes; los filtros llegan validados desde ReporteLogica.
CREATE OR ALTER PROCEDURE dbo.sp_Reporte_Resumen @desde DATE, @hasta DATE, @idSucursal INT = NULL, @idUsuario INT = NULL
AS BEGIN SET NOCOUNT ON;
 SELECT COUNT(*) AS ventas, CAST(ISNULL(SUM(subtotal),0) AS DECIMAL(18,2)) AS subtotal,
 CAST(ISNULL(SUM(descuento),0) AS DECIMAL(18,2)) AS descuentos, CAST(ISNULL(SUM(total),0) AS DECIMAL(18,2)) AS recaudacion
 FROM dbo.VENTA WHERE eliminado_en IS NULL AND fecha_hora >= @desde AND fecha_hora < DATEADD(DAY,1,@hasta)
 AND (@idSucursal IS NULL OR id_sucursal=@idSucursal) AND (@idUsuario IS NULL OR id_usuario=@idUsuario); END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Reporte_VentasPorDia @desde DATE, @hasta DATE, @idSucursal INT = NULL, @idUsuario INT = NULL
AS BEGIN SET NOCOUNT ON;
 ;WITH dias AS (SELECT @desde fecha UNION ALL SELECT DATEADD(DAY,1,fecha) FROM dias WHERE fecha < @hasta)
 SELECT d.fecha, COUNT(v.id_venta) ventas, CAST(ISNULL(SUM(v.total),0) AS DECIMAL(18,2)) recaudacion
 FROM dias d LEFT JOIN dbo.VENTA v ON CAST(v.fecha_hora AS DATE)=d.fecha AND v.eliminado_en IS NULL
 AND (@idSucursal IS NULL OR v.id_sucursal=@idSucursal) AND (@idUsuario IS NULL OR v.id_usuario=@idUsuario)
 GROUP BY d.fecha ORDER BY d.fecha OPTION(MAXRECURSION 366); END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Reporte_Vendedores @desde DATE, @hasta DATE, @idSucursal INT = NULL, @idUsuario INT = NULL
AS BEGIN SET NOCOUNT ON;
 SELECT u.id_usuario, CONCAT(u.nombre,N' ',u.apellido) vendedor, COUNT(v.id_venta) ventas, CAST(ISNULL(SUM(v.total),0) AS DECIMAL(18,2)) total
 FROM dbo.USUARIO u LEFT JOIN dbo.VENTA v ON v.id_usuario=u.id_usuario AND v.eliminado_en IS NULL AND v.fecha_hora>=@desde AND v.fecha_hora<DATEADD(DAY,1,@hasta)
 AND (@idSucursal IS NULL OR v.id_sucursal=@idSucursal)
 WHERE u.eliminado_en IS NULL AND (@idUsuario IS NULL OR u.id_usuario=@idUsuario)
 GROUP BY u.id_usuario,u.nombre,u.apellido ORDER BY total DESC,vendedor; END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Reporte_StockBajo @idSucursal INT = NULL
AS BEGIN SET NOCOUNT ON;
 SELECT p.id_producto,s.id_sucursal,p.codigo_barra,p.nombre producto,c.nombre categoria,s.nombre sucursal,i.stock,i.stock_minimo FROM dbo.INVENTARIO i
 INNER JOIN dbo.PRODUCTO p ON p.id_producto=i.id_producto INNER JOIN dbo.CATEGORIA c ON c.id_categoria=p.id_categoria INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=i.id_sucursal
 WHERE i.eliminado_en IS NULL AND p.eliminado_en IS NULL AND p.activo=1 AND s.eliminado_en IS NULL AND i.stock<=i.stock_minimo
 AND (@idSucursal IS NULL OR i.id_sucursal=@idSucursal) ORDER BY s.nombre,p.nombre; END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Reporte_DetalleVentas @desde DATE, @hasta DATE, @idSucursal INT = NULL, @idUsuario INT = NULL
AS BEGIN SET NOCOUNT ON;
 SELECT v.id_venta,v.fecha_hora,CONCAT(c.nombre,N' ',c.apellido) cliente,CONCAT(u.nombre,N' ',u.apellido) vendedor,s.nombre sucursal,v.subtotal,v.descuento,v.total,
        pagos.metodos_pago
 FROM dbo.VENTA v INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=v.id_sucursal
 OUTER APPLY (SELECT STRING_AGG(mp.nombre,N', ') metodos_pago FROM dbo.PAGO p INNER JOIN dbo.METODO_PAGO mp ON mp.id_metodo_pago=p.id_metodo_pago WHERE p.id_venta=v.id_venta AND p.eliminado_en IS NULL) pagos
 WHERE v.eliminado_en IS NULL AND v.fecha_hora>=@desde AND v.fecha_hora<DATEADD(DAY,1,@hasta)
 AND (@idSucursal IS NULL OR v.id_sucursal=@idSucursal) AND (@idUsuario IS NULL OR v.id_usuario=@idUsuario) ORDER BY v.fecha_hora DESC; END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Reporte_ListarVendedores @idSucursal INT = NULL
AS BEGIN SET NOCOUNT ON;
 SELECT u.id_usuario,CONCAT(u.nombre,N' ',u.apellido) nombre FROM dbo.USUARIO u
 WHERE u.eliminado_en IS NULL AND (@idSucursal IS NULL OR u.id_sucursal=@idSucursal) ORDER BY nombre; END;
GO


-- ============================================================
-- CATÁLOGOS AUXILIARES
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Sucursal_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_sucursal,
        nombre,
        telefono
    FROM dbo.SUCURSAL
    WHERE eliminado_en IS NULL
    ORDER BY nombre;
END;
GO


-- Devuelve todas las sucursales activas y una fila por cada perfil activo para el resumen dinámico.
CREATE OR ALTER PROCEDURE dbo.sp_Sucursal_ListarResumenUsuarios
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.id_sucursal,
        s.nombre AS nombre_sucursal,
        d.calle,
        l.nombre AS localidad,
        pr.nombre AS provincia,
        p.id_perfil,
        p.nombre AS perfil,
        COUNT(u.id_usuario) AS cantidad_usuarios
    FROM dbo.SUCURSAL AS s
    LEFT JOIN dbo.DIRECCION AS d
        ON d.id_direccion = s.id_direccion
       AND d.eliminado_en IS NULL
    LEFT JOIN dbo.LOCALIDAD AS l
        ON l.id_localidad = d.id_localidad
       AND l.eliminado_en IS NULL
    LEFT JOIN dbo.PROVINCIA AS pr
        ON pr.id_provincia = l.id_provincia
       AND pr.eliminado_en IS NULL
    CROSS JOIN dbo.PERFIL AS p
    LEFT JOIN dbo.USUARIO AS u
        ON u.id_sucursal = s.id_sucursal
       AND u.id_perfil = p.id_perfil
       AND u.eliminado_en IS NULL
    WHERE s.eliminado_en IS NULL
      AND p.eliminado_en IS NULL
    GROUP BY
        s.id_sucursal,
        s.nombre,
        d.calle,
        l.nombre,
        pr.nombre,
        p.id_perfil,
        p.nombre
    ORDER BY s.nombre, p.nombre;
END;
GO


-- Crea una sucursal y su dirección en una transacción para no dejar direcciones huérfanas.
CREATE OR ALTER PROCEDURE dbo.sp_Sucursal_Alta
    @nombre NVARCHAR(100),
    @idLocalidad INT,
    @calle NVARCHAR(150),
    @idUsuarioEjecutor INT,
    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';
    SET @nombre = LTRIM(RTRIM(ISNULL(@nombre, N'')));
    SET @calle = LTRIM(RTRIM(ISNULL(@calle, N'')));

    WHILE CHARINDEX(N'  ', @nombre) > 0 SET @nombre = REPLACE(@nombre, N'  ', N' ');
    WHILE CHARINDEX(N'  ', @calle) > 0 SET @calle = REPLACE(@calle, N'  ', N' ');

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;
        IF @nombre = N'' OR LEN(@nombre) > 100
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El nombre de la sucursal es obligatorio y debe tener hasta 100 caracteres.';
            RETURN;
        END;

        IF @calle = N'' OR LEN(@calle) > 150
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'La dirección es obligatoria y debe tener hasta 150 caracteres.';
            RETURN;
        END;

        IF NOT EXISTS (SELECT 1 FROM dbo.LOCALIDAD WHERE id_localidad = @idLocalidad AND eliminado_en IS NULL)
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La localidad indicada no existe o está inactiva.';
            RETURN;
        END;

        IF EXISTS (SELECT 1 FROM dbo.SUCURSAL WHERE UPPER(LTRIM(RTRIM(nombre))) = UPPER(@nombre))
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado = N'Ya existe una sucursal con ese nombre. Si está inactiva, reactivala en lugar de crearla nuevamente.';
            RETURN;
        END;

        BEGIN TRANSACTION;

        INSERT INTO dbo.DIRECCION (id_localidad, calle)
        VALUES (@idLocalidad, @calle);

        DECLARE @idDireccion INT = CAST(SCOPE_IDENTITY() AS INT);

        INSERT INTO dbo.SUCURSAL (nombre, id_direccion)
        VALUES (@nombre, @idDireccion);

        SET @IdGenerado = CAST(SCOPE_IDENTITY() AS INT);

        DECLARE @detalleAuditoriaSucursal NVARCHAR(300) = CONCAT(N'Sucursal registrada: ', @nombre, N'.');
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'ALTA', @entidad = N'SUCURSAL',
            @idEntidad = @IdGenerado, @detalle = @detalleAuditoriaSucursal,
            @idSucursal = @IdGenerado;

        COMMIT TRANSACTION;
        SET @MensajeResultado = N'Sucursal registrada correctamente.';
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
        THROW;
    END CATCH;
END;
GO


-- Actualiza nombre y ubicación de una sucursal, creando una dirección solo si aún no tiene una asociada.
CREATE OR ALTER PROCEDURE dbo.sp_Sucursal_Modificar
    @idSucursal INT,
    @nombre NVARCHAR(100),
    @idLocalidad INT,
    @calle NVARCHAR(150),
    @idUsuarioEjecutor INT,
    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';
    SET @nombre = LTRIM(RTRIM(ISNULL(@nombre, N'')));
    SET @calle = LTRIM(RTRIM(ISNULL(@calle, N'')));

    WHILE CHARINDEX(N'  ', @nombre) > 0 SET @nombre = REPLACE(@nombre, N'  ', N' ');
    WHILE CHARINDEX(N'  ', @calle) > 0 SET @calle = REPLACE(@calle, N'  ', N' ');

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.SUCURSAL WHERE id_sucursal = @idSucursal AND eliminado_en IS NULL)
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La sucursal indicada no existe o está inactiva.';
            RETURN;
        END;

        IF @nombre = N'' OR LEN(@nombre) > 100 OR @calle = N'' OR LEN(@calle) > 150
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El nombre y la dirección son obligatorios y exceden el máximo permitido.';
            RETURN;
        END;

        IF NOT EXISTS (SELECT 1 FROM dbo.LOCALIDAD WHERE id_localidad = @idLocalidad AND eliminado_en IS NULL)
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La localidad indicada no existe o está inactiva.';
            RETURN;
        END;

        IF EXISTS (SELECT 1 FROM dbo.SUCURSAL WHERE id_sucursal <> @idSucursal AND UPPER(LTRIM(RTRIM(nombre))) = UPPER(@nombre))
        BEGIN
            SET @CodigoResultado = 2;
            SET @MensajeResultado = N'Ya existe otra sucursal con ese nombre.';
            RETURN;
        END;

        BEGIN TRANSACTION;

        DECLARE @idDireccionActual INT;
        SELECT @idDireccionActual = id_direccion FROM dbo.SUCURSAL WHERE id_sucursal = @idSucursal;

        IF @idDireccionActual IS NULL
        BEGIN
            INSERT INTO dbo.DIRECCION (id_localidad, calle) VALUES (@idLocalidad, @calle);
            SET @idDireccionActual = CAST(SCOPE_IDENTITY() AS INT);
            UPDATE dbo.SUCURSAL SET id_direccion = @idDireccionActual WHERE id_sucursal = @idSucursal;
        END
        ELSE
        BEGIN
            UPDATE dbo.DIRECCION
            SET id_localidad = @idLocalidad, calle = @calle, eliminado_en = NULL
            WHERE id_direccion = @idDireccionActual;
        END;

        UPDATE dbo.SUCURSAL SET nombre = @nombre WHERE id_sucursal = @idSucursal;

        DECLARE @detalleAuditoriaSucursal NVARCHAR(300) = CONCAT(N'Sucursal actualizada: ', @nombre, N'.');
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'MODIFICACION', @entidad = N'SUCURSAL',
            @idEntidad = @idSucursal, @detalle = @detalleAuditoriaSucursal,
            @idSucursal = @idSucursal;

        COMMIT TRANSACTION;
        SET @IdGenerado = @idSucursal;
        SET @MensajeResultado = N'Sucursal actualizada correctamente.';
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
        THROW;
    END CATCH;
END;
GO


-- Realiza baja lógica sin desactivar una sucursal que todavía tenga usuarios activos asignados.
CREATE OR ALTER PROCEDURE dbo.sp_Sucursal_Baja
    @idSucursal INT,
    @idUsuarioEjecutor INT,
    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.SUCURSAL WHERE id_sucursal = @idSucursal AND eliminado_en IS NULL)
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La sucursal indicada no existe o ya está inactiva.';
            RETURN;
        END;

        IF EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_sucursal = @idSucursal AND eliminado_en IS NULL)
        BEGIN
            SET @CodigoResultado = 4;
            SET @MensajeResultado = N'No se puede dar de baja una sucursal con usuarios activos asignados.';
            RETURN;
        END;

        BEGIN TRANSACTION;
        UPDATE dbo.SUCURSAL SET eliminado_en = SYSDATETIME() WHERE id_sucursal = @idSucursal;
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'BAJA', @entidad = N'SUCURSAL',
            @idEntidad = @idSucursal, @detalle = N'Sucursal dada de baja.',
            @idSucursal = @idSucursal;
        COMMIT TRANSACTION;
        SET @IdGenerado = @idSucursal;
        SET @MensajeResultado = N'Sucursal dada de baja correctamente.';
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
        THROW;
    END CATCH;
END;
GO


-- Reactiva una sucursal previamente dada de baja sin crear un registro duplicado.
CREATE OR ALTER PROCEDURE dbo.sp_Sucursal_Reactivar
    @idSucursal INT,
    @idUsuarioEjecutor INT,
    @IdGenerado INT OUTPUT,
    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE id_usuario = @idUsuarioEjecutor AND eliminado_en IS NULL)
            THROW 51000, 'El usuario ejecutor no está activo.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.SUCURSAL WHERE id_sucursal = @idSucursal AND eliminado_en IS NOT NULL)
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La sucursal indicada no existe o ya está activa.';
            RETURN;
        END;

        BEGIN TRANSACTION;
        UPDATE dbo.SUCURSAL SET eliminado_en = NULL WHERE id_sucursal = @idSucursal;
        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuarioEjecutor, @accion = N'REACTIVACION', @entidad = N'SUCURSAL',
            @idEntidad = @idSucursal, @detalle = N'Sucursal reactivada.',
            @idSucursal = @idSucursal;
        COMMIT TRANSACTION;
        SET @IdGenerado = @idSucursal;
        SET @MensajeResultado = N'Sucursal reactivada correctamente.';
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
        THROW;
    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_MetodoPago_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_metodo_pago,
        nombre,
        descripcion
    FROM dbo.METODO_PAGO
    WHERE eliminado_en IS NULL
    ORDER BY nombre;
END;
GO


-- ============================================================
-- DASHBOARD
-- Consultas compactas para Inicio. @idSucursal NULL representa
-- el alcance global seleccionado en la sesión.
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Dashboard_ObtenerResumen
    @idSucursal INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        (SELECT COUNT(*) FROM dbo.VENTA AS v
         WHERE v.eliminado_en IS NULL
           AND CAST(v.fecha_hora AS DATE) = CAST(GETDATE() AS DATE)
           AND (@idSucursal IS NULL OR v.id_sucursal = @idSucursal)) AS ventas_hoy,
        CAST((SELECT ISNULL(SUM(v.total), 0) FROM dbo.VENTA AS v
              WHERE v.eliminado_en IS NULL
                AND CAST(v.fecha_hora AS DATE) = CAST(GETDATE() AS DATE)
                AND (@idSucursal IS NULL OR v.id_sucursal = @idSucursal)) AS DECIMAL(18,2)) AS ingresos_hoy,
        (SELECT COUNT(*) FROM dbo.INVENTARIO AS i
         INNER JOIN dbo.PRODUCTO AS p ON p.id_producto = i.id_producto
         WHERE i.eliminado_en IS NULL AND p.eliminado_en IS NULL AND p.activo = 1
           AND i.stock <= i.stock_minimo
           AND (@idSucursal IS NULL OR i.id_sucursal = @idSucursal)) AS stock_bajo,
        (SELECT COUNT(*) FROM dbo.PRODUCTO AS p
         WHERE p.eliminado_en IS NULL AND p.activo = 1) AS productos_activos;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Dashboard_VentasUltimos7Dias
    @idSucursal INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Dias AS
    (
        SELECT CAST(DATEADD(DAY, -6, CAST(GETDATE() AS DATE)) AS DATE) AS fecha
        UNION ALL SELECT DATEADD(DAY, 1, fecha) FROM Dias WHERE fecha < CAST(GETDATE() AS DATE)
    )
    SELECT d.fecha,
           COUNT(v.id_venta) AS ventas,
           CAST(ISNULL(SUM(v.total), 0) AS DECIMAL(18,2)) AS ingresos
    FROM Dias AS d
    LEFT JOIN dbo.VENTA AS v
      ON v.eliminado_en IS NULL
     AND CAST(v.fecha_hora AS DATE) = d.fecha
     AND (@idSucursal IS NULL OR v.id_sucursal = @idSucursal)
    GROUP BY d.fecha
    ORDER BY d.fecha
    OPTION (MAXRECURSION 7);
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Dashboard_ProductosMasVendidos
    @idSucursal INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (5)
        p.nombre AS producto,
        SUM(dv.cantidad) AS unidades_vendidas
    FROM dbo.DETALLE_VENTA AS dv
    INNER JOIN dbo.VENTA AS v ON v.id_venta = dv.id_venta
    INNER JOIN dbo.PRODUCTO AS p ON p.id_producto = dv.id_producto
    WHERE v.eliminado_en IS NULL
      AND dv.eliminado_en IS NULL
      AND v.fecha_hora >= DATEADD(DAY, -6, CAST(GETDATE() AS DATE))
      AND (@idSucursal IS NULL OR v.id_sucursal = @idSucursal)
    GROUP BY p.id_producto, p.nombre
    ORDER BY SUM(dv.cantidad) DESC, p.nombre;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Dashboard_ActividadReciente
    @idSucursal INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (5)
        v.fecha_hora,
        CONCAT(c.nombre, N' ', c.apellido) AS cliente,
        CONCAT(u.nombre, N' ', u.apellido) AS vendedor,
        v.total
    FROM dbo.VENTA AS v
    INNER JOIN dbo.CLIENTE AS c ON c.id_cliente = v.id_cliente
    INNER JOIN dbo.USUARIO AS u ON u.id_usuario = v.id_usuario
    WHERE v.eliminado_en IS NULL
      AND (@idSucursal IS NULL OR v.id_sucursal = @idSucursal)
    ORDER BY v.fecha_hora DESC;
END;
GO


-- ============================================================
-- AVISOS INTERNOS
-- La visibilidad se determina por AVISOS_VER y por el alcance
-- global o de sucursal, sin destinatarios configurados por perfil.
-- ============================================================

DROP PROCEDURE IF EXISTS dbo.sp_Perfil_AvisoDestino_Listar;
DROP PROCEDURE IF EXISTS dbo.sp_Perfil_AvisoDestino_Guardar;
DROP PROCEDURE IF EXISTS dbo.sp_Aviso_ListarDestinos;
GO

-- Publica un aviso global o de sucursal validando el permiso y el alcance real del autor.
CREATE OR ALTER PROCEDURE dbo.sp_Aviso_Publicar
    @idUsuario INT,
    @idSucursal INT = NULL,
    @titulo NVARCHAR(100),
    @mensaje NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @idPerfilEmisor INT, @alcanceGlobal BIT, @idSucursalFija INT;

    SELECT @idPerfilEmisor = u.id_perfil, @alcanceGlobal = p.alcance_global, @idSucursalFija = u.id_sucursal
    FROM dbo.USUARIO AS u INNER JOIN dbo.PERFIL AS p ON p.id_perfil = u.id_perfil
    WHERE u.id_usuario = @idUsuario AND u.eliminado_en IS NULL AND p.eliminado_en IS NULL;

    IF @idPerfilEmisor IS NULL THROW 51000, 'El autor del aviso no está activo.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.PERFIL_FUNCIONALIDAD pf INNER JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad = pf.id_funcionalidad WHERE pf.id_perfil = @idPerfilEmisor AND f.codigo = N'AVISOS_PUBLICAR' AND f.eliminado_en IS NULL)
        THROW 51000, 'No tiene permiso para publicar avisos.', 1;
    IF LEN(LTRIM(RTRIM(ISNULL(@titulo,N'')))) = 0 OR LEN(LTRIM(RTRIM(ISNULL(@mensaje,N'')))) = 0 OR LEN(@titulo) > 100 OR LEN(@mensaje) > 500
        THROW 51000, 'El título y el mensaje del aviso son obligatorios y deben respetar su longitud máxima.', 1;

    IF @alcanceGlobal = 0
    BEGIN
        IF @idSucursalFija IS NULL OR (@idSucursal IS NOT NULL AND @idSucursal <> @idSucursalFija)
            THROW 51000, 'El aviso debe utilizar la sucursal asignada al autor.', 1;
        SET @idSucursal = @idSucursalFija;
    END;

    IF @idSucursal IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.SUCURSAL WHERE id_sucursal = @idSucursal AND eliminado_en IS NULL)
        THROW 51000, 'La sucursal indicada no existe o está inactiva.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO dbo.AVISO(titulo,mensaje,id_usuario_autor,id_sucursal,activo)
        VALUES(LTRIM(RTRIM(@titulo)),LTRIM(RTRIM(@mensaje)),@idUsuario,@idSucursal,1);

        DECLARE @idAviso INT = CAST(SCOPE_IDENTITY() AS INT);

        DECLARE @detalleAuditoria NVARCHAR(300);
        SET @detalleAuditoria = CONCAT(N'Aviso publicado: ', LEFT(LTRIM(RTRIM(@titulo)), 200));

        EXEC dbo.sp_Auditoria_Registrar
            @idUsuario = @idUsuario,
            @accion = N'ALTA',
            @entidad = N'AVISO',
            @idEntidad = @idAviso,
            @detalle = @detalleAuditoria,
            @idSucursal = @idSucursal;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO


-- Lista avisos autorizados por AVISOS_VER y por el alcance vigente del receptor.
CREATE OR ALTER PROCEDURE dbo.sp_Aviso_ListarParaUsuario
    @idUsuario INT,
    @idSucursalOperativa INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT a.id_aviso,a.titulo,a.mensaje,a.fecha_creacion,
           CONCAT(autor.nombre,N' ',autor.apellido) AS autor,
           COALESCE(s.nombre,N'Todas las sucursales') AS alcance
    FROM dbo.AVISO a
    INNER JOIN dbo.USUARIO u ON u.id_usuario = @idUsuario AND u.eliminado_en IS NULL
    INNER JOIN dbo.PERFIL receptor ON receptor.id_perfil = u.id_perfil AND receptor.eliminado_en IS NULL
    INNER JOIN dbo.PERFIL_FUNCIONALIDAD pf ON pf.id_perfil = receptor.id_perfil
    INNER JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad = pf.id_funcionalidad AND f.codigo = N'AVISOS_VER' AND f.eliminado_en IS NULL
    INNER JOIN dbo.USUARIO autor ON autor.id_usuario = a.id_usuario_autor AND autor.eliminado_en IS NULL
    LEFT JOIN dbo.SUCURSAL s ON s.id_sucursal = a.id_sucursal
    WHERE a.activo = 1 AND a.eliminado_en IS NULL
      AND
      (
          a.id_sucursal IS NULL
          OR (receptor.alcance_global = 0 AND a.id_sucursal = u.id_sucursal)
          OR (receptor.alcance_global = 1 AND (@idSucursalOperativa IS NULL OR a.id_sucursal = @idSucursalOperativa))
      )
    ORDER BY a.fecha_creacion DESC;
END;
GO



-- ============================================================
-- SINCRONIZACIÓN AUTOMÁTICA DEL PERFIL GLOBAL
--
-- Si se agrega o reactiva una funcionalidad, el perfil global
-- recibe automáticamente ese permiso.
-- ============================================================

CREATE OR ALTER TRIGGER dbo.trg_Funcionalidad_SincronizarPerfilGlobal
ON dbo.FUNCIONALIDAD
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.PERFIL_FUNCIONALIDAD
    (
        id_perfil,
        id_funcionalidad
    )
    SELECT
        p.id_perfil,
        i.id_funcionalidad
    FROM inserted AS i
    CROSS JOIN dbo.PERFIL AS p
    WHERE i.eliminado_en IS NULL
      AND p.alcance_global = 1
      AND p.eliminado_en IS NULL
      AND i.codigo NOT IN
      (
          N'REPORTES_ALCANCE_PROPIO',
          N'REPORTES_ALCANCE_SUCURSAL'
      )
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.PERFIL_FUNCIONALIDAD AS pf
          WHERE pf.id_perfil = p.id_perfil
            AND pf.id_funcionalidad = i.id_funcionalidad
      );
END;
GO


-- ============================================================
-- SINCRONIZACIÓN INICIAL DEL PERFIL GLOBAL
-- ============================================================

DECLARE @CodigoResultadoSincronizacion INT;
DECLARE @MensajeResultadoSincronizacion NVARCHAR(250);

EXEC dbo.sp_Perfil_SincronizarAdministrador
    @CodigoResultado =
        @CodigoResultadoSincronizacion OUTPUT,
    @MensajeResultado =
        @MensajeResultadoSincronizacion OUTPUT;

SELECT
    @CodigoResultadoSincronizacion AS CodigoResultado,
    @MensajeResultadoSincronizacion AS MensajeResultado;
GO


-- ============================================================
-- FIN DEL SCRIPT
-- ============================================================
