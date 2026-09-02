Sí. Te lo dejo **todo junto en un solo bloque**, listo para copiar completo y pegar dentro de `AGENTS.md`.

````markdown
# AGENTS.md — Sistema Hierro y Forja / Taller de Programación II

> Documento operativo para integrantes del equipo y agentes de IA.
> Estado actualizado: 2026-09-02.
> Rama de integración: `desarrollo`.

## 1. Objetivo del proyecto

Desarrollar una aplicación de escritorio en C# con Windows Forms y SQL Server para gestionar un negocio con múltiples sucursales.

El sistema debe contemplar, como mínimo, autenticación de usuarios, perfiles y permisos, clientes, productos, inventario por sucursal, ventas, pagos, reportes y backup.

La prioridad inmediata es llegar a la primera entrega con una versión navegable: login funcional, usuario autenticado, formulario principal y varias vistas visibles.

## 2. Tecnologías y arquitectura

- C#
- Windows Forms
- .NET 10
- SQL Server
- Microsoft.Data.SqlClient
- Visual Studio 2026
- Git + GitHub

La solución está dividida estrictamente en tres capas:

```text
Capa_Vistas
     ↓
Capa_Logica
     ↓
Capa_Datos
```

Reglas:

- `Capa_Vistas` contiene formularios, controles, mensajes y navegación.
- `Capa_Logica` contiene validaciones, reglas del negocio, autenticación, sesión, permisos y cálculos.
- `Capa_Datos` contiene conexión a SQL Server y operaciones SQL.
- `Capa_Vistas` no debe ejecutar SQL directamente.
- `Capa_Datos` no debe mostrar `MessageBox`.
- No agregar una referencia directa `Capa_Vistas -> Capa_Datos`.
- La cadena correcta es `Capa_Vistas -> Capa_Logica -> Capa_Datos`.
- No crear una cuarta capa sin autorización del equipo.

## 3. Estructura actual del repositorio

```text
Sistema_Hierro_y_Forja/
│
├── BaseDatos/
│   ├── 01_Estructura.sql
│   └── 02_DatosIniciales.sql
│
├── Capa_Datos/
│   ├── Configuracion/
│   │   ├── configuracion.example.json
│   │   └── configuracion.json
│   ├── Conexion.cs
│   └── Capa_Datos.csproj
│
├── Capa_Logica/
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

Actualmente:

- Las carpetas físicas con la nomenclatura anterior fueron eliminadas.
- Los proyectos se llaman `Capa_Datos`, `Capa_Logica` y `Capa_Vistas`.
- Las referencias entre proyectos están configuradas como:
  `Capa_Vistas -> Capa_Logica -> Capa_Datos`.
- Se eliminaron los archivos iniciales `Class1.cs`.
- Se eliminó el formulario inicial `Form1`.
- `FormLogin` ya fue creado.
- `Program.cs` inicia la aplicación mostrando `FormLogin`.
- `Capa_Datos` tiene instalada la dependencia `Microsoft.Data.SqlClient`.
- `Conexion.cs` está preparado para obtener la configuración desde un archivo JSON.
- La solución compila correctamente.
- La base de datos y sus scripts iniciales ya fueron diseñados y probados.

## 4. Configuración de conexión

La configuración de SQL Server se encuentra dentro de:

```text
Capa_Datos/Configuracion/
```

Archivos:

```text
configuracion.example.json
configuracion.json
```

`configuracion.example.json`:

- se versiona en Git;
- sirve como plantilla para cada desarrollador o instalación.

`configuracion.json`:

- contiene la configuración concreta de cada PC;
- está excluido mediante `.gitignore`;
- no debe subirse al repositorio.

Formato actual:

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

`Conexion.cs` lee:

```text
AppContext.BaseDirectory/Configuracion/configuracion.json
```

Si el archivo no existe o es inválido debe producir un error claro y no utilizar valores silenciosos por defecto.

La configuración está pensada para permitir que posteriormente las PCs cliente se conecten a una instancia central de SQL Server sin modificar el código fuente.

Para desarrollo se utiliza actualmente:

```text
Servidor: localhost
Base de datos: SistemaGestion
Autenticación: Windows
```

En una instalación final podrá configurarse otra IP, nombre de servidor o autenticación SQL mediante el archivo de configuración.

No utilizar la cuenta `sa` como usuario de la aplicación final.

## 5. Base de datos actual

Base principal:

```text
SistemaGestion
```

Tablas actuales:

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

Relaciones importantes:

```text
PROVINCIA -> LOCALIDAD -> DIRECCION

