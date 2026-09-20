using Capa_Datos;

namespace Capa_Logica
{
    // Centraliza el registro de acciones confirmadas y evita que las vistas escriban auditoría directamente.
    public class AuditoriaLogica
    {
        private readonly AuditoriaDatos auditoriaDatos = new();

        // Registra al usuario de sesión sin aceptar identidades provistas por la Vista.
        public void Registrar(string accion, string entidad, int? idEntidad, string detalle, int? idSucursal = null)
        {
            if (!SesionActual.SesionIniciada)
            {
                return;
            }

            int? sucursalAuditoria = idSucursal
                ?? SesionActual.IdSucursal
                ?? SesionActual.IdSucursalOperativa;

            auditoriaDatos.Registrar(
                SesionActual.IdUsuario,
                accion,
                entidad,
                idEntidad,
                detalle,
                sucursalAuditoria);
        }
    }
}
