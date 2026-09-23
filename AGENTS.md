# AGENTS.md — Sistema Hierro y Forja

Documentación técnica verificada del proyecto final de Taller de Programación II. Mantenerla sincronizada con código y scripts; no describe funcionalidades inexistentes.

## 1. Estado, tecnología y solución

Aplicación de escritorio para gestión de múltiples sucursales desarrollada con C#, Windows Forms y .NET 10 sobre SQL Server. Dependencias verificadas: `Microsoft.Data.SqlClient` 7.0.2 en `Capa_Datos`, `System.Text.Json` para leer configuración y `ClosedXML` 0.105.1 en `Capa_Vistas` para Excel real (`.xlsx`). La solución `Sistema_Hierro_Y_Forja.slnx` contiene exactamente tres proyectos:

```text
Capa_Vistas → Capa_Logica → Capa_Datos → SQL Server
```

El punto de entrada es `Capa_Vistas/Program.cs` (`FormLogin`). `Capa_Datos/Conexion.cs` carga la configuración local desde `Configuracion/configuracion.json`; no versionar secretos. Los directorios `SistemaGestion.*` son restos fuera de la solución y no deben reutilizarse.

## 2. Arquitectura obligatoria

* **Capa_Vistas:** formularios, Designer, eventos, navegación, permisos visuales, responsive y `FormMensaje`. No referencia Datos, no abre conexiones y no ejecuta SQL.
* **Capa_Logica:** validaciones autoritativas, reglas, cálculos, autenticación, sesión y autorización. Consulta Datos; no contiene formularios, interfaces ni `MessageBox`.
* **Capa_Datos:** conexiones, parámetros, modelos de transporte y procedimientos almacenados. No depende de Vistas/Lógica ni muestra interfaces.

La Vista previene formatos y brinda feedback; Lógica valida reglas, permisos, alcance y contexto; SQL mantiene `NOT NULL`, `UNIQUE`, `CHECK`, FK, transacciones y controles finales. Los errores suben de Datos → Lógica → Vista y se presentan mediante `FormMensaje`. No poner SQL en Vista, MessageBox en Datos, credenciales hardcodeadas, perfiles/IDs fijos ni bajas físicas indebidas.

## 3. Clases y responsabilidades

Datos incluye `Conexion`, `UsuarioDatos`, `PerfilGestionDatos`, `ClienteDatos`, `DireccionDatos`, `ProductoDatos`, `CatalogoDatos`, `InventarioDatos`, `VentaDatos`, `ReporteDatos`, `SucursalDatos`, `DashboardDatos`, `AuditoriaDatos` y `BackupDatos`. Lógica incluye sus coordinadores equivalentes (`UsuarioLogica`, `PerfilLogica`, `ClienteLogica`, `ProductoLogica`, `CatalogoLogica`, `InventarioLogica`, `VentaLogica`, `ReporteLogica`, `SucursalLogica`, `DashboardLogica`, `AuditoriaLogica`, `BackupLogica`), además de `SesionActual` y `PasswordHelper`. Vistas contiene Login, Principal/Inicio, Usuarios, Mi Perfil, Sucursales, Clientes, Productos, Categorías, Marcas, Ventas, Reportes, Back Up, detalles/listados, `FormMensaje`, `FormOverlay` y `ExcelExportHelper`.

## 4. Base de datos

`01_Estructura.sql` define **20 tablas**: `PROVINCIA`, `LOCALIDAD`, `DIRECCION`, `PERFIL`, `FUNCIONALIDAD`, `SUCURSAL`, `CATEGORIA`, `MARCA`, `MARCA_CATEGORIA`, `METODO_PAGO`, `PERFIL_FUNCIONALIDAD`, `USUARIO`, `CLIENTE`, `PRODUCTO`, `INVENTARIO`, `VENTA`, `DETALLE_VENTA`, `PAGO`, `AVISO` y `AUDITORIA`. También define `dbo.VentaItemTipo` y `dbo.VentaPagoTipo`. Back Up no tiene tabla propia.

