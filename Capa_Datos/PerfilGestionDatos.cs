using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // ============================================================
    // MODELOS DE DATOS - GESTIÓN DE PERFILES Y PERMISOS
    // ============================================================

    public class PerfilDetalleDatos
    {
        public int IdPerfil { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public bool AlcanceGlobal { get; set; }
        public bool EsAdministrador { get; set; }
    }


    public class FuncionalidadPerfilDatos
    {
        public int IdFuncionalidad { get; set; }

        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public bool Asignada { get; set; }
        public bool Bloqueada { get; set; }
    }


    public class ResultadoPerfilDatos
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdGenerado { get; set; }

        public bool Exitoso =>
            Codigo == 0;
    }


    // ============================================================
    // Clase: PerfilGestionDatos
    //
    // Responsabilidad:
    // Acceso a SQL Server para administrar tipos de usuario,
    // funcionalidades y permisos.
    //
    // Las reglas de negocio se validarán nuevamente en Capa_Logica.
    // No contiene lógica visual ni muestra mensajes.
    // ============================================================

    public class PerfilGestionDatos
    {
        // ========================================================
        // LISTADO DE PERFILES
        // ========================================================

        public List<PerfilDatos> Listar()
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
        // DETALLE DE PERFIL
        // ========================================================

        public PerfilDetalleDatos? ObtenerPorId(
            int idPerfil)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Perfil_ObtenerPorId",
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

            if (!lector.Read())
            {
                return null;
            }

            return new PerfilDetalleDatos
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
                    ),

                EsAdministrador =
                    Convert.ToBoolean(
                        lector["es_administrador"]
                    )
            };
        }


        // ========================================================
        // LISTADO GENERAL DE FUNCIONALIDADES
        // ========================================================

        public List<FuncionalidadPerfilDatos> ListarFuncionalidades()
        {
            List<FuncionalidadPerfilDatos> funcionalidades =
                new List<FuncionalidadPerfilDatos>();

            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Funcionalidad_Listar",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                funcionalidades.Add(
                    new FuncionalidadPerfilDatos
                    {
                        IdFuncionalidad =
                            Convert.ToInt32(
                                lector["id_funcionalidad"]
                            ),

                        Codigo =
                            lector["codigo"].ToString()
                            ?? string.Empty,

                        Nombre =
                            lector["nombre"].ToString()
                            ?? string.Empty,

                        Descripcion =
                            lector["descripcion"].ToString()
                            ?? string.Empty,

                        Asignada = false,
                        Bloqueada = false
                    }
                );
            }

            return funcionalidades;
        }


        // ========================================================
        // FUNCIONALIDADES ASIGNADAS A UN PERFIL
        // ========================================================

        public List<FuncionalidadPerfilDatos>
            ListarFuncionalidadesPorPerfil(
                int idPerfil)
        {
            List<FuncionalidadPerfilDatos> funcionalidades =
                new List<FuncionalidadPerfilDatos>();

            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Perfil_ListarFuncionalidades",
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
                funcionalidades.Add(
                    new FuncionalidadPerfilDatos
                    {
                        IdFuncionalidad =
                            Convert.ToInt32(
                                lector["id_funcionalidad"]
                            ),

                        Codigo =
                            lector["codigo"].ToString()
                            ?? string.Empty,

                        Nombre =
                            lector["nombre"].ToString()
                            ?? string.Empty,

                        Descripcion =
                            lector["descripcion"].ToString()
                            ?? string.Empty,

                        Asignada =
                            Convert.ToBoolean(
                                lector["asignada"]
                            ),

                        Bloqueada =
                            Convert.ToBoolean(
                                lector["bloqueada"]
                            )
                    }
                );
            }

            return funcionalidades;
        }


        // ========================================================
        // ALTA DE PERFIL
        // ========================================================

        public ResultadoPerfilDatos Alta(
            string nombre,
            string descripcion)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Perfil_Alta",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@nombre",
                SqlDbType.NVarChar,
                50
            ).Value =
                nombre.Trim();

            comando.Parameters.Add(
                "@descripcion",
                SqlDbType.NVarChar,
                200
            ).Value =
                string.IsNullOrWhiteSpace(
                    descripcion)
                        ? DBNull.Value
                        : descripcion.Trim();

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

            return new ResultadoPerfilDatos
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
        // MODIFICACIÓN DE PERFIL
        // ========================================================

        public ResultadoPerfilDatos Modificar(
            int idPerfil,
            string nombre,
            string descripcion)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Perfil_Modificar",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idPerfil",
                SqlDbType.Int
            ).Value =
                idPerfil;

            comando.Parameters.Add(
                "@nombre",
                SqlDbType.NVarChar,
                50
            ).Value =
                nombre.Trim();

            comando.Parameters.Add(
                "@descripcion",
                SqlDbType.NVarChar,
                200
            ).Value =
                string.IsNullOrWhiteSpace(
                    descripcion)
                        ? DBNull.Value
                        : descripcion.Trim();

            return EjecutarResultado(
                conexion,
                comando
            );
        }


        // ========================================================
        // ASIGNAR FUNCIONALIDAD
        // ========================================================

        public ResultadoPerfilDatos AsignarFuncionalidad(
            int idPerfil,
            int idFuncionalidad)
        {
            return EjecutarCambioFuncionalidad(
                "dbo.sp_Perfil_AsignarFuncionalidad",
                idPerfil,
                idFuncionalidad
            );
        }


        // ========================================================
        // QUITAR FUNCIONALIDAD
        // ========================================================

        public ResultadoPerfilDatos QuitarFuncionalidad(
            int idPerfil,
            int idFuncionalidad)
        {
            return EjecutarCambioFuncionalidad(
                "dbo.sp_Perfil_QuitarFuncionalidad",
                idPerfil,
                idFuncionalidad
            );
        }


        // ========================================================
        // GUARDAR FUNCIONALIDADES DE FORMA ATÓMICA
        // ========================================================

        public ResultadoPerfilDatos GuardarFuncionalidades(
            int idPerfil,
            IEnumerable<int> idsFuncionalidades)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Perfil_GuardarFuncionalidades",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;


            comando.Parameters.Add(
                "@idPerfil",
                SqlDbType.Int
            ).Value =
                idPerfil;


            string ids =
                string.Join(
                    ",",
                    (
                        idsFuncionalidades
                        ?? Enumerable.Empty<int>()
                    )
                    .Where(
                        id =>
                            id > 0
                    )
                    .Distinct()
                );


            comando.Parameters.Add(
                "@idsFuncionalidades",
                SqlDbType.NVarChar,
                -1
            ).Value =
                ids;


            return EjecutarResultado(
                conexion,
                comando
            );
        }


        // ========================================================
        // BAJA LÓGICA
        // ========================================================

        public ResultadoPerfilDatos Baja(
            int idPerfil)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Perfil_Baja",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idPerfil",
                SqlDbType.Int
            ).Value =
                idPerfil;

            return EjecutarResultado(
                conexion,
                comando
            );
        }


        // ========================================================
        // SINCRONIZAR ADMINISTRADOR
        // ========================================================

        public ResultadoPerfilDatos SincronizarAdministrador()
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Perfil_SincronizarAdministrador",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            return EjecutarResultado(
                conexion,
                comando
            );
        }


        // ========================================================
        // HELPERS
        // ========================================================

        private static ResultadoPerfilDatos
            EjecutarCambioFuncionalidad(
                string procedimiento,
                int idPerfil,
                int idFuncionalidad)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    procedimiento,
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idPerfil",
                SqlDbType.Int
            ).Value =
                idPerfil;

            comando.Parameters.Add(
                "@idFuncionalidad",
                SqlDbType.Int
            ).Value =
                idFuncionalidad;

            return EjecutarResultado(
                conexion,
                comando
            );
        }


        private static ResultadoPerfilDatos EjecutarResultado(
            SqlConnection conexion,
            SqlCommand comando)
        {
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

            return new ResultadoPerfilDatos
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
    }
}
