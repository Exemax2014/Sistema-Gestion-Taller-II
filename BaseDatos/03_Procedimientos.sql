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
