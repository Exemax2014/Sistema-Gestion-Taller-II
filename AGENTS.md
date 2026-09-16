AGENTS.md — Sistema Hierro y Forja / Taller de Programación II

Documento operativo del proyecto para integrantes del equipo y agentes de IA.
Última actualización: 2026-09-16.
Rama de integración: desarrollo.
Ramas personales: exe-dev y josi-dev.

<!-- ===================================================================== -->

<!-- ========================= 1. CONTEXTO ================================ -->

<!-- ===================================================================== -->

Contexto

Proyecto final de Taller de Programación II.

Aplicación de escritorio en C# + Windows Forms + SQL Server para gestionar un negocio con múltiples sucursales.

Tecnologías:

C#

Windows Forms

.NET 10

SQL Server

Microsoft.Data.SqlClient

System.Text.Json

Visual Studio / SSMS

Git / GitHub

Funciones previstas:

autenticación;

perfiles y permisos;

clientes;

productos y categorías;

inventario;

ventas y pagos;

reportes;

backup;

futura conexión de varias PCs a SQL Server central.

<!-- ===================================================================== -->

<!-- =================== 2. ARQUITECTURA OBLIGATORIA ===================== -->

<!-- ===================================================================== -->

Arquitectura

Capa_Vistas
↓
Capa_Logica
↓
Capa_Datos
↓
SQL Server

Capa_Vistas

Puede:

formularios;

controles;

eventos;

navegación;

mensajes.

No puede:

ejecutar SQL;

abrir conexiones;

llamar directamente a Capa_Datos.

Capa_Logica

Puede:

validaciones;

reglas de negocio;

autenticación;

sesión;

permisos;

cálculos.

No puede:

contener formularios;

mostrar MessageBox;

ejecutar SQL directamente.

Capa_Datos

Puede:

conexiones;

procedimientos almacenados;

parámetros;

lectura de resultados.

No puede:

mostrar MessageBox;

contener lógica visual;

depender de Vistas o Lógica.

Referencias permitidas:

Capa_Vistas -> Capa_Logica
Capa_Logica -> Capa_Datos

Prohibido:

Capa_Vistas -> Capa_Datos

Principio de datos dinámicos:

La aplicación debe ser dinámica respecto de datos, permisos, perfiles,
sucursales, catálogos, estados y reglas configurables.

Los valores que provengan o puedan administrarse desde la base de datos
deben cargarse siguiendo:

Capa_Vistas
↓
Capa_Logica
↓
Capa_Datos
↓
SQL Server

No hardcodear en formularios ni en la lógica:

nombres de perfiles para decidir permisos;

listas de perfiles, sucursales, categorías, marcas o estados administrables;

permisos;

IDs de entidades;

datos de negocio que puedan cambiar desde SQL Server.

Sí pueden estar definidos en código los aspectos puramente visuales o técnicos,
por ejemplo colores, tamaños, textos fijos de interfaz y nombres de controles,
siempre que no representen datos o reglas de negocio.

<!-- ===================================================================== -->

<!-- ====================== 3. ESTRUCTURA ACTUAL ========================= -->

<!-- ===================================================================== -->

Estructura actual

Sistema_Hierro_y_Forja/
├── BaseDatos/
│   ├── 01_Estructura.sql
│   ├── 02_DatosIniciales.sql
│   ├── 03_Procedimientos.sql
│   └── 04_DatosPrueba.sql
├── Capa_Datos/
│   ├── Configuracion/
│   ├── Conexion.cs
│   └── UsuarioDatos.cs
├── Capa_Logica/
│   ├── PasswordHelper.cs
│   ├── SesionActual.cs
│   └── UsuarioLogica.cs
├── Capa_Vistas/
│   ├── FormLogin.*
│   ├── FormPrincipal.*
│   └── Program.cs
├── Sistema_Hierro_Y_Forja.slnx
├── AGENTS.md
├── README.md
└── .gitignore

No volver a utilizar nombres antiguos de proyectos o carpetas.

<!-- ===================================================================== -->

<!-- ======================= 4. BASE DE DATOS ============================ -->

<!-- ===================================================================== -->

Base de datos

Nombre:

SistemaGestion

Tablas:
PROVINCIA, LOCALIDAD, DIRECCION, PERFIL, FUNCIONALIDAD,
PERFIL_FUNCIONALIDAD, SUCURSAL, USUARIO, CLIENTE, CATEGORIA,
PRODUCTO, INVENTARIO, VENTA, DETALLE_VENTA, METODO_PAGO, PAGO.

Reglas:

bajas mediante eliminado_en;

