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
        ISNULL(s.nombre, N'Todas las sucursales') AS sucursal
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.id_perfil = u.id_perfil
    LEFT JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = u.id_sucursal
    WHERE u.eliminado_en IS NULL
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
        ISNULL(s.nombre, N'Todas las sucursales') AS sucursal
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.id_perfil = u.id_perfil
    LEFT JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = u.id_sucursal
    WHERE u.id_usuario = @idUsuario
      AND u.eliminado_en IS NULL;
END;
GO


/* ============================================================
   Procedimiento: sp_Usuario_Alta

   Regla de sucursal:
   - Administrador: no pertenece a una sucursal específica.
     Su id_sucursal se guarda en NULL.
   - Gerente y Vendedor: deben tener una sucursal asignada.

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

    DECLARE @nombrePerfil NVARCHAR(100);

    SET @IdGenerado = 0;
    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';


    BEGIN TRY

        /* -----------------------------------------------------
           Validar perfil
           ----------------------------------------------------- */

        SELECT
            @nombrePerfil = nombre
        FROM dbo.PERFIL
        WHERE id_perfil = @idPerfil
          AND eliminado_en IS NULL;


        IF @nombrePerfil IS NULL
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado =
                N'El perfil indicado no existe o está inactivo.';

            RETURN;
        END;


        /* -----------------------------------------------------
           Regla de sucursal según perfil
           ----------------------------------------------------- */

        IF @nombrePerfil = N'Administrador'
        BEGIN
            /*
               El administrador trabaja a nivel global,
               por lo tanto no se asocia a una sucursal.
            */
            SET @idSucursal = NULL;
        END
        ELSE
        BEGIN

            /*
               Gerentes, vendedores y demás perfiles
               deben estar asociados a una sucursal.
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

        IF LTRIM(RTRIM(ISNULL(@nombre, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@apellido, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@dni, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@nombreUsuario, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@contrasenaHash, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@correo, N''))) = N''
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado =
                N'Faltan datos obligatorios del usuario.';

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

            LTRIM(RTRIM(@nombre)),
            LTRIM(RTRIM(@apellido)),
            LTRIM(RTRIM(@dni)),
            NULLIF(LTRIM(RTRIM(@telefono)), N''),

            LTRIM(RTRIM(@nombreUsuario)),
            @contrasenaHash,
            LTRIM(RTRIM(@correo)),

            NULLIF(LTRIM(RTRIM(@sexo)), N''),
            @fechaNacimiento,
            @idDireccion
        );


        SET @IdGenerado =
            CAST(SCOPE_IDENTITY() AS INT);

        SET @CodigoResultado = 0;
        SET @MensajeResultado =
            N'Usuario registrado correctamente.';


    END TRY

    BEGIN CATCH

        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();

    END CATCH;

END;
GO


/* ============================================================
   Procedimiento: sp_Usuario_Modificar

   Regla de sucursal:
   - Administrador: no pertenece a una sucursal específica.
     Su id_sucursal se guarda en NULL.
   - Gerente y Vendedor: deben tener una sucursal asignada.

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

    DECLARE @nombrePerfil NVARCHAR(100);

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

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
            @nombrePerfil = nombre
        FROM dbo.PERFIL
        WHERE id_perfil = @idPerfil
          AND eliminado_en IS NULL;


        IF @nombrePerfil IS NULL
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado =
                N'El perfil indicado no existe o está inactivo.';

            RETURN;
        END;


        /* -----------------------------------------------------
           Regla de sucursal según perfil
           ----------------------------------------------------- */

        IF @nombrePerfil = N'Administrador'
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

        IF LTRIM(RTRIM(ISNULL(@nombre, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@apellido, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@dni, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@nombreUsuario, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@correo, N''))) = N''
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado =
                N'Faltan datos obligatorios del usuario.';

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

        UPDATE dbo.USUARIO
        SET
            id_perfil = @idPerfil,
            id_sucursal = @idSucursal,
            nombre = LTRIM(RTRIM(@nombre)),
            apellido = LTRIM(RTRIM(@apellido)),
            dni = LTRIM(RTRIM(@dni)),
            telefono = NULLIF(LTRIM(RTRIM(@telefono)), N''),
            nombre_usuario = LTRIM(RTRIM(@nombreUsuario)),
            correo = LTRIM(RTRIM(@correo)),
            sexo = NULLIF(LTRIM(RTRIM(@sexo)), N''),
            fecha_nacimiento = @fechaNacimiento,
            id_direccion = @idDireccion
        WHERE id_usuario = @idUsuario
          AND eliminado_en IS NULL;


        SET @CodigoResultado = 0;
        SET @MensajeResultado =
            N'Usuario modificado correctamente.';


    END TRY

    BEGIN CATCH

        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();

    END CATCH;

