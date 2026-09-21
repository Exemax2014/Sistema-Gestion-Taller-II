using Capa_Datos;
using System.Globalization;

namespace Capa_Logica
{
    // Valida autorización y destino, y coordina la generación de una copia de seguridad.
    public class BackupLogica
    {
        private readonly BackupDatos backupDatos = new();

        // Expone el permiso único para que la Vista aplique el estado y vuelva a validar la entrada.
        public bool PuedeRealizarBackup() => SesionActual.TienePermiso("BACKUP_REALIZAR");

        // Lee solamente el nombre del catálogo configurado, tras verificar autorización.
        public string ObtenerNombreBaseDatos()
        {
            if (!PuedeRealizarBackup())
                throw new InvalidOperationException("No tenés permisos para realizar copias de seguridad.");

            return backupDatos.ObtenerNombreBaseDatos();
        }

        // Mantiene el nombre del archivo generado por la aplicación y no acepta nombres de base desde la Vista.
        public string GenerarBackup(string carpetaDestino)
        {
            if (!PuedeRealizarBackup())
                throw new InvalidOperationException("No tenés permisos para realizar copias de seguridad.");

            string carpeta = (carpetaDestino ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(carpeta))
                throw new InvalidOperationException("Seleccioná una carpeta de destino.");
            if (!Directory.Exists(carpeta))
                throw new InvalidOperationException("La carpeta seleccionada no existe o no está disponible.");

            string nombreBase = backupDatos.ObtenerNombreBaseDatos().Trim();
            if (string.IsNullOrWhiteSpace(nombreBase))
                throw new InvalidOperationException("No se pudo obtener el nombre de la base de datos configurada.");

            string nombreArchivo = $"{SanitizarNombreArchivo(nombreBase)}_{DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture)}.bak";
            string rutaCompleta = Path.GetFullPath(Path.Combine(carpeta, nombreArchivo));

            try
            {
                backupDatos.Generar(nombreBase, rutaCompleta, SesionActual.IdUsuario, nombreArchivo);
            }
            catch (BackupAuditoriaException ex)
            {
                throw new InvalidOperationException(
                    $"El archivo {nombreArchivo} se generó, pero no se pudo registrar la acción en Auditoría. Contactá a soporte antes de repetir la operación. Detalle: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "No se pudo generar la copia de seguridad. Verificá que SQL Server tenga acceso a la carpeta seleccionada y permisos para realizar backups. " +
                    $"Detalle: {ex.Message}", ex);
            }

            return nombreArchivo;
        }

        // Convierte el catálogo configurado en un componente válido de nombre de archivo.
        private static string SanitizarNombreArchivo(string nombre)
        {
            HashSet<char> invalidos = Path.GetInvalidFileNameChars().ToHashSet();
            foreach (char caracter in "<>:\"/\\|?*")
                invalidos.Add(caracter);

            string resultado = new(nombre.Select(caracter => invalidos.Contains(caracter) || char.IsControl(caracter) ? '_' : caracter).ToArray());
            resultado = resultado.Trim().TrimEnd('.');
            return string.IsNullOrWhiteSpace(resultado) ? "BaseDatos" : resultado;
        }
    }
}
