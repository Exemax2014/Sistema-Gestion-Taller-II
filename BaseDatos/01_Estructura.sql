/* =========================================================
   SISTEMA DE GESTIÓN - TALLER DE PROGRAMACIÓN II
   Script 01 - Estructura de Base de Datos
   ========================================================= */

IF DB_ID('SistemaGestion') IS NULL
BEGIN
    CREATE DATABASE SistemaGestion;
END;
GO

USE SistemaGestion;
GO


/* ========================
   PROVINCIA
   ======================== */

CREATE TABLE dbo.PROVINCIA
(
    id_provincia INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) NOT NULL,
    eliminado_en DATETIME2 NULL,

    CONSTRAINT UQ_PROVINCIA_nombre
        UNIQUE (nombre)
);
GO


/* ========================
   LOCALIDAD
   ======================== */

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
GO


/* ========================
   DIRECCION
   ======================== */

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
GO


/* ========================
   PERFIL
   ======================== */

CREATE TABLE dbo.PERFIL
(
    id_perfil INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(50) NOT NULL,
    descripcion NVARCHAR(200) NULL,
    eliminado_en DATETIME2 NULL,

    CONSTRAINT UQ_PERFIL_nombre
        UNIQUE (nombre)
);
GO


/* ========================
   FUNCIONALIDAD
   ======================== */

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
GO


/* ========================
   SUCURSAL
   ======================== */

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
GO


/* ========================
   CATEGORIA
   ======================== */

CREATE TABLE dbo.CATEGORIA
(
    id_categoria INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(200) NULL,
    eliminado_en DATETIME2 NULL,

    CONSTRAINT UQ_CATEGORIA_nombre
        UNIQUE (nombre)
);
GO


/* ========================
   METODO_PAGO
   ======================== */

CREATE TABLE dbo.METODO_PAGO
(
    id_metodo_pago INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(200) NULL,
    eliminado_en DATETIME2 NULL,

    CONSTRAINT UQ_METODO_PAGO_nombre
        UNIQUE (nombre)
);
GO


/* ========================
   PERFIL_FUNCIONALIDAD
   ======================== */

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
GO


/* ========================
   USUARIO
   ======================== */

CREATE TABLE dbo.USUARIO
(
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,

    id_perfil INT NOT NULL,
    id_sucursal INT NOT NULL,

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
GO


/* ========================
   CLIENTE
   ======================== */

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
GO


/* ========================
   PRODUCTO
   ======================== */

CREATE TABLE dbo.PRODUCTO
(
    id_producto INT IDENTITY(1,1) PRIMARY KEY,

    id_categoria INT NOT NULL,

    codigo_barra NVARCHAR(50) NULL,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(250) NULL,

    precio_costo DECIMAL(18,2) NOT NULL,
    porcentaje_ganancia DECIMAL(5,2) NOT NULL,

    precio_venta AS
        CAST(
            precio_costo * (1 + porcentaje_ganancia / 100.0)
            AS DECIMAL(18,2)
        ),

    eliminado_en DATETIME2 NULL,

    CONSTRAINT FK_PRODUCTO_CATEGORIA
        FOREIGN KEY (id_categoria)
        REFERENCES dbo.CATEGORIA(id_categoria),

    CONSTRAINT CK_PRODUCTO_precio_costo
        CHECK (precio_costo >= 0),

    CONSTRAINT CK_PRODUCTO_porcentaje_ganancia
        CHECK (porcentaje_ganancia >= 0)
);
GO

CREATE UNIQUE INDEX UQ_PRODUCTO_codigo_barra
ON dbo.PRODUCTO(codigo_barra)
WHERE codigo_barra IS NOT NULL;
GO


/* ========================
   INVENTARIO
   ======================== */

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
GO


/* ========================
   VENTA
   ======================== */

CREATE TABLE dbo.VENTA
(
    id_venta INT IDENTITY(1,1) PRIMARY KEY,

    id_cliente INT NOT NULL,
    id_usuario INT NOT NULL,
    id_sucursal INT NOT NULL,

    fecha_hora DATETIME2 NOT NULL
        CONSTRAINT DF_VENTA_fecha_hora DEFAULT SYSDATETIME(),

    tipo_factura NVARCHAR(20) NOT NULL,

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
GO


/* ========================
   DETALLE_VENTA
   ======================== */

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
GO


/* ========================
   PAGO
   ======================== */

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
GO