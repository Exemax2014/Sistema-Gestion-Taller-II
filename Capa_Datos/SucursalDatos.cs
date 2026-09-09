using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // ============================================================
    // Clase: SucursalInfo
    //
    // Representa una sucursal obtenida desde la base de datos.
    // ============================================================
    public class SucursalInfo
    {
        public int IdSucursal { get; set; }

        public string Nombre { get; set; }
            = string.Empty;

        public string? Telefono { get; set; }


        // ========================================================
        // ToString
        //
        // Permite mostrar directamente el nombre de la sucursal
        // en controles como ComboBox.
        // ========================================================
        public override string ToString()
        {
            return Nombre;
        }
    }


    // ============================================================
    // Clase: SucursalDatos
    //
    // Responsabilidad:
    // Acceder a los datos relacionados con las sucursales.
    //
    // Esta clase pertenece exclusivamente a Capa_Datos.
    // ============================================================
    public class SucursalDatos
    {
        // ========================================================
        // Método: ObtenerActivas
        //
        // Obtiene todas las sucursales activas mediante:
        //
        // dbo.sp_Sucursal_Listar
        // ========================================================
        public List<SucursalInfo> ObtenerActivas()
        {
            List<SucursalInfo> sucursales =
                new List<SucursalInfo>();


            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Sucursal_Listar",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            conexion.Open();


            using SqlDataReader lector =
                comando.ExecuteReader();


            while (lector.Read())
            {
                SucursalInfo sucursal =
                    new SucursalInfo
                    {
                        IdSucursal =
                            Convert.ToInt32(
                                lector["id_sucursal"]
                            ),

                        Nombre =
                            lector["nombre"]
                                .ToString()
                            ?? string.Empty,

                        Telefono =
                            lector["telefono"]
                                == DBNull.Value
                                    ? null
                                    : lector["telefono"]
                                        .ToString()
                    };


                sucursales.Add(
                    sucursal
                );
            }


            return sucursales;
        }
    }
}