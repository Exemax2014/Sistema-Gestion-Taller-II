/* =========================================================
   SISTEMA DE GESTIÓN - TALLER DE PROGRAMACIÓN II
   Script 02 - Datos Iniciales
   Versión idempotente / actualizable

   Objetivo:
   - Insertar los datos iniciales solamente si no existen.
   - Actualizar nombre/descripción de registros existentes.
   - Reactivar registros iniciales que hayan sido dados de baja.
   - Evitar duplicados al ejecutar el script varias veces.
   - Mantener las asignaciones de permisos ya existentes.
   ========================================================= */

USE SistemaGestion;
GO


/* =========================================================
   PERFILES
   ========================================================= */

UPDATE p
SET
    p.descripcion = v.descripcion,
    p.eliminado_en = NULL
FROM dbo.PERFIL AS p
INNER JOIN
(
    VALUES
        (N'Administrador', N'Acceso general a la administración del sistema'),
        (N'Gerente',       N'Acceso a funciones de gestión y reportes'),
        (N'Vendedor',      N'Acceso principalmente a ventas y atención de clientes')
) AS v(nombre, descripcion)
    ON v.nombre = p.nombre;
GO

INSERT INTO dbo.PERFIL
(
    nombre,
    descripcion
)
SELECT
    v.nombre,
    v.descripcion
FROM
(
    VALUES
        (N'Administrador', N'Acceso general a la administración del sistema'),
        (N'Gerente',       N'Acceso a funciones de gestión y reportes'),
        (N'Vendedor',      N'Acceso principalmente a ventas y atención de clientes')
) AS v(nombre, descripcion)
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.PERFIL AS p
    WHERE p.nombre = v.nombre
);
GO


/* =========================================================
   FUNCIONALIDADES
   ========================================================= */

UPDATE f
SET
    f.nombre = v.nombre,
    f.descripcion = v.descripcion,
    f.eliminado_en = NULL
FROM dbo.FUNCIONALIDAD AS f
INNER JOIN
(
    VALUES
        (N'USUARIOS_VER',            N'Ver usuarios',                    N'Permite consultar usuarios del sistema'),
        (N'USUARIOS_ALTA',           N'Alta de usuarios',                N'Permite registrar nuevos usuarios'),
        (N'USUARIOS_BAJA',           N'Baja de usuarios',                N'Permite realizar la baja lógica de usuarios'),
        (N'USUARIOS_MODIFICAR',      N'Modificar usuarios',              N'Permite modificar datos de usuarios'),

        (N'PERMISOS_GESTIONAR',      N'Gestionar perfiles y permisos',   N'Permite asignar funcionalidades a los perfiles'),

        (N'BACKUP_REALIZAR',         N'Realizar backup',                 N'Permite generar copias de seguridad de la base de datos'),

        (N'VENTAS_VER',              N'Ver ventas',                      N'Permite consultar ventas realizadas'),
        (N'VENTAS_REALIZAR',         N'Realizar ventas',                 N'Permite registrar nuevas ventas'),

        (N'CLIENTES_VER',            N'Ver clientes',                    N'Permite consultar clientes'),
        (N'CLIENTES_ALTA',           N'Alta de clientes',                N'Permite registrar nuevos clientes'),
        (N'CLIENTES_BAJA',           N'Baja de clientes',                N'Permite realizar la baja lógica de clientes'),
        (N'CLIENTES_MODIFICAR',      N'Modificar clientes',              N'Permite modificar datos de clientes'),

        (N'PRODUCTOS_VER',           N'Ver productos',                   N'Permite consultar productos'),
        (N'PRODUCTOS_ALTA',          N'Alta de productos',               N'Permite registrar nuevos productos'),
        (N'PRODUCTOS_BAJA',          N'Baja de productos',               N'Permite realizar la baja lógica de productos'),
        (N'PRODUCTOS_MODIFICAR',     N'Modificar productos',             N'Permite modificar productos'),

        (N'REPORTES_ADMINISTRADOR',  N'Reportes de administrador',       N'Permite acceder a los reportes del administrador'),
        (N'REPORTES_GERENTE',        N'Reportes de gerente',             N'Permite acceder a los reportes del gerente'),
        (N'REPORTES_VENDEDOR',       N'Reportes de vendedor',            N'Permite acceder a los reportes del vendedor')
) AS v(codigo, nombre, descripcion)
    ON v.codigo = f.codigo;