END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Usuario_Baja
    @idUsuario INT,
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
            FROM dbo.USUARIO
            WHERE id_usuario = @idUsuario
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El usuario no existe o ya fue dado de baja.';
            RETURN;
        END;

        UPDATE dbo.USUARIO
        SET eliminado_en = SYSDATETIME()
        WHERE id_usuario = @idUsuario
          AND eliminado_en IS NULL;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Usuario dado de baja correctamente.';

    END TRY
    BEGIN CATCH
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
        descripcion
    FROM dbo.PERFIL
    WHERE eliminado_en IS NULL
    ORDER BY nombre;
END;
GO


-- ============================================================
-- CLIENTES
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Buscar
    @texto NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SET @texto = LTRIM(RTRIM(ISNULL(@texto, N'')));

    SELECT TOP 50
        c.id_cliente,
        c.nombre,
        c.apellido,
        c.documento,
        c.correo,
        c.telefono
    FROM dbo.CLIENTE AS c
    WHERE c.eliminado_en IS NULL
      AND
      (
          @texto = N''
          OR c.nombre LIKE N'%' + @texto + N'%'
          OR c.apellido LIKE N'%' + @texto + N'%'
          OR c.documento LIKE N'%' + @texto + N'%'
          OR ISNULL(c.correo, N'') LIKE N'%' + @texto + N'%'
          OR ISNULL(c.telefono, N'') LIKE N'%' + @texto + N'%'
      )
    ORDER BY c.apellido, c.nombre;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.id_cliente,
        c.nombre,
        c.apellido,
        c.documento,
        c.correo,
        c.telefono,
        c.id_direccion,
        l.nombre AS localidad,
        p.nombre AS provincia,
        d.calle,
        d.altura
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
    WHERE c.eliminado_en IS NULL
    ORDER BY c.apellido, c.nombre;
END;
GO


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

        IF LTRIM(RTRIM(ISNULL(@nombre, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@apellido, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@documento, N''))) = N''
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Nombre, apellido y documento son obligatorios.';
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
            LTRIM(RTRIM(@nombre)),
            LTRIM(RTRIM(@apellido)),
            LTRIM(RTRIM(@documento)),
            NULLIF(LTRIM(RTRIM(@correo)), N''),
            NULLIF(LTRIM(RTRIM(@telefono)), N''),
            @idDireccion
        );

        SET @IdGenerado = CAST(SCOPE_IDENTITY() AS INT);
        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Cliente registrado correctamente.';

    END TRY
    BEGIN CATCH
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
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
            WHERE id_cliente = @idCliente
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El cliente no existe o fue dado de baja.';
            RETURN;
        END;

        IF LTRIM(RTRIM(ISNULL(@nombre, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@apellido, N''))) = N''
           OR LTRIM(RTRIM(ISNULL(@documento, N''))) = N''
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'Nombre, apellido y documento son obligatorios.';
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

        UPDATE dbo.CLIENTE
        SET
            nombre = LTRIM(RTRIM(@nombre)),
            apellido = LTRIM(RTRIM(@apellido)),
            documento = LTRIM(RTRIM(@documento)),
            correo = NULLIF(LTRIM(RTRIM(@correo)), N''),
            telefono = NULLIF(LTRIM(RTRIM(@telefono)), N''),
            id_direccion = @idDireccion
        WHERE id_cliente = @idCliente
          AND eliminado_en IS NULL;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Cliente modificado correctamente.';

    END TRY
    BEGIN CATCH
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Baja
    @id_cliente INT,
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

        UPDATE dbo.CLIENTE
        SET eliminado_en = SYSDATETIME()
        WHERE id_cliente = @id_cliente
          AND eliminado_en IS NULL;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Cliente dado de baja correctamente.';

    END TRY
    BEGIN CATCH
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


