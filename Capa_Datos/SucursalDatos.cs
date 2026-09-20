using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // Representa una sucursal disponible para listados y selectores.
    public class SucursalInfo
    {
        public int IdSucursal { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }

        // Permite mostrar el nombre directamente en controles de selección.
        public override string ToString()
        {
            return Nombre;
        }
    }

    // Agrupa la cantidad de usuarios activos de un perfil en una sucursal.
    public class PerfilSucursalResumenInfo
    {
        public int IdPerfil { get; set; }
        public string Perfil { get; set; } = string.Empty;
        public int CantidadUsuarios { get; set; }
    }

    // Representa una sucursal junto a su ubicación y resumen dinámico de perfiles.
    public class SucursalResumenInfo
    {
        public int IdSucursal { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Calle { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public List<PerfilSucursalResumenInfo> UsuariosPorPerfil { get; } = new();
    }

    // Devuelve el resultado uniforme de las operaciones de gestión de sucursales.
    public class ResultadoSucursalDatos
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdGenerado { get; set; }
        public bool Exitoso => Codigo == 0;
    }

    // Ejecuta los procedimientos almacenados relacionados con sucursales.
    public class SucursalDatos
    {
        // Obtiene únicamente las sucursales activas para contextos operativos.
        public List<SucursalInfo> ObtenerActivas()
        {
            List<SucursalInfo> sucursales = new();

            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Sucursal_Listar", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            conexion.Open();

            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                sucursales.Add(new SucursalInfo
                {
                    IdSucursal = Convert.ToInt32(lector["id_sucursal"]),
                    Nombre = LeerTexto(lector, "nombre"),
                    Telefono = lector["telefono"] == DBNull.Value ? null : lector["telefono"].ToString()
                });
            }

            return sucursales;
        }

        // Obtiene cada sucursal activa y las cantidades de usuarios por perfil desde SQL.
        public List<SucursalResumenInfo> ObtenerResumenUsuariosPorPerfil()
        {
            Dictionary<int, SucursalResumenInfo> sucursales = new();

            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Sucursal_ListarResumenUsuarios", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            conexion.Open();

            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                int idSucursal = Convert.ToInt32(lector["id_sucursal"]);

                if (!sucursales.TryGetValue(idSucursal, out SucursalResumenInfo? sucursal))
                {
                    sucursal = new SucursalResumenInfo
                    {
                        IdSucursal = idSucursal,
                        Nombre = LeerTexto(lector, "nombre_sucursal"),
                        Calle = LeerTexto(lector, "calle"),
                        Localidad = LeerTexto(lector, "localidad"),
                        Provincia = LeerTexto(lector, "provincia")
                    };

                    sucursales.Add(idSucursal, sucursal);
                }

                sucursal.UsuariosPorPerfil.Add(new PerfilSucursalResumenInfo
                {
                    IdPerfil = Convert.ToInt32(lector["id_perfil"]),
                    Perfil = LeerTexto(lector, "perfil"),
                    CantidadUsuarios = Convert.ToInt32(lector["cantidad_usuarios"])
                });
            }

            return sucursales.Values.OrderBy(s => s.Nombre).ToList();
        }

        // Registra una sucursal y su dirección dentro de la misma operación SQL.
        public ResultadoSucursalDatos Alta(string nombre, int idLocalidad, string calle, int idUsuarioEjecutor)
        {
            return EjecutarOperacion("dbo.sp_Sucursal_Alta", comando =>
            {
                comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
                comando.Parameters.Add("@idLocalidad", SqlDbType.Int).Value = idLocalidad;
                comando.Parameters.Add("@calle", SqlDbType.NVarChar, 150).Value = calle;
                comando.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = idUsuarioEjecutor;
            });
        }

        // Actualiza los datos y ubicación de una sucursal existente.
        public ResultadoSucursalDatos Modificar(int idSucursal, string nombre, int idLocalidad, string calle, int idUsuarioEjecutor)
        {
            return EjecutarOperacion("dbo.sp_Sucursal_Modificar", comando =>
            {
                comando.Parameters.Add("@idSucursal", SqlDbType.Int).Value = idSucursal;
                comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
                comando.Parameters.Add("@idLocalidad", SqlDbType.Int).Value = idLocalidad;
                comando.Parameters.Add("@calle", SqlDbType.NVarChar, 150).Value = calle;
                comando.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = idUsuarioEjecutor;
            });
        }

        // Realiza la baja lógica de una sucursal sin eliminar su historial.
        public ResultadoSucursalDatos Baja(int idSucursal, int idUsuarioEjecutor)
        {
            return EjecutarOperacion("dbo.sp_Sucursal_Baja", comando =>
            {
                comando.Parameters.Add("@idSucursal", SqlDbType.Int).Value = idSucursal;
                comando.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = idUsuarioEjecutor;
            });
        }

        // Reactiva una sucursal dada de baja manteniendo sus relaciones existentes.
        public ResultadoSucursalDatos Reactivar(int idSucursal, int idUsuarioEjecutor)
        {
            return EjecutarOperacion("dbo.sp_Sucursal_Reactivar", comando =>
            {
                comando.Parameters.Add("@idSucursal", SqlDbType.Int).Value = idSucursal;
                comando.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = idUsuarioEjecutor;
            });
        }

        // Ejecuta un SP de gestión y materializa su contrato uniforme de salida.
        private static ResultadoSucursalDatos EjecutarOperacion(string procedimiento, Action<SqlCommand> agregarParametros)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand(procedimiento, conexion);
            comando.CommandType = CommandType.StoredProcedure;
            agregarParametros(comando);

            SqlParameter idGenerado = CrearSalida(comando, "@IdGenerado", SqlDbType.Int);
            SqlParameter codigo = CrearSalida(comando, "@CodigoResultado", SqlDbType.Int);
            SqlParameter mensaje = CrearSalida(comando, "@MensajeResultado", SqlDbType.NVarChar, 250);

            conexion.Open();
            comando.ExecuteNonQuery();

            return new ResultadoSucursalDatos
            {
                Codigo = Convert.ToInt32(codigo.Value),
                Mensaje = mensaje.Value?.ToString() ?? string.Empty,
                IdGenerado = idGenerado.Value == DBNull.Value ? 0 : Convert.ToInt32(idGenerado.Value)
            };
        }

        // Crea un parámetro de salida respetando el tamaño requerido por el procedimiento.
        private static SqlParameter CrearSalida(SqlCommand comando, string nombre, SqlDbType tipo, int tamano = 0)
        {
            SqlParameter parametro = tamano > 0
                ? comando.Parameters.Add(nombre, tipo, tamano)
                : comando.Parameters.Add(nombre, tipo);
            parametro.Direction = ParameterDirection.Output;
            return parametro;
        }

        // Convierte valores nulos de SQL en texto seguro para los modelos de Datos.
        private static string LeerTexto(SqlDataReader lector, string columna)
        {
            return lector[columna] == DBNull.Value ? string.Empty : lector[columna].ToString() ?? string.Empty;
        }
    }
}
