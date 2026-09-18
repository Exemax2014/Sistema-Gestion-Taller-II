using Capa_Datos;

namespace Capa_Logica
{
    public class DashboardResumenModelo
    {
        public int VentasHoy { get; set; }
        public decimal IngresosHoy { get; set; }
        public int StockBajo { get; set; }
        public int ProductosActivos { get; set; }
    }

    public class DashboardSerieModelo { public DateTime Fecha { get; set; } public int Ventas { get; set; } public decimal Ingresos { get; set; } }
    public class DashboardProductoModelo { public string Producto { get; set; } = string.Empty; public int UnidadesVendidas { get; set; } }
    public class DashboardVentaRecienteModelo { public DateTime FechaHora { get; set; } public string Cliente { get; set; } = string.Empty; public string Vendedor { get; set; } = string.Empty; public decimal Total { get; set; } }
    public class AvisoModelo { public int IdAviso { get; set; } public string Titulo { get; set; } = string.Empty; public string Mensaje { get; set; } = string.Empty; public DateTime FechaCreacion { get; set; } public string Autor { get; set; } = string.Empty; public string Destino { get; set; } = string.Empty; }
    public class AvisoDestinoModelo { public int IdFuncionalidad { get; set; } public string Nombre { get; set; } = string.Empty; }

    // Aplica permisos y alcance de sesión a las consultas compactas del dashboard.
    public class DashboardLogica
    {
        private readonly DashboardDatos dashboardDatos = new DashboardDatos();

        // Indica si el usuario puede consultar métricas comerciales en el inicio.
        public bool PuedeVerVentas() => SesionActual.TienePermiso("VENTAS_VER");

        // Indica si el usuario puede consultar indicadores de productos e inventario.
        public bool PuedeVerProductos() => SesionActual.TienePermiso("PRODUCTOS_VER");

        // Usa la sucursal operativa como único alcance para todas las métricas.
        public DashboardResumenModelo ObtenerResumen()
        {
            DashboardResumenDatos datos = dashboardDatos.ObtenerResumen(SesionActual.IdSucursalOperativa);
            return new DashboardResumenModelo { VentasHoy = datos.VentasHoy, IngresosHoy = datos.IngresosHoy, StockBajo = datos.StockBajo, ProductosActivos = datos.ProductosActivos };
        }

        // Obtiene el resumen diario solamente cuando el usuario tiene permiso de ventas.
        public List<DashboardSerieModelo> ObtenerSerieSemanal()
        {
            if (!PuedeVerVentas()) return new List<DashboardSerieModelo>();
            return dashboardDatos.ObtenerSerieSemanal(SesionActual.IdSucursalOperativa).Select(x => new DashboardSerieModelo { Fecha = x.Fecha, Ventas = x.Ventas, Ingresos = x.Ingresos }).ToList();
        }

        // Obtiene el ranking de productos sin habilitar datos a quien no puede verlos.
        public List<DashboardProductoModelo> ObtenerProductosMasVendidos()
        {
            if (!PuedeVerProductos() || !PuedeVerVentas()) return new List<DashboardProductoModelo>();
            return dashboardDatos.ObtenerProductosMasVendidos(SesionActual.IdSucursalOperativa).Select(x => new DashboardProductoModelo { Producto = x.Producto, UnidadesVendidas = x.UnidadesVendidas }).ToList();
        }

        // Obtiene actividad comercial reciente respetando el permiso de consulta de ventas.
        public List<DashboardVentaRecienteModelo> ObtenerActividadReciente()
        {
            if (!PuedeVerVentas()) return new List<DashboardVentaRecienteModelo>();
            return dashboardDatos.ObtenerActividadReciente(SesionActual.IdSucursalOperativa).Select(x => new DashboardVentaRecienteModelo { FechaHora = x.FechaHora, Cliente = x.Cliente, Vendedor = x.Vendedor, Total = x.Total }).ToList();
        }

        // Devuelve los avisos que SQL determina aplicables al usuario actual.
        public List<AvisoModelo> ListarAvisos()
        {
            return dashboardDatos.ListarAvisosParaUsuario(SesionActual.IdUsuario).Select(x => new AvisoModelo { IdAviso = x.IdAviso, Titulo = x.Titulo, Mensaje = x.Mensaje, FechaCreacion = x.FechaCreacion, Autor = x.Autor, Destino = x.Destino }).ToList();
        }

        // Consulta destinos autorizados para mostrar el publicador sólo cuando corresponde.
        public List<AvisoDestinoModelo> ListarDestinosAviso()
        {
            return dashboardDatos.ListarDestinosAviso(SesionActual.IdUsuario).Select(x => new AvisoDestinoModelo { IdFuncionalidad = x.IdFuncionalidad, Nombre = x.Nombre }).ToList();
        }

        // Valida el contenido y delega en SQL la autorización jerárquica definitiva.
        public void PublicarAviso(int idDestino, string titulo, string mensaje)
        {
            titulo = (titulo ?? string.Empty).Trim(); mensaje = (mensaje ?? string.Empty).Trim();
            if (idDestino <= 0 || string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(mensaje)) throw new InvalidOperationException("Debe completar el destinatario, el título y el mensaje del aviso.");
            if (titulo.Length > 100 || mensaje.Length > 500) throw new InvalidOperationException("El aviso supera la longitud permitida.");
            dashboardDatos.PublicarAviso(SesionActual.IdUsuario, idDestino, titulo, mensaje);
        }
    }
}