Se verificaron **80 procedimientos** en `03_Procedimientos.sql`. Ejemplos: `sp_Usuario_BuscarPorNombreUsuario`, `sp_Usuario_Alta/Modificar/Baja/Reactivar`, `sp_Perfil_ObtenerFuncionalidades`, `sp_Perfil_ListarFuncionalidades`, `sp_Perfil_Guardar`, `sp_Cliente_Buscar/Alta/Modificar/Baja/Reactivar`, `sp_Direccion_Alta/Modificar`, `sp_Producto_Buscar/Alta/Modificar/Baja`, `sp_Inventario_EstablecerStock/StockBajo`, `sp_Venta_Registrar`, `sp_Venta_ObtenerDetalle`, `sp_Reporte_Resumen`, `sp_Reporte_ProductosMasVendidos`, `sp_Reporte_VentasPorVendedor`, `sp_Reporte_StockBajo`, `sp_Reporte_DetalleVentas`, `sp_Sucursal_ListarResumenUsuarios/Alta/Modificar/Baja/Reactivar`, `sp_Aviso_Publicar/ListarParaUsuario`, `sp_Auditoria_Registrar/Listar` y los procedimientos de Dashboard.

Scripts oficiales:

```text
Producción: 01_Estructura.sql → 02_DatosIniciales.sql → 03_Procedimientos.sql
Testing:    01 → 02 → 03 → 05_ResetBasePruebas.sql → 04_DatosPrueba.sql
Reset:      05_ResetBasePruebas.sql
```

`02_DatosIniciales.sql` es idempotente y contiene **44 funcionalidades activas**. `05_ResetBasePruebas.sql` conserva ese catálogo. `04_DatosPrueba.sql` es exclusivo de desarrollo. `BaseDatos/Historico/05_CatalogoInicial.sql` queda fuera del flujo oficial.

## 5. Catálogo y autorización

El catálogo contiene CRUD independiente `VER/ALTA/MODIFICAR/BAJA` para `USUARIOS`, `CLIENTES`, `PRODUCTOS`, `CATEGORIAS`, `MARCAS` y `SUCURSALES`; `PERMISOS_GESTIONAR`; `VENTAS_VER`, `VENTAS_REALIZAR`; `AVISOS_VER`, `AVISOS_PUBLICAR`; `BACKUP_REALIZAR`; y los permisos granulares de Reportes: `REPORTES_VER`, `REPORTES_VENTAS`, `REPORTES_RECAUDACION`, `REPORTES_PRODUCTOS`, `REPORTES_STOCK`, `REPORTES_RENDIMIENTO_VENDEDORES`, `REPORTES_DETALLE_VENTAS`, `REPORTES_EXPORTAR`, `REPORTES_ALCANCE_PROPIO`, `REPORTES_ALCANCE_SUCURSAL`, `REPORTES_ALCANCE_GLOBAL`. Se conservan por compatibilidad `REPORTES_ADMINISTRADOR`, `REPORTES_GERENTE` y `REPORTES_VENDEDOR`, pero no autorizan el flujo nuevo.

`SesionActual` carga funcionalidades desde `PERFIL_FUNCIONALIDAD`; `TienePermiso(codigo)` es la fuente de autorización. Nunca decidir por nombre de perfil o ID fijo. `FormPrincipal` deja módulos visibles: autorizados tienen icono/texto normal y cursor Hand; no autorizados, icono atenuado, texto gris y clic bloqueado internamente. Productos y Usuarios se abren si alguna subvista está autorizada. Reportes requiere `REPORTES_VER` y al menos una sección. Back Up requiere `BACKUP_REALIZAR`.

Editar perfil es acción personal fuera de `accesosMenu`: requiere sesión válida, no `USUARIOS_MODIFICAR`; `FormMiPerfil` limita el usuario a `SesionActual.IdUsuario`.

