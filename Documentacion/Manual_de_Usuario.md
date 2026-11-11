# Manual de Usuario — Sistema Hierro & Forja

> **Sistema de Gestión Comercial — Taller de Programación II**  
> Manual operativo de la aplicación de escritorio **Hierro & Forja**.  
> Documento preparado a partir del estado final integrado del proyecto en la rama `desarrollo`.

---

<a id="indice"></a>

## Índice

1. [Introducción](#1-introduccion)
2. [Objetivo del manual](#2-objetivo-del-manual)
3. [Conceptos generales antes de usar el sistema](#3-conceptos-generales)
4. [Inicio de sesión](#4-inicio-de-sesion)
5. [Ventana principal](#5-ventana-principal)
   - [Menú lateral](#51-menu-lateral)
   - [Datos de sesión](#52-datos-de-sesion)
   - [Selector de sucursal](#53-selector-de-sucursal)
   - [Editar perfil](#54-editar-perfil)
   - [Cerrar sesión y salir](#55-cerrar-sesion)
6. [Inicio / Dashboard](#6-inicio-dashboard)
7. [Módulo Ventas](#7-modulo-ventas)
   - [Nueva venta](#71-nueva-venta)
   - [Selección de cliente](#72-seleccion-cliente-venta)
   - [Alta de cliente desde una venta](#73-alta-cliente-desde-venta)
   - [Búsqueda y agregado de productos](#74-productos-venta)
   - [Cantidades y carrito](#75-carrito)
   - [Formas de pago](#76-formas-de-pago)
   - [Resumen y confirmación](#77-confirmacion-venta)
   - [Venta sin guardar](#78-venta-sin-guardar)
   - [Mis ventas / historial](#79-mis-ventas)
   - [Detalle de venta y PDF](#710-detalle-venta)
8. [Módulo Clientes](#8-modulo-clientes)
   - [Listado y filtros](#81-listado-clientes)
   - [Nuevo cliente](#82-nuevo-cliente)
   - [Campos y validaciones](#83-campos-cliente)
   - [Direcciones](#84-direccion-cliente)
   - [Editar cliente](#85-editar-cliente)
   - [Baja y reactivación](#86-baja-cliente)
   - [Historial de compras](#87-historial-cliente)
9. [Módulo Productos](#9-modulo-productos)
   - [Listado y búsqueda](#91-listado-productos)
   - [Alta y edición](#92-alta-producto)
   - [Campos y validaciones](#93-campos-producto)
   - [Categorías](#94-categorias)
   - [Marcas](#95-marcas)
   - [Stock por sucursal](#96-stock-producto)
   - [Baja y reactivación](#97-baja-producto)
10. [Módulo Usuarios](#10-modulo-usuarios)
    - [Listado de usuarios](#101-listado-usuarios)
    - [Nuevo usuario](#102-nuevo-usuario)
    - [Campos y validaciones](#103-campos-usuario)
    - [Editar usuario](#104-editar-usuario)
    - [Baja de usuario](#105-baja-usuario)
    - [Historial de ventas por usuario](#106-historial-usuario)
11. [Tipos de usuario y permisos](#11-perfiles-permisos)
12. [Sucursales](#12-sucursales)
13. [Reportes](#13-reportes)
    - [General](#131-reporte-general)
    - [Por usuario](#132-reporte-usuario)
    - [Actividad](#133-reporte-actividad)
    - [Ventas detalladas](#134-ventas-detalladas)
    - [Stock bajo](#135-stock-bajo)
    - [Filtros](#136-filtros-reportes)
    - [Exportar Excel](#137-exportar-excel)
14. [Avisos internos](#14-avisos)
15. [Back Up](#15-backup)
16. [Mensajes, confirmaciones y cambios sin guardar](#16-mensajes)
17. [Permisos y diferencias entre usuarios](#17-permisos)
18. [Reglas de datos y formatos aceptados](#18-reglas-datos)
19. [Casos de uso completos](#19-casos-de-uso)
20. [Errores frecuentes y cómo resolverlos](#20-errores-frecuentes)
21. [Buenas prácticas de uso](#21-buenas-practicas)
22. [Glosario](#22-glosario)
23. [Guía de capturas para el futuro PDF](#23-guia-capturas)

---

<a id="1-introduccion"></a>

## 1. Introducción

**Hierro & Forja** es una aplicación de escritorio diseñada para administrar de forma centralizada las operaciones principales de un comercio con una o más sucursales.

Desde la aplicación es posible:

- iniciar sesión con un usuario autorizado;
- operar sobre una sucursal determinada;
- registrar ventas;
- administrar clientes;
- consultar historial de compras;
- administrar productos;
- gestionar categorías y marcas;
- controlar stock por sucursal;
- administrar usuarios;
- administrar tipos de usuario y permisos;
- administrar sucursales;
- consultar reportes;
- revisar actividad del sistema;
- exportar reportes a Excel;
- generar comprobantes PDF;
- consultar y publicar avisos internos;
- generar copias de seguridad de la base de datos.

La interfaz está preparada para mostrar u ocultar funciones según los permisos del usuario autenticado.

> **Importante:** si una opción no aparece, está deshabilitada o no permite realizar una acción, puede deberse al perfil y permisos de la sesión actual.

[Volver al índice](#indice)

---

<a id="2-objetivo-del-manual"></a>

## 2. Objetivo del manual

Este documento explica **cómo utilizar la aplicación**, no cómo programarla.

Para cada módulo se detallan:

- qué función cumple;
- cómo ingresar;
- qué controles aparecen;
- qué datos se pueden ingresar;
- qué formatos son válidos;
- qué botones se utilizan;
- qué confirmaciones solicita;
- qué ocurre al guardar;
- qué permisos influyen;
- qué errores pueden aparecer;
- qué capturas conviene incorporar luego al PDF definitivo.

Este archivo está pensado también como base para producir un **Manual de Usuario en PDF con capturas reales de la aplicación**.

[Volver al índice](#indice)

---

<a id="3-conceptos-generales"></a>

## 3. Conceptos generales antes de usar el sistema

### Usuario

Es la cuenta con la que una persona inicia sesión.

Cada usuario tiene:

- nombre;
- apellido;
- DNI;
- teléfono opcional;
- nombre de usuario;
- correo;
- sexo;
- fecha de nacimiento opcional;
- tipo de usuario o perfil;
- sucursal, cuando el perfil lo requiere;
- permisos.

### Perfil o tipo de usuario

Define qué funciones puede utilizar una cuenta.

Ejemplos de perfiles iniciales:

- Administrador;
- Gerente;
- Vendedor.

Los nombres de los perfiles no son lo que autoriza directamente una acción: el sistema trabaja con **permisos o funcionalidades** asignadas.

### Sucursal fija

Es la sucursal asignada a un usuario cuyo perfil no es global.

Ese usuario trabaja dentro de ese alcance.

### Sucursal operativa

Es la sucursal concreta sobre la que se está realizando una operación.

Para una venta debe existir una sucursal operativa específica porque el stock se descuenta de esa sucursal.

### Alcance global

Un usuario con alcance global puede consultar información de todas las sucursales y, cuando corresponde, seleccionar una sucursal específica.

### Baja lógica

Cuando se da de baja un cliente, usuario, producto, perfil o sucursal, normalmente no se elimina físicamente su historial.

El registro pasa a un estado inactivo para conservar la trazabilidad.

### Stock y stock mínimo

Cada producto puede tener valores distintos por sucursal.

- **Stock:** cantidad actual.
- **Stock mínimo:** cantidad utilizada como límite de alerta.
- Cuando el stock llega o baja del mínimo, el sistema lo identifica como **Stock bajo**.

### Consumidor Final

Para ventas en las que no se desea registrar un comprador particular, los datos de prueba pueden incluir un cliente genérico denominado **Consumidor Final**.

El vendedor sigue siendo siempre el usuario real que inició sesión.

[Volver al índice](#indice)

---

<a id="4-inicio-de-sesion"></a>

## 4. Inicio de sesión

La aplicación inicia en la pantalla **Login**.

### Campos disponibles

| Campo | Uso |
|---|---|
| Usuario | Nombre de usuario asignado a la cuenta |
| Contraseña | Contraseña correspondiente |

### Cómo ingresar

1. Escribir el nombre de usuario.
2. Escribir la contraseña.
3. Presionar **Ingresar** o la tecla `Enter`.
4. El sistema valida las credenciales.
5. Si son correctas, se abre la ventana principal.
6. Si son incorrectas, se muestra un mensaje y se limpia la contraseña.

### Errores posibles

- Usuario vacío.
- Contraseña vacía.
- Usuario inexistente.
- Contraseña incorrecta.
- Cuenta dada de baja.
- Problema de conexión con SQL Server.

### Botones de ventana

La pantalla permite:

- minimizar;
- cerrar la aplicación;
- mover la ventana arrastrando las áreas habilitadas.

Al cerrar, la aplicación solicita confirmación.

> 📷 **Captura sugerida para PDF:** pantalla completa de Login con flechas señalando Usuario, Contraseña e Ingresar.

[Volver al índice](#indice)

---

<a id="5-ventana-principal"></a>

## 5. Ventana principal

Luego del login se abre `FormPrincipal`, que contiene:

- menú lateral;
- datos de cuenta;
- sucursal;
- cabecera;
- fecha y hora;
- panel central;
- opciones de ventana.

Los diferentes módulos se cargan dentro del panel principal sin abrir una aplicación nueva.

---

<a id="51-menu-lateral"></a>

### 5.1 Menú lateral

Según permisos pueden aparecer:

- **Inicio**
- **Ventas**
- **Clientes**
- **Productos**
- **Usuarios**
- **Reportes**
- **Back Up**

Además:

- **Editar perfil**
- **Cerrar sesión**

Las opciones sin autorización pueden mostrarse atenuadas o quedar bloqueadas.

> 📷 **Captura sugerida:** menú lateral completo con cada módulo numerado.

---

<a id="52-datos-de-sesion"></a>

### 5.2 Datos de sesión

La interfaz muestra información del usuario autenticado, como:

- nombre de usuario;
- perfil;
- sucursal actual.

Los textos se adaptan al ancho disponible y pueden mostrar ayuda contextual si son extensos.

---

<a id="53-selector-de-sucursal"></a>

### 5.3 Selector de sucursal

#### Usuario con sucursal fija

No puede cambiar libremente de sucursal.

La aplicación muestra su sucursal asignada.

#### Usuario global

Puede disponer de un selector con:

- **Todas las sucursales**
- cada sucursal activa disponible.

### Cuándo usar “Todas las sucursales”

Es apropiado para consultas globales como reportes o dashboard.

### Cuándo seleccionar una sucursal específica

Para operaciones que modifican stock, especialmente una venta.

Una venta necesita saber de qué sucursal se descontarán los productos.

> 📷 **Captura sugerida:** selector de sucursal de un usuario global.

---

<a id="54-editar-perfil"></a>

### 5.4 Editar perfil

La opción **Editar perfil** permite modificar los datos propios del usuario autenticado.

#### Datos editables

| Campo | Regla principal |
|---|---|
| Nombre | Obligatorio |
| Apellido | Obligatorio |
| DNI | Obligatorio, solo números |
| Teléfono | Opcional |
| Usuario | Obligatorio |
| Correo | Obligatorio y válido |
| Sexo | Opcional / No especificado |
| Fecha de nacimiento | Opcional, no futura |
| Nueva contraseña | Opcional |
| Confirmar nueva contraseña | Obligatorio si se cambia la contraseña |
| Contraseña actual | Obligatoria para confirmar cambios |

### Cambio de contraseña

Si no se quiere cambiar la contraseña:

- dejar vacíos **Nueva contraseña** y **Confirmar**;
- ingresar igualmente la contraseña actual para validar la modificación de datos.

Si se desea cambiarla:

1. escribir nueva contraseña;
2. repetirla;
3. escribir contraseña actual;
4. guardar.

La nueva contraseña debe tener entre **8 y 100 caracteres**.

### Datos que el usuario no cambia desde aquí

No se modifican desde “Editar perfil”:

- perfil;
- permisos;
- sucursal asignada;
- estado de la cuenta.

> 📷 **Captura sugerida:** formulario Editar perfil dividido en “Datos” y “Seguridad”.

---

<a id="55-cerrar-sesion"></a>

### 5.5 Cerrar sesión y salir

**Cerrar sesión** finaliza la sesión actual y vuelve al Login.

Cerrar el programa finaliza completamente la aplicación.

Cuando corresponde, el sistema solicita confirmación.

[Volver al índice](#indice)

---

<a id="6-inicio-dashboard"></a>

## 6. Inicio / Dashboard

El módulo **Inicio** funciona como resumen general.

Según permisos y alcance puede mostrar:

- ventas del día;
- ingresos del día;
- cantidad de productos;
- productos con stock bajo;
- accesos rápidos;
- ventas recientes;
- productos más vendidos;
- actividad reciente;
- avisos internos.

### Tarjetas de resumen

Permiten conocer el estado actual sin ingresar a cada módulo.

### Accesos rápidos

Llevan directamente a funciones frecuentes cuando el usuario dispone del permiso correspondiente.

### Gráficos

Los gráficos utilizan datos reales según:

- fecha;
- usuario;
- alcance;
- sucursal.

### Actividad reciente

Resume operaciones registradas en auditoría.

### Avisos

Presenta comunicaciones internas visibles para la sesión.

> 📷 **Captura sugerida:** Dashboard completo.
>
> 📷 **Captura sugerida adicional:** detalle de tarjetas y gráficos.

[Volver al índice](#indice)

---

<a id="7-modulo-ventas"></a>

## 7. Módulo Ventas

El módulo **Ventas** permite registrar operaciones comerciales y consultar ventas anteriores.

Para registrar una venta se requiere el permiso:

```text
VENTAS_REALIZAR
```

Para consultar ventas:

```text
VENTAS_VER
```

---

<a id="71-nueva-venta"></a>

### 7.1 Nueva venta

El proceso se divide conceptualmente en:

1. selección de productos;
2. selección de cliente;
3. formas de pago;
4. revisión del resumen;
5. confirmación.

Antes de empezar, comprobar la sucursal operativa.

---

<a id="72-seleccion-cliente-venta"></a>

### 7.2 Selección de cliente

La venta requiere un cliente válido.

El selector permite buscar clientes activos.

La búsqueda puede utilizar datos como:

- nombre;
- apellido;
- documento.

Cuando el selector está vacío puede listar clientes disponibles.

### Consumidor Final

Cuando no se quiere identificar al comprador particular, seleccionar el cliente **Consumidor Final**, si se encuentra cargado en la base.

Esto no modifica el vendedor real de la operación.

---

<a id="73-alta-cliente-desde-venta"></a>

### 7.3 Alta de cliente desde una venta

El botón **Nuevo cliente** permite abrir el alta de cliente sin perder la venta en curso.

Flujo:

1. iniciar una venta;
2. presionar **Nuevo cliente**;
3. completar el formulario;
4. guardar;
5. regresar a la misma venta;
6. el cliente recién creado queda disponible para seleccionarlo.

Si se cancela el alta, se retorna a la venta sin reemplazarla.

---

<a id="74-productos-venta"></a>

### 7.4 Búsqueda y agregado de productos

Los productos disponibles dependen de:

- que estén activos;
- que puedan venderse;
- la sucursal operativa;
- el stock de esa sucursal.

Se puede buscar por texto y seleccionar un producto.

La vista muestra información como:

- código;
- nombre;
- precio;
- stock disponible.

Solo deben agregarse cantidades válidas.

---

<a id="75-carrito"></a>

### 7.5 Cantidades y carrito

Cada producto agregado forma parte del detalle de venta.

Reglas:

- debe existir al menos un producto;
- la cantidad debe ser mayor a cero;
- un mismo producto no se envía duplicado como dos líneas independientes;
- no se debe superar el stock disponible;
- el precio final vuelve a validarse al confirmar.

Las cantidades pueden modificarse antes de confirmar.

Si se elimina una línea, el resumen se actualiza.

### Resumen de productos

El panel de resumen muestra los productos incluidos, con sus nombres y cantidades, además de totales generales.

> 📷 **Captura sugerida:** carrito con varios productos y distintas cantidades.

---

<a id="76-formas-de-pago"></a>

### 7.6 Formas de pago

La aplicación permite utilizar uno o varios métodos.

Métodos iniciales:

- Efectivo;
- Débito;
- Crédito;
- Transferencia.

### Agregar un pago

1. seleccionar método;
2. ingresar monto;
3. presionar **Agregar pago**;
4. verificar la grilla de pagos.

### Reglas

- debe existir al menos un pago;
- cada monto debe ser mayor a cero;
- los importes admiten hasta 2 decimales;
- el mismo método no debe aparecer duplicado en líneas separadas;
- si se necesita aumentar un método, debe utilizarse el importe acumulado;
- la suma de pagos debe cubrir correctamente el total exigido por la venta.

La vista informa:

- total de venta;
- total pagado;
- saldo pendiente.

> 📷 **Captura sugerida:** panel Formas de pago con dos métodos combinados.

---

<a id="77-confirmacion-venta"></a>

### 7.7 Resumen y confirmación

Antes de confirmar revisar:

- sucursal;
- vendedor;
- cliente;
- productos;
- cantidades;
- formas de pago;
- total;
- total pagado;
- saldo.

Al presionar **Confirmar venta**, el sistema vuelve a validar la operación.

SQL Server verifica nuevamente:

- usuario;
- permiso;
- sucursal;
- cliente;
- productos;
- cantidades;
- stock;
- precios;
- pagos.

Si todo es correcto:

1. registra la venta;
2. registra detalle;
3. registra pagos;
4. descuenta stock;
5. registra trazabilidad;
6. confirma la transacción.

Si falla una parte, la operación completa se revierte.

---

<a id="78-venta-sin-guardar"></a>

### 7.8 Venta sin guardar

Si existe una venta en preparación y el usuario intenta navegar hacia otro módulo, el sistema puede advertir que hay cambios u operación sin guardar.

Se debe elegir entre:

- continuar en la venta;
- salir y descartar.

Esto evita perder accidentalmente el trabajo realizado.

---

<a id="79-mis-ventas"></a>

### 7.9 Mis ventas / historial

La opción **Mis ventas** permite consultar ventas realizadas.

Según el origen y permisos, la grilla puede mostrar:

- número de venta;
- fecha;
- cliente;
- sucursal;
- subtotal;
- descuento;
- total;
- botón **Ver detalle**.

Los filtros dependen del formulario y del alcance del usuario.

---

<a id="710-detalle-venta"></a>

### 7.10 Detalle de venta y PDF

Al abrir **Ver detalle** se presenta:

#### Datos generales

- número;
- fecha y hora;
- sucursal;
- tipo de factura, si existe;
- cliente;
- documento;
- vendedor.

#### Productos

- código;
- producto;
- cantidad;
- precio;
- subtotal.

#### Pagos

- método;
- monto.

#### Totales

- subtotal;
- descuento;
- total.

### Generar PDF

El botón **Generar PDF** abre el diálogo para elegir dónde guardar el comprobante.

El archivo utiliza extensión:

```text
.pdf
```

> 📷 **Captura sugerida:** detalle completo de una venta.
>
> 📷 **Captura sugerida adicional:** diálogo Guardar comprobante.

[Volver al índice](#indice)

---

<a id="8-modulo-clientes"></a>

## 8. Módulo Clientes

El módulo permite administrar personas o entidades que compran en el comercio.

Permisos asociados:

```text
CLIENTES_VER
CLIENTES_ALTA
CLIENTES_MODIFICAR
CLIENTES_BAJA
```

---

<a id="81-listado-clientes"></a>

### 8.1 Listado y filtros

La grilla permite consultar clientes y sus estados.

La búsqueda admite información personal relevante.

El filtro de estado puede diferenciar:

- activos;
- dados de baja;
- todos.

Acciones posibles según permisos:

- crear;
- ver detalle;
- editar;
- dar de baja;
- reactivar;
- ver compras.

---

<a id="82-nuevo-cliente"></a>

### 8.2 Nuevo cliente

Presionar **Nuevo cliente**.

Completar datos personales y dirección.

Al guardar:

1. se valida el formulario;
2. se resuelve la localidad;
3. se valida la dirección;
4. se crea la dirección;
5. se registra el cliente.

Si se ingresó una localidad que todavía no existe, el sistema puede solicitar confirmación para crearla.

---

<a id="83-campos-cliente"></a>

### 8.3 Campos y validaciones

| Campo | Obligatorio | Regla |
|---|---:|---|
| Documento / DNI | Sí | Solo números, máximo 20 |
| Nombre | Sí | Letras, espacios, apóstrofe y guion, máximo 100 |
| Apellido | Sí | Letras, espacios, apóstrofe y guion, máximo 100 |
| Teléfono | No | Máximo 30, debe contener al menos un número |
| Email | No | Si se completa, debe ser un correo válido, máximo 150 |
| Provincia | Sí | Debe existir en catálogo |
| Localidad | Sí | Debe pertenecer a la provincia o crearse correctamente |
| Calle | Sí | Máximo 150, caracteres de dirección válidos |
| Altura | Sí | Entero positivo |
| Piso | No | Solo dígitos, 0 a 99 |

### Teléfono

Se permiten:

```text
0-9
+
-
(
)
espacios
```

Ejemplos:

```text
3794123456
+54 379 412-3456
(379) 4123456
```

### Email

Ejemplo válido:

```text
cliente@correo.com
```

Si no se dispone de correo puede dejarse vacío.

---

<a id="84-direccion-cliente"></a>

### 8.4 Dirección del cliente

La dirección se divide en:

- Provincia
- Localidad
- Calle
- Altura
- Piso

### Provincia

Se elige de un catálogo.

### Localidad

Al seleccionar provincia se cargan localidades correspondientes.

El control admite sugerencias.

Si se escribe una localidad nueva, el sistema puede confirmar su creación.

### Calle

El sistema ofrece autocomplete basado en calles ya registradas para la localidad.

No obliga a elegir una sugerencia: se puede ingresar una calle nueva válida.

Ejemplos de nombres válidos:

```text
Junín
9 de Julio
25 de Mayo
Av. 3 de Abril
Ruta 12
```

### Altura

Debe ser:

- numérica;
- mayor que cero.

Ejemplo:

```text
2064
```

### Piso

Es opcional.

Ejemplos válidos:

```text
0
1
12
```

Máximo permitido: `99`.

> 📷 **Captura sugerida:** Nuevo cliente completo mostrando Provincia, Localidad, Calle, Altura y Piso.

---

<a id="85-editar-cliente"></a>

### 8.5 Editar cliente

Desde el listado abrir el detalle.

Si el usuario tiene `CLIENTES_MODIFICAR`, los campos pueden editarse.

Al guardar:

- se actualizan datos personales;
- se actualiza la dirección actual;
- si el cliente no poseía dirección, puede crearse una.

Si no se posee permiso de modificación, el formulario funciona como consulta.

### Cambios sin guardar

Si se modificó información y se intenta volver, el sistema advierte sobre los cambios pendientes.

---

<a id="86-baja-cliente"></a>

### 8.6 Baja y reactivación

Dar de baja no debe destruir el historial comercial.

Un cliente dado de baja puede aparecer en filtros correspondientes y, con permiso, reactivarse.

---

<a id="87-historial-cliente"></a>

### 8.7 Historial de compras

Desde Clientes se puede abrir **Compras**.

El historial presenta las ventas asociadas al cliente.

Puede mostrar:

- venta;
- fecha;
- sucursal;
- vendedor;
- factura;
- total;
- detalle.

El botón **Ver detalle** abre la transacción completa.

> 📷 **Captura sugerida:** Historial de compras de un cliente con varias ventas.

[Volver al índice](#indice)

---

<a id="9-modulo-productos"></a>

## 9. Módulo Productos

Permisos:

```text
PRODUCTOS_VER
PRODUCTOS_ALTA
PRODUCTOS_MODIFICAR
PRODUCTOS_BAJA
```

El módulo agrupa también la gestión relacionada con:

- categorías;
- marcas;
- inventario.

---

<a id="91-listado-productos"></a>

### 9.1 Listado y búsqueda

La grilla puede mostrar:

- código;
- producto;
- categoría;
- marca;
- costo;
- porcentaje de ganancia;
- precio de venta;
- estado;
- acciones.

Los productos pueden mantenerse activos o inactivos.

---

<a id="92-alta-producto"></a>

### 9.2 Alta y edición

Para crear:

1. presionar **Nuevo producto**;
2. completar datos;
3. seleccionar categoría;
4. seleccionar marca, si corresponde;
5. indicar costo y porcentaje;
6. revisar precio de venta calculado;
7. guardar.

Al editar se cargan los datos existentes.

---

<a id="93-campos-producto"></a>

### 9.3 Campos y validaciones

| Campo | Obligatorio | Regla |
|---|---:|---|
| Categoría | Sí | Debe ser válida |
| Marca | No siempre | Puede utilizarse “Sin marca”; depende de relación con categoría |
| Código de barras | No necesariamente | Solo dígitos, máximo 50 |
| Nombre | Sí | Máximo 100 |
| Descripción | No | Texto descriptivo |
| Precio de costo | Sí | No negativo, hasta 2 decimales |
| % ganancia | Sí | 0 a 999,99; hasta 2 decimales |
| Activo | Estado | Controla disponibilidad |

### Nombre del producto

Admite letras, números y signos comerciales habituales, por ejemplo:

```text
Taladro 13mm
Amoladora Angular 900W
Kit-3 Brochas Profesional
```

### Código de barras

Se trata como texto numérico, no como número matemático.

No se permiten letras.

### Precio de costo

Ejemplos:

```text
50000
50000,50
```

No puede ser negativo.

### Porcentaje de ganancia

Ejemplos:

```text
40
35,50
```

El sistema calcula el precio de venta a partir del costo y porcentaje.

> 📷 **Captura sugerida:** formulario Producto con precio de venta calculado.

---

<a id="94-categorias"></a>

### 9.4 Categorías

Categorías organizan los productos.

Según permisos se puede:

- listar;
- crear;
- modificar;
- dar de baja;
- reactivar.

Una categoría posee principalmente:

- nombre;
- descripción;
- estado.

Evitar nombres duplicados o vacíos.

Las categorías dadas de baja no deberían utilizarse para nuevas operaciones hasta su reactivación.

---

<a id="95-marcas"></a>

### 9.5 Marcas

Las marcas pueden asociarse a una o más categorías.

Esto permite que, al seleccionar una categoría en un producto, el selector de marca muestre opciones compatibles.

Acciones:

- alta;
- modificación;
- baja;
- reactivación;
- asignación de categorías.

Si se cambia la categoría de un producto y la marca actual deja de ser compatible, debe seleccionarse una marca válida o “Sin marca”.

> 📷 **Captura sugerida:** gestión de Marcas con categorías asociadas.

---

<a id="96-stock-producto"></a>

### 9.6 Stock por sucursal

Cada producto posee inventario independiente por sucursal.

La grilla incluye:

- Sucursal;
- Stock;
- Stock mínimo;
- Estado;
- Modificar.

### Modificar stock

1. elegir una fila;
2. presionar **Modificar**;
3. ingresar nuevo stock;
4. ingresar stock mínimo;
5. guardar.

Ambos valores son enteros no negativos.

Ejemplos:

```text
Stock: 25
Stock mínimo: 10
```

### Estado

Si el stock es bajo, la interfaz lo identifica visualmente.

Desde Reportes > Stock bajo también puede abrirse directamente la gestión de stock del producto.

> 📷 **Captura sugerida:** stock por sucursal con filas en estado Correcto y Stock bajo.

---

<a id="97-baja-producto"></a>

### 9.7 Baja y reactivación

Un producto puede darse de baja/inactivar según permisos.

La baja evita su uso operativo normal sin destruir su historial.

Los productos inactivos pueden reactivarse.

[Volver al índice](#indice)

---

<a id="10-modulo-usuarios"></a>

## 10. Módulo Usuarios

El módulo Usuarios agrupa tres áreas, según permisos:

- Usuarios;
- Tipos de usuario y permisos;
- Sucursales.

---

<a id="101-listado-usuarios"></a>

### 10.1 Listado de usuarios

La grilla puede mostrar:

- usuario;
- nombre;
- apellido;
- perfil;
- sucursal;
- estado;
- detalle;
- ventas.

Filtros:

- texto;
- perfil;
- estado.

La búsqueda puede coincidir con:

- nombre de usuario;
- nombre;
- apellido;
- DNI;
- correo.

---

<a id="102-nuevo-usuario"></a>

### 10.2 Nuevo usuario

Pasos:

1. presionar **Nuevo usuario**;
2. completar datos;
3. elegir sexo;
4. indicar fecha de nacimiento si corresponde;
5. seleccionar perfil;
6. seleccionar sucursal si ese perfil la requiere;
7. crear contraseña;
8. confirmarla;
9. guardar.

---

<a id="103-campos-usuario"></a>

### 10.3 Campos y validaciones

| Campo | Obligatorio | Regla |
|---|---:|---|
| Nombre | Sí | Máximo 100, letras/espacios/apóstrofe/guion |
| Apellido | Sí | Máximo 100 |
| DNI | Sí | Solo números, máximo 20 |
| Teléfono | No | Hasta 30; formato válido |
| Usuario | Sí | Hasta 50; letras, números, `.`, `_`, `-` |
| Correo | Sí | Correo válido, máximo 150 |
| Sexo | No | No especificado / Masculino / Femenino / Otro |
| Fecha nacimiento | No | No futura |
| Perfil | Sí | Debe ser válido |
| Sucursal | Según perfil | Obligatoria para perfiles no globales |
| Contraseña | En alta | 8 a 100 caracteres |
| Confirmación | En alta | Debe coincidir |

### Usuario

Ejemplos válidos:

```text
juan.perez
vendedor_1
gerente-centro
```

No se admiten espacios.

### Perfil y sucursal

Si el perfil tiene alcance global:

- no requiere sucursal fija.

Si no es global:

- debe seleccionarse una sucursal disponible.

---

<a id="104-editar-usuario"></a>

### 10.4 Editar usuario

Al editar:

- se cargan datos existentes;
- contraseña y confirmación no se muestran como parte de la edición normal;
- pueden modificarse los datos autorizados;
- perfil y sucursal se ajustan según las reglas del perfil.

---

<a id="105-baja-usuario"></a>

### 10.5 Baja de usuario

Al dar de baja:

- se solicita confirmación;
- el usuario deja de poder ingresar;
- se conserva historial.

No se permite que el usuario activo se dé de baja a sí mismo mediante esa acción.

---

<a id="106-historial-usuario"></a>

### 10.6 Historial de ventas por usuario

Desde la grilla puede abrirse el historial del vendedor.

Permite consultar sus ventas y abrir cada detalle.

[Volver al índice](#indice)

---

<a id="11-perfiles-permisos"></a>

## 11. Tipos de usuario y permisos

Esta subvista requiere autorización para gestionar permisos.

### Elementos principales

- lista de perfiles;
- nombre;
- descripción;
- funcionalidades disponibles;
- selección de permisos;
- guardar;
- eliminar perfil cuando corresponde.

### Crear un perfil

1. presionar **Nuevo tipo de usuario**;
2. escribir nombre;
3. escribir descripción;
4. marcar funcionalidades;
5. revisar alcance de reportes;
6. guardar.

### Nombre

- obligatorio;
- máximo 50;
- letras;
- números;
- espacios;
- guiones;
- apóstrofes.

### Descripción

- opcional;
- máximo 200;
- sin caracteres de control.

### Perfil global

El perfil global del sistema está protegido.

No puede modificarse ni darse de baja como un perfil común.

### PERMISOS_GESTIONAR

La administración de permisos es exclusiva del perfil global.

### Alcance de reportes

Un perfil no debe tener más de un alcance simultáneo.

Posibles alcances:

```text
REPORTES_ALCANCE_PROPIO
REPORTES_ALCANCE_SUCURSAL
REPORTES_ALCANCE_GLOBAL
```

El alcance global requiere un perfil global.

### Cambios sin guardar

Si se está editando un perfil y se intenta cambiar de subvista, se solicita confirmación antes de perder cambios.

> 📷 **Captura sugerida:** editor de Perfil con lista de funcionalidades.

[Volver al índice](#indice)

---

<a id="12-sucursales"></a>

## 12. Sucursales

La subvista **Sucursales** permite administrar puntos operativos.

Permisos:

```text
SUCURSALES_VER
SUCURSALES_ALTA
SUCURSALES_MODIFICAR
SUCURSALES_BAJA
```

### Listado

Cada bloque puede mostrar:

- nombre;
- estado;
- dirección;
- usuarios activos por perfil;
- modificar / ver detalle;
- dar de baja / reactivar.

### Nueva sucursal

Completar:

- nombre;
- provincia;
- localidad;
- calle;
- altura;
- piso cuando corresponda;
- estado activa.

### Dirección

La versión final del formulario utiliza la dirección desglosada para mantener consistencia con Clientes:

```text
Provincia
Localidad
Calle
Altura
Piso
```

Las reglas de dirección siguen el mismo criterio general del sistema.

### Localidades

Al seleccionar provincia se listan localidades.

Cuando la interfaz lo permite, una localidad nueva puede ser confirmada y creada.

### Baja

Una sucursal dada de baja deja de estar disponible para nuevas operaciones normales.

### Reactivación

Puede volver a habilitarse si el usuario posee permiso.

> 📷 **Captura sugerida:** listado de sucursales.
>
> 📷 **Captura sugerida adicional:** formulario Nueva/Modificar sucursal.

[Volver al índice](#indice)

---

<a id="13-reportes"></a>

## 13. Reportes

El módulo Reportes es dinámico.

Solo muestra secciones autorizadas.

Para abrirlo debe existir:

```text
REPORTES_VER
```

además de permisos para al menos una sección.

Las secciones finales son:

- General;
- Por usuario;
- Actividad;
- Ventas detalladas;
- Stock bajo.

---

<a id="131-reporte-general"></a>

### 13.1 General

Resume rendimiento comercial del período.

Puede incluir tarjetas:

- ventas;
- ingresos;
- productos;
- stock bajo.

También gráficos y tablas:

- ventas del período;
- ingresos por día;
- productos más vendidos;
- distribución por sucursal cuando el alcance lo permite;
- ranking de productos.

Los valores respetan filtros y alcance.

> 📷 **Captura sugerida:** Reportes > General con las tarjetas y gráficos completos.

---

<a id="132-reporte-usuario"></a>

### 13.2 Por usuario

Permite analizar rendimiento individual.

Si el alcance es propio, el sistema utiliza automáticamente el usuario actual.

Si el alcance permite elegir vendedores:

1. seleccionar sucursal, si corresponde;
2. seleccionar usuario;
3. establecer fechas;
4. aplicar filtros.

Indicadores:

- Ventas;
- Ingresos;
- Productos;
- Ticket promedio.

Además:

- evolución;
- productos vendidos;
- historial de ventas;
- Ver detalle.

Si no se selecciona un usuario cuando corresponde, aparece un estado indicando que debe elegirse uno.

> 📷 **Captura sugerida:** Por usuario con un vendedor seleccionado.

---

<a id="133-reporte-actividad"></a>

### 13.3 Actividad

Muestra la auditoría del sistema.

Columnas:

- Fecha;
- Usuario;
- Acción;
- Entidad;
- Detalle;
- Sucursal.

La columna Detalle puede mostrar tooltip con el texto completo.

Sirve para revisar operaciones realizadas por usuarios.

---

<a id="134-ventas-detalladas"></a>

### 13.4 Ventas detalladas

Lista transacciones del período.

Puede incluir:

- número;
- fecha;
- cliente;
- vendedor;
- sucursal;
- subtotal;
- descuento;
- total;
- pagos;
- Ver detalle.

Desde **Ver detalle** se abre la venta completa.

---

<a id="135-stock-bajo"></a>

### 13.5 Stock bajo

Lista productos que requieren reposición.

Columnas:

- Código;
- Producto;
- Categoría;
- Sucursal;
- Stock;
- Stock mínimo;
- Diferencia;
- Gestionar stock.

El botón **Gestionar stock** abre la vista de inventario del producto correspondiente.

> 📷 **Captura sugerida:** Stock bajo con varias filas.

---

<a id="136-filtros-reportes"></a>

### 13.6 Filtros

Filtros principales:

- Desde;
- Hasta;
- Sucursal;
- Usuario, en secciones compatibles.

### Fechas

La fecha **Desde** no puede ser posterior a **Hasta**.

Por defecto, el módulo puede iniciar con un período reciente.

### Sucursal

#### Alcance global

Puede ofrecer:

- Todas las sucursales;
- una sucursal específica.

#### Alcance de sucursal

La sucursal se muestra fija.

#### Alcance propio

La consulta se restringe al usuario actual según la sección.

### Botones

- **Aplicar filtros**
- **Limpiar**

Limpiar restaura el estado correspondiente a la sección.

---

<a id="137-exportar-excel"></a>

### 13.7 Exportar Excel

Disponible con:

```text
REPORTES_EXPORTAR
```

Genera archivos:

```text
.xlsx
```

Los archivos se crean con ClosedXML y no son CSV renombrados.

No se necesita Microsoft Excel instalado para generar el archivo.

> 📷 **Captura sugerida:** diálogo Guardar Excel.

[Volver al índice](#indice)

---

<a id="14-avisos"></a>

## 14. Avisos internos

El sistema maneja comunicaciones internas.

Permisos:

```text
AVISOS_VER
AVISOS_PUBLICAR
```

Un aviso puede ser:

### Global

Visible para usuarios según reglas del sistema.

La base almacena:

```text
id_sucursal = NULL
```

### Por sucursal

Dirigido a una sucursal específica.

### Información

Los avisos incluyen:

- título;
- mensaje;
- fecha;
- autor;
- alcance;
- estado.

Los avisos visibles se muestran principalmente en Inicio.

> 📷 **Captura sugerida:** sección Avisos del Dashboard.

[Volver al índice](#indice)

---

<a id="15-backup"></a>

## 15. Back Up

Requiere:

```text
BACKUP_REALIZAR
```

### Objetivo

Crear una copia de seguridad real de SQL Server en formato:

```text
.bak
```

### Cómo realizar una copia

1. ingresar a **Back Up**;
2. revisar información de base;
3. presionar el botón para realizar la copia;
4. confirmar si la interfaz lo solicita;
5. esperar respuesta de SQL Server;
6. verificar mensaje de éxito;
7. revisar historial.

### Ubicación

La carpeta de destino se obtiene del servidor SQL.

No se selecciona una carpeta local de Windows desde la aplicación.

### Nombre

Formato aproximado:

```text
SistemaGestion_yyyyMMdd_HHmmss.bak
```

### Historial

Se obtiene a partir de registros de auditoría asociados a backups.

Puede mostrar:

- fecha;
- usuario;
- sucursal;
- archivo o detalle.

### Restricciones

La aplicación no implementa restauración de backups.

> 📷 **Captura sugerida:** pantalla Back Up completa e historial.

[Volver al índice](#indice)

---

<a id="16-mensajes"></a>

## 16. Mensajes, confirmaciones y cambios sin guardar

El sistema utiliza diálogos propios (`FormMensaje`) para mantener una experiencia visual consistente.

### Tipos

- éxito;
- advertencia;
- error;
- confirmación.

### Confirmaciones frecuentes

- cerrar programa;
- cerrar sesión;
- salir con cambios sin guardar;
- dar de baja;
- reactivar;
- crear localidad nueva;
- confirmar operaciones sensibles.

### Cambios sin guardar

Aparecen especialmente al:

- editar cliente;
- preparar una venta;
- editar perfil/permisos;
- abandonar formularios con modificaciones.

Leer siempre el texto antes de aceptar.

[Volver al índice](#indice)

---

<a id="17-permisos"></a>

## 17. Permisos y diferencias entre usuarios

La interfaz cambia según la cuenta.

### Ejemplos

Un vendedor puede:

- vender;
- consultar clientes;
- registrar clientes;
- consultar sus reportes.

Un gerente puede disponer de:

- clientes;
- reportes de sucursal;
- stock;
- avisos.

Un administrador global puede disponer de:

- usuarios;
- permisos;
- sucursales;
- productos;
- reportes globales;
- backup.

### Regla importante

No se debe asumir que dos usuarios ven exactamente la misma pantalla.

Los botones y secciones dependen de las funcionalidades asignadas.

### Principales códigos

```text
USUARIOS_VER
USUARIOS_ALTA
USUARIOS_MODIFICAR
USUARIOS_BAJA

PERMISOS_GESTIONAR

VENTAS_VER
VENTAS_REALIZAR

CLIENTES_VER
CLIENTES_ALTA
CLIENTES_MODIFICAR
CLIENTES_BAJA

PRODUCTOS_VER
PRODUCTOS_ALTA
PRODUCTOS_MODIFICAR
PRODUCTOS_BAJA

CATEGORIAS_VER
CATEGORIAS_ALTA
CATEGORIAS_MODIFICAR
CATEGORIAS_BAJA

MARCAS_VER
MARCAS_ALTA
MARCAS_MODIFICAR
MARCAS_BAJA

SUCURSALES_VER
SUCURSALES_ALTA
SUCURSALES_MODIFICAR
SUCURSALES_BAJA

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

AVISOS_VER
AVISOS_PUBLICAR

BACKUP_REALIZAR
```

[Volver al índice](#indice)

---

<a id="18-reglas-datos"></a>

## 18. Reglas de datos y formatos aceptados

### Nombres y apellidos

Admiten:

- letras;
- espacios;
- `'`;
- `-`.

No deben contener números.

### DNI / Documento

Solo dígitos.

Máximo:

```text
20
```

### Teléfono

Opcional en Clientes y Usuarios.

Caracteres:

```text
0-9 + - ( ) espacio
```

Debe contener al menos un número si se completa.

### Correo

Formato general:

```text
usuario@dominio.com
```

No se aceptan formatos incompletos.

### Nombre de usuario

Permitidos:

```text
A-Z
a-z
0-9
.
_
-
```

Sin espacios.

### Contraseña de usuario

Alta:

```text
8 a 100 caracteres
```

### Fecha de nacimiento

Opcional.

No puede ser futura.

### Calle

Admite letras Unicode, números y signos habituales de dirección.

Ejemplos:

```text
Junín
9 de Julio
Av. 3 de Abril
Ruta 12
```

### Altura

- obligatoria en direcciones de clientes;
- solo números;
- mayor que cero.

### Piso

- opcional;
- entero entre 0 y 99.

### Código de barras

- hasta 50;
- solo dígitos si se completa.

### Precio de costo

- mayor o igual a cero;
- máximo 2 decimales.

### Porcentaje de ganancia

```text
0 a 999,99
```

Máximo 2 decimales.

### Stock

Entero no negativo.

### Stock mínimo

Entero no negativo.

### Monto de pago

- mayor que cero;
- máximo 2 decimales.

[Volver al índice](#indice)

---

<a id="19-casos-de-uso"></a>

## 19. Casos de uso completos

### Caso 1 — Registrar una venta normal

1. Iniciar sesión.
2. Seleccionar sucursal operativa.
3. Entrar a Ventas.
4. Buscar producto.
5. Agregar producto.
6. Definir cantidad.
7. Seleccionar cliente.
8. Seleccionar método de pago.
9. Ingresar monto.
10. Agregar pago.
11. Revisar saldo.
12. Revisar resumen.
13. Confirmar venta.
14. Abrir detalle.
15. Generar PDF si se desea.

---

### Caso 2 — Venta con cliente nuevo

1. Iniciar nueva venta.
2. Agregar productos.
3. Presionar **Nuevo cliente**.
4. Completar cliente.
5. Guardar.
6. Volver a la venta.
7. Seleccionar el nuevo cliente.
8. Agregar pagos.
9. Confirmar.

---

### Caso 3 — Venta sin comprador identificado

1. Iniciar nueva venta.
2. Agregar productos.
3. Seleccionar **Consumidor Final**.
4. Cargar pagos.
5. Confirmar.

El usuario vendedor sigue siendo el autenticado.

---

### Caso 4 — Pago combinado

Ejemplo total:

```text
$100.000
```

Pagos:

```text
Efectivo       $40.000
Transferencia  $60.000
```

La suma debe coincidir con el importe exigido.

---

### Caso 5 — Reponer stock desde Reportes

1. Entrar a Reportes.
2. Abrir **Stock bajo**.
3. Aplicar filtros.
4. Buscar producto.
5. Presionar **Gestionar stock**.
6. Elegir sucursal.
7. Modificar stock.
8. Modificar mínimo si corresponde.
9. Guardar.
10. Volver a Reportes.

---

### Caso 6 — Consultar compras de un cliente

1. Entrar a Clientes.
2. Buscar cliente.
3. Presionar **Compras**.
4. Revisar historial.
5. Presionar **Ver detalle**.
6. Revisar productos/pagos/totales.
7. Generar PDF si se requiere.

---

### Caso 7 — Crear un vendedor

1. Entrar a Usuarios.
2. Nuevo usuario.
3. Completar datos.
4. Elegir perfil Vendedor.
5. Seleccionar sucursal.
6. Crear contraseña.
7. Guardar.
8. Cerrar sesión.
9. Probar el acceso con la nueva cuenta.

---

### Caso 8 — Crear un perfil personalizado

1. Usuarios.
2. Tipos de usuario y permisos.
3. Nuevo tipo.
4. Escribir nombre.
5. Escribir descripción.
6. Marcar funcionalidades.
7. Seleccionar solo un alcance de reportes.
8. Guardar.
9. Crear/asignar usuario con ese perfil.

---

### Caso 9 — Generar un backup

1. Iniciar con una cuenta autorizada.
2. Ir a Back Up.
3. Realizar copia.
4. Verificar éxito.
5. Confirmar historial.

[Volver al índice](#indice)

---

<a id="20-errores-frecuentes"></a>

## 20. Errores frecuentes y cómo resolverlos

### “No tiene permiso…”

La cuenta no posee la funcionalidad necesaria.

Solución:

- utilizar otra cuenta;
- pedir al administrador revisar el perfil.

---

### No puedo vender en “Todas las sucursales”

Es correcto.

La venta necesita una sucursal concreta para descontar inventario.

Seleccionar una sucursal operativa.

---

### No aparecen productos para vender

Comprobar:

- sucursal correcta;
- producto activo;
- stock;
- texto de búsqueda.

---

### Stock insuficiente

La cantidad solicitada supera la disponibilidad.

Reducir la cantidad o actualizar inventario con autorización.

---

### No puedo modificar un cliente/producto/usuario

Puede existir permiso de consulta, pero no de modificación.

---

### Correo inválido

Revisar que tenga formato:

```text
nombre@dominio.com
```

---

### Localidad no encontrada

Si la interfaz admite alta, escribir la nueva localidad y confirmar su creación.

---

### La calle no aparece en sugerencias

El autocomplete se basa en calles existentes de esa localidad.

Se puede ingresar una nueva calle válida.

---

### Altura inválida

Ingresar solo un entero positivo.

Válido:

```text
1234
```

Inválido:

```text
0
12A
-20
```

---

### Piso inválido

Usar solo números de 0 a 99 o dejar vacío.

---

### No se puede confirmar la venta

Revisar:

- cliente;
- productos;
- cantidad;
- stock;
- métodos;
- montos;
- saldo;
- sucursal.

---

### No se genera el backup

Puede tratarse de permisos del servicio SQL Server sobre la carpeta de backups.

Debe revisarlo quien administra la base.

---

### No aparece una sección de Reportes

La sesión no posee el permiso correspondiente.

[Volver al índice](#indice)

---

<a id="21-buenas-practicas"></a>

## 21. Buenas prácticas de uso

1. Verificar siempre la sucursal antes de vender.
2. No compartir cuentas entre personas.
3. Utilizar un usuario real para cada vendedor.
4. Utilizar Consumidor Final solo cuando corresponde.
5. No crear clientes duplicados.
6. Revisar el DNI antes de dar de alta.
7. Revisar stock antes de confirmar grandes cantidades.
8. No cerrar una venta a mitad de carga sin leer la advertencia.
9. Mantener stock mínimo coherente.
10. No otorgar permisos administrativos innecesarios.
11. Realizar backups periódicos.
12. Revisar Stock bajo.
13. Utilizar Reportes para validar actividad comercial.
14. Revisar Auditoría ante operaciones dudosas.
15. No compartir contraseñas.
16. Cambiar la contraseña personal desde Editar perfil cuando sea necesario.

[Volver al índice](#indice)

---

<a id="22-glosario"></a>

## 22. Glosario

| Término | Significado |
|---|---|
| Alcance | Conjunto de datos que el usuario puede consultar |
| Alcance propio | Solo operaciones del usuario |
| Alcance sucursal | Operaciones de la sucursal asignada |
| Alcance global | Operaciones de todas las sucursales |
| Auditoría | Registro histórico de acciones |
| Baja lógica | Inactivación sin borrar historial |
| Cliente | Comprador registrado |
| Consumidor Final | Cliente genérico |
| Inventario | Stock de un producto por sucursal |
| Perfil | Tipo de usuario |
| Permiso | Funcionalidad habilitada |
| Sucursal operativa | Sucursal concreta utilizada en una operación |
| Stock mínimo | Umbral de alerta |
| Ticket promedio | Ingresos / cantidad de ventas |
| Venta | Transacción comercial confirmada |

[Volver al índice](#indice)

---

<a id="23-guia-capturas"></a>

## 23. Guía de capturas para el futuro PDF

Para convertir este manual en PDF ilustrado, se recomienda obtener capturas reales en este orden:

### Captura 01 — Login

Mostrar:

- Usuario;
- Contraseña;
- Ingresar.

### Captura 02 — Ventana principal

Mostrar:

- menú;
- usuario;
- perfil;
- sucursal;
- fecha.

### Captura 03 — Selector de sucursal

Usar cuenta global.

### Captura 04 — Dashboard

Mostrar toda la pantalla.

### Captura 05 — Nueva venta / productos

Mostrar productos y carrito.

### Captura 06 — Nueva venta / cliente y pagos

Mostrar selector de cliente y formas de pago.

### Captura 07 — Resumen de venta

Mostrar productos por nombre, cantidades y totales.

### Captura 08 — Detalle de venta

Mostrar productos, pagos y totales.

### Captura 09 — Clientes

Listado.

### Captura 10 — Nuevo cliente

Mostrar todos los campos, especialmente dirección.

### Captura 11 — Historial de compras

Cliente con varias ventas.

### Captura 12 — Productos

Listado.

### Captura 13 — Producto detalle

Mostrar categoría, marca, precio y estado.

### Captura 14 — Stock por sucursal

Mostrar estados Correcto / Stock bajo.

### Captura 15 — Categorías

Listado/edición.

### Captura 16 — Marcas

Listado/relación categorías.

### Captura 17 — Usuarios

Listado.

### Captura 18 — Nuevo usuario

Formulario completo.

### Captura 19 — Perfiles y permisos

Mostrar checkboxes/funcionalidades.

### Captura 20 — Sucursales

Listado.

### Captura 21 — Editar sucursal

Mostrar dirección desglosada.

### Captura 22 — Reportes General

Tarjetas y gráficos.

### Captura 23 — Reportes Por usuario

Vendedor seleccionado.

### Captura 24 — Actividad

Grilla con varias acciones.

### Captura 25 — Ventas detalladas

Grilla y botón Ver detalle.

### Captura 26 — Stock bajo

Mostrar botón Gestionar stock.

### Captura 27 — Exportar Excel

Diálogo de archivo.

### Captura 28 — Back Up

Vista completa e historial.

### Captura 29 — Editar perfil

Datos + seguridad.

### Captura 30 — Mensaje de confirmación

Ejemplo de baja o cambios sin guardar.

---

## Fin del manual

Este documento describe el uso operativo de la versión final del sistema **Hierro & Forja** integrada en la rama `desarrollo`.

Para el PDF definitivo se recomienda conservar esta misma estructura, incorporar las capturas indicadas y numerarlas como figuras:

```text
Figura 1 — Inicio de sesión
Figura 2 — Menú principal
Figura 3 — Dashboard
...
```

[Volver al índice](#indice)
