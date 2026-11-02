# Checklist de pruebas finales — Sistema Hierro y Forja

**Materia:** Taller de Programación II
**Integrantes:** Josias Correa, Exequiel
**Fecha de ejecución:** ____ / ____ / 2026
**Versión probada (commit / rama):** ______________________

## Preparación del entorno

1. Ejecutar en SQL Server, en este orden: `01_Estructura.sql` → `02_DatosIniciales.sql` → `03_Procedimientos.sql` → `05_ResetBasePruebas.sql` → `04_DatosPrueba.sql`.
2. Compilar la solución completa (sin errores) y ejecutar `Capa_Vistas`.
3. Usuarios de prueba (clave de todos: `Prueba2026!`): `coordinador_global_test`, `gerente_a`, `gerente_b`, `vendedor_a1`, `vendedor_a2`, `vendedor_a3`, `vendedor_b1`, `vendedor_b2`, `vendedor_b3`, `vendedor_central1`, `vendedor_central2`.

**Convención de resultado:** `OK` = funciona como se espera · `Falla` = no funciona (anotar detalle) · `Pend.` = sin probar.

---

## 1. Inicio de sesión y sesión

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 1.1 | Login con usuario y clave válidos | Ingresa al sistema y muestra el menú según el perfil | Pend. | |
| 1.2 | Login con clave incorrecta | Mensaje de error, no ingresa | Pend. | |
| 1.3 | Login con usuario inexistente | Mensaje de error, no ingresa | Pend. | |
| 1.4 | Login con campos vacíos | Validación, no ingresa | Pend. | |
| 1.5 | Cerrar sesión | Vuelve al login y no queda sesión activa | Pend. | |
| 1.6 | Nombre de usuario largo en el menú lateral | El texto se ve completo (no cortado) | Pend. | |

## 2. Permisos por perfil

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 2.1 | Administrador: ve todos los módulos | Menú completo | Pend. | |
| 2.2 | Gerente (`gerente_a`): ve solo lo que su perfil permite | Sin módulos no autorizados | Pend. | |
| 2.3 | Vendedor (`vendedor_a1`): ve solo lo que su perfil permite | Sin módulos no autorizados | Pend. | |
| 2.4 | Quitar un permiso a un perfil y volver a ingresar | El módulo deja de mostrarse | Pend. | |
| 2.5 | Agregar un permiso a un perfil y volver a ingresar | El módulo aparece | Pend. | |
| 2.6 | Alcance por sucursal (gerente A vs. gerente B) | Cada uno ve solo datos de su alcance | Pend. | |

## 3. Usuarios

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 3.1 | Listar usuarios | Se muestran con sucursal y perfil | Pend. | |
| 3.2 | Alta de usuario válido | Se guarda y aparece en el listado | Pend. | |
| 3.3 | Alta con DNI o correo duplicado | Mensaje de error, no se guarda | Pend. | |
| 3.4 | Alta con campos obligatorios vacíos | Validación | Pend. | |
| 3.5 | Modificar usuario | Cambios guardados | Pend. | |
| 3.6 | Baja / desactivar usuario | No puede ingresar | Pend. | |
| 3.7 | Mi perfil: editar datos propios | Cambios guardados sin errores | Pend. | |
| 3.8 | Mi perfil: cambiar clave | Ingresa con la clave nueva | Pend. | |

## 4. Tipos de usuario y permisos

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 4.1 | Listar perfiles y sus permisos | Se muestran correctamente | Pend. | |
| 4.2 | Crear perfil nuevo con permisos | Se guarda | Pend. | |
| 4.3 | Modificar permisos de un perfil | Se guarda y se aplica al reingresar | Pend. | |
| 4.4 | Perfil con usuarios asignados | No se puede eliminar (o avisa) | Pend. | |

## 5. Sucursales

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 5.1 | Listar sucursales | Se muestran las 3 de prueba | Pend. | |
| 5.2 | Alta de sucursal | Se guarda | Pend. | |
| 5.3 | Modificar sucursal | Cambios guardados | Pend. | |
| 5.4 | Baja de sucursal con datos asociados | Se bloquea o avisa | Pend. | |

## 6. Clientes

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 6.1 | Listar clientes | Se muestran los 40 de prueba | Pend. | |
| 6.2 | Buscar por apellido / DNI | Filtra el listado | Pend. | |
| 6.3 | Alta de cliente válido | Se guarda | Pend. | |
| 6.4 | Alta con DNI duplicado | Mensaje de error | Pend. | |
| 6.5 | Modificar cliente | Cambios guardados | Pend. | |
| 6.6 | Baja de cliente | Deja de aparecer como activo | Pend. | |

