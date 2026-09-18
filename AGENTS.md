# AGENTS.md — Sistema Hierro y Forja / Taller de Programación II

Documento operativo del proyecto para integrantes del equipo y agentes de IA.
Última actualización: 2026-09-18.

## 1. Contexto

Proyecto final de Taller de Programación II: aplicación de escritorio C# + Windows Forms + .NET 10 + SQL Server para un negocio con múltiples sucursales.

Tecnologías: C#, Windows Forms, .NET 10, SQL Server, `Microsoft.Data.SqlClient`, `System.Text.Json`, Visual Studio / SSMS y Git / GitHub.

Alcance: autenticación, perfiles y permisos, clientes, productos/categorías, inventario, ventas/pagos, reportes, backup y futura conexión de varias PCs a SQL Server central.

## 2. Arquitectura obligatoria

```text
Capa_Vistas → Capa_Logica → Capa_Datos → SQL Server
```

**Capa_Vistas:** formularios, controles, eventos, navegación y mensajes. No ejecuta SQL, no abre conexiones ni llama directamente a Capa_Datos.

**Capa_Logica:** validaciones, reglas de negocio, autenticación, sesión, permisos y cálculos. No contiene formularios, no muestra MessageBox ni ejecuta SQL directamente.

**Capa_Datos:** conexiones, procedimientos almacenados, parámetros y lectura de resultados. No muestra MessageBox, no contiene lógica visual ni depende de Vistas o Lógica.

Referencias permitidas: `Capa_Vistas -> Capa_Logica` y `Capa_Logica -> Capa_Datos`. Está prohibida la referencia `Capa_Vistas -> Capa_Datos`.

Los datos administrables (perfiles, permisos, sucursales, catálogos, estados e IDs) se obtienen dinámicamente por las tres capas; no se hardcodean en vistas ni lógica. Solo aspectos visuales o técnicos pueden definirse en código.

## 3. Estructura actual

```text
Sistema_Hierro_Y_Forja/
├── BaseDatos/
│   ├── 01_Estructura.sql
│   ├── 02_DatosIniciales.sql
│   ├── 03_Procedimientos.sql
│   ├── 04_DatosPrueba.sql
│   └── 05_CatalogoInicial.sql
├── Capa_Datos/      Conexion y servicios de usuarios, perfiles, clientes,
│                    direcciones, productos, inventario, sucursales y ventas
├── Capa_Logica/     Sesión, autenticación, permisos y lógica de esos módulos
├── Capa_Vistas/     Login, Principal/Inicio, Clientes, Productos, Usuarios,
│                    Ventas, listados/detalles, mensajes y reportes
├── Sistema_Hierro_Y_Forja.slnx
├── AGENTS.md
├── README.md
└── .gitignore
```

La solución contiene únicamente `Capa_Datos`, `Capa_Logica` y `Capa_Vistas`.

`SistemaGestion.Datos`, `SistemaGestion.Logica` y `SistemaGestion.Vistas` son restos de la nomenclatura anterior: solo contienen `bin/` y `obj/`, y no forman parte de la solución actual. No volver a utilizar esos nombres.

## 4. Base de datos

Base: `SistemaGestion`, con 18 tablas, incluida `MARCA`.

Reglas: bajas mediante `eliminado_en`; no usar borrado físico cuando corresponda baja lógica; `PRODUCTO.precio_venta` se calcula desde costo + porcentaje; `DETALLE_VENTA.precio_unitario` conserva el precio histórico; inventario por producto+sucursal; permisos por PERFIL, FUNCIONALIDAD y PERFIL_FUNCIONALIDAD.

Scripts:

- `01_Estructura.sql`: esquema e integridad.
- `02_DatosIniciales.sql`: datos y permisos iniciales.
- `03_Procedimientos.sql`: procedimientos versionados.
- `04_DatosPrueba.sql`: datos exclusivos de desarrollo/prueba.
- `05_CatalogoInicial.sql`: catálogo inicial idempotente de categorías, marcas, productos e inventario por sucursal.

Las familias actuales de procedimientos cubren autenticación y usuarios; perfiles y funcionalidades; provincias/localidades y direcciones; clientes; categorías, marcas y productos; inventario; ventas, pagos y sus consultas; reportes; sucursales y métodos de pago. Convención: `sp_<Entidad>_<Accion>`.

Todo procedimiento creado o modificado en SSMS debe actualizar también `03_Procedimientos.sql`.

## 5. Configuración y seguridad

La conexión se crea desde `Capa_Datos/Conexion.cs`. La configuración local está en `Capa_Datos/Configuracion/configuracion.json`: no se sube a Git; se versiona `configuracion.example.json`; no se hardcodean credenciales reales ni se usan contraseñas en texto plano o `sa` como cuenta normal. Autenticación con PBKDF2 + SHA-256.

## 6. Autenticación, sesión y permisos

Flujo: `FormLogin -> UsuarioLogica.IniciarSesion() -> UsuarioDatos.BuscarPorNombreUsuario() -> sp_Usuario_BuscarPorNombreUsuario -> PasswordHelper.Verificar() -> UsuarioDatos.ObtenerFuncionalidadesPerfil() -> sp_Perfil_ObtenerFuncionalidades -> SesionActual -> FormPrincipal`.

`SesionActual` mantiene usuario, perfil, sucursal asignada/operativa, estado y funcionalidades permitidas en memoria. No consulta Datos ni SQL Server. La lógica carga la sesión y expone las decisiones de permisos; las vistas no definen permisos manualmente.

## 7. Validaciones

Todo campo editable se valida en tres niveles cuando corresponda:

- Vista: prevención inmediata de formato, longitud, caracteres, obligatorios, rangos y selección.
- Lógica: regla autoritativa, reglas entre campos y contexto de sesión/permisos/estado/sucursal.
- Base: integridad final con NOT NULL, UNIQUE, CHECK, FK y procedimientos.

