/* =========================================================
   SISTEMA DE GESTION - DATOS DE PRUEBA COMPLETOS
   Requiere: 01_Estructura.sql, 02_DatosIniciales.sql y 03_Procedimientos.sql.

   Escenario de demostracion:
   - 11 usuarios demo (clave comun: Prueba2026!)
   - 18 localidades de Corrientes
   - 180 clientes demo + 1 cliente generico "Consumidor Final"
   - 25 productos
   - 3 sucursales con inventario
   - 1000 ventas distribuidas en los ultimos 180 dias
     * fuerte presencia en los ultimos 30 dias
     * ventas todos los dias de los ultimos 14 dias
     * todos los usuarios demo con VENTAS_REALIZAR reciben ventas
   - pagos simples y combinados
   - 24 avisos internos historicos y recientes
   - 180 movimientos de auditoria, incluidos 15 backups demo
   - 10 combinaciones producto/sucursal en stock bajo

   Flujo recomendado:
      01 -> 02 -> 03 -> 05_ResetBasePruebas -> 04_DatosPrueba

   El propio script elimina antes las ventas/avisos/auditorias DEMO
   que administra, por lo que tambien es seguro reejecutarlo.
   ========================================================= */
USE SistemaGestion;
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Hoy DATE = CONVERT(DATE, SYSDATETIME());
    DECLARE @HashPrueba NVARCHAR(255) =
        N'100000.U2lzdGVtYUdlc3Rpb25UZXN0IQ==.Xpot7xb3msPxNqukQW/1CuKSb2t/VS/n+0pXsHpHekc=';

    /* ========================================================
       PRECONDICIONES
       ======================================================== */
    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.PERFIL
        WHERE alcance_global = 1
          AND eliminado_en IS NULL
    )
        THROW 51001, 'Falta un perfil global activo. Ejecute 02_DatosIniciales.sql.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.SUCURSAL
        WHERE nombre = N'Sucursal Central'
          AND eliminado_en IS NULL
    )
        THROW 51002, 'Falta Sucursal Central. Ejecute 02_DatosIniciales.sql.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM
        (
            VALUES
                (N'VENTAS_REALIZAR'),(N'VENTAS_VER'),
                (N'CLIENTES_VER'),(N'CLIENTES_ALTA'),
                (N'PRODUCTOS_VER'),
                (N'AVISOS_VER'),(N'AVISOS_PUBLICAR'),
                (N'REPORTES_VER'),
                (N'REPORTES_VENTAS'),
                (N'REPORTES_RECAUDACION'),
                (N'REPORTES_PRODUCTOS'),
                (N'REPORTES_STOCK'),
                (N'REPORTES_RENDIMIENTO_VENDEDORES'),
                (N'REPORTES_DETALLE_VENTAS'),
                (N'REPORTES_EXPORTAR'),
                (N'REPORTES_ALCANCE_PROPIO'),
                (N'REPORTES_ALCANCE_SUCURSAL'),
                (N'REPORTES_ALCANCE_GLOBAL'),
                (N'REPORTES_GERENTE'),
                (N'REPORTES_VENDEDOR')
        ) AS r(codigo)
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM dbo.FUNCIONALIDAD f
            WHERE f.codigo = r.codigo
              AND f.eliminado_en IS NULL
        )
    )
        THROW 51003, 'Faltan funcionalidades oficiales. Ejecute 02_DatosIniciales.sql.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM
        (
            VALUES
                (N'Efectivo'),
                (N'Debito'),
                (N'Credito'),
                (N'Transferencia')
        ) AS r(nombre)
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM dbo.METODO_PAGO mp
            WHERE mp.nombre COLLATE Latin1_General_100_CI_AI =
                  r.nombre COLLATE Latin1_General_100_CI_AI
              AND mp.eliminado_en IS NULL
        )
    )
        THROW 51004, 'Faltan metodos de pago activos. Ejecute 02_DatosIniciales.sql.', 1;

    /* ========================================================
       UBICACION - PROVINCIA Y 18 LOCALIDADES
       ======================================================== */
    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.PROVINCIA
        WHERE nombre = N'Corrientes'
    )
        INSERT INTO dbo.PROVINCIA(nombre) VALUES(N'Corrientes');
    ELSE
        UPDATE dbo.PROVINCIA
        SET eliminado_en = NULL
        WHERE nombre = N'Corrientes';

    DECLARE @LocalidadesPrueba TABLE
    (
        numero INT PRIMARY KEY,
        nombre NVARCHAR(100) UNIQUE,
        codigo_postal NVARCHAR(20) NULL
    );

    INSERT INTO @LocalidadesPrueba(numero,nombre,codigo_postal) VALUES
      (1,N'Corrientes',N'3400'),
      (2,N'Riachuelo',N'3414'),
      (3,N'Santa Ana de los Guácaras',N'3401'),
      (4,N'San Luis del Palmar',N'3403'),
      (5,N'Paso de la Patria',N'3409'),
      (6,N'San Cosme',N'3401'),
      (7,N'Itatí',N'3414'),
      (8,N'Empedrado',N'3418'),
      (9,N'Saladas',N'3420'),
      (10,N'Bella Vista',N'3432'),
      (11,N'Goya',N'3450'),
      (12,N'Mercedes',N'3470'),
      (13,N'Curuzú Cuatiá',N'3460'),
      (14,N'Monte Caseros',N'3220'),
      (15,N'Santo Tomé',N'3340'),
      (16,N'Ituzaingó',N'3302'),
      (17,N'Gobernador Virasoro',N'3342'),
      (18,N'Esquina',N'3196');

    UPDATE l
    SET l.codigo_postal = COALESCE(x.codigo_postal,l.codigo_postal),
        l.eliminado_en = NULL
    FROM dbo.LOCALIDAD l
    INNER JOIN dbo.PROVINCIA p
        ON p.id_provincia = l.id_provincia
    INNER JOIN @LocalidadesPrueba x
        ON x.nombre = l.nombre
    WHERE p.nombre = N'Corrientes';

    INSERT INTO dbo.LOCALIDAD(id_provincia,nombre,codigo_postal)
    SELECT p.id_provincia,x.nombre,x.codigo_postal
    FROM @LocalidadesPrueba x
    CROSS JOIN
    (
        SELECT TOP 1 id_provincia
        FROM dbo.PROVINCIA
        WHERE nombre = N'Corrientes'
          AND eliminado_en IS NULL
        ORDER BY id_provincia
    ) p
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.LOCALIDAD l
        WHERE l.id_provincia = p.id_provincia
          AND l.nombre = x.nombre
    );

    /* ========================================================
       SUCURSALES DEMO
       ======================================================== */
    DECLARE @Sucursales TABLE
    (
        nombre NVARCHAR(100) PRIMARY KEY,
        calle NVARCHAR(150),
        altura NVARCHAR(20),
        telefono NVARCHAR(30)
    );

    INSERT INTO @Sucursales VALUES
      (N'Sucursal Prueba Centro',N'9 de Julio',N'100',N'+54 379 4000100'),
      (N'Sucursal Prueba Norte',N'Av. Maipú',N'200',N'+54 379 4000200');

    INSERT INTO dbo.DIRECCION(id_localidad,calle,altura,piso)
    SELECT l.id_localidad,x.calle,x.altura,NULL
    FROM @Sucursales x
    CROSS JOIN
    (
        SELECT TOP 1 l.id_localidad
        FROM dbo.LOCALIDAD l
        INNER JOIN dbo.PROVINCIA p
            ON p.id_provincia = l.id_provincia
        WHERE p.nombre = N'Corrientes'
          AND l.nombre = N'Corrientes'
          AND l.eliminado_en IS NULL
        ORDER BY l.id_localidad
    ) l
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.DIRECCION d
        WHERE d.id_localidad = l.id_localidad
          AND d.calle = x.calle
          AND d.altura = x.altura
          AND d.eliminado_en IS NULL
    );

    UPDATE s
    SET s.id_direccion = d.id_direccion,
        s.telefono = x.telefono,
        s.eliminado_en = NULL
    FROM dbo.SUCURSAL s
    INNER JOIN @Sucursales x
        ON x.nombre = s.nombre
    INNER JOIN dbo.DIRECCION d
        ON d.calle = x.calle
       AND d.altura = x.altura
       AND d.eliminado_en IS NULL;

    INSERT INTO dbo.SUCURSAL(nombre,telefono,id_direccion)
    SELECT x.nombre,x.telefono,d.id_direccion
    FROM @Sucursales x
    INNER JOIN dbo.DIRECCION d
        ON d.calle = x.calle
       AND d.altura = x.altura
       AND d.eliminado_en IS NULL
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.SUCURSAL s
        WHERE s.nombre = x.nombre
    );

    /* ========================================================
       PERFILES Y PERMISOS DEMO
       ======================================================== */
    IF NOT EXISTS (SELECT 1 FROM dbo.PERFIL WHERE nombre = N'Prueba Gerencia')
        INSERT INTO dbo.PERFIL(nombre,descripcion,alcance_global)
        VALUES(N'Prueba Gerencia',N'Perfil de prueba con alcance por sucursal.',0);
    ELSE
        UPDATE dbo.PERFIL
        SET descripcion = N'Perfil de prueba con alcance por sucursal.',
            eliminado_en = NULL,
            alcance_global = 0
        WHERE nombre = N'Prueba Gerencia';

    IF NOT EXISTS (SELECT 1 FROM dbo.PERFIL WHERE nombre = N'Prueba Ventas')
        INSERT INTO dbo.PERFIL(nombre,descripcion,alcance_global)
        VALUES(N'Prueba Ventas',N'Perfil de prueba con alcance propio.',0);
    ELSE
        UPDATE dbo.PERFIL
        SET descripcion = N'Perfil de prueba con alcance propio.',
            eliminado_en = NULL,
            alcance_global = 0
        WHERE nombre = N'Prueba Ventas';

    DECLARE @Permisos TABLE
    (
        perfil NVARCHAR(50),
        codigo NVARCHAR(50),
        PRIMARY KEY(perfil,codigo)
    );

    INSERT INTO @Permisos VALUES
      (N'Prueba Gerencia',N'VENTAS_VER'),(N'Prueba Gerencia',N'VENTAS_REALIZAR'),
      (N'Prueba Gerencia',N'CLIENTES_VER'),(N'Prueba Gerencia',N'CLIENTES_ALTA'),
      (N'Prueba Gerencia',N'CLIENTES_MODIFICAR'),
      (N'Prueba Gerencia',N'PRODUCTOS_VER'),(N'Prueba Gerencia',N'AVISOS_VER'),
      (N'Prueba Gerencia',N'AVISOS_PUBLICAR'),(N'Prueba Gerencia',N'REPORTES_GERENTE'),
      (N'Prueba Gerencia',N'REPORTES_VER'),
      (N'Prueba Gerencia',N'REPORTES_VENTAS'),(N'Prueba Gerencia',N'REPORTES_RECAUDACION'),
      (N'Prueba Gerencia',N'REPORTES_PRODUCTOS'),(N'Prueba Gerencia',N'REPORTES_STOCK'),
      (N'Prueba Gerencia',N'REPORTES_RENDIMIENTO_VENDEDORES'),
      (N'Prueba Gerencia',N'REPORTES_DETALLE_VENTAS'),
      (N'Prueba Gerencia',N'REPORTES_EXPORTAR'),(N'Prueba Gerencia',N'REPORTES_ALCANCE_SUCURSAL'),
      (N'Prueba Ventas',N'VENTAS_VER'),(N'Prueba Ventas',N'VENTAS_REALIZAR'),
      (N'Prueba Ventas',N'CLIENTES_VER'),(N'Prueba Ventas',N'CLIENTES_ALTA'),
      (N'Prueba Ventas',N'PRODUCTOS_VER'),(N'Prueba Ventas',N'AVISOS_VER'),
      (N'Prueba Ventas',N'REPORTES_VENDEDOR'),(N'Prueba Ventas',N'REPORTES_VER'),
      (N'Prueba Ventas',N'REPORTES_VENTAS'),(N'Prueba Ventas',N'REPORTES_RECAUDACION'),
      (N'Prueba Ventas',N'REPORTES_PRODUCTOS'),(N'Prueba Ventas',N'REPORTES_DETALLE_VENTAS'),
      (N'Prueba Ventas',N'REPORTES_ALCANCE_PROPIO');

    INSERT INTO dbo.PERFIL_FUNCIONALIDAD(id_perfil,id_funcionalidad)
    SELECT p.id_perfil,f.id_funcionalidad
    FROM @Permisos x
    INNER JOIN dbo.PERFIL p
        ON p.nombre = x.perfil
       AND p.eliminado_en IS NULL
    INNER JOIN dbo.FUNCIONALIDAD f
        ON f.codigo = x.codigo
       AND f.eliminado_en IS NULL
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.PERFIL_FUNCIONALIDAD pf
        WHERE pf.id_perfil = p.id_perfil
          AND pf.id_funcionalidad = f.id_funcionalidad
    );

    /* ========================================================
       11 USUARIOS DEMO
       ======================================================== */
    DECLARE @Usuarios TABLE
    (
        usuario NVARCHAR(50) PRIMARY KEY,
        nombre NVARCHAR(100),
        apellido NVARCHAR(100),
        dni NVARCHAR(20),
        correo NVARCHAR(150),
        telefono NVARCHAR(30),
        perfil NVARCHAR(50) NULL,
        sucursal NVARCHAR(100) NULL,
        es_global BIT
    );

    INSERT INTO @Usuarios VALUES
      (N'coordinador_global_test',N'Carla',N'Coordinadora Global',N'97000001',N'coordinador.global@test.local',N'+54 379 4100001',NULL,NULL,1),
      (N'gerente_a',N'Juan',N'Gerente A',N'97000010',N'juan.gerentea@test.local',N'+54 379 4100010',N'Prueba Gerencia',N'Sucursal Prueba Centro',0),
      (N'gerente_b',N'Ana',N'Gerente B',N'97000011',N'ana.gerenteb@test.local',N'+54 379 4100011',N'Prueba Gerencia',N'Sucursal Prueba Norte',0),
      (N'vendedor_a1',N'Pedro',N'Vendedor A1',N'97000020',N'pedro.vendedora1@test.local',N'+54 379 4100020',N'Prueba Ventas',N'Sucursal Prueba Centro',0),
      (N'vendedor_a2',N'Lucas',N'Vendedor A2',N'97000021',N'lucas.vendedora2@test.local',N'+54 379 4100021',N'Prueba Ventas',N'Sucursal Prueba Centro',0),
      (N'vendedor_a3',N'Micaela',N'Vendedora A3',N'97000025',N'micaela.vendedora3@test.local',N'+54 379 4100025',N'Prueba Ventas',N'Sucursal Prueba Centro',0),
      (N'vendedor_b1',N'Maria',N'Vendedora B1',N'97000022',N'maria.vendedorab1@test.local',N'+54 379 4100022',N'Prueba Ventas',N'Sucursal Prueba Norte',0),
      (N'vendedor_b2',N'Diego',N'Vendedor B2',N'97000023',N'diego.vendedorb2@test.local',N'+54 379 4100023',N'Prueba Ventas',N'Sucursal Prueba Norte',0),
      (N'vendedor_b3',N'Nicolas',N'Vendedor B3',N'97000026',N'nicolas.vendedorb3@test.local',N'+54 379 4100026',N'Prueba Ventas',N'Sucursal Prueba Norte',0),
      (N'vendedor_central1',N'Lucia',N'Vendedora Central 1',N'97000024',N'lucia.central1@test.local',N'+54 379 4100024',N'Prueba Ventas',N'Sucursal Central',0),
      (N'vendedor_central2',N'Ezequiel',N'Vendedor Central 2',N'97000027',N'ezequiel.central2@test.local',N'+54 379 4100027',N'Prueba Ventas',N'Sucursal Central',0);

    IF EXISTS
    (
        SELECT 1
        FROM @Usuarios x
        INNER JOIN dbo.USUARIO u
            ON (u.dni = x.dni OR u.correo = x.correo)
           AND u.nombre_usuario <> x.usuario
    )
        THROW 51005, 'Un DNI o correo reservado ya pertenece a otro usuario.', 1;

    UPDATE u
    SET u.id_perfil = COALESCE(p.id_perfil,pg.id_perfil),
        u.id_sucursal = CASE WHEN x.es_global = 1 THEN NULL ELSE s.id_sucursal END,
        u.nombre = x.nombre,
        u.apellido = x.apellido,
        u.dni = x.dni,
        u.correo = x.correo,
        u.telefono = x.telefono,
        u.contrasena_hash = @HashPrueba,
        u.eliminado_en = NULL
    FROM dbo.USUARIO u
    INNER JOIN @Usuarios x
        ON x.usuario = u.nombre_usuario
    LEFT JOIN dbo.PERFIL p
        ON p.nombre = x.perfil
       AND p.eliminado_en IS NULL
    OUTER APPLY
    (
        SELECT TOP 1 id_perfil
        FROM dbo.PERFIL
        WHERE alcance_global = 1
          AND eliminado_en IS NULL
        ORDER BY id_perfil
    ) pg
    LEFT JOIN dbo.SUCURSAL s
        ON s.nombre = x.sucursal
       AND s.eliminado_en IS NULL;

    INSERT INTO dbo.USUARIO
    (
        id_perfil,id_sucursal,nombre,apellido,dni,telefono,
        nombre_usuario,contrasena_hash,correo
    )
    SELECT COALESCE(p.id_perfil,pg.id_perfil),
           CASE WHEN x.es_global = 1 THEN NULL ELSE s.id_sucursal END,
           x.nombre,x.apellido,x.dni,x.telefono,x.usuario,@HashPrueba,x.correo
    FROM @Usuarios x
    LEFT JOIN dbo.PERFIL p
        ON p.nombre = x.perfil
       AND p.eliminado_en IS NULL
    OUTER APPLY
    (
        SELECT TOP 1 id_perfil
        FROM dbo.PERFIL
        WHERE alcance_global = 1
          AND eliminado_en IS NULL
        ORDER BY id_perfil
    ) pg
    LEFT JOIN dbo.SUCURSAL s
        ON s.nombre = x.sucursal
       AND s.eliminado_en IS NULL
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.USUARIO u
        WHERE u.nombre_usuario = x.usuario
    );

    /* ========================================================
       VENDEDORES DEMO REALES SEGUN PERMISO VENTAS_REALIZAR
       Un usuario global se asocia a Sucursal Central solo para
       las ventas historicas de este escenario.
       ======================================================== */
    DECLARE @Vendedores TABLE
    (
        numero INT PRIMARY KEY,
        usuario NVARCHAR(50) UNIQUE,
        sucursal NVARCHAR(100)
    );

    INSERT INTO @Vendedores(numero,usuario,sucursal)
    SELECT ROW_NUMBER() OVER(ORDER BY u.nombre_usuario),
           u.nombre_usuario,
           COALESCE(s.nombre,N'Sucursal Central')
    FROM dbo.USUARIO u
    INNER JOIN @Usuarios du
        ON du.usuario = u.nombre_usuario
    INNER JOIN dbo.PERFIL_FUNCIONALIDAD pf
        ON pf.id_perfil = u.id_perfil
    INNER JOIN dbo.FUNCIONALIDAD f
        ON f.id_funcionalidad = pf.id_funcionalidad
       AND f.codigo = N'VENTAS_REALIZAR'
       AND f.eliminado_en IS NULL
    LEFT JOIN dbo.SUCURSAL s
        ON s.id_sucursal = u.id_sucursal
       AND s.eliminado_en IS NULL
    WHERE u.eliminado_en IS NULL;

    DECLARE @CantidadVendedores INT = (SELECT COUNT(*) FROM @Vendedores);

    IF @CantidadVendedores = 0
        THROW 51006, 'No hay usuarios demo activos con permiso VENTAS_REALIZAR.', 1;

    /* ========================================================
       180 CLIENTES NORMALES
       Se generan de forma determinista combinando nombres/apellidos.
       ======================================================== */
    DECLARE @Nombres TABLE(numero INT PRIMARY KEY,nombre NVARCHAR(100));
    INSERT INTO @Nombres VALUES
      (1,N'Ana'),(2,N'Bruno'),(3,N'Carolina'),(4,N'Daniel'),(5,N'Elena'),
      (6,N'Federico'),(7,N'Gabriela'),(8,N'Hernan'),(9,N'Ines'),(10,N'Javier'),
      (11,N'Luciana'),(12,N'Marcos'),(13,N'Natalia'),(14,N'Oscar'),(15,N'Paula'),
      (16,N'Ricardo'),(17,N'Sofia'),(18,N'Tomas'),(19,N'Valeria'),(20,N'Walter'),
      (21,N'Agustina'),(22,N'Bautista'),(23,N'Camila'),(24,N'Dario'),(25,N'Emilia'),
      (26,N'Facundo'),(27,N'Gisela'),(28,N'Hugo'),(29,N'Julieta'),(30,N'Kevin');

    DECLARE @Apellidos TABLE(numero INT PRIMARY KEY,apellido NVARCHAR(100));
    INSERT INTO @Apellidos VALUES
      (1,N'Martinez'),(2,N'Lopez'),(3,N'Gomez'),(4,N'Fernandez'),(5,N'Rodriguez'),
      (6,N'Benitez'),(7,N'Torres'),(8,N'Acosta'),(9,N'Ramos'),(10,N'Sosa'),
      (11,N'Maidana'),(12,N'Ojeda'),(13,N'Cardozo'),(14,N'Gonzalez'),(15,N'Romero'),
      (16,N'Aguirre'),(17,N'Vera'),(18,N'Cabral'),(19,N'Mendez'),(20,N'Almiron'),
      (21,N'Paredes'),(22,N'Duarte'),(23,N'Escobar'),(24,N'Quiroga'),(25,N'Rios'),
      (26,N'Medina'),(27,N'Franco'),(28,N'Vallejos'),(29,N'Castro'),(30,N'Ortega');

    DECLARE @Calles TABLE(numero INT PRIMARY KEY,calle NVARCHAR(150));
    INSERT INTO @Calles VALUES
      (1,N'Junín'),(2,N'Córdoba'),(3,N'Catamarca'),(4,N'Salta'),(5,N'La Rioja'),
      (6,N'Mendoza'),(7,N'9 de Julio'),(8,N'25 de Mayo'),(9,N'Av. 3 de Abril'),
      (10,N'Av. Maipú'),(11,N'Bolívar'),(12,N'Belgrano'),(13,N'San Juan'),
      (14,N'Tucumán'),(15,N'Santa Fe'),(16,N'Buenos Aires'),(17,N'Pellegrini'),
      (18,N'Quintana'),(19,N'España'),(20,N'Ruta 12');

    DECLARE @Clientes TABLE
    (
        numero INT PRIMARY KEY,
        documento NVARCHAR(20) UNIQUE,
        nombre NVARCHAR(100),
        apellido NVARCHAR(100),
        correo NVARCHAR(150),
        telefono NVARCHAR(30),
        localidad NVARCHAR(100),
        calle NVARCHAR(150),
        altura NVARCHAR(20),
        piso NVARCHAR(2) NULL
    );

    ;WITH Combinaciones AS
    (
        SELECT TOP (180)
            ROW_NUMBER() OVER(ORDER BY n.numero,a.numero) AS numero,
            n.nombre,
            a.apellido
        FROM @Nombres n
        CROSS JOIN @Apellidos a
        ORDER BY n.numero,a.numero
    )
    INSERT INTO @Clientes
    (
        numero,documento,nombre,apellido,correo,telefono,
        localidad,calle,altura,piso
    )
    SELECT c.numero,
           CONVERT(NVARCHAR(20),98000100 + c.numero),
           c.nombre,
           c.apellido,
           N'cliente' + RIGHT(N'000' + CONVERT(NVARCHAR(3),c.numero),3) + N'@test.local',
           N'+54 379 ' + RIGHT(N'0000000' + CONVERT(NVARCHAR(7),4200000 + c.numero),7),
           l.nombre,
           ca.calle,
           CONVERT(NVARCHAR(20),100 + c.numero * 7),
           CASE WHEN c.numero % 4 = 0 THEN NULL
                ELSE CONVERT(NVARCHAR(2),c.numero % 12) END
    FROM Combinaciones c
    INNER JOIN @LocalidadesPrueba l
        ON l.numero = ((c.numero - 1) % 18) + 1
    INNER JOIN @Calles ca
        ON ca.numero = ((c.numero * 7 - 1) % 20) + 1;

    /* Direcciones normales */
    INSERT INTO dbo.DIRECCION(id_localidad,calle,altura,piso)
    SELECT l.id_localidad,x.calle,x.altura,x.piso
    FROM @Clientes x
    INNER JOIN dbo.LOCALIDAD l
        ON l.nombre = x.localidad
       AND l.eliminado_en IS NULL
    INNER JOIN dbo.PROVINCIA p
        ON p.id_provincia = l.id_provincia
       AND p.nombre = N'Corrientes'
       AND p.eliminado_en IS NULL
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.DIRECCION d
        WHERE d.id_localidad = l.id_localidad
          AND d.calle = x.calle
          AND d.altura = x.altura
          AND ISNULL(d.piso,N'') = ISNULL(x.piso,N'')
          AND d.eliminado_en IS NULL
    );

    UPDATE d
    SET d.eliminado_en = NULL
    FROM dbo.DIRECCION d
    INNER JOIN dbo.LOCALIDAD l
        ON l.id_localidad = d.id_localidad
    INNER JOIN @Clientes x
        ON x.localidad = l.nombre
       AND x.calle = d.calle
       AND x.altura = d.altura
       AND ISNULL(x.piso,N'') = ISNULL(d.piso,N'');

    UPDATE c
    SET c.nombre = x.nombre,
        c.apellido = x.apellido,
        c.correo = x.correo,
        c.telefono = x.telefono,
        c.id_direccion = d.id_direccion,
        c.eliminado_en = NULL
    FROM dbo.CLIENTE c
    INNER JOIN @Clientes x
        ON x.documento = c.documento
    INNER JOIN dbo.LOCALIDAD l
        ON l.nombre = x.localidad
       AND l.eliminado_en IS NULL
    INNER JOIN dbo.PROVINCIA p
        ON p.id_provincia = l.id_provincia
       AND p.nombre = N'Corrientes'
    INNER JOIN dbo.DIRECCION d
        ON d.id_localidad = l.id_localidad
       AND d.calle = x.calle
       AND d.altura = x.altura
       AND ISNULL(d.piso,N'') = ISNULL(x.piso,N'')
       AND d.eliminado_en IS NULL;

    INSERT INTO dbo.CLIENTE(nombre,apellido,documento,correo,telefono,id_direccion)
    SELECT x.nombre,x.apellido,x.documento,x.correo,x.telefono,d.id_direccion
    FROM @Clientes x
    INNER JOIN dbo.LOCALIDAD l
        ON l.nombre = x.localidad
       AND l.eliminado_en IS NULL
    INNER JOIN dbo.PROVINCIA p
        ON p.id_provincia = l.id_provincia
       AND p.nombre = N'Corrientes'
    INNER JOIN dbo.DIRECCION d
        ON d.id_localidad = l.id_localidad
       AND d.calle = x.calle
       AND d.altura = x.altura
       AND ISNULL(d.piso,N'') = ISNULL(x.piso,N'')
       AND d.eliminado_en IS NULL
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.CLIENTE c
        WHERE c.documento = x.documento
    );

    /* ========================================================
       CLIENTE GENERICO: CONSUMIDOR FINAL
       Representa comprador no identificado. La venta sigue
       perteneciendo siempre a un usuario/vendedor real.
       ======================================================== */
    DECLARE @DocumentoConsumidorFinal NVARCHAR(20) = N'99999999';
    DECLARE @IdLocalidadCF INT =
    (
        SELECT TOP 1 l.id_localidad
        FROM dbo.LOCALIDAD l
        INNER JOIN dbo.PROVINCIA p
            ON p.id_provincia = l.id_provincia
        WHERE p.nombre = N'Corrientes'
          AND l.nombre = N'Corrientes'
          AND l.eliminado_en IS NULL
        ORDER BY l.id_localidad
    );

    DECLARE @IdDireccionCF INT;
    SELECT TOP 1 @IdDireccionCF = d.id_direccion
    FROM dbo.DIRECCION d
    WHERE d.id_localidad = @IdLocalidadCF
      AND d.calle = N'Junín'
      AND d.altura = N'1'
      AND ISNULL(d.piso,N'') = N''
      AND d.eliminado_en IS NULL
    ORDER BY d.id_direccion;

    IF @IdDireccionCF IS NULL
    BEGIN
        INSERT INTO dbo.DIRECCION(id_localidad,calle,altura,piso)
        VALUES(@IdLocalidadCF,N'Junín',N'1',NULL);
        SET @IdDireccionCF = CAST(SCOPE_IDENTITY() AS INT);
    END;

    IF EXISTS (SELECT 1 FROM dbo.CLIENTE WHERE documento = @DocumentoConsumidorFinal)
    BEGIN
        UPDATE dbo.CLIENTE
        SET nombre = N'Consumidor',
            apellido = N'Final',
            correo = NULL,
            telefono = NULL,
            id_direccion = @IdDireccionCF,
            eliminado_en = NULL
        WHERE documento = @DocumentoConsumidorFinal;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.CLIENTE(nombre,apellido,documento,correo,telefono,id_direccion)
        VALUES(N'Consumidor',N'Final',@DocumentoConsumidorFinal,NULL,NULL,@IdDireccionCF);
    END;

    /* Limpia direcciones heredadas del antiguo escenario "Cliente Prueba". */
    UPDATE d
    SET d.eliminado_en = SYSDATETIME()
    FROM dbo.DIRECCION d
    INNER JOIN dbo.LOCALIDAD l
        ON l.id_localidad = d.id_localidad
    INNER JOIN dbo.PROVINCIA p
        ON p.id_provincia = l.id_provincia
    WHERE p.nombre = N'Corrientes'
      AND d.calle = N'Cliente Prueba'
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.CLIENTE c
          WHERE c.id_direccion = d.id_direccion
      );

    /* ========================================================
       25 PRODUCTOS
       ======================================================== */
    DECLARE @Productos TABLE
    (
        nombre NVARCHAR(100) PRIMARY KEY,
        categoria NVARCHAR(100),
        marca NVARCHAR(100),
        codigo NVARCHAR(50) UNIQUE,
        costo DECIMAL(18,2),
        ganancia DECIMAL(5,2)
    );

    INSERT INTO @Productos VALUES
        (N'Amoladora Angular 900W',N'Ferretería',N'Bosch',N'7799900000011',60000,40),
        (N'Soldadora Inverter 220A',N'Herrería',N'Lusqtoff',N'7799900000012',85000,40),
        (N'Guantes de Seguridad Industrial',N'Pinturería',N'Ingco',N'7799900000013',15000,40),
        (N'Atornillador Inalámbrico',N'Durlok',N'Total',N'7799900000014',70000,40),
        (N'Cinta Métrica 8m',N'Construcción',N'Bremen',N'7799900000015',57000,40),
        (N'Martillo de Uña',N'Ferretería',N'Total',N'7799900000016',67000,40),
        (N'Calibrador Digital',N'Construcción',N'DeWalt',N'7799900000017',50000,40),
        (N'Casco de Soldar Fotosensible',N'Herrería',N'Makita',N'7799900000018',92000,40),
        (N'Prensa de Banco Reforzada',N'Carpintería',N'Lusqtoff',N'7799900000019',90000,40),
        (N'Kit-3 Brochas Profesional',N'Pinturería',N'Stanley',N'7799900000020',22000,40),
        (N'Sierra Circular',N'Carpintería',N'Makita',N'7799900000021',97000,40),
        (N'Atornillador Inalámbrico para Durlok',N'Durlok',N'Milwaukee',N'7799900000022',77000,40),
        (N'Sierra Circular Inalámbrica DeWalt',N'Carpintería',N'DeWalt',N'7799900000023',104000,40),
        (N'Taladro Inalámbrico DeWalt con Baterías',N'Construcción',N'DeWalt',N'7799900000024',64000,40),
        (N'Atornillador de Impacto Inalámbrico DeWalt',N'Ferretería',N'DeWalt',N'7799900000025',74000,40),
        (N'Sierra Circular Makita',N'Carpintería',N'Makita',N'7799900000026',111000,40),
        (N'Martillo de Goma Ingco',N'Herrería',N'Ingco',N'7799900000027',99000,40),
        (N'Caja de Herramientas Apilable con Ruedas Milwaukee',N'Ferretería',N'Milwaukee',N'7799900000028',81000,40),
        (N'Caja de Herramientas Apilable con Manija Milwaukee',N'Ferretería',N'Milwaukee',N'7799900000029',88000,40),
        (N'Sierra Caladora con Cable Makita',N'Carpintería',N'Makita',N'7799900000030',118000,40),
        (N'Amoladora Angular 1500W',N'Ferretería',N'DeWalt',N'7799900000031',95000,40),
        (N'Taladro Makita 13mm 220v 710w',N'Carpintería',N'Makita',N'7799900000032',125000,40),
        (N'Medidor de Distancia Láser 100 pies',N'Carpintería',N'DeWalt',N'7799900000033',132000,40),
        (N'Multímetro Digital Baw RM113D',N'Electricidad',N'Baw',N'7799900000034',30000,40),
        (N'Multímetro Digital Automático Recargable',N'Electricidad',N'Neng',N'7799900000035',37000,40);

    IF EXISTS
    (
        SELECT 1
        FROM @Productos x
        INNER JOIN dbo.PRODUCTO p
            ON p.codigo_barra = x.codigo
        WHERE p.nombre <> x.nombre
    )
    OR EXISTS
    (
        SELECT 1
        FROM @Productos x
        INNER JOIN dbo.PRODUCTO p
            ON p.nombre = x.nombre
        WHERE ISNULL(p.codigo_barra,N'') <> x.codigo
    )
        THROW 51007, 'Un identificador reservado del catalogo de prueba ya pertenece a otro producto.', 1;

    UPDATE c
    SET c.eliminado_en = NULL
    FROM dbo.CATEGORIA c
    INNER JOIN (SELECT DISTINCT categoria FROM @Productos) x
        ON x.categoria = c.nombre;

    INSERT INTO dbo.CATEGORIA(nombre)
    SELECT DISTINCT x.categoria
    FROM @Productos x
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.CATEGORIA c
        WHERE c.nombre = x.categoria
    );

    UPDATE m
    SET m.eliminado_en = NULL
    FROM dbo.MARCA m
    INNER JOIN (SELECT DISTINCT marca FROM @Productos) x
        ON x.marca = m.nombre;

    INSERT INTO dbo.MARCA(nombre)
    SELECT DISTINCT x.marca
    FROM @Productos x
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.MARCA m
        WHERE m.nombre = x.marca
    );

    INSERT INTO dbo.MARCA_CATEGORIA(id_marca,id_categoria)
    SELECT DISTINCT m.id_marca,c.id_categoria
    FROM @Productos x
    INNER JOIN dbo.MARCA m ON m.nombre = x.marca
    INNER JOIN dbo.CATEGORIA c ON c.nombre = x.categoria
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.MARCA_CATEGORIA mc
        WHERE mc.id_marca = m.id_marca
          AND mc.id_categoria = c.id_categoria
    );

    UPDATE p
    SET p.id_categoria = c.id_categoria,
        p.id_marca = m.id_marca,
        p.activo = 1,
        p.eliminado_en = NULL,
        p.precio_costo = x.costo,
        p.porcentaje_ganancia = x.ganancia,
        p.descripcion = N'Producto del escenario ampliado de pruebas.'
    FROM dbo.PRODUCTO p
    INNER JOIN @Productos x ON x.codigo = p.codigo_barra
    INNER JOIN dbo.CATEGORIA c ON c.nombre = x.categoria
    INNER JOIN dbo.MARCA m ON m.nombre = x.marca;

    INSERT INTO dbo.PRODUCTO
    (
        id_categoria,id_marca,codigo_barra,nombre,descripcion,
        precio_costo,porcentaje_ganancia,activo
    )
    SELECT c.id_categoria,m.id_marca,x.codigo,x.nombre,
           N'Producto del escenario ampliado de pruebas.',
           x.costo,x.ganancia,1
    FROM @Productos x
    INNER JOIN dbo.CATEGORIA c ON c.nombre = x.categoria
    INNER JOIN dbo.MARCA m ON m.nombre = x.marca
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.PRODUCTO p
        WHERE p.codigo_barra = x.codigo
    );

    DECLARE @ProductosNum TABLE
    (
        numero INT PRIMARY KEY,
        nombre NVARCHAR(100) UNIQUE,
        id_producto INT
    );

    INSERT INTO @ProductosNum(numero,nombre,id_producto)
    SELECT ROW_NUMBER() OVER(ORDER BY p.codigo_barra),p.nombre,p.id_producto
    FROM dbo.PRODUCTO p
    WHERE p.codigo_barra BETWEEN N'7799900000011' AND N'7799900000035'
      AND p.eliminado_en IS NULL;

    IF (SELECT COUNT(*) FROM @ProductosNum) <> 25
        THROW 51008, 'No se pudieron resolver los 25 productos del escenario.', 1;

    /* ========================================================
       INVENTARIO BASE ALTO - SUFICIENTE PARA 1000 VENTAS
       ======================================================== */
    DECLARE @SucStock TABLE(nombre NVARCHAR(100) PRIMARY KEY);
    INSERT INTO @SucStock VALUES
      (N'Sucursal Central'),
      (N'Sucursal Prueba Centro'),
      (N'Sucursal Prueba Norte');

    MERGE dbo.INVENTARIO AS destino
    USING
    (
        SELECT pn.id_producto,
               s.id_sucursal,
               2500 + pn.numero * 45 +
               CASE s.nombre
                    WHEN N'Sucursal Central' THEN 150
                    WHEN N'Sucursal Prueba Centro' THEN 250
                    ELSE 200
               END AS stock,
               20 + (pn.numero % 4) * 5 AS stock_minimo
        FROM @ProductosNum pn
        CROSS JOIN @SucStock ss
        INNER JOIN dbo.SUCURSAL s
            ON s.nombre = ss.nombre
           AND s.eliminado_en IS NULL
    ) AS origen
    ON destino.id_producto = origen.id_producto
   AND destino.id_sucursal = origen.id_sucursal
    WHEN MATCHED THEN
        UPDATE SET destino.stock = origen.stock,
                   destino.stock_minimo = origen.stock_minimo,
                   destino.eliminado_en = NULL
    WHEN NOT MATCHED THEN
        INSERT(id_producto,id_sucursal,stock,stock_minimo)
        VALUES(origen.id_producto,origen.id_sucursal,origen.stock,origen.stock_minimo);

    /* ========================================================
       LIMPIEZA DE DATOS DEMO TRANSACCIONALES PARA REEJECUTAR
       Se identifican por usuarios demo + clientes demo/CF.
       ======================================================== */
    DELETE p
    FROM dbo.PAGO p
    INNER JOIN dbo.VENTA v ON v.id_venta = p.id_venta
    INNER JOIN dbo.USUARIO u ON u.id_usuario = v.id_usuario
    INNER JOIN dbo.CLIENTE c ON c.id_cliente = v.id_cliente
    WHERE u.nombre_usuario IN (SELECT usuario FROM @Usuarios)
      AND
      (
          TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280
          OR c.documento = @DocumentoConsumidorFinal
      );

    DELETE d
    FROM dbo.DETALLE_VENTA d
    INNER JOIN dbo.VENTA v ON v.id_venta = d.id_venta
    INNER JOIN dbo.USUARIO u ON u.id_usuario = v.id_usuario
    INNER JOIN dbo.CLIENTE c ON c.id_cliente = v.id_cliente
    WHERE u.nombre_usuario IN (SELECT usuario FROM @Usuarios)
      AND
      (
          TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280
          OR c.documento = @DocumentoConsumidorFinal
      );

    DELETE v
    FROM dbo.VENTA v
    INNER JOIN dbo.USUARIO u ON u.id_usuario = v.id_usuario
    INNER JOIN dbo.CLIENTE c ON c.id_cliente = v.id_cliente
    WHERE u.nombre_usuario IN (SELECT usuario FROM @Usuarios)
      AND
      (
          TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280
          OR c.documento = @DocumentoConsumidorFinal
      );

    DELETE FROM dbo.AVISO
    WHERE titulo LIKE N'DEMO:%';

    DELETE FROM dbo.AUDITORIA
    WHERE detalle LIKE N'DEMO:%';

    /* ========================================================
       1000 VENTAS / 180 DIAS

       Distribucion temporal:
       - 240 ventas en dias 0..13 (ultimos 14 dias)
       - 180 ventas en dias 14..30
       - 580 ventas en dias 31..180

       Cada usuario demo con VENTAS_REALIZAR recibe ventas por
       round-robin. Consumidor Final representa ~12,5%.
       ======================================================== */
    DECLARE @Ventas TABLE
    (
        n INT PRIMARY KEY,
        dias INT,
        minutos INT,
        usuario NVARCHAR(50),
        sucursal NVARCHAR(100),
        cliente NVARCHAR(20),
        descuento_pct DECIMAL(5,2),
        modo_pago TINYINT
    );

    ;WITH N AS
    (
        SELECT TOP (1000)
            ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS n
        FROM sys.all_objects a
        CROSS JOIN sys.all_objects b
    )
    INSERT INTO @Ventas
    (
        n,dias,minutos,usuario,sucursal,cliente,descuento_pct,modo_pago
    )
    SELECT n.n,
           CASE
               WHEN n.n <= 240 THEN -((n.n - 1) % 14)
               WHEN n.n <= 420 THEN -(14 + ((n.n - 241) % 17))
               ELSE -(31 + ((n.n - 421) % 150))
           END,
           510 + ((n.n * 37) % 660),
           v.usuario,
           v.sucursal,
           CASE
               WHEN n.n % 8 = 0 THEN @DocumentoConsumidorFinal
               WHEN n.n % 5 = 0 THEN CONVERT(NVARCHAR(20),98000100 + 1 + ((n.n * 7) % 20))
               WHEN n.n % 3 = 0 THEN CONVERT(NVARCHAR(20),98000100 + 1 + ((n.n * 11) % 60))
               ELSE CONVERT(NVARCHAR(20),98000100 + 1 + ((n.n * 13) % 180))
           END,
           CASE
               WHEN n.n % 67 = 0 THEN 15
               WHEN n.n % 19 = 0 THEN 10
               WHEN n.n % 7 = 0 THEN 5
               ELSE 0
           END,
           CASE
               WHEN n.n % 20 IN (0,1,2) THEN 5 + (n.n % 3) /* 5,6,7 = pagos combinados */
               ELSE 1 + (n.n % 4)                         /* 1..4 = pago simple */
           END
    FROM N n
    INNER JOIN @Vendedores v
        ON v.numero = ((n.n - 1) % @CantidadVendedores) + 1;

    IF (SELECT COUNT(*) FROM @Ventas) <> 1000
        THROW 51009, 'No se generaron las 1000 ventas previstas.', 1;

    IF EXISTS
    (
        SELECT v.usuario
        FROM @Vendedores v
        LEFT JOIN @Ventas x ON x.usuario = v.usuario
        GROUP BY v.usuario
        HAVING COUNT(x.n) < 20
    )
        THROW 51010, 'Existe un vendedor demo con menos de 20 ventas.', 1;

    /* 1 a 5 productos por venta. Los productos 1..5 son los mas populares. */
    DECLARE @Detalles TABLE
    (
        n INT,
        numero_producto INT,
        cantidad INT,
        PRIMARY KEY(n,numero_producto)
    );

    INSERT INTO @Detalles(n,numero_producto,cantidad)
    SELECT v.n,
           CASE s.slot
               WHEN 0 THEN 1 + ((v.n - 1) % 5)
               WHEN 1 THEN 6 + ((v.n * 7) % 20)
               WHEN 2 THEN 6 + ((v.n * 7 + 7) % 20)
               WHEN 3 THEN 6 + ((v.n * 7 + 13) % 20)
               ELSE 6 + ((v.n * 7 + 17) % 20)
           END,
           1 + ((v.n + s.slot * 2) % 4)
    FROM @Ventas v
    CROSS APPLY (VALUES(0),(1),(2),(3),(4)) s(slot)
    WHERE s.slot < 1 + (v.n % 5);

    /* Totales calculados con precio vigente. */
    DECLARE @Totales TABLE
    (
        n INT PRIMARY KEY,
        subtotal DECIMAL(18,2),
        descuento DECIMAL(18,2),
        total DECIMAL(18,2)
    );

    INSERT INTO @Totales(n,subtotal,descuento,total)
    SELECT v.n,
           CAST(SUM(p.precio_venta * d.cantidad) AS DECIMAL(18,2)),
           CAST(ROUND(SUM(p.precio_venta * d.cantidad) * v.descuento_pct / 100.0,2) AS DECIMAL(18,2)),
           CAST(
               SUM(p.precio_venta * d.cantidad)
               - ROUND(SUM(p.precio_venta * d.cantidad) * v.descuento_pct / 100.0,2)
               AS DECIMAL(18,2)
           )
    FROM @Ventas v
    INNER JOIN @Detalles d ON d.n = v.n
    INNER JOIN @ProductosNum pn ON pn.numero = d.numero_producto
    INNER JOIN dbo.PRODUCTO p ON p.id_producto = pn.id_producto
    GROUP BY v.n,v.descuento_pct;

    DECLARE @VentaIds TABLE
    (
        n INT PRIMARY KEY,
        id_venta INT UNIQUE
    );

    MERGE dbo.VENTA AS destino
    USING
    (
        SELECT x.n,
               c.id_cliente,
               u.id_usuario,
               s.id_sucursal,
               DATEADD(MINUTE,x.minutos,DATEADD(DAY,x.dias,CAST(@Hoy AS DATETIME2))) AS fecha_hora,
               t.subtotal,
               t.descuento,
               t.total
        FROM @Ventas x
        INNER JOIN @Totales t ON t.n = x.n
        INNER JOIN dbo.CLIENTE c ON c.documento = x.cliente AND c.eliminado_en IS NULL
        INNER JOIN dbo.USUARIO u ON u.nombre_usuario = x.usuario AND u.eliminado_en IS NULL
        INNER JOIN dbo.SUCURSAL s ON s.nombre = x.sucursal AND s.eliminado_en IS NULL
    ) AS origen
    ON 1 = 0
    WHEN NOT MATCHED THEN
        INSERT(id_cliente,id_usuario,id_sucursal,fecha_hora,tipo_factura,subtotal,descuento,total)
        VALUES(origen.id_cliente,origen.id_usuario,origen.id_sucursal,origen.fecha_hora,NULL,
               origen.subtotal,origen.descuento,origen.total)
    OUTPUT origen.n,inserted.id_venta INTO @VentaIds(n,id_venta);

    IF (SELECT COUNT(*) FROM @VentaIds) <> 1000
        THROW 51011, 'No se insertaron las 1000 ventas previstas.', 1;

    INSERT INTO dbo.DETALLE_VENTA(id_venta,id_producto,cantidad,precio_unitario,subtotal)
    SELECT vi.id_venta,
           pn.id_producto,
           d.cantidad,
           CAST(p.precio_venta AS DECIMAL(18,2)),
           CAST(p.precio_venta * d.cantidad AS DECIMAL(18,2))
    FROM @Detalles d
    INNER JOIN @VentaIds vi ON vi.n = d.n
    INNER JOIN @ProductosNum pn ON pn.numero = d.numero_producto
    INNER JOIN dbo.PRODUCTO p ON p.id_producto = pn.id_producto;

    /* ========================================================
       PAGOS
       modo 1..4: simple
       modo 5: 40% Efectivo + resto Transferencia
       modo 6: 50% Credito + resto Efectivo
       modo 7: 30% Debito + resto Transferencia
       ======================================================== */
    DECLARE @Pagos TABLE
    (
        n INT,
        metodo NVARCHAR(100),
        proporcion_base DECIMAL(5,2),
        es_resto BIT,
        PRIMARY KEY(n,metodo)
    );

    INSERT INTO @Pagos
    SELECT n,N'Efectivo',1,0 FROM @Ventas WHERE modo_pago = 1
    UNION ALL
    SELECT n,N'Débito',1,0 FROM @Ventas WHERE modo_pago = 2
    UNION ALL
    SELECT n,N'Crédito',1,0 FROM @Ventas WHERE modo_pago = 3
    UNION ALL
    SELECT n,N'Transferencia',1,0 FROM @Ventas WHERE modo_pago = 4
    UNION ALL
    SELECT n,N'Efectivo',0.40,0 FROM @Ventas WHERE modo_pago = 5
    UNION ALL
    SELECT n,N'Transferencia',0.40,1 FROM @Ventas WHERE modo_pago = 5
    UNION ALL
    SELECT n,N'Crédito',0.50,0 FROM @Ventas WHERE modo_pago = 6
    UNION ALL
    SELECT n,N'Efectivo',0.50,1 FROM @Ventas WHERE modo_pago = 6
    UNION ALL
    SELECT n,N'Débito',0.30,0 FROM @Ventas WHERE modo_pago = 7
    UNION ALL
    SELECT n,N'Transferencia',0.30,1 FROM @Ventas WHERE modo_pago = 7;

    INSERT INTO dbo.PAGO(id_venta,id_metodo_pago,monto)
    SELECT vi.id_venta,
           mp.id_metodo_pago,
           CASE
               WHEN pg.es_resto = 1
                   THEN t.total - CAST(ROUND(t.total * pg.proporcion_base,2) AS DECIMAL(18,2))
               ELSE CAST(ROUND(t.total * pg.proporcion_base,2) AS DECIMAL(18,2))
           END
    FROM @Pagos pg
    INNER JOIN @VentaIds vi ON vi.n = pg.n
    INNER JOIN @Totales t ON t.n = pg.n
    INNER JOIN dbo.METODO_PAGO mp
        ON mp.nombre COLLATE Latin1_General_100_CI_AI =
           pg.metodo COLLATE Latin1_General_100_CI_AI
       AND mp.eliminado_en IS NULL;

    /* Valida que cada venta haya quedado completamente pagada. */
    IF EXISTS
    (
        SELECT vi.id_venta
        FROM @VentaIds vi
        INNER JOIN dbo.VENTA v ON v.id_venta = vi.id_venta
        LEFT JOIN dbo.PAGO p ON p.id_venta = vi.id_venta AND p.eliminado_en IS NULL
        GROUP BY vi.id_venta,v.total
        HAVING ABS(v.total - ISNULL(SUM(p.monto),0)) > 0.01
    )
        THROW 51012, 'El escenario genero ventas cuyo total no coincide con sus pagos.', 1;

    /* ========================================================
       STOCK ACTUAL
       10 combinaciones quedan intencionalmente en stock bajo.
       ======================================================== */
    ;WITH Consumo AS
    (
        SELECT v.id_sucursal,d.id_producto,SUM(d.cantidad) AS cantidad
        FROM @VentaIds vi
        INNER JOIN dbo.VENTA v ON v.id_venta = vi.id_venta
        INNER JOIN dbo.DETALLE_VENTA d
            ON d.id_venta = v.id_venta
           AND d.eliminado_en IS NULL
        GROUP BY v.id_sucursal,d.id_producto
    )
    UPDATE i
    SET i.stock =
        CASE
            WHEN pn.numero IN (3,11,19,24) AND s.nombre = N'Sucursal Prueba Centro'
                THEN CASE pn.numero WHEN 3 THEN 0 WHEN 11 THEN 3 WHEN 19 THEN 5 ELSE 7 END
            WHEN pn.numero IN (2,8,21) AND s.nombre = N'Sucursal Prueba Norte'
                THEN CASE pn.numero WHEN 2 THEN 2 WHEN 8 THEN 0 ELSE 4 END
            WHEN pn.numero IN (5,14,25) AND s.nombre = N'Sucursal Central'
                THEN CASE pn.numero WHEN 5 THEN 1 WHEN 14 THEN 6 ELSE 8 END
            ELSE 1800 + pn.numero * 25 - ISNULL(c.cantidad,0)
        END,
        i.stock_minimo =
        CASE
            WHEN (pn.numero IN (3,11,19,24) AND s.nombre = N'Sucursal Prueba Centro')
              OR (pn.numero IN (2,8,21) AND s.nombre = N'Sucursal Prueba Norte')
              OR (pn.numero IN (5,14,25) AND s.nombre = N'Sucursal Central')
                THEN 10
            ELSE 20 + (pn.numero % 3) * 5
        END,
        i.eliminado_en = NULL
    FROM dbo.INVENTARIO i
    INNER JOIN dbo.SUCURSAL s ON s.id_sucursal = i.id_sucursal
    INNER JOIN @ProductosNum pn ON pn.id_producto = i.id_producto
    LEFT JOIN Consumo c
        ON c.id_sucursal = i.id_sucursal
       AND c.id_producto = i.id_producto;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.INVENTARIO
        WHERE stock < 0
    )
        THROW 51013, 'El escenario genero stock negativo.', 1;

    /* ========================================================
       24 AVISOS
       ======================================================== */
    DECLARE @Avisos TABLE
    (
        titulo NVARCHAR(100) PRIMARY KEY,
        mensaje NVARCHAR(500),
        autor NVARCHAR(50),
        sucursal NVARCHAR(100) NULL,
        dias INT
    );

    INSERT INTO @Avisos VALUES
      (N'DEMO: Revisión semanal de indicadores',N'Revisar ventas, recaudación y productos destacados antes de la reunión semanal.',N'coordinador_global_test',NULL,-175),
      (N'DEMO: Objetivos comerciales del semestre',N'Se actualizaron los objetivos del semestre. Revisar avance por sucursal y vendedor.',N'coordinador_global_test',NULL,-160),
      (N'DEMO: Control de inventario Centro',N'Realizar control de inventario y verificar diferencias antes del cierre.',N'gerente_a',N'Sucursal Prueba Centro',-148),
      (N'DEMO: Reposición prioritaria Norte',N'Priorizar la reposición de productos marcados con stock bajo.',N'gerente_b',N'Sucursal Prueba Norte',-136),
      (N'DEMO: Capacitación interna',N'Capacitación sobre registro de ventas y medios de pago.',N'coordinador_global_test',NULL,-124),
      (N'DEMO: Cambio de horario Centro',N'El sábado la sucursal atenderá en horario reducido.',N'gerente_a',N'Sucursal Prueba Centro',-111),
      (N'DEMO: Revisión de stock mínimo',N'Controlar valores de stock mínimo de herramientas eléctricas.',N'gerente_b',N'Sucursal Prueba Norte',-98),
      (N'DEMO: Nuevo catálogo disponible',N'Ya está disponible el nuevo catálogo de productos para consulta.',N'coordinador_global_test',NULL,-86),
      (N'DEMO: Cierre mensual Centro',N'Completar controles pendientes antes del cierre mensual.',N'gerente_a',N'Sucursal Prueba Centro',-74),
      (N'DEMO: Cierre mensual Norte',N'Completar controles pendientes antes del cierre mensual.',N'gerente_b',N'Sucursal Prueba Norte',-73),
      (N'DEMO: Promoción herramientas',N'Informar a clientes sobre la promoción vigente en herramientas seleccionadas.',N'coordinador_global_test',NULL,-61),
      (N'DEMO: Seguimiento de productos agotados',N'Revisar productos agotados y solicitar reposición cuando corresponda.',N'gerente_a',N'Sucursal Prueba Centro',-50),
      (N'DEMO: Control preventivo Norte',N'Verificar inventario de seguridad antes del fin de semana.',N'gerente_b',N'Sucursal Prueba Norte',-42),
      (N'DEMO: Recordatorio de comprobantes',N'Verificar que cada venta quede asociada correctamente al cliente y forma de pago.',N'coordinador_global_test',NULL,-35),
      (N'DEMO: Seguimiento comercial Centro',N'Revisar clientes frecuentes y oportunidades de venta de la semana.',N'gerente_a',N'Sucursal Prueba Centro',-28),
      (N'DEMO: Seguimiento comercial Norte',N'Revisar clientes frecuentes y oportunidades de venta de la semana.',N'gerente_b',N'Sucursal Prueba Norte',-24),
      (N'DEMO: Revisión de indicadores',N'Revisar indicadores acumulados de los últimos meses para preparar la defensa.',N'coordinador_global_test',NULL,-18),
      (N'DEMO: Reunión de equipo Centro',N'Reunión breve de seguimiento comercial antes de la apertura.',N'gerente_a',N'Sucursal Prueba Centro',-13),
      (N'DEMO: Reunión de equipo Norte',N'Reunión breve de seguimiento comercial antes de la apertura.',N'gerente_b',N'Sucursal Prueba Norte',-11),
      (N'DEMO: Control de pagos combinados',N'Verificar que las ventas con más de un medio de pago queden correctamente registradas.',N'coordinador_global_test',NULL,-8),
      (N'DEMO: Reposición Centro',N'Revisar el reporte de stock bajo y preparar la próxima reposición.',N'gerente_a',N'Sucursal Prueba Centro',-6),
      (N'DEMO: Reposición Norte',N'Revisar el reporte de stock bajo y preparar la próxima reposición.',N'gerente_b',N'Sucursal Prueba Norte',-4),
      (N'DEMO: Prioridades de la semana',N'Priorizar atención al cliente, control de stock y seguimiento de ventas.',N'coordinador_global_test',NULL,-2),
      (N'DEMO: Stock crítico',N'Atender primero los productos con stock en cero o por debajo del mínimo.',N'gerente_b',N'Sucursal Prueba Norte',-1);

    INSERT INTO dbo.AVISO
    (
        titulo,mensaje,fecha_creacion,id_usuario_autor,id_sucursal,activo
    )
    SELECT x.titulo,x.mensaje,
           DATEADD(MINUTE,600 + ((ROW_NUMBER() OVER(ORDER BY x.dias,x.titulo) * 23) % 420),
                   DATEADD(DAY,x.dias,CAST(@Hoy AS DATETIME2))),
           u.id_usuario,s.id_sucursal,1
    FROM @Avisos x
    INNER JOIN dbo.USUARIO u
        ON u.nombre_usuario = x.autor
       AND u.eliminado_en IS NULL
    LEFT JOIN dbo.SUCURSAL s
        ON s.nombre = x.sucursal
       AND s.eliminado_en IS NULL;

    /* ========================================================
       180 MOVIMIENTOS DE AUDITORIA
       Cada 12 movimientos uno es BACKUP => 15 backups.
       ======================================================== */
    ;WITH N AS
    (
        SELECT TOP (180)
            ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS n
        FROM sys.all_objects a
        CROSS JOIN sys.all_objects b
    ), Datos AS
    (
        SELECT n,
               CASE
                   WHEN n <= 30 THEN -((n - 1) % 14)
                   ELSE -(14 + ((n * 11) % 167))
               END AS dias,
               CASE
                   WHEN n % 11 = 0 THEN N'coordinador_global_test'
                   WHEN n % 11 = 1 THEN N'gerente_a'
                   WHEN n % 11 = 2 THEN N'gerente_b'
                   WHEN n % 11 = 3 THEN N'vendedor_a1'
                   WHEN n % 11 = 4 THEN N'vendedor_a2'
                   WHEN n % 11 = 5 THEN N'vendedor_a3'
                   WHEN n % 11 = 6 THEN N'vendedor_b1'
                   WHEN n % 11 = 7 THEN N'vendedor_b2'
                   WHEN n % 11 = 8 THEN N'vendedor_b3'
                   WHEN n % 11 = 9 THEN N'vendedor_central1'
                   ELSE N'vendedor_central2'
               END AS usuario,
               CASE
                   WHEN n % 12 = 0 THEN N'BACKUP'
                   WHEN n % 9 = 0 THEN N'REACTIVACION'
                   WHEN n % 7 = 0 THEN N'BAJA'
                   WHEN n % 3 = 0 THEN N'MODIFICACION'
                   ELSE N'ALTA'
               END AS accion,
               CASE
                   WHEN n % 12 = 0 THEN N'BASE_DATOS'
                   WHEN n % 10 = 0 THEN N'INVENTARIO'
                   WHEN n % 8 = 0 THEN N'AVISO'
                   WHEN n % 6 = 0 THEN N'PRODUCTO'
                   WHEN n % 5 = 0 THEN N'USUARIO'
                   WHEN n % 4 = 0 THEN N'VENTA'
                   ELSE N'CLIENTE'
               END AS entidad
        FROM N
    )
    INSERT INTO dbo.AUDITORIA
    (
        id_usuario,fecha,accion,entidad,id_entidad,detalle,id_sucursal
    )
    SELECT u.id_usuario,
           DATEADD(MINUTE,510 + ((d.n * 41) % 660),
                   DATEADD(DAY,d.dias,CAST(@Hoy AS DATETIME2))),
           d.accion,
           d.entidad,
           NULL,
           N'DEMO: ' +
           CASE d.entidad
               WHEN N'VENTA' THEN N'Venta registrada y confirmada correctamente.'
               WHEN N'INVENTARIO' THEN N'Se actualizó el stock de un producto de la sucursal.'
               WHEN N'CLIENTE' THEN
                   CASE d.accion
                       WHEN N'BAJA' THEN N'Cliente dado de baja para verificar trazabilidad.'
                       WHEN N'REACTIVACION' THEN N'Cliente reactivado luego de una baja de prueba.'
                       ELSE N'Datos del cliente actualizados correctamente.'
                   END
               WHEN N'PRODUCTO' THEN N'Precio, marca o datos del producto actualizados.'
               WHEN N'USUARIO' THEN N'Datos de un usuario actualizados para la demostración.'
               WHEN N'BASE_DATOS' THEN N'Copia de seguridad generada correctamente en el servidor.'
               WHEN N'AVISO' THEN N'Aviso interno publicado para el equipo.'
               ELSE N'Movimiento administrativo de prueba.'
           END
           + N' Ref. #' + CONVERT(NVARCHAR(10),d.n),
           COALESCE(u.id_sucursal,
                    (SELECT TOP 1 id_sucursal FROM dbo.SUCURSAL WHERE nombre=N'Sucursal Central' AND eliminado_en IS NULL))
    FROM Datos d
    INNER JOIN dbo.USUARIO u
        ON u.nombre_usuario = d.usuario
       AND u.eliminado_en IS NULL;

    /* ========================================================
       VALIDACIONES INTERNAS ANTES DE COMMIT
       ======================================================== */
    IF (SELECT COUNT(*) FROM @Clientes) <> 180
        THROW 51014, 'No se generaron los 180 clientes normales previstos.', 1;

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.CLIENTE
        WHERE documento = @DocumentoConsumidorFinal
          AND eliminado_en IS NULL
    )
        THROW 51015, 'No se pudo preparar Consumidor Final.', 1;

    IF (SELECT COUNT(*) FROM @VentaIds) <> 1000
        THROW 51016, 'Cantidad final de ventas demo distinta de 1000.', 1;

    IF EXISTS
    (
        SELECT fecha.dia
        FROM
        (
            SELECT 0 dia UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3
            UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7
            UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL SELECT 10 UNION ALL SELECT 11
            UNION ALL SELECT 12 UNION ALL SELECT 13
        ) fecha
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM @Ventas v
            WHERE v.dias = -fecha.dia
        )
    )
        THROW 51017, 'Falta al menos un dia con ventas dentro de los ultimos 14 dias.', 1;

    IF
    (
        SELECT COUNT(*)
        FROM dbo.INVENTARIO i
        INNER JOIN @ProductosNum pn ON pn.id_producto = i.id_producto
        INNER JOIN dbo.SUCURSAL s ON s.id_sucursal = i.id_sucursal
        WHERE s.nombre IN (N'Sucursal Central',N'Sucursal Prueba Centro',N'Sucursal Prueba Norte')
          AND i.eliminado_en IS NULL
          AND i.stock <= i.stock_minimo
    ) <> 10
        THROW 51018, 'La cantidad de stocks bajos demo no coincide con las 10 combinaciones previstas.', 1;

    IF (SELECT COUNT(*) FROM dbo.AVISO WHERE titulo LIKE N'DEMO:%') <> 24
        THROW 51019, 'La cantidad de avisos DEMO no coincide con 24.', 1;

    IF (SELECT COUNT(*) FROM dbo.AUDITORIA WHERE detalle LIKE N'DEMO:%') <> 180
        THROW 51020, 'La cantidad de auditorias DEMO no coincide con 180.', 1;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