GO

INSERT INTO dbo.FUNCIONALIDAD
(
    codigo,
    nombre,
    descripcion
)
SELECT
    v.codigo,
    v.nombre,
    v.descripcion
FROM
(
    VALUES
        (N'USUARIOS_VER',            N'Ver usuarios',                    N'Permite consultar usuarios del sistema'),
        (N'USUARIOS_ALTA',           N'Alta de usuarios',                N'Permite registrar nuevos usuarios'),
        (N'USUARIOS_BAJA',           N'Baja de usuarios',                N'Permite realizar la baja lógica de usuarios'),
        (N'USUARIOS_MODIFICAR',      N'Modificar usuarios',              N'Permite modificar datos de usuarios'),

        (N'PERMISOS_GESTIONAR',      N'Gestionar perfiles y permisos',   N'Permite asignar funcionalidades a los perfiles'),

        (N'BACKUP_REALIZAR',         N'Realizar backup',                 N'Permite generar copias de seguridad de la base de datos'),

        (N'VENTAS_VER',              N'Ver ventas',                      N'Permite consultar ventas realizadas'),
        (N'VENTAS_REALIZAR',         N'Realizar ventas',                 N'Permite registrar nuevas ventas'),

        (N'CLIENTES_VER',            N'Ver clientes',                    N'Permite consultar clientes'),
        (N'CLIENTES_ALTA',           N'Alta de clientes',                N'Permite registrar nuevos clientes'),
        (N'CLIENTES_BAJA',           N'Baja de clientes',                N'Permite realizar la baja lógica de clientes'),
        (N'CLIENTES_MODIFICAR',      N'Modificar clientes',              N'Permite modificar datos de clientes'),

        (N'PRODUCTOS_VER',           N'Ver productos',                   N'Permite consultar productos'),
        (N'PRODUCTOS_ALTA',          N'Alta de productos',               N'Permite registrar nuevos productos'),
        (N'PRODUCTOS_BAJA',          N'Baja de productos',               N'Permite realizar la baja lógica de productos'),
        (N'PRODUCTOS_MODIFICAR',     N'Modificar productos',             N'Permite modificar productos'),

        (N'REPORTES_ADMINISTRADOR',  N'Reportes de administrador',       N'Permite acceder a los reportes del administrador'),
        (N'REPORTES_GERENTE',        N'Reportes de gerente',             N'Permite acceder a los reportes del gerente'),
        (N'REPORTES_VENDEDOR',       N'Reportes de vendedor',            N'Permite acceder a los reportes del vendedor')
) AS v(codigo, nombre, descripcion)
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.FUNCIONALIDAD AS f
    WHERE f.codigo = v.codigo
);
GO


/* =========================================================
   MÉTODOS DE PAGO
   ========================================================= */

UPDATE mp
SET
    mp.descripcion = v.descripcion,
    mp.eliminado_en = NULL
FROM dbo.METODO_PAGO AS mp
INNER JOIN
(
    VALUES
        (N'Efectivo',       N'Pago realizado en efectivo'),
        (N'Débito',         N'Pago realizado con tarjeta de débito'),
        (N'Crédito',        N'Pago realizado con tarjeta de crédito'),
        (N'Transferencia',  N'Pago realizado mediante transferencia bancaria')
) AS v(nombre, descripcion)
    ON v.nombre = mp.nombre;
GO

INSERT INTO dbo.METODO_PAGO
(
    nombre,
    descripcion
)
SELECT
    v.nombre,
    v.descripcion