## 6. Sesión y alcance

`SesionActual.AlcanceGlobal` indica capacidad global; `IdSucursal` es la sucursal fija asignada; `IdSucursalOperativa` es la sucursal elegida temporalmente por un global. Global inicia en “Todas las sucursales” (`IdSucursalOperativa = null`) y puede seleccionar una activa. El usuario fijo no cambia sucursal. Vender, descontar stock o registrar inventario requiere sucursal operativa concreta (`ObtenerIdSucursalOperativa()`); “Todas” no habilita esas operaciones.

## 7. Navegación y mensajes WinForms

`FormPrincipal` mantiene menú, cabecera y `pnlContenido`. `AbrirFormularioEnPanel()` carga módulos con `TopLevel = false`, `FormBorderStyle = None`, `Dock = Fill`, agrega, muestra y trae al frente el formulario; no recrea `FormPrincipal`. Designer contiene estructura; `.cs`, eventos, permisos, navegación, controles dinámicos y Resize.

`FormMensaje` centraliza éxito, advertencia, error y confirmación Sí/No. Con principal visible resuelve la instancia real, muestra `FormOverlay` sobre sus bounds, centra el diálogo usando coordenadas de pantalla y elimina el overlay en `finally`. Antes de Login conserva el owner disponible. Los formularios legacy `FormReportesGerente` y `FormReportesVendedor` aún contienen MessageBox, pero están fuera de la navegación activa; el flujo actual usa `FormReportesGeneral` y `FormMensaje`.

## 8. Módulos

* **Inicio/Dashboard:** resumen, actividad, gráficos y avisos según alcance.
* **Usuarios:** listado, búsqueda, CRUD lógico/reactivación, historial, perfiles/permisos y sucursales.
* **Perfiles:** funcionalidades dinámicas desde SQL y guardado transaccional independiente.
* **Mi Perfil:** datos propios y contraseña opcional confirmada; no rol/permisos/sucursal/estado.
* **Sucursales:** activas/inactivas, dirección, localidad/provincia, conteos, CRUD y autorización granular.
* **Clientes:** búsqueda, estados, CRUD, historial y dirección separada en calle, altura y piso nullable, con provincia/localidad.
* **Productos:** catálogo, códigos, nombre comercial, categoría, marca, CRUD e inventario; Categorías y Marcas son submódulos.
* **Categorías/Marcas:** CRUD lógico y relación muchos-a-muchos `MARCA_CATEGORIA`; marcas filtradas dinámicamente por categoría.
* **Inventario:** stock y mínimo por producto+sucursal, alcance y `PRODUCTOS_MODIFICAR`.
* **Ventas:** cliente, productos, cantidades, pagos, descuento, transacción, detalle, historial y stock.
* **Avisos:** `AVISOS_PUBLICAR` publica; `AVISOS_VER` visualiza; `id_sucursal NULL` es global.
* **Reportes:** cuatro secciones, filtros, alcance y Excel.
* **Back Up:** `.bak` real en servidor SQL, sin Restore ni tabla propia.

## 9. Reportes y Excel

`FormReportesGeneral` ofrece General, Por usuario, Ventas detalladas y Stock bajo. La primera sección y visibilidad se determinan mediante capacidades de `ReporteLogica`, no `Button.Visible`. El alcance propio, fijo o global se respeta en filtros; al fijo se informa sucursal de solo lectura y al global se permite “Todas” o una sucursal activa.

`ExcelExportHelper` + ClosedXML genera workbooks `.xlsx` reales, con hojas/tablas, encabezados, filtros, fechas y números. No es CSV renombrado ni requiere Excel instalado; no exporta botones visuales. `REPORTES_EXPORTAR` controla la acción. Cada vista tiene panel hermano y scroll vertical propio; el layout usa el viewport real y no fuerza anchos mayores que `ClientSize`.