/* =========================================================
   VERIFICACION DEL ESCENARIO
   Estas consultas no modifican datos.
   ========================================================= */

/* A) Resumen general */
SELECT N'Localidades demo Corrientes' AS indicador,COUNT(*) AS cantidad
FROM dbo.LOCALIDAD l
INNER JOIN dbo.PROVINCIA p ON p.id_provincia=l.id_provincia
WHERE p.nombre=N'Corrientes'
  AND l.nombre IN
  (
    N'Corrientes',N'Riachuelo',N'Santa Ana de los Guácaras',N'San Luis del Palmar',
    N'Paso de la Patria',N'San Cosme',N'Itatí',N'Empedrado',N'Saladas',N'Bella Vista',
    N'Goya',N'Mercedes',N'Curuzú Cuatiá',N'Monte Caseros',N'Santo Tomé',N'Ituzaingó',
    N'Gobernador Virasoro',N'Esquina'
  )
  AND l.eliminado_en IS NULL
UNION ALL
SELECT N'Clientes demo normales',COUNT(*)
FROM dbo.CLIENTE
WHERE TRY_CONVERT(INT,documento) BETWEEN 98000101 AND 98000280
  AND eliminado_en IS NULL
UNION ALL
SELECT N'Consumidor Final',COUNT(*)
FROM dbo.CLIENTE
WHERE documento=N'99999999' AND eliminado_en IS NULL
UNION ALL
SELECT N'Usuarios demo',COUNT(*)
FROM dbo.USUARIO
WHERE nombre_usuario IN
(
 N'coordinador_global_test',N'gerente_a',N'gerente_b',
 N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',N'vendedor_b1',N'vendedor_b2',
 N'vendedor_b3',N'vendedor_central1',N'vendedor_central2'
)
AND eliminado_en IS NULL
UNION ALL
SELECT N'Productos demo',COUNT(*)
FROM dbo.PRODUCTO
WHERE codigo_barra BETWEEN N'7799900000011' AND N'7799900000035'
  AND eliminado_en IS NULL
