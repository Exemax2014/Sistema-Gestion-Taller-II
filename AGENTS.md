# AGENTS.md — Sistema Hierro y Forja / Taller de Programación II

> Documento operativo del proyecto para integrantes del equipo y agentes de IA.
> Última actualización: 2026-09-02.
> Rama de integración: `desarrollo`.
> Ramas personales: `exe-dev` y `josi-dev`.

---

# 1. Contexto y objetivo del proyecto

Sistema Hierro y Forja es el proyecto final de Taller de Programación II.

Se desarrolla una aplicación de escritorio en C# con Windows Forms y SQL Server para gestionar un negocio con múltiples sucursales.

El sistema contempla:

- autenticación de usuarios;
- perfiles y permisos;
- clientes;
- productos y categorías;
- inventario por sucursal;
- ventas;
- detalle de ventas;
- métodos de pago y pagos;
- reportes;
- backup;
- funcionamiento futuro con varias PCs conectadas a una base SQL Server central.

La aplicación debe mantener una arquitectura estricta de tres capas.

---

# 2. Tecnologías utilizadas

- C#
- Windows Forms
- .NET 10
- SQL Server
- Microsoft.Data.SqlClient
- System.Text.Json
- Visual Studio
- SQL Server Management Studio
- Git
- GitHub

---

# 3. Arquitectura obligatoria

La solución está dividida en tres capas:

```text
Capa_Vistas
     ↓
Capa_Logica
     ↓
Capa_Datos
     ↓
SQL Server
```

## Capa_Vistas

Responsabilidades:

- formularios Windows Forms;
- controles;
- eventos;
- navegación;
- presentación de mensajes;
- interacción directa con el usuario.

No debe:

- ejecutar SQL;
- abrir conexiones SQL;
- llamar directamente a `Capa_Datos`.

## Capa_Logica

Responsabilidades:

- validaciones;
- reglas del negocio;
- autenticación;
- verificación de contraseñas;
- sesión;
- permisos;
- cálculos;
- coordinación entre Vistas y Datos.

No debe:

- contener formularios;
- mostrar `MessageBox`;
- ejecutar consultas SQL directamente.

## Capa_Datos

Responsabilidades:

- conexión a SQL Server;
- ejecución de procedimientos almacenados;
- envío de parámetros;
- lectura y transformación de resultados provenientes de SQL Server.

No debe:

- mostrar `MessageBox`;
- contener lógica visual;
- depender de `Capa_Logica` o `Capa_Vistas`.

## Referencias permitidas

```text
Capa_Vistas -> Capa_Logica
Capa_Logica -> Capa_Datos
Capa_Datos -> SQL Server
```

No crear:

```text
Capa_Vistas -> Capa_Datos
```

No crear una cuarta capa sin una decisión explícita del equipo.

---

# 4. Estructura actual del repositorio

```text
Sistema_Hierro_y_Forja/
│
├── BaseDatos/
│   ├── 01_Estructura.sql
│   ├── 02_DatosIniciales.sql
│   └── 03_Procedimientos.sql
│
├── Capa_Datos/
│   ├── Configuracion/
│   │   ├── configuracion.example.json
│   │   └── configuracion.json
│   ├── Conexion.cs
│   ├── UsuarioDatos.cs
│   └── Capa_Datos.csproj
│
├── Capa_Logica/
│   ├── PasswordHelper.cs
│   ├── SesionActual.cs
│   ├── UsuarioLogica.cs
│   └── Capa_Logica.csproj
│
├── Capa_Vistas/
│   ├── FormLogin.cs
│   ├── FormLogin.Designer.cs
│   ├── FormLogin.resx
│   ├── Program.cs
│   └── Capa_Vistas.csproj
│
├── Sistema_Hierro_Y_Forja.slnx
├── AGENTS.md
├── README.md
├── .gitignore
└── .gitattributes
```

Las carpetas físicas y proyectos utilizan actualmente:

```text
Capa_Datos
Capa_Logica
Capa_Vistas
```