## 10. Back Up

El flujo es `FormBackup → BackupLogica → BackupDatos`. Datos obtiene el catálogo desde el connection string y el directorio del servidor con `SERVERPROPERTY('InstanceDefaultBackupPath')`; si es NULL usa `sys.dm_server_registry`. No hay FolderBrowserDialog, ruta del cliente, `xp_cmdshell` ni Restore.

Genera `<Base>_yyyyMMdd_HHmmss.bak`, sanitiza el nombre y ejecuta `BACKUP DATABASE` con parámetros. Después llama `sp_Auditoria_Registrar` con `BACKUP / BASE_DATOS` y únicamente el nombre del archivo. No registra credenciales, connection strings ni secretos. No existe tabla de Back Up.

## 11. Validación, errores y auditoría

Vista valida obligatorios, longitud, Unicode, números, calle/altura/piso, barcode, cantidades, fechas y combos. Lógica valida reglas autoritativas, unicidad, permisos, alcance y relaciones. SQL valida integridad y transacciones. Datos usa conexiones parametrizadas y `using`; Lógica traduce resultados; Vista conserva formularios y muestra `FormMensaje`.

`AUDITORIA` registra usuario, fecha, acción, entidad, entidad afectada, detalle y sucursal. `sp_Auditoria_Registrar/Listar` cubren usuarios, perfiles/permisos, clientes, productos, categorías, marcas, inventario, sucursales, ventas, avisos y Back Up. Nunca registrar contraseñas, hashes, credenciales o connection strings.

## 12. Ventas, stock y concurrencia

La Vista arma el modelo; `VentaLogica` valida cliente, ítems, pagos, duplicados, importes, permiso y sucursal; `VentaDatos` crea los TVP `dbo.VentaItemTipo` y `dbo.VentaPagoTipo` y llama `sp_Venta_Registrar`.

El procedimiento vuelve a validar usuario, `VENTAS_REALIZAR`, sucursal, alcance, cliente, productos, pagos y total dentro de una transacción. Comprueba stock con `UPDLOCK, HOLDLOCK` sobre `INVENTARIO`, evitando que ventas concurrentes consuman las mismas unidades; inserta venta/detalle/pagos, descuenta stock y hace COMMIT. Stock insuficiente o cualquier error produce ROLLBACK completo. Esta garantía depende del SQL actual y debe reevaluarse si se cambia el procedimiento o el aislamiento.

## 13. Pruebas, diseño y seguridad

`04_DatosPrueba.sql` contiene usuarios, sucursales, productos, inventario, ventas y auditoría para testing. `CREDENCIALES_PRUEBA.txt` es local e ignorado; nunca documentar secretos. Tema visual: charcoal/dorado/blanco. Resize debe usar ClientSize y paneles estables; no acumular Top/Height ni recrear controles Designer.

Antes de modificar, inspeccionar flujo completo, confirmar capa y probar GUI: un build correcto no garantiza runtime correcto. Mantener scripts idempotentes, actualizar `03_Procedimientos.sql` al crear/modificar SP, no duplicar permisos, no hardcodear perfiles, respetar capas y no hacer commit/push/merge automáticamente.

## 14. Discrepancias de la documentación anterior y pendientes

La documentación previa decía CSV, pero el estado real es Excel `.xlsx` con ClosedXML. No documentaba Back Up, que ahora existe como generación server-side con `BACKUP_REALIZAR`. También omitía los conteos verificados: 20 tablas, 80 procedimientos y 44 funcionalidades. Los formularios de Reportes por perfil son legacy y no forman parte de la navegación actual.

Pendientes reales: prueba funcional integral en GUI y base limpia; revisión de despliegue/instalador; configuración de SQL Server central (red, TCP/IP, firewall y cuenta de servicio); validación manual de scripts en otra instancia. No están implementados Restore, historial propio de copias ni tabla adicional de backups.
