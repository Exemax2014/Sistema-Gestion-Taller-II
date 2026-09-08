/* =========================================================
   SISTEMA DE GESTIÓN - TALLER DE PROGRAMACIÓN II
   Script 05 - Catálogo Inicial
   Sistema Hierro y Forja

   OBJETIVO
   - Cargar categorías.
   - Cargar marcas.
   - Cargar productos iniciales.
   - Cargar inventario por sucursal.
   - Permitir ejecutar el script varias veces sin duplicar datos.

   DECISIONES DEL MODELO
   - PRODUCTO NO guarda stock global.
   - El stock se guarda exclusivamente en INVENTARIO.
   - INVENTARIO identifica PRODUCTO + SUCURSAL.
   - Se cargan stocks diferentes para Sucursal Central y
     Sucursal Norte para facilitar las pruebas multisucursal.
   - No se almacenan imágenes del catálogo web.
   - No se importan precio ni precio_anterior del catálogo web.
   - precio_costo y porcentaje_ganancia de este archivo son
     valores de desarrollo para probar el precio_venta calculado.
   - El campo ventas del catálogo web no se importa; las ventas
     reales se obtendrán desde VENTA y DETALLE_VENTA.

   FUENTES DEL CATÁLOGO
   - categorias.json
   - marcas.json
   - productos.json

   NOTA:
   Se excluye el registro "salamines Lario" por no corresponder
   al dominio del sistema de ferretería/herramientas.
   ========================================================= */

USE SistemaGestion;
GO

/* =========================================================
   CATEGORÍAS
   ========================================================= */

IF EXISTS (SELECT 1 FROM dbo.CATEGORIA WHERE nombre = N'Carpintería')
BEGIN
    UPDATE dbo.CATEGORIA
    SET eliminado_en = NULL
    WHERE nombre = N'Carpintería';
END
ELSE
BEGIN
    INSERT INTO dbo.CATEGORIA (nombre, descripcion)
    VALUES (N'Carpintería', NULL);
END;
GO

IF EXISTS (SELECT 1 FROM dbo.CATEGORIA WHERE nombre = N'Construcción')
BEGIN
    UPDATE dbo.CATEGORIA
    SET eliminado_en = NULL
    WHERE nombre = N'Construcción';
END
ELSE
BEGIN
    INSERT INTO dbo.CATEGORIA (nombre, descripcion)
    VALUES (N'Construcción', NULL);
END;
GO

IF EXISTS (SELECT 1 FROM dbo.CATEGORIA WHERE nombre = N'Durlok')
BEGIN
    UPDATE dbo.CATEGORIA
    SET eliminado_en = NULL
    WHERE nombre = N'Durlok';
END
ELSE
BEGIN
    INSERT INTO dbo.CATEGORIA (nombre, descripcion)
    VALUES (N'Durlok', NULL);
END;
GO

IF EXISTS (SELECT 1 FROM dbo.CATEGORIA WHERE nombre = N'Electricidad')
BEGIN
    UPDATE dbo.CATEGORIA
    SET eliminado_en = NULL
    WHERE nombre = N'Electricidad';
END
ELSE
BEGIN
    INSERT INTO dbo.CATEGORIA (nombre, descripcion)
    VALUES (N'Electricidad', NULL);
END;
GO

IF EXISTS (SELECT 1 FROM dbo.CATEGORIA WHERE nombre = N'Ferretería')
BEGIN
    UPDATE dbo.CATEGORIA
    SET eliminado_en = NULL
    WHERE nombre = N'Ferretería';
END
ELSE
BEGIN
    INSERT INTO dbo.CATEGORIA (nombre, descripcion)
    VALUES (N'Ferretería', NULL);
END;
GO

IF EXISTS (SELECT 1 FROM dbo.CATEGORIA WHERE nombre = N'Herrería')
BEGIN
    UPDATE dbo.CATEGORIA
    SET eliminado_en = NULL
    WHERE nombre = N'Herrería';
END
ELSE
BEGIN
    INSERT INTO dbo.CATEGORIA (nombre, descripcion)
    VALUES (N'Herrería', NULL);
END;
GO

IF EXISTS (SELECT 1 FROM dbo.CATEGORIA WHERE nombre = N'Pinturería')
BEGIN
    UPDATE dbo.CATEGORIA
    SET eliminado_en = NULL
    WHERE nombre = N'Pinturería';
