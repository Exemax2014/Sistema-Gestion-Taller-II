
/* =========================================================
   SOLO PARA DESARROLLO / TESTING.
   Este script elimina datos operativos y devuelve la base al
   estado inicial.
   NO EJECUTAR EN PRODUCCION.

   Estado objetivo: 01_Estructura.sql + 02_DatosIniciales.sql
   + 03_Procedimientos.sql. No elimina objetos de base de datos.
   ========================================================= */
USE SistemaGestion;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    /* -----------------------------------------------------
       Datos iniciales que deben sobrevivir al reset.
       Las claves naturales son las mismas de 02.
       ----------------------------------------------------- */
    DECLARE @PerfilesIniciales TABLE(nombre NVARCHAR(50) PRIMARY KEY, descripcion NVARCHAR(200), alcance_global BIT);
    INSERT INTO @PerfilesIniciales VALUES
        (N'Administrador',N'Acceso general a la administracion del sistema',1),
        (N'Gerente',N'Acceso a funciones de gestion y reportes',0),
        (N'Vendedor',N'Acceso principalmente a ventas y atencion de clientes',0);

    /*
       Debe mantenerse sincronizada con el catálogo oficial de 02_DatosIniciales.sql.
       FUNCIONALIDAD no distingue en el esquema las claves oficiales de las de prueba;
       por eso esta lista explícita es necesaria para retirar únicamente las extras.
    */
    DECLARE @FuncionesIniciales TABLE(codigo NVARCHAR(50) PRIMARY KEY);
    INSERT INTO @FuncionesIniciales VALUES
        (N'USUARIOS_VER'),(N'USUARIOS_ALTA'),(N'USUARIOS_BAJA'),(N'USUARIOS_MODIFICAR'),
        (N'PERMISOS_GESTIONAR'),(N'BACKUP_REALIZAR'),(N'VENTAS_VER'),(N'VENTAS_REALIZAR'),
        (N'CLIENTES_VER'),(N'CLIENTES_ALTA'),(N'CLIENTES_BAJA'),(N'CLIENTES_MODIFICAR'),
        (N'PRODUCTOS_VER'),(N'PRODUCTOS_ALTA'),(N'PRODUCTOS_BAJA'),(N'PRODUCTOS_MODIFICAR'),
        (N'CATEGORIAS_VER'),(N'CATEGORIAS_ALTA'),(N'CATEGORIAS_MODIFICAR'),(N'CATEGORIAS_BAJA'),
        (N'MARCAS_VER'),(N'MARCAS_ALTA'),(N'MARCAS_MODIFICAR'),(N'MARCAS_BAJA'),
        (N'SUCURSALES_VER'),(N'SUCURSALES_ALTA'),(N'SUCURSALES_MODIFICAR'),(N'SUCURSALES_BAJA'),
        (N'REPORTES_ADMINISTRADOR'),(N'REPORTES_GERENTE'),(N'REPORTES_VENDEDOR'),
        (N'REPORTES_VER'),(N'REPORTES_VENTAS'),(N'REPORTES_RECAUDACION'),
        (N'REPORTES_PRODUCTOS'),(N'REPORTES_STOCK'),(N'REPORTES_RENDIMIENTO_VENDEDORES'),
        (N'REPORTES_DETALLE_VENTAS'),(N'REPORTES_EXPORTAR'),
        (N'REPORTES_ALCANCE_PROPIO'),(N'REPORTES_ALCANCE_SUCURSAL'),
        (N'REPORTES_ALCANCE_GLOBAL'),(N'AVISOS_VER'),(N'AVISOS_PUBLICAR');

    DECLARE @MetodosIniciales TABLE(nombre NVARCHAR(100) PRIMARY KEY);
    INSERT INTO @MetodosIniciales VALUES
        (N'Efectivo'),(N'Débito'),(N'Crédito'),(N'Transferencia');

    /* -----------------------------------------------------
       1. Eliminar datos operativos en orden de dependencias.
       ----------------------------------------------------- */
    DELETE FROM dbo.AUDITORIA;
    DELETE FROM dbo.PAGO;
    DELETE mp
    FROM dbo.METODO_PAGO AS mp
    WHERE NOT EXISTS (SELECT 1 FROM @MetodosIniciales AS mi WHERE mi.nombre = mp.nombre);
    DELETE FROM dbo.DETALLE_VENTA;
    DELETE FROM dbo.VENTA;
    DELETE FROM dbo.AVISO;
    DELETE FROM dbo.INVENTARIO;
    DELETE FROM dbo.PRODUCTO;
    DELETE FROM dbo.MARCA_CATEGORIA;
    DELETE FROM dbo.CLIENTE;

    /* Los usuarios de testing son todos excepto el administrador inicial. */
    DELETE FROM dbo.USUARIO
    WHERE nombre_usuario <> N'admin';

    -- Libera al administrador de referencias modificadas durante una prueba.
    UPDATE u
    SET u.id_perfil = p.id_perfil,
        u.id_sucursal = NULL,
        u.id_direccion = NULL
    FROM dbo.USUARIO AS u
    INNER JOIN dbo.PERFIL AS p ON p.nombre = N'Administrador'
    WHERE u.nombre_usuario = N'admin';

    /* Las funcionalidades iniciales se reconstruyen desde las reglas de 02 y 03. */
    DELETE FROM dbo.PERFIL_FUNCIONALIDAD;

    DELETE p
    FROM dbo.PERFIL AS p
    WHERE NOT EXISTS
    (
        SELECT 1 FROM @PerfilesIniciales AS pi WHERE pi.nombre = p.nombre
    );

    DELETE f
    FROM dbo.FUNCIONALIDAD AS f
    WHERE NOT EXISTS
    (
        SELECT 1 FROM @FuncionesIniciales AS fi WHERE fi.codigo = f.codigo
    );

    /* Sucursales, categorias y marcas no pertenecen al estado inicial salvo Central. */
    DELETE FROM dbo.SUCURSAL WHERE nombre <> N'Sucursal Central';
    DELETE FROM dbo.CATEGORIA;
    DELETE FROM dbo.MARCA;

    /* -----------------------------------------------------
       2. Restaurar perfiles iniciales y sus permisos exactos.
       ----------------------------------------------------- */
    UPDATE p
    SET p.descripcion = pi.descripcion,
        p.alcance_global = pi.alcance_global,
        p.eliminado_en = NULL
    FROM dbo.PERFIL AS p
    INNER JOIN @PerfilesIniciales AS pi ON pi.nombre = p.nombre;

    IF EXISTS
    (
        SELECT 1 FROM @PerfilesIniciales AS pi
        WHERE NOT EXISTS (SELECT 1 FROM dbo.PERFIL AS p WHERE p.nombre = pi.nombre)
    )
        THROW 51001, 'Falta un perfil inicial. Ejecute 02_DatosIniciales.sql antes del reset.', 1;

    IF EXISTS
    (
        SELECT 1 FROM @FuncionesIniciales AS fi
        WHERE NOT EXISTS (SELECT 1 FROM dbo.FUNCIONALIDAD AS f WHERE f.codigo = fi.codigo)
    )
        THROW 51002, 'Falta una funcionalidad inicial. Ejecute 02_DatosIniciales.sql antes del reset.', 1;

    UPDATE f
    SET f.eliminado_en = NULL
    FROM dbo.FUNCIONALIDAD AS f
    INNER JOIN @FuncionesIniciales AS fi ON fi.codigo = f.codigo;

    UPDATE mp
    SET mp.eliminado_en = NULL
    FROM dbo.METODO_PAGO AS mp
    INNER JOIN @MetodosIniciales AS mi ON mi.nombre = mp.nombre;

    DECLARE @PermisosNoGlobales TABLE(perfil NVARCHAR(50), codigo NVARCHAR(50), PRIMARY KEY(perfil,codigo));
    INSERT INTO @PermisosNoGlobales VALUES
        (N'Gerente',N'CLIENTES_VER'),(N'Gerente',N'CLIENTES_ALTA'),(N'Gerente',N'CLIENTES_BAJA'),(N'Gerente',N'PRODUCTOS_VER'),
        (N'Gerente',N'REPORTES_GERENTE'),(N'Gerente',N'REPORTES_VER'),(N'Gerente',N'REPORTES_VENTAS'),(N'Gerente',N'REPORTES_RECAUDACION'),
        (N'Gerente',N'REPORTES_PRODUCTOS'),(N'Gerente',N'REPORTES_STOCK'),(N'Gerente',N'REPORTES_RENDIMIENTO_VENDEDORES'),
        (N'Gerente',N'REPORTES_DETALLE_VENTAS'),(N'Gerente',N'REPORTES_EXPORTAR'),(N'Gerente',N'REPORTES_ALCANCE_SUCURSAL'),(N'Gerente',N'AVISOS_VER'),(N'Gerente',N'AVISOS_PUBLICAR'),
        (N'Vendedor',N'VENTAS_VER'),(N'Vendedor',N'VENTAS_REALIZAR'),(N'Vendedor',N'CLIENTES_VER'),(N'Vendedor',N'CLIENTES_ALTA'),
        (N'Vendedor',N'PRODUCTOS_VER'),(N'Vendedor',N'AVISOS_VER'),(N'Vendedor',N'REPORTES_VENDEDOR'),(N'Vendedor',N'REPORTES_VER'),(N'Vendedor',N'REPORTES_VENTAS'),
        (N'Vendedor',N'REPORTES_RECAUDACION'),(N'Vendedor',N'REPORTES_PRODUCTOS'),(N'Vendedor',N'REPORTES_DETALLE_VENTAS'),(N'Vendedor',N'REPORTES_ALCANCE_PROPIO');

    INSERT INTO dbo.PERFIL_FUNCIONALIDAD(id_perfil,id_funcionalidad)
    SELECT p.id_perfil,f.id_funcionalidad
    FROM @PermisosNoGlobales AS png
    INNER JOIN dbo.PERFIL AS p ON p.nombre = png.perfil
    INNER JOIN dbo.FUNCIONALIDAD AS f ON f.codigo = png.codigo;

    /* 03 define la regla autoritativa: el perfil global recibe todas
       las funcionalidades activas, salvo los alcances incompatibles. */
    DECLARE @CodigoResultado INT, @MensajeResultado NVARCHAR(250);
    EXEC dbo.sp_Perfil_SincronizarAdministrador
        @CodigoResultado = @CodigoResultado OUTPUT,
        @MensajeResultado = @MensajeResultado OUTPUT;

    IF @CodigoResultado <> 0
        THROW 51003, 'No se pudieron reconstruir los permisos del perfil global.', 1;

    /* -----------------------------------------------------
       3. Restaurar ubicacion, sucursal y administrador de 02.
       ----------------------------------------------------- */
    IF NOT EXISTS (SELECT 1 FROM dbo.PROVINCIA WHERE nombre = N'Corrientes')
        THROW 51004, 'Falta la provincia inicial Corrientes. Ejecute 02_DatosIniciales.sql.', 1;

    IF EXISTS
    (
        SELECT 1 FROM dbo.LOCALIDAD AS l
        INNER JOIN dbo.PROVINCIA AS p ON p.id_provincia = l.id_provincia
        WHERE p.nombre = N'Corrientes' AND l.nombre = N'Corrientes'
    )
    BEGIN
        UPDATE l SET l.codigo_postal = N'3400', l.eliminado_en = NULL
        FROM dbo.LOCALIDAD AS l
        INNER JOIN dbo.PROVINCIA AS p ON p.id_provincia = l.id_provincia
        WHERE p.nombre = N'Corrientes' AND l.nombre = N'Corrientes';
    END
    ELSE
    BEGIN
        INSERT INTO dbo.LOCALIDAD(id_provincia,nombre,codigo_postal)
        SELECT id_provincia,N'Corrientes',N'3400' FROM dbo.PROVINCIA WHERE nombre=N'Corrientes';
    END;

    IF EXISTS
    (
        SELECT 1 FROM dbo.DIRECCION AS d
        INNER JOIN dbo.LOCALIDAD AS l ON l.id_localidad=d.id_localidad
        INNER JOIN dbo.PROVINCIA AS p ON p.id_provincia=l.id_provincia
        WHERE p.nombre=N'Corrientes' AND l.nombre=N'Corrientes' AND d.calle=N'Junín' AND d.altura=N'2064'
    )
    BEGIN
        UPDATE d SET d.eliminado_en=NULL
        FROM dbo.DIRECCION AS d
        INNER JOIN dbo.LOCALIDAD AS l ON l.id_localidad=d.id_localidad
        INNER JOIN dbo.PROVINCIA AS p ON p.id_provincia=l.id_provincia
        WHERE p.nombre=N'Corrientes' AND l.nombre=N'Corrientes' AND d.calle=N'Junín' AND d.altura=N'2064';
    END
    ELSE
    BEGIN
        INSERT INTO dbo.DIRECCION(id_localidad,calle,altura)
        SELECT l.id_localidad,N'Junín',N'2064'
        FROM dbo.LOCALIDAD AS l INNER JOIN dbo.PROVINCIA AS p ON p.id_provincia=l.id_provincia
        WHERE p.nombre=N'Corrientes' AND l.nombre=N'Corrientes';
    END;

    DECLARE @IdDireccionCentral INT;
    SELECT TOP 1 @IdDireccionCentral=d.id_direccion
    FROM dbo.DIRECCION AS d
    INNER JOIN dbo.LOCALIDAD AS l ON l.id_localidad=d.id_localidad
    INNER JOIN dbo.PROVINCIA AS p ON p.id_provincia=l.id_provincia
    WHERE p.nombre=N'Corrientes' AND l.nombre=N'Corrientes' AND d.calle=N'Junín' AND d.altura=N'2064'
    ORDER BY d.id_direccion;

    IF EXISTS (SELECT 1 FROM dbo.SUCURSAL WHERE nombre=N'Sucursal Central')
        UPDATE dbo.SUCURSAL SET id_direccion=@IdDireccionCentral,telefono=NULL,eliminado_en=NULL WHERE nombre=N'Sucursal Central';
    ELSE
        INSERT INTO dbo.SUCURSAL(nombre,telefono,id_direccion) VALUES(N'Sucursal Central',NULL,@IdDireccionCentral);

    DECLARE @IdPerfilAdministrador INT;
    SELECT @IdPerfilAdministrador=id_perfil FROM dbo.PERFIL WHERE nombre=N'Administrador' AND alcance_global=1 AND eliminado_en IS NULL;

    IF EXISTS (SELECT 1 FROM dbo.USUARIO WHERE nombre_usuario=N'admin')
        UPDATE dbo.USUARIO
        SET id_perfil=@IdPerfilAdministrador,id_sucursal=NULL,nombre=N'Administrador',apellido=N'Inicial',dni=N'90000000',telefono=NULL,
            contrasena_hash=N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',correo=N'admin@sistemagestion.local',sexo=NULL,fecha_nacimiento=NULL,id_direccion=NULL,eliminado_en=NULL
        WHERE nombre_usuario=N'admin';
    ELSE
        INSERT INTO dbo.USUARIO(id_perfil,id_sucursal,nombre,apellido,dni,telefono,nombre_usuario,contrasena_hash,correo)
        VALUES(@IdPerfilAdministrador,NULL,N'Administrador',N'Inicial',N'90000000',NULL,N'admin',N'100000.zpJ5ba3fjhu0UZQQlS0CSA==.aYlVb4EHb1iq2DDfmmZ/PYf/+s/KygiBFxj6yvyEmAI=',N'admin@sistemagestion.local');

    /* Direcciones y localidades agregadas en testing se eliminan solo
       cuando ya no estan referenciadas; provincias y metodos se preservan. */
    DELETE d
    FROM dbo.DIRECCION AS d
    WHERE d.id_direccion <> @IdDireccionCentral
      AND NOT EXISTS (SELECT 1 FROM dbo.CLIENTE AS c WHERE c.id_direccion=d.id_direccion)
      AND NOT EXISTS (SELECT 1 FROM dbo.USUARIO AS u WHERE u.id_direccion=d.id_direccion)
      AND NOT EXISTS (SELECT 1 FROM dbo.SUCURSAL AS s WHERE s.id_direccion=d.id_direccion);

    DELETE l
    FROM dbo.LOCALIDAD AS l
    INNER JOIN dbo.PROVINCIA AS p ON p.id_provincia=l.id_provincia
    WHERE NOT (p.nombre=N'Corrientes' AND l.nombre=N'Corrientes')
      AND NOT EXISTS (SELECT 1 FROM dbo.DIRECCION AS d WHERE d.id_localidad=l.id_localidad);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO
