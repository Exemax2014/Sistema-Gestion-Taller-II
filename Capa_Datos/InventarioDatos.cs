using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    public class InventarioInfo
    {
        public int IdInventario { get; set; }

        public int IdProducto { get; set; }

        public int IdSucursal { get; set; }

        public int Stock { get; set; }

        public int StockMinimo { get; set; }
    }


    public class ResultadoInventarioDatos
    {
        public int Codigo { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        public bool Exitoso => Codigo == 0;
    }


    // ============================================================
    // Clase: InventarioDatos
    //
    // Maneja el stock de un producto para una sucursal.
    // ============================================================

    public class InventarioDatos
    {
        public InventarioInfo? ObtenerProductoSucursal(
            int idProducto,
            int idSucursal)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Inventario_ObtenerProductoSucursal",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            comando.Parameters.Add(
                "@idProducto",
                SqlDbType.Int
            ).Value = idProducto;


            comando.Parameters.Add(
                "@idSucursal",
                SqlDbType.Int
            ).Value = idSucursal;


            conexion.Open();


            using SqlDataReader lector =
                comando.ExecuteReader();


            if (!lector.Read())
            {
                return null;
            }


            return new InventarioInfo
            {
                IdInventario =
                    Convert.ToInt32(
                        lector["id_inventario"]
                    ),

                IdProducto =
                    Convert.ToInt32(
                        lector["id_producto"]
                    ),

                IdSucursal =
                    Convert.ToInt32(
                        lector["id_sucursal"]
                    ),

                Stock =
                    Convert.ToInt32(
                        lector["stock"]
                    ),

                StockMinimo =
                    Convert.ToInt32(
                        lector["stock_minimo"]
                    )
            };
        }


        public ResultadoInventarioDatos EstablecerStock(
            int idProducto,
            int idSucursal,
            int stock,
            int stockMinimo)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Inventario_EstablecerStock",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            comando.Parameters.Add(
                "@idProducto",
                SqlDbType.Int
            ).Value = idProducto;


            comando.Parameters.Add(
                "@idSucursal",
                SqlDbType.Int
            ).Value = idSucursal;


            comando.Parameters.Add(
                "@stock",
                SqlDbType.Int
            ).Value = stock;


            comando.Parameters.Add(
                "@stockMinimo",
                SqlDbType.Int
            ).Value = stockMinimo;


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


            return new ResultadoInventarioDatos
            {
                Codigo =
                    Convert.ToInt32(
                        codigoResultado.Value
                    ),

                Mensaje =
                    mensajeResultado.Value
                        ?.ToString()
                    ?? string.Empty
            };
        }
    }
}