FROM
(
    VALUES
        (N'Efectivo',       N'Pago realizado en efectivo'),
        (N'Débito',         N'Pago realizado con tarjeta de débito'),
        (N'Crédito',        N'Pago realizado con tarjeta de crédito'),
        (N'Transferencia',  N'Pago realizado mediante transferencia bancaria')
) AS v(nombre, descripcion)
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.METODO_PAGO AS mp
    WHERE mp.nombre = v.nombre
);
GO


/* =========================================================
   UBICACIÓN INICIAL
   ========================================================= */

/* PROVINCIA: Corrientes */
IF EXISTS
(
    SELECT 1
    FROM dbo.PROVINCIA
    WHERE nombre = N'Corrientes'
)
BEGIN
    UPDATE dbo.PROVINCIA
    SET eliminado_en = NULL
    WHERE nombre = N'Corrientes';
END
ELSE
BEGIN
    INSERT INTO dbo.PROVINCIA (nombre)
    VALUES (N'Corrientes');
END;
GO


/* LOCALIDAD: Corrientes, Corrientes */
IF EXISTS
(
    SELECT 1
    FROM dbo.LOCALIDAD AS l
    INNER JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND l.nombre = N'Corrientes'
)
BEGIN
    UPDATE l
    SET
        l.codigo_postal = N'3400',
        l.eliminado_en = NULL
    FROM dbo.LOCALIDAD AS l
    INNER JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND l.nombre = N'Corrientes';
END
ELSE
BEGIN
    INSERT INTO dbo.LOCALIDAD
    (
        id_provincia,
        nombre,
        codigo_postal
    )
    SELECT
        p.id_provincia,
        N'Corrientes',
        N'3400'
    FROM dbo.PROVINCIA AS p
    WHERE p.nombre = N'Corrientes';
END;
GO


/* DIRECCIÓN inicial de la sucursal */
IF EXISTS
(
    SELECT 1
    FROM dbo.DIRECCION AS d
    INNER JOIN dbo.LOCALIDAD AS l
        ON l.id_localidad = d.id_localidad
    INNER JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND l.nombre = N'Corrientes'
      AND d.calle = N'Junín'
      AND d.altura = N'2064'
)
BEGIN
    UPDATE d
    SET d.eliminado_en = NULL
    FROM dbo.DIRECCION AS d
    INNER JOIN dbo.LOCALIDAD AS l
        ON l.id_localidad = d.id_localidad
    INNER JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND l.nombre = N'Corrientes'
      AND d.calle = N'Junín'
      AND d.altura = N'2064';
END
ELSE
BEGIN
    INSERT INTO dbo.DIRECCION
    (
        id_localidad,
        calle,
        altura
    )
    SELECT
        l.id_localidad,
        N'Junín',
        N'2064'
    FROM dbo.LOCALIDAD AS l
    INNER JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND l.nombre = N'Corrientes';
END;
GO


/* SUCURSAL CENTRAL
   Esta sucursal será utilizada más adelante para cargar el stock inicial
   del catálogo. El stock seguirá almacenado en INVENTARIO por sucursal. */
IF EXISTS
(
    SELECT 1
    FROM dbo.SUCURSAL
    WHERE nombre = N'Sucursal Central'
)
BEGIN
    UPDATE s
    SET
        s.id_direccion = d.id_direccion,
        s.eliminado_en = NULL
    FROM dbo.SUCURSAL AS s
    CROSS APPLY
    (
        SELECT TOP 1
            dir.id_direccion
        FROM dbo.DIRECCION AS dir
        INNER JOIN dbo.LOCALIDAD AS l
            ON l.id_localidad = dir.id_localidad
        INNER JOIN dbo.PROVINCIA AS p
            ON p.id_provincia = l.id_provincia
        WHERE p.nombre = N'Corrientes'
          AND l.nombre = N'Corrientes'
          AND dir.calle = N'Junín'
          AND dir.altura = N'2064'
        ORDER BY dir.id_direccion
    ) AS d
    WHERE s.nombre = N'Sucursal Central';
