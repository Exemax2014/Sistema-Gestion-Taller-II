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


