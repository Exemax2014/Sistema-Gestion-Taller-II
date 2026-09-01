# AGENTS.md — Sistema de Gestión / Taller de Programación II

> Documento operativo para integrantes del equipo y agentes de IA.
> Estado base: 2026-09-01.
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
- Visual Studio 2026
- Git + GitHub

La solución está dividida en tres capas:

```text
SistemaGestion.Vistas
        ↓
SistemaGestion.Logica
        ↓
SistemaGestion.Datos
```

Reglas:

- `Vistas` contiene formularios, controles, mensajes y navegación.
- `Logica` contiene validaciones, reglas del negocio, autenticación, sesión, permisos y cálculos.
- `Datos` contiene conexión a SQL Server y operaciones SQL.
- `Vistas` no debe ejecutar SQL directamente.
- `Datos` no debe mostrar `MessageBox`.
- No agregar una referencia directa `Vistas -> Datos`.
- La cadena correcta es `Vistas -> Logica -> Datos`.

## 3. Estructura actual del repositorio

```text
Sistema-Gestion-Taller-II/
│
├── BaseDatos/
│   ├── 01_Estructura.sql
│   └── 02_DatosIniciales.sql
│
├── SistemaGestion.Vistas/
├── SistemaGestion.Logica/
├── SistemaGestion.Datos/
│
├── SistemaGestion.slnx
├── README.md
├── .gitignore
└── .gitattributes
```

Actualmente:

- `SistemaGestion.Vistas` conserva el `Form1` inicial y todavía no tiene las vistas definitivas.
- `SistemaGestion.Logica` todavía conserva solamente la clase inicial `Class1.cs`.
- `SistemaGestion.Datos` todavía conserva solamente la clase inicial `Class1.cs`.
- La solución compila.
- Las referencias entre capas están configuradas.
- La base de datos y sus scripts iniciales ya fueron diseñados y probados.

## 4. Base de datos actual

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

## 5. Datos iniciales existentes en los scripts

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

## 6. Perfiles y permisos iniciales

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

## 7. Git y ramas

Ramas principales:

```text
master       -> versión estable
desarrollo   -> integración del equipo
exe-dev      -> trabajo de Exequiel
josi-dev     -> trabajo de Josias (crear desde desarrollo)
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

## 8. Primera entrega — miércoles 2026-09-02

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
Login
  ↓
Validación en Base de Datos
  ↓
Sesión iniciada
  ↓
Formulario Principal
  ├── Clientes
  ├── Productos
  ├── Ventas
  └── Usuarios / otra vista
```

Las vistas secundarias pueden estar inicialmente incompletas si la consigna de la entrega solo exige mostrar avance, pero el login y la navegación principal deben funcionar.

## 9. División inmediata de tareas

### Exequiel — infraestructura + login

Responsable de:

- conexión de `SistemaGestion.Datos` con SQL Server;
- configuración de cadena de conexión sin repetirla por todo el proyecto;
- acceso a datos de usuario para login;
- lógica de autenticación;
- estrategia inicial de hash de contraseña;
- clase de sesión del usuario autenticado;
- `FrmLogin`;
- cambio de `Program.cs` para iniciar por el login;
- integración del login con el formulario principal realizado por Josias;
- prueba final de integración.

Archivos sugeridos:

```text
SistemaGestion.Datos/
    Conexion.cs
    UsuarioDatos.cs

SistemaGestion.Logica/
    UsuarioLogica.cs
    SesionActual.cs
    PasswordHelper.cs

SistemaGestion.Vistas/
    FrmLogin.cs
    FrmLogin.Designer.cs
```

### Josias — formulario principal + vistas

Responsable de:

- `FrmPrincipal`;
- menú principal;
- zona de navegación/contenido;
- diseño inicial de vistas:
  - `FrmClientes`;
  - `FrmProductos`;
  - `FrmVentas`;
  - `FrmUsuarios` o vista equivalente;
- botones de navegación;
- diseño consistente de formularios;
- recibir el usuario/perfil autenticado para mostrarlo en la cabecera.

Debe evitar modificar los archivos que esté trabajando Exequiel, especialmente `FrmLogin`, clases de autenticación y acceso a datos.

Archivos sugeridos:

```text
SistemaGestion.Vistas/
    FrmPrincipal.cs
    FrmPrincipal.Designer.cs
    FrmClientes.cs
    FrmClientes.Designer.cs
    FrmProductos.cs
    FrmProductos.Designer.cs
    FrmVentas.cs
    FrmVentas.Designer.cs
    FrmUsuarios.cs
    FrmUsuarios.Designer.cs
```

## 10. Orden de integración para la primera entrega

1. Ambos parten desde `desarrollo` actualizado.
2. Exequiel trabaja en `exe-dev`.
3. Josias trabaja en `josi-dev`.
4. Cada uno hace commits pequeños y push de su rama.
5. Integrar primero las vistas de Josias a `desarrollo`.
6. Exequiel hace Pull de `desarrollo`.
7. Exequiel integra `desarrollo` dentro de `exe-dev`.
8. Conectar `FrmLogin` con `FrmPrincipal`.
9. Merge final de `exe-dev` hacia `desarrollo`.
10. Compilar y probar desde `desarrollo`.
11. No pasar a `master` hasta tener una versión demostrable estable.

## 11. Después de la primera entrega

Orden de desarrollo propuesto:

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

## 12. Decisiones pendientes

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

## 13. Reglas para agentes de IA

Antes de modificar código:

1. Leer este archivo.
2. Leer `README.md`.
3. Revisar los scripts de `BaseDatos`.
4. Confirmar la rama actual.
5. Entender qué capa corresponde al cambio.

Un agente NO debe:

- cambiar la arquitectura de tres capas sin autorización;
- ejecutar SQL directamente desde formularios;
- meter `MessageBox` en `Datos`;
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

## 14. Regla para cambios de base de datos

`01_Estructura.sql` representa la estructura completa para crear una base nueva.

`02_DatosIniciales.sql` representa los datos mínimos iniciales.

A partir de nuevas modificaciones al esquema, no editar una base de producción manualmente sin registrar el cambio.

Crear cuando sea necesario:

```text
BaseDatos/03_Actualizaciones.sql
```

o scripts de actualización numerados.

La estructura final y los scripts de actualización deben permanecer sincronizados.

## 15. Registro de avance

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

Pendiente inmediato:

- crear `josi-dev`;
- conexión C# -> SQL Server;
- usuario admin válido para login;
- hash de contraseña;
- `FrmLogin`;
- `FrmPrincipal`;
- vistas iniciales;
- integración y prueba para la entrega del 2026-09-02.