END
ELSE
BEGIN
    INSERT INTO dbo.CATEGORIA (nombre, descripcion)
    VALUES (N'Pinturería', NULL);
END;
GO

/* =========================================================
   MARCAS
   ========================================================= */

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Baw')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Baw';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Baw');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Bosch')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Bosch';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Bosch');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Bremen')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Bremen';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Bremen');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'DeWalt')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'DeWalt';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'DeWalt');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Gamma')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Gamma';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Gamma');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Ingco')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Ingco';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Ingco');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Lusqtoff')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Lusqtoff';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Lusqtoff');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Makita')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Makita';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Makita');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Milwaukee')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Milwaukee';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Milwaukee');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Neng')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Neng';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Neng');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Stanley')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Stanley';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Stanley');
END;
GO

IF EXISTS (SELECT 1 FROM dbo.MARCA WHERE nombre = N'Total')
BEGIN
    UPDATE dbo.MARCA
    SET eliminado_en = NULL
    WHERE nombre = N'Total';
END
ELSE
BEGIN
    INSERT INTO dbo.MARCA (nombre)
    VALUES (N'Total');
END;
GO

/* =========================================================
   PRODUCTOS
   =========================================================
   El nombre se utiliza como clave natural para este catálogo
   inicial. Si el producto ya existe, se actualizan categoría,
   marca, descripción, costo, ganancia y estado activo.
   ========================================================= */

IF OBJECT_ID('tempdb..#CatalogoProducto') IS NOT NULL
    DROP TABLE #CatalogoProducto;

CREATE TABLE #CatalogoProducto
(
    nombre NVARCHAR(100) NOT NULL,
    categoria NVARCHAR(100) NOT NULL,
    marca NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(MAX) NULL,
    precio_costo DECIMAL(18,2) NOT NULL,
    porcentaje_ganancia DECIMAL(5,2) NOT NULL,
    activo BIT NOT NULL,
    stock_fuente INT NOT NULL
);
GO

