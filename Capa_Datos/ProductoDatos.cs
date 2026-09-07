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
    // Clase: ProductoListaInfo
    //
    // Representa un producto tal como se muestra en la grilla
    // del módulo de Productos (sin depender de una sucursal ni
    // de stock).
    // ============================================================
    public class ProductoListaInfo
    {
        public int IdProducto { get; set; }
        public string CodigoBarra { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioCosto { get; set; }
        public decimal PorcentajeGanancia { get; set; }
        public decimal PrecioVenta { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }

    // ============================================================
    // Clase: CategoriaInfo
    //
    // Representa una categoría para poblar combos de selección.
    // ============================================================
    public class CategoriaInfo
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // Se sobrescribe ToString para que el ComboBox muestre
        // directamente el nombre de la categoría.
        public override string ToString() => Nombre;
    }

    // ============================================================
    // Clase: ProductoDatos
    //
    // Contiene las operaciones de acceso a datos relacionadas
    // con los productos y sus categorías.
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

            using SqlConnection conexion = Conexion.CrearConexion();

            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Producto_Buscar",
                conexion
            );

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@texto",
                SqlDbType.NVarChar,
                100
            ).Value = texto.Trim();

            comando.Parameters.Add(
                "@idSucursal",
                SqlDbType.Int
            ).Value = idSucursal;

            conexion.Open();

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

        // ========================================================
        // Método: ObtenerActivos
        //
        // Trae todos los productos activos junto con el nombre
        // de su categoría, mediante dbo.sp_Producto_Listar.
        //
        // Pensado para poblar la grilla del módulo de Productos.
        // ========================================================
        public List<ProductoListaInfo> ObtenerActivos()
        {
            List<ProductoListaInfo> productos = new List<ProductoListaInfo>();

            using SqlConnection conexion = Conexion.CrearConexion();

            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Producto_Listar",
                conexion
            );

            comando.CommandType = CommandType.StoredProcedure;

            conexion.Open();

            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                productos.Add(new ProductoListaInfo
                {
                    IdProducto = Convert.ToInt32(lector["id_producto"]),
                    CodigoBarra = lector["codigo_barra"] == DBNull.Value
                        ? string.Empty
                        : lector["codigo_barra"].ToString() ?? string.Empty,
                    Nombre = lector["nombre"].ToString() ?? string.Empty,
                    Descripcion = lector["descripcion"] == DBNull.Value
                        ? string.Empty
                        : lector["descripcion"].ToString() ?? string.Empty,
                    PrecioCosto = Convert.ToDecimal(lector["precio_costo"]),
                    PorcentajeGanancia = Convert.ToDecimal(lector["porcentaje_ganancia"]),
                    PrecioVenta = Convert.ToDecimal(lector["precio_venta"]),
                    Categoria = lector["categoria"].ToString() ?? string.Empty
                });
            }

            return productos;
        }

        // ========================================================
        // Método: ObtenerCategorias
        //
        // Trae las categorías activas mediante
        // dbo.sp_Categoria_Listar, para poblar el combo del Alta.
        // ========================================================
        public List<CategoriaInfo> ObtenerCategorias()
        {
            List<CategoriaInfo> categorias = new List<CategoriaInfo>();

            using SqlConnection conexion = Conexion.CrearConexion();

            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Categoria_Listar",
                conexion
            );

            comando.CommandType = CommandType.StoredProcedure;

            conexion.Open();

            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                categorias.Add(new CategoriaInfo
                {
                    IdCategoria = Convert.ToInt32(lector["id_categoria"]),
                    Nombre = lector["nombre"].ToString() ?? string.Empty
                });
            }

            return categorias;
        }

        // ========================================================
        // Método: Alta
        //
        // Inserta un nuevo producto mediante dbo.sp_Producto_Alta.
        // Devuelve el id_producto generado por SQL Server.
        // ========================================================
        public int Alta(
            int idCategoria,
            string? codigoBarra,
            string nombre,
            string? descripcion,
            decimal precioCosto,
            decimal porcentajeGanancia)
        {
            using SqlConnection conexion = Conexion.CrearConexion();

            using SqlCommand comando = new SqlCommand(
                "dbo.sp_Producto_Alta",
                conexion
            );

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@id_categoria", SqlDbType.Int).Value = idCategoria;
            comando.Parameters.Add("@codigo_barra", SqlDbType.NVarChar, 50).Value =
                (object?)codigoBarra ?? DBNull.Value;
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
            comando.Parameters.Add("@descripcion", SqlDbType.NVarChar, 250).Value =
                (object?)descripcion ?? DBNull.Value;
            comando.Parameters.Add("@precio_costo", SqlDbType.Decimal).Value = precioCosto;
            comando.Parameters.Add("@porcentaje_ganancia", SqlDbType.Decimal).Value = porcentajeGanancia;

            conexion.Open();

            object? resultado = comando.ExecuteScalar();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }
    }
}