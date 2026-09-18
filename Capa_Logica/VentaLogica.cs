using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // MODELOS DE CONSULTA
    // ============================================================

    public class VentaResumenModelo
    {
        public int IdVenta { get; set; }
        public DateTime FechaHora { get; set; }
        public string TipoFactura { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }

        public int IdCliente { get; set; }
        public string Cliente { get; set; } = string.Empty;

        public int IdUsuario { get; set; }
        public string Vendedor { get; set; } = string.Empty;

        public int IdSucursal { get; set; }
        public string Sucursal { get; set; } = string.Empty;
    }


    public class VentaDetalleModelo
    {
        public int IdVenta { get; set; }
        public DateTime FechaHora { get; set; }
        public string TipoFactura { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }

        public int IdCliente { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string DocumentoCliente { get; set; } = string.Empty;

        public int IdUsuario { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;

        public int IdSucursal { get; set; }
        public string Sucursal { get; set; } = string.Empty;

        public List<VentaItemModelo> Items { get; set; } =
            new List<VentaItemModelo>();

        public List<VentaPagoModelo> Pagos { get; set; } =
            new List<VentaPagoModelo>();
    }


    public class VentaItemModelo
    {
        public int IdDetalleVenta { get; set; }
        public int IdProducto { get; set; }

        public string CodigoBarra { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }


    public class VentaPagoModelo
    {
        public int IdPago { get; set; }
        public int IdMetodoPago { get; set; }

        public string MetodoPago { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }


    // ============================================================
    // MODELOS AUXILIARES PARA LA PANTALLA DE VENTAS
    //
    // Evitan que Capa_Vistas dependa directamente de Capa_Datos.
    // ============================================================

    public class ProductoVentaModelo
    {
        public int IdProducto { get; set; }
        public string CodigoBarra { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
    }


    public class ClienteVentaModelo
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }


    // ============================================================
    // MODELOS PARA REGISTRAR UNA VENTA
    // ============================================================

    public class VentaRegistrarModelo
    {
        public int IdCliente { get; set; }

        public List<VentaItemGuardarModelo> Items { get; set; } =
            new List<VentaItemGuardarModelo>();

        public List<VentaPagoGuardarModelo> Pagos { get; set; } =
            new List<VentaPagoGuardarModelo>();
    }


    public class VentaItemGuardarModelo
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }


    public class VentaPagoGuardarModelo
    {
        public int IdMetodoPago { get; set; }
        public decimal Monto { get; set; }
    }


    public class MetodoPagoModelo
    {
        public int IdMetodoPago { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }


    // ============================================================
    // RESULTADOS
    // ============================================================

    public class ResultadoConsultaVentas
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;

        public List<VentaResumenModelo> Ventas { get; set; } =
            new List<VentaResumenModelo>();
    }


    public class ResultadoDetalleVenta
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;

        public VentaDetalleModelo? Venta { get; set; }
    }


    public class ResultadoVenta
    {
        public int Codigo { get; set; }
        public int IdVenta { get; set; }
        public string Mensaje { get; set; } = string.Empty;

        public bool Exitoso =>
            Codigo == 0;
    }


    // ============================================================
    // LÓGICA DE VENTAS
    // ============================================================

    public class VentaLogica
    {
        private const decimal MontoMaximo = 9999999999999999.99m;

        private readonly VentaDatos ventaDatos;


        public VentaLogica()
        {
            ventaDatos =
                new VentaDatos();
        }


        // ========================================================
        // PERMISOS
        // ========================================================

        public bool PuedeVerVentas()
        {
            return SesionActual.TienePermiso(
                "VENTAS_VER"
            );
        }


        public bool PuedeRealizarVentas()
        {
            return SesionActual.TienePermiso(
                "VENTAS_REALIZAR"
            );
        }


        public bool PuedeVerClientes()
        {
            return SesionActual.TienePermiso(
                "CLIENTES_VER"
            );
        }


        public bool PuedeVerUsuarios()
        {
            return SesionActual.TienePermiso(
                "USUARIOS_VER"
            );
        }


        // ========================================================
        // BÚSQUEDA DE PRODUCTOS PARA VENTA
        //
        // La Vista recibe modelos de Capa_Logica.
        // La búsqueda real reutiliza ProductoLogica y respeta
        // la sucursal operativa actual.
        // ========================================================

        public List<ProductoVentaModelo> BuscarProductosParaVenta(
            string texto)
        {
            if (!PuedeRealizarVentas())
            {
                return new List<ProductoVentaModelo>();
            }


            int idSucursal;


            try
            {
                idSucursal =
                    SesionActual
                        .ObtenerIdSucursalOperativa();
            }
            catch (InvalidOperationException)
            {
                return new List<ProductoVentaModelo>();
            }


            ProductoLogica productoLogica =
                new ProductoLogica();


            var productos =
                productoLogica.Buscar(
                    texto ?? string.Empty,
                    idSucursal
                );


            return productos
                .Select(
                    producto =>
                        new ProductoVentaModelo
                        {
                            IdProducto =
                                producto.IdProducto,

                            CodigoBarra =
                                producto.CodigoBarra,

                            Nombre =
                                producto.Nombre,

                            Descripcion =
                                producto.Descripcion,

                            PrecioVenta =
                                producto.PrecioVenta,

                            Stock =
                                producto.Stock
                        }
                )
                .ToList();
        }


        // ========================================================
        // BÚSQUEDA DE CLIENTES PARA VENTA
        //
        // Reutiliza ClienteLogica pero expone únicamente un modelo
        // propio de Capa_Logica hacia la Vista.
        // ========================================================

        public List<ClienteVentaModelo> BuscarClientesParaVenta(
            string texto)
        {
            if (!PuedeRealizarVentas())
            {
                return new List<ClienteVentaModelo>();
            }


            ClienteLogica clienteLogica =
                new ClienteLogica();


            var clientes =
                clienteLogica.Buscar(
                    texto ?? string.Empty
                );


            return clientes
                .Select(
                    cliente =>
                        new ClienteVentaModelo
                        {
                            IdCliente =
                                cliente.IdCliente,

                            Nombre =
                                cliente.Nombre
                        }
                )
                .ToList();
        }


        // ========================================================
        // MÉTODOS DE PAGO
        // ========================================================

        public List<MetodoPagoModelo> ObtenerMetodosPago()
        {
            if (!PuedeRealizarVentas())
            {
                return new List<MetodoPagoModelo>();
            }


            return ventaDatos
                .ListarMetodosPago()
                .Select(
                    metodo =>
                        new MetodoPagoModelo
                        {
                            IdMetodoPago =
                                metodo.IdMetodoPago,

                            Nombre =
                                metodo.Nombre,

                            Descripcion =
                                metodo.Descripcion
                        }
                )
                .ToList();
        }


        // ========================================================
        // REGISTRAR VENTA
        //
        // La Vista solamente envía:
        // - cliente;
        // - productos + cantidades;
        // - pagos.
        //
        // Usuario y sucursal se toman de la sesión actual.
        //
        // Precio, total, fecha y stock definitivo se validan
        // nuevamente dentro de SQL Server.
        // ========================================================

        public ResultadoVenta Registrar(
            VentaRegistrarModelo venta)
        {
            if (!PuedeRealizarVentas())
            {
                return ErrorRegistro(
                    5,
                    "No tiene permiso para realizar ventas."
                );
            }


            if (!SesionActual.SesionIniciada)
            {
                return ErrorRegistro(
                    5,
                    "No hay una sesión iniciada."
                );
            }


            if (venta == null)
            {
                return ErrorRegistro(
                    3,
                    "Los datos de la venta no son válidos."
                );
            }


            if (venta.IdCliente <= 0)
            {
                return ErrorRegistro(
                    3,
                    "Debe seleccionar un cliente."
                );
            }


            ResultadoVenta validacionItems =
                ValidarItems(
                    venta.Items
                );


            if (!validacionItems.Exitoso)
            {
                return validacionItems;
            }


            ResultadoVenta validacionPagos =
                ValidarPagos(
                    venta.Pagos
                );


            if (!validacionPagos.Exitoso)
            {
                return validacionPagos;
            }


            int idSucursal;


            try
            {
                idSucursal =
                    SesionActual
                        .ObtenerIdSucursalOperativa();
            }
            catch (InvalidOperationException ex)
            {
                return ErrorRegistro(
                    5,
                    ex.Message
                );
            }


            VentaRegistrarDatos datos =
                new VentaRegistrarDatos
                {
                    IdCliente =
                        venta.IdCliente,

                    IdUsuario =
                        SesionActual.IdUsuario,

                    IdSucursal =
                        idSucursal,

                    Items =
                        venta.Items
                            .Select(
                                item =>
                                    new VentaItemGuardarDatos
                                    {
                                        IdProducto =
                                            item.IdProducto,

                                        Cantidad =
                                            item.Cantidad
                                    }
                            )
                            .ToList(),

                    Pagos =
                        venta.Pagos
                            .Select(
                                pago =>
                                    new VentaPagoGuardarDatos
                                    {
                                        IdMetodoPago =
                                            pago.IdMetodoPago,

                                        Monto =
                                            pago.Monto
                                    }
                            )
                            .ToList()
                };


            ResultadoVentaDatos resultadoDatos =
                ventaDatos.Registrar(
                    datos
                );


            return new ResultadoVenta
            {
                Codigo =
                    resultadoDatos.Codigo,

                IdVenta =
                    resultadoDatos.IdGenerado,

                Mensaje =
                    resultadoDatos.Mensaje
            };
        }


        // ========================================================
        // VALIDACIONES DE REGISTRO
        // ========================================================

        private static ResultadoVenta ValidarItems(
            List<VentaItemGuardarModelo>? items)
        {
            if (
                items == null
                ||
                items.Count == 0)
            {
                return ErrorRegistro(
                    3,
                    "Debe agregar al menos un producto a la venta."
                );
            }


            foreach (VentaItemGuardarModelo item in items)
            {
                if (item.IdProducto <= 0)
                {
                    return ErrorRegistro(
                        3,
                        "Uno de los productos seleccionados no es válido."
                    );
                }


                if (item.Cantidad <= 0)
                {
                    return ErrorRegistro(
                        3,
                        "La cantidad de cada producto debe ser mayor que cero."
                    );
                }
            }


            bool hayProductosDuplicados =
                items
                    .GroupBy(
                        item =>
                            item.IdProducto
                    )
                    .Any(
                        grupo =>
                            grupo.Count() > 1
                    );


            if (hayProductosDuplicados)
            {
                return ErrorRegistro(
                    3,
                    "Un mismo producto no puede aparecer duplicado en la venta."
                );
            }


            return ResultadoCorrecto();
        }


        // Valida los pagos de forma autoritativa antes de formar el TVP para SQL Server.
        private static ResultadoVenta ValidarPagos(
            List<VentaPagoGuardarModelo>? pagos)
        {
            if (
                pagos == null
                ||
                pagos.Count == 0)
            {
                return ErrorRegistro(
                    3,
                    "Debe agregar al menos una forma de pago."
                );
            }


            foreach (VentaPagoGuardarModelo pago in pagos)
            {
                if (pago.IdMetodoPago <= 0)
                {
                    return ErrorRegistro(
                        3,
                        "Uno de los métodos de pago seleccionados no es válido."
                    );
                }


                string? errorMonto = ValidarMontoPago(pago.Monto);

                if (errorMonto != null)
                {
                    return ErrorRegistro(
                        3,
                        errorMonto
                    );
                }
            }


            bool hayMetodosDuplicados =
                pagos
                    .GroupBy(
                        pago =>
                            pago.IdMetodoPago
                    )
                    .Any(
                        grupo =>
                            grupo.Count() > 1
                    );


            if (hayMetodosDuplicados)
            {
                return ErrorRegistro(
                    3,
                    "Un mismo método de pago no puede aparecer duplicado. Sume los importes antes de confirmar."
                );
            }


            return ResultadoCorrecto();
        }


        // Comprueba el rango y la escala compatibles con PAGO.monto DECIMAL(18,2).
        public static string? ValidarMontoPago(decimal monto)
        {
            if (monto <= 0)
            {
                return "El monto de cada pago debe ser mayor que cero.";
            }

            if (monto > MontoMaximo || !TieneHastaDosDecimales(monto))
            {
                return "El monto de pago debe tener hasta 2 decimales y estar dentro del rango permitido.";
            }

            return null;
        }


        // Obtiene la escala real del decimal para no aceptar valores que SQL redondearía.
        private static bool TieneHastaDosDecimales(decimal valor)
        {
            int escala = (decimal.GetBits(valor)[3] >> 16) & 0x7F;
            return escala <= 2;
        }


        // ========================================================
        // HISTORIAL POR VENDEDOR
        // ========================================================

        public ResultadoConsultaVentas ListarPorVendedor(
            int idUsuario,
            DateTime desde,
            DateTime hasta)
        {
            ResultadoConsultaVentas? errorFechas =
                ValidarFechas(
                    desde,
                    hasta
                );


            if (errorFechas != null)
            {
                return errorFechas;
            }


            if (!PuedeVerVentas())
            {
                return ErrorListado(
                    "No tiene permiso para consultar ventas."
                );
            }


            if (idUsuario <= 0)
            {
                return ErrorListado(
                    "El usuario indicado no es válido."
                );
            }


            bool consultaPropia =
                SesionActual.IdUsuario
                ==
                idUsuario;


            if (
                !consultaPropia
                &&
                !PuedeVerUsuarios())
            {
                return ErrorListado(
                    "No tiene permiso para consultar las ventas de otro usuario."
                );
            }


            List<VentaResumenDatos> datos =
                ventaDatos.ListarPorVendedor(
                    idUsuario,
                    desde,
                    hasta
                );


            return new ResultadoConsultaVentas
            {
                Exitoso = true,
                Ventas =
                    datos
                        .Select(
                            MapearResumen
                        )
                        .ToList()
            };
        }


        // ========================================================
        // HISTORIAL POR CLIENTE
        // ========================================================

        public ResultadoConsultaVentas ListarPorCliente(
            int idCliente,
            DateTime desde,
            DateTime hasta)
        {
            ResultadoConsultaVentas? errorFechas =
                ValidarFechas(
                    desde,
                    hasta
                );


            if (errorFechas != null)
            {
                return errorFechas;
            }


            if (!PuedeVerVentas())
            {
                return ErrorListado(
                    "No tiene permiso para consultar ventas."
                );
            }


            if (!PuedeVerClientes())
            {
                return ErrorListado(
                    "No tiene permiso para consultar clientes."
                );
            }


            if (idCliente <= 0)
            {
                return ErrorListado(
                    "El cliente indicado no es válido."
                );
            }


            List<VentaResumenDatos> datos =
                ventaDatos.ListarPorCliente(
                    idCliente,
                    desde,
                    hasta
                );


            return new ResultadoConsultaVentas
            {
                Exitoso = true,
                Ventas =
                    datos
                        .Select(
                            MapearResumen
                        )
                        .ToList()
            };
        }


        // ========================================================
        // DETALLE DESDE HISTORIAL DE VENDEDOR
        // ========================================================

        public ResultadoDetalleVenta ObtenerDetallePorVendedor(
            int idVenta,
            int idUsuarioConsultado)
        {
            if (!PuedeVerVentas())
            {
                return ErrorDetalle(
                    "No tiene permiso para consultar ventas."
                );
            }


            if (
                idVenta <= 0
                ||
                idUsuarioConsultado <= 0)
            {
                return ErrorDetalle(
                    "La venta o el usuario indicado no son válidos."
                );
            }


            bool consultaPropia =
                SesionActual.IdUsuario
                ==
                idUsuarioConsultado;


            if (
                !consultaPropia
                &&
                !PuedeVerUsuarios())
            {
                return ErrorDetalle(
                    "No tiene permiso para consultar las ventas de otro usuario."
                );
            }


            VentaDetalleDatos? datos =
                ventaDatos.ObtenerDetalle(
                    idVenta
                );


            if (datos == null)
            {
                return ErrorDetalle(
                    "La venta indicada no existe."
                );
            }


            if (
                datos.IdUsuario
                !=
                idUsuarioConsultado)
            {
                return ErrorDetalle(
                    "La venta no pertenece al vendedor consultado."
                );
            }


            return new ResultadoDetalleVenta
            {
                Exitoso = true,
                Venta =
                    MapearDetalle(
                        datos
                    )
            };
        }


        // ========================================================
        // DETALLE DESDE HISTORIAL DE CLIENTE
        // ========================================================

        public ResultadoDetalleVenta ObtenerDetallePorCliente(
            int idVenta,
            int idClienteConsultado)
        {
            if (!PuedeVerVentas())
            {
                return ErrorDetalle(
                    "No tiene permiso para consultar ventas."
                );
            }


            if (!PuedeVerClientes())
            {
                return ErrorDetalle(
                    "No tiene permiso para consultar clientes."
                );
            }


            if (
                idVenta <= 0
                ||
                idClienteConsultado <= 0)
            {
                return ErrorDetalle(
                    "La venta o el cliente indicado no son válidos."
                );
            }


            VentaDetalleDatos? datos =
                ventaDatos.ObtenerDetalle(
                    idVenta
                );


            if (datos == null)
            {
                return ErrorDetalle(
                    "La venta indicada no existe."
                );
            }


            if (
                datos.IdCliente
                !=
                idClienteConsultado)
            {
                return ErrorDetalle(
                    "La venta no pertenece al cliente consultado."
                );
            }


            return new ResultadoDetalleVenta
            {
                Exitoso = true,
                Venta =
                    MapearDetalle(
                        datos
                    )
            };
        }


        // ========================================================
        // DETALLE DE UNA VENTA PROPIA
        // ========================================================

        public ResultadoDetalleVenta ObtenerDetallePropio(
            int idVenta)
        {
            if (!PuedeVerVentas())
            {
                return ErrorDetalle(
                    "No tiene permiso para consultar ventas."
                );
            }


            if (idVenta <= 0)
            {
                return ErrorDetalle(
                    "La venta indicada no es válida."
                );
            }


            VentaDetalleDatos? datos =
                ventaDatos.ObtenerDetalle(
                    idVenta
                );


            if (datos == null)
            {
                return ErrorDetalle(
                    "La venta indicada no existe."
                );
            }


            if (
                datos.IdUsuario
                !=
                SesionActual.IdUsuario)
            {
                return ErrorDetalle(
                    "No tiene permiso para consultar esta venta."
                );
            }


            return new ResultadoDetalleVenta
            {
                Exitoso = true,
                Venta =
                    MapearDetalle(
                        datos
                    )
            };
        }


        // ========================================================
        // MAPEOS
        // ========================================================

        private static VentaResumenModelo MapearResumen(
            VentaResumenDatos datos)
        {
            return new VentaResumenModelo
            {
                IdVenta =
                    datos.IdVenta,

                FechaHora =
                    datos.FechaHora,

                TipoFactura =
                    datos.TipoFactura,

                Subtotal =
                    datos.Subtotal,

                Descuento =
                    datos.Descuento,

                Total =
                    datos.Total,

                IdCliente =
                    datos.IdCliente,

                Cliente =
                    datos.Cliente,

                IdUsuario =
                    datos.IdUsuario,

                Vendedor =
                    datos.Vendedor,

                IdSucursal =
                    datos.IdSucursal,

                Sucursal =
                    datos.Sucursal
            };
        }


        private static VentaDetalleModelo MapearDetalle(
            VentaDetalleDatos datos)
        {
            return new VentaDetalleModelo
            {
                IdVenta =
                    datos.IdVenta,

                FechaHora =
                    datos.FechaHora,

                TipoFactura =
                    datos.TipoFactura,

                Subtotal =
                    datos.Subtotal,

                Descuento =
                    datos.Descuento,

                Total =
                    datos.Total,

                IdCliente =
                    datos.IdCliente,

                Cliente =
                    datos.Cliente,

                DocumentoCliente =
                    datos.DocumentoCliente,

                IdUsuario =
                    datos.IdUsuario,

                Vendedor =
                    datos.Vendedor,

                NombreUsuario =
                    datos.NombreUsuario,

                IdSucursal =
                    datos.IdSucursal,

                Sucursal =
                    datos.Sucursal,

                Items =
                    datos.Items
                        .Select(
                            item =>
                                new VentaItemModelo
                                {
                                    IdDetalleVenta =
                                        item.IdDetalleVenta,

                                    IdProducto =
                                        item.IdProducto,

                                    CodigoBarra =
                                        item.CodigoBarra,

                                    Producto =
                                        item.Producto,

                                    Cantidad =
                                        item.Cantidad,

                                    PrecioUnitario =
                                        item.PrecioUnitario,

                                    Subtotal =
                                        item.Subtotal
                                }
                        )
                        .ToList(),

                Pagos =
                    datos.Pagos
                        .Select(
                            pago =>
                                new VentaPagoModelo
                                {
                                    IdPago =
                                        pago.IdPago,

                                    IdMetodoPago =
                                        pago.IdMetodoPago,

                                    MetodoPago =
                                        pago.MetodoPago,

                                    Monto =
                                        pago.Monto
                                }
                        )
                        .ToList()
            };
        }


        // ========================================================
        // HELPERS DE RESULTADOS
        // ========================================================

        private static ResultadoConsultaVentas? ValidarFechas(
            DateTime desde,
            DateTime hasta)
        {
            if (
                desde.Date
                >
                hasta.Date)
            {
                return ErrorListado(
                    "La fecha Desde no puede ser posterior a la fecha Hasta."
                );
            }


            return null;
        }


        private static ResultadoConsultaVentas ErrorListado(
            string mensaje)
        {
            return new ResultadoConsultaVentas
            {
                Exitoso = false,
                Mensaje = mensaje,
                Ventas =
                    new List<VentaResumenModelo>()
            };
        }


        private static ResultadoDetalleVenta ErrorDetalle(
            string mensaje)
        {
            return new ResultadoDetalleVenta
            {
                Exitoso = false,
                Mensaje = mensaje,
                Venta = null
            };
        }


        private static ResultadoVenta ErrorRegistro(
            int codigo,
            string mensaje)
        {
            return new ResultadoVenta
            {
                Codigo = codigo,
                IdVenta = 0,
                Mensaje = mensaje
            };
        }


        private static ResultadoVenta ResultadoCorrecto()
        {
            return new ResultadoVenta
            {
                Codigo = 0,
                IdVenta = 0,
                Mensaje = string.Empty
            };
        }
    }
}
