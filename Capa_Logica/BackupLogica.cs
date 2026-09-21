using Capa_Datos;

namespace Capa_Logica
{
    public sealed class BackupDestino
    {
        public string NombreBaseDatos { get; init; } = string.Empty;
        public string DirectorioServidor { get; init; } = string.Empty;
    }

    public sealed class BackupResultado
    {
        public string NombreArchivo { get; init; } = string.Empty;
        public string DirectorioServidor { get; init; } = string.Empty;
    }

    // Coordina la consulta de destino y la generación del backup tras validar el permiso vigente.
    public class BackupLogica
    {
        private readonly BackupDatos backupDatos = new();

        // Expone el permiso único para que la Vista aplique el estado y vuelva a validar la entrada.
        public bool PuedeRealizarBackup() => SesionActual.TienePermiso("BACKUP_REALIZAR");

        // Obtiene nombre y ruta del servidor únicamente después de validar autorización.
        public BackupDestino ObtenerDestino()
        {
            ValidarPermiso();
            BackupDestinoDatos destino = backupDatos.ObtenerDestino();
            return new BackupDestino
            {
                NombreBaseDatos = destino.NombreBaseDatos,
                DirectorioServidor = destino.DirectorioServidor
            };
        }

        // La ruta no se recibe desde Vista: Datos vuelve a consultar el destino configurado en SQL Server.
        public BackupResultado GenerarBackup()
        {
            ValidarPermiso();
            try
            {
                BackupResultadoDatos resultado = backupDatos.Generar(SesionActual.IdUsuario);
                return new BackupResultado
                {
                    NombreArchivo = resultado.NombreArchivo,
                    DirectorioServidor = resultado.DirectorioServidor
                };
            }
            catch (BackupAuditoriaException ex)
            {
                throw new InvalidOperationException(
                    $"{ex.Message} Contactá a soporte antes de repetir la operación. Detalle: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }

        private static void ValidarPermiso()
        {
            if (!SesionActual.TienePermiso("BACKUP_REALIZAR"))
                throw new InvalidOperationException("No tenés permisos para realizar copias de seguridad.");
        }
    }
}