No volver a utilizar la nomenclatura anterior de los proyectos.

Los archivos iniciales `Class1.cs` y `Form1` fueron eliminados.

---

# 5. Estado funcional actual

## 5.1 Conexión a SQL Server

`Capa_Datos/Conexion.cs` administra la creación de conexiones SQL.

Utiliza:

```text
Microsoft.Data.SqlClient
```

La configuración se obtiene desde:

```text
AppContext.BaseDirectory/Configuracion/configuracion.json
```

La cadena de conexión no está hardcodeada dentro del código.

La configuración permite cambiar:

- servidor;
- base de datos;
- autenticación Windows o SQL;
- usuario;
- contraseña;
- TrustServerCertificate.

Si el archivo falta o tiene datos inválidos, debe generarse un error explícito.

No utilizar valores silenciosos por defecto.

---

# 6. Configuración local y despliegue

Dentro de:

```text
Capa_Datos/Configuracion/
```

existen:

```text
configuracion.example.json
configuracion.json
```

## configuracion.example.json

Se versiona en GitHub.

Sirve como plantilla para nuevas PCs o desarrolladores.

Formato:

```json
{
  "Servidor": "localhost",
  "BaseDatos": "SistemaGestion",
  "AutenticacionWindows": true,
  "Usuario": "",
  "Contrasena": "",
  "TrustServerCertificate": true
}
```

## configuracion.json

Es la configuración real de cada PC.

Está excluido mediante `.gitignore`:

```text
Capa_Datos/Configuracion/configuracion.json
```

No subirlo a GitHub.

Cada desarrollador o instalación debe tener su propia copia.

Para desarrollo actual:

```text
Servidor: localhost
Base de datos: SistemaGestion
Autenticación: Windows
```

Para el despliegue final se pretende poder utilizar:

```text
PC Cliente 1 ─┐
PC Cliente 2 ─┼──> SQL Server central ──> SistemaGestion
PC Cliente 3 ─┘
```

El servidor podrá cambiarse mediante `configuracion.json` sin modificar ni recompilar el código.

No utilizar `sa` como usuario SQL de la aplicación final.

---

# 7. Base de datos actual

Nombre:

```text
SistemaGestion
```

El nombre `SistemaGestion` sigue siendo válido como nombre de la base de datos y no debe confundirse con antiguos nombres de proyectos.

## Tablas

1. `PROVINCIA`
2. `LOCALIDAD`
3. `DIRECCION`
4. `PERFIL`
5. `FUNCIONALIDAD`
6. `SUCURSAL`
7. `CATEGORIA`
8. `METODO_PAGO`
9. `PERFIL_FUNCIONALIDAD`
10. `USUARIO`
11. `CLIENTE`
12. `PRODUCTO`
13. `INVENTARIO`
14. `VENTA`
15. `DETALLE_VENTA`
16. `PAGO`

## Relaciones principales

```text
PROVINCIA -> LOCALIDAD -> DIRECCION

DIRECCION -> SUCURSAL
DIRECCION -> USUARIO
DIRECCION -> CLIENTE

PERFIL -> USUARIO

PERFIL
   ↕
PERFIL_FUNCIONALIDAD
   ↕
FUNCIONALIDAD

CATEGORIA -> PRODUCTO

PRODUCTO + SUCURSAL -> INVENTARIO

CLIENTE -> VENTA
USUARIO -> VENTA
SUCURSAL -> VENTA

VENTA -> DETALLE_VENTA
PRODUCTO -> DETALLE_VENTA

VENTA -> PAGO
METODO_PAGO -> PAGO
```

---

# 8. Decisiones actuales de diseño de base de datos

Las bajas se implementan mediante:

```text
eliminado_en
```

No agregar un segundo campo `activo` salvo decisión explícita.

`PRODUCTO.precio_venta` es una columna calculada a partir de:

```text
precio_costo
porcentaje_ganancia
```

`DETALLE_VENTA.precio_unitario` se almacena para conservar el precio histórico correspondiente a la venta.