INSERT INTO #CatalogoProducto
(nombre, categoria, marca, descripcion, precio_costo, porcentaje_ganancia, activo, stock_fuente)
VALUES
(N'Amoladora Angular 900W', N'Ferretería', N'Bosch', N'Equipo compacto para corte y desbaste con excelente agarre y potencia constante.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Eléctrica', 60000.00, 40.00, 1, 0),
(N'Soldadora Inverter 220A', N'Herrería', N'Lusqtoff', N'Ideal para trabajos exigentes con buena estabilidad de arco y estructura reforzada.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Eléctrica', 85000.00, 40.00, 1, 6),
(N'Calibrador Digital', N'Construcción', N'DeWalt', N'Herramienta de precisión para mediciones rápidas, confiables y de lectura simple.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Manual', 50000.00, 40.00, 0, 10),
(N'Casco de Soldar Fotosensible', N'Herrería', N'Makita', N'Protección visual y frontal para tareas de soldadura con pantalla de oscurecimiento automático.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Manual', 92000.00, 40.00, 1, 7),
(N'Prensa de Banco Reforzada', N'Carpintería', N'Lusqtoff', N'Base firme y cuerpo robusto para sujeción segura en trabajos de taller y montaje.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Manual', 90000.00, 40.00, 0, 8),
(N'Guantes de Seguridad Industrial', N'Pinturería', N'Ingco', N'Protección y comodidad para manipulación diaria en entornos de trabajo exigentes.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Manual', 15000.00, 40.00, 1, 2),
(N'Atornillador Inalámbrico', N'Durlok', N'Total', N'Ideal para montaje en seco, fijaciones ágiles y trabajo continuo con buena autonomía.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Inalámbrica', 70000.00, 40.00, 1, 7),
(N'Cinta Métrica 8m', N'Construcción', N'Bremen', N'Lectura clara, cuerpo resistente y traba segura para mediciones de uso intensivo.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Manual', 57000.00, 40.00, 1, 10),
(N'Kit-3 Brochas Profesional', N'Pinturería', N'Stanley', N'Buena cobertura, mango firme y terminación pareja para trabajos de pintura interior y exterior.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Manual', 22000.00, 40.00, 1, 10),
(N'Sierra Circular', N'Carpintería', N'Makita', N'Corte preciso y estable para trabajos de carpintería y montaje con excelente desempeño.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Eléctrica', 97000.00, 40.00, 1, 10),
(N'Atornillador Inalámbrico para Durlok', N'Durlok', N'Milwaukee', N'Fijación segura para placas y estructuras livianas con instalación rápida y práctica. Una buena herramienta inalambrica ideal para trabajos de exigencia' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Inalámbrica', 77000.00, 40.00, 1, 10),
(N'Martillo de Uña', N'Ferretería', N'Total', N'Herramienta clásica, resistente y cómoda para tareas generales de fijación y desmontaje.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Manual', 67000.00, 40.00, 1, 10),
(N'Sierra Circular Inalámbrica DeWalt', N'Carpintería', N'DeWalt', N'Sierra circular inalámbrica de alto rendimiento ideal para cortes precisos en madera.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Inalámbrica', 104000.00, 40.00, 1, 10),
(N'Taladro Inalámbrico DeWalt con Baterías', N'Construcción', N'DeWalt', N'Taladro atornillador con baterías de larga duración, ideal para uso intensivo en obra.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Inalámbrica', 64000.00, 40.00, 1, 10),
(N'Atornillador de Impacto Inalámbrico DeWalt', N'Ferretería', N'DeWalt', N'Atornillador compacto y potente, perfecto para trabajos exigentes.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Inalámbrica', 74000.00, 40.00, 1, 10),
(N'Sierra Circular Makita', N'Carpintería', N'Makita', N'Sierra circular con cable de gran precisión para cortes continuos en madera.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Eléctrica', 111000.00, 40.00, 1, 10),
(N'Martillo de Goma Ingco', N'Herrería', N'Ingco', N'Martillo de goma resistente ideal para trabajos sin dañar superficies.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Manual', 99000.00, 40.00, 1, 10),
(N'Caja de Herramientas Apilable con Ruedas Milwaukee', N'Ferretería', N'Milwaukee', N'Sistema modular con ruedas para transporte cómodo de herramientas.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Manual', 81000.00, 40.00, 1, 10),
(N'Caja de Herramientas Apilable con Manija Milwaukee', N'Ferretería', N'Milwaukee', N'Caja resistente y práctica con manija para organización de herramientas.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Manual', 88000.00, 40.00, 1, 10),
(N'Sierra Caladora con Cable Makita', N'Carpintería', N'Makita', N'Sierra caladora precisa para cortes rectos y curvos en madera.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Eléctrica', 118000.00, 40.00, 1, 9),
(N'Amoladora Angular 1500W', N'Ferretería', N'DeWalt', N'Equipo compacto para corte y desbaste, excelencia que nos destaca en el mercado.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Eléctrica', 95000.00, 40.00, 1, 10),
(N'Taladro Maquita 13mm 220v 710w', N'Carpintería', N'Makita', N'El taladro percutor Makita HP1630 de 13 mm de mandril y 16 mm (5/8″)(5/8pulgadas) de capacidad, posee un potente motor de 710 W que proporciona 0-3,200 RPM y 0-48,000 IPM para las aplicaciones más exigentes. Ofrece una operación de 2 modos para «Sólo rotación» o «Martilleo con rotación» para múltiples aplicaciones con un mango lateral giratorio 360°' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Eléctrica', 125000.00, 40.00, 1, 17),
(N'Medidor de distancia Dwht77100 láser de 100 pies Dewalt', N'Carpintería', N'DeWalt', N'Puede llevar consigo este medidor de distancia láser de 100 pies, delgado y compacto, a casi cualquier lugar de trabajo, ya que cabe cómodamente en su bolsillo. La operación de 2 botones de la herramienta permite realizar mediciones sin complicaciones, lo que permite al usuario cambiar rápidamente entre unidades de medida (pies, pulgadas y metros). Para facilitar la lectura en interiores y exteriores, la pantalla LCD retroiluminada permanece constantemente brillante y fue diseñada para ayudarlo a leer las mediciones bajo la luz solar directa.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Inalámbrica', 132000.00, 40.00, 1, 12),
(N'Multimetro Digital Baw Rm113d Trms Autorrango 600v 10a', N'Electricidad', N'Baw', N'El multímetro digital RM113D es un instrumento de medición multi-función de alta precisión, rápida respuesta y elevado nivel de seguridad.' + NCHAR(13) + NCHAR(10) + N'Incorpora un CI especial de hasta 6000 cuentas. Este CI está compuesto por un convertidor A/D de alta precisión y un procesador digital de alta velocidad que puede realizar cálculos RMS reales de alta velocidad.' + NCHAR(13) + NCHAR(10) + N'Provee mediciones precisas, alta resolución, velocidad de operación rápida, calibración de software completa, sin cambios en la precisión con el uso a largo plazo.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Inalámbrica', 30000.00, 40.00, 1, 8),
(N'Multímetro Digital Automático 6000 Cuentas Recargable', N'Electricidad', N'Neng', N'Multímetro digital inteligente de 6000 cuentas de verdadero valor eficaz (TRMS) con carga automática por USB-C.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Equipo de medición multifunción con rango automático, NCV, pantalla retroiluminada y batería recargable por USB-C.' + NCHAR(13) + NCHAR(10) + N'' + NCHAR(13) + NCHAR(10) + N'Tipo de energía: Inalámbrica', 37000.00, 40.00, 1, 12);
GO

/* Actualizar productos existentes */
UPDATE p
SET
    p.id_categoria = c.id_categoria,
    p.id_marca = m.id_marca,
    p.descripcion = cp.descripcion,
    p.precio_costo = cp.precio_costo,
    p.porcentaje_ganancia = cp.porcentaje_ganancia,
    p.activo = cp.activo,
    p.eliminado_en = NULL
FROM dbo.PRODUCTO AS p
INNER JOIN #CatalogoProducto AS cp
    ON cp.nombre = p.nombre
INNER JOIN dbo.CATEGORIA AS c
    ON c.nombre = cp.categoria
   AND c.eliminado_en IS NULL
INNER JOIN dbo.MARCA AS m
    ON m.nombre = cp.marca
   AND m.eliminado_en IS NULL;
GO

/* Insertar productos faltantes */
INSERT INTO dbo.PRODUCTO
(
    id_categoria,
    id_marca,
    codigo_barra,
    nombre,
    descripcion,
    precio_costo,
    porcentaje_ganancia,
    activo
)
SELECT
    c.id_categoria,
    m.id_marca,
    NULL,
    cp.nombre,
    cp.descripcion,
    cp.precio_costo,
    cp.porcentaje_ganancia,
    cp.activo
FROM #CatalogoProducto AS cp
INNER JOIN dbo.CATEGORIA AS c
    ON c.nombre = cp.categoria
   AND c.eliminado_en IS NULL
INNER JOIN dbo.MARCA AS m
    ON m.nombre = cp.marca
   AND m.eliminado_en IS NULL
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.PRODUCTO AS p
    WHERE p.nombre = cp.nombre
);
GO


/* =========================================================
   INVENTARIO POR SUCURSAL
   =========================================================
   Sucursal Central:
   - usa como stock inicial el valor disponible en la fuente.

   Sucursal Norte:
   - utiliza valores distintos para probar correctamente
     el funcionamiento multisucursal.

   stock_minimo:
   - se fija inicialmente en 2 unidades.
   ========================================================= */

DECLARE @IdSucursalCentral INT;
DECLARE @IdSucursalNorte INT;

SELECT @IdSucursalCentral = id_sucursal
FROM dbo.SUCURSAL
WHERE nombre = N'Sucursal Central'
  AND eliminado_en IS NULL;

SELECT @IdSucursalNorte = id_sucursal
FROM dbo.SUCURSAL
WHERE nombre = N'Sucursal Norte'
  AND eliminado_en IS NULL;

IF @IdSucursalCentral IS NULL
    THROW 51001, 'No existe Sucursal Central. Ejecute primero 04_DatosPrueba.sql.', 1;

IF @IdSucursalNorte IS NULL
    THROW 51002, 'No existe Sucursal Norte. Ejecute primero 04_DatosPrueba.sql.', 1;


/* ---------------------------------------------------------
   STOCK - SUCURSAL CENTRAL
   --------------------------------------------------------- */

UPDATE i
SET
    i.stock = cp.stock_fuente,
    i.stock_minimo = 2,
    i.eliminado_en = NULL
FROM dbo.INVENTARIO AS i
INNER JOIN dbo.PRODUCTO AS p
    ON p.id_producto = i.id_producto
INNER JOIN #CatalogoProducto AS cp
    ON cp.nombre = p.nombre
WHERE i.id_sucursal = @IdSucursalCentral;

INSERT INTO dbo.INVENTARIO
(
    id_producto,
    id_sucursal,
    stock,
    stock_minimo
)
SELECT
    p.id_producto,
    @IdSucursalCentral,
    cp.stock_fuente,
    2
FROM #CatalogoProducto AS cp
INNER JOIN dbo.PRODUCTO AS p
    ON p.nombre = cp.nombre
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.INVENTARIO AS i
    WHERE i.id_producto = p.id_producto
      AND i.id_sucursal = @IdSucursalCentral
);


