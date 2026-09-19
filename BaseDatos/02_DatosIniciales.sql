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
   - Definir el alcance de sucursal de los perfiles iniciales.
   ========================================================= */

USE SistemaGestion;
GO


/* =========================================================
   PERFILES
   ========================================================= */

UPDATE p
SET
    p.descripcion = v.descripcion,
    p.alcance_global = v.alcance_global,
    p.eliminado_en = NULL
FROM dbo.PERFIL AS p
INNER JOIN
(
    VALUES
        (N'Administrador', N'Acceso general a la administración del sistema', CAST(1 AS BIT)),
        (N'Gerente',       N'Acceso a funciones de gestión y reportes',       CAST(0 AS BIT)),
        (N'Vendedor',      N'Acceso principalmente a ventas y atención de clientes', CAST(0 AS BIT))
) AS v(nombre, descripcion, alcance_global)
    ON v.nombre = p.nombre;
GO

INSERT INTO dbo.PERFIL
(
    nombre,
    descripcion,
    alcance_global
)
SELECT
    v.nombre,
    v.descripcion,
    v.alcance_global
FROM
(
    VALUES
        (N'Administrador', N'Acceso general a la administración del sistema', CAST(1 AS BIT)),
        (N'Gerente',       N'Acceso a funciones de gestión y reportes',       CAST(0 AS BIT)),
        (N'Vendedor',      N'Acceso principalmente a ventas y atención de clientes', CAST(0 AS BIT))
) AS v(nombre, descripcion, alcance_global)
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

        (N'SUCURSALES_VER',          N'Ver sucursales',                  N'Permite consultar sucursales y sus usuarios por perfil'),
        (N'SUCURSALES_ALTA',         N'Alta de sucursales',              N'Permite registrar nuevas sucursales'),
        (N'SUCURSALES_MODIFICAR',    N'Modificar sucursales',            N'Permite modificar sucursales existentes'),
        (N'SUCURSALES_BAJA',         N'Baja de sucursales',              N'Permite dar de baja o reactivar sucursales'),

        (N'REPORTES_ADMINISTRADOR',  N'Reportes de administrador',       N'Permite acceder a los reportes del administrador'),
        (N'REPORTES_GERENTE',        N'Reportes de gerente',             N'Permite acceder a los reportes del gerente'),
        (N'REPORTES_VENDEDOR',       N'Reportes de vendedor',            N'Permite acceder a los reportes del vendedor'),
        (N'REPORTES_VER',            N'Acceder a reportes',              N'Permite acceder al módulo Reportes'),
        (N'REPORTES_VENTAS',         N'Consultar ventas',                N'Permite consultar indicadores de ventas'),
        (N'REPORTES_RECAUDACION',    N'Consultar recaudación',           N'Permite consultar recaudación y descuentos'),
        (N'REPORTES_PRODUCTOS',      N'Consultar productos vendidos',    N'Permite consultar rankings de productos'),
        (N'REPORTES_STOCK',          N'Consultar stock',                 N'Permite consultar alertas de stock'),
        (N'REPORTES_RENDIMIENTO_VENDEDORES', N'Consultar rendimiento de vendedores', N'Permite consultar rendimiento por vendedor'),
        (N'REPORTES_DETALLE_VENTAS', N'Consultar detalle de ventas',     N'Permite consultar el listado detallado de ventas'),
        (N'REPORTES_EXPORTAR',       N'Exportar reportes',               N'Permite exportar información de reportes'),
        (N'REPORTES_ALCANCE_PROPIO', N'Alcance propio de reportes',      N'Restringe los reportes a las operaciones propias'),
        (N'REPORTES_ALCANCE_SUCURSAL', N'Alcance sucursal de reportes',  N'Restringe los reportes a la sucursal asignada'),
        (N'REPORTES_ALCANCE_GLOBAL', N'Alcance global de reportes',      N'Permite consultar todas las sucursales o una seleccionada'),
        (N'AVISOS_VER',              N'Ver avisos internos',             N'Permite recibir y visualizar avisos internos'),
        (N'AVISOS_PUBLICAR',         N'Publicar avisos internos',        N'Permite publicar avisos según el alcance del usuario')
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

        (N'SUCURSALES_VER',          N'Ver sucursales',                  N'Permite consultar sucursales y sus usuarios por perfil'),
        (N'SUCURSALES_ALTA',         N'Alta de sucursales',              N'Permite registrar nuevas sucursales'),
        (N'SUCURSALES_MODIFICAR',    N'Modificar sucursales',            N'Permite modificar sucursales existentes'),
        (N'SUCURSALES_BAJA',         N'Baja de sucursales',              N'Permite dar de baja o reactivar sucursales'),

        (N'REPORTES_ADMINISTRADOR',  N'Reportes de administrador',       N'Permite acceder a los reportes del administrador'),
        (N'REPORTES_GERENTE',        N'Reportes de gerente',             N'Permite acceder a los reportes del gerente'),
        (N'REPORTES_VENDEDOR',       N'Reportes de vendedor',            N'Permite acceder a los reportes del vendedor'),
        (N'REPORTES_VER',            N'Acceder a reportes',              N'Permite acceder al módulo Reportes'),
        (N'REPORTES_VENTAS',         N'Consultar ventas',                N'Permite consultar indicadores de ventas'),
        (N'REPORTES_RECAUDACION',    N'Consultar recaudación',           N'Permite consultar recaudación y descuentos'),
        (N'REPORTES_PRODUCTOS',      N'Consultar productos vendidos',    N'Permite consultar rankings de productos'),
        (N'REPORTES_STOCK',          N'Consultar stock',                 N'Permite consultar alertas de stock'),
        (N'REPORTES_RENDIMIENTO_VENDEDORES', N'Consultar rendimiento de vendedores', N'Permite consultar rendimiento por vendedor'),
        (N'REPORTES_DETALLE_VENTAS', N'Consultar detalle de ventas',     N'Permite consultar el listado detallado de ventas'),
        (N'REPORTES_EXPORTAR',       N'Exportar reportes',               N'Permite exportar información de reportes'),
        (N'REPORTES_ALCANCE_PROPIO', N'Alcance propio de reportes',      N'Restringe los reportes a las operaciones propias'),
        (N'REPORTES_ALCANCE_SUCURSAL', N'Alcance sucursal de reportes',  N'Restringe los reportes a la sucursal asignada'),
        (N'REPORTES_ALCANCE_GLOBAL', N'Alcance global de reportes',      N'Permite consultar todas las sucursales o una seleccionada'),
        (N'AVISOS_VER',              N'Ver avisos internos',             N'Permite recibir y visualizar avisos internos'),
        (N'AVISOS_PUBLICAR',         N'Publicar avisos internos',        N'Permite publicar avisos según el alcance del usuario')
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

      N'SUCURSALES_VER',
      N'SUCURSALES_ALTA',
      N'SUCURSALES_MODIFICAR',
      N'SUCURSALES_BAJA',

      N'REPORTES_ADMINISTRADOR',
      N'REPORTES_VER',
      N'REPORTES_VENTAS',
      N'REPORTES_RECAUDACION',
      N'REPORTES_PRODUCTOS',
      N'REPORTES_STOCK',
      N'REPORTES_RENDIMIENTO_VENDEDORES',
      N'REPORTES_DETALLE_VENTAS',
      N'REPORTES_EXPORTAR',
      N'REPORTES_ALCANCE_GLOBAL',
      N'AVISOS_VER',
      N'AVISOS_PUBLICAR'
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

      N'REPORTES_GERENTE',
      N'REPORTES_VER',
      N'REPORTES_VENTAS',
      N'REPORTES_RECAUDACION',
      N'REPORTES_PRODUCTOS',
      N'REPORTES_STOCK',
      N'REPORTES_RENDIMIENTO_VENDEDORES',
      N'REPORTES_DETALLE_VENTAS',
      N'REPORTES_EXPORTAR',
      N'REPORTES_ALCANCE_SUCURSAL',
      N'AVISOS_VER',
      N'AVISOS_PUBLICAR'
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

      N'REPORTES_VENDEDOR',
      N'REPORTES_VER',
      N'REPORTES_VENTAS',
      N'REPORTES_RECAUDACION',
      N'REPORTES_PRODUCTOS',
      N'REPORTES_DETALLE_VENTAS',
      N'REPORTES_ALCANCE_PROPIO'
      ,N'AVISOS_VER'
  )
  AND NOT EXISTS
  (
      SELECT 1
      FROM dbo.PERFIL_FUNCIONALIDAD AS pf
      WHERE pf.id_perfil = p.id_perfil
        AND pf.id_funcionalidad = f.id_funcionalidad
  );
