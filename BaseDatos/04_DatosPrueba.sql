/* =========================================================
   SISTEMA DE GESTION - DATOS DE PRUEBA
   Requiere 01_Estructura.sql, 02_DatosIniciales.sql y
   03_Procedimientos.sql ejecutados previamente.

   Identificadores reservados para limpieza futura:
   usuarios *_test, gerente_a/b, vendedor_a1/a2/b1/b2;
   sucursales "Sucursal Prueba ..."; documentos 98000101-98000110;
   codigos de barra 7799900000011-7799900000016.
   ========================================================= */
USE SistemaGestion;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Hoy DATE = CONVERT(DATE, SYSDATETIME());
    DECLARE @HashPrueba NVARCHAR(255) = N'100000.U2lzdGVtYUdlc3Rpb25UZXN0IQ==.Xpot7xb3msPxNqukQW/1CuKSb2t/VS/n+0pXsHpHekc=';

    -- Verifica que las funcionalidades y el perfil global iniciales esten disponibles.
    IF NOT EXISTS (SELECT 1 FROM dbo.PERFIL WHERE alcance_global = 1 AND eliminado_en IS NULL)
        THROW 51001, 'Falta un perfil global activo. Ejecute 02_DatosIniciales.sql.', 1;

    IF EXISTS
    (
        SELECT 1 FROM
        (VALUES (N'VENTAS_REALIZAR'),(N'VENTAS_VER'),(N'CLIENTES_VER'),(N'CLIENTES_ALTA'),
                (N'PRODUCTOS_VER'),(N'AVISOS_VER'),(N'AVISOS_PUBLICAR'),(N'REPORTES_VER'),
                (N'REPORTES_VENTAS'),(N'REPORTES_RECAUDACION'),(N'REPORTES_PRODUCTOS'),
                (N'REPORTES_STOCK'),(N'REPORTES_RENDIMIENTO_VENDEDORES'),
                (N'REPORTES_DETALLE_VENTAS'),(N'REPORTES_EXPORTAR'),
                (N'REPORTES_ALCANCE_PROPIO'),(N'REPORTES_ALCANCE_SUCURSAL'),
                (N'REPORTES_GERENTE'),(N'REPORTES_VENDEDOR')) AS r(codigo)
        WHERE NOT EXISTS (SELECT 1 FROM dbo.FUNCIONALIDAD AS f WHERE f.codigo=r.codigo AND f.eliminado_en IS NULL)
    )
        THROW 51002, 'Faltan funcionalidades oficiales. Ejecute 02_DatosIniciales.sql.', 1;

    -- Evita generar ventas incompletas si falta un metodo de pago inicial.
    IF EXISTS
    (
        SELECT 1 FROM (VALUES(N'Efectivo'),(N'Debito'),(N'Credito'),(N'Transferencia')) AS r(nombre)
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.METODO_PAGO mp
            WHERE mp.nombre COLLATE Latin1_General_100_CI_AI=r.nombre COLLATE Latin1_General_100_CI_AI
              AND mp.eliminado_en IS NULL
        )
    )
        THROW 51003, 'Faltan metodos de pago activos. Ejecute 02_DatosIniciales.sql.', 1;

    -- Ubicacion y sucursales reservadas; solo se reactivan registros con nombres de prueba.
    IF NOT EXISTS (SELECT 1 FROM dbo.PROVINCIA WHERE nombre=N'Corrientes')
        INSERT INTO dbo.PROVINCIA(nombre) VALUES(N'Corrientes');
    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.LOCALIDAD l INNER JOIN dbo.PROVINCIA p ON p.id_provincia=l.id_provincia
        WHERE p.nombre=N'Corrientes' AND l.nombre=N'Corrientes'
    )
        INSERT INTO dbo.LOCALIDAD(id_provincia,nombre,codigo_postal)
        SELECT id_provincia,N'Corrientes',N'3400' FROM dbo.PROVINCIA WHERE nombre=N'Corrientes';

    DECLARE @Sucursales TABLE(nombre NVARCHAR(100) PRIMARY KEY, calle NVARCHAR(150), altura NVARCHAR(20), telefono NVARCHAR(30));
    INSERT INTO @Sucursales VALUES
        (N'Sucursal Prueba Centro',N'Pasaje de Prueba',N'100',N'+54 379 4000100'),
        (N'Sucursal Prueba Norte',N'Pasaje de Prueba',N'200',N'+54 379 4000200');

    INSERT INTO dbo.DIRECCION(id_localidad,calle,altura)
    SELECT l.id_localidad,x.calle,x.altura FROM @Sucursales x
    CROSS JOIN (SELECT TOP 1 l.id_localidad FROM dbo.LOCALIDAD l INNER JOIN dbo.PROVINCIA p ON p.id_provincia=l.id_provincia WHERE p.nombre=N'Corrientes' AND l.nombre=N'Corrientes') l
    WHERE NOT EXISTS(SELECT 1 FROM dbo.DIRECCION d WHERE d.id_localidad=l.id_localidad AND d.calle=x.calle AND d.altura=x.altura);

    UPDATE s SET s.id_direccion=d.id_direccion,s.telefono=x.telefono,s.eliminado_en=NULL
    FROM dbo.SUCURSAL s INNER JOIN @Sucursales x ON x.nombre=s.nombre
    INNER JOIN dbo.DIRECCION d ON d.calle=x.calle AND d.altura=x.altura
    INNER JOIN dbo.LOCALIDAD l ON l.id_localidad=d.id_localidad
    INNER JOIN dbo.PROVINCIA p ON p.id_provincia=l.id_provincia AND p.nombre=N'Corrientes';
    INSERT INTO dbo.SUCURSAL(nombre,telefono,id_direccion)
    SELECT x.nombre,x.telefono,d.id_direccion FROM @Sucursales x
    INNER JOIN dbo.DIRECCION d ON d.calle=x.calle AND d.altura=x.altura
    INNER JOIN dbo.LOCALIDAD l ON l.id_localidad=d.id_localidad
    INNER JOIN dbo.PROVINCIA p ON p.id_provincia=l.id_provincia AND p.nombre=N'Corrientes'
    WHERE NOT EXISTS(SELECT 1 FROM dbo.SUCURSAL s WHERE s.nombre=x.nombre);

    -- Perfiles de prueba: el alcance efectivo se determina por funcionalidades y alcance_global.
    DECLARE @Perfiles TABLE(nombre NVARCHAR(50) PRIMARY KEY,descripcion NVARCHAR(200));
    INSERT INTO @Perfiles VALUES
        (N'Prueba Gerencia',N'Perfil de prueba con alcance de reportes por sucursal.'),
        (N'Prueba Ventas',N'Perfil de prueba con alcance de reportes propio.');
    UPDATE p SET p.descripcion=x.descripcion,p.alcance_global=0,p.eliminado_en=NULL FROM dbo.PERFIL p INNER JOIN @Perfiles x ON x.nombre=p.nombre;
    INSERT INTO dbo.PERFIL(nombre,descripcion,alcance_global)
    SELECT nombre,descripcion,0 FROM @Perfiles x WHERE NOT EXISTS(SELECT 1 FROM dbo.PERFIL p WHERE p.nombre=x.nombre);

    DECLARE @Permisos TABLE(perfil NVARCHAR(50),codigo NVARCHAR(50),PRIMARY KEY(perfil,codigo));
    INSERT INTO @Permisos VALUES
        (N'Prueba Gerencia',N'VENTAS_VER'),(N'Prueba Gerencia',N'VENTAS_REALIZAR'),(N'Prueba Gerencia',N'CLIENTES_VER'),(N'Prueba Gerencia',N'CLIENTES_ALTA'),(N'Prueba Gerencia',N'PRODUCTOS_VER'),(N'Prueba Gerencia',N'AVISOS_VER'),(N'Prueba Gerencia',N'AVISOS_PUBLICAR'),
        (N'Prueba Gerencia',N'REPORTES_GERENTE'),(N'Prueba Gerencia',N'REPORTES_VER'),(N'Prueba Gerencia',N'REPORTES_VENTAS'),(N'Prueba Gerencia',N'REPORTES_RECAUDACION'),(N'Prueba Gerencia',N'REPORTES_PRODUCTOS'),(N'Prueba Gerencia',N'REPORTES_STOCK'),(N'Prueba Gerencia',N'REPORTES_RENDIMIENTO_VENDEDORES'),(N'Prueba Gerencia',N'REPORTES_DETALLE_VENTAS'),(N'Prueba Gerencia',N'REPORTES_EXPORTAR'),(N'Prueba Gerencia',N'REPORTES_ALCANCE_SUCURSAL'),
        (N'Prueba Ventas',N'VENTAS_VER'),(N'Prueba Ventas',N'VENTAS_REALIZAR'),(N'Prueba Ventas',N'CLIENTES_VER'),(N'Prueba Ventas',N'CLIENTES_ALTA'),(N'Prueba Ventas',N'PRODUCTOS_VER'),(N'Prueba Ventas',N'AVISOS_VER'),
        (N'Prueba Ventas',N'REPORTES_VENDEDOR'),(N'Prueba Ventas',N'REPORTES_VER'),(N'Prueba Ventas',N'REPORTES_VENTAS'),(N'Prueba Ventas',N'REPORTES_RECAUDACION'),(N'Prueba Ventas',N'REPORTES_PRODUCTOS'),(N'Prueba Ventas',N'REPORTES_DETALLE_VENTAS'),(N'Prueba Ventas',N'REPORTES_ALCANCE_PROPIO');
    INSERT INTO dbo.PERFIL_FUNCIONALIDAD(id_perfil,id_funcionalidad)
    SELECT p.id_perfil,f.id_funcionalidad FROM @Permisos x INNER JOIN dbo.PERFIL p ON p.nombre=x.perfil AND p.eliminado_en IS NULL INNER JOIN dbo.FUNCIONALIDAD f ON f.codigo=x.codigo AND f.eliminado_en IS NULL
    WHERE NOT EXISTS(SELECT 1 FROM dbo.PERFIL_FUNCIONALIDAD pf WHERE pf.id_perfil=p.id_perfil AND pf.id_funcionalidad=f.id_funcionalidad);

    INSERT INTO dbo.PERFIL_AVISO_DESTINO(id_perfil_emisor,id_perfil_destino)
    SELECT emisor.id_perfil,destino.id_perfil
    FROM (VALUES(N'Prueba Gerencia',N'Prueba Ventas')) x(emisor,destino)
    INNER JOIN dbo.PERFIL emisor ON emisor.nombre=x.emisor AND emisor.eliminado_en IS NULL
    INNER JOIN dbo.PERFIL destino ON destino.nombre=x.destino AND destino.eliminado_en IS NULL
    WHERE NOT EXISTS(SELECT 1 FROM dbo.PERFIL_AVISO_DESTINO pad WHERE pad.id_perfil_emisor=emisor.id_perfil AND pad.id_perfil_destino=destino.id_perfil);

    -- Usuarios legibles: todos usan la contrasena Prueba2026! con PBKDF2 valido.
    DECLARE @Usuarios TABLE(usuario NVARCHAR(50) PRIMARY KEY,nombre NVARCHAR(100),apellido NVARCHAR(100),dni NVARCHAR(20),correo NVARCHAR(150),telefono NVARCHAR(30),perfil NVARCHAR(50) NULL,sucursal NVARCHAR(100) NULL,es_global BIT);
    INSERT INTO @Usuarios VALUES
        (N'coordinador_global_test',N'Carla',N'Coordinadora Global',N'97000001',N'coordinador.global@test.local',N'+54 379 4100001',NULL,NULL,1),
        (N'gerente_a',N'Juan',N'Gerente A',N'97000010',N'juan.gerentea@test.local',N'+54 379 4100010',N'Prueba Gerencia',N'Sucursal Prueba Centro',0),
        (N'gerente_b',N'Ana',N'Gerente B',N'97000011',N'ana.gerenteb@test.local',N'+54 379 4100011',N'Prueba Gerencia',N'Sucursal Prueba Norte',0),
        (N'vendedor_a1',N'Pedro',N'Vendedor A1',N'97000020',N'pedro.vendedora1@test.local',N'+54 379 4100020',N'Prueba Ventas',N'Sucursal Prueba Centro',0),
        (N'vendedor_a2',N'Lucas',N'Vendedor A2',N'97000021',N'lucas.vendedora2@test.local',N'+54 379 4100021',N'Prueba Ventas',N'Sucursal Prueba Centro',0),
        (N'vendedor_b1',N'Maria',N'Vendedor B1',N'97000022',N'maria.vendedorab1@test.local',N'+54 379 4100022',N'Prueba Ventas',N'Sucursal Prueba Norte',0),
        (N'vendedor_b2',N'Diego',N'Vendedor B2',N'97000023',N'diego.vendedorb2@test.local',N'+54 379 4100023',N'Prueba Ventas',N'Sucursal Prueba Norte',0);
    IF EXISTS(SELECT 1 FROM @Usuarios x INNER JOIN dbo.USUARIO u ON (u.dni=x.dni OR u.correo=x.correo) AND u.nombre_usuario<>x.usuario)
        THROW 51003, 'Un DNI o correo reservado ya pertenece a otro usuario.', 1;
    UPDATE u SET u.id_perfil=COALESCE(p.id_perfil,pg.id_perfil),u.id_sucursal=CASE WHEN x.es_global=1 THEN NULL ELSE s.id_sucursal END,u.nombre=x.nombre,u.apellido=x.apellido,u.dni=x.dni,u.correo=x.correo,u.telefono=x.telefono,u.contrasena_hash=@HashPrueba,u.eliminado_en=NULL
    FROM dbo.USUARIO u INNER JOIN @Usuarios x ON x.usuario=u.nombre_usuario LEFT JOIN dbo.PERFIL p ON p.nombre=x.perfil AND p.eliminado_en IS NULL
    OUTER APPLY(SELECT TOP 1 id_perfil FROM dbo.PERFIL WHERE alcance_global=1 AND eliminado_en IS NULL ORDER BY id_perfil) pg LEFT JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal AND s.eliminado_en IS NULL;
    INSERT INTO dbo.USUARIO(id_perfil,id_sucursal,nombre,apellido,dni,telefono,nombre_usuario,contrasena_hash,correo)
    SELECT COALESCE(p.id_perfil,pg.id_perfil),CASE WHEN x.es_global=1 THEN NULL ELSE s.id_sucursal END,x.nombre,x.apellido,x.dni,x.telefono,x.usuario,@HashPrueba,x.correo
    FROM @Usuarios x LEFT JOIN dbo.PERFIL p ON p.nombre=x.perfil AND p.eliminado_en IS NULL
    OUTER APPLY(SELECT TOP 1 id_perfil FROM dbo.PERFIL WHERE alcance_global=1 AND eliminado_en IS NULL ORDER BY id_perfil) pg LEFT JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal AND s.eliminado_en IS NULL
    WHERE NOT EXISTS(SELECT 1 FROM dbo.USUARIO u WHERE u.nombre_usuario=x.usuario);
    IF EXISTS
    (
        SELECT 1 FROM @Usuarios x WHERE x.usuario LIKE N'vendedor_%' AND NOT EXISTS
        (SELECT 1 FROM dbo.USUARIO u INNER JOIN dbo.PERFIL_FUNCIONALIDAD pf ON pf.id_perfil=u.id_perfil INNER JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad=pf.id_funcionalidad WHERE u.nombre_usuario=x.usuario AND u.eliminado_en IS NULL AND f.codigo=N'VENTAS_REALIZAR' AND f.eliminado_en IS NULL)
    ) THROW 51004, 'Un usuario vendedor de prueba no posee VENTAS_REALIZAR.', 1;

    -- Clientes reservados con direccion propia, todos validos y activos.
    DECLARE @Clientes TABLE(documento NVARCHAR(20) PRIMARY KEY,nombre NVARCHAR(100),apellido NVARCHAR(100),correo NVARCHAR(150),telefono NVARCHAR(30),altura NVARCHAR(20));
    INSERT INTO @Clientes VALUES
        (N'98000101',N'Ana',N'Martinez',N'ana.martinez@test.local',N'+54 379 4200101',N'101'),(N'98000102',N'Bruno',N'Lopez',N'bruno.lopez@test.local',N'+54 379 4200102',N'102'),
        (N'98000103',N'Carolina',N'Gomez',N'carolina.gomez@test.local',N'+54 379 4200103',N'103'),(N'98000104',N'Daniel',N'Fernandez',N'daniel.fernandez@test.local',N'+54 379 4200104',N'104'),
        (N'98000105',N'Elena',N'Rodriguez',N'elena.rodriguez@test.local',N'+54 379 4200105',N'105'),(N'98000106',N'Federico',N'Benitez',N'federico.benitez@test.local',N'+54 379 4200106',N'106'),
        (N'98000107',N'Gabriela',N'Torres',N'gabriela.torres@test.local',N'+54 379 4200107',N'107'),(N'98000108',N'Hernan',N'Acosta',N'hernan.acosta@test.local',N'+54 379 4200108',N'108'),
        (N'98000109',N'Ines',N'Ramos',N'ines.ramos@test.local',N'+54 379 4200109',N'109'),(N'98000110',N'Javier',N'Sosa',N'javier.sosa@test.local',N'+54 379 4200110',N'110');
    INSERT INTO dbo.DIRECCION(id_localidad,calle,altura)
    SELECT l.id_localidad,N'Cliente Prueba',x.altura FROM @Clientes x
    CROSS JOIN(SELECT TOP 1 l.id_localidad FROM dbo.LOCALIDAD l INNER JOIN dbo.PROVINCIA p ON p.id_provincia=l.id_provincia WHERE p.nombre=N'Corrientes' AND l.nombre=N'Corrientes') l
    WHERE NOT EXISTS(SELECT 1 FROM dbo.DIRECCION d WHERE d.id_localidad=l.id_localidad AND d.calle=N'Cliente Prueba' AND d.altura=x.altura);
    UPDATE c SET c.nombre=x.nombre,c.apellido=x.apellido,c.correo=x.correo,c.telefono=x.telefono,c.id_direccion=d.id_direccion,c.eliminado_en=NULL
    FROM dbo.CLIENTE c INNER JOIN @Clientes x ON x.documento=c.documento INNER JOIN dbo.DIRECCION d ON d.calle=N'Cliente Prueba' AND d.altura=x.altura;
    INSERT INTO dbo.CLIENTE(nombre,apellido,documento,correo,telefono,id_direccion)
    SELECT x.nombre,x.apellido,x.documento,x.correo,x.telefono,d.id_direccion FROM @Clientes x INNER JOIN dbo.DIRECCION d ON d.calle=N'Cliente Prueba' AND d.altura=x.altura
    WHERE NOT EXISTS(SELECT 1 FROM dbo.CLIENTE c WHERE c.documento=x.documento);

    -- Subconjunto del catálogo histórico, con códigos reservados si aún no existe el producto.
    DECLARE @Productos TABLE(nombre NVARCHAR(100) PRIMARY KEY,categoria NVARCHAR(100),marca NVARCHAR(100),codigo NVARCHAR(50),costo DECIMAL(18,2),ganancia DECIMAL(5,2));
    INSERT INTO @Productos VALUES
        (N'Amoladora Angular 900W',N'Ferretería',N'Bosch',N'7799900000011',60000,40),(N'Soldadora Inverter 220A',N'Herrería',N'Lusqtoff',N'7799900000012',85000,40),
        (N'Guantes de Seguridad Industrial',N'Ferretería',N'Ingco',N'7799900000013',15000,40),(N'Atornillador Inalámbrico',N'Construcción',N'Total',N'7799900000014',70000,40),
        (N'Cinta Métrica 8m',N'Construcción',N'Bremen',N'7799900000015',57000,40),(N'Martillo de Uña',N'Ferretería',N'Total',N'7799900000016',67000,40);
    INSERT INTO dbo.CATEGORIA(nombre) SELECT DISTINCT categoria FROM @Productos x WHERE NOT EXISTS(SELECT 1 FROM dbo.CATEGORIA c WHERE c.nombre=x.categoria);
    INSERT INTO dbo.MARCA(nombre) SELECT DISTINCT marca FROM @Productos x WHERE NOT EXISTS(SELECT 1 FROM dbo.MARCA m WHERE m.nombre=x.marca);
    INSERT INTO dbo.PRODUCTO(id_categoria,id_marca,codigo_barra,nombre,descripcion,precio_costo,porcentaje_ganancia,activo)
    SELECT c.id_categoria,m.id_marca,x.codigo,x.nombre,N'Producto incluido para el escenario de pruebas.',x.costo,x.ganancia,1 FROM @Productos x INNER JOIN dbo.CATEGORIA c ON c.nombre=x.categoria INNER JOIN dbo.MARCA m ON m.nombre=x.marca
    WHERE NOT EXISTS(SELECT 1 FROM dbo.PRODUCTO p WHERE p.codigo_barra=x.codigo) AND NOT EXISTS(SELECT 1 FROM dbo.PRODUCTO p WHERE p.nombre=x.nombre);
    IF EXISTS(SELECT 1 FROM @Productos x WHERE NOT EXISTS(SELECT 1 FROM dbo.PRODUCTO p WHERE p.nombre=x.nombre AND p.activo=1 AND p.eliminado_en IS NULL))
        THROW 51005, 'No se pudo resolver un producto activo del catalogo de prueba.', 1;

    -- Stock por sucursal: valores altos para ventas y tres alertas de stock bajo.
    DECLARE @Stock TABLE(sucursal NVARCHAR(100),producto NVARCHAR(100),inicial INT,minimo INT,PRIMARY KEY(sucursal,producto));
    INSERT INTO @Stock VALUES
        (N'Sucursal Prueba Centro',N'Amoladora Angular 900W',80,5),(N'Sucursal Prueba Centro',N'Soldadora Inverter 220A',35,4),(N'Sucursal Prueba Centro',N'Guantes de Seguridad Industrial',20,10),
        (N'Sucursal Prueba Centro',N'Atornillador Inalámbrico',45,5),(N'Sucursal Prueba Centro',N'Cinta Métrica 8m',65,5),(N'Sucursal Prueba Centro',N'Martillo de Uña',55,5),
        (N'Sucursal Prueba Norte',N'Amoladora Angular 900W',60,5),(N'Sucursal Prueba Norte',N'Soldadora Inverter 220A',7,8),(N'Sucursal Prueba Norte',N'Guantes de Seguridad Industrial',45,5),
        (N'Sucursal Prueba Norte',N'Atornillador Inalámbrico',35,5),(N'Sucursal Prueba Norte',N'Cinta Métrica 8m',25,8),(N'Sucursal Prueba Norte',N'Martillo de Uña',40,5);
    UPDATE i SET i.stock=x.inicial,i.stock_minimo=x.minimo,i.eliminado_en=NULL FROM dbo.INVENTARIO i INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=i.id_sucursal INNER JOIN dbo.PRODUCTO p ON p.id_producto=i.id_producto INNER JOIN @Stock x ON x.sucursal=s.nombre AND x.producto=p.nombre;
    INSERT INTO dbo.INVENTARIO(id_producto,id_sucursal,stock,stock_minimo)
    SELECT p.id_producto,s.id_sucursal,x.inicial,x.minimo FROM @Stock x INNER JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal INNER JOIN dbo.PRODUCTO p ON p.nombre=x.producto
    WHERE NOT EXISTS(SELECT 1 FROM dbo.INVENTARIO i WHERE i.id_producto=p.id_producto AND i.id_sucursal=s.id_sucursal);

    -- 36 ventas distribuidas en los ultimos 30 dias, con ventas hoy y pagos variados.
    DECLARE @Ventas TABLE(n INT PRIMARY KEY,dias INT,minutos INT,sucursal NVARCHAR(100),usuario NVARCHAR(50),cliente NVARCHAR(20),descuento DECIMAL(5,2),pago TINYINT);
    INSERT INTO @Ventas VALUES
    (1,-29,570,N'Sucursal Prueba Centro',N'vendedor_a1',N'98000101',0,1),(2,-28,630,N'Sucursal Prueba Norte',N'vendedor_b1',N'98000102',0,2),(3,-27,690,N'Sucursal Prueba Centro',N'vendedor_a2',N'98000103',5,5),(4,-26,750,N'Sucursal Prueba Norte',N'vendedor_b2',N'98000104',0,3),
    (5,-25,810,N'Sucursal Prueba Centro',N'vendedor_a1',N'98000105',10,4),(6,-24,870,N'Sucursal Prueba Norte',N'vendedor_b1',N'98000106',0,1),(7,-23,930,N'Sucursal Prueba Centro',N'vendedor_a2',N'98000107',0,2),(8,-22,990,N'Sucursal Prueba Norte',N'vendedor_b2',N'98000108',5,5),
    (9,-21,1050,N'Sucursal Prueba Centro',N'vendedor_a1',N'98000109',0,3),(10,-20,1110,N'Sucursal Prueba Norte',N'vendedor_b1',N'98000110',0,4),(11,-19,600,N'Sucursal Prueba Centro',N'vendedor_a2',N'98000101',5,1),(12,-18,660,N'Sucursal Prueba Norte',N'vendedor_b2',N'98000102',0,2),
    (13,-17,720,N'Sucursal Prueba Centro',N'vendedor_a1',N'98000103',0,5),(14,-16,780,N'Sucursal Prueba Norte',N'vendedor_b1',N'98000104',10,3),(15,-15,840,N'Sucursal Prueba Centro',N'vendedor_a2',N'98000105',0,4),(16,-14,900,N'Sucursal Prueba Norte',N'vendedor_b2',N'98000106',0,1),
    (17,-13,960,N'Sucursal Prueba Centro',N'vendedor_a1',N'98000107',5,2),(18,-12,1020,N'Sucursal Prueba Norte',N'vendedor_b1',N'98000108',0,5),(19,-11,1080,N'Sucursal Prueba Centro',N'vendedor_a2',N'98000109',0,3),(20,-10,1140,N'Sucursal Prueba Norte',N'vendedor_b2',N'98000110',10,4),
    (21,-9,585,N'Sucursal Prueba Centro',N'vendedor_a1',N'98000102',0,1),(22,-8,645,N'Sucursal Prueba Norte',N'vendedor_b1',N'98000103',0,2),(23,-7,705,N'Sucursal Prueba Centro',N'vendedor_a2',N'98000104',5,5),(24,-6,765,N'Sucursal Prueba Norte',N'vendedor_b2',N'98000105',0,3),
    (25,-5,825,N'Sucursal Prueba Centro',N'vendedor_a1',N'98000106',0,4),(26,-4,885,N'Sucursal Prueba Norte',N'vendedor_b1',N'98000107',10,1),(27,-3,945,N'Sucursal Prueba Centro',N'vendedor_a2',N'98000108',0,2),(28,-2,1005,N'Sucursal Prueba Norte',N'vendedor_b2',N'98000109',5,5),
    (29,-1,1065,N'Sucursal Prueba Centro',N'vendedor_a1',N'98000110',0,3),(30,-1,1125,N'Sucursal Prueba Norte',N'vendedor_b1',N'98000101',0,4),(31,0,570,N'Sucursal Prueba Centro',N'vendedor_a1',N'98000102',5,1),(32,0,630,N'Sucursal Prueba Centro',N'vendedor_a2',N'98000103',0,2),
    (33,0,690,N'Sucursal Prueba Norte',N'vendedor_b1',N'98000104',0,5),(34,0,750,N'Sucursal Prueba Norte',N'vendedor_b2',N'98000105',10,3),(35,0,810,N'Sucursal Prueba Centro',N'vendedor_a1',N'98000106',0,4),(36,0,870,N'Sucursal Prueba Norte',N'vendedor_b1',N'98000107',0,1);
    DECLARE @Detalles TABLE(n INT,producto NVARCHAR(100),cantidad INT,PRIMARY KEY(n,producto));
    INSERT INTO @Detalles SELECT n,N'Amoladora Angular 900W',CASE WHEN n%3=0 THEN 2 ELSE 1 END FROM @Ventas;
    INSERT INTO @Detalles SELECT n,N'Cinta Métrica 8m',1 FROM @Ventas WHERE n%2=0;
    INSERT INTO @Detalles SELECT n,N'Guantes de Seguridad Industrial',2 FROM @Ventas WHERE n%3=0;
    INSERT INTO @Detalles SELECT n,N'Atornillador Inalámbrico',1 FROM @Ventas WHERE n%4=0;
    INSERT INTO @Detalles SELECT n,N'Soldadora Inverter 220A',1 FROM @Ventas WHERE n%5=0;
    INSERT INTO @Detalles SELECT n,N'Martillo de Uña',1 FROM @Ventas WHERE n%6=0;

    -- Las entidades reservadas son la marca indirecta que evita duplicar el lote completo.
    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.VENTA v INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=v.id_sucursal
        WHERE u.nombre_usuario IN(N'vendedor_a1',N'vendedor_a2',N'vendedor_b1',N'vendedor_b2') AND c.documento BETWEEN N'98000101' AND N'98000110' AND s.nombre IN(N'Sucursal Prueba Centro',N'Sucursal Prueba Norte') AND v.eliminado_en IS NULL
    )
    BEGIN
        ;WITH Totales AS
        (
            SELECT d.n,CAST(SUM(p.precio_venta*d.cantidad) AS DECIMAL(18,2)) subtotal FROM @Detalles d INNER JOIN dbo.PRODUCTO p ON p.nombre=d.producto GROUP BY d.n
        )
        INSERT INTO dbo.VENTA(id_cliente,id_usuario,id_sucursal,fecha_hora,tipo_factura,subtotal,descuento,total)
        SELECT c.id_cliente,u.id_usuario,s.id_sucursal,DATEADD(MINUTE,x.minutos,DATEADD(DAY,x.dias,CAST(@Hoy AS DATETIME2))),NULL,t.subtotal,
               CAST(ROUND(t.subtotal*x.descuento/100.0,2) AS DECIMAL(18,2)),CAST(t.subtotal-ROUND(t.subtotal*x.descuento/100.0,2) AS DECIMAL(18,2))
        FROM @Ventas x INNER JOIN Totales t ON t.n=x.n INNER JOIN dbo.CLIENTE c ON c.documento=x.cliente AND c.eliminado_en IS NULL INNER JOIN dbo.USUARIO u ON u.nombre_usuario=x.usuario AND u.eliminado_en IS NULL INNER JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal AND s.eliminado_en IS NULL
        WHERE EXISTS(SELECT 1 FROM dbo.PERFIL_FUNCIONALIDAD pf INNER JOIN dbo.FUNCIONALIDAD f ON f.id_funcionalidad=pf.id_funcionalidad WHERE pf.id_perfil=u.id_perfil AND f.codigo=N'VENTAS_REALIZAR' AND f.eliminado_en IS NULL);

        INSERT INTO dbo.DETALLE_VENTA(id_venta,id_producto,cantidad,precio_unitario,subtotal)
        SELECT v.id_venta,p.id_producto,d.cantidad,CAST(p.precio_venta AS DECIMAL(18,2)),CAST(p.precio_venta*d.cantidad AS DECIMAL(18,2)) FROM @Ventas x INNER JOIN @Detalles d ON d.n=x.n INNER JOIN dbo.USUARIO u ON u.nombre_usuario=x.usuario INNER JOIN dbo.CLIENTE c ON c.documento=x.cliente INNER JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal
        INNER JOIN dbo.VENTA v ON v.id_usuario=u.id_usuario AND v.id_cliente=c.id_cliente AND v.id_sucursal=s.id_sucursal AND v.fecha_hora=DATEADD(MINUTE,x.minutos,DATEADD(DAY,x.dias,CAST(@Hoy AS DATETIME2))) INNER JOIN dbo.PRODUCTO p ON p.nombre=d.producto;

        DECLARE @Pagos TABLE(n INT,metodo NVARCHAR(100),proporcion DECIMAL(5,2),PRIMARY KEY(n,metodo));
        INSERT INTO @Pagos SELECT n,N'Efectivo',1 FROM @Ventas WHERE pago=1 UNION ALL SELECT n,N'Debito',1 FROM @Ventas WHERE pago=2 UNION ALL SELECT n,N'Credito',1 FROM @Ventas WHERE pago=3 UNION ALL SELECT n,N'Transferencia',1 FROM @Ventas WHERE pago=4 UNION ALL SELECT n,N'Efectivo',.5 FROM @Ventas WHERE pago=5 UNION ALL SELECT n,N'Credito',.5 FROM @Ventas WHERE pago=5;
        INSERT INTO dbo.PAGO(id_venta,id_metodo_pago,monto)
        SELECT v.id_venta,mp.id_metodo_pago,CAST(ROUND(v.total*pg.proporcion,2) AS DECIMAL(18,2)) FROM @Ventas x INNER JOIN @Pagos pg ON pg.n=x.n INNER JOIN dbo.USUARIO u ON u.nombre_usuario=x.usuario INNER JOIN dbo.CLIENTE c ON c.documento=x.cliente INNER JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal
        INNER JOIN dbo.VENTA v ON v.id_usuario=u.id_usuario AND v.id_cliente=c.id_cliente AND v.id_sucursal=s.id_sucursal AND v.fecha_hora=DATEADD(MINUTE,x.minutos,DATEADD(DAY,x.dias,CAST(@Hoy AS DATETIME2)))
        INNER JOIN dbo.METODO_PAGO mp ON mp.nombre COLLATE Latin1_General_100_CI_AI=pg.metodo COLLATE Latin1_General_100_CI_AI AND mp.eliminado_en IS NULL;
    END;

    -- Resta todas las ventas reservadas del stock de sus sucursales, manteniendo el resultado estable al reejecutar.
    ;WITH Consumo AS
    (
        SELECT v.id_sucursal,d.id_producto,SUM(d.cantidad) cantidad FROM dbo.VENTA v INNER JOIN dbo.DETALLE_VENTA d ON d.id_venta=v.id_venta AND d.eliminado_en IS NULL INNER JOIN dbo.USUARIO u ON u.id_usuario=v.id_usuario INNER JOIN dbo.CLIENTE c ON c.id_cliente=v.id_cliente INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=v.id_sucursal
        WHERE v.eliminado_en IS NULL AND u.nombre_usuario IN(N'vendedor_a1',N'vendedor_a2',N'vendedor_b1',N'vendedor_b2') AND c.documento BETWEEN N'98000101' AND N'98000110' AND s.nombre IN(N'Sucursal Prueba Centro',N'Sucursal Prueba Norte') GROUP BY v.id_sucursal,d.id_producto
    )
    UPDATE i SET i.stock=x.inicial-ISNULL(c.cantidad,0),i.stock_minimo=x.minimo,i.eliminado_en=NULL FROM dbo.INVENTARIO i INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=i.id_sucursal INNER JOIN dbo.PRODUCTO p ON p.id_producto=i.id_producto INNER JOIN @Stock x ON x.sucursal=s.nombre AND x.producto=p.nombre LEFT JOIN Consumo c ON c.id_sucursal=i.id_sucursal AND c.id_producto=i.id_producto;
    IF EXISTS(SELECT 1 FROM dbo.INVENTARIO i INNER JOIN dbo.SUCURSAL s ON s.id_sucursal=i.id_sucursal WHERE s.nombre IN(N'Sucursal Prueba Centro',N'Sucursal Prueba Norte') AND i.stock<0)
        THROW 51006, 'Las ventas de prueba exceden el stock inicial.', 1;

    -- Avisos de prueba por perfiles destino configurados dinámicamente.
    DECLARE @Avisos TABLE(titulo NVARCHAR(100) PRIMARY KEY,mensaje NVARCHAR(500),autor NVARCHAR(50),sucursal NVARCHAR(100) NULL,destino NVARCHAR(50));
    INSERT INTO @Avisos VALUES
        (N'Aviso prueba global',N'Revisar los indicadores semanales de ambas sucursales.',N'coordinador_global_test',NULL,N'Prueba Gerencia'),
        (N'Aviso prueba Centro',N'Priorizar la reposicion de guantes de seguridad.',N'gerente_a',N'Sucursal Prueba Centro',N'Prueba Ventas'),
        (N'Aviso prueba Norte',N'Controlar el stock bajo antes del proximo cierre.',N'gerente_b',N'Sucursal Prueba Norte',N'Prueba Ventas');
    INSERT INTO dbo.AVISO(titulo,mensaje,id_usuario_autor,id_sucursal,activo)
    SELECT x.titulo,x.mensaje,u.id_usuario,s.id_sucursal,1 FROM @Avisos x INNER JOIN dbo.USUARIO u ON u.nombre_usuario=x.autor AND u.eliminado_en IS NULL LEFT JOIN dbo.SUCURSAL s ON s.nombre=x.sucursal AND s.eliminado_en IS NULL
    WHERE NOT EXISTS(SELECT 1 FROM dbo.AVISO a WHERE a.titulo=x.titulo AND a.id_usuario_autor=u.id_usuario AND a.eliminado_en IS NULL);

    INSERT INTO dbo.AVISO_PERFIL_DESTINO(id_aviso,id_perfil)
    SELECT a.id_aviso,p.id_perfil FROM dbo.AVISO a INNER JOIN @Avisos x ON x.titulo=a.titulo INNER JOIN dbo.PERFIL p ON p.nombre=x.destino AND p.eliminado_en IS NULL
    WHERE NOT EXISTS(SELECT 1 FROM dbo.AVISO_PERFIL_DESTINO apd WHERE apd.id_aviso=a.id_aviso AND apd.id_perfil=p.id_perfil);

    UPDATE a SET a.activo=1,a.eliminado_en=NULL
    FROM dbo.AVISO a INNER JOIN @Avisos x ON x.titulo=a.titulo
    INNER JOIN dbo.USUARIO u ON u.id_usuario=a.id_usuario_autor AND u.nombre_usuario=x.autor;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

/* 05_ResetBasePruebas.sql puede identificar este escenario
   por los nombres y rangos reservados documentados al inicio. */
