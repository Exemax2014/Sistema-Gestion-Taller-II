using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // ============================================================
    // Clase: ClienteInfo
    //
    // Representa los datos de un cliente que necesita Capa_Vistas
    // para mostrarlo en los buscadores (por ejemplo, en Ventas).
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

            // Crear la conexión utilizando la configuración
            // centralizada de Capa_Datos.
            using SqlConnection conexion = Conexion.CrearConexion();

            // Indicar el nombre del procedimiento almacenado
            // que se ejecutará en SQL Server.
            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Cliente_Buscar",
                conexion
            );

            // Informar que el comando corresponde a un
            // procedimiento almacenado y no a una consulta SQL directa.
            comando.CommandType = CommandType.StoredProcedure;

            // Enviar el texto de búsqueda como parámetro.
            comando.Parameters.Add(
                "@texto",
                SqlDbType.NVarChar,
                100
            ).Value = texto.Trim();

            // Abrir la conexión con SQL Server.
            conexion.Open();

            // Ejecutar el procedimiento y recorrer todos los
            // clientes recibidos.
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
    }
}