no usar borrado físico donde corresponda baja lógica;

PRODUCTO.precio_venta se calcula desde costo + porcentaje;

DETALLE_VENTA.precio_unitario conserva el precio histórico;

inventario por producto + sucursal;

permisos mediante PERFIL, FUNCIONALIDAD, PERFIL_FUNCIONALIDAD;

no hardcodear accesos únicamente por nombre de perfil.

Scripts de desarrollo:

01_Estructura.sql
02_DatosIniciales.sql
03_Procedimientos.sql
04_DatosPrueba.sql

04_DatosPrueba.sql es solo para desarrollo y pruebas.

Procedimientos actuales:

dbo.sp_Usuario_BuscarPorNombreUsuario
dbo.sp_Perfil_ObtenerFuncionalidades

Convención:

sp_<Entidad>_<Accion>

Todo procedimiento creado o modificado en SSMS debe actualizar también 03_Procedimientos.sql.

<!-- ===================================================================== -->

<!-- =================== 5. CONFIGURACIÓN Y SEGURIDAD ==================== -->

<!-- ===================================================================== -->

Configuración y seguridad

La conexión se crea desde:

Capa_Datos/Conexion.cs

Configuración local:

Capa_Datos/Configuracion/configuracion.json

Reglas:

configuracion.json no se sube a Git;

se versiona configuracion.example.json;

no hardcodear credenciales reales;

no guardar contraseñas en texto plano;

no usar sa como cuenta normal de la aplicación;

autenticación de usuarios con PBKDF2 + SHA-256.

<!-- ===================================================================== -->

<!-- ================== 6. AUTENTICACIÓN Y PERMISOS ====================== -->

<!-- ===================================================================== -->

Autenticación y permisos

Flujo actual:

FormLogin
↓
UsuarioLogica.IniciarSesion()
↓
UsuarioDatos.BuscarPorNombreUsuario()
↓
sp_Usuario_BuscarPorNombreUsuario
↓
PasswordHelper.Verificar()
↓
UsuarioDatos.ObtenerFuncionalidadesPerfil()
↓
sp_Perfil_ObtenerFuncionalidades
↓
SesionActual
↓
FormPrincipal

SesionActual mantiene en memoria:

usuario;

perfil;

sucursal asignada;

sucursal operativa;

estado de sesión;

funcionalidades permitidas.

SesionActual no consulta directamente Capa_Datos ni SQL Server.

Los datos de sesión y permisos se obtienen durante la autenticación mediante:

UsuarioLogica
↓
UsuarioDatos
↓
SQL Server

y luego UsuarioLogica carga:

SesionActual.Iniciar(...)

Consulta en memoria:

SesionActual.TienePermiso("VENTAS_VER")

Los permisos provienen de SQL Server y no deben definirse manualmente en la vista.

<!-- ===================================================================== -->

<!-- ======================== 7. VALIDACIONES ============================ -->

<!-- ===================================================================== -->

Validaciones

Todo campo editable por el usuario debe validarse en tres niveles cuando corresponda.

Vista:

validación preventiva e inmediata;

impedir o advertir entradas inválidas antes de enviar;

controlar formato, longitud, caracteres permitidos, campos obligatorios,
rangos y selección de opciones;

no confiar solamente en la Vista.

Capa_Logica:

repetir las validaciones de negocio antes de llamar a Capa_Datos;

ser la validación autoritativa de la aplicación;

validar reglas entre campos y reglas dependientes de sesión, permisos,
estado, sucursal u otras entidades.

Base de datos:

mantener restricciones de integridad cuando corresponda;

NOT NULL, UNIQUE, CHECK, FK y reglas implementadas mediante procedimientos
almacenados o mecanismos definidos por el proyecto.

Regla:

Vista = experiencia de usuario y prevención.
Lógica = regla de negocio y validación autoritativa.
Base de datos = integridad final.

Nunca depender de una sola capa para validar información.

<!-- ===================================================================== -->

<!-- ======================== 8. VISTAS WINFORMS ========================= -->

<!-- ===================================================================== -->

Convención de vistas

Separación obligatoria:

FormNombre.Designer.cs -> estructura visual
FormNombre.cs          -> comportamiento

Designer.cs

Debe contener principalmente:

controles;

paneles;

tamaños;

posiciones;

colores;

fuentes;

Dock / Anchor.

La vista debe poder modificarse desde el Diseñador de Visual Studio.

Preferir el Diseñador para cambios visuales.

FormNombre.cs

Debe contener:

eventos;

navegación;

carga de datos;

sesión;

permisos;

interacción con Capa_Logica.