GO


/* Relaciones iniciales de avisos. El código de aplicación nunca depende de estos nombres. */
INSERT INTO dbo.PERFIL_AVISO_DESTINO (id_perfil_emisor, id_perfil_destino)
SELECT emisor.id_perfil, destino.id_perfil
FROM
(
    VALUES (N'Administrador', N'Gerente'), (N'Gerente', N'Vendedor')
) AS relaciones(nombre_emisor, nombre_destino)
INNER JOIN dbo.PERFIL AS emisor
    ON emisor.nombre = relaciones.nombre_emisor AND emisor.eliminado_en IS NULL
INNER JOIN dbo.PERFIL AS destino
    ON destino.nombre = relaciones.nombre_destino AND destino.eliminado_en IS NULL
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.PERFIL_AVISO_DESTINO AS pad
    WHERE pad.id_perfil_emisor = emisor.id_perfil
      AND pad.id_perfil_destino = destino.id_perfil
);
GO


/* =========================================================
   USUARIO ADMINISTRADOR INICIAL

   Permite ingresar por primera vez y administrar perfiles,
   sucursales, usuarios y catálogos. El hash es PBKDF2 válido;
   sólo se crea o reactiva por el nombre de usuario reservado.
   ========================================================= */
IF EXISTS
(
    SELECT 1
    FROM dbo.USUARIO
    WHERE nombre_usuario = N'admin'
)
BEGIN
    UPDATE u
    SET
        u.id_perfil = p.id_perfil,
        u.id_sucursal = NULL,
        u.eliminado_en = NULL
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.nombre = N'Administrador'
       AND p.alcance_global = 1
       AND p.eliminado_en IS NULL
    WHERE u.nombre_usuario = N'admin';
