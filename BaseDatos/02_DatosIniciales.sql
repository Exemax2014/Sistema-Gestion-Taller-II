/* =========================================================
   SISTEMA DE GESTIÓN - TALLER DE PROGRAMACIÓN II
   Script 02 - Datos Iniciales
   ========================================================= */

USE SistemaGestion;
GO


/* =========================================================
   PERFILES
   ========================================================= */

INSERT INTO dbo.PERFIL (nombre, descripcion)
VALUES
('Administrador', 'Acceso general a la administración del sistema'),
('Gerente', 'Acceso a funciones de gestión y reportes'),
('Vendedor', 'Acceso principalmente a ventas y atención de clientes');
GO


/* =========================================================
   FUNCIONALIDADES
   ========================================================= */

INSERT INTO dbo.FUNCIONALIDAD (codigo, nombre, descripcion)
VALUES
('USUARIOS_VER', 'Ver usuarios', 'Permite consultar usuarios del sistema'),
('USUARIOS_ALTA', 'Alta de usuarios', 'Permite registrar nuevos usuarios'),
('USUARIOS_BAJA', 'Baja de usuarios', 'Permite realizar la baja lógica de usuarios'),
('USUARIOS_MODIFICAR', 'Modificar usuarios', 'Permite modificar datos de usuarios'),

('PERMISOS_GESTIONAR', 'Gestionar perfiles y permisos',
 'Permite asignar funcionalidades a los perfiles'),

('BACKUP_REALIZAR', 'Realizar backup',
 'Permite generar copias de seguridad de la base de datos'),

('VENTAS_VER', 'Ver ventas', 'Permite consultar ventas realizadas'),
('VENTAS_REALIZAR', 'Realizar ventas', 'Permite registrar nuevas ventas'),

('CLIENTES_VER', 'Ver clientes', 'Permite consultar clientes'),
('CLIENTES_ALTA', 'Alta de clientes', 'Permite registrar nuevos clientes'),
('CLIENTES_BAJA', 'Baja de clientes', 'Permite realizar la baja lógica de clientes'),
('CLIENTES_MODIFICAR', 'Modificar clientes', 'Permite modificar datos de clientes'),

('PRODUCTOS_VER', 'Ver productos', 'Permite consultar productos'),
('PRODUCTOS_ALTA', 'Alta de productos', 'Permite registrar nuevos productos'),
('PRODUCTOS_BAJA', 'Baja de productos', 'Permite realizar la baja lógica de productos'),
('PRODUCTOS_MODIFICAR', 'Modificar productos', 'Permite modificar productos'),

('REPORTES_ADMINISTRADOR', 'Reportes de administrador',
 'Permite acceder a los reportes del administrador'),

('REPORTES_GERENTE', 'Reportes de gerente',
 'Permite acceder a los reportes del gerente'),

('REPORTES_VENDEDOR', 'Reportes de vendedor',
 'Permite acceder a los reportes del vendedor');
GO


/* =========================================================
   MÉTODOS DE PAGO
   ========================================================= */

INSERT INTO dbo.METODO_PAGO (nombre, descripcion)
VALUES
('Efectivo', 'Pago realizado en efectivo'),
('Débito', 'Pago realizado con tarjeta de débito'),
('Crédito', 'Pago realizado con tarjeta de crédito'),
('Transferencia', 'Pago realizado mediante transferencia bancaria');
GO


/* =========================================================
   UBICACIÓN INICIAL
   ========================================================= */

INSERT INTO dbo.PROVINCIA (nombre)
VALUES ('Corrientes');
GO


INSERT INTO dbo.LOCALIDAD
(
    id_provincia,
    nombre,
    codigo_postal
)
SELECT
    id_provincia,
    'Corrientes',
    '3400'
FROM dbo.PROVINCIA
WHERE nombre = 'Corrientes';
GO


INSERT INTO dbo.DIRECCION
(
    id_localidad,
    calle,
    altura
)
SELECT
    l.id_localidad,
    'Junín',
    '2064'
FROM dbo.LOCALIDAD l
INNER JOIN dbo.PROVINCIA p
    ON l.id_provincia = p.id_provincia
WHERE l.nombre = 'Corrientes'
AND p.nombre = 'Corrientes';
GO


INSERT INTO dbo.SUCURSAL
(
    nombre,
    telefono,
    id_direccion
)
SELECT
    'Sucursal Central',
    NULL,
    d.id_direccion
FROM dbo.DIRECCION d
INNER JOIN dbo.LOCALIDAD l
    ON d.id_localidad = l.id_localidad
INNER JOIN dbo.PROVINCIA p
    ON l.id_provincia = p.id_provincia
WHERE d.calle = 'Junín'
AND d.altura = '2064'
AND l.nombre = 'Corrientes'
AND p.nombre = 'Corrientes';
GO


/* =========================================================
   PERMISOS - ADMINISTRADOR
   ========================================================= */

INSERT INTO dbo.PERFIL_FUNCIONALIDAD
(
    id_perfil,
    id_funcionalidad
)
SELECT
    p.id_perfil,
    f.id_funcionalidad
FROM dbo.PERFIL p
CROSS JOIN dbo.FUNCIONALIDAD f
WHERE p.nombre = 'Administrador'
AND f.codigo IN
(
    'USUARIOS_VER',
    'USUARIOS_ALTA',
    'USUARIOS_BAJA',
    'USUARIOS_MODIFICAR',

    'PERMISOS_GESTIONAR',

    'BACKUP_REALIZAR',

    'VENTAS_VER',

    'CLIENTES_VER',
    'CLIENTES_ALTA',
    'CLIENTES_BAJA',
    'CLIENTES_MODIFICAR',

    'PRODUCTOS_VER',
    'PRODUCTOS_ALTA',
    'PRODUCTOS_BAJA',
    'PRODUCTOS_MODIFICAR',

    'REPORTES_ADMINISTRADOR'
);
GO


/* =========================================================
   PERMISOS - GERENTE
   ========================================================= */

INSERT INTO dbo.PERFIL_FUNCIONALIDAD
(
    id_perfil,
    id_funcionalidad
)
SELECT
    p.id_perfil,
    f.id_funcionalidad
FROM dbo.PERFIL p
CROSS JOIN dbo.FUNCIONALIDAD f
WHERE p.nombre = 'Gerente'
AND f.codigo IN
(
    'CLIENTES_VER',
    'CLIENTES_ALTA',
    'CLIENTES_BAJA',

    'PRODUCTOS_VER',
    'PRODUCTOS_ALTA',

    'REPORTES_GERENTE'
);
GO


/* =========================================================
   PERMISOS - VENDEDOR
   ========================================================= */

INSERT INTO dbo.PERFIL_FUNCIONALIDAD
(
    id_perfil,
    id_funcionalidad
)
SELECT
    p.id_perfil,
    f.id_funcionalidad
FROM dbo.PERFIL p
CROSS JOIN dbo.FUNCIONALIDAD f
WHERE p.nombre = 'Vendedor'
AND f.codigo IN
(
    'VENTAS_VER',
    'VENTAS_REALIZAR',

    'CLIENTES_VER',
    'CLIENTES_ALTA',

    'REPORTES_VENDEDOR'
);
GO