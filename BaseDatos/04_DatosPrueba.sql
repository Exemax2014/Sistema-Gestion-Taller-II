/* =========================================================
   SISTEMA DE GESTIÓN - TALLER DE PROGRAMACIÓN II
   Script 04 - Datos de prueba para desarrollo
   Versión ampliada: múltiples sucursales y usuarios

   IMPORTANTE:
   Este script crea datos exclusivamente para pruebas
   del equipo de desarrollo.

   No debe utilizarse para una instalación productiva.

   Criterio:
   - Crea/actualiza dos sucursales de prueba.
   - Crea/actualiza un administrador general.
   - Crea/actualiza dos gerentes, uno por sucursal.
   - Crea/actualiza cuatro vendedores, dos por sucursal.
   - Puede ejecutarse varias veces sin duplicar registros.
   ========================================================= */

USE SistemaGestion;
GO


/* =========================================================
   ASEGURAR UBICACIÓN BASE: CORRIENTES
   ========================================================= */

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.PROVINCIA
    WHERE nombre = N'Corrientes'
)
BEGIN
    INSERT INTO dbo.PROVINCIA (nombre)
    VALUES (N'Corrientes');
END;
GO


IF NOT EXISTS
(
    SELECT 1
    FROM dbo.LOCALIDAD AS l
    INNER JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND l.nombre = N'Corrientes'
)
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


/* =========================================================
   DIRECCIONES DE PRUEBA
   ========================================================= */

/* Dirección Sucursal Central */
IF NOT EXISTS
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


/* Dirección Sucursal Norte */
IF NOT EXISTS
(
    SELECT 1
    FROM dbo.DIRECCION AS d
    INNER JOIN dbo.LOCALIDAD AS l
        ON l.id_localidad = d.id_localidad
    INNER JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND l.nombre = N'Corrientes'
      AND d.calle = N'Av. Independencia'
      AND d.altura = N'4200'
)
BEGIN
    INSERT INTO dbo.DIRECCION
    (
        id_localidad,
        calle,
        altura
    )
    SELECT
        l.id_localidad,
        N'Av. Independencia',
        N'4200'
    FROM dbo.LOCALIDAD AS l
    INNER JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND l.nombre = N'Corrientes';
END;
GO


/* =========================================================
   SUCURSALES DE PRUEBA
   ========================================================= */

/* Sucursal Central */
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


/* Sucursal Norte */
IF EXISTS
(
    SELECT 1
    FROM dbo.SUCURSAL
    WHERE nombre = N'Sucursal Norte'
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
          AND dir.calle = N'Av. Independencia'
          AND dir.altura = N'4200'
        ORDER BY dir.id_direccion
    ) AS d
    WHERE s.nombre = N'Sucursal Norte';
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
        N'Sucursal Norte',
        NULL,
        d.id_direccion
    FROM dbo.DIRECCION AS d
    INNER JOIN dbo.LOCALIDAD AS l
        ON l.id_localidad = d.id_localidad
    INNER JOIN dbo.PROVINCIA AS p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND l.nombre = N'Corrientes'
      AND d.calle = N'Av. Independencia'
      AND d.altura = N'4200'
    ORDER BY d.id_direccion;
END;
GO


/* =========================================================
   HASH DE PRUEBA
   =========================================================
   Todos los usuarios usan la misma contraseña de prueba
   que ya venía utilizando el proyecto.
   ========================================================= */


/* =========================================================
   ADMINISTRADOR GENERAL
   =========================================================
   El administrador no pertenece a una sucursal específica.
   Puede trabajar con todas las sucursales del sistema.
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
        u.nombre = N'Administrador',
        u.apellido = N'Sistema',
        u.dni = N'99000000',
        u.contrasena_hash = N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        u.correo = N'admin.desarrollo@local.test',
        u.eliminado_en = NULL
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.nombre = N'Administrador'
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
        N'Sistema',
        N'99000000',
        N'admin',
        N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        N'admin.desarrollo@local.test'
    FROM dbo.PERFIL AS p
    WHERE p.nombre = N'Administrador'
      AND p.eliminado_en IS NULL
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.USUARIO
          WHERE nombre_usuario = N'admin'
             OR dni = N'99000000'
             OR correo = N'admin.desarrollo@local.test'
      );
END;
GO


/* =========================================================
   GERENTE - SUCURSAL CENTRAL
   ========================================================= */