Nunca depender de una sola capa.

## 8. Vistas WinForms y FormPrincipal

`FormNombre.Designer.cs` contiene estructura visual; `FormNombre.cs`, eventos, navegación, carga e interacción con Capa_Logica. No recrear por código controles existentes en Designer; priorizar el Diseñador para cambios visuales.

`FormPrincipal` contiene cabecera, menú lateral, zona de usuario y `pnlContenido`. Los módulos internos se cargan en ese panel con `TopLevel = false`, `FormBorderStyle = None` y `Dock = Fill`; no repiten cabecera, menú ni cierre de sesión.

Todos los botones de menú permanecen visibles: con permiso se habilitan y sin permiso quedan grisados/deshabilitados. INICIO siempre está habilitado.

## 9. Convenciones de código

Nombres: `Form<Nombre>`, `<Entidad>Datos`, `<Entidad>Logica`, `sp_<Entidad>_<Accion>`.

En Datos usar `Conexion.CrearConexion()`, `using`, parámetros SQL y `CommandType.StoredProcedure`; nunca concatenar entradas de usuario. Cada método nuevo o reescrito de forma significativa debe llevar un comentario breve sobre su propósito y decisiones no evidentes. Los comentarios explican responsabilidades o decisiones, no instrucciones obvias.

## 10. Git

`master` es estable; `desarrollo` es la rama de integración; `exe-dev` y `josi-dev` son ramas personales.

Flujo: actualizar `desarrollo`, actualizar la rama personal, trabajar, compilar/probar, commit+push personal y merge a la rama elegida. No trabajar directamente sobre `master`.

## 11. División actual

Exequiel: Usuarios, integración, arquitectura, autenticación, sesión, permisos, lógica, datos, procedimientos y base de datos.

Josias: Clientes.

Ambos módulos respetan Vista -> Lógica -> Datos -> SQL Server, datos dinámicos y validaciones en las tres capas. Las vistas se cargan en `pnlContenido`; FormPrincipal no se recrea.

## 12. Restricciones para agentes

Antes de modificar: leer este archivo, revisar el código, confirmar la rama, identificar la capa correcta y revisar `BaseDatos/` si el cambio afecta datos.

No cambiar la arquitectura, crear una cuarta capa, crear referencia Vistas->Datos, ejecutar SQL desde formularios, mostrar MessageBox desde Datos, hardcodear credenciales o datos administrables, subir `configuracion.json`, concatenar SQL, cambiar esquema sin scripts, hacer bajas físicas indebidas, cambiar convenciones sin autorización, asumir decisiones pendientes, ni hacer commit/push/merge sin autorización.

Sí realizar cambios pequeños, reutilizar código, obtener datos dinámicos desde Lógica, validar en Vista y Lógica, respetar capas, comentar decisiones importantes, actualizar scripts cuando corresponda, compilar tras cambios relevantes, informar archivos modificados y actualizar este documento al cerrar hitos.

## 13. Estado completado

- Solución de tres capas, configuración externa, conexión SQL, autenticación PBKDF2 + SHA-256, SesionActual y permisos obtenidos desde SQL.
- Base `SistemaGestion` con 18 tablas, scripts de estructura/datos/pruebas y catálogo inicial; MARCA e inventario por producto+sucursal.
- FormPrincipal, FormInicio, navegación dentro de `pnlContenido`, cierre de sesión y menú visible condicionado por permisos.
- Clientes: listado/búsqueda por estado, alta, modificación, baja lógica, reactivación, historial de compras, dirección y provincias/localidades dinámicas; validaciones reforzadas en Vista, Lógica y SQL.
- Productos: alta/modificación con validación autoritativa unificada; límites y prevención de formato para nombre, código, costo, ganancia, stock y stock mínimo.
- Usuarios: listado, filtros, alta, modificación, baja/reactivación y carga dinámica de perfiles/sucursales. `sp_Usuario_ObtenerPorId` devuelve `activo` y admite usuarios inactivos; detalle y mensajes corregidos.
- Perfiles y permisos: gestión desde SQL y validaciones reforzadas de nombre/descripción, sin cambiar perfiles globales ni la regla de `PERMISOS_GESTIONAR`.
- Ventas: selección de cliente/productos, carrito, pagos, registro transaccional, actualización de stock, detalle/listado y aviso preventivo si falta sucursal operativa.
- Grillas de Usuarios, Clientes y Productos con estados, acciones, alineación y presentación visual unificadas.
- Reportes: formularios General, Gerente y Vendedor, con procedimientos de recaudación, productos más vendidos y ventas por vendedor.

## 14. Pendiente y decisiones abiertas

- Validaciones restantes de Ventas y refuerzo de validaciones de Usuarios.
- Inventario: prevención de entrada para stock/stock mínimo y reemplazar `InventarioLogica.PuedeModificarSucursal` para decidir por permisos y datos de `SesionActual`, no por nombre de perfil.
- Ejecutar en la instancia SQL Server los procedimientos/scripts actualizados cuando corresponda.
- Pruebas funcionales finales e integrales de módulos, restricciones y permisos; backup, despliegue SQL Server central, TCP/IP/firewall, cuenta SQL específica y pruebas multisucursal.

No asumir sin consultar: si Administrador puede realizar ventas; alcance final del Vendedor; contenido exacto de reportes; tipos de factura; reglas finales de descuento; política de cambio/restablecimiento de contraseñas; cuenta SQL definitiva; datos finales de producción.

## Regla de continuidad

Este archivo debe indicar rápidamente qué existe, cómo se estructura, qué reglas no se rompen, qué está terminado y qué falta. Mantenerlo corto y operativo.
