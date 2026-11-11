# Documentación — Sistema Hierro & Forja

Esta carpeta contiene la documentación de entrega, pruebas y soporte del proyecto.

## Archivos

- [`Manual_de_Usuario.md`](./Manual_de_Usuario.md)  
  Manual operativo completo de la aplicación. Es la fuente para generar el PDF final con capturas.

- [`Guia_Instalacion_y_Ejecucion.md`](./Guia_Instalacion_y_Ejecucion.md)  
  Guía rápida para preparar SQL Server, ejecutar los scripts, configurar la conexión, compilar y ejecutar la aplicación.

- [`Credenciales_Prueba.md`](./Credenciales_Prueba.md)  
  Usuarios y contraseñas del escenario demo.

- [`Checklist_Pruebas_Finales.md`](./Checklist_Pruebas_Finales.md)  
  Checklist completo para validar la versión final antes de entrega.

## Importante

Esta carpeta `Documentacion/` pertenece a la documentación del proyecto.

El PDF que se mostrará desde el futuro botón **Ayuda** debe quedar separado, por ejemplo:

```text
Capa_Vistas/
└── Documentacion/
    └── Manual_de_Usuario.pdf
```

La aplicación debe copiar ese PDF al directorio de salida y abrirlo desde allí.

## Orden recomendado antes de entregar

1. Actualizar `desarrollo`.
2. Ejecutar la base desde cero:
   `01 → 02 → 03 → 05 → 04`.
3. Compilar con 0 errores y 0 warnings.
4. Ejecutar `Checklist_Pruebas_Finales.md`.
5. Completar observaciones y fallas.
6. Generar el PDF final del Manual de Usuario.
7. Incorporar el PDF a `Capa_Vistas/Documentacion/`.