IF EXISTS
(
    SELECT 1
    FROM dbo.USUARIO
    WHERE nombre_usuario = N'gerente_a'
)
BEGIN
    UPDATE u
    SET
        u.id_perfil = p.id_perfil,
        u.id_sucursal = s.id_sucursal,
        u.nombre = N'Gerente',
        u.apellido = N'Central',
        u.dni = N'99000010',
        u.contrasena_hash = N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        u.correo = N'gerente.central@local.test',
        u.eliminado_en = NULL
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.nombre = N'Gerente'
       AND p.eliminado_en IS NULL
    INNER JOIN dbo.SUCURSAL AS s
        ON s.nombre = N'Sucursal Central'
       AND s.eliminado_en IS NULL
    WHERE u.nombre_usuario = N'gerente_a';
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
        s.id_sucursal,
        N'Gerente',
        N'Central',
        N'99000010',
        N'gerente_a',
        N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        N'gerente.central@local.test'
    FROM dbo.PERFIL AS p
    CROSS JOIN dbo.SUCURSAL AS s
    WHERE p.nombre = N'Gerente'
      AND p.eliminado_en IS NULL
      AND s.nombre = N'Sucursal Central'
      AND s.eliminado_en IS NULL
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.USUARIO
          WHERE nombre_usuario = N'gerente_a'
             OR dni = N'99000010'
             OR correo = N'gerente.central@local.test'
      );
END;
GO


/* =========================================================
   GERENTE - SUCURSAL NORTE
   ========================================================= */

IF EXISTS
(
    SELECT 1
    FROM dbo.USUARIO
    WHERE nombre_usuario = N'gerente_b'
)
BEGIN
    UPDATE u
    SET
        u.id_perfil = p.id_perfil,
        u.id_sucursal = s.id_sucursal,
        u.nombre = N'Gerente',
        u.apellido = N'Norte',
        u.dni = N'99000011',
        u.contrasena_hash = N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        u.correo = N'gerente.norte@local.test',
        u.eliminado_en = NULL
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.nombre = N'Gerente'
       AND p.eliminado_en IS NULL
    INNER JOIN dbo.SUCURSAL AS s
        ON s.nombre = N'Sucursal Norte'
       AND s.eliminado_en IS NULL
    WHERE u.nombre_usuario = N'gerente_b';
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
        s.id_sucursal,
        N'Gerente',
        N'Norte',
        N'99000011',
        N'gerente_b',
        N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        N'gerente.norte@local.test'
    FROM dbo.PERFIL AS p
    CROSS JOIN dbo.SUCURSAL AS s
    WHERE p.nombre = N'Gerente'
      AND p.eliminado_en IS NULL
      AND s.nombre = N'Sucursal Norte'
      AND s.eliminado_en IS NULL
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.USUARIO
          WHERE nombre_usuario = N'gerente_b'
             OR dni = N'99000011'
             OR correo = N'gerente.norte@local.test'
      );
END;
GO


/* =========================================================
   VENDEDORES - SUCURSAL CENTRAL
   ========================================================= */

/* Vendedor A1 */
IF EXISTS
(
    SELECT 1
    FROM dbo.USUARIO
    WHERE nombre_usuario = N'vendedor_a1'
)
BEGIN
    UPDATE u
    SET
        u.id_perfil = p.id_perfil,
        u.id_sucursal = s.id_sucursal,
        u.nombre = N'Vendedor',
        u.apellido = N'Central 1',
        u.dni = N'99000020',
        u.contrasena_hash = N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        u.correo = N'vendedor.central1@local.test',
        u.eliminado_en = NULL
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.nombre = N'Vendedor'
       AND p.eliminado_en IS NULL
    INNER JOIN dbo.SUCURSAL AS s
        ON s.nombre = N'Sucursal Central'
       AND s.eliminado_en IS NULL
    WHERE u.nombre_usuario = N'vendedor_a1';
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
        s.id_sucursal,
        N'Vendedor',
        N'Central 1',
        N'99000020',
        N'vendedor_a1',
        N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        N'vendedor.central1@local.test'
    FROM dbo.PERFIL AS p
    CROSS JOIN dbo.SUCURSAL AS s
    WHERE p.nombre = N'Vendedor'
      AND p.eliminado_en IS NULL
      AND s.nombre = N'Sucursal Central'
      AND s.eliminado_en IS NULL
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.USUARIO
          WHERE nombre_usuario = N'vendedor_a1'
             OR dni = N'99000020'
             OR correo = N'vendedor.central1@local.test'
      );
END;
GO


/* Vendedor A2 */
IF EXISTS
(
    SELECT 1
    FROM dbo.USUARIO
    WHERE nombre_usuario = N'vendedor_a2'
)
BEGIN
    UPDATE u
    SET
        u.id_perfil = p.id_perfil,
        u.id_sucursal = s.id_sucursal,
        u.nombre = N'Vendedor',
        u.apellido = N'Central 2',
        u.dni = N'99000021',
        u.contrasena_hash = N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        u.correo = N'vendedor.central2@local.test',
        u.eliminado_en = NULL
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.nombre = N'Vendedor'
       AND p.eliminado_en IS NULL
    INNER JOIN dbo.SUCURSAL AS s
        ON s.nombre = N'Sucursal Central'
       AND s.eliminado_en IS NULL
    WHERE u.nombre_usuario = N'vendedor_a2';
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
        s.id_sucursal,
        N'Vendedor',
        N'Central 2',
        N'99000021',
        N'vendedor_a2',
        N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        N'vendedor.central2@local.test'
    FROM dbo.PERFIL AS p
    CROSS JOIN dbo.SUCURSAL AS s
    WHERE p.nombre = N'Vendedor'
      AND p.eliminado_en IS NULL
      AND s.nombre = N'Sucursal Central'
      AND s.eliminado_en IS NULL
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.USUARIO
          WHERE nombre_usuario = N'vendedor_a2'
             OR dni = N'99000021'
             OR correo = N'vendedor.central2@local.test'
      );
