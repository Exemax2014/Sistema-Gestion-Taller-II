using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // ============================================================
    // Modelos utilizados por DireccionDatos.
    // ============================================================

    public class ProvinciaInfo
    {
        public int IdProvincia { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class LocalidadInfo
    {
        public int IdLocalidad { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
    }

    public class ResultadoDireccionDatos
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdGenerado { get; set; }
        public bool Exitoso => Codigo == 0;
    }

    // ============================================================
    // Clase: DireccionDatos
    //
    // Ejecuta los procedimientos almacenados de provincias,
    // localidades y direcciones.
    // ============================================================

    public class DireccionDatos
    {
        public List<ProvinciaInfo> ObtenerProvincias()
        {
            List<ProvinciaInfo> provincias = new List<ProvinciaInfo>();

            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Provincia_Listar", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                provincias.Add(new ProvinciaInfo
                {
                    IdProvincia = Convert.ToInt32(lector["id_provincia"]),
                    Nombre = LeerTexto(lector, "nombre")
                });
            }

            return provincias;
        }

        public List<LocalidadInfo> ObtenerLocalidadesPorProvincia(int idProvincia)
        {
            List<LocalidadInfo> localidades = new List<LocalidadInfo>();

            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Localidad_ListarPorProvincia", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@idProvincia", SqlDbType.Int).Value = idProvincia;

            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                localidades.Add(new LocalidadInfo
                {
                    IdLocalidad = Convert.ToInt32(lector["id_localidad"]),
                    Nombre = LeerTexto(lector, "nombre"),
                    CodigoPostal = LeerTexto(lector, "codigo_postal")
                });
            }

            return localidades;
        }

        // Resuelve la localidad por provincia y nombre mediante el procedimiento
        // idempotente, que evita duplicados y puede reactivar registros existentes.
        public ResultadoDireccionDatos ObtenerOCrearLocalidad(int idProvincia, string nombre)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Localidad_ObtenerOCrear", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@idProvincia", SqlDbType.Int).Value = idProvincia;
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre.Trim();

            SqlParameter idGenerado = CrearSalida(comando, "@IdGenerado", SqlDbType.Int);
            SqlParameter codigoResultado = CrearSalida(comando, "@CodigoResultado", SqlDbType.Int);
            SqlParameter mensajeResultado = CrearSalida(comando, "@MensajeResultado", SqlDbType.NVarChar, 250);

            conexion.Open();
            comando.ExecuteNonQuery();

            return new ResultadoDireccionDatos
            {
                Codigo = Convert.ToInt32(codigoResultado.Value),
                Mensaje = mensajeResultado.Value?.ToString() ?? string.Empty,
                IdGenerado = idGenerado.Value == DBNull.Value ? 0 : Convert.ToInt32(idGenerado.Value)
            };
        }

        public ResultadoDireccionDatos Alta(int idLocalidad, string calle, string? altura)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Direccion_Alta", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@idLocalidad", SqlDbType.Int).Value = idLocalidad;
            comando.Parameters.Add("@calle", SqlDbType.NVarChar, 150).Value = calle.Trim();
            comando.Parameters.Add("@altura", SqlDbType.NVarChar, 20).Value =
                string.IsNullOrWhiteSpace(altura) ? DBNull.Value : altura.Trim();

            SqlParameter idGenerado = CrearSalida(comando, "@IdGenerado", SqlDbType.Int);
            SqlParameter codigoResultado = CrearSalida(comando, "@CodigoResultado", SqlDbType.Int);
            SqlParameter mensajeResultado = CrearSalida(comando, "@MensajeResultado", SqlDbType.NVarChar, 250);

            conexion.Open();
            comando.ExecuteNonQuery();

            return new ResultadoDireccionDatos
            {
                Codigo = Convert.ToInt32(codigoResultado.Value),
                Mensaje = mensajeResultado.Value?.ToString() ?? string.Empty,
                IdGenerado = idGenerado.Value == DBNull.Value ? 0 : Convert.ToInt32(idGenerado.Value)
            };
        }

        public ResultadoDireccionDatos Modificar(int idDireccion, int idLocalidad, string calle, string? altura)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Direccion_Modificar", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@idDireccion", SqlDbType.Int).Value = idDireccion;
            comando.Parameters.Add("@idLocalidad", SqlDbType.Int).Value = idLocalidad;
            comando.Parameters.Add("@calle", SqlDbType.NVarChar, 150).Value = calle.Trim();
            comando.Parameters.Add("@altura", SqlDbType.NVarChar, 20).Value =
                string.IsNullOrWhiteSpace(altura) ? DBNull.Value : altura.Trim();

            SqlParameter codigoResultado = CrearSalida(comando, "@CodigoResultado", SqlDbType.Int);
            SqlParameter mensajeResultado = CrearSalida(comando, "@MensajeResultado", SqlDbType.NVarChar, 250);

            conexion.Open();
            comando.ExecuteNonQuery();

            return new ResultadoDireccionDatos
            {
                Codigo = Convert.ToInt32(codigoResultado.Value),
                Mensaje = mensajeResultado.Value?.ToString() ?? string.Empty
            };
        }

        private static SqlParameter CrearSalida(SqlCommand comando, string nombre, SqlDbType tipo, int tamano = 0)
        {
            SqlParameter parametro = tamano > 0
                ? comando.Parameters.Add(nombre, tipo, tamano)
                : comando.Parameters.Add(nombre, tipo);

            parametro.Direction = ParameterDirection.Output;
            return parametro;
        }

        private static string LeerTexto(SqlDataReader lector, string columna)
        {
            if (lector[columna] == DBNull.Value)
            {
                return string.Empty;
            }

            return lector[columna].ToString() ?? string.Empty;
        }
    }
}
