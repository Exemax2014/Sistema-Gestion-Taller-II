using Capa_Datos;

namespace Capa_Logica
{
    public enum AlcanceReportes
    {
        Ninguno,
        Propio,
        Sucursal,
        Global
    }

    public class ReporteDiaModelo { public DateTime Fecha { get; set; } public int Ventas { get; set; } public decimal Recaudacion { get; set; } }

    // Centraliza las capacidades del módulo Reportes sin depender de nombres de perfiles.
    public class ReporteLogica
    {
        private readonly ReporteDatos reporteDatos = new ReporteDatos();
        // Indica si el usuario puede ingresar al módulo Reportes.
        public bool PuedeVerReportes()
        {
            return SesionActual.TienePermiso(
                "REPORTES_VER"
            );
        }

        // Indica si el usuario puede consultar indicadores generales de ventas.
        public bool PuedeVerVentas()
        {
            return PuedeConsultarSeccion(
                "REPORTES_VENTAS"
            );
        }

        // Indica si el usuario puede consultar recaudación y descuentos.
        public bool PuedeVerRecaudacion()
        {
            return PuedeConsultarSeccion(
                "REPORTES_RECAUDACION"
            );
        }

        // Indica si el usuario puede consultar rankings de productos vendidos.
        public bool PuedeVerProductos()
        {
            return PuedeConsultarSeccion(
                "REPORTES_PRODUCTOS"
            );
        }

        // Indica si el usuario puede consultar indicadores de stock.
        public bool PuedeVerStock()
        {
            return PuedeConsultarSeccion(
                "REPORTES_STOCK"
            );
        }

        // Indica si el usuario puede consultar rendimiento de vendedores.
        public bool PuedeVerRendimientoVendedores()
        {
            return PuedeConsultarSeccion(
                "REPORTES_RENDIMIENTO_VENDEDORES"
            );
        }

        // Indica si el usuario puede consultar el listado detallado de ventas.
        public bool PuedeVerDetalleVentas()
        {
            return PuedeConsultarSeccion(
                "REPORTES_DETALLE_VENTAS"
            );
        }

        // Indica si el usuario puede exportar resultados ya autorizados del módulo.
        public bool PuedeExportar()
        {
            return PuedeConsultarSeccion(
                "REPORTES_EXPORTAR"
            );
        }

        // Resuelve el único alcance efectivo a partir de permisos y del alcance global cargado en sesión.
        public AlcanceReportes ObtenerAlcanceReportes()
        {
            if (!PuedeVerReportes())
            {
                return AlcanceReportes.Ninguno;
            }

            // La sesión sólo deja IdSucursal nulo a perfiles cuyo PERFIL.alcance_global es verdadero.
            bool perfilGlobal =
                !SesionActual.IdSucursal.HasValue;

            if (perfilGlobal)
            {
                return SesionActual.TienePermiso(
                    "REPORTES_ALCANCE_GLOBAL"
                )
                    ? AlcanceReportes.Global
                    : AlcanceReportes.Ninguno;
            }

            if (
                SesionActual.TienePermiso(
                    "REPORTES_ALCANCE_SUCURSAL"
                )
                && SesionActual.IdSucursal.HasValue)
            {
                return AlcanceReportes.Sucursal;
            }

            if (SesionActual.TienePermiso("REPORTES_ALCANCE_PROPIO"))
            {
                return AlcanceReportes.Propio;
            }

            return AlcanceReportes.Ninguno;
        }

        // Explica por qué se impide una consulta cuando REPORTES_VER no tiene un alcance válido.
        public string ObtenerMensajeSinAlcance()
        {
            return PuedeVerReportes()
                && ObtenerAlcanceReportes() == AlcanceReportes.Ninguno
                    ? "El perfil tiene acceso a Reportes, pero no posee un alcance válido para consultar datos."
                    : string.Empty;
        }

        // Valida fechas, permisos y alcance antes de entregar un resumen al formulario.
        public ReporteResumenDatos ObtenerResumen(DateTime desde, DateTime hasta, int? sucursal, int? vendedor)
        {
            if (!PuedeVerVentas() && !PuedeVerRecaudacion()) throw new InvalidOperationException("No tiene permiso para consultar el resumen de ventas.");
            ValidarConsulta(desde, hasta, sucursal, vendedor, null);
            return reporteDatos.ObtenerResumen(desde, hasta, ResolverSucursal(sucursal), ResolverUsuario(vendedor));
        }

        // Devuelve la serie diaria sólo si existe permiso de ventas o recaudación.
        public List<ReporteDiaModelo> ObtenerVentasPorDia(DateTime desde, DateTime hasta, int? sucursal, int? vendedor)
        {
            if (!PuedeVerVentas() && !PuedeVerRecaudacion()) throw new InvalidOperationException("No tiene permiso para consultar la evolución de ventas.");
            ValidarConsulta(desde, hasta, sucursal, vendedor, null);
            return reporteDatos.ObtenerDias(desde, hasta, ResolverSucursal(sucursal), ResolverUsuario(vendedor)).Select(x=>new ReporteDiaModelo{Fecha=x.Fecha,Ventas=x.Ventas,Recaudacion=x.Recaudacion}).ToList();
        }

