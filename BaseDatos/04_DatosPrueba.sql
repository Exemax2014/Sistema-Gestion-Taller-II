/* =========================================================
   SISTEMA DE GESTION - DATOS DE PRUEBA AMPLIADOS
   Requiere: 01_Estructura.sql, 02_DatosIniciales.sql y 03_Procedimientos.sql.

   Escenario de demostracion:
   - 11 usuarios de prueba (clave comun: Prueba2026!)
   - 40 clientes
   - 25 productos
   - 3 sucursales con inventario
   - 320 ventas distribuidas en los ultimos 150 dias
   - 18 avisos internos historicos
   - 90 movimientos de auditoria, incluidos backups historicos

   Flujo recomendado:
      01 -> 02 -> 03 -> 05_ResetBasePruebas -> 04_DatosPrueba
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

    /* ---------- PRECONDICIONES ---------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.PERFIL WHERE alcance_global=1 AND eliminado_en IS NULL)
        THROW 51001, 'Falta un perfil global activo. Ejecute 02_DatosIniciales.sql.', 1;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.SUCURSAL
        WHERE nombre=N'Sucursal Central' AND eliminado_en IS NULL
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
            WHERE f.codigo=r.codigo AND f.eliminado_en IS NULL
        )
    )
        THROW 51003, 'Faltan funcionalidades oficiales. Ejecute 02_DatosIniciales.sql.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM (VALUES(N'Efectivo'),(N'Debito'),(N'Credito'),(N'Transferencia')) AS r(nombre)
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

    /* ---------- UBICACION Y SUCURSALES ---------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.PROVINCIA WHERE nombre=N'Corrientes')
        INSERT INTO dbo.PROVINCIA(nombre) VALUES(N'Corrientes');

    IF NOT EXISTS (
        SELECT 1 FROM dbo.LOCALIDAD l
        INNER JOIN dbo.PROVINCIA p ON p.id_provincia=l.id_provincia
        WHERE p.nombre=N'Corrientes' AND l.nombre=N'Corrientes'
    )
        INSERT INTO dbo.LOCALIDAD(id_provincia,nombre,codigo_postal)
        SELECT id_provincia,N'Corrientes',N'3400'
        FROM dbo.PROVINCIA WHERE nombre=N'Corrientes';

    DECLARE @Sucursales TABLE(nombre NVARCHAR(100) PRIMARY KEY,calle NVARCHAR(150),altura NVARCHAR(20),telefono NVARCHAR(30));
    INSERT INTO @Sucursales VALUES
      (N'Sucursal Prueba Centro',N'Pasaje de Prueba',N'100',N'+54 379 4000100'),
      (N'Sucursal Prueba Norte',N'Pasaje de Prueba',N'200',N'+54 379 4000200');

    INSERT INTO dbo.DIRECCION(id_localidad,calle,altura)
    SELECT l.id_localidad,x.calle,x.altura
    FROM @Sucursales x
    CROSS JOIN (
        SELECT TOP 1 l.id_localidad
        FROM dbo.LOCALIDAD l
        INNER JOIN dbo.PROVINCIA p ON p.id_provincia=l.id_provincia
        WHERE p.nombre=N'Corrientes' AND l.nombre=N'Corrientes'
    ) l
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.DIRECCION d
        WHERE d.id_localidad=l.id_localidad AND d.calle=x.calle AND d.altura=x.altura
    );

    UPDATE s SET s.id_direccion=d.id_direccion,s.telefono=x.telefono,s.eliminado_en=NULL
    FROM dbo.SUCURSAL s
    INNER JOIN @Sucursales x ON x.nombre=s.nombre
    INNER JOIN dbo.DIRECCION d ON d.calle=x.calle AND d.altura=x.altura;

    INSERT INTO dbo.SUCURSAL(nombre,telefono,id_direccion)
    SELECT x.nombre,x.telefono,d.id_direccion
    FROM @Sucursales x
    INNER JOIN dbo.DIRECCION d ON d.calle=x.calle AND d.altura=x.altura
    WHERE NOT EXISTS (SELECT 1 FROM dbo.SUCURSAL s WHERE s.nombre=x.nombre);

    /* ---------- PERFILES Y PERMISOS ---------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.PERFIL WHERE nombre=N'Prueba Gerencia')
        INSERT INTO dbo.PERFIL(nombre,descripcion,alcance_global)
        VALUES(N'Prueba Gerencia',N'Perfil de prueba con alcance por sucursal.',0);
    ELSE
        UPDATE dbo.PERFIL SET eliminado_en=NULL,alcance_global=0
        WHERE nombre=N'Prueba Gerencia';

    IF NOT EXISTS (SELECT 1 FROM dbo.PERFIL WHERE nombre=N'Prueba Ventas')
        INSERT INTO dbo.PERFIL(nombre,descripcion,alcance_global)
        VALUES(N'Prueba Ventas',N'Perfil de prueba con alcance propio.',0);
    ELSE
        UPDATE dbo.PERFIL SET eliminado_en=NULL,alcance_global=0
        WHERE nombre=N'Prueba Ventas';

    DECLARE @Permisos TABLE(perfil NVARCHAR(50),codigo NVARCHAR(50),PRIMARY KEY(perfil,codigo));
    INSERT INTO @Permisos VALUES
      (N'Prueba Gerencia',N'VENTAS_VER'),(N'Prueba Gerencia',N'VENTAS_REALIZAR'),
      (N'Prueba Gerencia',N'CLIENTES_VER'),(N'Prueba Gerencia',N'CLIENTES_ALTA'),
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
      (N'Prueba Ventas',N'REPORTES_VENDEDOR'),(N'Prueba Ventas',N'REPORTES_VER'),(N'Prueba Ventas',N'REPORTES_VENTAS'),
      (N'Prueba Ventas',N'REPORTES_RECAUDACION'),(N'Prueba Ventas',N'REPORTES_PRODUCTOS'),
      (N'Prueba Ventas',N'REPORTES_DETALLE_VENTAS'),(N'Prueba Ventas',N'REPORTES_ALCANCE_PROPIO');

    INSERT INTO dbo.PERFIL_FUNCIONALIDAD(id_perfil,id_funcionalidad)
    SELECT p.id_perfil,f.id_funcionalidad
    FROM @Permisos x
    INNER JOIN dbo.PERFIL p ON p.nombre=x.perfil AND p.eliminado_en IS NULL
    INNER JOIN dbo.FUNCIONALIDAD f ON f.codigo=x.codigo AND f.eliminado_en IS NULL
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.PERFIL_FUNCIONALIDAD pf
        WHERE pf.id_perfil=p.id_perfil AND pf.id_funcionalidad=f.id_funcionalidad
    );

    /* ---------- USUARIOS ---------- */
    DECLARE @Usuarios TABLE(
        usuario NVARCHAR(50) PRIMARY KEY,nombre NVARCHAR(100),apellido NVARCHAR(100),
        dni NVARCHAR(20),correo NVARCHAR(150),telefono NVARCHAR(30),
        perfil NVARCHAR(50) NULL,sucursal NVARCHAR(100) NULL,es_global BIT
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

    IF EXISTS (
        SELECT 1 FROM @Usuarios x
        INNER JOIN dbo.USUARIO u ON (u.dni=x.dni OR u.correo=x.correo)
        AND u.nombre_usuario<>x.usuario
    ) THROW 51003, 'Un DNI o correo reservado ya pertenece a otro usuario.', 1;

    UPDATE u SET
        u.id_perfil=COALESCE(p.id_perfil,pg.id_perfil),
        u.id_sucursal=CASE WHEN x.es_global=1 THEN NULL ELSE s.id_sucursal END,
        u.nombre=x.nombre,u.apellido=x.apellido,u.dni=x.dni,u.correo=x.correo,
        u.telefono=x.telefono,u.contrasena_hash=@HashPrueba,u.eliminado_en=NULL
    FROM dbo.USUARIO u
    INNER JOIN @Usuarios x ON x.usuario=u.nombre_usuario
    LEFT JOIN dbo.PERFIL p ON p.nombre=x.perfil AND p.eliminado_en IS NULL
    OUTER APPLY (
        SELECT TOP 1 id_perfil FROM dbo.PERFIL
        WHERE alcance_global=1 AND eliminado_en IS NULL ORDER BY id_perfil
    ) pg
    LEFT JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal AND s.eliminado_en IS NULL;

    INSERT INTO dbo.USUARIO(id_perfil,id_sucursal,nombre,apellido,dni,telefono,nombre_usuario,contrasena_hash,correo)
    SELECT COALESCE(p.id_perfil,pg.id_perfil),
           CASE WHEN x.es_global=1 THEN NULL ELSE s.id_sucursal END,
           x.nombre,x.apellido,x.dni,x.telefono,x.usuario,@HashPrueba,x.correo
    FROM @Usuarios x
    LEFT JOIN dbo.PERFIL p ON p.nombre=x.perfil AND p.eliminado_en IS NULL
    OUTER APPLY (
        SELECT TOP 1 id_perfil FROM dbo.PERFIL
        WHERE alcance_global=1 AND eliminado_en IS NULL ORDER BY id_perfil
    ) pg
    LEFT JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal AND s.eliminado_en IS NULL
    WHERE NOT EXISTS (SELECT 1 FROM dbo.USUARIO u WHERE u.nombre_usuario=x.usuario);

    IF EXISTS
    (
        SELECT 1
        FROM @Usuarios x
        WHERE x.usuario LIKE N'vendedor_%'
          AND NOT EXISTS
          (
              SELECT 1
              FROM dbo.USUARIO u
              INNER JOIN dbo.PERFIL_FUNCIONALIDAD pf ON pf.id_perfil=u.id_perfil
              INNER JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad=pf.id_funcionalidad
              WHERE u.nombre_usuario=x.usuario
                AND u.eliminado_en IS NULL
                AND f.codigo=N'VENTAS_REALIZAR'
                AND f.eliminado_en IS NULL
          )
    )
        THROW 51005, 'Un vendedor de prueba no posee VENTAS_REALIZAR.', 1;

    /* ---------- 40 CLIENTES ---------- */
    DECLARE @NombresCliente TABLE(n INT PRIMARY KEY,nombre NVARCHAR(100),apellido NVARCHAR(100));
    INSERT INTO @NombresCliente VALUES
        (1,N'Ana',N'Martinez'),
        (2,N'Bruno',N'Lopez'),
        (3,N'Carolina',N'Gomez'),
        (4,N'Daniel',N'Fernandez'),
        (5,N'Elena',N'Rodriguez'),
        (6,N'Federico',N'Benitez'),
        (7,N'Gabriela',N'Torres'),
        (8,N'Hernan',N'Acosta'),
        (9,N'Ines',N'Ramos'),
        (10,N'Javier',N'Sosa'),
        (11,N'Luciana',N'Maidana'),
        (12,N'Marcos',N'Ojeda'),
        (13,N'Natalia',N'Cardozo'),
        (14,N'Oscar',N'Gonzalez'),
        (15,N'Paula',N'Romero'),
        (16,N'Ricardo',N'Aguirre'),
        (17,N'Sofia',N'Vera'),
        (18,N'Tomas',N'Cabral'),
        (19,N'Valeria',N'Mendez'),
        (20,N'Walter',N'Almiron'),
        (21,N'Agustina',N'Paredes'),
        (22,N'Bautista',N'Duarte'),
        (23,N'Camila',N'Escobar'),
        (24,N'Dario',N'Nunez'),
        (25,N'Emilia',N'Quiroga'),
        (26,N'Facundo',N'Rios'),
        (27,N'Gisela',N'Medina'),
        (28,N'Hugo',N'Franco'),
        (29,N'Julieta',N'Vallejos'),
        (30,N'Kevin',N'Ramirez'),
        (31,N'Lorena',N'Castro'),
        (32,N'Matias',N'Dominguez'),
        (33,N'Noelia',N'Ferreyra'),
        (34,N'Pablo',N'Leiva'),
        (35,N'Rocio',N'Canteros'),
        (36,N'Santiago',N'Molina'),
        (37,N'Tamara',N'Meza'),
        (38,N'Ulises',N'Sanchez'),
        (39,N'Victoria',N'Navarro'),
        (40,N'Yamila',N'Ortega');

    DECLARE @Clientes TABLE(
        documento NVARCHAR(20) PRIMARY KEY,nombre NVARCHAR(100),apellido NVARCHAR(100),
        correo NVARCHAR(150),telefono NVARCHAR(30),altura NVARCHAR(20)
    );
    INSERT INTO @Clientes
    SELECT
      N'98000' + RIGHT(N'000' + CONVERT(NVARCHAR(3),100+n),3),
      nombre,apellido,
      LOWER(nombre + N'.' + apellido + N'@test.local'),
      N'+54 379 42' + RIGHT(N'0000' + CONVERT(NVARCHAR(4),1000+n),4),
      CONVERT(NVARCHAR(20),100+n)
    FROM @NombresCliente;

    INSERT INTO dbo.DIRECCION(id_localidad,calle,altura)
    SELECT l.id_localidad,N'Cliente Prueba',x.altura
    FROM @Clientes x
    CROSS JOIN (
        SELECT TOP 1 l.id_localidad FROM dbo.LOCALIDAD l
        INNER JOIN dbo.PROVINCIA p ON p.id_provincia=l.id_provincia
        WHERE p.nombre=N'Corrientes' AND l.nombre=N'Corrientes'
    ) l
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.DIRECCION d
        WHERE d.id_localidad=l.id_localidad AND d.calle=N'Cliente Prueba' AND d.altura=x.altura
    );

    UPDATE c SET c.nombre=x.nombre,c.apellido=x.apellido,c.correo=x.correo,
                 c.telefono=x.telefono,c.id_direccion=d.id_direccion,c.eliminado_en=NULL
    FROM dbo.CLIENTE c
    INNER JOIN @Clientes x ON x.documento=c.documento
    INNER JOIN dbo.DIRECCION d ON d.calle=N'Cliente Prueba' AND d.altura=x.altura;

    INSERT INTO dbo.CLIENTE(nombre,apellido,documento,correo,telefono,id_direccion)
    SELECT x.nombre,x.apellido,x.documento,x.correo,x.telefono,d.id_direccion
    FROM @Clientes x
    INNER JOIN dbo.DIRECCION d ON d.calle=N'Cliente Prueba' AND d.altura=x.altura
    WHERE NOT EXISTS (SELECT 1 FROM dbo.CLIENTE c WHERE c.documento=x.documento);

    /* ---------- 25 PRODUCTOS ---------- */
    DECLARE @Productos TABLE(
        nombre NVARCHAR(100) PRIMARY KEY,categoria NVARCHAR(100),marca NVARCHAR(100),
        codigo NVARCHAR(50) UNIQUE,costo DECIMAL(18,2),ganancia DECIMAL(5,2)
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
        INNER JOIN dbo.PRODUCTO p ON p.codigo_barra=x.codigo
        WHERE p.nombre<>x.nombre
    )
    OR EXISTS
    (
        SELECT 1
        FROM @Productos x
        INNER JOIN dbo.PRODUCTO p ON p.nombre=x.nombre
        WHERE ISNULL(p.codigo_barra,N'')<>x.codigo
    )
        THROW 51006, 'Un identificador reservado del catalogo de prueba ya pertenece a otro producto.', 1;

    UPDATE c
    SET c.eliminado_en=NULL
    FROM dbo.CATEGORIA c
    INNER JOIN (SELECT DISTINCT categoria FROM @Productos) x ON x.categoria=c.nombre;

    INSERT INTO dbo.CATEGORIA(nombre)
    SELECT DISTINCT x.categoria FROM @Productos x
    WHERE NOT EXISTS (SELECT 1 FROM dbo.CATEGORIA c WHERE c.nombre=x.categoria);

    UPDATE m
    SET m.eliminado_en=NULL
    FROM dbo.MARCA m
    INNER JOIN (SELECT DISTINCT marca FROM @Productos) x ON x.marca=m.nombre;

    INSERT INTO dbo.MARCA(nombre)
    SELECT DISTINCT x.marca FROM @Productos x
    WHERE NOT EXISTS (SELECT 1 FROM dbo.MARCA m WHERE m.nombre=x.marca);

    INSERT INTO dbo.MARCA_CATEGORIA(id_marca,id_categoria)
    SELECT DISTINCT m.id_marca,c.id_categoria
    FROM @Productos x
    INNER JOIN dbo.MARCA m ON m.nombre=x.marca
    INNER JOIN dbo.CATEGORIA c ON c.nombre=x.categoria
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.MARCA_CATEGORIA mc
        WHERE mc.id_marca=m.id_marca AND mc.id_categoria=c.id_categoria
    );

    UPDATE p SET p.id_categoria=c.id_categoria,p.id_marca=m.id_marca,p.activo=1,
                 p.eliminado_en=NULL,p.precio_costo=x.costo,p.porcentaje_ganancia=x.ganancia
    FROM dbo.PRODUCTO p
    INNER JOIN @Productos x ON x.codigo=p.codigo_barra
    INNER JOIN dbo.CATEGORIA c ON c.nombre=x.categoria
    INNER JOIN dbo.MARCA m ON m.nombre=x.marca;

    INSERT INTO dbo.PRODUCTO(id_categoria,id_marca,codigo_barra,nombre,descripcion,precio_costo,porcentaje_ganancia,activo)
    SELECT c.id_categoria,m.id_marca,x.codigo,x.nombre,
           N'Producto del escenario ampliado de pruebas.',x.costo,x.ganancia,1
    FROM @Productos x
    INNER JOIN dbo.CATEGORIA c ON c.nombre=x.categoria
    INNER JOIN dbo.MARCA m ON m.nombre=x.marca
    WHERE NOT EXISTS (SELECT 1 FROM dbo.PRODUCTO p WHERE p.codigo_barra=x.codigo);

    DECLARE @ProductosNum TABLE(numero INT PRIMARY KEY,nombre NVARCHAR(100));
    INSERT INTO @ProductosNum
    SELECT ROW_NUMBER() OVER(ORDER BY codigo),nombre FROM @Productos;

    /* ---------- INVENTARIO BASE ---------- */
    DECLARE @SucStock TABLE(nombre NVARCHAR(100) PRIMARY KEY);
    INSERT INTO @SucStock VALUES(N'Sucursal Central'),(N'Sucursal Prueba Centro'),(N'Sucursal Prueba Norte');

    MERGE dbo.INVENTARIO AS d
    USING (
      SELECT p.id_producto,s.id_sucursal,
             1100 + pn.numero*35 + CASE s.nombre WHEN N'Sucursal Central' THEN 50 WHEN N'Sucursal Prueba Centro' THEN 120 ELSE 90 END AS stock,
             15 + (pn.numero%5)*5 AS stock_minimo
      FROM @ProductosNum pn
      INNER JOIN dbo.PRODUCTO p ON p.nombre=pn.nombre
      CROSS JOIN @SucStock ss
      INNER JOIN dbo.SUCURSAL s ON s.nombre=ss.nombre
    ) AS o
    ON d.id_producto=o.id_producto AND d.id_sucursal=o.id_sucursal
    WHEN MATCHED THEN UPDATE SET d.stock=o.stock,d.stock_minimo=o.stock_minimo,d.eliminado_en=NULL
    WHEN NOT MATCHED THEN INSERT(id_producto,id_sucursal,stock,stock_minimo)
      VALUES(o.id_producto,o.id_sucursal,o.stock,o.stock_minimo);

    /* ---------- LIMPIEZA DEMO PARA REEJECUTAR ---------- */
    DELETE p FROM dbo.PAGO p
      INNER JOIN dbo.VENTA v ON v.id_venta=p.id_venta
      INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
      INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
      WHERE u.nombre_usuario IN(N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',
            N'vendedor_b1',N'vendedor_b2',N'vendedor_b3',
            N'vendedor_central1',N'vendedor_central2')
        AND c.documento BETWEEN N'98000101' AND N'98000140';

    DELETE d FROM dbo.DETALLE_VENTA d
      INNER JOIN dbo.VENTA v ON v.id_venta=d.id_venta
      INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
      INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
      WHERE u.nombre_usuario IN(N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',
            N'vendedor_b1',N'vendedor_b2',N'vendedor_b3',
            N'vendedor_central1',N'vendedor_central2')
        AND c.documento BETWEEN N'98000101' AND N'98000140';

    DELETE v FROM dbo.VENTA v
      INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
      INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
      WHERE u.nombre_usuario IN(N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',
            N'vendedor_b1',N'vendedor_b2',N'vendedor_b3',
            N'vendedor_central1',N'vendedor_central2')
        AND c.documento BETWEEN N'98000101' AND N'98000140';

    DELETE FROM dbo.AVISO WHERE titulo LIKE N'DEMO:%';
    DELETE FROM dbo.AUDITORIA WHERE detalle LIKE N'DEMO:%';

    /* ---------- 320 VENTAS / 150 DIAS ---------- */
    DECLARE @Ventas TABLE(
      n INT PRIMARY KEY,dias INT,minutos INT,sucursal NVARCHAR(100),
      usuario NVARCHAR(50),cliente NVARCHAR(20),descuento DECIMAL(5,2),pago TINYINT
    );

    ;WITH N AS (
      SELECT TOP (320) ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) n
      FROM sys.all_objects a CROSS JOIN sys.all_objects b
    )
    INSERT INTO @Ventas
    SELECT n,
      -((n*17+n/7)%151),
      510+((n*37)%660),
      CASE n%8 WHEN 0 THEN N'Sucursal Prueba Centro' WHEN 1 THEN N'Sucursal Prueba Centro'
               WHEN 2 THEN N'Sucursal Prueba Centro' WHEN 3 THEN N'Sucursal Prueba Norte'
               WHEN 4 THEN N'Sucursal Prueba Norte' WHEN 5 THEN N'Sucursal Prueba Norte'
               ELSE N'Sucursal Central' END,
      CASE n%8 WHEN 0 THEN N'vendedor_a1' WHEN 1 THEN N'vendedor_a2'
               WHEN 2 THEN N'vendedor_a3' WHEN 3 THEN N'vendedor_b1'
               WHEN 4 THEN N'vendedor_b2' WHEN 5 THEN N'vendedor_b3'
               WHEN 6 THEN N'vendedor_central1' ELSE N'vendedor_central2' END,
      N'98000' + RIGHT(N'000' + CONVERT(NVARCHAR(3),((n*7)%40)+101),3),
      CASE WHEN n%17=0 THEN 10 WHEN n%7=0 THEN 5 ELSE 0 END,
      CAST((n%5)+1 AS TINYINT)
    FROM N;

    DECLARE @Detalles TABLE(n INT,producto NVARCHAR(100),cantidad INT,PRIMARY KEY(n,producto));
    INSERT INTO @Detalles
    SELECT v.n,pn.nombre,1+((v.n+s.slot)%3)
    FROM @Ventas v
    CROSS APPLY (VALUES(0),(1),(2),(3)) s(slot)
    INNER JOIN @ProductosNum pn ON pn.numero=((v.n*3+s.slot*7)%25)+1
    WHERE s.slot <= (v.n%4);

    ;WITH Totales AS (
      SELECT d.n,CAST(SUM(p.precio_venta*d.cantidad) AS DECIMAL(18,2)) subtotal
      FROM @Detalles d INNER JOIN dbo.PRODUCTO p ON p.nombre=d.producto
      GROUP BY d.n
    )
    INSERT INTO dbo.VENTA(id_cliente,id_usuario,id_sucursal,fecha_hora,tipo_factura,subtotal,descuento,total)
    SELECT c.id_cliente,u.id_usuario,s.id_sucursal,
      DATEADD(MINUTE,x.minutos,DATEADD(DAY,x.dias,CAST(@Hoy AS DATETIME2))),
      NULL,t.subtotal,
      CAST(ROUND(t.subtotal*x.descuento/100.0,2) AS DECIMAL(18,2)),
      CAST(t.subtotal-ROUND(t.subtotal*x.descuento/100.0,2) AS DECIMAL(18,2))
    FROM @Ventas x
    INNER JOIN Totales t ON t.n=x.n
    INNER JOIN dbo.CLIENTE c ON c.documento=x.cliente
    INNER JOIN dbo.USUARIO u ON u.nombre_usuario=x.usuario
    INNER JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal;

    INSERT INTO dbo.DETALLE_VENTA(id_venta,id_producto,cantidad,precio_unitario,subtotal)
    SELECT v.id_venta,p.id_producto,d.cantidad,CAST(p.precio_venta AS DECIMAL(18,2)),
           CAST(p.precio_venta*d.cantidad AS DECIMAL(18,2))
    FROM @Ventas x
    INNER JOIN @Detalles d ON d.n=x.n
    INNER JOIN dbo.USUARIO u ON u.nombre_usuario=x.usuario
    INNER JOIN dbo.CLIENTE c ON c.documento=x.cliente
    INNER JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal
    INNER JOIN dbo.VENTA v ON v.id_usuario=u.id_usuario AND v.id_cliente=c.id_cliente
      AND v.id_sucursal=s.id_sucursal
      AND v.fecha_hora=DATEADD(MINUTE,x.minutos,DATEADD(DAY,x.dias,CAST(@Hoy AS DATETIME2)))
    INNER JOIN dbo.PRODUCTO p ON p.nombre=d.producto;

    DECLARE @Pagos TABLE(n INT,metodo NVARCHAR(100),proporcion DECIMAL(5,2),PRIMARY KEY(n,metodo));
    INSERT INTO @Pagos
      SELECT n,N'Efectivo',1 FROM @Ventas WHERE pago=1 UNION ALL
      SELECT n,N'Débito',1 FROM @Ventas WHERE pago=2 UNION ALL
      SELECT n,N'Crédito',1 FROM @Ventas WHERE pago=3 UNION ALL
      SELECT n,N'Transferencia',1 FROM @Ventas WHERE pago=4 UNION ALL
      SELECT n,N'Efectivo',0.40 FROM @Ventas WHERE pago=5 UNION ALL
      SELECT n,N'Transferencia',0.60 FROM @Ventas WHERE pago=5;

    INSERT INTO dbo.PAGO(id_venta,id_metodo_pago,monto)
    SELECT v.id_venta,mp.id_metodo_pago,
      CASE WHEN pg.proporcion=0.60
           THEN v.total-CAST(ROUND(v.total*0.40,2) AS DECIMAL(18,2))
           ELSE CAST(ROUND(v.total*pg.proporcion,2) AS DECIMAL(18,2)) END
    FROM @Ventas x
    INNER JOIN @Pagos pg ON pg.n=x.n
    INNER JOIN dbo.USUARIO u ON u.nombre_usuario=x.usuario
    INNER JOIN dbo.CLIENTE c ON c.documento=x.cliente
    INNER JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal
    INNER JOIN dbo.VENTA v ON v.id_usuario=u.id_usuario AND v.id_cliente=c.id_cliente
      AND v.id_sucursal=s.id_sucursal
      AND v.fecha_hora=DATEADD(MINUTE,x.minutos,DATEADD(DAY,x.dias,CAST(@Hoy AS DATETIME2)))
    INNER JOIN dbo.METODO_PAGO mp
      ON mp.nombre COLLATE Latin1_General_100_CI_AI=pg.metodo COLLATE Latin1_General_100_CI_AI
     AND mp.eliminado_en IS NULL;

    /* Stock actual: algunos productos quedan agotados/bajos para demostrar reportes. */
    ;WITH Consumo AS (
      SELECT v.id_sucursal,d.id_producto,SUM(d.cantidad) cantidad
      FROM dbo.VENTA v
      INNER JOIN dbo.DETALLE_VENTA d ON d.id_venta=v.id_venta AND d.eliminado_en IS NULL
      INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
      INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
      WHERE u.nombre_usuario IN(N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',
            N'vendedor_b1',N'vendedor_b2',N'vendedor_b3',
            N'vendedor_central1',N'vendedor_central2')
        AND c.documento BETWEEN N'98000101' AND N'98000140'
      GROUP BY v.id_sucursal,d.id_producto
    )
    UPDATE i SET
      i.stock=CASE
        WHEN pn.numero IN(3,11,19) AND s.nombre=N'Sucursal Prueba Centro'
          THEN CASE pn.numero WHEN 3 THEN 0 WHEN 11 THEN 3 ELSE 5 END
        WHEN pn.numero IN(2,8,21) AND s.nombre=N'Sucursal Prueba Norte'
          THEN CASE pn.numero WHEN 2 THEN 2 WHEN 8 THEN 0 ELSE 4 END
        WHEN pn.numero IN(5,14) AND s.nombre=N'Sucursal Central'
          THEN CASE pn.numero WHEN 5 THEN 1 ELSE 6 END
        ELSE 750+pn.numero*20-ISNULL(c.cantidad,0) END,
      i.stock_minimo=CASE WHEN pn.numero IN(2,3,5,8,11,14,19,21) THEN 10 ELSE 20 END,
      i.eliminado_en=NULL
    FROM dbo.INVENTARIO i
    INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=i.id_sucursal
    INNER JOIN dbo.PRODUCTO p ON p.id_producto=i.id_producto
    INNER JOIN @ProductosNum pn ON pn.nombre=p.nombre
    LEFT JOIN Consumo c ON c.id_sucursal=i.id_sucursal AND c.id_producto=i.id_producto;

    IF EXISTS (SELECT 1 FROM dbo.INVENTARIO WHERE stock<0)
      THROW 51007, 'El escenario genero stock negativo.', 1;

    /* ---------- 18 AVISOS ---------- */
    DECLARE @Avisos TABLE(
      titulo NVARCHAR(100) PRIMARY KEY,mensaje NVARCHAR(500),
      autor NVARCHAR(50),sucursal NVARCHAR(100) NULL,dias INT
    );
    INSERT INTO @Avisos VALUES
      (N'DEMO: Revisión semanal de indicadores',N'Revisar ventas, recaudación y productos destacados antes de la reunión semanal.',N'coordinador_global_test',NULL,-145),
      (N'DEMO: Objetivos comerciales del mes',N'Se actualizaron los objetivos mensuales. Revisar avance por sucursal y vendedor.',N'coordinador_global_test',NULL,-132),
      (N'DEMO: Control de inventario Centro',N'Realizar control de inventario y verificar diferencias antes del cierre.',N'gerente_a',N'Sucursal Prueba Centro',-121),
      (N'DEMO: Reposición prioritaria Norte',N'Priorizar la reposición de productos marcados con stock bajo.',N'gerente_b',N'Sucursal Prueba Norte',-110),
      (N'DEMO: Capacitación interna',N'Capacitación sobre registro de ventas y medios de pago el próximo viernes.',N'coordinador_global_test',NULL,-98),
      (N'DEMO: Cambio de horario Centro',N'El sábado la sucursal atenderá en horario reducido.',N'gerente_a',N'Sucursal Prueba Centro',-87),
      (N'DEMO: Revisión de stock mínimo',N'Controlar valores de stock mínimo de herramientas eléctricas.',N'gerente_b',N'Sucursal Prueba Norte',-76),
      (N'DEMO: Nuevo catálogo disponible',N'Ya está disponible el nuevo catálogo de productos para consulta.',N'coordinador_global_test',NULL,-65),
      (N'DEMO: Cierre mensual Centro',N'Completar controles pendientes antes del cierre mensual.',N'gerente_a',N'Sucursal Prueba Centro',-54),
      (N'DEMO: Cierre mensual Norte',N'Completar controles pendientes antes del cierre mensual.',N'gerente_b',N'Sucursal Prueba Norte',-53),
      (N'DEMO: Promoción herramientas',N'Informar a clientes sobre la promoción vigente en herramientas seleccionadas.',N'coordinador_global_test',NULL,-42),
      (N'DEMO: Seguimiento de productos agotados',N'Revisar productos agotados y solicitar reposición cuando corresponda.',N'gerente_a',N'Sucursal Prueba Centro',-31),
      (N'DEMO: Control preventivo Norte',N'Verificar inventario de seguridad antes del fin de semana.',N'gerente_b',N'Sucursal Prueba Norte',-24),
      (N'DEMO: Recordatorio de comprobantes',N'Verificar que cada venta quede asociada correctamente al cliente y forma de pago.',N'coordinador_global_test',NULL,-18),
      (N'DEMO: Reunión de equipo Centro',N'Reunión breve de seguimiento comercial antes de la apertura.',N'gerente_a',N'Sucursal Prueba Centro',-12),
      (N'DEMO: Reunión de equipo Norte',N'Reunión breve de seguimiento comercial antes de la apertura.',N'gerente_b',N'Sucursal Prueba Norte',-9),
      (N'DEMO: Revisión de indicadores',N'Revisar indicadores acumulados de los últimos meses para preparar la defensa.',N'coordinador_global_test',NULL,-4),
      (N'DEMO: Stock crítico',N'Atender primero los productos con stock en cero o por debajo del mínimo.',N'gerente_b',N'Sucursal Prueba Norte',-1);

    INSERT INTO dbo.AVISO(titulo,mensaje,fecha_creacion,id_usuario_autor,id_sucursal,activo)
    SELECT x.titulo,x.mensaje,DATEADD(HOUR,10,DATEADD(DAY,x.dias,CAST(@Hoy AS DATETIME2))),
           u.id_usuario,s.id_sucursal,1
    FROM @Avisos x
    INNER JOIN dbo.USUARIO u ON u.nombre_usuario=x.autor
    LEFT JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal;

    /* ---------- 90 MOVIMIENTOS DE AUDITORIA ---------- */
    ;WITH N AS (
      SELECT TOP (90) ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) n FROM sys.all_objects
    ), D AS (
      SELECT n,-((n*13)%151) dias,
        CASE n%9 WHEN 0 THEN N'coordinador_global_test' WHEN 1 THEN N'gerente_a'
          WHEN 2 THEN N'gerente_b' WHEN 3 THEN N'vendedor_a1' WHEN 4 THEN N'vendedor_a2'
          WHEN 5 THEN N'vendedor_b1' WHEN 6 THEN N'vendedor_b2'
          WHEN 7 THEN N'vendedor_central1' ELSE N'vendedor_central2' END usuario,
        CASE n%8 WHEN 0 THEN N'ALTA' WHEN 1 THEN N'MODIFICACION' WHEN 2 THEN N'ALTA'
          WHEN 3 THEN N'MODIFICACION' WHEN 4 THEN N'BAJA' WHEN 5 THEN N'REACTIVACION'
          WHEN 6 THEN N'BACKUP' ELSE N'ALTA' END accion,
        CASE n%8 WHEN 0 THEN N'VENTA' WHEN 1 THEN N'INVENTARIO' WHEN 2 THEN N'CLIENTE'
          WHEN 3 THEN N'PRODUCTO' WHEN 4 THEN N'CLIENTE' WHEN 5 THEN N'CLIENTE'
          WHEN 6 THEN N'BASE_DATOS' ELSE N'AVISO' END entidad
      FROM N
    )
    INSERT INTO dbo.AUDITORIA(id_usuario,fecha,accion,entidad,id_entidad,detalle,id_sucursal)
    SELECT u.id_usuario,
      DATEADD(MINUTE,540+((d.n*41)%600),DATEADD(DAY,d.dias,CAST(@Hoy AS DATETIME2))),
      d.accion,d.entidad,NULL,
      N'DEMO: '+CASE d.entidad
        WHEN N'VENTA' THEN N'Venta registrada y confirmada correctamente.'
        WHEN N'INVENTARIO' THEN N'Se actualizó el stock de un producto de la sucursal.'
        WHEN N'CLIENTE' THEN CASE d.accion WHEN N'BAJA' THEN N'Cliente dado de baja para verificar trazabilidad.'
          WHEN N'REACTIVACION' THEN N'Cliente reactivado luego de una baja de prueba.'
          ELSE N'Datos del cliente actualizados correctamente.' END
        WHEN N'PRODUCTO' THEN N'Precio, marca o datos del producto actualizados.'
        WHEN N'BASE_DATOS' THEN N'Copia de seguridad generada correctamente en el servidor.'
        WHEN N'AVISO' THEN N'Aviso interno publicado para el equipo.'
        ELSE N'Movimiento administrativo de prueba.' END
        +N' Ref. #'+CONVERT(NVARCHAR(10),d.n),
      u.id_sucursal
    FROM D d
    INNER JOIN dbo.USUARIO u ON u.nombre_usuario=d.usuario;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE()<>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