El inventario se administra por:

```text
producto + sucursal
```

Provincia y localidad están normalizadas.

La dirección contiene localidad, calle y altura.

Los teléfonos pertenecen a usuario, cliente o sucursal y no a la dirección.

Los permisos se administran mediante:

```text
PERFIL
FUNCIONALIDAD
PERFIL_FUNCIONALIDAD
```

Evitar hardcodear todos los accesos únicamente según el nombre del perfil.

---

# 9. Scripts de base de datos

Los scripts se mantienen dentro de:

```text
BaseDatos/
```

## 01_Estructura.sql

Contiene la estructura completa necesaria para crear una base nueva.

## 02_DatosIniciales.sql

Contiene los datos mínimos iniciales:

- perfiles;
- funcionalidades;
- asignación inicial de permisos;
- métodos de pago;
- provincia;
- localidad;
- dirección inicial;
- sucursal inicial.

## 03_Procedimientos.sql

Contiene todos los procedimientos almacenados utilizados por la aplicación.

Todo procedimiento creado o modificado desde SQL Server Management Studio debe reflejarse también en este archivo.

La base local y los scripts versionados deben permanecer sincronizados.

Para futuras modificaciones adicionales del esquema utilizar nuevos scripts numerados a partir de:

```text
04_...
05_...
06_...
```

No reutilizar el número `03`, ya que corresponde a procedimientos almacenados.

---

# 10. Procedimientos almacenados actuales

Actualmente existe:

```text
dbo.sp_Usuario_BuscarPorNombreUsuario
```

Responsabilidad:

- recibe un nombre de usuario;
- busca un usuario que no esté dado de baja;
- verifica que su perfil y sucursal tampoco estén dados de baja;
- devuelve datos del usuario;
- devuelve perfil;
- devuelve sucursal;
- devuelve `contrasena_hash`.

Es utilizado desde:

```text
Capa_Datos
└── UsuarioDatos.BuscarPorNombreUsuario()
```

Flujo:

```text
UsuarioDatos
     ↓
Conexion.CrearConexion()
     ↓
dbo.sp_Usuario_BuscarPorNombreUsuario
     ↓
SQL Server
```

El procedimiento fue probado directamente desde SQL Server Management Studio.

---

# 11. Autenticación implementada

El login ya funciona contra SQL Server.

Flujo actual:

```text
FormLogin
     ↓
UsuarioLogica.IniciarSesion()
     ↓
UsuarioDatos.BuscarPorNombreUsuario()
     ↓
dbo.sp_Usuario_BuscarPorNombreUsuario
     ↓
SQL Server
     ↓
UsuarioLoginDatos
     ↓
PasswordHelper.Verificar()
     ↓
SesionActual.Iniciar()
```

## UsuarioDatos

`Capa_Datos/UsuarioDatos.cs`

Responsabilidades actuales:

- ejecutar `sp_Usuario_BuscarPorNombreUsuario`;
- enviar `@nombreUsuario` como parámetro;
- leer el resultado;
- devolver un objeto `UsuarioLoginDatos`.

No contiene la lógica de verificación de contraseña.

## PasswordHelper

`Capa_Logica/PasswordHelper.cs`

Implementa:

```text
PBKDF2 + SHA-256
```

con:

- salt aleatorio;
- hash;
- cantidad de iteraciones;
- comparación mediante `FixedTimeEquals`.

Formato almacenado:

```text
iteraciones.salt.hash
```

La contraseña original no se almacena en la base.

## UsuarioLogica

`Capa_Logica/UsuarioLogica.cs`

Responsabilidades:

- validar campos vacíos;
- buscar usuario mediante `UsuarioDatos`;
- verificar contraseña con `PasswordHelper`;
- iniciar `SesionActual` si las credenciales son válidas;
- devolver un resultado y mensaje a `Capa_Vistas`.

No muestra `MessageBox`.

## SesionActual

`Capa_Logica/SesionActual.cs`

