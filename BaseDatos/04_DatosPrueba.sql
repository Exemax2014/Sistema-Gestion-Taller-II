/* =========================================================
   SISTEMA DE GESTIÓN - TALLER DE PROGRAMACIÓN II
   Script 04 - Datos de prueba para desarrollo

   IMPORTANTE:
   Este script crea usuarios exclusivamente para pruebas
   del equipo de desarrollo.

   No debe utilizarse para una instalación productiva.
   ========================================================= */

USE SistemaGestion;
GO


/* =========================================================
   USUARIO ADMINISTRADOR DE PRUEBA
   ========================================================= */

INSERT INTO dbo.USUARIO
(
    id_perfil,
    id_sucursal,
    nombre,
    apellido,
    dni,
    nombre_usuario,
    contrasena_hash,
    correo
)
SELECT
    p.id_perfil,
    s.id_sucursal,
    'Administrador',
    'Sistema',
    '99000000',
    'admin',
    '100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
    'admin.desarrollo@local.test'

FROM dbo.PERFIL AS p
CROSS JOIN dbo.SUCURSAL AS s

WHERE
    p.nombre = 'Administrador'
    AND s.nombre = 'Sucursal Central'
    AND NOT EXISTS
    (
        SELECT 1
        FROM dbo.USUARIO
        WHERE nombre_usuario = 'admin'
    );
GO


/* =========================================================
   USUARIO VENDEDOR DE PRUEBA

   Permite comprobar visualmente que FormPrincipal:
   - habilita módulos permitidos;
   - mantiene visibles los módulos sin permiso;
   - deshabilita y muestra en gris dichos módulos.
   ========================================================= */

INSERT INTO dbo.USUARIO
(
    id_perfil,
    id_sucursal,
    nombre,
    apellido,
    dni,
    nombre_usuario,
    contrasena_hash,
    correo
)
SELECT
    p.id_perfil,
    s.id_sucursal,
    'Vendedor',
    'Prueba',
    '99000001',
    'vendedor',
    '100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
    'vendedor.prueba@local.test'

FROM dbo.PERFIL AS p
CROSS JOIN dbo.SUCURSAL AS s

WHERE
    p.nombre = 'Vendedor'
    AND s.nombre = 'Sucursal Central'
    AND NOT EXISTS
    (
        SELECT 1
        FROM dbo.USUARIO
        WHERE nombre_usuario = 'vendedor'
    );
GO

/* =========================================================
   USUARIO GERENTE DE PRUEBA
   ========================================================= */

INSERT INTO dbo.USUARIO
(
    id_perfil,
    id_sucursal,
    nombre,
    apellido,
    dni,
    nombre_usuario,
    contrasena_hash,
    correo
)
SELECT
    p.id_perfil,
    s.id_sucursal,
    'Gerente',
    'Prueba',
    '99000002',
    'gerente',
    '100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
    'gerente.prueba@local.test'

FROM dbo.PERFIL AS p
CROSS JOIN dbo.SUCURSAL AS s

WHERE
    p.nombre = 'Gerente'
    AND s.nombre = 'Sucursal Central'
    AND NOT EXISTS
    (
        SELECT 1
        FROM dbo.USUARIO
        WHERE nombre_usuario = 'gerente'
    );
GO
