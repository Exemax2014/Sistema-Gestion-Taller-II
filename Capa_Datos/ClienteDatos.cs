using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // ============================================================
    // Clase: ClienteInfo
    //
    // Representa los datos de un cliente que necesita Capa_Vistas
    // para mostrarlo en los buscadores (por ejemplo, en Ventas)
    // y en la grilla del módulo de Clientes.
    // ============================================================
    public class ClienteInfo
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }

    // ============================================================
    // Clase: ClienteDatos
    //
    // Contiene las operaciones de acceso a datos relacionadas
    // con los clientes.
    //
    // Esta clase pertenece exclusivamente a Capa_Datos.
    // ============================================================
    public class ClienteDatos
    {
        // ========================================================
        // Método: Buscar
        //
        // Busca clientes mediante el procedimiento almacenado:
        // dbo.sp_Cliente_Buscar
        //
        // Devuelve una lista de clientes activos que coinciden
        // parcialmente con el texto recibido.
        // ========================================================
        public List<ClienteInfo> Buscar(string texto)
        {
            List<ClienteInfo> clientes = new List<ClienteInfo>();

            using SqlConnection conexion = Conexion.CrearConexion();

            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Cliente_Buscar",
                conexion
            );

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@texto",
                SqlDbType.NVarChar,
                100
            ).Value = texto.Trim();

            conexion.Open();

            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                clientes.Add(new ClienteInfo
                {
                    IdCliente = Convert.ToInt32(lector["id_cliente"]),
                    Nombre = lector["nombre"].ToString() ?? string.Empty,
                    Apellido = lector["apellido"].ToString() ?? string.Empty,
                    Documento = lector["documento"].ToString() ?? string.Empty,
                    Correo = lector["correo"].ToString() ?? string.Empty,
                    Telefono = lector["telefono"].ToString() ?? string.Empty
                });
            }

            return clientes;
        }

        // ========================================================
        // Método: ObtenerActivos
        //
        // Trae todos los clientes activos mediante:
        // dbo.sp_Cliente_Listar
        //
        // Pensado para poblar la grilla del módulo de Clientes.
        // ========================================================
        public List<ClienteInfo> ObtenerActivos()
        {
            List<ClienteInfo> clientes = new List<ClienteInfo>();

            using SqlConnection conexion = Conexion.CrearConexion();

            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Cliente_Listar",
                conexion
            );

            comando.CommandType = CommandType.StoredProcedure;

            conexion.Open();

            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                clientes.Add(new ClienteInfo
                {
                    IdCliente = Convert.ToInt32(lector["id_cliente"]),
                    Nombre = lector["nombre"].ToString() ?? string.Empty,
                    Apellido = lector["apellido"].ToString() ?? string.Empty,
                    Documento = lector["documento"].ToString() ?? string.Empty,
                    Correo = lector["correo"].ToString() ?? string.Empty,
                    Telefono = lector["telefono"].ToString() ?? string.Empty
                });
            }

            return clientes;
        }

        // ========================================================
        // Método: Alta
        //
        // Inserta un nuevo cliente mediante:
        // dbo.sp_Cliente_Alta
        //
        // Devuelve el id_cliente generado por SQL Server.
        // ========================================================
        public int Alta(
            string nombre,
            string apellido,
            string documento,
            string? correo,
            string? telefono)
        {
            using SqlConnection conexion = Conexion.CrearConexion();

            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Cliente_Alta",
                conexion
            );

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
            comando.Parameters.Add("@apellido", SqlDbType.NVarChar, 100).Value = apellido;
            comando.Parameters.Add("@documento", SqlDbType.NVarChar, 20).Value = documento;
            comando.Parameters.Add("@correo", SqlDbType.NVarChar, 150).Value =
                (object?)correo ?? DBNull.Value;
            comando.Parameters.Add("@telefono", SqlDbType.NVarChar, 30).Value =
                (object?)telefono ?? DBNull.Value;

            conexion.Open();

            object? resultado = comando.ExecuteScalar();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }

        // ========================================================
        // Método: Baja
        //
        // Baja lógica de un cliente mediante:
        // dbo.sp_Cliente_Baja
        // ========================================================
        public void Baja(int idCliente)
        {
            using SqlConnection conexion = Conexion.CrearConexion();

            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Cliente_Baja",
                conexion
            );

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@id_cliente", SqlDbType.Int).Value = idCliente;

            conexion.Open();
            comando.ExecuteNonQuery();
        }
    }
}