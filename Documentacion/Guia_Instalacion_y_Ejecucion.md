# Guía de Instalación y Ejecución — Sistema Hierro & Forja

## 1. Requisitos

- Windows 10/11.
- Visual Studio compatible con .NET 10 y Windows Forms.
- .NET 10 SDK.
- Microsoft SQL Server.
- SQL Server Management Studio recomendado.
- Git.

Comprobar .NET:

```powershell
dotnet --version
```

## 2. Clonar el repositorio

```powershell
git clone https://github.com/Exemax2014/Sistema-Gestion-Taller-II.git
cd Sistema-Gestion-Taller-II
git checkout desarrollo
git pull origin desarrollo
```

## 3. Crear la base de datos

Abrir SQL Server Management Studio y ejecutar, en este orden:

```text
BaseDatos/01_Estructura.sql
BaseDatos/02_DatosIniciales.sql
BaseDatos/03_Procedimientos.sql
```

Para cargar el escenario completo de demostración:

```text
BaseDatos/05_ResetBasePruebas.sql
BaseDatos/04_DatosPrueba.sql
```

Orden completo recomendado para una instalación demo limpia:

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

### Estado esperado del escenario demo

- 18 localidades de Corrientes.
- 180 clientes normales.
- 1 cliente genérico `Consumidor Final`.
- 181 clientes demo totales.
- 25 productos.
- 3 sucursales.
- 11 usuarios demo.
- 1.000 ventas distribuidas en los últimos 180 días.
- ventas todos los días de los últimos 14 días.
- pagos simples y combinados.
- 24 avisos.
- 180 movimientos de auditoría.
- 15 backups demo incluidos dentro de auditoría.
- 10 combinaciones producto/sucursal en stock bajo.

## 4. Configurar la conexión

Crear localmente:

```text
Capa_Datos/Configuracion/configuracion.json
```

Ejemplo con autenticación de Windows:

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

Ejemplo con SQL Server Express:

```json
{
  "Servidor": ".\\SQLEXPRESS",
  "BaseDatos": "SistemaGestion",
  "AutenticacionWindows": true,
  "Usuario": null,
  "Contrasena": null,
  "TrustServerCertificate": true
}
```

No subir credenciales SQL reales al repositorio.

## 5. Restaurar paquetes

Desde la raíz del proyecto:

```powershell
dotnet restore .\Sistema_Hierro_Y_Forja.slnx
```

## 6. Compilar

```powershell
dotnet build .\Sistema_Hierro_Y_Forja.slnx --no-restore -m:1 -p:UseSharedCompilation=false
```

Resultado esperado:

```text
0 errores
0 warnings
```

## 7. Ejecutar

```powershell
dotnet run --project .\Capa_Vistas\Capa_Vistas.csproj
```

También puede ejecutarse desde Visual Studio estableciendo `Capa_Vistas` como proyecto de inicio.

## 8. Credenciales

Ver:

```text
Documentacion/Credenciales_Prueba.md
```

## 9. Recargar solamente datos demo

Si la estructura y procedimientos ya están actualizados:

```text
05_ResetBasePruebas.sql
04_DatosPrueba.sql
```

## 10. Actualizar procedimientos

Si el código informa un error del tipo:

```text
Could not find stored procedure ...
```

ejecutar nuevamente:

```text
03_Procedimientos.sql
```

## 11. Backup

El módulo Back Up genera un `.bak` real desde SQL Server.

La carpeta de destino es la configurada/obtenida por el servidor SQL, no una carpeta elegida desde WinForms.

La cuenta del servicio de SQL Server debe tener permiso de escritura.

## 12. Verificación final

```powershell
git status
git diff --check
```

y completar:

```text
Documentacion/Checklist_Pruebas_Finales.md
```
