# Checklist de Pruebas Finales — Sistema Hierro & Forja

**Materia:** Taller de Programación II  
**Integrantes:** Josias Correa, Exequiel  
**Fecha de ejecución:** ____ / ____ / 2026  
**Versión probada (commit / rama):** ______________________________  
**PC / resolución / escala Windows:** ______________________________  

## Preparación del entorno

1. Actualizar la rama `desarrollo`.
2. Ejecutar en SQL Server:
   `01_Estructura.sql → 02_DatosIniciales.sql → 03_Procedimientos.sql → 05_ResetBasePruebas.sql → 04_DatosPrueba.sql`.
3. Compilar:
   `dotnet build .\Sistema_Hierro_Y_Forja.slnx --no-restore -m:1 -p:UseSharedCompilation=false`
4. Confirmar `0 errores` y `0 warnings`.
5. Verificar `git diff --check`.
6. Utilizar las credenciales de `Documentacion/Credenciales_Prueba.md`.

### Escenario demo esperado

- 18 localidades.
- 180 clientes normales + 1 Consumidor Final.
- 25 productos.
- 3 sucursales.
- 11 usuarios demo.
- 1.000 ventas.
- ventas en todos los últimos 14 días.
- 24 avisos.
- 180 auditorías, incluyendo 15 backups demo.
- 10 combinaciones de stock bajo.

**Convención:** `OK` = funciona · `Falla` = no funciona · `Pend.` = pendiente.

---

## 1. Inicio de sesión y sesión

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 1.1 | Login válido con `admin` | Ingresa y muestra menú completo | Pend. | |
| 1.2 | Login válido con gerente | Menú acorde a permisos | Pend. | |
| 1.3 | Login válido con vendedor | Menú acorde a permisos | Pend. | |
| 1.4 | Clave incorrecta | No ingresa, mensaje claro | Pend. | |
| 1.5 | Usuario inexistente | No ingresa | Pend. | |
| 1.6 | Usuario vacío | Validación | Pend. | |
| 1.7 | Contraseña vacía | Validación | Pend. | |
| 1.8 | Usuario dado de baja | No puede ingresar | Pend. | |
| 1.9 | Enter en Login | Ejecuta intento de ingreso | Pend. | |
| 1.10 | Cerrar sesión | Regresa al Login y limpia sesión | Pend. | |
| 1.11 | Cerrar aplicación | Solicita confirmación / cierra correctamente | Pend. | |

---

## 2. Menú, sesión y sucursal operativa

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 2.1 | Administrador | Ve todos los módulos autorizados | Pend. | |
| 2.2 | Gerente A | Solo ve funciones permitidas | Pend. | |
| 2.3 | Vendedor A1 | Solo ve funciones permitidas | Pend. | |
| 2.4 | Usuario global | Puede ver `Todas las sucursales` | Pend. | |
| 2.5 | Usuario con sucursal fija | No cambia a sucursales no autorizadas | Pend. | |
| 2.6 | Cambiar sucursal operativa | Inicio y consultas reflejan el contexto | Pend. | |
| 2.7 | Intentar vender con `Todas las sucursales` | Solicita elegir sucursal concreta | Pend. | |
| 2.8 | Nombre/perfil largo | No se corta de forma ilegible | Pend. | |

---

## 3. Inicio / Dashboard

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 3.1 | Abrir Inicio como admin | Carga sin errores | Pend. | |
| 3.2 | Tarjeta Ventas de hoy | Valor > 0 con datos demo | Pend. | |
| 3.3 | Tarjeta Ingresos de hoy | Valor coherente | Pend. | |
| 3.4 | Tarjeta Stock bajo | Refleja alertas demo | Pend. | |
| 3.5 | Tarjeta Productos | Muestra 25 activos | Pend. | |
| 3.6 | Ventas últimos 7 días | Hay barras todos los días | Pend. | |
| 3.7 | Productos más vendidos | Ranking visible y diferenciado | Pend. | |
| 3.8 | Actividad reciente | Carga últimas ventas | Pend. | |
| 3.9 | Avisos | Muestra avisos correspondientes | Pend. | |
| 3.10 | Acceso rápido Nueva venta | Abre Ventas | Pend. | |
| 3.11 | Acceso rápido Productos | Abre Productos | Pend. | |
| 3.12 | Acceso rápido Clientes | Abre Clientes | Pend. | |
| 3.13 | Resolución 1920×1080 | Sin superposición | Pend. | |
| 3.14 | Resolución 1600×900 | Sin superposición / clipping | Pend. | |
| 3.15 | Resolución 1366×768 | Scroll/local layout correcto | Pend. | |
| 3.16 | Escala Windows 100/125% si disponible | UI sigue usable | Pend. | |