END
ELSE
BEGIN
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
        NULL,
        N'Administrador',
        N'Inicial',
        N'90000000',
        N'admin',
        N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        N'admin@sistemagestion.local'
    FROM dbo.PERFIL AS p
    WHERE p.nombre = N'Administrador'
      AND p.alcance_global = 1
      AND p.eliminado_en IS NULL
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.USUARIO AS u
          WHERE u.dni = N'90000000'
             OR u.correo = N'admin@sistemagestion.local'
      );
END;
GO


/* =========================================================
   CATÁLOGO DE PROVINCIAS ARGENTINAS

   Se mantiene idempotente para instalaciones existentes y no
   habilita el alta de provincias desde las vistas.
   ========================================================= */
DECLARE @ProvinciasIniciales TABLE
(
    nombre NVARCHAR(100) NOT NULL PRIMARY KEY
);

INSERT INTO @ProvinciasIniciales (nombre)
VALUES
    (N'Buenos Aires'),
    (N'Catamarca'),
    (N'Chaco'),
    (N'Chubut'),
    (N'Ciudad Autónoma de Buenos Aires'),
    (N'Córdoba'),
    (N'Corrientes'),
    (N'Entre Ríos'),
    (N'Formosa'),
    (N'Jujuy'),
    (N'La Pampa'),
    (N'La Rioja'),
    (N'Mendoza'),
    (N'Misiones'),
    (N'Neuquén'),
    (N'Río Negro'),
    (N'Salta'),
    (N'San Juan'),
    (N'San Luis'),
    (N'Santa Cruz'),
    (N'Santa Fe'),
    (N'Santiago del Estero'),
    (N'Tierra del Fuego'),
    (N'Tucumán');

UPDATE p
SET eliminado_en = NULL
FROM dbo.PROVINCIA AS p
INNER JOIN @ProvinciasIniciales AS pi
    ON UPPER(LTRIM(RTRIM(p.nombre))) = UPPER(pi.nombre);

INSERT INTO dbo.PROVINCIA (nombre)
SELECT pi.nombre
FROM @ProvinciasIniciales AS pi
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.PROVINCIA AS p
    WHERE UPPER(LTRIM(RTRIM(p.nombre))) = UPPER(pi.nombre)
);
GO

/* =========================================================
   FIN DEL SCRIPT
   Puede ejecutarse nuevamente sin duplicar los datos iniciales.
   ========================================================= */