END
ELSE
BEGIN
    INSERT INTO dbo.SUCURSAL
    (
        nombre,
        telefono,
        id_direccion
    )
    SELECT TOP 1
        N'Sucursal Central',
        NULL,
        d.id_direccion
    FROM dbo.DIRECCION AS d
    INNER JOIN dbo.LOCALIDAD AS l
        ON l.id_localidad = d.id_localidad
    INNER JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND l.nombre = N'Corrientes'
      AND d.calle = N'Junín'
      AND d.altura = N'2064'
    ORDER BY d.id_direccion;
END;
GO


/* =========================================================
   PERMISOS - ADMINISTRADOR
   ========================================================= */

/*
   Se insertan únicamente las relaciones faltantes.
   Si el permiso ya existe, no se duplica.
   No se eliminan permisos extra que hayan sido agregados manualmente.
*/

INSERT INTO dbo.PERFIL_FUNCIONALIDAD
(
    id_perfil,
    id_funcionalidad
)
SELECT
    p.id_perfil,
    f.id_funcionalidad
FROM dbo.PERFIL AS p
CROSS JOIN dbo.FUNCIONALIDAD AS f
WHERE p.nombre = N'Administrador'
  AND f.codigo IN
  (
      N'USUARIOS_VER',
      N'USUARIOS_ALTA',
      N'USUARIOS_BAJA',
      N'USUARIOS_MODIFICAR',

      N'PERMISOS_GESTIONAR',

      N'BACKUP_REALIZAR',

      N'VENTAS_VER',

      N'CLIENTES_VER',
      N'CLIENTES_ALTA',
      N'CLIENTES_BAJA',
      N'CLIENTES_MODIFICAR',

      N'PRODUCTOS_VER',
      N'PRODUCTOS_ALTA',
      N'PRODUCTOS_BAJA',
      N'PRODUCTOS_MODIFICAR',

      N'REPORTES_ADMINISTRADOR'
  )
  AND NOT EXISTS
  (
      SELECT 1
      FROM dbo.PERFIL_FUNCIONALIDAD AS pf
      WHERE pf.id_perfil = p.id_perfil
        AND pf.id_funcionalidad = f.id_funcionalidad
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
FROM dbo.PERFIL AS p
CROSS JOIN dbo.FUNCIONALIDAD AS f
WHERE p.nombre = N'Gerente'
  AND f.codigo IN
  (
      N'CLIENTES_VER',
      N'CLIENTES_ALTA',
      N'CLIENTES_BAJA',

      N'PRODUCTOS_VER',

      N'REPORTES_GERENTE'
  )
  AND NOT EXISTS
  (
      SELECT 1
      FROM dbo.PERFIL_FUNCIONALIDAD AS pf
      WHERE pf.id_perfil = p.id_perfil
        AND pf.id_funcionalidad = f.id_funcionalidad
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
FROM dbo.PERFIL AS p
CROSS JOIN dbo.FUNCIONALIDAD AS f
WHERE p.nombre = N'Vendedor'
  AND f.codigo IN
  (
      N'VENTAS_VER',
      N'VENTAS_REALIZAR',

      N'CLIENTES_VER',
      N'CLIENTES_ALTA',

      N'PRODUCTOS_VER',

      N'REPORTES_VENDEDOR'
  )
  AND NOT EXISTS
  (
      SELECT 1
      FROM dbo.PERFIL_FUNCIONALIDAD AS pf
      WHERE pf.id_perfil = p.id_perfil
        AND pf.id_funcionalidad = f.id_funcionalidad
  );
GO


/* =========================================================
   FIN DEL SCRIPT
   Puede ejecutarse nuevamente sin duplicar los datos iniciales.
   ========================================================= */