---

## 4. Mi perfil

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 4.1 | Abrir Editar perfil | Carga datos propios | Pend. | |
| 4.2 | Editar nombre/apellido | Guarda correctamente | Pend. | |
| 4.3 | DNI no numérico | Rechaza | Pend. | |
| 4.4 | Teléfono válido | Acepta `+`, `-`, espacios, paréntesis | Pend. | |
| 4.5 | Email inválido | Rechaza | Pend. | |
| 4.6 | Fecha futura | Rechaza | Pend. | |
| 4.7 | Cambiar contraseña válida | Guarda y permite login con nueva clave | Pend. | |
| 4.8 | Confirmación distinta | Rechaza | Pend. | |
| 4.9 | Contraseña actual incorrecta | No guarda | Pend. | |

---

## 5. Usuarios

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 5.1 | Listar usuarios | Muestra perfil/sucursal/estado | Pend. | |
| 5.2 | Buscar por usuario | Filtra | Pend. | |
| 5.3 | Buscar por nombre/apellido | Filtra | Pend. | |
| 5.4 | Filtrar por perfil | Filtra | Pend. | |
| 5.5 | Filtrar por estado | Activos/Baja/Todos | Pend. | |
| 5.6 | Alta válida | Guarda | Pend. | |
| 5.7 | DNI duplicado | Rechaza | Pend. | |
| 5.8 | Correo duplicado | Rechaza | Pend. | |
| 5.9 | Usuario con espacios | Rechaza | Pend. | |
| 5.10 | Usuario con `._-` | Acepta | Pend. | |
| 5.11 | Contraseña < 8 caracteres | Rechaza | Pend. | |
| 5.12 | Perfil no global sin sucursal | Rechaza | Pend. | |
| 5.13 | Perfil global | Permite sin sucursal fija | Pend. | |
| 5.14 | Editar usuario | Guarda | Pend. | |
| 5.15 | Dar de baja usuario | No puede ingresar | Pend. | |
| 5.16 | Usuario no puede darse de baja a sí mismo | Acción bloqueada | Pend. | |
| 5.17 | Reactivar usuario si está disponible | Vuelve a quedar activo | Pend. | |
| 5.18 | Ver ventas de usuario | Abre historial correcto | Pend. | |

---

## 6. Tipos de usuario y permisos

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 6.1 | Listar perfiles | Correcto | Pend. | |
| 6.2 | Crear perfil | Guarda | Pend. | |
| 6.3 | Nombre vacío | Rechaza | Pend. | |
| 6.4 | Nombre > 50 | Rechaza | Pend. | |
| 6.5 | Descripción > 200 | Rechaza | Pend. | |
| 6.6 | Modificar permisos | Guarda | Pend. | |
| 6.7 | Asignar dos alcances REPORTES | Rechaza | Pend. | |
| 6.8 | Alcance global en perfil no global | Rechaza | Pend. | |
| 6.9 | Asignar `PERMISOS_GESTIONAR` a perfil común | Rechaza | Pend. | |
| 6.10 | Modificar perfil global | Bloqueado | Pend. | |
| 6.11 | Dar de baja perfil global | Bloqueado | Pend. | |
| 6.12 | Salir con cambios sin guardar | Solicita confirmación | Pend. | |

---

## 7. Sucursales

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 7.1 | Listar sucursales | Muestra 3 del escenario demo | Pend. | |
| 7.2 | Nueva sucursal válida | Guarda | Pend. | |
| 7.3 | Nombre vacío | Rechaza | Pend. | |
| 7.4 | Provincia inválida | Rechaza | Pend. | |
| 7.5 | Localidad inválida | Rechaza | Pend. | |
| 7.6 | Localidad de otra provincia | Rechaza | Pend. | |
| 7.7 | Calle válida | Guarda | Pend. | |
| 7.8 | Altura válida | Guarda | Pend. | |
| 7.9 | Altura `0` | Rechaza | Pend. | |
| 7.10 | Altura `12A` | Rechaza | Pend. | |
| 7.11 | Piso vacío | Acepta | Pend. | |
| 7.12 | Piso 0–99 | Acepta | Pend. | |
| 7.13 | Piso 100 | Rechaza | Pend. | |
| 7.14 | Editar sucursal | Carga y guarda dirección actual | Pend. | |
| 7.15 | Baja sucursal | Se bloquea/avisa según asociaciones | Pend. | |
| 7.16 | Reactivar sucursal | Vuelve a estar disponible | Pend. | |