UNION ALL
SELECT N'Ventas demo',COUNT(*)
FROM dbo.VENTA v
INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
WHERE u.nombre_usuario IN
(
 N'coordinador_global_test',N'gerente_a',N'gerente_b',
 N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',N'vendedor_b1',N'vendedor_b2',
 N'vendedor_b3',N'vendedor_central1',N'vendedor_central2'
)
AND (TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280 OR c.documento=N'99999999')
AND v.eliminado_en IS NULL
UNION ALL
SELECT N'Avisos DEMO',COUNT(*) FROM dbo.AVISO WHERE titulo LIKE N'DEMO:%'
UNION ALL
SELECT N'Auditorias DEMO',COUNT(*) FROM dbo.AUDITORIA WHERE detalle LIKE N'DEMO:%'
UNION ALL
SELECT N'Backups DEMO',COUNT(*) FROM dbo.AUDITORIA
WHERE detalle LIKE N'DEMO:%' AND accion=N'BACKUP' AND entidad=N'BASE_DATOS';

/* B) Clientes por localidad */
SELECT p.nombre AS Provincia,l.nombre AS Localidad,COUNT(*) AS Clientes
FROM dbo.CLIENTE c
INNER JOIN dbo.DIRECCION d ON d.id_direccion=c.id_direccion
INNER JOIN dbo.LOCALIDAD l ON l.id_localidad=d.id_localidad
INNER JOIN dbo.PROVINCIA p ON p.id_provincia=l.id_provincia
WHERE TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280
  AND c.eliminado_en IS NULL
