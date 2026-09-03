AGENTS.md — Sistema Hierro y Forja / Taller de Programación II

Documento operativo del proyecto para integrantes del equipo y agentes de IA.
Última actualización: 2026-09-02.
Rama de integración: desarrollo.
Ramas personales: exe-dev y josi-dev.

<!-- ===================================================================== -->

<!-- ========================= 1. CONTEXTO ================================ -->

<!-- ===================================================================== -->

1. Contexto

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

2. Arquitectura

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

<!-- ===================================================================== -->

<!-- ====================== 3. ESTRUCTURA ACTUAL ========================= -->

<!-- ===================================================================== -->

3. Estructura actual

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

4. Base de datos

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

5. Configuración y seguridad

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

6. Autenticación y permisos

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

SesionActual mantiene:

usuario;

perfil;

sucursal;

estado de sesión;

funcionalidades permitidas.

Consulta:

SesionActual.TienePermiso("VENTAS_VER")

Los permisos provienen de SQL Server.

<!-- ===================================================================== -->

<!-- ======================== 7. VISTAS WINFORMS ========================= -->

<!-- ===================================================================== -->

7. Convención de vistas

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

8. FormPrincipal

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

9. Convenciones

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

10. Git

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

11. División actual

Exequiel

Principalmente:

infraestructura;

base de datos;

autenticación;

sesión;

permisos;

lógica;

acceso a datos;

procedimientos;

integración general.

Josias

Principalmente:

vistas particulares.

Previsto:

FormVentas
FormClientes
FormProductos
FormUsuarios
FormReportes

FormPrincipal ya existe y no debe recrearse.

Las vistas particulares deben cargarse dentro de pnlContenido.

<!-- ===================================================================== -->

<!-- =================== 12. RESTRICCIONES PARA AGENTES ================== -->

<!-- ===================================================================== -->

12. Reglas para agentes de IA

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

respetar las capas;

comentar código importante;

actualizar scripts cuando corresponda;

compilar después de cambios importantes;

informar archivos modificados;

actualizar este documento al cerrar hitos.

<!-- ===================================================================== -->

<!-- ======================= 13. ESTADO COMPLETADO ======================= -->

<!-- ===================================================================== -->

13. Estado completado

Hasta 2026-09-02:

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

04_DatosPrueba.sql para desarrollo.

<!-- ===================================================================== -->

<!-- ================ 14. PROYECCIÓN / TRABAJO PENDIENTE ================= -->

<!-- ===================================================================== -->

14. Proyección pendiente

Esta sección debe reducirse a medida que se completa el proyecto.

Cuando algo se termina:

quitarlo de aquí;

agregar el hito importante a Estado completado;

actualizar reglas si cambió alguna decisión.

Próximo objetivo

Integrar las vistas particulares dentro de FormPrincipal -> pnlContenido.

Usuarios y permisos

listar;

alta;

modificación;

baja lógica;

gestionar perfiles/permisos;

cambio o restablecimiento de contraseña.

Clientes

listar y buscar;

alta;

modificación;

baja lógica;

dirección.

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