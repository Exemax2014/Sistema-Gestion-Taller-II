using Microsoft.Data.SqlClient;
using System.Data;

namespace Capa_Datos
{
    public class AuditoriaDatosModelo
    {
        public DateTime Fecha { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string Entidad { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public string Sucursal { get; set; } = string.Empty;
    }

    // Accede al historial persistente sin depender de formularios ni de la sesión de la aplicación.
    public class AuditoriaDatos
    {
        // Registra una acción administrativa ya confirmada por la capa de negocio.
        public void Registrar(int idUsuario, string accion, string entidad, int? idEntidad, string? detalle, int? idSucursal)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new("dbo.sp_Auditoria_Registrar", conexion) { CommandType = CommandType.StoredProcedure };
            comando.Parameters.AddWithValue("@idUsuario", idUsuario);
            comando.Parameters.AddWithValue("@accion", accion);
            comando.Parameters.AddWithValue("@entidad", entidad);
            comando.Parameters.AddWithValue("@idEntidad", (object?)idEntidad ?? DBNull.Value);
            comando.Parameters.AddWithValue("@detalle", (object?)detalle ?? DBNull.Value);
            comando.Parameters.AddWithValue("@idSucursal", (object?)idSucursal ?? DBNull.Value);
            conexion.Open();
            comando.ExecuteNonQuery();
        }

        // Devuelve actividad ya filtrada por el procedimiento autorizado para el alcance del solicitante.
        public List<AuditoriaDatosModelo> Listar(int idUsuarioSolicitante, DateTime desde, DateTime hasta, int? idSucursal, int? idUsuario)
        {
            List<AuditoriaDatosModelo> resultado = new();
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new("dbo.sp_Auditoria_Listar", conexion) { CommandType = CommandType.StoredProcedure };
            comando.Parameters.AddWithValue("@idUsuarioSolicitante", idUsuarioSolicitante);
            comando.Parameters.AddWithValue("@desde", desde.Date);
            comando.Parameters.AddWithValue("@hasta", hasta.Date);
            comando.Parameters.AddWithValue("@idSucursal", (object?)idSucursal ?? DBNull.Value);
            comando.Parameters.AddWithValue("@idUsuario", (object?)idUsuario ?? DBNull.Value);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                resultado.Add(new AuditoriaDatosModelo
                {
                    Fecha = lector.GetDateTime(0),
                    Accion = lector.GetString(1),
                    Entidad = lector.GetString(2),
                    Detalle = lector.IsDBNull(3) ? string.Empty : lector.GetString(3),
                    Sucursal = lector.IsDBNull(4) ? "Todas las sucursales" : lector.GetString(4)
                });
            }
            return resultado;
        }
    }
}