---

## 8. Clientes y direcciones

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 8.1 | Listado | Muestra 181 clientes demo aproximadamente | Pend. | |
| 8.2 | Buscar apellido | Filtra | Pend. | |
| 8.3 | Buscar DNI | Filtra | Pend. | |
| 8.4 | Filtro Activos/Baja/Todos | Funciona | Pend. | |
| 8.5 | Alta válida | Guarda | Pend. | |
| 8.6 | DNI duplicado | Rechaza | Pend. | |
| 8.7 | Nombre con números | Rechaza | Pend. | |
| 8.8 | Teléfono opcional vacío | Acepta | Pend. | |
| 8.9 | Email opcional vacío | Acepta | Pend. | |
| 8.10 | Email inválido | Rechaza | Pend. | |
| 8.11 | Provincia | Lista catálogo | Pend. | |
| 8.12 | Localidad existente | Se selecciona/reutiliza | Pend. | |
| 8.13 | Localidad nueva | Pide confirmación, crea y selecciona | Pend. | |
| 8.14 | Cambio de provincia | Actualiza localidades | Pend. | |
| 8.15 | Autocomplete calle | Sugiere calles de localidad | Pend. | |
| 8.16 | Calle `Junín` | Acepta | Pend. | |
| 8.17 | Calle `9 de Julio` | Acepta | Pend. | |
| 8.18 | Calle `Av. 3 de Abril` | Acepta | Pend. | |
| 8.19 | Altura `2064` | Acepta | Pend. | |
| 8.20 | Altura `0` | Rechaza | Pend. | |
| 8.21 | Piso vacío | Acepta | Pend. | |
| 8.22 | Piso `15` | Acepta | Pend. | |
| 8.23 | Piso `100` | Rechaza | Pend. | |
| 8.24 | Editar cliente | Conserva/carga dirección | Pend. | |
| 8.25 | Salir editando sin guardar | Pide confirmación | Pend. | |
| 8.26 | Baja cliente | Deja de aparecer activo | Pend. | |
| 8.27 | Reactivar cliente | Vuelve a activo | Pend. | |
| 8.28 | Compras | Abre historial del cliente | Pend. | |
| 8.29 | Consumidor Final | Existe y puede usarse para una venta | Pend. | |

---

## 9. Productos, categorías, marcas e inventario

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 9.1 | Listado productos | 25 productos demo | Pend. | |
| 9.2 | Alta válida | Guarda | Pend. | |
| 9.3 | Código solo dígitos | Acepta | Pend. | |
| 9.4 | Código con letras | Rechaza | Pend. | |
| 9.5 | Código duplicado | Rechaza | Pend. | |
| 9.6 | Nombre vacío | Rechaza | Pend. | |
| 9.7 | Costo negativo | Rechaza | Pend. | |
| 9.8 | Costo con >2 decimales | Rechaza | Pend. | |
| 9.9 | Ganancia 0 | Acepta | Pend. | |
| 9.10 | Ganancia > 999,99 | Rechaza | Pend. | |
| 9.11 | Precio venta calculado | Coherente | Pend. | |
| 9.12 | Categoría filtra marcas | Correcto | Pend. | |
| 9.13 | Cambiar categoría con marca incompatible | Obliga a corregir marca | Pend. | |
| 9.14 | Sin marca | Permitido cuando corresponde | Pend. | |
| 9.15 | Modificar producto | Guarda | Pend. | |
| 9.16 | Baja producto | Queda inactivo | Pend. | |
| 9.17 | Reactivar producto | Vuelve a activo | Pend. | |
| 9.18 | Alta categoría | Guarda | Pend. | |
| 9.19 | Modificar categoría | Guarda | Pend. | |
| 9.20 | Alta marca | Guarda | Pend. | |
| 9.21 | Asociar categorías a marca | Guarda | Pend. | |
| 9.22 | Stock por sucursal | Muestra sucursales autorizadas | Pend. | |
| 9.23 | Modificar stock | Guarda | Pend. | |
| 9.24 | Stock negativo | Rechaza | Pend. | |
| 9.25 | Stock mínimo negativo | Rechaza | Pend. | |
| 9.26 | Estado Stock bajo | Se ve visualmente | Pend. | |

---

