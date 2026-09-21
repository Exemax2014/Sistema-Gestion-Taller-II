using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // Clase: InventarioLogica
    //
    // Coordina las operaciones relacionadas con inventario.
    //
    // Reglas:
    // - Todos los perfiles pueden consultar el stock.
    // - La modificación exige la funcionalidad de productos correspondiente.
    // - El alcance de sucursal se resuelve desde la sesión actual.
    // ============================================================

    public class InventarioLogica
    {
        private readonly InventarioDatos inventarioDatos =
            new InventarioDatos();


        private readonly SucursalDatos sucursalDatos =
            new SucursalDatos();


        // ========================================================
        // OBTENER STOCK DE UN PRODUCTO
        // ========================================================

        public List<StockSucursalModelo> ObtenerStockProducto(
            int idProducto)
        {
            List<StockSucursalModelo> resultado =
                new List<StockSucursalModelo>();


            if (idProducto <= 0)
            {
                return resultado;
            }


            List<SucursalInfo> sucursales =
                sucursalDatos.ObtenerActivas();


            foreach (SucursalInfo sucursal in sucursales)
            {
                InventarioInfo? inventario =
                    inventarioDatos.ObtenerProductoSucursal(
                        idProducto,
                        sucursal.IdSucursal
                    );


                resultado.Add(
                    new StockSucursalModelo
                    {
                        IdSucursal =
                            sucursal.IdSucursal,

                        Sucursal =
                            sucursal.Nombre,

                        Stock =
                            inventario?.Stock
                            ?? 0,

                        StockMinimo =
                            inventario?.StockMinimo
                            ?? 0,

                        // La Vista recibe directamente
                        // si esta sucursal puede modificarse.
                        PuedeModificar =
                            PuedeModificarSucursal(
                                sucursal.IdSucursal
                            )
                    }
                );
            }


            return resultado;
        }


        // ========================================================
        // ESTABLECER STOCK
        // ========================================================

        // Valida el stock y el alcance de sesión antes de delegar la autorización final al procedimiento.
        public ResultadoInventario EstablecerStock(
            int idProducto,
            int idSucursal,
            int stock,
            int stockMinimo)
        {
            if (
                idProducto <= 0
                ||
                idSucursal <= 0)
            {
                return new ResultadoInventario
                {
                    Codigo = 3,

                    Mensaje =
                        "Producto o sucursal no válidos."
                };
            }


            if (
                stock < 0
                ||
                stockMinimo < 0)
            {
                return new ResultadoInventario
                {
                    Codigo = 3,

                    Mensaje =
                        "El stock no puede ser negativo."
                };
            }


            // Aunque la Vista oculte el botón,
            // la operación vuelve a validarse acá.
            if (!PuedeModificarSucursal(idSucursal))
            {
                return new ResultadoInventario
                {
                    Codigo = 5,

                    Mensaje =
                        "No tenés permiso para modificar el stock de esa sucursal."
                };
            }


            ResultadoInventarioDatos resultado =
                inventarioDatos.EstablecerStock(
                    SesionActual.IdUsuario,
                    idProducto,
                    idSucursal,
                    stock,
                    stockMinimo
                );


            return new ResultadoInventario
            {
                Codigo =
                    resultado.Codigo,

                Mensaje =
                    resultado.Mensaje
            };
        }


        // ========================================================
        // REGLA DE MODIFICACIÓN POR SUCURSAL
        //
        // Esta regla pertenece a Capa_Logica y combina la funcionalidad
        // existente de productos con el alcance dinámico de la sesión.
        // ========================================================

        private bool PuedeModificarSucursal(
            int idSucursal)
        {
            if (
                idSucursal <= 0
                ||
                !SesionActual.TienePermiso(
                    "PRODUCTOS_MODIFICAR"
                ))
            {
                return false;
            }


            // El alcance global se determina desde la sesión, no por ausencia de sucursal fija.
            if (SesionActual.AlcanceGlobal)
            {
                return true;
            }


            // Un perfil con alcance fijo solo puede modificar su sucursal asignada.
            return SesionActual.IdSucursal.HasValue
                && SesionActual.IdSucursal.Value == idSucursal;
        }
    }
}