GROUP BY p.nombre,l.nombre
ORDER BY p.nombre,l.nombre;

/* C) Ventas por vendedor */
SELECT u.nombre_usuario AS Vendedor,
       COALESCE(s.nombre,N'Todas las sucursales') AS Sucursal,
       COUNT(*) AS Ventas,
       SUM(v.total) AS Facturacion
FROM dbo.VENTA v
INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
LEFT JOIN dbo.SUCURSAL s ON s.id_sucursal=v.id_sucursal
WHERE u.nombre_usuario IN
(
 N'coordinador_global_test',N'gerente_a',N'gerente_b',
 N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',N'vendedor_b1',N'vendedor_b2',
 N'vendedor_b3',N'vendedor_central1',N'vendedor_central2'
)
AND (TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280 OR c.documento=N'99999999')
GROUP BY u.nombre_usuario,s.nombre
ORDER BY Ventas DESC,u.nombre_usuario;

/* D) Ventas por sucursal */
SELECT s.nombre AS Sucursal,COUNT(*) AS Ventas,SUM(v.total) AS Facturacion
FROM dbo.VENTA v
INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=v.id_sucursal
WHERE u.nombre_usuario IN
(
 N'coordinador_global_test',N'gerente_a',N'gerente_b',
 N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',N'vendedor_b1',N'vendedor_b2',
 N'vendedor_b3',N'vendedor_central1',N'vendedor_central2'
)
AND (TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280 OR c.documento=N'99999999')
GROUP BY s.nombre
ORDER BY Ventas DESC;