## 10. Ventas

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 10.1 | Nueva venta | Abre correctamente | Pend. | |
| 10.2 | Productos por sucursal | Solo vendibles con stock | Pend. | |
| 10.3 | Buscar producto | Filtra | Pend. | |
| 10.4 | Agregar producto | Aparece en carrito | Pend. | |
| 10.5 | Agregar mismo producto | No crea duplicado incorrecto | Pend. | |
| 10.6 | Cantidad 0 | Rechaza | Pend. | |
| 10.7 | Cantidad > stock | Rechaza/avisa | Pend. | |
| 10.8 | Quitar producto | Actualiza total | Pend. | |
| 10.9 | Seleccionar cliente | Correcto | Pend. | |
| 10.10 | Buscar cliente por apellido | Encuentra | Pend. | |
| 10.11 | Buscar cliente por DNI | Encuentra | Pend. | |
| 10.12 | Alta cliente desde venta | Vuelve a la misma venta | Pend. | |
| 10.13 | Seleccionar Consumidor Final | Permite continuar | Pend. | |
| 10.14 | Sin cliente | No confirma | Pend. | |
| 10.15 | Sin productos | No confirma | Pend. | |
| 10.16 | Pago Efectivo | Acepta | Pend. | |
| 10.17 | Pago Débito | Acepta | Pend. | |
| 10.18 | Pago Crédito | Acepta | Pend. | |
| 10.19 | Pago Transferencia | Acepta | Pend. | |
| 10.20 | Pago combinado | Suma correctamente | Pend. | |
| 10.21 | Método duplicado | Rechaza duplicado | Pend. | |
| 10.22 | Pago <= 0 | Rechaza | Pend. | |
| 10.23 | Pago >2 decimales | Rechaza | Pend. | |
| 10.24 | Saldo pendiente | No confirma si no corresponde | Pend. | |
| 10.25 | Resumen muestra nombres de productos | Visible | Pend. | |
| 10.26 | Confirmar venta | Registra todo | Pend. | |
| 10.27 | Stock después de venta | Se descuenta | Pend. | |
| 10.28 | Número venta | Único | Pend. | |
| 10.29 | Salir con venta en curso | Pide confirmación | Pend. | |
| 10.30 | Mis ventas | Lista según alcance | Pend. | |
| 10.31 | Ver detalle | Cabecera, productos, pagos y totales | Pend. | |
| 10.32 | Generar PDF | Crea comprobante | Pend. | |

---

## 11. Reportes

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 11.1 | Abrir Reportes | Muestra secciones permitidas | Pend. | |
| 11.2 | General | Tarjetas coherentes | Pend. | |
| 11.3 | Ventas del período | Gráfico con datos | Pend. | |
| 11.4 | Ingresos por día | Gráfico con datos | Pend. | |
| 11.5 | Productos más vendidos | Ranking diferenciado | Pend. | |
| 11.6 | Por usuario sin selección | Estado vacío claro | Pend. | |
| 11.7 | Por usuario seleccionado | Carga ventas/ingresos/productos/ticket | Pend. | |
| 11.8 | Ticket promedio | Ingresos / ventas | Pend. | |
| 11.9 | Actividad | Carga auditoría | Pend. | |
| 11.10 | Tooltip detalle auditoría | Muestra texto completo | Pend. | |
| 11.11 | Ventas detalladas | Lista y filtros | Pend. | |
| 11.12 | Ver detalle desde reportes | Abre venta | Pend. | |
| 11.13 | Volver desde detalle | Regresa a sección origen | Pend. | |
| 11.14 | Stock bajo | Lista alertas | Pend. | |
| 11.15 | Stock mínimo encabezado | Se lee completo | Pend. | |
| 11.16 | Diferencia encabezado | Se lee completo | Pend. | |
| 11.17 | Gestionar stock | Abre producto/sucursal correcta | Pend. | |
| 11.18 | Desde > Hasta | Rechaza | Pend. | |
| 11.19 | Filtro sucursal global | Filtra | Pend. | |
| 11.20 | Gerente | Solo su sucursal | Pend. | |
| 11.21 | Vendedor | Solo alcance propio | Pend. | |
| 11.22 | Limpiar filtros | Restaura estado | Pend. | |
| 11.23 | Exportar Excel | Genera `.xlsx` real | Pend. | |
| 11.24 | Usuario sin permiso exportar | No tiene acción | Pend. | |

---