/* ---------- VERIFICACION RAPIDA ---------- */
SELECT N'Clientes de prueba' indicador,COUNT(*) cantidad
FROM dbo.CLIENTE WHERE documento BETWEEN N'98000101' AND N'98000140'
UNION ALL
SELECT N'Usuarios de prueba',COUNT(*) FROM dbo.USUARIO
WHERE nombre_usuario IN(N'coordinador_global_test',N'gerente_a',N'gerente_b',
 N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',N'vendedor_b1',N'vendedor_b2',
 N'vendedor_b3',N'vendedor_central1',N'vendedor_central2')
UNION ALL
SELECT N'Productos de prueba',COUNT(*) FROM dbo.PRODUCTO
WHERE codigo_barra BETWEEN N'7799900000011' AND N'7799900000035'
UNION ALL
SELECT N'Ventas de prueba',COUNT(*) FROM dbo.VENTA v
INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
WHERE u.nombre_usuario IN(N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',
            N'vendedor_b1',N'vendedor_b2',N'vendedor_b3',
            N'vendedor_central1',N'vendedor_central2')
  AND c.documento BETWEEN N'98000101' AND N'98000140'
UNION ALL
SELECT N'Avisos DEMO',COUNT(*) FROM dbo.AVISO WHERE titulo LIKE N'DEMO:%'
UNION ALL
SELECT N'Movimientos AUDITORIA DEMO',COUNT(*) FROM dbo.AUDITORIA WHERE detalle LIKE N'DEMO:%';

SELECT MIN(v.fecha_hora) primera_venta,MAX(v.fecha_hora) ultima_venta,
       COUNT(*) total_ventas,SUM(v.total) facturacion_demo
FROM dbo.VENTA v
INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
WHERE u.nombre_usuario IN(N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',
            N'vendedor_b1',N'vendedor_b2',N'vendedor_b3',
            N'vendedor_central1',N'vendedor_central2')
  AND c.documento BETWEEN N'98000101' AND N'98000140';

SELECT u.nombre_usuario vendedor,s.nombre sucursal,COUNT(*) ventas,SUM(v.total) facturacion
FROM dbo.VENTA v
INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario
INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente
INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=v.id_sucursal
WHERE u.nombre_usuario IN(N'vendedor_a1',N'vendedor_a2',N'vendedor_a3',
            N'vendedor_b1',N'vendedor_b2',N'vendedor_b3',
            N'vendedor_central1',N'vendedor_central2')
  AND c.documento BETWEEN N'98000101' AND N'98000140'
GROUP BY u.nombre_usuario,s.nombre
ORDER BY s.nombre,u.nombre_usuario;
GO
