# 🔨 Hierro & Forja — Sistema de Gestión Comercial

> **Proyecto final de Taller de Programación II**  
> Aplicación de escritorio para la gestión integral de ventas, clientes, productos, inventario, usuarios, sucursales, reportes, avisos y copias de seguridad.

[![C#](https://img.shields.io/badge/C%23-.NET%2010-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Windows Forms](https://img.shields.io/badge/UI-Windows%20Forms-0078D4)](https://learn.microsoft.com/dotnet/desktop/winforms/)
[![SQL Server](https://img.shields.io/badge/DB-SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server/)
[![Architecture](https://img.shields.io/badge/Arquitectura-3%20capas-C48A24)](#arquitectura)
[![Status](https://img.shields.io/badge/Estado-Desarrollo%20avanzado-2E8B57)](#estado-del-proyecto)

---

<a id="indice"></a>

## 📑 Índice

1. [Descripción general](#descripcion-general)
2. [Objetivos del sistema](#objetivos)
3. [Tecnologías y dependencias](#tecnologias)
4. [Arquitectura](#arquitectura)
5. [Estructura del repositorio](#estructura-repositorio)
6. [Módulos principales](#modulos)
   - [Inicio / Dashboard](#modulo-inicio)
   - [Ventas](#modulo-ventas)
   - [Clientes](#modulo-clientes)
   - [Productos, categorías y marcas](#modulo-productos)
   - [Inventario y stock](#modulo-inventario)
   - [Usuarios](#modulo-usuarios)
   - [Perfiles y permisos](#modulo-perfiles)
   - [Sucursales](#modulo-sucursales)
   - [Reportes](#modulo-reportes)
   - [Avisos internos](#modulo-avisos)
   - [Back Up](#modulo-backup)
   - [Auditoría](#modulo-auditoria)
7. [Sesión, permisos y alcance](#sesion-permisos)
8. [Base de datos](#base-datos)
   - [Tablas principales](#tablas)
   - [Procedimientos almacenados](#procedimientos)
   - [Convenciones SQL](#convenciones-sql)
9. [Scripts de base de datos](#scripts-base)
10. [Instalación y puesta en marcha](#instalacion)
    - [Requisitos](#requisitos)
    - [Clonar el repositorio](#clonar)
    - [Crear la base de datos](#crear-base)
    - [Configurar la conexión](#configuracion-conexion)
    - [Restaurar paquetes y compilar](#compilar)
    - [Ejecutar la aplicación](#ejecutar)
11. [Datos de prueba](#datos-prueba)
12. [Credenciales de prueba](#credenciales)
13. [Validaciones](#validaciones)
14. [Ventas, pagos y concurrencia](#ventas-concurrencia)
15. [Exportación a Excel y PDF](#exportaciones)
16. [Responsive y criterios de interfaz](#interfaz)
17. [Back Up y seguridad](#backup-seguridad)
18. [Flujo Git recomendado](#git)
19. [Pruebas manuales recomendadas](#pruebas)
20. [Solución de problemas frecuentes](#troubleshooting)
21. [Estado actual y pendientes](#estado-del-proyecto)
22. [Integrantes](#integrantes)

---

<a id="descripcion-general"></a>

## 1. 📌 Descripción general

**Hierro & Forja** es un sistema de gestión comercial de escritorio desarrollado en **C# con Windows Forms y .NET 10**, conectado a una base de datos **Microsoft SQL Server**.

El sistema está pensado para trabajar con **múltiples sucursales**, perfiles de usuario con permisos configurables y distintos alcances de información.

Permite administrar el circuito completo de una operación comercial:

```text
Inicio de sesión
    ↓
Selección de sucursal operativa
    ↓
Clientes / Productos / Stock
    ↓
Venta + múltiples medios de pago
    ↓
Actualización transaccional de inventario
    ↓
Comprobante PDF
    ↓
Reportes / Auditoría / Back Up
```

La aplicación mantiene una separación estricta entre:

```text
Capa_Vistas → Capa_Logica → Capa_Datos → SQL Server
```

La interfaz fue diseñada con una identidad visual basada en **charcoal / negro, dorado y blanco**, utilizando formularios embebidos dentro de un contenedor principal.

[⬆ Volver al índice](#indice)

---

<a id="objetivos"></a>

## 2. 🎯 Objetivos del sistema

El proyecto busca resolver las necesidades principales de un comercio con varias sucursales:

- Registrar ventas de manera segura y transaccional.
- Administrar clientes y su historial de compras.
- Gestionar productos, categorías y marcas.
- Controlar stock y stock mínimo por sucursal.
- Permitir múltiples medios de pago por venta.
- Gestionar usuarios, perfiles, permisos y alcance.
- Administrar sucursales y sus direcciones.
- Generar reportes comerciales.
- Consultar actividad y auditoría.
- Publicar avisos internos.
- Generar copias de seguridad `.bak`.
- Exportar información a Excel.
- Generar comprobantes de venta en PDF.
- Mantener trazabilidad de operaciones sensibles.

[⬆ Volver al índice](#indice)

---

<a id="tecnologias"></a>

## 3. 🧰 Tecnologías y dependencias

| Tecnología | Uso |
|---|---|
| **C#** | Lenguaje principal |
| **.NET 10** | Plataforma de ejecución |
| **Windows Forms** | Interfaz gráfica |
| **SQL Server** | Motor de base de datos |
| **Microsoft.Data.SqlClient 7.1.1** | Acceso a SQL Server |
| **ClosedXML 0.105.1** | Exportación real a `.xlsx` |
| **PDFsharp-GDI 6.2.4** | Generación de comprobantes PDF |
| **System.Security.Cryptography.Pkcs 9.0.21** | Soporte criptográfico |
| **System.Text.Json** | Lectura de configuración local |
| **Git / GitHub** | Control de versiones |

La solución principal es:

[`Sistema_Hierro_Y_Forja.slnx`](./Sistema_Hierro_Y_Forja.slnx)

[⬆ Volver al índice](#indice)

---

<a id="arquitectura"></a>

## 4. 🏗️ Arquitectura

El proyecto usa una arquitectura en **tres capas**.

```text
┌─────────────────────────┐
│       Capa_Vistas       │
│ WinForms · navegación   │
│ eventos · responsive    │
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│       Capa_Logica       │
│ reglas · validaciones   │
│ permisos · sesión       │
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│       Capa_Datos        │
│ SqlClient · modelos     │
│ stored procedures       │
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│       SQL Server        │
│ tablas · SP · FK · TX   │
└─────────────────────────┘
```

### Capa_Vistas

[`Capa_Vistas/`](./Capa_Vistas/)

Responsabilidades:

- Formularios Windows Forms.
- Eventos.
- Navegación.
- Layout responsive.
- Presentación de mensajes.
- Aplicación visual de permisos.
- DataGridView.
- Generación de Excel y PDF desde helpers de Vista.

No debe:

- Abrir conexiones SQL.
- Ejecutar SQL.
- Contener reglas de negocio autoritativas.

### Capa_Logica

[`Capa_Logica/`](./Capa_Logica/)

Responsabilidades:

- Validaciones de negocio.
- Permisos.
- Alcance de sucursal.
- Sesión actual.
- Autenticación.
- Coordinación de operaciones.
- Modelos usados por las vistas.

Ejemplos:

- `UsuarioLogica`
- `ClienteLogica`
- `VentaLogica`
- `ProductoLogica`
- `InventarioLogica`
- `ReporteLogica`
- `SucursalLogica`
- `BackupLogica`
- `SesionActual`
- `PasswordHelper`

### Capa_Datos

[`Capa_Datos/`](./Capa_Datos/)

Responsabilidades:

- Crear conexiones.
- Ejecutar procedimientos almacenados.
- Construir parámetros SQL.
- Leer resultados.
- Manejar modelos de transporte.

Ejemplos:

- `Conexion`
- `UsuarioDatos`
- `ClienteDatos`
- `DireccionDatos`
- `VentaDatos`
- `ReporteDatos`
- `SucursalDatos`
- `AuditoriaDatos`
- `BackupDatos`

### Regla general

```text
Vista → Lógica → Datos → SQL
```

Nunca:

```text
Vista → SQL
Vista → Datos
Datos → Vista
```

[⬆ Volver al índice](#indice)

---

<a id="estructura-repositorio"></a>

## 5. 📂 Estructura del repositorio

```text
Sistema-Gestion-Taller-II/
│
├── BaseDatos/
│   ├── 01_Estructura.sql
│   ├── 02_DatosIniciales.sql
│   ├── 03_Procedimientos.sql
│   ├── 04_DatosPrueba.sql
│   ├── 05_ResetBasePruebas.sql
│   └── Historico/
│
├── Capa_Datos/
│   ├── Configuracion/
│   │   └── configuracion.json      # local / no versionar secretos
│   ├── Conexion.cs
│   ├── UsuarioDatos.cs
│   ├── ClienteDatos.cs
│   ├── DireccionDatos.cs
│   ├── ProductoDatos.cs
│   ├── InventarioDatos.cs
│   ├── VentaDatos.cs
│   ├── ReporteDatos.cs
│   ├── SucursalDatos.cs
│   ├── AuditoriaDatos.cs
│   └── BackupDatos.cs
│
├── Capa_Logica/
│   ├── UsuarioLogica.cs
│   ├── ClienteLogica.cs
│   ├── ProductoLogica.cs
│   ├── InventarioLogica.cs
│   ├── VentaLogica.cs
│   ├── ReporteLogica.cs
│   ├── SucursalLogica.cs
│   ├── BackupLogica.cs
│   ├── SesionActual.cs
│   └── PasswordHelper.cs
│
├── Capa_Vistas/
│   ├── Program.cs
│   ├── FormLogin.cs
│   ├── FormPrincipal.cs
│   ├── FormInicio.cs
│   ├── FormVentas.cs
│   ├── FormClientes.cs
│   ├── FormClienteDetalle.cs
│   ├── FormProductos.cs
│   ├── FormProductoDetalle.cs
│   ├── FormUsuarios.cs
│   ├── FormUsuarioDetalle.cs
│   ├── FormReportesGeneral.cs
│   ├── FormBackup.cs
│   ├── FormListadoVentas.cs
│   ├── FormVentaDetalle.cs
│   ├── FormMensaje.cs
│   └── ...
│
├── AGENTS.md
├── README.md
└── Sistema_Hierro_Y_Forja.slnx
```

### Archivos importantes

- [README.md](./README.md)
- [AGENTS.md](./AGENTS.md)
- [01_Estructura.sql](./BaseDatos/01_Estructura.sql)
- [02_DatosIniciales.sql](./BaseDatos/02_DatosIniciales.sql)
- [03_Procedimientos.sql](./BaseDatos/03_Procedimientos.sql)
- [04_DatosPrueba.sql](./BaseDatos/04_DatosPrueba.sql)
- [05_ResetBasePruebas.sql](./BaseDatos/05_ResetBasePruebas.sql)

[⬆ Volver al índice](#indice)

---

<a id="modulos"></a>

## 6. 🧩 Módulos principales

<a id="modulo-inicio"></a>

### 6.1 Inicio / Dashboard

[`FormInicio.cs`](./Capa_Vistas/FormInicio.cs)

Incluye:

- Ventas del día.
- Ingresos del día.
- Productos con stock bajo.
- Cantidad de productos activos.
- Accesos rápidos.
- Gráfico de ventas recientes.
- Ranking de productos más vendidos.
- Actividad reciente.
- Avisos internos.

El Dashboard respeta el alcance de la sesión y utiliza un layout persistente y responsive.

---

<a id="modulo-ventas"></a>

### 6.2 Ventas

[`FormVentas.cs`](./Capa_Vistas/FormVentas.cs)

Funciones principales:

- Selección de sucursal operativa.
- Selección/búsqueda de cliente.
- Alta rápida de cliente desde la venta.
- Selección de productos.
- Cantidades.
- Carrito.
- Resumen de productos.
- Múltiples medios de pago.
- Pagos combinados.
- Validación de importe total.
- Confirmación transaccional.
- Descuento de stock.
- Historial de ventas.
- Consulta de detalle.
- Comprobante PDF.

La venta no se confirma si existen inconsistencias entre:

- productos,
- cantidades,
- pagos,
- permisos,
- sucursal,
- cliente,
- stock disponible.

La vista también protege operaciones en curso mediante control de cambios antes de abandonar el módulo.

---

<a id="modulo-clientes"></a>

### 6.3 Clientes

Archivos principales:

- [`FormClientes.cs`](./Capa_Vistas/FormClientes.cs)
- [`FormClienteDetalle.cs`](./Capa_Vistas/FormClienteDetalle.cs)

Funciones:

- Listado.
- Búsqueda por nombre, apellido, DNI o teléfono.
- Filtro por estado.
- Alta.
- Modificación.
- Baja lógica.
- Reactivación.
- Historial de compras.
- Acceso al detalle de ventas.

#### Dirección

La dirección se maneja estructuradamente:

```text
Provincia
Localidad
Calle
Altura
Piso (opcional)
```

El sistema:

- lista provincias,
- carga localidades según provincia,
- permite reutilizar o crear localidades,
- sugiere calles existentes mediante autocomplete,
- permite calles nuevas,
- valida altura y piso.

El cliente genérico **Consumidor Final** puede utilizarse en ventas donde no se desea identificar al comprador.

---

<a id="modulo-productos"></a>

### 6.4 Productos, categorías y marcas

Archivos principales:

- [`FormProductos.cs`](./Capa_Vistas/FormProductos.cs)
- [`FormProductoDetalle.cs`](./Capa_Vistas/FormProductoDetalle.cs)

Características:

- Código de barras.
- Nombre.
- Descripción.
- Categoría.
- Marca.
- Precio de costo.
- Porcentaje de ganancia.
- Precio de venta calculado.
- Estado activo.
- Baja lógica.
- Inventario por sucursal.

Categorías y marcas poseen CRUD independiente.

La relación entre marca y categoría se modela mediante:

```text
MARCA_CATEGORIA
```

---

<a id="modulo-inventario"></a>

### 6.5 Inventario y stock

El inventario se administra por:

```text
PRODUCTO + SUCURSAL
```

Cada combinación posee:

- `stock`
- `stock_minimo`

El sistema permite:

- consultar stock,
- modificar stock según permisos,
- detectar stock bajo,
- visualizar diferencias,
- gestionar todas las sucursales autorizadas.

Regla de alerta:

```text
stock <= stock_minimo → Stock bajo
stock > stock_minimo  → Correcto
```

---

<a id="modulo-usuarios"></a>

### 6.6 Usuarios

Archivos principales:

- [`FormUsuarios.cs`](./Capa_Vistas/FormUsuarios.cs)
- [`FormUsuarioDetalle.cs`](./Capa_Vistas/FormUsuarioDetalle.cs)

Funciones:

- Listado.
- Búsqueda.
- Alta.
- Modificación.
- Baja lógica.
- Reactivación.
- Perfil.
- Sucursal asignada.
- Estado.
- Edición del perfil propio.
- Cambio opcional de contraseña propia.

La contraseña nunca se almacena en texto plano.

---

<a id="modulo-perfiles"></a>

### 6.7 Perfiles y permisos

Los permisos se encuentran en SQL y se asignan mediante:

```text
PERFIL
FUNCIONALIDAD
PERFIL_FUNCIONALIDAD
```

El sistema no decide permisos por el nombre del perfil.

La autorización se realiza mediante códigos como:

```text
USUARIOS_VER
VENTAS_REALIZAR
CLIENTES_ALTA
PRODUCTOS_MODIFICAR
REPORTES_VER
BACKUP_REALIZAR
...
```

El perfil global puede operar con alcance de todas las sucursales.

---

<a id="modulo-sucursales"></a>

### 6.8 Sucursales

Incluye:

- Listado de sucursales.
- Alta.
- Modificación.
- Baja lógica.
- Reactivación.
- Dirección.
- Provincia.
- Localidad.
- Calle.
- Altura.
- Piso.
- Distribución de usuarios por perfil.

Los usuarios de alcance global pueden seleccionar una **Sucursal operativa** o trabajar en contexto de **Todas las sucursales** cuando la operación lo permita.

---

<a id="modulo-reportes"></a>

### 6.9 Reportes

[`FormReportesGeneral.cs`](./Capa_Vistas/FormReportesGeneral.cs)

La vista unificada de Reportes contiene actualmente:

1. **General**
2. **Por usuario**
3. **Actividad**
4. **Ventas detalladas**
5. **Stock bajo**

#### General

Incluye:

- Ventas.
- Ingresos.
- Productos vendidos.
- Stock bajo.
- Gráfico de ventas del período.
- Ingresos por día.
- Productos más vendidos.
- Tabla de ranking.

#### Por usuario

Permite seleccionar un vendedor y visualizar:

- Ventas.
- Ingresos.
- Productos.
- Ticket promedio.
- Evolución comercial.
- Productos vendidos.
- Historial asociado.

#### Actividad

Consulta auditoría por:

- fecha,
- usuario,
- acción,
- entidad,
- detalle,
- sucursal.

#### Ventas detalladas

Permite filtrar y consultar cada venta con acceso a su detalle.

#### Stock bajo

Muestra:

- Código.
- Producto.
- Categoría.
- Sucursal.
- Stock.
- Stock mínimo.
- Diferencia.
- Acción de gestión.

#### Excel

Los reportes autorizados pueden exportarse a `.xlsx` mediante ClosedXML.

---

<a id="modulo-avisos"></a>

### 6.10 Avisos internos

Los avisos pueden ser:

```text
Globales        → id_sucursal = NULL
Por sucursal    → id_sucursal = <id>
```

Permisos:

```text
AVISOS_VER
AVISOS_PUBLICAR
```

Inicio muestra los avisos correspondientes al alcance del usuario.

---

<a id="modulo-backup"></a>

### 6.11 Back Up

[`FormBackup.cs`](./Capa_Vistas/FormBackup.cs)

Flujo:

```text
FormBackup
    ↓
BackupLogica
    ↓
BackupDatos
    ↓
SQL Server BACKUP DATABASE
```

Características:

- Genera un `.bak` real.
- Utiliza la ruta de backup del servidor SQL.
- No necesita seleccionar una carpeta del cliente.
- Registra la operación en Auditoría.
- Muestra historial de backups registrados.

Formato de archivo:

```text
<BaseDatos>_yyyyMMdd_HHmmss.bak
```

No existe funcionalidad de Restore desde la aplicación.

---

<a id="modulo-auditoria"></a>

### 6.12 Auditoría

La tabla `AUDITORIA` permite registrar:

- usuario,
- fecha,
- acción,
- entidad,
- ID afectado,
- detalle,
- sucursal.

Se utiliza para operaciones como:

- altas,
- modificaciones,
- bajas,
- inventario,
- ventas,
- avisos,
- perfiles,
- backup.

Nunca debe registrar:

- contraseña,
- hash,
- connection string,
- credenciales SQL.

[⬆ Volver al índice](#indice)

---

<a id="sesion-permisos"></a>

## 7. 🔐 Sesión, permisos y alcance

`SesionActual` mantiene el contexto del usuario autenticado.

Conceptos principales:

| Campo | Descripción |
|---|---|
| `IdUsuario` | Usuario autenticado |
| `IdPerfil` | Perfil |
| `IdSucursal` | Sucursal fija |
| `AlcanceGlobal` | Permite trabajar globalmente |
| `IdSucursalOperativa` | Sucursal seleccionada temporalmente |

### Alcance global

Un usuario global puede iniciar con:

```text
Todas las sucursales
```

y cambiar la sucursal operativa cuando una operación necesita una sucursal concreta.

### Alcance fijo

Un usuario no global trabaja únicamente con su sucursal asignada.

### Permisos principales

#### Usuarios

```text
USUARIOS_VER
USUARIOS_ALTA
USUARIOS_MODIFICAR
USUARIOS_BAJA
```

#### Perfiles

```text
PERMISOS_GESTIONAR
```

#### Ventas

```text
VENTAS_VER
VENTAS_REALIZAR
```

#### Clientes

```text
CLIENTES_VER
CLIENTES_ALTA
CLIENTES_MODIFICAR
CLIENTES_BAJA
```

#### Productos

```text
PRODUCTOS_VER
PRODUCTOS_ALTA
PRODUCTOS_MODIFICAR
PRODUCTOS_BAJA
```

#### Categorías

```text
CATEGORIAS_VER
CATEGORIAS_ALTA
CATEGORIAS_MODIFICAR
CATEGORIAS_BAJA
```

#### Marcas

```text
MARCAS_VER
MARCAS_ALTA
MARCAS_MODIFICAR
MARCAS_BAJA
```

#### Sucursales

```text
SUCURSALES_VER
SUCURSALES_ALTA
SUCURSALES_MODIFICAR
SUCURSALES_BAJA
```

#### Reportes

```text
REPORTES_VER
REPORTES_VENTAS
REPORTES_RECAUDACION
REPORTES_PRODUCTOS
REPORTES_STOCK
REPORTES_RENDIMIENTO_VENDEDORES
REPORTES_DETALLE_VENTAS
REPORTES_EXPORTAR

REPORTES_ALCANCE_PROPIO
REPORTES_ALCANCE_SUCURSAL
REPORTES_ALCANCE_GLOBAL
```

#### Avisos

```text
AVISOS_VER
AVISOS_PUBLICAR
```

#### Backup

```text
BACKUP_REALIZAR
```

[⬆ Volver al índice](#indice)

---

<a id="base-datos"></a>

## 8. 🗄️ Base de datos

Nombre de la base:

```text
SistemaGestion
```

Motor:

```text
Microsoft SQL Server
```

<a id="tablas"></a>

### 8.1 Tablas principales

El esquema actual contiene **20 tablas principales**:

| Tabla | Responsabilidad |
|---|---|
| `PROVINCIA` | Provincias |
| `LOCALIDAD` | Localidades |
| `DIRECCION` | Calle, altura y piso |
| `PERFIL` | Perfiles |
| `FUNCIONALIDAD` | Permisos |
| `PERFIL_FUNCIONALIDAD` | Relación perfil-permiso |
| `SUCURSAL` | Sucursales |
| `USUARIO` | Usuarios |
| `CLIENTE` | Clientes |
| `CATEGORIA` | Categorías |
| `MARCA` | Marcas |
| `MARCA_CATEGORIA` | Relación marca-categoría |
| `PRODUCTO` | Catálogo |
| `INVENTARIO` | Stock por producto/sucursal |
| `METODO_PAGO` | Métodos de pago |
| `VENTA` | Cabecera de venta |
| `DETALLE_VENTA` | Productos vendidos |
| `PAGO` | Pagos de la venta |
| `AVISO` | Avisos internos |
| `AUDITORIA` | Trazabilidad |

Además se utilizan tipos de tabla:

```text
dbo.VentaItemTipo
dbo.VentaPagoTipo
```

para enviar múltiples productos y pagos a SQL Server en una sola operación.

---

<a id="procedimientos"></a>

### 8.2 Procedimientos almacenados

[`03_Procedimientos.sql`](./BaseDatos/03_Procedimientos.sql)

El proyecto utiliza **más de 80 procedimientos almacenados**, organizados por módulo.

Convención:

```text
sp_<Entidad>_<Accion>
```

Ejemplos:

```text
sp_Usuario_BuscarPorNombreUsuario
sp_Usuario_Alta
sp_Usuario_Modificar
sp_Usuario_Baja
sp_Cliente_Buscar
sp_Cliente_Alta
sp_Direccion_Alta
sp_Direccion_Modificar
sp_Direccion_ListarCallesPorLocalidad
sp_Producto_Buscar
sp_Inventario_EstablecerStock
sp_Venta_Registrar
sp_Venta_ObtenerDetalle
sp_Reporte_Resumen
sp_Reporte_ProductosMasVendidos
sp_Reporte_StockBajo
sp_Sucursal_ListarResumenUsuarios
sp_Aviso_Publicar
sp_Auditoria_Registrar
```

---

<a id="convenciones-sql"></a>

### 8.3 Convenciones SQL

El diseño utiliza:

- PK.
- FK.
- `UNIQUE`.
- `CHECK`.
- Bajas lógicas mediante `eliminado_en`.
- Transacciones.
- Parámetros SQL.
- Procedimientos almacenados.
- Validación de permisos en operaciones críticas.
- Idempotencia en scripts de estructura/datos iniciales.

No se concatena entrada del usuario en consultas SQL.

[⬆ Volver al índice](#indice)

---

<a id="scripts-base"></a>

## 9. 📜 Scripts de base de datos

Directorio:

[`BaseDatos/`](./BaseDatos/)

### `01_Estructura.sql`

[`BaseDatos/01_Estructura.sql`](./BaseDatos/01_Estructura.sql)

Responsabilidades:

- Crear `SistemaGestion` si no existe.
- Crear tablas.
- Crear relaciones.
- Crear constraints.
- Crear tipos de tabla para ventas.
- Actualizar una base existente cuando corresponde.

Es **idempotente / actualizable**.

---

### `02_DatosIniciales.sql`

[`BaseDatos/02_DatosIniciales.sql`](./BaseDatos/02_DatosIniciales.sql)

Carga datos esenciales:

- perfiles base,
- funcionalidades,
- métodos de pago,
- provincia/localidad inicial,
- catálogo de provincias argentinas,
- Sucursal Central,
- usuario administrador inicial,
- permisos base.

Es **idempotente**.

---

### `03_Procedimientos.sql`

[`BaseDatos/03_Procedimientos.sql`](./BaseDatos/03_Procedimientos.sql)

Crea o actualiza procedimientos almacenados mediante:

```sql
CREATE OR ALTER PROCEDURE
```

Siempre debe volver a ejecutarse después de agregar o modificar procedimientos.

---

### `04_DatosPrueba.sql`

[`BaseDatos/04_DatosPrueba.sql`](./BaseDatos/04_DatosPrueba.sql)

Exclusivo de desarrollo/testing.

El escenario ampliado actual está pensado para demostrar todos los módulos:

- 11 usuarios demo.
- Clave común de usuarios demo: `Prueba2026!`.
- 18 localidades de Corrientes.
- 180 clientes normales.
- 1 cliente genérico `Consumidor Final`.
- 25 productos.
- 3 sucursales.
- 1.000 ventas.
- Actividad intensa en fechas recientes.
- Ventas todos los días de los últimos 14 días.
- Pagos simples y combinados.
- Avisos.
- Auditoría.
- Backups históricos de prueba.
- Combinaciones de stock bajo.

> Este script es para desarrollo. No ejecutar en una base productiva.

---

### `05_ResetBasePruebas.sql`

[`BaseDatos/05_ResetBasePruebas.sql`](./BaseDatos/05_ResetBasePruebas.sql)

Elimina datos operativos de testing y devuelve la base al estado:

```text
01 + 02 + 03
```

No elimina objetos de base de datos.

> ⚠️ Solo desarrollo/testing.

---

### Orden oficial

#### Instalación base

```text
01_Estructura.sql
        ↓
02_DatosIniciales.sql
        ↓
03_Procedimientos.sql
```

#### Instalación para demo/testing

```text
01_Estructura.sql
        ↓
02_DatosIniciales.sql
        ↓
03_Procedimientos.sql
        ↓
05_ResetBasePruebas.sql
        ↓
04_DatosPrueba.sql
```

#### Recargar datos demo

```text
05_ResetBasePruebas.sql
        ↓
04_DatosPrueba.sql
```

#### Actualizar solamente procedimientos

```text
03_Procedimientos.sql
```

[⬆ Volver al índice](#indice)

---

<a id="instalacion"></a>

## 10. 🚀 Instalación y puesta en marcha

<a id="requisitos"></a>

### 10.1 Requisitos

- Windows 10/11.
- .NET 10 SDK.
- Visual Studio compatible con .NET 10 y Windows Forms.
- SQL Server.
- SQL Server Management Studio recomendado.
- Git.

Comprobar .NET:

```powershell
dotnet --version
```

---

<a id="clonar"></a>

### 10.2 Clonar el repositorio

```powershell
git clone https://github.com/Exemax2014/Sistema-Gestion-Taller-II.git
cd Sistema-Gestion-Taller-II
```

Para trabajar sobre la rama integrada:

```powershell
git checkout desarrollo
git pull origin desarrollo
```

---

<a id="crear-base"></a>

### 10.3 Crear la base de datos

Abrir **SQL Server Management Studio** y conectarse a la instancia deseada.

Ejecutar los scripts en este orden:

```text
BaseDatos/01_Estructura.sql
BaseDatos/02_DatosIniciales.sql
BaseDatos/03_Procedimientos.sql
```

Para incluir el escenario de demostración:

```text
BaseDatos/05_ResetBasePruebas.sql
BaseDatos/04_DatosPrueba.sql
```

La base creada se llama:

```text
SistemaGestion
```

#### Comprobación rápida

```sql
USE SistemaGestion;
GO

SELECT DB_NAME() AS BaseActual;

SELECT COUNT(*) AS Usuarios
FROM dbo.USUARIO;

SELECT COUNT(*) AS Productos
FROM dbo.PRODUCTO;

SELECT COUNT(*) AS Ventas
FROM dbo.VENTA;
```

---

<a id="configuracion-conexion"></a>

### 10.4 Configurar la conexión

La conexión se carga desde:

```text
Capa_Datos/Configuracion/configuracion.json
```

Este archivo debe existir localmente y copiarse a la salida.

> ⚠️ No versionar contraseñas ni secretos reales.

#### Ejemplo con autenticación de Windows

```json
{
  "Servidor": "localhost",
  "BaseDatos": "SistemaGestion",
  "AutenticacionWindows": true,
  "Usuario": null,
  "Contrasena": null,
  "TrustServerCertificate": true
}
```

Ejemplos posibles de servidor:

```text
localhost
.\SQLEXPRESS
DESKTOP-PC\SQLEXPRESS
```

#### Ejemplo con usuario SQL Server

```json
{
  "Servidor": "localhost",
  "BaseDatos": "SistemaGestion",
  "AutenticacionWindows": false,
  "Usuario": "usuario_sql",
  "Contrasena": "CAMBIAR_ESTA_CLAVE",
  "TrustServerCertificate": true
}
```

La clase responsable es:

[`Capa_Datos/Conexion.cs`](./Capa_Datos/Conexion.cs)

---

<a id="compilar"></a>

### 10.5 Restaurar paquetes y compilar

Desde la raíz:

```powershell
dotnet restore .\Sistema_Hierro_Y_Forja.slnx
```

Compilar:

```powershell
dotnet build .\Sistema_Hierro_Y_Forja.slnx
```

Comando utilizado habitualmente durante desarrollo:

```powershell
dotnet build .\Sistema_Hierro_Y_Forja.slnx --no-restore -m:1 -p:UseSharedCompilation=false
```

Resultado esperado:

```text
0 errores
0 advertencias
```

---

<a id="ejecutar"></a>

### 10.6 Ejecutar la aplicación

Desde terminal:

```powershell
dotnet run --project .\Capa_Vistas\Capa_Vistas.csproj
```

O desde Visual Studio:

1. Abrir `Sistema_Hierro_Y_Forja.slnx`.
2. Establecer `Capa_Vistas` como proyecto de inicio.
3. Ejecutar.

Punto de entrada:

[`Capa_Vistas/Program.cs`](./Capa_Vistas/Program.cs)

El primer formulario es:

```text
FormLogin
```

[⬆ Volver al índice](#indice)

---

<a id="datos-prueba"></a>

## 11. 🧪 Datos de prueba

El script `04_DatosPrueba.sql` prepara un entorno amplio para probar:

- Dashboard.
- Ventas.
- Reportes.
- Ranking de productos.
- Stock bajo.
- Historial de clientes.
- Ventas por usuario.
- Ventas por sucursal.
- Auditoría.
- Avisos.
- Historial de Back Up.

### Escenario ampliado

```text
18 localidades
180 clientes normales
1 Consumidor Final
25 productos
3 sucursales
~11 usuarios demo
1.000 ventas
24 avisos aproximadamente
~180 eventos de auditoría
15 backups históricos demo aproximadamente
10 combinaciones de stock bajo aproximadamente
```

Las ventas están distribuidas en los últimos meses con mayor densidad reciente para que:

- Inicio muestre actividad.
- Los últimos 7/14 días tengan datos.
- Todos los vendedores tengan ventas.
- Los reportes tengan tendencias visibles.
- Los rankings no queden vacíos.

### Consumidor Final

Para ventas sin comprador identificado se utiliza un **cliente genérico**, no un vendedor ficticio.

Conceptualmente:

```text
Vendedor real autenticado
        ↓
Venta
        ↓
Cliente = Consumidor Final
```

De esta forma los reportes continúan indicando correctamente quién realizó la venta.

[⬆ Volver al índice](#indice)

---

<a id="credenciales"></a>

## 12. 🔑 Credenciales de prueba

### Usuarios demo

Los usuarios generados por `04_DatosPrueba.sql` utilizan la clave común:

```text
Prueba2026!
```

Los nombres de usuario específicos se encuentran definidos dentro del script de datos de prueba.

### Administrador inicial

Usuario reservado:

```text
admin
```

La contraseña del administrador no se documenta en texto plano en este README.

Las credenciales locales de prueba pueden mantenerse en un archivo privado ignorado por Git, por ejemplo:

```text
CREDENCIALES_PRUEBA.txt
```

> Nunca subir contraseñas reales al repositorio.

[⬆ Volver al índice](#indice)

---

<a id="validaciones"></a>

## 13. ✅ Validaciones

Las validaciones se aplican en distintos niveles.

```text
Vista
  ↓
Lógica
  ↓
SQL Server
```

### Vista

Previene entradas incorrectas y brinda feedback rápido.

Ejemplos:

- campos obligatorios,
- DNI,
- email,
- teléfono,
- fechas,
- importes,
- cantidades,
- calle,
- altura,
- piso,
- combos obligatorios.

### Lógica

Es la validación autoritativa de la aplicación:

- permisos,
- alcance,
- existencia,
- relaciones,
- formatos,
- unicidad,
- reglas de negocio.

### SQL

Garantiza integridad final:

- PK,
- FK,
- UNIQUE,
- CHECK,
- transacciones.

### Direcciones

Formato:

```text
Provincia
Localidad
Calle
Altura
Piso
```

Ejemplos válidos de calle:

```text
Junín
9 de Julio
25 de Mayo
Av. 3 de Abril
Ruta 12
```

Altura:

```text
entero positivo
```

Piso:

```text
opcional
0–99
```

[⬆ Volver al índice](#indice)

---

<a id="ventas-concurrencia"></a>

## 14. 💳 Ventas, pagos y concurrencia

La venta utiliza TVP para enviar múltiples productos y pagos:

```text
dbo.VentaItemTipo
dbo.VentaPagoTipo
```

Flujo:

```text
FormVentas
    ↓
VentaLogica
    ↓
VentaDatos
    ↓
sp_Venta_Registrar
```

SQL vuelve a comprobar:

- usuario,
- permiso,
- sucursal,
- cliente,
- productos,
- cantidades,
- pagos,
- total,
- stock.

### Concurrencia

El procedimiento utiliza bloqueos sobre inventario:

```text
UPDLOCK
HOLDLOCK
```

para evitar que dos operaciones consuman simultáneamente las mismas unidades disponibles.

La transacción realiza:

```text
Insert VENTA
Insert DETALLE_VENTA
Insert PAGO
Actualizar INVENTARIO
COMMIT
```

Si falla cualquier paso:

```text
ROLLBACK
```

[⬆ Volver al índice](#indice)

---

<a id="exportaciones"></a>

## 15. 📤 Exportación a Excel y PDF

### Excel

Se utiliza:

```text
ClosedXML
```

Características:

- archivo `.xlsx` real,
- encabezados,
- tablas,
- formatos numéricos,
- fechas,
- filtros,
- no requiere Microsoft Excel instalado.

Permiso:

```text
REPORTES_EXPORTAR
```

### PDF

Se utiliza:

```text
PDFsharp-GDI
```

Principalmente para comprobantes/detalles de ventas.

[⬆ Volver al índice](#indice)

---

<a id="interfaz"></a>

## 16. 🖥️ Responsive y criterios de interfaz

La interfaz utiliza:

- `Dock`
- `Anchor`
- `TableLayoutPanel`
- paneles persistentes
- scroll local
- layouts responsive

Criterios actuales:

- evitar reconstruir controles en cada `Resize`,
- evitar coordenadas absolutas innecesarias,
- mantener paneles estables,
- usar `Dock = Fill` donde corresponde,
- no mostrar controles a medio construir durante la carga.

### DataGridView

Las grillas del sistema comparten criterios de uso:

- columnas no redimensionables por el usuario,
- filas no redimensionables,
- encabezados legibles,
- columnas de texto con `Fill` cuando corresponde,
- columnas numéricas con ancho controlado,
- scroll horizontal local cuando no entra el contenido.

La intención es que la aplicación mantenga una presentación estable en distintas resoluciones.

[⬆ Volver al índice](#indice)

---

<a id="backup-seguridad"></a>

## 17. 🛡️ Back Up y seguridad

El sistema genera backups reales mediante SQL Server.

No utiliza:

```text
xp_cmdshell
FolderBrowserDialog para ruta de servidor
Restore desde la aplicación
```

El directorio se obtiene desde SQL Server.

La cuenta del servicio de SQL Server debe poseer permisos de escritura en la carpeta de destino.

### Seguridad

Buenas prácticas aplicadas:

- contraseñas hasheadas,
- SQL parametrizado,
- permisos por funcionalidad,
- autorización por capa,
- credenciales externas al código,
- bajas lógicas,
- auditoría,
- transacciones,
- no guardar secretos en logs.

[⬆ Volver al índice](#indice)

---

<a id="git"></a>

## 18. 🌿 Flujo Git recomendado

Repositorio:

```text
https://github.com/Exemax2014/Sistema-Gestion-Taller-II
```

Rama integrada utilizada durante el desarrollo:

```text
desarrollo
```

Ramas de trabajo utilizadas:

```text
exe-dev
josi-dev
```

Flujo recomendado:

```text
feature / rama personal
        ↓
commit
        ↓
push
        ↓
merge a desarrollo
```

### Revisar estado

```powershell
git status
```

### Actualizar referencias

```powershell
git fetch origin
```

### Actualizar desarrollo

```powershell
git checkout desarrollo
git pull origin desarrollo
```

### Revisar cambios

```powershell
git diff
git diff --check
```

### Historial

```powershell
git log --oneline --decorate -10
```

No utilizar `--force` salvo una situación controlada y deliberada.

[⬆ Volver al índice](#indice)

---

<a id="pruebas"></a>

## 19. 🧪 Pruebas manuales recomendadas

Un build exitoso no reemplaza las pruebas de interfaz y base de datos.

### Login

- Credenciales correctas.
- Credenciales incorrectas.
- Usuario inactivo.

### Inicio

- Primera carga.
- Resize.
- Cambio de módulo y regreso.
- Dashboard con datos.
- Avisos.

### Ventas

- Seleccionar cliente.
- Alta de cliente desde venta.
- Agregar producto.
- Cambiar cantidad.
- Quitar producto.
- Pago único.
- Pago combinado.
- Stock insuficiente.
- Confirmación.
- PDF.
- Advertencia por venta sin guardar.

### Clientes

- Alta.
- Edición.
- Baja/reactivación.
- Provincia/localidad.
- Calle autocomplete.
- Nueva localidad.
- Historial de compras.

### Productos

- Alta.
- Edición.
- Baja.
- Categoría.
- Marca.
- Stock por sucursal.

### Usuarios

- Alta.
- Edición.
- Perfil.
- Sucursal.
- Baja/reactivación.
- Edición de perfil propio.

### Sucursales

- Alta.
- Edición.
- Dirección.
- Baja/reactivación.

### Reportes

- General.
- Por usuario.
- Actividad.
- Ventas detalladas.
- Stock bajo.
- Filtros.
- Alcance global/sucursal/propio.
- Exportar Excel.

### Backup

- Generar `.bak`.
- Consultar historial.
- Confirmar auditoría.

### Responsive

Probar al menos:

```text
1920×1080
1600×900
1366×768
```

[⬆ Volver al índice](#indice)

---

<a id="troubleshooting"></a>

## 20. 🧯 Solución de problemas frecuentes

### `Could not find stored procedure ...`

Ejemplo:

```text
dbo.sp_Direccion_ListarCallesPorLocalidad
```

Causa habitual:

```text
Código actualizado + base con procedimientos viejos
```

Solución:

Ejecutar nuevamente:

[`03_Procedimientos.sql`](./BaseDatos/03_Procedimientos.sql)

---

### Error por columna inexistente como `altura`

Causa habitual:

el contrato de lectura del código y el `SELECT` del procedimiento almacenado no están sincronizados.

Primera acción:

```text
volver a ejecutar 03_Procedimientos.sql actualizado
```

Si persiste, comparar las columnas devueltas por el SP con las que espera `Capa_Datos`.

---

### `No se encontró configuracion.json`

Crear:

```text
Capa_Datos/Configuracion/configuracion.json
```

con los valores de la instancia SQL.

---

### No aparecen localidades o calles

Verificar:

```text
02_DatosIniciales.sql
04_DatosPrueba.sql
03_Procedimientos.sql
```

Las sugerencias de calle dependen de las direcciones ya existentes para la localidad.

---

### Error de conexión SQL Server

Revisar:

- nombre de servidor,
- instancia,
- autenticación,
- permisos,
- `TrustServerCertificate`,
- servicio de SQL Server activo.

---

### El backup falla

Revisar permisos de escritura de la **cuenta del servicio SQL Server** sobre la carpeta de backup.

La ruta corresponde al servidor, no al cliente WinForms.

---

### Paquetes faltantes

```powershell
dotnet restore .\Sistema_Hierro_Y_Forja.slnx
```

---

### Build limpio pero la GUI falla

El proyecto utiliza numerosos layouts dinámicos y consultas SQL.

Siempre realizar prueba manual después de:

- cambios responsive,
- cambios en SP,
- cambios de permisos,
- cambios de navegación,
- cambios de formularios.

[⬆ Volver al índice](#indice)

---

<a id="estado-del-proyecto"></a>

## 21. 📊 Estado actual y pendientes

### Implementado

- ✅ Login y sesión.
- ✅ Arquitectura en tres capas.
- ✅ Permisos dinámicos.
- ✅ Alcance global / sucursal / propio.
- ✅ Usuarios.
- ✅ Perfiles y permisos.
- ✅ Sucursales.
- ✅ Clientes.
- ✅ Historial de compras.
- ✅ Productos.
- ✅ Categorías.
- ✅ Marcas.
- ✅ Inventario por sucursal.
- ✅ Ventas.
- ✅ Pagos múltiples.
- ✅ Control de stock transaccional.
- ✅ Comprobante PDF.
- ✅ Dashboard.
- ✅ Avisos.
- ✅ Reportes.
- ✅ Exportación Excel.
- ✅ Auditoría.
- ✅ Back Up.
- ✅ Responsive general.
- ✅ Datos de prueba amplios.

### Pendientes / fuera de alcance actual

- ⏳ Prueba funcional integral sobre una instalación completamente limpia.
- ⏳ Validación final en otra PC / DPI / instancia SQL.
- ⏳ Instalador o mecanismo formal de despliegue.
- ⏳ Configuración de SQL Server para escenario de red real.
- ⏳ Restore de backup desde la aplicación.

No está implementado:

```text
Restore de base desde WinForms
```

[⬆ Volver al índice](#indice)

---

<a id="integrantes"></a>

## 22. 👥 Integrantes

Proyecto final de **Taller de Programación II**.

Integrantes:

- **Exequiel**
- **Josias**

Repositorio:

[github.com/Exemax2014/Sistema-Gestion-Taller-II](https://github.com/Exemax2014/Sistema-Gestion-Taller-II)

---

## 📌 Nota final

Para levantar una instalación funcional desde cero:

```text
1. Clonar repositorio
2. Ejecutar 01_Estructura.sql
3. Ejecutar 02_DatosIniciales.sql
4. Ejecutar 03_Procedimientos.sql
5. Crear Configuracion/configuracion.json
6. dotnet restore
7. dotnet build
8. Ejecutar Capa_Vistas
```

Para una base completa de demostración:

```text
01 → 02 → 03 → 05 → 04
```

---

[⬆ Volver al índice](#indice)