No crear nuevamente por código controles que ya existen en Designer.

<!-- ===================================================================== -->

<!-- ========================= 8. FORM PRINCIPAL ========================= -->

<!-- ===================================================================== -->

FormPrincipal

FormPrincipal ya está implementado.

Estructura:

Cabecera
+
Menú lateral
+
Zona de usuario
+
pnlContenido

Los módulos particulares se cargan dentro de:

pnlContenido

Previstos:

FormVentas
FormClientes
FormProductos
FormUsuarios
FormReportes

Los formularios internos no deben repetir cabecera, menú ni cierre de sesión.

Configuración al cargarlos:

TopLevel = false
FormBorderStyle = None
Dock = Fill

Permisos del menú

Todos los botones permanecen visibles.

Tiene permiso    -> habilitado
No tiene permiso -> visible + deshabilitado + gris

No ocultar botones por falta de permiso.

Permisos principales:

Ventas    -> VENTAS_VER
Clientes  -> CLIENTES_VER
Productos -> PRODUCTOS_VER
Usuarios  -> USUARIOS_VER

Reportes:

REPORTES_ADMINISTRADOR

REPORTES_GERENTE

REPORTES_VENDEDOR

INICIO siempre está habilitado.

<!-- ===================================================================== -->

<!-- ====================== 9. CONVENCIONES DE CÓDIGO ==================== -->

<!-- ===================================================================== -->

Convenciones

Nombres:

Form<Nombre>
<Entidad>Datos
<Entidad>Logica
sp_<Entidad>_<Accion>

Acceso a datos:

usar Conexion.CrearConexion();

usar using;

usar parámetros SQL;

usar CommandType.StoredProcedure;

no concatenar entradas del usuario.

Comentarios:

explicar responsabilidad, propósito y decisiones importantes;

evitar comentar instrucciones obvias.

<!-- ===================================================================== -->

<!-- =========================== 10. GIT ================================= -->

<!-- ===================================================================== -->

Git

master     -> versión estable
desarrollo -> integración
exe-dev    -> Exequiel
josi-dev   -> Josias

Flujo:

actualizar desarrollo
↓
actualizar rama personal
↓
trabajar
↓
compilar/probar
↓
commit + push personal
↓
merge a desarrollo

La rama seleccionada es la que recibe el merge.

No trabajar directamente sobre master.

<!-- ===================================================================== -->

<!-- ===================== 11. DIVISIÓN DE TRABAJO ======================= -->

<!-- ===================================================================== -->

División actual

Exequiel

Responsable principal de:

módulo Usuarios;

integración general;

arquitectura;

autenticación;

sesión;

permisos;

lógica;

acceso a datos;

procedimientos;

base de datos.

Josias

Responsable principal de:

módulo Clientes.

Regla para ambos módulos:

seguir la misma arquitectura ya aplicada en Productos e Inventario;

Vista -> Lógica -> Datos -> SQL Server;

datos dinámicos;

validación preventiva en Vista;

validación de negocio en Capa_Logica;

integridad final en SQL Server;

sin acceso directo de Vistas a Capa_Datos.

FormPrincipal ya existe y no debe recrearse.

Las vistas particulares deben cargarse dentro de pnlContenido.

<!-- ===================================================================== -->

<!-- =================== 13. RESTRICCIONES PARA AGENTES ================== -->

<!-- ===================================================================== -->

Reglas para agentes de IA

Antes de modificar:

leer AGENTS.md;

revisar código existente;

confirmar rama actual;

identificar la capa correcta;

revisar BaseDatos/ si el cambio afecta datos.

NO:

cambiar la arquitectura;

crear una cuarta capa sin autorización;

crear referencia Vistas -> Datos;

ejecutar SQL desde formularios;

mostrar MessageBox desde Datos;

hardcodear credenciales;

hardcodear datos de negocio, permisos, perfiles, sucursales o catálogos administrables;

subir configuracion.json;

concatenar entradas del usuario en SQL;

cambiar esquema sin actualizar scripts;

hacer borrado físico donde exista baja lógica;

cambiar convenciones sin autorización;

asumir decisiones pendientes;

trabajar sobre master;

hacer commit, push o merge sin autorización del usuario.

SÍ:

realizar cambios pequeños;

reutilizar código existente;

preferir datos dinámicos obtenidos mediante Capa_Logica;

validar campos editables en Vista y volver a validar reglas en Capa_Logica;

respetar las capas;

comentar código importante;

actualizar scripts cuando corresponda;

compilar después de cambios importantes;

informar archivos modificados;

