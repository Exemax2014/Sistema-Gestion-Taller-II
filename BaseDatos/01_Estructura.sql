/* =========================================================
   SISTEMA DE GESTIÓN - TALLER DE PROGRAMACIÓN II
   Script 01 - Estructura de Base de Datos
   Versión idempotente / actualizable

   Objetivo:
   - Crear la base si no existe.
   - Crear tablas únicamente si no existen.
   - Adaptar PRODUCTO si la base ya existía.
   - Agregar MARCA.
   - Mantener el stock por PRODUCTO + SUCURSAL mediante INVENTARIO.
   - Preparar los tipos de tabla usados para registrar ventas con múltiples ítems y pagos.
   - Evitar errores al ejecutar este script más de una vez.
   ========================================================= */

IF DB_ID('SistemaGestion') IS NULL
BEGIN
    CREATE DATABASE SistemaGestion;
END;
GO

/* Crea la base destino en una instalacion limpia sin afectar una existente. */
IF DB_ID(N'SistemaGestion') IS NULL
BEGIN
    CREATE DATABASE SistemaGestion;
END;
GO

USE SistemaGestion;
GO


/* ========================
   PROVINCIA
   ======================== */

IF OBJECT_ID('dbo.PROVINCIA', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PROVINCIA
    (
        id_provincia INT IDENTITY(1,1) PRIMARY KEY,
        nombre NVARCHAR(100) NOT NULL,
        eliminado_en DATETIME2 NULL,

        CONSTRAINT UQ_PROVINCIA_nombre
            UNIQUE (nombre)
    );
END;
GO


/* ========================
   LOCALIDAD
   ======================== */

IF OBJECT_ID('dbo.LOCALIDAD', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.LOCALIDAD
    (
        id_localidad INT IDENTITY(1,1) PRIMARY KEY,
        id_provincia INT NOT NULL,

        nombre NVARCHAR(100) NOT NULL,
        codigo_postal NVARCHAR(20) NULL,
        eliminado_en DATETIME2 NULL,

        CONSTRAINT FK_LOCALIDAD_PROVINCIA
            FOREIGN KEY (id_provincia)
            REFERENCES dbo.PROVINCIA(id_provincia),

        CONSTRAINT UQ_LOCALIDAD_PROVINCIA_NOMBRE
            UNIQUE (id_provincia, nombre)
    );
END;
GO


/* ========================
   DIRECCION
   ======================== */

IF OBJECT_ID('dbo.DIRECCION', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.DIRECCION
    (
        id_direccion INT IDENTITY(1,1) PRIMARY KEY,
        id_localidad INT NOT NULL,

        calle NVARCHAR(150) NOT NULL,
        altura NVARCHAR(20) NULL,
        eliminado_en DATETIME2 NULL,

        CONSTRAINT FK_DIRECCION_LOCALIDAD
            FOREIGN KEY (id_localidad)
            REFERENCES dbo.LOCALIDAD(id_localidad)
    );
END;
GO


/* ========================
   PERFIL
   ======================== */

IF OBJECT_ID('dbo.PERFIL', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PERFIL
    (
        id_perfil INT IDENTITY(1,1) PRIMARY KEY,
        nombre NVARCHAR(50) NOT NULL,
        descripcion NVARCHAR(200) NULL,

        /* Indica si los usuarios de este perfil trabajan a nivel global.
           1 = no requieren una sucursal fija.
           0 = deben tener una sucursal asignada. */
        alcance_global BIT NOT NULL
            CONSTRAINT DF_PERFIL_alcance_global DEFAULT (0),

        eliminado_en DATETIME2 NULL,

        CONSTRAINT UQ_PERFIL_nombre
            UNIQUE (nombre)
    );
END;
GO


/* =========================================================
   ADAPTACIÓN PERFIL - ALCANCE DE SUCURSAL
   =========================================================
   Permite que la regla de sucursal dependa de un dato del perfil
   y no del nombre hardcodeado "Administrador".

   Se agrega con valor 0 para conservar de forma segura los perfiles
   existentes. El Script 02 asignará los valores iniciales correctos.
   ========================================================= */

IF OBJECT_ID('dbo.PERFIL', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.PERFIL', 'alcance_global') IS NULL
BEGIN
    ALTER TABLE dbo.PERFIL
    ADD alcance_global BIT NOT NULL
        CONSTRAINT DF_PERFIL_alcance_global DEFAULT (0) WITH VALUES;
END;
GO


/* ========================
   FUNCIONALIDAD
   ======================== */

IF OBJECT_ID('dbo.FUNCIONALIDAD', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.FUNCIONALIDAD
    (
        id_funcionalidad INT IDENTITY(1,1) PRIMARY KEY,
        codigo NVARCHAR(50) NOT NULL,
        nombre NVARCHAR(100) NOT NULL,
        descripcion NVARCHAR(200) NULL,
        eliminado_en DATETIME2 NULL,

        CONSTRAINT UQ_FUNCIONALIDAD_codigo
            UNIQUE (codigo)
    );
END;
GO


/* ========================
   SUCURSAL
   ======================== */

IF OBJECT_ID('dbo.SUCURSAL', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SUCURSAL
    (
        id_sucursal INT IDENTITY(1,1) PRIMARY KEY,

        nombre NVARCHAR(100) NOT NULL,
        telefono NVARCHAR(30) NULL,
        eliminado_en DATETIME2 NULL,

        id_direccion INT NULL,

        CONSTRAINT FK_SUCURSAL_DIRECCION
            FOREIGN KEY (id_direccion)
            REFERENCES dbo.DIRECCION(id_direccion)
    );
END;
GO


/* ========================
   CATEGORIA
   ======================== */

IF OBJECT_ID('dbo.CATEGORIA', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CATEGORIA
    (
        id_categoria INT IDENTITY(1,1) PRIMARY KEY,
        nombre NVARCHAR(100) NOT NULL,
        descripcion NVARCHAR(200) NULL,
        eliminado_en DATETIME2 NULL,

        CONSTRAINT UQ_CATEGORIA_nombre
            UNIQUE (nombre)
    );
END;
GO


/* ========================
   MARCA
   ======================== */

IF OBJECT_ID('dbo.MARCA', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.MARCA
    (
        id_marca INT IDENTITY(1,1) PRIMARY KEY,
        nombre NVARCHAR(100) NOT NULL,
        eliminado_en DATETIME2 NULL,

        CONSTRAINT UQ_MARCA_nombre
            UNIQUE (nombre)
    );
END;
GO

/* Si MARCA ya existía pero no tenía la restricción UNIQUE por nombre,
   se agrega siempre que no existan nombres duplicados. */
IF OBJECT_ID('dbo.MARCA', 'U') IS NOT NULL
   AND NOT EXISTS
   (
       SELECT 1
       FROM sys.key_constraints
       WHERE parent_object_id = OBJECT_ID('dbo.MARCA')
         AND name = 'UQ_MARCA_nombre'
   )
   AND NOT EXISTS
   (
       SELECT nombre
       FROM dbo.MARCA
       GROUP BY nombre
       HAVING COUNT(*) > 1
   )
BEGIN
    ALTER TABLE dbo.MARCA
    ADD CONSTRAINT UQ_MARCA_nombre UNIQUE (nombre);
END;
GO


/* ========================
   METODO_PAGO
   ======================== */

IF OBJECT_ID('dbo.METODO_PAGO', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.METODO_PAGO
    (
        id_metodo_pago INT IDENTITY(1,1) PRIMARY KEY,
        nombre NVARCHAR(100) NOT NULL,
        descripcion NVARCHAR(200) NULL,
        eliminado_en DATETIME2 NULL,

        CONSTRAINT UQ_METODO_PAGO_nombre
            UNIQUE (nombre)
    );
END;
GO


/* ========================
   PERFIL_FUNCIONALIDAD
   ======================== */

IF OBJECT_ID('dbo.PERFIL_FUNCIONALIDAD', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PERFIL_FUNCIONALIDAD
    (
        id_perfil INT NOT NULL,
        id_funcionalidad INT NOT NULL,

        CONSTRAINT PK_PERFIL_FUNCIONALIDAD
            PRIMARY KEY (id_perfil, id_funcionalidad),

        CONSTRAINT FK_PERFIL_FUNCIONALIDAD_PERFIL
            FOREIGN KEY (id_perfil)
            REFERENCES dbo.PERFIL(id_perfil),

        CONSTRAINT FK_PERFIL_FUNCIONALIDAD_FUNCIONALIDAD
            FOREIGN KEY (id_funcionalidad)
            REFERENCES dbo.FUNCIONALIDAD(id_funcionalidad)
    );
END;
GO


/* ========================
   USUARIO
   ======================== */

IF OBJECT_ID('dbo.USUARIO', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.USUARIO
    (
        id_usuario INT IDENTITY(1,1) PRIMARY KEY,

        id_perfil INT NOT NULL,
        id_sucursal INT NULL,

        nombre NVARCHAR(100) NOT NULL,
        apellido NVARCHAR(100) NOT NULL,
        dni NVARCHAR(20) NOT NULL,
        telefono NVARCHAR(30) NULL,
        nombre_usuario NVARCHAR(50) NOT NULL,
        contrasena_hash NVARCHAR(255) NOT NULL,
        correo NVARCHAR(150) NOT NULL,
        sexo NVARCHAR(20) NULL,
        fecha_nacimiento DATE NULL,
        eliminado_en DATETIME2 NULL,

        id_direccion INT NULL,

        CONSTRAINT UQ_USUARIO_dni
            UNIQUE (dni),

        CONSTRAINT UQ_USUARIO_nombre_usuario
            UNIQUE (nombre_usuario),

        CONSTRAINT UQ_USUARIO_correo
            UNIQUE (correo),

        CONSTRAINT FK_USUARIO_PERFIL
            FOREIGN KEY (id_perfil)
            REFERENCES dbo.PERFIL(id_perfil),

        CONSTRAINT FK_USUARIO_SUCURSAL
            FOREIGN KEY (id_sucursal)
            REFERENCES dbo.SUCURSAL(id_sucursal),

        CONSTRAINT FK_USUARIO_DIRECCION
            FOREIGN KEY (id_direccion)
            REFERENCES dbo.DIRECCION(id_direccion)
    );
END;
GO

/* =========================================================
   ADAPTACIÓN USUARIO - SUCURSAL OPCIONAL
   =========================================================
   La obligatoriedad de sucursal se determina dinámicamente
   mediante PERFIL.alcance_global.

   Los perfiles globales pueden tener id_sucursal = NULL.
   Los perfiles no globales deben tener una sucursal asignada.
   ========================================================= */

IF OBJECT_ID('dbo.USUARIO', 'U') IS NOT NULL
BEGIN
    ALTER TABLE dbo.USUARIO
    ALTER COLUMN id_sucursal INT NULL;
END;
GO


/* ========================
   CLIENTE
   ======================== */

IF OBJECT_ID('dbo.CLIENTE', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CLIENTE
    (
        id_cliente INT IDENTITY(1,1) PRIMARY KEY,

        nombre NVARCHAR(100) NOT NULL,
        apellido NVARCHAR(100) NOT NULL,
        documento NVARCHAR(20) NOT NULL,
        correo NVARCHAR(150) NULL,
        telefono NVARCHAR(30) NULL,
        eliminado_en DATETIME2 NULL,

        id_direccion INT NULL,

        CONSTRAINT UQ_CLIENTE_documento
            UNIQUE (documento),

        CONSTRAINT FK_CLIENTE_DIRECCION
            FOREIGN KEY (id_direccion)
            REFERENCES dbo.DIRECCION(id_direccion)
    );
END;
GO


/* ========================
   PRODUCTO
   ======================== */

IF OBJECT_ID('dbo.PRODUCTO', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PRODUCTO
    (
        id_producto INT IDENTITY(1,1) PRIMARY KEY,

        id_categoria INT NOT NULL,
        id_marca INT NULL,

        codigo_barra NVARCHAR(50) NULL,
        nombre NVARCHAR(100) NOT NULL,

        /* NVARCHAR(MAX) porque varias descripciones reales del catálogo
           superan los 250 caracteres. */
        descripcion NVARCHAR(MAX) NULL,

        precio_costo DECIMAL(18,2) NOT NULL,
        porcentaje_ganancia DECIMAL(5,2) NOT NULL,

        precio_venta AS
            CAST(
                precio_costo * (1 + porcentaje_ganancia / 100.0)
                AS DECIMAL(18,2)
            ),

        /* Activo permite ocultar temporalmente un producto sin darle baja lógica. */
        activo BIT NOT NULL
            CONSTRAINT DF_PRODUCTO_activo DEFAULT (1),

        eliminado_en DATETIME2 NULL,

        CONSTRAINT FK_PRODUCTO_CATEGORIA
            FOREIGN KEY (id_categoria)
            REFERENCES dbo.CATEGORIA(id_categoria),

        CONSTRAINT FK_PRODUCTO_MARCA
            FOREIGN KEY (id_marca)
            REFERENCES dbo.MARCA(id_marca),

        CONSTRAINT CK_PRODUCTO_precio_costo
            CHECK (precio_costo >= 0),

        CONSTRAINT CK_PRODUCTO_porcentaje_ganancia
            CHECK (porcentaje_ganancia >= 0)
    );
END;
GO


/* =========================================================
   ADAPTACIONES PARA UNA BASE YA EXISTENTE
   ========================================================= */

/* Agregar id_marca si PRODUCTO ya existía con el modelo anterior.
   Se deja NULL para no romper productos existentes que todavía
   no tengan una marca asignada. */
IF OBJECT_ID('dbo.PRODUCTO', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.PRODUCTO', 'id_marca') IS NULL
BEGIN
    ALTER TABLE dbo.PRODUCTO
    ADD id_marca INT NULL;
END;
GO


/* Agregar activo a PRODUCTO si todavía no existe.
   WITH VALUES asigna 1 a los productos ya existentes. */
IF OBJECT_ID('dbo.PRODUCTO', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.PRODUCTO', 'activo') IS NULL
BEGIN
    ALTER TABLE dbo.PRODUCTO
    ADD activo BIT NOT NULL
        CONSTRAINT DF_PRODUCTO_activo DEFAULT (1) WITH VALUES;
END;
GO


/* Ampliar la descripción de PRODUCTO para soportar las
   descripciones reales del catálogo. */
IF OBJECT_ID('dbo.PRODUCTO', 'U') IS NOT NULL
   AND EXISTS
   (
       SELECT 1
       FROM sys.columns
       WHERE object_id = OBJECT_ID('dbo.PRODUCTO')
         AND name = 'descripcion'
         AND max_length <> -1
   )
BEGIN
    ALTER TABLE dbo.PRODUCTO
    ALTER COLUMN descripcion NVARCHAR(MAX) NULL;
END;
GO


/* Agregar FK PRODUCTO -> MARCA si todavía no existe. */
IF OBJECT_ID('dbo.PRODUCTO', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.MARCA', 'U') IS NOT NULL
   AND NOT EXISTS
   (
       SELECT 1
       FROM sys.foreign_keys
       WHERE parent_object_id = OBJECT_ID('dbo.PRODUCTO')
         AND name = 'FK_PRODUCTO_MARCA'
   )
BEGIN
    ALTER TABLE dbo.PRODUCTO
    ADD CONSTRAINT FK_PRODUCTO_MARCA
        FOREIGN KEY (id_marca)
        REFERENCES dbo.MARCA(id_marca);
END;
GO


/* Crear el índice único de código de barra solamente si no existe. */
IF OBJECT_ID('dbo.PRODUCTO', 'U') IS NOT NULL
   AND NOT EXISTS
   (
       SELECT 1
       FROM sys.indexes
       WHERE object_id = OBJECT_ID('dbo.PRODUCTO')
         AND name = 'UQ_PRODUCTO_codigo_barra'
   )
   AND NOT EXISTS
   (
       SELECT 1
       FROM sys.stats
       WHERE object_id = OBJECT_ID('dbo.PRODUCTO')
         AND name = 'UQ_PRODUCTO_codigo_barra'
   )
BEGIN
    EXEC
    (
        'CREATE UNIQUE INDEX UQ_PRODUCTO_codigo_barra
         ON dbo.PRODUCTO(codigo_barra)
         WHERE codigo_barra IS NOT NULL;'
    );
END;
GO

/* ========================
   INVENTARIO
   ======================== */

/*
   IMPORTANTE:
   El stock NO pertenece directamente a PRODUCTO.
   Se administra por sucursal.

   Un mismo producto puede tener:
   - 10 unidades en Sucursal Central
   - 4 unidades en Sucursal 2
   - 0 unidades en Sucursal 3

   La combinación (id_producto, id_sucursal) debe ser única.
*/

IF OBJECT_ID('dbo.INVENTARIO', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.INVENTARIO
    (
        id_inventario INT IDENTITY(1,1) PRIMARY KEY,

        id_producto INT NOT NULL,
        id_sucursal INT NOT NULL,

        stock INT NOT NULL,
        stock_minimo INT NOT NULL,

        eliminado_en DATETIME2 NULL,

        CONSTRAINT FK_INVENTARIO_PRODUCTO
            FOREIGN KEY (id_producto)
            REFERENCES dbo.PRODUCTO(id_producto),

        CONSTRAINT FK_INVENTARIO_SUCURSAL
            FOREIGN KEY (id_sucursal)
            REFERENCES dbo.SUCURSAL(id_sucursal),

        CONSTRAINT UQ_INVENTARIO_PRODUCTO_SUCURSAL
            UNIQUE (id_producto, id_sucursal),

        CONSTRAINT CK_INVENTARIO_stock
            CHECK (stock >= 0),

        CONSTRAINT CK_INVENTARIO_stock_minimo
            CHECK (stock_minimo >= 0)
    );
END;
GO


/* =========================================================
   TIPOS DE TABLA PARA REGISTRO DE VENTAS
   =========================================================
   Permiten enviar desde Capa_Datos todos los productos y pagos
   de una venta en una sola llamada a sp_Venta_Registrar.

   La Vista no envía precios ni subtotales de productos:
   SQL Server volverá a obtener el precio vigente de PRODUCTO.
   ========================================================= */

IF TYPE_ID(N'dbo.VentaItemTipo') IS NULL
BEGIN
    EXEC
    (
        N'
        CREATE TYPE dbo.VentaItemTipo AS TABLE
        (
            id_producto INT NOT NULL PRIMARY KEY,
            cantidad INT NOT NULL
                CHECK (cantidad > 0)
        );
        '
    );
END;
GO


IF TYPE_ID(N'dbo.VentaPagoTipo') IS NULL
BEGIN
    EXEC
    (
        N'
        CREATE TYPE dbo.VentaPagoTipo AS TABLE
        (
            id_metodo_pago INT NOT NULL PRIMARY KEY,
            monto DECIMAL(18,2) NOT NULL
                CHECK (monto > 0)
        );
        '
    );
END;
GO


/* ========================
   VENTA
   ======================== */

IF OBJECT_ID('dbo.VENTA', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.VENTA
    (
        id_venta INT IDENTITY(1,1) PRIMARY KEY,

        id_cliente INT NOT NULL,
        id_usuario INT NOT NULL,
        id_sucursal INT NOT NULL,

        fecha_hora DATETIME2 NOT NULL
            CONSTRAINT DF_VENTA_fecha_hora DEFAULT SYSDATETIME(),

        /* La facturación real todavía no está definida.
           Se permite NULL hasta implementar los tipos de comprobante. */
        tipo_factura NVARCHAR(20) NULL,

        subtotal DECIMAL(18,2) NOT NULL,

        descuento DECIMAL(18,2) NOT NULL
            CONSTRAINT DF_VENTA_descuento DEFAULT 0,

        total DECIMAL(18,2) NOT NULL,

        eliminado_en DATETIME2 NULL,

        CONSTRAINT FK_VENTA_CLIENTE
            FOREIGN KEY (id_cliente)
            REFERENCES dbo.CLIENTE(id_cliente),

        CONSTRAINT FK_VENTA_USUARIO
            FOREIGN KEY (id_usuario)
            REFERENCES dbo.USUARIO(id_usuario),

        CONSTRAINT FK_VENTA_SUCURSAL
            FOREIGN KEY (id_sucursal)
            REFERENCES dbo.SUCURSAL(id_sucursal),

        CONSTRAINT CK_VENTA_subtotal
            CHECK (subtotal >= 0),

        CONSTRAINT CK_VENTA_descuento
            CHECK (descuento >= 0),

        CONSTRAINT CK_VENTA_total
            CHECK (total >= 0)
    );
END;
GO


/* =========================================================
   ADAPTACIÓN VENTA - TIPO DE FACTURA OPCIONAL
   =========================================================
   La política de comprobantes todavía no está definida.
   Se mantiene la columna para una futura implementación,
   pero por ahora no forma parte del flujo de registro.
   ========================================================= */

IF OBJECT_ID('dbo.VENTA', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.VENTA', 'tipo_factura') IS NOT NULL
BEGIN
    ALTER TABLE dbo.VENTA
    ALTER COLUMN tipo_factura NVARCHAR(20) NULL;
END;
GO


/* ========================
   DETALLE_VENTA
   ======================== */

IF OBJECT_ID('dbo.DETALLE_VENTA', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.DETALLE_VENTA
    (
        id_detalle_venta INT IDENTITY(1,1) PRIMARY KEY,

        id_venta INT NOT NULL,
        id_producto INT NOT NULL,

        cantidad INT NOT NULL,
        precio_unitario DECIMAL(18,2) NOT NULL,
        subtotal DECIMAL(18,2) NOT NULL,

        eliminado_en DATETIME2 NULL,

        CONSTRAINT FK_DETALLE_VENTA_VENTA
            FOREIGN KEY (id_venta)
            REFERENCES dbo.VENTA(id_venta),

        CONSTRAINT FK_DETALLE_VENTA_PRODUCTO
            FOREIGN KEY (id_producto)
            REFERENCES dbo.PRODUCTO(id_producto),

        CONSTRAINT CK_DETALLE_VENTA_cantidad
            CHECK (cantidad > 0),

        CONSTRAINT CK_DETALLE_VENTA_precio_unitario
            CHECK (precio_unitario >= 0),

        CONSTRAINT CK_DETALLE_VENTA_subtotal
            CHECK (subtotal >= 0)
    );
END;
GO


/* ========================
   PAGO
   ======================== */

IF OBJECT_ID('dbo.PAGO', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PAGO
    (
        id_pago INT IDENTITY(1,1) PRIMARY KEY,

        id_venta INT NOT NULL,
        id_metodo_pago INT NOT NULL,

        monto DECIMAL(18,2) NOT NULL,

        eliminado_en DATETIME2 NULL,

        CONSTRAINT FK_PAGO_VENTA
            FOREIGN KEY (id_venta)
            REFERENCES dbo.VENTA(id_venta),

        CONSTRAINT FK_PAGO_METODO_PAGO
            FOREIGN KEY (id_metodo_pago)
            REFERENCES dbo.METODO_PAGO(id_metodo_pago),

        CONSTRAINT CK_PAGO_monto
            CHECK (monto > 0)
    );
END;
GO


/* ========================
   AVISO
   ========================
   Comunicaciones internas con destinatarios configurables por perfil. */
IF OBJECT_ID('dbo.AVISO', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AVISO
    (
        id_aviso INT IDENTITY(1,1) PRIMARY KEY,
        titulo NVARCHAR(100) NOT NULL,
        mensaje NVARCHAR(500) NOT NULL,
        fecha_creacion DATETIME2 NOT NULL
            CONSTRAINT DF_AVISO_fecha_creacion DEFAULT SYSDATETIME(),
        id_usuario_autor INT NOT NULL,
        id_sucursal INT NULL,
        activo BIT NOT NULL
            CONSTRAINT DF_AVISO_activo DEFAULT (1),
        eliminado_en DATETIME2 NULL,

        CONSTRAINT FK_AVISO_USUARIO_AUTOR
            FOREIGN KEY (id_usuario_autor)
            REFERENCES dbo.USUARIO(id_usuario),
        CONSTRAINT FK_AVISO_SUCURSAL
            FOREIGN KEY (id_sucursal)
            REFERENCES dbo.SUCURSAL(id_sucursal),
        CONSTRAINT CK_AVISO_titulo_no_vacio
            CHECK (LEN(LTRIM(RTRIM(titulo))) > 0),
        CONSTRAINT CK_AVISO_mensaje_no_vacio
            CHECK (LEN(LTRIM(RTRIM(mensaje))) > 0)
    );
END;
GO

/* Relaciona perfiles que pueden publicar avisos con sus perfiles destino. */
IF OBJECT_ID('dbo.PERFIL_AVISO_DESTINO', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PERFIL_AVISO_DESTINO
    (
        id_perfil_emisor INT NOT NULL,
        id_perfil_destino INT NOT NULL,
        CONSTRAINT PK_PERFIL_AVISO_DESTINO PRIMARY KEY (id_perfil_emisor, id_perfil_destino),
        CONSTRAINT FK_PERFIL_AVISO_DESTINO_EMISOR FOREIGN KEY (id_perfil_emisor) REFERENCES dbo.PERFIL(id_perfil),
        CONSTRAINT FK_PERFIL_AVISO_DESTINO_DESTINO FOREIGN KEY (id_perfil_destino) REFERENCES dbo.PERFIL(id_perfil),
        CONSTRAINT CK_PERFIL_AVISO_DESTINO_DISTINTO CHECK (id_perfil_emisor <> id_perfil_destino)
    );
END;
GO

/* Permite que un aviso tenga uno o más perfiles destinatarios. */
IF OBJECT_ID('dbo.AVISO_PERFIL_DESTINO', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AVISO_PERFIL_DESTINO
    (
        id_aviso INT NOT NULL,
        id_perfil INT NOT NULL,
        CONSTRAINT PK_AVISO_PERFIL_DESTINO PRIMARY KEY (id_aviso, id_perfil),
        CONSTRAINT FK_AVISO_PERFIL_DESTINO_AVISO FOREIGN KEY (id_aviso) REFERENCES dbo.AVISO(id_aviso),
        CONSTRAINT FK_AVISO_PERFIL_DESTINO_PERFIL FOREIGN KEY (id_perfil) REFERENCES dbo.PERFIL(id_perfil)
    );
END;
GO

/* Migra avisos heredados por funcionalidad hacia los perfiles que tenían ese permiso. */
IF COL_LENGTH('dbo.AVISO', 'id_funcionalidad_destino') IS NOT NULL
BEGIN
    /*
       La columna heredada sólo se menciona dentro de SQL dinámico.
       Así, una base ya migrada no falla durante la compilación de este lote.
    */
    DECLARE @sqlMigracionAvisos NVARCHAR(MAX) = N'
        INSERT INTO dbo.AVISO_PERFIL_DESTINO (id_aviso, id_perfil)
        SELECT DISTINCT a.id_aviso, pf.id_perfil
        FROM dbo.AVISO AS a
        INNER JOIN dbo.PERFIL_FUNCIONALIDAD AS pf
            ON pf.id_funcionalidad = a.id_funcionalidad_destino
        INNER JOIN dbo.PERFIL AS p
            ON p.id_perfil = pf.id_perfil
           AND p.eliminado_en IS NULL
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM dbo.AVISO_PERFIL_DESTINO AS apd
            WHERE apd.id_aviso = a.id_aviso
              AND apd.id_perfil = pf.id_perfil
        );

        IF EXISTS
        (
            SELECT 1 FROM sys.indexes
            WHERE object_id = OBJECT_ID(N''dbo.AVISO'')
              AND name = N''IX_AVISO_DestinoActivo''
        )
            DROP INDEX IX_AVISO_DestinoActivo ON dbo.AVISO;
        ELSE IF EXISTS
        (
            SELECT 1 FROM sys.stats
            WHERE object_id = OBJECT_ID(N''dbo.AVISO'')
              AND name = N''IX_AVISO_DestinoActivo''
        )
            DROP STATISTICS dbo.AVISO.IX_AVISO_DestinoActivo;

        IF EXISTS
        (
            SELECT 1 FROM sys.foreign_keys
            WHERE parent_object_id = OBJECT_ID(N''dbo.AVISO'')
              AND name = N''FK_AVISO_FUNCIONALIDAD_DESTINO''
        )
            ALTER TABLE dbo.AVISO
            DROP CONSTRAINT FK_AVISO_FUNCIONALIDAD_DESTINO;

        ALTER TABLE dbo.AVISO
        DROP COLUMN id_funcionalidad_destino;';

    EXEC sys.sp_executesql @sqlMigracionAvisos;
END;
GO

/* Acelera la lectura de avisos activos por alcance y por perfil destinatario. */
IF OBJECT_ID(N'dbo.AVISO', N'U') IS NOT NULL
AND NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.AVISO')
      AND name = N'IX_AVISO_DestinoActivo'
)
AND NOT EXISTS (
    SELECT 1
    FROM sys.stats
    WHERE object_id = OBJECT_ID(N'dbo.AVISO')
      AND name = N'IX_AVISO_DestinoActivo'
)
BEGIN
    EXEC(N'
        CREATE INDEX IX_AVISO_DestinoActivo
        ON dbo.AVISO(activo, id_sucursal, fecha_creacion DESC);
    ');
END;

IF OBJECT_ID(N'dbo.AVISO_PERFIL_DESTINO', N'U') IS NOT NULL
AND NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.AVISO_PERFIL_DESTINO')
      AND name = N'IX_AVISO_PERFIL_DESTINO_PerfilAviso'
)
AND NOT EXISTS (
    SELECT 1
    FROM sys.stats
    WHERE object_id = OBJECT_ID(N'dbo.AVISO_PERFIL_DESTINO')
      AND name = N'IX_AVISO_PERFIL_DESTINO_PerfilAviso'
)
BEGIN
    EXEC(N'
        CREATE INDEX IX_AVISO_PERFIL_DESTINO_PerfilAviso
        ON dbo.AVISO_PERFIL_DESTINO(id_perfil, id_aviso);
    ');
END;
GO


/* =========================================================
   FIN DEL SCRIPT
   Este archivo puede volver a ejecutarse sin recrear tablas
   ni perder los datos existentes.
   ========================================================= */