        // Devuelve productos vendidos respetando el alcance resuelto para la sesión.
        public List<ReporteProductoDatos> ObtenerProductos(DateTime desde, DateTime hasta, int? sucursal, int? vendedor)
        {
            ValidarConsulta(desde, hasta, sucursal, vendedor, "REPORTES_PRODUCTOS");
            return reporteDatos.ObtenerProductos(desde, hasta, ResolverSucursal(sucursal), ResolverUsuario(vendedor));
        }

        // Devuelve stock bajo con sucursal explícita cuando el alcance es global.
        public List<ReporteStockDatos> ObtenerStock(int? sucursal)
        {
            if (!PuedeVerStock()) throw new InvalidOperationException("No tiene permiso para consultar stock.");
            ValidarAlcance(sucursal, null);
            return reporteDatos.ObtenerStock(ResolverSucursal(sucursal));
        }

        // Devuelve rendimiento por vendedor, excluido para alcance propio.
        public List<ReporteVendedorDatos> ObtenerRendimiento(DateTime desde, DateTime hasta, int? sucursal, int? vendedor)
        {
            if (ObtenerAlcanceReportes() == AlcanceReportes.Propio) throw new InvalidOperationException("El alcance propio no permite comparar vendedores.");
            ValidarConsulta(desde, hasta, sucursal, vendedor, "REPORTES_RENDIMIENTO_VENDEDORES");
            return reporteDatos.ObtenerVendedores(desde, hasta, ResolverSucursal(sucursal), ResolverUsuario(vendedor));
        }

        // Devuelve ventas detalladas dentro de la sucursal o usuario autorizado.
        public List<ReporteVentaDatos> ObtenerDetalleVentas(DateTime desde, DateTime hasta, int? sucursal, int? vendedor)
        {
            ValidarConsulta(desde, hasta, sucursal, vendedor, "REPORTES_DETALLE_VENTAS");
            return reporteDatos.ObtenerDetalle(desde, hasta, ResolverSucursal(sucursal), ResolverUsuario(vendedor));
        }

        // Lista opciones de vendedor sólo para alcances que pueden seleccionarlo.
        public List<ReporteVendedorFiltroDatos> ListarVendedores(int? sucursal)
        {
            if (ObtenerAlcanceReportes() == AlcanceReportes.Propio) return new List<ReporteVendedorFiltroDatos>();
            ValidarAlcance(sucursal, null);
            return reporteDatos.ListarVendedores(ResolverSucursal(sucursal));
        }

        // Impide que la Vista fuerce sucursal, vendedor o fechas fuera del alcance efectivo.
        private void ValidarConsulta(DateTime desde, DateTime hasta, int? sucursal, int? vendedor, string? permiso)
        {
            if (desde.Date > hasta.Date) throw new InvalidOperationException("La fecha Desde no puede ser posterior a la fecha Hasta.");
            if (permiso != null && !PuedeConsultarSeccion(permiso)) throw new InvalidOperationException("No tiene permiso para consultar esta sección de Reportes.");
            ValidarAlcance(sucursal, vendedor);
        }

        // Restringe parámetros de filtro al alcance que Lógica determinó para la sesión.
        private void ValidarAlcance(int? sucursal, int? vendedor)
        {
            AlcanceReportes alcance=ObtenerAlcanceReportes();
            if (alcance == AlcanceReportes.Ninguno) throw new InvalidOperationException(ObtenerMensajeSinAlcance());
            if (alcance == AlcanceReportes.Propio && (sucursal.HasValue || (vendedor.HasValue && vendedor != SesionActual.IdUsuario))) throw new InvalidOperationException("El alcance propio sólo permite consultar sus operaciones.");
            if (alcance == AlcanceReportes.Sucursal && sucursal.HasValue && sucursal != SesionActual.IdSucursal) throw new InvalidOperationException("La sucursal indicada no está autorizada.");
        }

        // Normaliza la sucursal para que Datos nunca reciba un filtro fuera del contexto autorizado.
        private int? ResolverSucursal(int? sucursal) => ObtenerAlcanceReportes() == AlcanceReportes.Sucursal ? SesionActual.IdSucursal : sucursal;

        // Fuerza el usuario actual en alcance propio y respeta filtros sólo en otros alcances.
        private int? ResolverUsuario(int? vendedor) => ObtenerAlcanceReportes() == AlcanceReportes.Propio ? SesionActual.IdUsuario : vendedor;

        // Exige acceso general, permiso específico y un alcance válido antes de habilitar una sección.
        private bool PuedeConsultarSeccion(
            string codigoFuncionalidad)
        {
            return PuedeVerReportes()
                && ObtenerAlcanceReportes() != AlcanceReportes.Ninguno
                && SesionActual.TienePermiso(
                    codigoFuncionalidad
                );
        }
    }
}
