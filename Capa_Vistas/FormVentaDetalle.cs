using Capa_Logica;

namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormVentaDetalle
    //
    // Detalle único y reutilizable de una venta.
    //
    // Puede abrirse desde:
    // - historial de vendedor;
    // - historial de cliente;
    // - módulo Ventas.
    //
    // La autorización real se valida en VentaLogica.
    // ============================================================

    public partial class FormVentaDetalle : Form
    {
        private readonly VentaLogica ventaLogica;

        private readonly int idVenta;
        private readonly TipoHistorialVentas tipoOrigen;
        private readonly int idReferencia;

        private VentaDetalleModelo? ventaActual;


        // ========================================================
        // CONSTRUCTOR
        //
        // tipoOrigen:
        // - Vendedor: valida que la venta pertenezca al vendedor
        //   consultado.
        // - Cliente: valida que la venta pertenezca al cliente
        //   consultado.
        // ========================================================

        public FormVentaDetalle(
            int idVenta,
            TipoHistorialVentas tipoOrigen,
            int idReferencia)
        {
            InitializeComponent();

            ventaLogica =
                new VentaLogica();

            this.idVenta =
                idVenta;

            this.tipoOrigen =
                tipoOrigen;

            this.idReferencia =
                idReferencia;


            ConfigurarGrillas();
            ConfigurarEventos();

            CargarDetalle();
        }


        // ========================================================
        // GRILLAS
        // ========================================================

        private void ConfigurarGrillas()
        {
            ConfigurarGrillaProductos();
            ConfigurarGrillaPagos();
        }


        private void ConfigurarGrillaProductos()
        {
            dgvProductos.AutoGenerateColumns =
                false;

            dgvProductos.Columns.Clear();


            DataGridViewTextBoxColumn colCodigo =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCodigo",
                    DataPropertyName = "CodigoBarra",
                    HeaderText = "Código",
                    Width = 110
                };


            DataGridViewTextBoxColumn colProducto =
                new DataGridViewTextBoxColumn
                {
                    Name = "colProducto",
                    DataPropertyName = "Producto",
                    HeaderText = "Producto",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,

                    MinimumWidth = 180
                };


            DataGridViewTextBoxColumn colCantidad =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCantidad",
                    DataPropertyName = "Cantidad",
                    HeaderText = "Cant.",
                    Width = 70,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            DataGridViewTextBoxColumn colPrecio =
                new DataGridViewTextBoxColumn
                {
                    Name = "colPrecio",
                    DataPropertyName = "PrecioUnitario",
                    HeaderText = "Precio",
                    Width = 105,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            DataGridViewTextBoxColumn colSubtotal =
                new DataGridViewTextBoxColumn
                {
                    Name = "colSubtotal",
                    DataPropertyName = "Subtotal",
                    HeaderText = "Subtotal",
                    Width = 110,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            dgvProductos.Columns.AddRange(
                colCodigo,
                colProducto,
                colCantidad,
                colPrecio,
                colSubtotal
            );
        }


        private void ConfigurarGrillaPagos()
        {
            dgvPagos.AutoGenerateColumns =
                false;

            dgvPagos.Columns.Clear();


            DataGridViewTextBoxColumn colMetodo =
                new DataGridViewTextBoxColumn
                {
                    Name = "colMetodo",
                    DataPropertyName = "MetodoPago",
                    HeaderText = "Método",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,

                    MinimumWidth = 130
                };


            DataGridViewTextBoxColumn colMonto =
                new DataGridViewTextBoxColumn
                {
                    Name = "colMonto",
                    DataPropertyName = "Monto",
                    HeaderText = "Monto",
                    Width = 120,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            dgvPagos.Columns.AddRange(
                colMetodo,
                colMonto
            );
        }


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            btnCerrar.Click +=
                BtnCerrar_Click;

            btnGenerarPdf.Click +=
                BtnGenerarPdf_Click;
        }


        // ========================================================
        // CARGA
        // ========================================================

        private void CargarDetalle()
        {
            ResultadoDetalleVenta resultado;


            if (tipoOrigen == TipoHistorialVentas.Vendedor)
            {
                resultado =
                    ventaLogica.ObtenerDetallePorVendedor(
                        idVenta,
                        idReferencia
                    );
            }
            else
            {
                resultado =
                    ventaLogica.ObtenerDetallePorCliente(
                        idVenta,
                        idReferencia
                    );
            }


            if (
                !resultado.Exitoso
                ||
                resultado.Venta == null)
            {
                MostrarMensaje(
                    "No se pudo cargar la venta",
                    resultado.Mensaje
                );

                btnGenerarPdf.Enabled =
                    false;

                return;
            }


            ventaActual =
                resultado.Venta;


            MostrarDatos(
                ventaActual
            );
        }


        // ========================================================
        // MOSTRAR DATOS
        // ========================================================

        private void MostrarDatos(
            VentaDetalleModelo venta)
        {
            lblTitulo.Text =
                $"Venta N° {venta.IdVenta}";

            lblSubtitulo.Text =
                "Consulta la información completa de la transacción.";


            lblFecha.Text =
                venta.FechaHora.ToString(
                    "dd/MM/yyyy HH:mm"
                );

            lblSucursal.Text =
                string.IsNullOrWhiteSpace(
                    venta.Sucursal)
                    ? "-"
                    : venta.Sucursal;

            lblTipoFactura.Text =
                string.IsNullOrWhiteSpace(
                    venta.TipoFactura)
                    ? "-"
                    : venta.TipoFactura;

            lblCliente.Text =
                string.IsNullOrWhiteSpace(
                    venta.Cliente)
                    ? "-"
                    : venta.Cliente;

            lblDocumento.Text =
                string.IsNullOrWhiteSpace(
                    venta.DocumentoCliente)
                    ? "-"
                    : venta.DocumentoCliente;

            lblVendedor.Text =
                string.IsNullOrWhiteSpace(
                    venta.Vendedor)
                    ? "-"
                    : venta.Vendedor;


            dgvProductos.DataSource =
                venta.Items;

            dgvPagos.DataSource =
                venta.Pagos;


            lblSubtotal.Text =
                venta.Subtotal.ToString(
                    "C2"
                );

            lblDescuento.Text =
                venta.Descuento.ToString(
                    "C2"
                );

            lblTotal.Text =
                venta.Total.ToString(
                    "C2"
                );
        }


        // ========================================================
        // PDF
        // ========================================================

        private void BtnGenerarPdf_Click(
            object? sender,
            EventArgs e)
        {
            if (ventaActual == null)
            {
                MostrarMensaje(
                    "Venta no disponible",
                    "No hay una venta cargada para generar el PDF."
                );

                return;
            }


            // La generación real del PDF se conectará
            // en el siguiente paso con un servicio reutilizable.
            MostrarMensaje(
                "Generar PDF",
                $"La venta N° {ventaActual.IdVenta} está lista para generar el comprobante en PDF."
            );
        }


        // ========================================================
        // CERRAR
        // ========================================================

        private void BtnCerrar_Click(
            object? sender,
            EventArgs e)
        {
            Close();
        }


        // ========================================================
        // MENSAJES
        // ========================================================

        private void MostrarMensaje(
            string titulo,
            string mensaje)
        {
            using FormMensaje formMensaje =
                new FormMensaje(
                    titulo,
                    mensaje
                );


            formMensaje.ShowDialog(
                this
            );
        }
    }
}
