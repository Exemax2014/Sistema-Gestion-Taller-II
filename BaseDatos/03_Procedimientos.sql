
-- ============================================================
-- PROCEDIMIENTOS ALMACENADOS
-- Sistema Hierro y Forja
-- Base de datos: SistemaGestion
--
-- Este archivo contiene los procedimientos almacenados
-- utilizados por la aplicación.
-- Debe mantenerse sincronizado con los procedimientos
-- existentes en SQL Server.
-- ============================================================

USE SistemaGestion;
GO


-- ============================================================
-- USUARIOS
-- ============================================================


-- ============================================================
-- Procedimiento: sp_Usuario_BuscarPorNombreUsuario
--
-- Descripción:
-- Busca un usuario activo a partir de su nombre de usuario.
-- Devuelve también los datos de su perfil y de la sucursal
-- a la que se encuentra asignado.
--
-- Utilizado por:
-- Capa_Datos -> UsuarioDatos
--
-- Parámetros:
-- @nombreUsuario:
--     Nombre de usuario que se desea buscar.
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Usuario_BuscarPorNombreUsuario
    @nombreUsuario NVARCHAR(100)
AS
BEGIN
    -- Evita que SQL Server envíe mensajes adicionales
    -- indicando la cantidad de filas afectadas.
    SET NOCOUNT ON;

    -- Obtener los datos necesarios para el proceso de login.
    SELECT TOP 1
        u.id_usuario,
        u.id_perfil,
        u.id_sucursal,
        u.nombre,
        u.apellido,
        u.nombre_usuario,
        u.contrasena_hash,
        p.nombre AS perfil,
        s.nombre AS sucursal

    FROM dbo.USUARIO AS u

    -- Obtener el perfil asociado al usuario.
    INNER JOIN dbo.PERFIL AS p
        ON p.id_perfil = u.id_perfil

    -- Obtener la sucursal asociada al usuario.
    INNER JOIN dbo.SUCURSAL AS s
        ON s.id_sucursal = u.id_sucursal

    WHERE
        -- Buscar por el nombre de usuario recibido como parámetro.
        u.nombre_usuario = @nombreUsuario

        -- Solo considerar usuarios que no fueron dados de baja.
        AND u.eliminado_en IS NULL

        -- El perfil también debe encontrarse activo.
        AND p.eliminado_en IS NULL

        -- La sucursal también debe encontrarse activa.
        AND s.eliminado_en IS NULL;
END;
GO



-- ============================================================
-- PERFILES Y FUNCIONALIDADES
-- ============================================================


-- ============================================================
-- Procedimiento: sp_Perfil_ObtenerFuncionalidades
--
-- Descripción:
-- Obtiene los códigos de funcionalidades asignados a un perfil.
--
-- Estos códigos serán utilizados por la aplicación para
-- determinar qué módulos y acciones puede utilizar el usuario.
--
-- Parámetro:
-- @idPerfil:
--     Identificador del perfil cuyas funcionalidades
--     se desean consultar.
--
-- Utilizado por:
-- Capa_Datos -> UsuarioDatos
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_Perfil_ObtenerFuncionalidades
    @idPerfil INT
AS
BEGIN
    -- Evitar mensajes adicionales con cantidad de filas afectadas.
    SET NOCOUNT ON;

    -- Obtener únicamente funcionalidades activas pertenecientes
    -- a un perfil que también se encuentre activo.
    SELECT
        f.codigo

    FROM dbo.PERFIL_FUNCIONALIDAD AS pf

    INNER JOIN dbo.PERFIL AS p
        ON p.id_perfil = pf.id_perfil

    INNER JOIN dbo.FUNCIONALIDAD AS f
        ON f.id_funcionalidad = pf.id_funcionalidad

    WHERE
        pf.id_perfil = @idPerfil
        AND p.eliminado_en IS NULL
        AND f.eliminado_en IS NULL

    ORDER BY
        f.codigo;
END;
GO

-- ============================================================
-- AGREGAR A: 03_Procedimientos.sql
-- Sección sugerida: antes de "-- CLIENTES" (porque Cliente
-- depende de Dirección) o en una sección nueva "UBICACIONES".
--
-- Estos procedimientos faltaban para poder completar el alta
-- y modificación de una dirección desde Clientes (y a futuro
-- Usuarios, que también usa id_direccion).
-- ============================================================

USE SistemaGestion;
GO


-- ============================================================
-- UBICACIONES (PROVINCIA / LOCALIDAD / DIRECCION)
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
-- OPCIONAL: permisos de Clientes (FUNCIONALIDAD + PERFIL_FUNCIONALIDAD)
--
-- Solo asigna las 4 funcionalidades al Administrador.
-- DECISIÓN ABIERTA (según AGENTS.md): falta definir el alcance
-- final de Gerente y Vendedor sobre Clientes. No los asigno acá
-- para no asumir esa decisión por ustedes.
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM dbo.FUNCIONALIDAD WHERE codigo = N'CLIENTES_VER')
BEGIN
    INSERT INTO dbo.FUNCIONALIDAD (codigo, nombre, descripcion)
    VALUES (N'CLIENTES_VER', N'Ver clientes', N'Permite ver el listado y detalle de clientes.');
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.FUNCIONALIDAD WHERE codigo = N'CLIENTES_ALTA')
BEGIN
    INSERT INTO dbo.FUNCIONALIDAD (codigo, nombre, descripcion)
    VALUES (N'CLIENTES_ALTA', N'Alta de clientes', N'Permite registrar nuevos clientes.');
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.FUNCIONALIDAD WHERE codigo = N'CLIENTES_MODIFICAR')
BEGIN
    INSERT INTO dbo.FUNCIONALIDAD (codigo, nombre, descripcion)
    VALUES (N'CLIENTES_MODIFICAR', N'Modificar clientes', N'Permite modificar clientes existentes.');
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.FUNCIONALIDAD WHERE codigo = N'CLIENTES_BAJA')
BEGIN
    INSERT INTO dbo.FUNCIONALIDAD (codigo, nombre, descripcion)
    VALUES (N'CLIENTES_BAJA', N'Baja de clientes', N'Permite dar de baja clientes.');