/* ---------------------------------------------------------
   STOCK - SUCURSAL NORTE
   ---------------------------------------------------------
   Se genera un stock de prueba diferente y estable a partir
   del stock de origen. No representa stock real del negocio.
   --------------------------------------------------------- */

UPDATE i
SET
    i.stock =
        CASE
            WHEN cp.stock_fuente = 0 THEN 5
            WHEN p.id_producto % 4 = 0 THEN
                CASE WHEN cp.stock_fuente - 3 < 0 THEN 0 ELSE cp.stock_fuente - 3 END
            WHEN p.id_producto % 4 = 1 THEN cp.stock_fuente + 4
            WHEN p.id_producto % 4 = 2 THEN
                CASE WHEN cp.stock_fuente - 1 < 0 THEN 0 ELSE cp.stock_fuente - 1 END
            ELSE cp.stock_fuente + 2
        END,
    i.stock_minimo = 2,
    i.eliminado_en = NULL
FROM dbo.INVENTARIO AS i
INNER JOIN dbo.PRODUCTO AS p
    ON p.id_producto = i.id_producto
INNER JOIN #CatalogoProducto AS cp
    ON cp.nombre = p.nombre
WHERE i.id_sucursal = @IdSucursalNorte;

INSERT INTO dbo.INVENTARIO
(
    id_producto,
    id_sucursal,
    stock,
    stock_minimo
)
SELECT
    p.id_producto,
    @IdSucursalNorte,
    CASE
        WHEN cp.stock_fuente = 0 THEN 5
        WHEN p.id_producto % 4 = 0 THEN
            CASE WHEN cp.stock_fuente - 3 < 0 THEN 0 ELSE cp.stock_fuente - 3 END
        WHEN p.id_producto % 4 = 1 THEN cp.stock_fuente + 4
        WHEN p.id_producto % 4 = 2 THEN
            CASE WHEN cp.stock_fuente - 1 < 0 THEN 0 ELSE cp.stock_fuente - 1 END
        ELSE cp.stock_fuente + 2
    END,
    2
