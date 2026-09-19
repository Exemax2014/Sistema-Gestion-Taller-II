using Microsoft.Data.SqlClient;
using System.Data;

namespace Capa_Datos
{
    // ============================================================
    // MODELOS DE CONSULTA
    // ============================================================

    public class VentaResumenDatos
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


    public class VentaDetalleDatos
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

        public List<VentaItemDatos> Items { get; set; } =
            new List<VentaItemDatos>();

        public List<VentaPagoDatos> Pagos { get; set; } =
            new List<VentaPagoDatos>();
    }


    public class VentaItemDatos
    {
        public int IdDetalleVenta { get; set; }
        public int IdProducto { get; set; }

        public string CodigoBarra { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }


    public class VentaPagoDatos
    {
        public int IdPago { get; set; }
        public int IdMetodoPago { get; set; }

        public string MetodoPago { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }


    // ============================================================
    // MODELOS PARA REGISTRAR UNA VENTA
    // ============================================================

    public class VentaRegistrarDatos
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public int IdSucursal { get; set; }

        public List<VentaItemGuardarDatos> Items { get; set; } =
            new List<VentaItemGuardarDatos>();

        public List<VentaPagoGuardarDatos> Pagos { get; set; } =
            new List<VentaPagoGuardarDatos>();
    }


    public class VentaItemGuardarDatos
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }


    public class VentaPagoGuardarDatos
    {
        public int IdMetodoPago { get; set; }
        public decimal Monto { get; set; }
    }


    public class ResultadoVentaDatos
    {
        public int IdGenerado { get; set; }
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }


    // ============================================================
    // CATÁLOGO DE MÉTODOS DE PAGO
    // ============================================================

    public class MetodoPagoDatos
    {
        public int IdMetodoPago { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }


    // ============================================================
    // ACCESO A DATOS DE VENTAS
    // ============================================================

    public class VentaDatos
    {
        public List<VentaResumenDatos> ListarPorVendedor(
            int idUsuario,
            DateTime desde,
            DateTime hasta)
        {
            List<VentaResumenDatos> ventas =
                new List<VentaResumenDatos>();

            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Venta_ListarPorVendedor",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idUsuario",
                SqlDbType.Int
            ).Value = idUsuario;

            comando.Parameters.Add(
                "@desde",
                SqlDbType.Date
            ).Value = desde.Date;

            comando.Parameters.Add(
                "@hasta",
                SqlDbType.Date
            ).Value = hasta.Date;

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                ventas.Add(
                    LeerVentaResumen(
                        lector
                    )
                );
            }

            return ventas;
        }


        public List<VentaResumenDatos> ListarPorCliente(
            int idCliente,
            DateTime desde,
            DateTime hasta)
        {
            List<VentaResumenDatos> ventas =
                new List<VentaResumenDatos>();

            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Venta_ListarPorCliente",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idCliente",
                SqlDbType.Int
            ).Value = idCliente;

            comando.Parameters.Add(
                "@desde",
                SqlDbType.Date
            ).Value = desde.Date;

            comando.Parameters.Add(
                "@hasta",
                SqlDbType.Date
            ).Value = hasta.Date;

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                ventas.Add(
                    LeerVentaResumen(
                        lector
                    )
                );
            }

            return ventas;
        }


        public VentaDetalleDatos? ObtenerDetalle(
            int idVenta)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Venta_ObtenerDetalle",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idVenta",
                SqlDbType.Int
            ).Value = idVenta;

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            if (!lector.Read())
            {
                return null;
            }

            VentaDetalleDatos venta =
                new VentaDetalleDatos
                {
                    IdVenta =
                        Convert.ToInt32(
                            lector["id_venta"]
                        ),

                    FechaHora =
                        Convert.ToDateTime(
                            lector["fecha_hora"]
                        ),

                    TipoFactura =
                        ObtenerTexto(
                            lector,
                            "tipo_factura"
                        ),

                    Subtotal =
                        Convert.ToDecimal(
                            lector["subtotal"]
                        ),

                    Descuento =
                        Convert.ToDecimal(
                            lector["descuento"]
                        ),

                    Total =
                        Convert.ToDecimal(
                            lector["total"]
                        ),

                    IdCliente =
                        Convert.ToInt32(
                            lector["id_cliente"]
                        ),

                    Cliente =
                        ObtenerTexto(
                            lector,
                            "cliente"
                        ),

                    DocumentoCliente =
                        ObtenerTexto(
                            lector,
                            "documento_cliente"
                        ),

                    IdUsuario =
                        Convert.ToInt32(
                            lector["id_usuario"]
                        ),

                    Vendedor =
                        ObtenerTexto(
                            lector,
                            "vendedor"
                        ),

                    NombreUsuario =
                        ObtenerTexto(
                            lector,
                            "nombre_usuario"
                        ),

                    IdSucursal =
                        Convert.ToInt32(
                            lector["id_sucursal"]
                        ),

                    Sucursal =
                        ObtenerTexto(
                            lector,
                            "sucursal"
                        )
                };

            if (lector.NextResult())
            {
                while (lector.Read())
                {
                    venta.Items.Add(
                        new VentaItemDatos
                        {
                            IdDetalleVenta =
                                Convert.ToInt32(
                                    lector["id_detalle_venta"]
                                ),

                            IdProducto =
                                Convert.ToInt32(
                                    lector["id_producto"]
                                ),

                            CodigoBarra =
                                ObtenerTexto(
                                    lector,
                                    "codigo_barra"
                                ),

                            Producto =
                                ObtenerTexto(
                                    lector,
                                    "producto"
                                ),

                            Cantidad =
                                Convert.ToInt32(
                                    lector["cantidad"]
                                ),

                            PrecioUnitario =
                                Convert.ToDecimal(
                                    lector["precio_unitario"]
                                ),

                            Subtotal =
                                Convert.ToDecimal(
                                    lector["subtotal"]
                                )
                        }
                    );
                }
            }

            if (lector.NextResult())
            {
                while (lector.Read())
                {
                    venta.Pagos.Add(
                        new VentaPagoDatos
                        {
                            IdPago =
                                Convert.ToInt32(
                                    lector["id_pago"]
                                ),

                            IdMetodoPago =
                                Convert.ToInt32(
                                    lector["id_metodo_pago"]
                                ),

                            MetodoPago =
                                ObtenerTexto(
                                    lector,
                                    "metodo_pago"
                                ),

                            Monto =
                                Convert.ToDecimal(
                                    lector["monto"]
                                )
                        }
                    );
                }
            }

            return venta;
        }


        public List<MetodoPagoDatos> ListarMetodosPago()
        {
            List<MetodoPagoDatos> metodos =
                new List<MetodoPagoDatos>();

            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_MetodoPago_Listar",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                metodos.Add(
                    new MetodoPagoDatos
                    {
                        IdMetodoPago =
                            Convert.ToInt32(
                                lector["id_metodo_pago"]
                            ),

                        Nombre =
                            ObtenerTexto(
                                lector,
                                "nombre"
                            ),

                        Descripcion =
                            ObtenerTexto(
                                lector,
                                "descripcion"
                            )
                    }
                );
            }

            return metodos;
        }


        public ResultadoVentaDatos Registrar(
            VentaRegistrarDatos venta)
        {
            DataTable tablaItems =
                CrearTablaItems(
                    venta.Items
                );

            DataTable tablaPagos =
                CrearTablaPagos(
                    venta.Pagos
                );

            using SqlConnection conexion =
                Conexion.CrearConexion();

            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Venta_Registrar",
                    conexion
                );

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@idCliente",
                SqlDbType.Int
            ).Value = venta.IdCliente;

            comando.Parameters.Add(
                "@idUsuario",
                SqlDbType.Int
            ).Value = venta.IdUsuario;

            comando.Parameters.Add(
                "@idSucursal",
                SqlDbType.Int
            ).Value = venta.IdSucursal;

            SqlParameter parametroItems =
                comando.Parameters.Add(
                    "@items",
                    SqlDbType.Structured
                );

            parametroItems.TypeName =
                "dbo.VentaItemTipo";

            parametroItems.Value =
                tablaItems;

            SqlParameter parametroPagos =
                comando.Parameters.Add(
                    "@pagos",
                    SqlDbType.Structured
                );

            parametroPagos.TypeName =
                "dbo.VentaPagoTipo";

            parametroPagos.Value =
                tablaPagos;

            SqlParameter parametroIdGenerado =
                new SqlParameter(
                    "@IdGenerado",
                    SqlDbType.Int
                )
                {
                    Direction =
                        ParameterDirection.Output
                };

            SqlParameter parametroCodigo =
                new SqlParameter(
                    "@CodigoResultado",
                    SqlDbType.Int
                )
                {
                    Direction =
                        ParameterDirection.Output
                };

            SqlParameter parametroMensaje =
                new SqlParameter(
                    "@MensajeResultado",
                    SqlDbType.NVarChar,
                    250
                )
                {
                    Direction =
                        ParameterDirection.Output
                };

            comando.Parameters.Add(
                parametroIdGenerado
            );

            comando.Parameters.Add(
                parametroCodigo
            );

            comando.Parameters.Add(
                parametroMensaje
            );

            conexion.Open();

            comando.ExecuteNonQuery();

            return new ResultadoVentaDatos
            {
                IdGenerado =
                    parametroIdGenerado.Value == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            parametroIdGenerado.Value
                        ),

                Codigo =
                    parametroCodigo.Value == DBNull.Value
                        ? 500
                        : Convert.ToInt32(
                            parametroCodigo.Value
                        ),

                Mensaje =
                    parametroMensaje.Value == DBNull.Value
                        ? string.Empty
                        : Convert.ToString(
                            parametroMensaje.Value
                        )
                        ?? string.Empty
            };
        }


        private static DataTable CrearTablaItems(
            IEnumerable<VentaItemGuardarDatos> items)
        {
            DataTable tabla =
                new DataTable();

            tabla.Columns.Add(
                "id_producto",
                typeof(int)
            );

            tabla.Columns.Add(
                "cantidad",
                typeof(int)
            );

            foreach (VentaItemGuardarDatos item in items)
            {
                tabla.Rows.Add(
                    item.IdProducto,
                    item.Cantidad
                );
            }

            return tabla;
        }


        private static DataTable CrearTablaPagos(
            IEnumerable<VentaPagoGuardarDatos> pagos)
        {
            DataTable tabla =
                new DataTable();

            tabla.Columns.Add(
                "id_metodo_pago",
                typeof(int)
            );

            tabla.Columns.Add(
                "monto",
                typeof(decimal)
            );

            foreach (VentaPagoGuardarDatos pago in pagos)
            {
                tabla.Rows.Add(
                    pago.IdMetodoPago,
                    pago.Monto
                );
            }

            return tabla;
        }


        private static VentaResumenDatos LeerVentaResumen(
            SqlDataReader lector)
        {
            return new VentaResumenDatos
            {
                IdVenta =
                    Convert.ToInt32(
                        lector["id_venta"]
                    ),

                FechaHora =
                    Convert.ToDateTime(
                        lector["fecha_hora"]
                    ),

                TipoFactura =
                    ObtenerTexto(
                        lector,
                        "tipo_factura"
                    ),

                Subtotal =
                    Convert.ToDecimal(
                        lector["subtotal"]
                    ),

                Descuento =
                    Convert.ToDecimal(
                        lector["descuento"]
                    ),

                Total =
                    Convert.ToDecimal(
                        lector["total"]
                    ),

                IdCliente =
                    Convert.ToInt32(
                        lector["id_cliente"]
                    ),

                Cliente =
                    ObtenerTexto(
                        lector,
                        "cliente"
                    ),

                IdUsuario =
                    Convert.ToInt32(
                        lector["id_usuario"]
                    ),

                Vendedor =
                    ObtenerTexto(
                        lector,
                        "vendedor"
                    ),

                IdSucursal =
                    Convert.ToInt32(
                        lector["id_sucursal"]
                    ),

                Sucursal =
                    ObtenerTexto(
                        lector,
                        "sucursal"
                    )
            };
        }


        private static string ObtenerTexto(
            SqlDataReader lector,
            string columna)
        {
            object valor =
                lector[columna];

            if (valor == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(
                valor
            )
            ?? string.Empty;
        }
    }
}