END;
GO

INSERT INTO dbo.PERFIL_FUNCIONALIDAD (id_perfil, id_funcionalidad)
SELECT p.id_perfil, f.id_funcionalidad
FROM dbo.PERFIL AS p
CROSS JOIN dbo.FUNCIONALIDAD AS f
WHERE p.nombre = N'Administrador'
  AND f.codigo IN (N'CLIENTES_VER', N'CLIENTES_ALTA', N'CLIENTES_MODIFICAR', N'CLIENTES_BAJA')
  AND NOT EXISTS (
      SELECT 1 FROM dbo.PERFIL_FUNCIONALIDAD AS pf
      WHERE pf.id_perfil = p.id_perfil AND pf.id_funcionalidad = f.id_funcionalidad
  );
GO


-- ============================================================
-- CLIENTES
-- ============================================================

-- ============================================================
-- Procedimiento: sp_Cliente_Buscar
--
-- Descripción:
-- Busca clientes activos cuyo nombre, apellido o documento
-- coincidan parcialmente con el texto recibido.
--
-- Utilizado por:
-- Capa_Datos -> ClienteDatos
--
-- Parámetros:
-- @texto:
--     Texto a buscar dentro de nombre, apellido o documento.
-- ============================================================
CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Buscar
    @texto NVARCHAR(100)
AS
BEGIN
    -- Evita que SQL Server envíe mensajes adicionales
    -- indicando la cantidad de filas afectadas.
    SET NOCOUNT ON;

    -- Buscar clientes activos que coincidan parcialmente
    -- con el texto recibido en nombre, apellido o documento.
    SELECT TOP 20
        c.id_cliente,
        c.nombre,
        c.apellido,
        c.documento,
        c.correo,
        c.telefono
    FROM dbo.CLIENTE AS c
    WHERE
        c.eliminado_en IS NULL
        AND (
            c.nombre LIKE '%' + @texto + '%'
            OR c.apellido LIKE '%' + @texto + '%'
            OR c.documento LIKE '%' + @texto + '%'
        )
    ORDER BY
        c.apellido,
        c.nombre;
END;
GO


-- ============================================================
-- PRODUCTOS
-- ============================================================

-- ============================================================
-- Procedimiento: sp_Producto_Buscar
--
-- Descripción:
-- Busca productos activos cuyo código de barra o nombre
-- coincidan parcialmente con el texto recibido, junto con
-- el stock disponible en la sucursal indicada.
--
-- Utilizado por:
-- Capa_Datos -> ProductoDatos
--
-- Parámetros:
-- @texto:
--     Texto a buscar dentro de código de barra o nombre.
-- @idSucursal:
--     Sucursal sobre la que se desea consultar el stock.
-- ============================================================
CREATE OR ALTER PROCEDURE dbo.sp_Producto_Buscar
    @texto NVARCHAR(100),
    @idSucursal INT
AS
BEGIN
    -- Evita que SQL Server envíe mensajes adicionales
    -- indicando la cantidad de filas afectadas.
    SET NOCOUNT ON;

    -- Buscar productos activos que coincidan parcialmente
    -- con el texto recibido, junto con su stock en la
    -- sucursal indicada.
    SELECT TOP 20
        p.id_producto,
        p.codigo_barra,
        p.nombre,
        p.descripcion,
        p.precio_venta,
        ISNULL(i.stock, 0) AS stock
    FROM dbo.PRODUCTO AS p
    LEFT JOIN dbo.INVENTARIO AS i
        ON i.id_producto = p.id_producto
        AND i.id_sucursal = @idSucursal
        AND i.eliminado_en IS NULL
    WHERE
        p.eliminado_en IS NULL
        AND (
            p.codigo_barra LIKE '%' + @texto + '%'
            OR p.nombre LIKE '%' + @texto + '%'
        )
    ORDER BY
        p.nombre;
END;
GO

-- ========================================================
-- CLIENTES
-- ========================================================

-- ========================================================
-- Procedimiento: sp_Cliente_Listar
--
-- Descripción:
-- Devuelve todos los clientes activos (para la grilla del
-- módulo de Clientes).
-- ========================================================
CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_cliente,
        nombre,
        apellido,
        documento,
        correo,
        telefono
    FROM dbo.CLIENTE
    WHERE eliminado_en IS NULL
    ORDER BY apellido, nombre;
END
GO

-- ========================================================
-- Procedimiento: sp_Cliente_Alta
--
-- Descripción:
-- Inserta un nuevo cliente. Devuelve el id generado.
-- ========================================================
CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Alta
    @nombre NVARCHAR(100),
    @apellido NVARCHAR(100),
    @documento NVARCHAR(20),
    @correo NVARCHAR(150) = NULL,
    @telefono NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.CLIENTE (nombre, apellido, documento, correo, telefono)
    VALUES (@nombre, @apellido, @documento, @correo, @telefono);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS id_cliente;
END
GO

-- ========================================================
-- Procedimiento: sp_Cliente_Baja
--
-- Descripción:
-- Baja lógica del cliente. Nunca se hace borrado físico.
-- ========================================================
CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Baja
    @id_cliente INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.CLIENTE
    SET eliminado_en = SYSDATETIME()
    WHERE id_cliente = @id_cliente
      AND eliminado_en IS NULL;
END
GO


--- comentario de prueba