FROM #CatalogoProducto AS cp
INNER JOIN dbo.PRODUCTO AS p
    ON p.nombre = cp.nombre
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.INVENTARIO AS i
    WHERE i.id_producto = p.id_producto
      AND i.id_sucursal = @IdSucursalNorte
);
GO


/* =========================================================
   VERIFICACIÓN
   ========================================================= */

SELECT
    p.id_producto,
    p.nombre AS producto,
    c.nombre AS categoria,
    m.nombre AS marca,
    p.precio_costo,
    p.porcentaje_ganancia,
    p.precio_venta,
    p.activo,
    s.nombre AS sucursal,
    i.stock,
    i.stock_minimo
FROM dbo.PRODUCTO AS p
INNER JOIN dbo.CATEGORIA AS c
    ON c.id_categoria = p.id_categoria
LEFT JOIN dbo.MARCA AS m
    ON m.id_marca = p.id_marca
INNER JOIN dbo.INVENTARIO AS i
    ON i.id_producto = p.id_producto
   AND i.eliminado_en IS NULL
INNER JOIN dbo.SUCURSAL AS s
    ON s.id_sucursal = i.id_sucursal
WHERE p.nombre IN
(
    SELECT nombre
    FROM #CatalogoProducto
)
ORDER BY
    p.nombre,
    s.nombre;
GO

DROP TABLE #CatalogoProducto;
GO

/* =========================================================
   FIN DEL SCRIPT

   Este archivo puede ejecutarse nuevamente:
   - no duplica categorías;
   - no duplica marcas;
   - no duplica productos;
   - no duplica inventarios producto+sucursal;
   - actualiza los valores iniciales del catálogo de desarrollo.
   ========================================================= */