DIRECCION -> USUARIO
DIRECCION -> CLIENTE
DIRECCION -> SUCURSAL

PERFIL -> USUARIO
PERFIL <-> FUNCIONALIDAD por PERFIL_FUNCIONALIDAD

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

### Decisiones de diseño ya tomadas

- Las bajas se manejan mediante `eliminado_en`.
- No agregar un segundo campo `activo` salvo decisión explícita del equipo.
- `PRODUCTO.precio_venta` es una columna calculada a partir de `precio_costo` y `porcentaje_ganancia`.
- `DETALLE_VENTA.precio_unitario` sí se almacena para conservar el precio histórico de la venta.
- El inventario es por combinación `producto + sucursal`.
- Provincia y localidad se normalizaron para evitar valores repetidos o mal escritos.
- La dirección específica guarda localidad, calle y altura.
- Los teléfonos pertenecen a usuario, cliente o sucursal, no a la dirección.
- `PERFIL_FUNCIONALIDAD` permite permisos configurables y evita hardcodear todos los accesos por nombre de perfil.

## 6. Datos iniciales existentes en los scripts

`02_DatosIniciales.sql` carga:

- Perfiles: Administrador, Gerente y Vendedor.
- 19 funcionalidades.
- 27 asignaciones iniciales de permisos.
- Métodos de pago: Efectivo, Débito, Crédito y Transferencia.
- Provincia Corrientes.
- Localidad Corrientes, CP 3400.
- Dirección ficticia Junín 2064.
- Sucursal Central.

El script no crea todavía un usuario administrador definitivo.

En la base local de desarrollo se creó previamente un administrador temporal, pero su `contrasena_hash` no debe considerarse una implementación final.

## 7. Perfiles y permisos iniciales

### Administrador

Permisos iniciales:

- usuarios: ver, alta, baja y modificar;
- perfiles/permisos: gestionar;
- backup;
- ventas: ver;
- clientes: ver, alta, baja y modificar;
- productos: ver, alta, baja y modificar;
- reportes de administrador.

Pendiente definir si también puede realizar ventas.

### Gerente

Permisos iniciales:

- clientes: ver, alta y baja;
- productos: ver y alta;
- reportes de gerente.

### Vendedor

Permisos iniciales:

- ventas: ver y realizar;
- clientes: ver y alta;
- reportes de vendedor.

El acceso a productos necesario para vender puede resolverse dentro de la funcionalidad de ventas sin habilitar necesariamente el módulo completo de productos.

## 8. Git y ramas

Ramas principales:

```text
master       -> versión estable
desarrollo   -> integración del equipo
exe-dev      -> trabajo de Exequiel
josi-dev     -> trabajo de Josias
```

Flujo recomendado:

```text
Antes de trabajar:

desarrollo -> Pull
exe-dev/josi-dev -> merge de desarrollo

Durante el trabajo:

modificar -> commit -> push de rama personal

Para integrar:

desarrollo -> Pull
merge rama personal -> desarrollo
compilar/probar
push desarrollo
```

Regla clave:

> La rama actualmente seleccionada es la rama que recibe el merge.

No trabajar directamente sobre `master`.

## 9. Primera entrega — 2026-09-02

### Objetivo mínimo de demostración

La aplicación debe poder:

1. iniciar mostrando un formulario de login;
2. consultar SQL Server;
3. autenticar un usuario válido;
4. recordar el usuario, perfil y sucursal autenticados durante la sesión;
5. abrir un formulario principal;
6. mostrar varias vistas desde el menú;
7. demostrar visualmente que el proyecto está dividido en capas.

No intentar terminar todo el sistema para esta entrega.

### Resultado deseado

