using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // ============================================================
    // Clase: ProductoInfo
    //
    // Representa los datos de un producto, junto con su stock
    // en una sucursal puntual, que necesita Capa_Vistas para
    // mostrarlo en los buscadores (por ejemplo, en Ventas).
    // ============================================================
    public class ProductoInfo
    {
        public int IdProducto { get; set; }
        public string CodigoBarra { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
    }

    // ============================================================
    // Clase: ProductoDatos
    //
    // Contiene las operaciones de acceso a datos relacionadas
    // con los productos.
    //
    // Esta clase pertenece exclusivamente a Capa_Datos.
    // ============================================================
    public class ProductoDatos
    {
        // ========================================================
        // Método: Buscar
        //
        // Busca productos mediante el procedimiento almacenado:
        // dbo.sp_Producto_Buscar
        //
        // Devuelve una lista de productos activos que coinciden
        // parcialmente con el texto recibido, junto con su stock
        // en la sucursal indicada.
        // ========================================================
        public List<ProductoInfo> Buscar(string texto, int idSucursal)
        {
            List<ProductoInfo> productos = new List<ProductoInfo>();

            // Crear la conexión utilizando la configuración
            // centralizada de Capa_Datos.
            using SqlConnection conexion = Conexion.CrearConexion();

            // Indicar el nombre del procedimiento almacenado
            // que se ejecutará en SQL Server.
            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Producto_Buscar",
                conexion
            );

            // Informar que el comando corresponde a un
            // procedimiento almacenado y no a una consulta SQL directa.
            comando.CommandType = CommandType.StoredProcedure;

            // Enviar el texto de búsqueda y la sucursal como parámetros.
            comando.Parameters.Add(
                "@texto",
                SqlDbType.NVarChar,
                100
            ).Value = texto.Trim();

            comando.Parameters.Add(
                "@idSucursal",
                SqlDbType.Int
            ).Value = idSucursal;

            // Abrir la conexión con SQL Server.
            conexion.Open();

            // Ejecutar el procedimiento y recorrer todos los
            // productos recibidos.
            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                productos.Add(new ProductoInfo
                {
                    IdProducto = Convert.ToInt32(lector["id_producto"]),
                    CodigoBarra = lector["codigo_barra"].ToString() ?? string.Empty,
                    Nombre = lector["nombre"].ToString() ?? string.Empty,
                    Descripcion = lector["descripcion"].ToString() ?? string.Empty,
                    PrecioVenta = Convert.ToDecimal(lector["precio_venta"]),
                    Stock = Convert.ToInt32(lector["stock"])
                });
            }

            return productos;
        }
    }
}