/* E) Ventas por dia - ultimos 14 dias */
SELECT CONVERT(DATE,v.fecha_hora) AS Dia,COUNT(*) AS Ventas,SUM(v.total) AS Facturacion
FROM dbo.VENTA v
INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
WHERE u.nombre_usuario IN
(
 N'coordinador_global_test',N'gerente_a',N'gerente_b',
 N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',N'vendedor_b1',N'vendedor_b2',
 N'vendedor_b3',N'vendedor_central1',N'vendedor_central2'
)
AND (TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280 OR c.documento=N'99999999')
AND v.fecha_hora >= DATEADD(DAY,-13,CONVERT(DATE,SYSDATETIME()))
GROUP BY CONVERT(DATE,v.fecha_hora)
ORDER BY Dia;

/* F) Ventas por metodo de pago */
SELECT mp.nombre AS Metodo,
       COUNT(DISTINCT p.id_venta) AS Ventas,
       COUNT(*) AS Pagos,
       SUM(p.monto) AS Importe
FROM dbo.PAGO p
INNER JOIN dbo.METODO_PAGO mp ON mp.id_metodo_pago=p.id_metodo_pago
INNER JOIN dbo.VENTA v ON v.id_venta=p.id_venta
INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
WHERE u.nombre_usuario IN
(
 N'coordinador_global_test',N'gerente_a',N'gerente_b',
 N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',N'vendedor_b1',N'vendedor_b2',
 N'vendedor_b3',N'vendedor_central1',N'vendedor_central2'
)
AND (TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280 OR c.documento=N'99999999')
GROUP BY mp.nombre
ORDER BY Importe DESC;