Mantiene durante la ejecución:

- id de usuario;
- id de perfil;
- id de sucursal;
- nombre;
- apellido;
- nombre de usuario;
- perfil;
- sucursal;
- estado de sesión.

También permite cerrar y limpiar la sesión.

## FormLogin

`Capa_Vistas/FormLogin.cs`

El formulario:

- recibe usuario;
- recibe contraseña;
- llama a `UsuarioLogica`;
- muestra mensajes;
- limpia la contraseña cuando corresponde.

No consulta SQL directamente.

Actualmente, luego de un login correcto muestra temporalmente un `MessageBox` con:

- usuario;
- perfil;
- sucursal.

Ese comportamiento debe reemplazarse posteriormente por la apertura de `FormPrincipal`.

## Prueba realizada

Se verificó exitosamente un login real utilizando:

```text
FormLogin -> Capa_Logica -> Capa_Datos -> Procedimiento -> SQL Server
```

El administrador de desarrollo posee actualmente un hash válido.

No guardar en este documento contraseñas en texto plano.

---

# 12. Perfiles y permisos iniciales

## Administrador

Permisos iniciales:

- usuarios: ver;
- usuarios: alta;
- usuarios: baja;
- usuarios: modificar;
- permisos: gestionar;
- backup;
- ventas: ver;
- clientes: ver;
- clientes: alta;
- clientes: baja;
- clientes: modificar;
- productos: ver;
- productos: alta;
- productos: baja;
- productos: modificar;
- reporte administrador.

Pendiente definir si también realiza ventas.

## Gerente

Permisos iniciales:

- clientes: ver;
- clientes: alta;
- clientes: baja;
- productos: ver;
- productos: alta;
- reporte gerente.

## Vendedor

Permisos iniciales:

- ventas: ver;
- ventas: realizar;
- clientes: ver;
- clientes: alta;
- reporte vendedor.

El acceso a productos necesario durante una venta puede resolverse dentro de la funcionalidad de ventas sin habilitar necesariamente el módulo completo de productos.

---

# 13. Convenciones de nombres del proyecto

Mantener los nombres actuales.

## Proyectos

```text
Capa_Datos
Capa_Logica
Capa_Vistas
```

## Formularios

Usar:

```text
FormLogin
FormPrincipal
FormClientes
FormProductos
FormVentas
FormUsuarios
```

Convención:

```text
Form<Nombre>
```

No cambiar a `Frm...` salvo decisión explícita del equipo.

## Clases de acceso a datos

Convención:

```text
<Entidad>Datos
```

Ejemplos:

```text
UsuarioDatos
ClienteDatos
ProductoDatos
VentaDatos
```

## Clases de lógica

Convención:

```text
<Entidad>Logica
```

Ejemplos:

```text
UsuarioLogica
ClienteLogica
ProductoLogica
VentaLogica
```

---

# 14. Convención de comentarios de código

Las clases, métodos, funciones y procedimientos almacenados deben tener comentarios que permitan entender su propósito.

Comentar principalmente:

- responsabilidad de la clase;
- propósito del método;
- parámetros cuando no sean evidentes;
- reglas de negocio;
- decisiones importantes;
- operaciones que puedan resultar difíciles de comprender;
- procedimientos almacenados;
- bloques relevantes de SQL.

Los comentarios deben explicar el propósito o razón del código.

Evitar comentarios innecesarios sobre instrucciones obvias.

Ejemplo correcto:

```csharp
// Verificar la contraseña utilizando el hash almacenado
// antes de crear la sesión del usuario.
```

Evitar comentarios sin valor como:

```csharp
// Crear variable.
int numero = 1;
```

---

# 15. Convención para procedimientos almacenados

Las operaciones SQL reutilizables de la aplicación deben realizarse preferentemente mediante procedimientos almacenados cuando corresponda.

## Formato de nombre

Usar:

```text
sp_<Entidad>_<Accion>
```

Ejemplos:

```text
sp_Usuario_BuscarPorNombreUsuario
sp_Usuario_Listar
sp_Usuario_Insertar
sp_Usuario_Modificar

sp_Cliente_Listar
sp_Cliente_BuscarPorDocumento
sp_Cliente_Insertar
sp_Cliente_Modificar

sp_Producto_Listar
sp_Producto_BuscarPorCodigo
sp_Producto_Insertar
sp_Producto_Modificar

sp_Venta_Registrar
sp_Venta_BuscarPorId
```

Usar PascalCase después del prefijo `sp_`.

Evitar:

```text
sp_Prueba
sp_Consulta1
sp_Datos
sp_Algo
```

## Acciones recomendadas

Mantener palabras consistentes:

```text
Listar
BuscarPor...
Insertar
Modificar
Eliminar
Registrar
Obtener...
```

`Eliminar` puede representar una baja lógica cuando la entidad utiliza `eliminado_en`.

No asumir borrado físico.

## Parámetros

Usar nombres descriptivos:

```sql
@nombreUsuario
@idCliente
@documento
@idSucursal
```

No utilizar:

```sql
@dato
@valor
@param1
@param2
```

Los tipos y tamaños enviados desde C# deben coincidir con los definidos en SQL Server.

## Ejecución desde C#

Utilizar:

```csharp
CommandType.StoredProcedure
```

Enviar valores mediante parámetros.

No concatenar entradas del usuario dentro de sentencias SQL.

Los procedimientos almacenados deben ser invocados exclusivamente desde `Capa_Datos`.

## Documentación

Cada procedimiento debe indicar mediante comentarios:

- nombre;
- descripción;
- parámetros;
- objetivo;
- reglas relevantes.

## Versionado

Cuando se crea o modifica un procedimiento:

```text
1. Crear/modificar en SQL Server.
2. Probarlo en SSMS.
3. Actualizar BaseDatos/03_Procedimientos.sql.
4. Adaptar Capa_Datos.
5. Compilar.
6. Probar desde la aplicación.
7. Versionar el cambio en Git.
```

Durante desarrollo puede utilizarse:

```sql
CREATE OR ALTER PROCEDURE
```

en el script versionado.

---

# 16. Convenciones de acceso a datos

`Capa_Datos` debe:

- utilizar `Conexion.CrearConexion()`;
- utilizar `using` para conexiones, comandos y lectores;
- utilizar parámetros SQL;
- utilizar procedimientos almacenados cuando corresponda;
- transformar resultados SQL en objetos utilizables por `Capa_Logica`.

No concatenar datos ingresados por el usuario dentro de SQL.

No abrir conexiones desde formularios.

No mostrar mensajes visuales desde Datos.

---

# 17. Convenciones de seguridad

No almacenar contraseñas de usuarios en texto plano.

Utilizar hashes para autenticación.

Actualmente el proyecto utiliza PBKDF2 con SHA-256.

No subir a GitHub:

- credenciales reales;
- contraseñas de conexión;
- `configuracion.json`;
- secretos de producción.

No utilizar `sa` desde la aplicación final.

Las contraseñas de demostración o desarrollo no deben reutilizarse posteriormente como credenciales reales.

---

# 18. Convención para bajas

Las tablas que utilizan:

```text
eliminado_en
```

implementan baja lógica.

Al listar o buscar registros activos debe considerarse normalmente:

```sql
eliminado_en IS NULL
```

No reemplazar una baja lógica por:

```sql
DELETE
```

sin una decisión explícita del proyecto.

---

# 19. Git y ramas

Ramas:

```text
master       -> versión estable
desarrollo   -> integración
exe-dev      -> trabajo de Exequiel
josi-dev     -> trabajo de Josias
```

## Flujo normal

Antes de comenzar trabajo nuevo:

```text
desarrollo
    ↓ Pull

rama personal
    ↓
merge desarrollo
```

Durante el trabajo:

```text
modificar
   ↓
compilar/probar
   ↓
commit
   ↓
push rama personal
```

Para integrar:

```text
desarrollo
   ↓
Pull
   ↓
merge rama personal
   ↓
compilar/probar
   ↓
Push
```

Regla fundamental:

> La rama actualmente seleccionada es la que recibe el merge.

No trabajar directamente sobre `master`.

No hacer merge a `master` hasta contar con una versión estable que el equipo decida publicar.

---

# 20. División actual de trabajo

## Exequiel

Área principal:

```text
infraestructura + base de datos + autenticación
```

Actualmente implementado:

- conexión;
- configuración;
- procedimientos iniciales;
- acceso a datos del login;
- hash;
- lógica de autenticación;
- sesión;
- FormLogin.

Pendiente inmediato:

- integración con FormPrincipal;
- permisos de sesión;
- integración general.

## Josias

Área principal:

```text
FormPrincipal + vistas iniciales
```

Previsto:

```text
FormPrincipal
FormClientes
FormProductos
FormVentas
FormUsuarios
```

Debe evitar modificar simultáneamente archivos de autenticación mientras Exequiel trabaje sobre ellos, salvo coordinación previa.

---

# 21. Reglas obligatorias para agentes de IA

Antes de modificar cualquier archivo, un agente debe:

1. Leer `AGENTS.md`.
2. Leer `README.md`.
3. Revisar la estructura actual.
4. Revisar los scripts de `BaseDatos` si el cambio involucra datos.
5. Confirmar la rama actual.
6. Identificar correctamente qué capa corresponde al cambio.
7. Revisar código existente antes de crear una solución nueva.

Un agente NO debe:

- cambiar la arquitectura de tres capas;
- crear una cuarta capa sin autorización;
- agregar referencia directa `Capa_Vistas -> Capa_Datos`;
- ejecutar SQL desde formularios;
- mostrar `MessageBox` desde `Capa_Datos`;
- hardcodear credenciales reales;
- subir secretos;
- subir `configuracion.json`;
- utilizar `sa` como cuenta normal de aplicación;
- concatenar entradas del usuario dentro de SQL;
- cambiar el esquema sin actualizar scripts;
- borrar físicamente datos que utilizan baja lógica;
- cambiar convenciones de nombres sin autorización;
- modificar decisiones pendientes como si estuvieran aprobadas;
- trabajar directamente sobre `master`;
- hacer commits o push si el usuario no lo autorizó.

Un agente debe:

- realizar cambios pequeños y revisables;
- respetar las capas;
- respetar las convenciones de nombres;
- utilizar comentarios útiles;
- utilizar procedimientos almacenados según la convención del proyecto;
- actualizar scripts cuando corresponda;
- compilar después de modificar código;
- informar los archivos modificados;
- explicar cambios importantes;
- indicar si debe ejecutarse SQL adicional;
- mantener este documento actualizado cuando se cierre un hito importante.

---

# 22. Estado de avance

## 2026-09-01

Completado:

- solución inicial;
- arquitectura de tres capas;
- referencias entre proyectos;
- Git y ramas;
- SQL Server instalado;
- base `SistemaGestion`;
- modelo de 16 tablas;
- claves foráneas;
- datos iniciales;
- scripts de estructura y datos probados.

## 2026-09-02 — reorganización

Completado:

- proyectos renombrados;
- carpetas físicas normalizadas;
- solución renombrada;
- `Class1.cs` eliminado;
- `Form1` eliminado;
- `FormLogin` creado;
- `Program.cs` inicia `FormLogin`;
- `Microsoft.Data.SqlClient` agregado;
- configuración JSON externalizada;
- `configuracion.json` excluido de Git.

## 2026-09-02 — autenticación

Completado:

- `Conexion.cs` validado;
- `03_Procedimientos.sql` creado;
- `sp_Usuario_BuscarPorNombreUsuario` creado y probado;
- `UsuarioDatos.cs` implementado mediante procedimiento almacenado;
- `PasswordHelper.cs` implementado;
- PBKDF2 + SHA-256 implementado;
- administrador de desarrollo actualizado con hash válido;
- `SesionActual.cs` implementado;
- `UsuarioLogica.cs` implementado;
- `FormLogin` conectado a `Capa_Logica`;
- login real contra SQL Server probado correctamente;
- recuperación correcta de usuario, perfil y sucursal.