actualizar este documento al cerrar hitos.

<!-- ===================================================================== -->

<!-- ======================= 13. ESTADO COMPLETADO ======================= -->

<!-- ===================================================================== -->

Estado completado

Hasta 2026-09-16:

solución y arquitectura de tres capas;

Git y ramas;

base SistemaGestion con 16 tablas;

scripts de estructura y datos iniciales;

configuración JSON externa;

conexión SQL;

FormLogin;

autenticación real;

PBKDF2 + SHA-256;

SesionActual;

sp_Usuario_BuscarPorNombreUsuario;

sp_Perfil_ObtenerFuncionalidades;

carga de permisos desde SQL;

SesionActual.TienePermiso();

FormPrincipal;

diseño mediante Designer;

navegación general;

pnlContenido;

cierre de sesión;

menú condicionado por permisos;

botones sin permiso visibles, grisados y deshabilitados;

pruebas con Administrador y Vendedor;

04_DatosPrueba.sql para desarrollo;

MARCA y catálogo inicial;

inventario por PRODUCTO + SUCURSAL;

lógica de Productos e Inventario;

permisos de Productos aplicados desde Capa_Logica;

FormProductos y FormProductoDetalle;

FormClientes maquetado con columnas dinámicas fuera del Designer;

FormUsuarios maquetado con columnas dinámicas fuera del Designer;

FormReportesGeneral maquetado;

FormInicio maquetado e integrado;

FormPrincipal actualizado para cargar FormInicio y módulos dentro de pnlContenido;

patrón Designer seguro: estructura visual en Designer y comportamiento/columnas dinámicas en .cs.

<!-- ===================================================================== -->

<!-- ================ 14. PROYECCIÓN / TRABAJO PENDIENTE ================= -->

<!-- ===================================================================== -->

Proyección pendiente

Esta sección debe reducirse a medida que se completa el proyecto.

Cuando algo se termina:

quitarlo de aquí;

agregar el hito importante a Estado completado;

actualizar reglas si cambió alguna decisión.

Próximo objetivo

Completar módulos Usuarios y Clientes con el patrón aplicado en Productos/Inventario.

Usuarios — responsable: Exequiel

listar y buscar dinámicamente;

alta;

modificación;

baja lógica;

cargar perfiles y sucursales desde SQL Server;

gestionar perfiles/permisos sin hardcodear nombres de perfil;

cambio o restablecimiento de contraseña;

validar todos los campos en Vista y repetir reglas en Capa_Logica;

mantener restricciones de integridad en SQL Server.

Clientes — responsable: Josias

listar y buscar dinámicamente;

alta;

modificación;

baja lógica;

dirección;

cargar localidades/provincias dinámicamente cuando corresponda;

validar todos los campos en Vista y repetir reglas en Capa_Logica;

mantener restricciones de integridad en SQL Server.

Corrección arquitectónica pendiente

Mover de FormPrincipal y otras vistas las decisiones de permisos que todavía consultan
directamente SesionActual.TienePermiso(...), para que la Vista consulte métodos de
Capa_Logica.

Revisar ComboBox, listas, estados y datos temporales de las vistas ya maquetadas y
reemplazar valores de negocio hardcodeados por carga dinámica desde Capa_Logica.

Productos y categorías

categorías;

productos;

búsquedas;

altas/modificaciones/bajas;

costos y porcentaje de ganancia.

Inventario

stock por sucursal;

stock mínimo;

movimientos;

disponibilidad.

Ventas

nueva venta;

selección de cliente;

productos y cantidades;

precios;

descuentos;

total;

pagos;

actualización de stock;

transacción SQL.

Reportes

Administrador;

Gerente;

Vendedor.

Backup

generación;

permisos;

restauración de prueba.

Despliegue

SQL Server central;

TCP/IP y firewall;

cuenta SQL específica;

configuración por PC;

pruebas con varias PCs;

pruebas multisucursal.

Decisiones abiertas

No asumir sin consultar:

si Administrador puede realizar ventas;

alcance final del Vendedor;

contenido exacto de reportes;

tipos de factura;

reglas finales de descuento;

política de cambio/restablecimiento de contraseñas;

cuenta SQL definitiva;

datos finales de producción.

<!-- ===================================================================== -->

<!-- ======================== REGLA DE CONTINUIDAD ======================= -->

<!-- ===================================================================== -->

Regla de continuidad

Este archivo debe permitir entender rápidamente:

qué existe
cómo está estructurado
qué reglas no se pueden romper
qué está terminado
qué falta hacer

Mantenerlo corto. No convertir AGENTS.md en documentación extensa.