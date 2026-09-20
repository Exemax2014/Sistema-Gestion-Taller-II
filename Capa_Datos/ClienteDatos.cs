using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // ============================================================
    // Modelos utilizados por ClienteDatos.
    // ============================================================

    public class ClienteBusquedaInfo
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Calle { get; set; } = string.Empty;
        public string Altura { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class ClienteListaInfo
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int? IdDireccion { get; set; }
        public string Localidad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Calle { get; set; } = string.Empty;
        public string Altura { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class ClienteDetalleInfo
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int? IdDireccion { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Altura { get; set; } = string.Empty;
        public int? IdLocalidad { get; set; }
        public string Localidad { get; set; } = string.Empty;
        public int? IdProvincia { get; set; }
        public string Provincia { get; set; } = string.Empty;
    }

    public class ResultadoClienteDatos
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdGenerado { get; set; }
        public bool Exitoso => Codigo == 0;
    }

    // ============================================================
    // Clase: ClienteDatos
    //
    // Ejecuta los procedimientos almacenados de clientes.
    //
    // El parámetro "estado" acepta "ACTIVOS", "BAJA" o "TODOS",
    // y se envía tal cual a los procedimientos correspondientes.
    // ============================================================

    public class ClienteDatos
    {
        // Búsqueda rápida (usada por el buscador del listado).
        public List<ClienteBusquedaInfo> Buscar(string texto, string estado = "ACTIVOS")
        {
            List<ClienteBusquedaInfo> clientes = new List<ClienteBusquedaInfo>();

            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Cliente_Buscar", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@texto", SqlDbType.NVarChar, 100).Value = texto.Trim();
            comando.Parameters.Add("@estado", SqlDbType.NVarChar, 10).Value = estado;

            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                clientes.Add(new ClienteBusquedaInfo
                {
                    IdCliente = Convert.ToInt32(lector["id_cliente"]),
                    Nombre = LeerTexto(lector, "nombre"),
                    Apellido = LeerTexto(lector, "apellido"),
                    Documento = LeerTexto(lector, "documento"),
                    Correo = LeerTexto(lector, "correo"),
                    Telefono = LeerTexto(lector, "telefono"),
                    Localidad = LeerTexto(lector, "localidad"),
                    Provincia = LeerTexto(lector, "provincia"),
                    Calle = LeerTexto(lector, "calle"),
                    Altura = LeerTexto(lector, "altura"),
                    Activo = Convert.ToBoolean(lector["activo"])
                });
            }

            return clientes;
        }

        // Trae el listado no eliminado (o de baja, o todos), con dirección.
        public List<ClienteListaInfo> ObtenerTodos(string estado = "ACTIVOS")
        {
            List<ClienteListaInfo> clientes = new List<ClienteListaInfo>();

            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Cliente_Listar", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@estado", SqlDbType.NVarChar, 10).Value = estado;

            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                clientes.Add(new ClienteListaInfo
                {
                    IdCliente = Convert.ToInt32(lector["id_cliente"]),
                    Nombre = LeerTexto(lector, "nombre"),
                    Apellido = LeerTexto(lector, "apellido"),
                    Documento = LeerTexto(lector, "documento"),
                    Correo = LeerTexto(lector, "correo"),
                    Telefono = LeerTexto(lector, "telefono"),
                    IdDireccion = lector["id_direccion"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(lector["id_direccion"]),
                    Localidad = LeerTexto(lector, "localidad"),
                    Provincia = LeerTexto(lector, "provincia"),
                    Calle = LeerTexto(lector, "calle"),
                    Altura = LeerTexto(lector, "altura"),
                    Activo = Convert.ToBoolean(lector["activo"])
                });
            }

            return clientes;
        }

        // Trae el detalle completo de un cliente, incluida su dirección.
        public ClienteDetalleInfo? ObtenerPorId(int idCliente)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Cliente_ObtenerPorId", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@idCliente", SqlDbType.Int).Value = idCliente;

            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();

            if (!lector.Read())
            {
                return null;
            }

            return new ClienteDetalleInfo
            {
                IdCliente = Convert.ToInt32(lector["id_cliente"]),
                Nombre = LeerTexto(lector, "nombre"),
                Apellido = LeerTexto(lector, "apellido"),
                Documento = LeerTexto(lector, "documento"),
                Correo = LeerTexto(lector, "correo"),
                Telefono = LeerTexto(lector, "telefono"),
                IdDireccion = lector["id_direccion"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(lector["id_direccion"]),
                Calle = LeerTexto(lector, "calle"),
                Altura = LeerTexto(lector, "altura"),
                IdLocalidad = lector["id_localidad"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(lector["id_localidad"]),
                Localidad = LeerTexto(lector, "localidad"),
                IdProvincia = lector["id_provincia"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(lector["id_provincia"]),
                Provincia = LeerTexto(lector, "provincia")
            };
        }

        public ResultadoClienteDatos Alta(
            string nombre, string apellido, string documento,
            string? correo, string? telefono, int? idDireccion,
            int idUsuarioEjecutor, int? idSucursalAuditoria)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Cliente_Alta", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            CargarParametros(comando, nombre, apellido, documento, correo, telefono, idDireccion);
            comando.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = idUsuarioEjecutor;
            comando.Parameters.Add("@idSucursalAuditoria", SqlDbType.Int).Value = (object?)idSucursalAuditoria ?? DBNull.Value;

            SqlParameter idGenerado = CrearSalida(comando, "@IdGenerado", SqlDbType.Int);
            SqlParameter codigoResultado = CrearSalida(comando, "@CodigoResultado", SqlDbType.Int);
            SqlParameter mensajeResultado = CrearSalida(comando, "@MensajeResultado", SqlDbType.NVarChar, 250);

            conexion.Open();
            comando.ExecuteNonQuery();

            return new ResultadoClienteDatos
            {
                Codigo = Convert.ToInt32(codigoResultado.Value),
                Mensaje = mensajeResultado.Value?.ToString() ?? string.Empty,
                IdGenerado = idGenerado.Value == DBNull.Value ? 0 : Convert.ToInt32(idGenerado.Value)
            };
        }

        public ResultadoClienteDatos Modificar(
            int idCliente, string nombre, string apellido, string documento,
            string? correo, string? telefono, int? idDireccion,
            int idUsuarioEjecutor, int? idSucursalAuditoria)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Cliente_Modificar", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@idCliente", SqlDbType.Int).Value = idCliente;
            CargarParametros(comando, nombre, apellido, documento, correo, telefono, idDireccion);
            comando.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = idUsuarioEjecutor;
            comando.Parameters.Add("@idSucursalAuditoria", SqlDbType.Int).Value = (object?)idSucursalAuditoria ?? DBNull.Value;

            SqlParameter codigoResultado = CrearSalida(comando, "@CodigoResultado", SqlDbType.Int);
            SqlParameter mensajeResultado = CrearSalida(comando, "@MensajeResultado", SqlDbType.NVarChar, 250);

            conexion.Open();
            comando.ExecuteNonQuery();

            return new ResultadoClienteDatos
            {
                Codigo = Convert.ToInt32(codigoResultado.Value),
                Mensaje = mensajeResultado.Value?.ToString() ?? string.Empty
            };
        }

        public ResultadoClienteDatos Baja(int idCliente, int idUsuarioEjecutor, int? idSucursalAuditoria)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Cliente_Baja", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@id_cliente", SqlDbType.Int).Value = idCliente;
            comando.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = idUsuarioEjecutor;
            comando.Parameters.Add("@idSucursalAuditoria", SqlDbType.Int).Value = (object?)idSucursalAuditoria ?? DBNull.Value;

            SqlParameter codigoResultado = CrearSalida(comando, "@CodigoResultado", SqlDbType.Int);
            SqlParameter mensajeResultado = CrearSalida(comando, "@MensajeResultado", SqlDbType.NVarChar, 250);

            conexion.Open();
            comando.ExecuteNonQuery();

            return new ResultadoClienteDatos
            {
                Codigo = Convert.ToInt32(codigoResultado.Value),
                Mensaje = mensajeResultado.Value?.ToString() ?? string.Empty
            };
        }

        // Reactiva un cliente que estaba dado de baja.
        public ResultadoClienteDatos Reactivar(int idCliente, int idUsuarioEjecutor, int? idSucursalAuditoria)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = new SqlCommand("dbo.sp_Cliente_Reactivar", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@id_cliente", SqlDbType.Int).Value = idCliente;
            comando.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = idUsuarioEjecutor;
            comando.Parameters.Add("@idSucursalAuditoria", SqlDbType.Int).Value = (object?)idSucursalAuditoria ?? DBNull.Value;

            SqlParameter codigoResultado = CrearSalida(comando, "@CodigoResultado", SqlDbType.Int);
            SqlParameter mensajeResultado = CrearSalida(comando, "@MensajeResultado", SqlDbType.NVarChar, 250);

            conexion.Open();
            comando.ExecuteNonQuery();

            return new ResultadoClienteDatos
            {
                Codigo = Convert.ToInt32(codigoResultado.Value),
                Mensaje = mensajeResultado.Value?.ToString() ?? string.Empty
            };
        }

        private static void CargarParametros(
            SqlCommand comando, string nombre, string apellido, string documento,
            string? correo, string? telefono, int? idDireccion)
        {
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre.Trim();
            comando.Parameters.Add("@apellido", SqlDbType.NVarChar, 100).Value = apellido.Trim();
            comando.Parameters.Add("@documento", SqlDbType.NVarChar, 20).Value = documento.Trim();
            comando.Parameters.Add("@correo", SqlDbType.NVarChar, 150).Value =
                string.IsNullOrWhiteSpace(correo) ? DBNull.Value : correo.Trim();
            comando.Parameters.Add("@telefono", SqlDbType.NVarChar, 30).Value =
                string.IsNullOrWhiteSpace(telefono) ? DBNull.Value : telefono.Trim();
            comando.Parameters.Add("@idDireccion", SqlDbType.Int).Value =
                (object?)idDireccion ?? DBNull.Value;
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