-- ============================================================
-- CATEGORÍAS
-- ============================================================

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
    LEFT JOIN dbo.INVENTARIO AS i
        ON i.id_producto = p.id_producto
       AND i.id_sucursal = @idSucursal
       AND i.eliminado_en IS NULL
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

        SET @codigoBarra = NULLIF(LTRIM(RTRIM(@codigoBarra)), N'');

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
               SELECT 1
               FROM dbo.MARCA
               WHERE id_marca = @idMarca
                 AND eliminado_en IS NULL
           )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La marca indicada no existe o está inactiva.';
            RETURN;
        END;

        IF LTRIM(RTRIM(ISNULL(@nombre, N''))) = N''
           OR @precioCosto < 0
           OR @porcentajeGanancia < 0
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
            LTRIM(RTRIM(@nombre)),
            NULLIF(LTRIM(RTRIM(@descripcion)), N''),
            @precioCosto,
            @porcentajeGanancia,
            @activo
        );

        SET @IdGenerado = CAST(SCOPE_IDENTITY() AS INT);
        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Producto registrado correctamente.';

    END TRY
    BEGIN CATCH
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

    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        SET @codigoBarra = NULLIF(LTRIM(RTRIM(@codigoBarra)), N'');

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.PRODUCTO
            WHERE id_producto = @idProducto
              AND eliminado_en IS NULL
        )
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
               SELECT 1
               FROM dbo.MARCA
               WHERE id_marca = @idMarca
                 AND eliminado_en IS NULL
           )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'La marca indicada no existe o está inactiva.';
            RETURN;
        END;

        IF LTRIM(RTRIM(ISNULL(@nombre, N''))) = N''
           OR @precioCosto < 0
           OR @porcentajeGanancia < 0
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

        UPDATE dbo.PRODUCTO
        SET
            id_categoria = @idCategoria,
            id_marca = @idMarca,
            codigo_barra = @codigoBarra,
            nombre = LTRIM(RTRIM(@nombre)),
            descripcion = NULLIF(LTRIM(RTRIM(@descripcion)), N''),
            precio_costo = @precioCosto,
            porcentaje_ganancia = @porcentajeGanancia,
            activo = @activo
        WHERE id_producto = @idProducto
          AND eliminado_en IS NULL;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Producto modificado correctamente.';

    END TRY
    BEGIN CATCH
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Producto_Baja
    @idProducto INT,
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
            FROM dbo.PRODUCTO
            WHERE id_producto = @idProducto
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El producto no existe o ya fue dado de baja.';
            RETURN;
        END;

        UPDATE dbo.PRODUCTO
        SET eliminado_en = SYSDATETIME()
        WHERE id_producto = @idProducto
          AND eliminado_en IS NULL;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Producto dado de baja correctamente.';

    END TRY
    BEGIN CATCH
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