/* G) Cantidad de ventas con pagos combinados */
SELECT COUNT(*) AS VentasConPagoCombinado
FROM
(
    SELECT p.id_venta
    FROM dbo.PAGO p
    INNER JOIN dbo.VENTA v ON v.id_venta=p.id_venta
    INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
    INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
    WHERE u.nombre_usuario IN
    (
     N'coordinador_global_test',N'gerente_a',N'gerente_b',
     N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',N'vendedor_b1',N'vendedor_b2',
     N'vendedor_b3',N'vendedor_central1',N'vendedor_central2'
    )
    AND (TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280 OR c.documento=N'99999999')
    GROUP BY p.id_venta
    HAVING COUNT(*) > 1
) x;

/* H) Top 10 productos */
SELECT TOP 10 pr.nombre AS Producto,
       SUM(d.cantidad) AS Unidades,
       SUM(d.subtotal) AS Importe
FROM dbo.DETALLE_VENTA d
INNER JOIN dbo.VENTA v ON v.id_venta=d.id_venta
INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
INNER JOIN dbo.PRODUCTO pr ON pr.id_producto=d.id_producto
WHERE u.nombre_usuario IN
(
 N'coordinador_global_test',N'gerente_a',N'gerente_b',
 N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',N'vendedor_b1',N'vendedor_b2',
 N'vendedor_b3',N'vendedor_central1',N'vendedor_central2'
)
AND (TRY_CONVERT(INT,c.documento) BETWEEN 98000101 AND 98000280 OR c.documento=N'99999999')
GROUP BY pr.nombre
ORDER BY Unidades DESC,Importe DESC;