END;
GO


/* =========================================================
   VENDEDORES - SUCURSAL NORTE
   ========================================================= */

/* Vendedor B1 */
IF EXISTS
(
    SELECT 1
    FROM dbo.USUARIO
    WHERE nombre_usuario = N'vendedor_b1'
)
BEGIN
    UPDATE u
    SET
        u.id_perfil = p.id_perfil,
        u.id_sucursal = s.id_sucursal,
        u.nombre = N'Vendedor',
        u.apellido = N'Norte 1',
        u.dni = N'99000022',
        u.contrasena_hash = N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        u.correo = N'vendedor.norte1@local.test',
        u.eliminado_en = NULL
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.nombre = N'Vendedor'
       AND p.eliminado_en IS NULL
    INNER JOIN dbo.SUCURSAL AS s
        ON s.nombre = N'Sucursal Norte'
       AND s.eliminado_en IS NULL
    WHERE u.nombre_usuario = N'vendedor_b1';
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
        s.id_sucursal,
        N'Vendedor',
        N'Norte 1',
        N'99000022',
        N'vendedor_b1',
        N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        N'vendedor.norte1@local.test'
    FROM dbo.PERFIL AS p
    CROSS JOIN dbo.SUCURSAL AS s
    WHERE p.nombre = N'Vendedor'
      AND p.eliminado_en IS NULL
      AND s.nombre = N'Sucursal Norte'
      AND s.eliminado_en IS NULL
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.USUARIO
          WHERE nombre_usuario = N'vendedor_b1'
             OR dni = N'99000022'
             OR correo = N'vendedor.norte1@local.test'
      );
END;
GO


/* Vendedor B2 */
IF EXISTS
(
    SELECT 1
    FROM dbo.USUARIO
    WHERE nombre_usuario = N'vendedor_b2'
)
BEGIN
    UPDATE u
    SET
        u.id_perfil = p.id_perfil,
        u.id_sucursal = s.id_sucursal,
        u.nombre = N'Vendedor',
        u.apellido = N'Norte 2',
        u.dni = N'99000023',
        u.contrasena_hash = N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        u.correo = N'vendedor.norte2@local.test',
        u.eliminado_en = NULL
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p
        ON p.nombre = N'Vendedor'
       AND p.eliminado_en IS NULL
    INNER JOIN dbo.SUCURSAL AS s
        ON s.nombre = N'Sucursal Norte'
       AND s.eliminado_en IS NULL
    WHERE u.nombre_usuario = N'vendedor_b2';
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
        s.id_sucursal,
        N'Vendedor',
        N'Norte 2',
        N'99000023',
        N'vendedor_b2',
        N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',
        N'vendedor.norte2@local.test'
    FROM dbo.PERFIL AS p
    CROSS JOIN dbo.SUCURSAL AS s
    WHERE p.nombre = N'Vendedor'
      AND p.eliminado_en IS NULL
      AND s.nombre = N'Sucursal Norte'
      AND s.eliminado_en IS NULL
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.USUARIO
          WHERE nombre_usuario = N'vendedor_b2'
             OR dni = N'99000023'
             OR correo = N'vendedor.norte2@local.test'
      );
END;
GO


/* =========================================================
   VERIFICACIÓN RÁPIDA
   ========================================================= */

SELECT
    ISNULL(s.nombre, N'Todas las sucursales') AS sucursal,
    p.nombre AS perfil,
    u.nombre_usuario,
    u.nombre,
    u.apellido,
    u.dni,
    u.correo,
    u.eliminado_en
FROM dbo.USUARIO AS u
INNER JOIN dbo.PERFIL AS p
    ON p.id_perfil = u.id_perfil
LEFT JOIN dbo.SUCURSAL AS s
    ON s.id_sucursal = u.id_sucursal
WHERE u.nombre_usuario IN
(
    N'admin',
    N'gerente_a',
    N'gerente_b',
    N'vendedor_a1',
    N'vendedor_a2',
    N'vendedor_b1',
    N'vendedor_b2'
)
ORDER BY
    s.nombre,
    p.nombre,
    u.nombre_usuario;
GO


/* =========================================================
   FIN DEL SCRIPT

   Usuarios de prueba:

   ADMINISTRADOR
   - admin
     Todas las sucursales

   SUCURSAL CENTRAL
   - gerente_a
   - vendedor_a1
   - vendedor_a2

   SUCURSAL NORTE
   - gerente_b
   - vendedor_b1
   - vendedor_b2

   Todos utilizan el mismo hash de contraseña de prueba.
   ========================================================= */
