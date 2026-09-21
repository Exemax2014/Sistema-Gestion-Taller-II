using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // ============================================================
    // MODELOS DE DATOS - USUARIO
    // ============================================================

    public class UsuarioLoginDatos
    {
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int? IdSucursal { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string ContrasenaHash { get; set; } = string.Empty;

        public string Perfil { get; set; } = string.Empty;
        public string Sucursal { get; set; } = string.Empty;
    }


    public class UsuarioListadoDatos
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }

        public int IdPerfil { get; set; }
        public string Perfil { get; set; } = string.Empty;

        public int? IdSucursal { get; set; }
        public string Sucursal { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }


    public class UsuarioDetalleDatos
    {
        public int IdUsuario { get; set; }

        public int IdPerfil { get; set; }
        public int? IdSucursal { get; set; }
        public int? IdDireccion { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }

        public string Perfil { get; set; } = string.Empty;
        public string Sucursal { get; set; } = string.Empty;
    }


    public class UsuarioGuardarDatos
    {
        public int IdPerfil { get; set; }
        public int? IdSucursal { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }

        public int? IdDireccion { get; set; }

        // Solo se utiliza en el alta.
        public string ContrasenaHash { get; set; } = string.Empty;
    }


    public class PerfilDatos
    {
        public int IdPerfil { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool AlcanceGlobal { get; set; }
    }


    public class ResultadoUsuarioDatos
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdGenerado { get; set; }

        public bool Exitoso =>
            Codigo == 0;
    }


    // ============================================================
    // Clase: UsuarioDatos
    //
    // Responsabilidad:
    // Acceso a SQL Server para autenticación y administración
    // de usuarios.
    //
    // No contiene reglas visuales ni muestra mensajes.
    // ============================================================

    public class UsuarioDatos
    {
        // ========================================================
        // AUTENTICACIÓN
        // ========================================================

        public UsuarioLoginDatos? BuscarPorNombreUsuario(
            string nombreUsuario)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Usuario_BuscarPorNombreUsuario",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@nombreUsuario",
                SqlDbType.NVarChar,
                100
            ).Value =
                nombreUsuario.Trim();

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            if (!lector.Read())
            {
                return null;
            }

            return new UsuarioLoginDatos
            {
                IdUsuario =
                    Convert.ToInt32(
                        lector["id_usuario"]
                    ),

                IdPerfil =
                    Convert.ToInt32(
                        lector["id_perfil"]
                    ),

                IdSucursal =
                    lector["id_sucursal"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(
                            lector["id_sucursal"]
                        ),

                Nombre =
                    lector["nombre"].ToString()
                    ?? string.Empty,

                Apellido =
                    lector["apellido"].ToString()
                    ?? string.Empty,

                NombreUsuario =
                    lector["nombre_usuario"].ToString()
                    ?? string.Empty,

                ContrasenaHash =
                    lector["contrasena_hash"].ToString()
                    ?? string.Empty,

                Perfil =
                    lector["perfil"].ToString()
                    ?? string.Empty,

                Sucursal =
                    lector["sucursal"].ToString()
                    ?? string.Empty
            };
        }


        public List<string> ObtenerFuncionalidadesPerfil(
            int idPerfil)
        {
            List<string> funcionalidades =
                new List<string>();

            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Perfil_ObtenerFuncionalidades",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idPerfil",
                SqlDbType.Int
            ).Value =
                idPerfil;

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                string codigo =
                    lector["codigo"].ToString()
                    ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(
                    codigo))
                {
                    funcionalidades.Add(
                        codigo
                    );
                }
            }

            return funcionalidades;
        }


        // ========================================================
        // LISTADO
        // ========================================================

        public List<UsuarioListadoDatos> Listar(
            bool? activo = null)
        {
            List<UsuarioListadoDatos> usuarios =
                new List<UsuarioListadoDatos>();

            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Usuario_Listar",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            SqlParameter parametroActivo =
                comando.Parameters.Add(
                    "@activo",
                    SqlDbType.Bit
                );

            parametroActivo.Value =
                activo.HasValue
                    ? activo.Value
                    : DBNull.Value;

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                usuarios.Add(
                    new UsuarioListadoDatos
                    {
                        IdUsuario =
                            Convert.ToInt32(
                                lector["id_usuario"]
                            ),

                        Nombre =
                            lector["nombre"].ToString()
                            ?? string.Empty,

                        Apellido =
                            lector["apellido"].ToString()
                            ?? string.Empty,

                        Dni =
                            lector["dni"].ToString()
                            ?? string.Empty,

                        Telefono =
                            lector["telefono"].ToString()
                            ?? string.Empty,

                        NombreUsuario =
                            lector["nombre_usuario"].ToString()
                            ?? string.Empty,

                        Correo =
                            lector["correo"].ToString()
                            ?? string.Empty,

                        Sexo =
                            lector["sexo"].ToString()
                            ?? string.Empty,

                        FechaNacimiento =
                            lector["fecha_nacimiento"]
                                == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(
                                        lector["fecha_nacimiento"]
                                    ),

                        IdPerfil =
                            Convert.ToInt32(
                                lector["id_perfil"]
                            ),

                        Perfil =
                            lector["perfil"].ToString()
                            ?? string.Empty,

                        IdSucursal =
                            lector["id_sucursal"]
                                == DBNull.Value
                                    ? null
                                    : Convert.ToInt32(
                                        lector["id_sucursal"]
                                    ),

                        Sucursal =
                            lector["sucursal"].ToString()
                            ?? string.Empty,

                        Activo =
                            Convert.ToBoolean(
                                lector["activo"]
                            )
                    }
                );
            }

            return usuarios;
        }


        // ========================================================
        // DETALLE
        // ========================================================

        public UsuarioDetalleDatos? ObtenerPorId(
            int idUsuario)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Usuario_ObtenerPorId",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idUsuario",
                SqlDbType.Int
            ).Value =
                idUsuario;

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            if (!lector.Read())
            {
                return null;
            }

            return new UsuarioDetalleDatos
            {
                IdUsuario =
                    Convert.ToInt32(
                        lector["id_usuario"]
                    ),

                IdPerfil =
                    Convert.ToInt32(
                        lector["id_perfil"]
                    ),

                IdSucursal =
                    lector["id_sucursal"]
                        == DBNull.Value
                            ? null
                            : Convert.ToInt32(
                                lector["id_sucursal"]
                            ),

                IdDireccion =
                    lector["id_direccion"]
                        == DBNull.Value
                            ? null
                            : Convert.ToInt32(
                                lector["id_direccion"]
                            ),

                Nombre =
                    lector["nombre"].ToString()
                    ?? string.Empty,

                Apellido =
                    lector["apellido"].ToString()
                    ?? string.Empty,

                Dni =
                    lector["dni"].ToString()
                    ?? string.Empty,

                Telefono =
                    lector["telefono"].ToString()
                    ?? string.Empty,

                NombreUsuario =
                    lector["nombre_usuario"].ToString()
                    ?? string.Empty,

                Correo =
                    lector["correo"].ToString()
                    ?? string.Empty,

                Sexo =
                    lector["sexo"].ToString()
                    ?? string.Empty,

                FechaNacimiento =
                    lector["fecha_nacimiento"]
                        == DBNull.Value
                            ? null
                            : Convert.ToDateTime(
                                lector["fecha_nacimiento"]
                            ),

                Perfil =
                    lector["perfil"].ToString()
                    ?? string.Empty,

                Sucursal =
                    lector["sucursal"].ToString()
                    ?? string.Empty
            };
        }


        // ========================================================
        // PERFILES
        // ========================================================

        public List<PerfilDatos> ListarPerfiles()
        {
            List<PerfilDatos> perfiles =
                new List<PerfilDatos>();

            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Perfil_Listar",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                perfiles.Add(
                    new PerfilDatos
                    {
                        IdPerfil =
                            Convert.ToInt32(
                                lector["id_perfil"]
                            ),

                        Nombre =
                            lector["nombre"].ToString()
                            ?? string.Empty,

                        Descripcion =
                            lector["descripcion"].ToString()
                            ?? string.Empty,

                        AlcanceGlobal =
                            Convert.ToBoolean(
                                lector["alcance_global"]
                            )
                    }
                );
            }

            return perfiles;
        }


        // ========================================================
        // ALTA
        // ========================================================

        public ResultadoUsuarioDatos Alta(
            UsuarioGuardarDatos usuario)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Usuario_Alta",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            CargarParametrosUsuario(
                comando,
                usuario,
                incluirContrasena: true
            );

            SqlParameter idGenerado =
                comando.Parameters.Add(
                    "@IdGenerado",
                    SqlDbType.Int
                );

            idGenerado.Direction =
                ParameterDirection.Output;

            SqlParameter codigoResultado =
                comando.Parameters.Add(
                    "@CodigoResultado",
                    SqlDbType.Int
                );

            codigoResultado.Direction =
                ParameterDirection.Output;

            SqlParameter mensajeResultado =
                comando.Parameters.Add(
                    "@MensajeResultado",
                    SqlDbType.NVarChar,
                    250
                );

            mensajeResultado.Direction =
                ParameterDirection.Output;

            conexion.Open();

            comando.ExecuteNonQuery();

            return new ResultadoUsuarioDatos
            {
                IdGenerado =
                    idGenerado.Value == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            idGenerado.Value
                        ),

                Codigo =
                    codigoResultado.Value == DBNull.Value
                        ? 500
                        : Convert.ToInt32(
                            codigoResultado.Value
                        ),

                Mensaje =
                    mensajeResultado.Value?.ToString()
                    ?? string.Empty
            };
        }


        // ========================================================
        // MODIFICACIÓN
        // ========================================================

        public ResultadoUsuarioDatos Modificar(
            int idUsuario,
            UsuarioGuardarDatos usuario)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Usuario_Modificar",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idUsuario",
                SqlDbType.Int
            ).Value =
                idUsuario;

            CargarParametrosUsuario(
                comando,
                usuario,
                incluirContrasena: false
            );

            SqlParameter codigoResultado =
                comando.Parameters.Add(
                    "@CodigoResultado",
                    SqlDbType.Int
                );

            codigoResultado.Direction =
                ParameterDirection.Output;

            SqlParameter mensajeResultado =
                comando.Parameters.Add(
                    "@MensajeResultado",
                    SqlDbType.NVarChar,
                    250
                );

            mensajeResultado.Direction =
                ParameterDirection.Output;

            conexion.Open();

            comando.ExecuteNonQuery();

            return new ResultadoUsuarioDatos
            {
                Codigo =
                    codigoResultado.Value == DBNull.Value
                        ? 500
                        : Convert.ToInt32(
                            codigoResultado.Value
                        ),

                Mensaje =
                    mensajeResultado.Value?.ToString()
                    ?? string.Empty
            };
        }


        // ========================================================
        // BAJA LÓGICA
        // ========================================================

        public ResultadoUsuarioDatos Baja(
            int idUsuario)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Usuario_Baja",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idUsuario",
                SqlDbType.Int
            ).Value =
                idUsuario;

            SqlParameter codigoResultado =
                comando.Parameters.Add(
                    "@CodigoResultado",
                    SqlDbType.Int
                );

            codigoResultado.Direction =
                ParameterDirection.Output;

            SqlParameter mensajeResultado =
                comando.Parameters.Add(
                    "@MensajeResultado",
                    SqlDbType.NVarChar,
                    250
                );

            mensajeResultado.Direction =
                ParameterDirection.Output;

            conexion.Open();

            comando.ExecuteNonQuery();

            return new ResultadoUsuarioDatos
            {
                Codigo =
                    codigoResultado.Value == DBNull.Value
                        ? 500
                        : Convert.ToInt32(
                            codigoResultado.Value
                        ),

                Mensaje =
                    mensajeResultado.Value?.ToString()
                    ?? string.Empty
            };
        }


        // ========================================================
        // PARÁMETROS COMPARTIDOS
        // ========================================================

        private static void CargarParametrosUsuario(
            SqlCommand comando,
            UsuarioGuardarDatos usuario,
            bool incluirContrasena)
        {
            comando.Parameters.Add(
                "@idPerfil",
                SqlDbType.Int
            ).Value =
                usuario.IdPerfil;

            comando.Parameters.Add(
                "@idSucursal",
                SqlDbType.Int
            ).Value =
                usuario.IdSucursal.HasValue
                    ? usuario.IdSucursal.Value
                    : DBNull.Value;

            comando.Parameters.Add(
                "@nombre",
                SqlDbType.NVarChar,
                100
            ).Value =
                usuario.Nombre.Trim();

            comando.Parameters.Add(
                "@apellido",
                SqlDbType.NVarChar,
                100
            ).Value =
                usuario.Apellido.Trim();

            comando.Parameters.Add(
                "@dni",
                SqlDbType.NVarChar,
                20
            ).Value =
                usuario.Dni.Trim();

            comando.Parameters.Add(
                "@telefono",
                SqlDbType.NVarChar,
                30
            ).Value =
                string.IsNullOrWhiteSpace(
                    usuario.Telefono)
                        ? DBNull.Value
                        : usuario.Telefono.Trim();

            comando.Parameters.Add(
                "@nombreUsuario",
                SqlDbType.NVarChar,
                50
            ).Value =
                usuario.NombreUsuario.Trim();

            if (incluirContrasena)
            {
                comando.Parameters.Add(
                    "@contrasenaHash",
                    SqlDbType.NVarChar,
                    255
                ).Value =
                    usuario.ContrasenaHash;
            }

            comando.Parameters.Add(
                "@correo",
                SqlDbType.NVarChar,
                150
            ).Value =
                usuario.Correo.Trim();

            comando.Parameters.Add(
                "@sexo",
                SqlDbType.NVarChar,
                20
            ).Value =
                string.IsNullOrWhiteSpace(
                    usuario.Sexo)
                        ? DBNull.Value
                        : usuario.Sexo.Trim();

            comando.Parameters.Add(
                "@fechaNacimiento",
                SqlDbType.Date
            ).Value =
                usuario.FechaNacimiento.HasValue
                    ? usuario.FechaNacimiento.Value.Date
                    : DBNull.Value;

            comando.Parameters.Add(
                "@idDireccion",
                SqlDbType.Int
            ).Value =
                usuario.IdDireccion.HasValue
                    ? usuario.IdDireccion.Value
                    : DBNull.Value;
        }
    }
}
