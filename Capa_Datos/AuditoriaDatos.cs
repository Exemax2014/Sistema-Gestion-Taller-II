using Microsoft.Data.SqlClient;
using System.Data;

namespace Capa_Datos
{
    public class AuditoriaDatosModelo
    {
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; } = string.Empty;
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
            IReadOnlyDictionary<string, int> ordinales = ObtenerOrdinalesContrato(lector, "dbo.sp_Auditoria_Listar");
            while (lector.Read())
            {
                resultado.Add(CrearModelo(lector, ordinales));
            }
            return resultado;
        }

        // Consulta aislada: el procedimiento sólo devuelve eventos de backup y valida su permiso propio.
        public List<AuditoriaDatosModelo> ListarBackups(int idUsuarioSolicitante)
        {
            List<AuditoriaDatosModelo> resultado = new();
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new("dbo.sp_Auditoria_ListarBackups", conexion) { CommandType = CommandType.StoredProcedure };
            comando.Parameters.AddWithValue("@idUsuarioSolicitante", idUsuarioSolicitante);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            IReadOnlyDictionary<string, int> ordinales = ObtenerOrdinalesContrato(lector, "dbo.sp_Auditoria_ListarBackups");
            while (lector.Read())
            {
                resultado.Add(CrearModelo(lector, ordinales));
            }
            return resultado;
        }

        // Los procedimientos fueron ampliados con Usuario. Se valida el esquema
        // antes de leer para diagnosticar una BD que todavía conserva la versión
        // anterior, sin desplazar columnas ni atribuir un usuario inexistente.
        private static IReadOnlyDictionary<string, int> ObtenerOrdinalesContrato(SqlDataReader lector, string procedimiento)
        {
            string[] columnasRequeridas = { "Fecha", "Usuario", "Accion", "Entidad", "Detalle", "Sucursal" };
            Dictionary<string, int> ordinales = new(StringComparer.OrdinalIgnoreCase);

            for (int indice = 0; indice < lector.FieldCount; indice++)
                ordinales[lector.GetName(indice)] = indice;

            string[] faltantes = columnasRequeridas.Where(columna => !ordinales.ContainsKey(columna)).ToArray();
            if (faltantes.Length > 0)
            {
                throw new InvalidOperationException(
                    $"El procedimiento instalado '{procedimiento}' tiene un contrato desactualizado: faltan las columnas {string.Join(", ", faltantes)}. " +
                    "Ejecute BaseDatos/03_Procedimientos.sql en esta base de datos y vuelva a intentar.");
            }

            return ordinales;
        }

        private static AuditoriaDatosModelo CrearModelo(SqlDataReader lector, IReadOnlyDictionary<string, int> ordinales)
        {
            int fecha = ordinales["Fecha"];
            int usuario = ordinales["Usuario"];
            int accion = ordinales["Accion"];
            int entidad = ordinales["Entidad"];
            int detalle = ordinales["Detalle"];
            int sucursal = ordinales["Sucursal"];

            return new AuditoriaDatosModelo
            {
                Fecha = lector.GetDateTime(fecha),
                Usuario = lector.IsDBNull(usuario) ? string.Empty : lector.GetString(usuario),
                Accion = lector.IsDBNull(accion) ? string.Empty : lector.GetString(accion),
                Entidad = lector.IsDBNull(entidad) ? string.Empty : lector.GetString(entidad),
                Detalle = lector.IsDBNull(detalle) ? string.Empty : lector.GetString(detalle),
                Sucursal = lector.IsDBNull(sucursal) ? "Todas las sucursales" : lector.GetString(sucursal)
            };
        }
    }
}