---

# 23. Decisiones todavía pendientes

No asumir sin consultar al equipo:

- si Administrador puede realizar ventas;
- alcance final de permisos del Vendedor;
- contenido exacto de cada reporte;
- tipos de factura;
- reglas finales de descuento;
- datos definitivos de la sucursal;
- permisos adicionales;
- comportamiento final de bajas de ventas y pagos;
- política completa para cambio/restablecimiento de contraseñas;
- cuenta SQL definitiva para despliegue;
- datos de producción.

---

# 24. Próximo objetivo inmediato

Completar el flujo:

```text
FormLogin
     ↓
autenticación correcta
     ↓
SesionActual
     ↓
FormPrincipal
```

El `MessageBox` temporal de login correcto debe ser reemplazado por la apertura de `FormPrincipal`.

Después integrar las vistas desarrolladas por Josias.

---

# 25. Proyección de desarrollo

## Fase 1 — Base funcional

Estado:

```text
Login                  -> implementado
Conexión SQL           -> implementada
Hash                   -> implementado
Sesión                 -> implementada
FormPrincipal          -> pendiente de integrar
Permisos en navegación -> pendiente
```

## Fase 2 — Usuarios y permisos

Desarrollar:

- listar usuarios;
- alta;
- modificación;
- baja lógica;
- perfiles;
- funcionalidades;
- asignación de permisos;
- proteger accesos según sesión.

Procedimientos correspondientes deberán incorporarse a `03_Procedimientos.sql`.

## Fase 3 — Clientes

Desarrollar:

- listar;
- buscar;
- alta;
- modificación;
- baja lógica;
- dirección.

## Fase 4 — Productos y categorías

Desarrollar:

- categorías;
- productos;
- costos;
- porcentaje de ganancia;
- precio de venta calculado;
- búsquedas;
- altas;
- modificaciones;
- bajas.

## Fase 5 — Inventario

Desarrollar:

- stock por producto y sucursal;
- stock mínimo;
- actualización de stock;
- consultas de disponibilidad.

## Fase 6 — Ventas

Desarrollar:

- selección de cliente;
- búsqueda de productos;
- detalle de venta;
- cantidades;
- precios históricos;
- subtotal;
- descuentos;
- total;
- método o métodos de pago;
- actualización de stock;
- transacción SQL para mantener consistencia.

## Fase 7 — Reportes

Desarrollar reportes diferenciados para:

- Administrador;
- Gerente;
- Vendedor.

## Fase 8 — Backup

Implementar:

- generación de backup;
- reglas de acceso;
- pruebas de restauración.

## Fase 9 — Despliegue en red

Objetivo final:

```text
PC Servidor
├── SQL Server
└── SistemaGestion

PC Cliente 1 ─┐
PC Cliente 2 ─┼── conexión por red
PC Cliente 3 ─┘
```

Configurar:

- SQL Server central;
- TCP/IP;
- firewall;
- usuario SQL específico para la aplicación;
- archivo `configuracion.json` por cliente;
- pruebas desde varias PCs;
- pruebas multisucursal.

---

# 26. Regla de continuidad

Cuando se cierre un hito importante:

- actualizar este `AGENTS.md`;
- actualizar el estado actual;
- mover tareas completadas fuera de pendientes;
- registrar nuevas decisiones;
- mantener la proyección futura actualizada.

El objetivo es que un integrante, un nuevo chat o un agente de IA pueda leer este archivo y comprender:

```text
1. Qué es el proyecto.
2. Cómo está construido.
3. Qué existe actualmente.
4. Qué reglas debe respetar.
5. Qué convenciones se utilizan.
6. Qué está pendiente.
7. Qué debe hacerse después.
```