## 12. DataGridView y responsive

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 12.1 | Redimensionar columnas manualmente | No permite | Pend. | |
| 12.2 | Redimensionar filas | No permite | Pend. | |
| 12.3 | Encabezados | Se leen completos | Pend. | |
| 12.4 | Columnas Fill | Absorben espacio correctamente | Pend. | |
| 12.5 | Grilla ancha | Scroll horizontal local | Pend. | |
| 12.6 | Botones en grillas | No quedan cortados | Pend. | |
| 12.7 | Colores de estado | Se mantienen | Pend. | |
| 12.8 | Clientes 1366×768 | UI usable | Pend. | |
| 12.9 | Productos 1366×768 | UI usable | Pend. | |
| 12.10 | Usuarios 1366×768 | UI usable | Pend. | |
| 12.11 | Reportes 1366×768 | UI usable | Pend. | |
| 12.12 | Inicio en PC de ambos integrantes | Diseño consistente | Pend. | |

---

## 13. Avisos

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 13.1 | Avisos globales | Visibles según permiso | Pend. | |
| 13.2 | Avisos de sucursal | Solo alcance correspondiente | Pend. | |
| 13.3 | Publicar aviso autorizado | Guarda | Pend. | |
| 13.4 | Usuario sin permiso publicar | No puede publicar | Pend. | |
| 13.5 | Inicio | Muestra avisos recientes | Pend. | |

---

## 14. Back Up y auditoría

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 14.1 | Abrir Back Up con permiso | Correcto | Pend. | |
| 14.2 | Usuario sin `BACKUP_REALIZAR` | Opción no disponible | Pend. | |
| 14.3 | Realizar Back Up | SQL Server genera `.bak` | Pend. | |
| 14.4 | Ruta de backup | Usa carpeta del servidor SQL | Pend. | |
| 14.5 | Historial | Muestra registros de backup | Pend. | |
| 14.6 | Auditoría del backup | Registra acción | Pend. | |
| 14.7 | Fallo de permisos de carpeta | Mensaje entendible | Pend. | |
| 14.8 | Actividad general | Registra acciones relevantes | Pend. | |

---

## 15. Base de datos y scripts

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 15.1 | Ejecutar `01` desde cero | Sin errores | Pend. | |
| 15.2 | Ejecutar `02` | Sin errores | Pend. | |
| 15.3 | Ejecutar `03` | SP actualizados | Pend. | |
| 15.4 | Ejecutar `05` | Limpia demo sin dañar iniciales | Pend. | |
| 15.5 | Ejecutar `04` | Genera escenario completo | Pend. | |
| 15.6 | Repetir `05 → 04` | No duplica datos | Pend. | |
| 15.7 | Localidades demo | 18 | Pend. | |
| 15.8 | Clientes demo | 181 incluyendo Consumidor Final | Pend. | |
| 15.9 | Productos | 25 | Pend. | |
| 15.10 | Ventas demo | 1.000 | Pend. | |
| 15.11 | Vendedores autorizados | Todos tienen ventas | Pend. | |
| 15.12 | Últimos 14 días | Todos tienen actividad | Pend. | |
| 15.13 | Avisos demo | 24 aprox. | Pend. | |
| 15.14 | Auditoría demo | 180 aprox. | Pend. | |
| 15.15 | Backups demo | 15 aprox. | Pend. | |
| 15.16 | Stock bajo | 10 combinaciones aprox. | Pend. | |

---

## 16. Compilación, navegación y estabilidad

| N° | Caso | Resultado esperado | Resultado | Observaciones |
|---|---|---|---|---|
| 16.1 | `dotnet restore` | Correcto | Pend. | |
| 16.2 | Build completo | 0 errores / 0 warnings | Pend. | |
| 16.3 | `git diff --check` | Sin salida | Pend. | |
| 16.4 | Navegar por todos los módulos | Sin cierres inesperados | Pend. | |
| 16.5 | Volver repetidamente | Sin duplicar controles | Pend. | |
| 16.6 | Cambiar tamaño ventana | Sin excepciones | Pend. | |
| 16.7 | Abrir/cerrar detalles repetidos | Sin errores | Pend. | |
| 16.8 | Mensajes de error | Claros y sin stack trace | Pend. | |
| 16.9 | Sesión permanece correcta tras navegación | Sí | Pend. | |

---

## Resumen

| Total de casos | OK | Falla | Pend. |
|---:|---:|---:|---:|
| 182 | | | |

## Fallas encontradas

| N° de caso | Descripción | Gravedad | Responsable | Estado |
|---|---|---|---|---|
| | | | | |

## Criterio de cierre

La versión puede considerarse lista para entrega cuando:

- no existan fallas críticas;
- el build sea `0 errores / 0 warnings`;
- scripts `01 → 02 → 03 → 05 → 04` funcionen;
- ventas, stock y reportes sean consistentes;
- los permisos se respeten;
- la UI sea usable en las PCs de ambos integrantes;
- las fallas menores restantes estén documentadas.