## 7. Productos, categorías, marcas e inventario

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 7.1 | Listar productos | Se muestran los 25 de prueba | Pend. | |
| 7.2 | Alta de producto válido | Se guarda | Pend. | |
| 7.3 | Alta con código de barras duplicado | Mensaje de error | Pend. | |
| 7.4 | Modificar producto / precio | Cambios guardados | Pend. | |
| 7.5 | Alta / modificación de categoría y marca | Se guarda | Pend. | |
| 7.6 | Stock por sucursal | Muestra el stock correcto | Pend. | |
| 7.7 | Botón "Volver" en Productos | Regresa a la pantalla anterior | Pend. | |

## 8. Ventas

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 8.1 | Nueva venta como vendedor: lista de productos | Muestra productos con stock en su sucursal | Pend. | |
| 8.2 | Seleccionar cliente (búsqueda ágil) | Encuentra por apellido / DNI | Pend. | |
| 8.3 | Agregar productos y cantidades | Calcula subtotal y total | Pend. | |
| 8.4 | Cantidad mayor al stock | Bloquea o avisa | Pend. | |
| 8.5 | Confirmar venta | Se registra y descuenta stock | Pend. | |
| 8.6 | Número de venta asignado al confirmar | Número único, sin repetirse | Pend. | |
| 8.7 | Venta sin cliente / sin productos | Validación | Pend. | |
| 8.8 | Historial de ventas | Lista las ventas según alcance | Pend. | |
| 8.9 | Abrir detalle de venta | Muestra cabecera e ítems | Pend. | |
| 8.10 | Cerrar detalle de venta | Vuelve al historial | Pend. | |
| 8.11 | Generar comprobante PDF | Genera el PDF de la venta | Pend. | |

## 9. Reportes

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 9.1 | General: ventas, recaudación y productos | Valores coherentes con los datos | Pend. | |
| 9.2 | Tarjeta "Productos" unificada | Se ve igual que las demás tarjetas | Pend. | |
| 9.3 | Por usuario (rendimiento de vendedores) | Ranking correcto | Pend. | |
| 9.4 | Ventas detalladas | Lista con filtros de fecha | Pend. | |
| 9.5 | Abrir detalle de venta desde Ventas detalladas y volver | Regresa a **Ventas detalladas**, no a General | Pend. | |
| 9.6 | Lo mismo desde Por usuario / Stock bajo | Regresa a la sección de origen | Pend. | |
| 9.7 | Stock bajo | Lista productos bajo el mínimo | Pend. | |
| 9.8 | Filtro por sucursal | Filtra según la sucursal elegida | Pend. | |
| 9.9 | Rango de fechas por defecto | Muestra datos al abrir | Pend. | |
| 9.10 | Gerente: solo ve su alcance | Sin datos de otras sucursales | Pend. | |
| 9.11 | Usuario sin permisos REPORTES_* | Mensaje de acceso denegado | Pend. | |

## 10. Inicio / Dashboard

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 10.1 | Inicio como Administrador | Carga sin errores | Pend. | |
| 10.2 | Inicio como Gerente | Diseño correcto, sin elementos superpuestos | Pend. | |
| 10.3 | Inicio como Vendedor | Muestra lo que corresponde | Pend. | |
| 10.4 | Avisos internos | Se listan los avisos de prueba | Pend. | |

## 11. Back Up y auditoría

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 11.1 | Realizar back up | Genera el archivo en la carpeta elegida | Pend. | |
| 11.2 | Back up sin permiso | Opción no disponible | Pend. | |
| 11.3 | Historial de back ups | Muestra los realizados | Pend. | |
| 11.4 | Movimientos de auditoría | Se registran las acciones realizadas | Pend. | |

## 12. Pruebas generales

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|----|------|--------------------|-----------|---------------|
| 12.1 | La solución compila sin errores | 0 errores | Pend. | |
| 12.2 | Base se crea desde cero con los scripts en orden | Sin errores | Pend. | |
| 12.3 | Navegación entre todas las pantallas | Sin cierres inesperados | Pend. | |
| 12.4 | Mensajes de error claros ante datos inválidos | Mensajes entendibles | Pend. | |

---

## Resumen

| Total de casos | OK | Falla | Pend. |
|----------------|----|-------|-------|
| 75 | | | |

## Fallas encontradas

| N° de caso | Descripción | Gravedad | Responsable | Estado |
|------------|-------------|----------|-------------|--------|
| | | | | |
