using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // ============================================================
    // Clase: UsuarioLoginDatos
    //
    // Representa los datos de un usuario que necesita el sistema
    // durante el proceso de inicio de sesión.
    //
    // Esta clase recibe los valores obtenidos desde SQL Server
    // mediante el procedimiento almacenado de búsqueda de usuario.
    // ============================================================
    public class UsuarioLoginDatos
    {
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int IdSucursal { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string ContrasenaHash { get; set; } = string.Empty;

        public string Perfil { get; set; } = string.Empty;
        public string Sucursal { get; set; } = string.Empty;
    }


    // ============================================================
    // Clase: UsuarioDatos
    //
    // Contiene las operaciones de acceso a datos relacionadas
    // con los usuarios.
    //
    // Esta clase pertenece exclusivamente a Capa_Datos.
    // ============================================================
    public class UsuarioDatos
    {
        // ========================================================
        // Método: BuscarPorNombreUsuario
        //
        // Busca un usuario mediante el procedimiento almacenado:
        // dbo.sp_Usuario_BuscarPorNombreUsuario
        //
        // Devuelve:
        // - UsuarioLoginDatos si encuentra un usuario válido.
        // - null si no existe.
        // ========================================================
        public UsuarioLoginDatos? BuscarPorNombreUsuario(string nombreUsuario)
        {
            // Crear la conexión utilizando la configuración
            // centralizada de Capa_Datos.
            using SqlConnection conexion = Conexion.CrearConexion();

            // Indicar el nombre del procedimiento almacenado
            // que se ejecutará en SQL Server.
            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Usuario_BuscarPorNombreUsuario",
                conexion
            );

            // Informar que el comando corresponde a un
            // procedimiento almacenado y no a una consulta SQL directa.
            comando.CommandType = CommandType.StoredProcedure;

            // Enviar el nombre de usuario como parámetro.
            // Se utiliza el mismo tipo y tamaño definido en SQL Server.
            comando.Parameters.Add(
                "@nombreUsuario",
                SqlDbType.NVarChar,
                100
            ).Value = nombreUsuario.Trim();

            // Abrir la conexión con SQL Server.
            conexion.Open();

            // Ejecutar el procedimiento y obtener el resultado.
            using SqlDataReader lector = comando.ExecuteReader();

            // Si el procedimiento no devolvió ninguna fila,
            // el usuario no existe o no se encuentra activo.
            if (!lector.Read())
            {
                return null;
            }

            // Convertir la fila recibida desde SQL Server
            // en un objeto que pueda utilizar posteriormente Capa_Logica.
            return new UsuarioLoginDatos
            {
                IdUsuario = Convert.ToInt32(lector["id_usuario"]),
                IdPerfil = Convert.ToInt32(lector["id_perfil"]),
                IdSucursal = Convert.ToInt32(lector["id_sucursal"]),

                Nombre = lector["nombre"].ToString() ?? string.Empty,
                Apellido = lector["apellido"].ToString() ?? string.Empty,
                NombreUsuario = lector["nombre_usuario"].ToString() ?? string.Empty,
                ContrasenaHash = lector["contrasena_hash"].ToString() ?? string.Empty,

                Perfil = lector["perfil"].ToString() ?? string.Empty,
                Sucursal = lector["sucursal"].ToString() ?? string.Empty
            };
        }
    }
}