```text
Programa
   ↓
FormLogin
   ↓
Validación en Base de Datos
   ↓
Sesión iniciada
   ↓
FormPrincipal
   ├── FormClientes
   ├── FormProductos
   ├── FormVentas
   └── FormUsuarios
```

Las vistas secundarias pueden estar inicialmente incompletas si la consigna de la entrega solo exige mostrar avance, pero el login y la navegación principal deben funcionar.

## 10. División inmediata de tareas

### Exequiel — infraestructura + login

Responsable de:

- conexión de `Capa_Datos` con SQL Server;
- configuración de conexión sin repetirla por todo el proyecto;
- acceso a datos de usuario para login;
- lógica de autenticación;
- estrategia inicial de hash de contraseña;
- clase de sesión del usuario autenticado;
- `FormLogin`;
- `Program.cs` para iniciar por el login;
- integración del login con el formulario principal realizado por Josias;
- prueba final de integración.

Archivos previstos:

```text
Capa_Datos/
    Conexion.cs
    UsuarioDatos.cs

Capa_Logica/
    UsuarioLogica.cs
    SesionActual.cs
    PasswordHelper.cs

Capa_Vistas/
    FormLogin.cs
    FormLogin.Designer.cs
```

### Josias — formulario principal + vistas

Responsable de:

- `FormPrincipal`;
- menú principal;
- zona de navegación/contenido;
- diseño inicial de vistas:
  - `FormClientes`;
  - `FormProductos`;
  - `FormVentas`;
  - `FormUsuarios` o vista equivalente;
- botones de navegación;
- diseño consistente de formularios;
- recibir el usuario/perfil autenticado para mostrarlo en la cabecera.

Debe evitar modificar los archivos que esté trabajando Exequiel, especialmente `FormLogin`, clases de autenticación y acceso a datos.

Archivos previstos:

```text
Capa_Vistas/
    FormPrincipal.cs
    FormPrincipal.Designer.cs
    FormClientes.cs
    FormClientes.Designer.cs
    FormProductos.cs
    FormProductos.Designer.cs
    FormVentas.cs
    FormVentas.Designer.cs
    FormUsuarios.cs
    FormUsuarios.Designer.cs
```

## 11. Orden de integración para la primera entrega

1. Ambos parten desde `desarrollo` actualizado.
2. Exequiel trabaja en `exe-dev`.
3. Josias trabaja en `josi-dev`.
4. Cada uno hace commits pequeños y push de su rama.
5. Integrar primero las vistas de Josias a `desarrollo`.
6. Exequiel hace Pull de `desarrollo`.
7. Exequiel integra `desarrollo` dentro de `exe-dev`.
8. Conectar `FormLogin` con `FormPrincipal`.
9. Merge final de `exe-dev` hacia `desarrollo`.
10. Compilar y probar desde `desarrollo`.
11. No pasar a `master` hasta tener una versión demostrable estable.

## 12. Después de la primera entrega

### Fase 1 — Base funcional

- Login.
- Sesión.
- Menú según permisos.
- Conexión a SQL Server.

### Fase 2 — ABM principales

- Usuarios.
- Clientes.
- Categorías.
- Productos.
- Sucursales.
- Direcciones.

### Fase 3 — Inventario

- Stock por sucursal.
- Stock mínimo.
- Actualización de existencias.

### Fase 4 — Ventas

- Buscar cliente.
- Buscar productos.
- Agregar productos al detalle.
- Calcular subtotal, descuento y total.
- Registrar venta y detalle.
- Registrar uno o más pagos.
- Descontar stock en una transacción.

### Fase 5 — Seguridad y permisos

- Gestión de perfiles.
- Gestión de funcionalidades.
- Menú dinámico según permisos.
- Proteger `PERMISOS_GESTIONAR`.
- Garantizar al menos un administrador activo.

### Fase 6 — Reportes

- Reporte Administrador.
- Reporte Gerente.
- Reporte Vendedor.

### Fase 7 — Backup y despliegue

- Backup de SQL Server.
- Configuración de conexión.
- SQL Server central en una PC/servidor.
- Clientes WinForms desde otras PCs de la red.
- Prueba multisucursal.

## 13. Decisiones pendientes

No asumir estas decisiones sin consultar al equipo:

- si Administrador puede realizar ventas;
- alcance exacto de altas de clientes para Vendedor;
- contenido de cada uno de los tres reportes;
- tipos de factura admitidos;
- política final de descuentos;
- algoritmo definitivo para contraseña;
- datos definitivos de la sucursal inicial;
- necesidad de más permisos específicos;
- comportamiento exacto de las bajas de ventas/pagos.

## 14. Reglas para agentes de IA

Antes de modificar código:

1. Leer este archivo.
2. Leer `README.md`.
3. Revisar los scripts de `BaseDatos`.
4. Confirmar la rama actual.
5. Entender qué capa corresponde al cambio.

Un agente NO debe:

- cambiar la arquitectura de tres capas sin autorización;
- ejecutar SQL directamente desde formularios;
- meter `MessageBox` en `Capa_Datos`;
- hardcodear credenciales reales;
- subir contraseñas, secretos o cadenas privadas a GitHub;
- cambiar el esquema de la base sin actualizar los scripts;
- borrar tablas o datos sin autorización;
- reemplazar baja lógica por borrado físico sin autorización;
- modificar decisiones pendientes como si ya estuvieran aprobadas;
- trabajar directamente sobre `master`.

Un agente debe:

- preferir cambios pequeños y revisables;
- respetar nombres existentes;
- compilar después de modificar código;
- explicar cualquier cambio de arquitectura;
- actualizar este documento cuando se complete un hito importante;
- informar qué archivos modificó;
- indicar si una modificación requiere ejecutar SQL adicional.

## 15. Regla para cambios de base de datos

`01_Estructura.sql` representa la estructura completa para crear una base nueva.

`02_DatosIniciales.sql` representa los datos mínimos iniciales.

A partir de nuevas modificaciones al esquema, no editar una base de producción manualmente sin registrar el cambio.

Crear cuando sea necesario:

```text
BaseDatos/03_Actualizaciones.sql
```

o scripts de actualización numerados.

La estructura final y los scripts de actualización deben permanecer sincronizados.

## 16. Registro de avance

Agregar una entrada cuando se cierre un hito relevante.

Formato:

```text
AAAA-MM-DD
Responsable:
Rama:
Cambio:
Archivos principales:
Prueba realizada:
Pendiente:
```

### 2026-09-01

Responsable: Exequiel + asistencia de diseño

Rama integrada: `desarrollo`

Completado:

- solución creada con tres capas;
- referencias entre proyectos configuradas;
- repositorio Git y flujo de ramas configurado;
- SQL Server instalado;
- base `SistemaGestion` creada;
- modelo normalizado a 16 tablas;
- claves foráneas verificadas;
- datos iniciales cargados;
- scripts `01_Estructura.sql` y `02_DatosIniciales.sql` probados reconstruyendo una base desde cero;
- scripts integrados en `desarrollo`.

### 2026-09-02

Responsable: Exequiel

Rama: `exe-dev`

Completado:

- proyectos renombrados a `Capa_Datos`, `Capa_Logica` y `Capa_Vistas`;
- carpetas físicas antiguas eliminadas;
- referencias entre capas actualizadas;
- solución renombrada a `Sistema_Hierro_Y_Forja.slnx`;
- archivos `Class1.cs` eliminados;
- formulario inicial `Form1` eliminado;
- `FormLogin` creado y configurado como formulario inicial;
- namespace principal actualizado;
- paquete `Microsoft.Data.SqlClient` instalado en `Capa_Datos`;
- `Conexion.cs` creado;
- configuración de conexión externalizada mediante JSON;
- `configuracion.example.json` preparado para Git;
- `configuracion.json` excluido mediante `.gitignore`;
- solución recompilada correctamente después de la reorganización.

Pendiente inmediato:

- probar conexión real C# -> SQL Server utilizando `Conexion.cs`;
- crear `UsuarioDatos.cs`;
- implementar lógica de autenticación;
- definir e implementar hash de contraseña;
- crear `SesionActual.cs`;
- contar con un usuario administrador válido para login;
- integrar `FormLogin` con `FormPrincipal`;
- integrar vistas de Josias;
- realizar prueba completa para la primera entrega.
````