/* I) Stock bajo */
SELECT pr.codigo_barra,pr.nombre AS Producto,s.nombre AS Sucursal,
       i.stock,i.stock_minimo,(i.stock-i.stock_minimo) AS Diferencia
FROM dbo.INVENTARIO i
INNER JOIN dbo.PRODUCTO pr ON pr.id_producto=i.id_producto
INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=i.id_sucursal
WHERE pr.codigo_barra BETWEEN N'7799900000011' AND N'7799900000035'
  AND s.nombre IN (N'Sucursal Central',N'Sucursal Prueba Centro',N'Sucursal Prueba Norte')
  AND i.eliminado_en IS NULL
  AND i.stock <= i.stock_minimo
ORDER BY s.nombre,pr.nombre;

/* J) Ventas de Consumidor Final */
SELECT COUNT(*) AS VentasConsumidorFinal,
       SUM(v.total) AS FacturacionConsumidorFinal
FROM dbo.VENTA v
INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
WHERE c.documento=N'99999999'
  AND u.nombre_usuario IN
  (
   N'coordinador_global_test',N'gerente_a',N'gerente_b',
   N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',N'vendedor_b1',N'vendedor_b2',
   N'vendedor_b3',N'vendedor_central1',N'vendedor_central2'
  );

/* K) Auditoria por accion */
SELECT accion,entidad,COUNT(*) AS Cantidad
FROM dbo.AUDITORIA
WHERE detalle LIKE N'DEMO:%'
GROUP BY accion,entidad
ORDER BY accion,entidad;

/* L) Avisos por alcance */
SELECT CASE WHEN a.id_sucursal IS NULL THEN N'Global' ELSE s.nombre END AS Alcance,
       COUNT(*) AS Avisos
FROM dbo.AVISO a
LEFT JOIN dbo.SUCURSAL s ON s.id_sucursal=a.id_sucursal
WHERE a.titulo LIKE N'DEMO:%'
GROUP BY CASE WHEN a.id_sucursal IS NULL THEN N'Global' ELSE s.nombre END
ORDER BY Alcance;
GO