-- ============================================================
-- INVENTARIO
-- ============================================================

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

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.PRODUCTO
            WHERE id_producto = @idProducto
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'El producto indicado no existe o fue dado de baja.';
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

        IF @stock < 0 OR @stockMinimo < 0
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'El stock y el stock mínimo no pueden ser negativos.';
            RETURN;
        END;

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

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Stock actualizado correctamente para la sucursal indicada.';

    END TRY
    BEGIN CATCH
        SET @CodigoResultado = 500;
        SET @MensajeResultado = ERROR_MESSAGE();
    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Inventario_AjustarStock
    @idProducto INT,
    @idSucursal INT,
    @cantidad INT,

    @CodigoResultado INT OUTPUT,
    @MensajeResultado NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @CodigoResultado = 0;
    SET @MensajeResultado = N'Operación realizada correctamente.';

    BEGIN TRY

        IF @cantidad = 0
        BEGIN
            SET @CodigoResultado = 3;
            SET @MensajeResultado = N'La cantidad de ajuste no puede ser cero.';
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.INVENTARIO
            WHERE id_producto = @idProducto
              AND id_sucursal = @idSucursal
              AND eliminado_en IS NULL
        )
        BEGIN
            SET @CodigoResultado = 1;
            SET @MensajeResultado = N'No existe inventario para ese producto en esa sucursal.';
            RETURN;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.INVENTARIO
            WHERE id_producto = @idProducto
              AND id_sucursal = @idSucursal
              AND eliminado_en IS NULL
              AND stock + @cantidad < 0
        )
        BEGIN
            SET @CodigoResultado = 4;
            SET @MensajeResultado = N'El ajuste dejaría el stock en un valor negativo.';
            RETURN;
        END;

        UPDATE dbo.INVENTARIO
        SET stock = stock + @cantidad
        WHERE id_producto = @idProducto
          AND id_sucursal = @idSucursal
          AND eliminado_en IS NULL;

        SET @CodigoResultado = 0;
        SET @MensajeResultado = N'Stock ajustado correctamente.';

    END TRY
    BEGIN CATCH
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
        c.nombre + N' ' + c.apellido AS cliente,
        s.nombre AS sucursal
    FROM dbo.VENTA AS v
    INNER JOIN dbo.CLIENTE AS c
        ON c.id_cliente = v.id_cliente
    INNER JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = v.id_sucursal
    WHERE v.eliminado_en IS NULL
      AND v.id_usuario = @idUsuario
      AND v.fecha_hora >= @desde
      AND v.fecha_hora < DATEADD(DAY, 1, @hasta)
    ORDER BY v.fecha_hora DESC;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Venta_ObtenerDetalle
    @idVenta INT
AS
BEGIN
    SET NOCOUNT ON;

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
    WHERE dv.id_venta = @idVenta
      AND dv.eliminado_en IS NULL
    ORDER BY dv.id_detalle_venta;

    SELECT
        pg.id_pago,
        mp.nombre AS metodo_pago,
        pg.monto
    FROM dbo.PAGO AS pg
    INNER JOIN dbo.METODO_PAGO AS mp
        ON mp.id_metodo_pago = pg.id_metodo_pago
    WHERE pg.id_venta = @idVenta
      AND pg.eliminado_en IS NULL
    ORDER BY pg.id_pago;
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
    @idSucursal INT = NULL
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
-- FIN DEL SCRIPT
--
-- NOTA IMPORTANTE SOBRE VENTA:
-- La registración completa de una venta todavía debe resolverse
-- como UNA TRANSACCIÓN ATÓMICA que incluya:
--
-- 1. VENTA
-- 2. DETALLE_VENTA
-- 3. PAGO
-- 4. descuento de INVENTARIO en la SUCURSAL de la venta
--
-- También deberá devolver:
-- @IdGenerado
-- @CodigoResultado
-- @MensajeResultado
--
-- Código 4 quedará reservado para STOCK INSUFICIENTE.
--
-- Para implementarlo correctamente primero se debe definir cómo
-- Capa_Logica enviará múltiples productos y múltiples pagos
-- (por ejemplo, usando Table-Valued Parameters).
-- ============================================================
