using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    public class ReporteResumenDatos { public int Ventas { get; set; } public decimal Subtotal { get; set; } public decimal Descuentos { get; set; } public decimal Recaudacion { get; set; } }
    public class ReporteDiaDatos { public DateTime Fecha { get; set; } public int Ventas { get; set; } public decimal Recaudacion { get; set; } }
    public class ReporteProductoDatos { public string Producto { get; set; } = ""; public string Categoria { get; set; } = ""; public string Marca { get; set; } = ""; public int Unidades { get; set; } public decimal Importe { get; set; } }
    public class ReporteVendedorDatos { public int IdUsuario { get; set; } public string Vendedor { get; set; } = ""; public int Ventas { get; set; } public decimal Total { get; set; } }
    public class ReporteStockDatos { public string Producto { get; set; } = ""; public string Sucursal { get; set; } = ""; public int Stock { get; set; } public int StockMinimo { get; set; } }
    public class ReporteVentaDatos { public int IdVenta { get; set; } public DateTime Fecha { get; set; } public string Cliente { get; set; } = ""; public string Vendedor { get; set; } = ""; public string Sucursal { get; set; } = ""; public decimal Subtotal { get; set; } public decimal Descuento { get; set; } public decimal Total { get; set; } }
    public class ReporteVendedorFiltroDatos { public int IdUsuario { get; set; } public string Nombre { get; set; } = ""; }

    // Ejecuta consultas de Reportes mediante procedimientos almacenados y parámetros tipados.
    public class ReporteDatos
    {
        // Obtiene los importes agregados del período solicitado.
        public ReporteResumenDatos ObtenerResumen(DateTime desde, DateTime hasta, int? sucursal, int? usuario)
        {
            using SqlCommand c = Comando("dbo.sp_Reporte_Resumen", desde, hasta, sucursal, usuario); return Lista(c, r => new ReporteResumenDatos { Ventas=r.GetInt32(0), Subtotal=r.GetDecimal(1), Descuentos=r.GetDecimal(2), Recaudacion=r.GetDecimal(3) }).FirstOrDefault() ?? new();
        }
        // Obtiene la evolución diaria para el gráfico principal.
        public List<ReporteDiaDatos> ObtenerDias(DateTime desde, DateTime hasta, int? sucursal, int? usuario) => Lista(Comando("dbo.sp_Reporte_VentasPorDia", desde, hasta, sucursal, usuario), r => new ReporteDiaDatos { Fecha=r.GetDateTime(0), Ventas=r.GetInt32(1), Recaudacion=r.GetDecimal(2) });
        // Obtiene el ranking de productos del período y alcance indicado.
        public List<ReporteProductoDatos> ObtenerProductos(DateTime desde, DateTime hasta, int? sucursal, int? usuario) => Lista(Comando("dbo.sp_Reporte_ProductosMasVendidos", desde, hasta, sucursal, usuario), r => new ReporteProductoDatos { Producto=r.GetString(2), Categoria=r.GetString(3), Marca=r.IsDBNull(4)?"":r.GetString(4), Unidades=r.GetInt32(5), Importe=r.GetDecimal(6) });
        // Obtiene el rendimiento agregado por vendedor autorizado.
        public List<ReporteVendedorDatos> ObtenerVendedores(DateTime desde, DateTime hasta, int? sucursal, int? usuario) => Lista(Comando("dbo.sp_Reporte_Vendedores", desde, hasta, sucursal, usuario), r => new ReporteVendedorDatos { IdUsuario=r.GetInt32(0), Vendedor=r.GetString(1), Ventas=r.GetInt32(2), Total=r.GetDecimal(3) });
        // Obtiene alertas de stock manteniendo visible la sucursal en alcance global.
        public List<ReporteStockDatos> ObtenerStock(int? sucursal) => Lista(ComandoStock(sucursal), r => new ReporteStockDatos { Producto=r.GetString(0), Sucursal=r.GetString(1), Stock=r.GetInt32(2), StockMinimo=r.GetInt32(3) });
        // Obtiene el detalle de ventas sin cargar información de pagos o ítems innecesaria.
        public List<ReporteVentaDatos> ObtenerDetalle(DateTime desde, DateTime hasta, int? sucursal, int? usuario) => Lista(Comando("dbo.sp_Reporte_DetalleVentas", desde, hasta, sucursal, usuario), r => new ReporteVentaDatos { IdVenta=r.GetInt32(0), Fecha=r.GetDateTime(1), Cliente=r.GetString(2), Vendedor=r.GetString(3), Sucursal=r.GetString(4), Subtotal=r.GetDecimal(5), Descuento=r.GetDecimal(6), Total=r.GetDecimal(7) });
        // Lista vendedores activos para el filtro sin definir perfiles por nombre.
        public List<ReporteVendedorFiltroDatos> ListarVendedores(int? sucursal) => Lista(ComandoVendedores(sucursal), r => new ReporteVendedorFiltroDatos { IdUsuario=r.GetInt32(0), Nombre=r.GetString(1) });
        private static SqlCommand Comando(string sp, DateTime desde, DateTime hasta, int? sucursal, int? usuario) { SqlConnection cn=Conexion.CrearConexion(); SqlCommand c=new(sp,cn){CommandType=CommandType.StoredProcedure}; c.Parameters.Add("@desde",SqlDbType.Date).Value=desde.Date;c.Parameters.Add("@hasta",SqlDbType.Date).Value=hasta.Date;c.Parameters.Add("@idSucursal",SqlDbType.Int).Value=sucursal??(object)DBNull.Value;c.Parameters.Add("@idUsuario",SqlDbType.Int).Value=usuario??(object)DBNull.Value;return c; }
        private static SqlCommand ComandoStock(int? sucursal) { SqlConnection cn=Conexion.CrearConexion(); SqlCommand c=new("dbo.sp_Reporte_StockBajo",cn){CommandType=CommandType.StoredProcedure};c.Parameters.Add("@idSucursal",SqlDbType.Int).Value=sucursal??(object)DBNull.Value;return c; }
        private static SqlCommand ComandoVendedores(int? sucursal) { SqlConnection cn=Conexion.CrearConexion(); SqlCommand c=new("dbo.sp_Reporte_ListarVendedores",cn){CommandType=CommandType.StoredProcedure};c.Parameters.Add("@idSucursal",SqlDbType.Int).Value=sucursal??(object)DBNull.Value;return c; }
        // Abre, ejecuta y mapea lectores homogéneos sin exponer conexiones a Lógica.
        private static List<T> Lista<T>(SqlCommand c, Func<SqlDataReader,T> map) { List<T> l=new(); using(c){c.Connection.Open();using SqlDataReader r=c.ExecuteReader();while(r.Read())l.Add(map(r));}return l; }
    }
}
