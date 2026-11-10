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
        private readonly FormPrincipal? formPrincipal;
        private readonly OrigenHistorialVentas origenHistorial;
        private readonly string nombreReferencia;

        private readonly int idVenta;
        private readonly TipoHistorialVentas tipoOrigen;
        private readonly int idReferencia;
        private readonly bool consultaDesdeReportes;
        private readonly FormReportesGeneral.SeccionReporte? seccionReportesOrigen;

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
            : this(
                null,
                OrigenHistorialVentas.Modal,
                idVenta,
                tipoOrigen,
                idReferencia,
                string.Empty
            )
        {
        }


        public FormVentaDetalle(
            FormPrincipal? formPrincipal,
            OrigenHistorialVentas origenHistorial,
            int idVenta,
            TipoHistorialVentas tipoOrigen,
            int idReferencia,
            string nombreReferencia,
            bool consultaDesdeReportes = false,
            FormReportesGeneral.SeccionReporte? seccionReportesOrigen = null)
        {
            InitializeComponent();

            this.formPrincipal =
                formPrincipal;

            this.origenHistorial =
                origenHistorial;

            this.nombreReferencia =
                nombreReferencia
                ?? string.Empty;

            ventaLogica =
                new VentaLogica();

            this.idVenta =
                idVenta;

            this.tipoOrigen =
                tipoOrigen;

            this.idReferencia =
                idReferencia;

            this.consultaDesdeReportes =
                consultaDesdeReportes;

            this.seccionReportesOrigen =
                seccionReportesOrigen;


            ConfigurarGrillas();
            ConfigurarEventos();

            // La vista cambia de dos columnas a una sola antes de que el panel
            // derecho pierda ancho. El scroll es solo vertical para el contenido.
            pnlPrincipal.AutoScroll = true;
            Resize += (_, _) => AjustarLayoutResponsive();
            Load += (_, _) => AjustarLayoutResponsive();

            CargarDetalle();
        }

        private void AjustarLayoutResponsive()
        {
            if (pnlPrincipal.ClientSize.Width <= 0)
                return;

            const int margen = 24;
            const int separacion = 18;
            const int altoCabecera = 82;
            const int altoDatosAmplio = 102;
            const int altoDatosAngosto = 184;
            const int altoAcciones = 64;
            const int altoTotales = 108;
            const int anchoMinimoLateral = 285;

            int anchoDisponible = Math.Max(320, pnlPrincipal.ClientSize.Width - margen * 2);
            bool dosColumnas = anchoDisponible >= 900;
            int y = 18;

            pnlPrincipal.SuspendLayout();
            pnlCabecera.SetBounds(margen, y, anchoDisponible, altoCabecera);
            y += altoCabecera + 8;

            int altoDatos = dosColumnas ? altoDatosAmplio : altoDatosAngosto;
            pnlDatosVenta.SetBounds(margen, y, anchoDisponible, altoDatos);
            AjustarDatosVenta(anchoDisponible, dosColumnas);
            y += altoDatos + separacion;

            if (dosColumnas)
            {
                int anchoLateral = Math.Max(anchoMinimoLateral, (int)(anchoDisponible * .28));
                int anchoProductos = anchoDisponible - anchoLateral - separacion;
                int altoCuerpo = Math.Max(250, pnlPrincipal.ClientSize.Height - y - altoAcciones - separacion - 24);
                int altoPagos = Math.Max(180, altoCuerpo - altoTotales - separacion);

                pnlProductos.SetBounds(margen, y, anchoProductos, altoCuerpo);
                pnlPagos.SetBounds(margen + anchoProductos + separacion, y, anchoLateral, altoPagos);
                pnlTotales.SetBounds(margen + anchoProductos + separacion, y + altoPagos + separacion, anchoLateral, altoTotales);
                y += altoCuerpo + separacion;
            }
            else
            {
                int altoProductos = Math.Max(250, Math.Min(380, pnlPrincipal.ClientSize.Height / 2));
                int altoPagos = 190;
                pnlProductos.SetBounds(margen, y, anchoDisponible, altoProductos);
                y += altoProductos + separacion;
                pnlPagos.SetBounds(margen, y, anchoDisponible, altoPagos);
                y += altoPagos + separacion;
                pnlTotales.SetBounds(margen, y, anchoDisponible, altoTotales);
                y += altoTotales + separacion;
            }

            AjustarPanelGrilla(pnlProductos, dgvProductos);
            AjustarPanelGrilla(pnlPagos, dgvPagos);
            AjustarTotales();

            bool accionesApiladas = anchoDisponible < 380;
            int altoAccionesReal = accionesApiladas ? 108 : altoAcciones;
            pnlAcciones.SetBounds(margen, y, anchoDisponible, altoAccionesReal);
            btnCerrar.SetBounds(0, 12, Math.Min(190, anchoDisponible), 40);
            btnGenerarPdf.SetBounds(
                accionesApiladas ? 0 : Math.Max(200, anchoDisponible - 140),
                accionesApiladas ? 60 : 12,
                Math.Min(140, anchoDisponible),
                40);
            y += altoAccionesReal + 24;

            pnlPrincipal.AutoScrollMinSize = new Size(0, y);
            pnlPrincipal.ResumeLayout(true);
        }

        private void AjustarPanelGrilla(Panel panel, DataGridView grilla)
        {
            grilla.SetBounds(18, 47, Math.Max(120, panel.ClientSize.Width - 36), Math.Max(80, panel.ClientSize.Height - 66));
        }

        private void AjustarTotales()
        {
            int anchoImporte = Math.Max(115, pnlTotales.ClientSize.Width / 2 - 8);
            int izquierdaImporte = pnlTotales.ClientSize.Width - anchoImporte - 18;
            lblSubtotal.SetBounds(izquierdaImporte, 10, anchoImporte, 23);
            lblDescuento.SetBounds(izquierdaImporte, 39, anchoImporte, 23);
            lblTotal.SetBounds(izquierdaImporte, 68, anchoImporte, 28);
        }

        private void AjustarDatosVenta(int ancho, bool dosColumnas)
        {
            Label[] titulos = { lblFechaTitulo, lblSucursalTitulo, lblTipoFacturaTitulo, lblClienteTitulo, lblDocumentoTitulo, lblVendedorTitulo };
            Label[] valores = { lblFecha, lblSucursal, lblTipoFactura, lblCliente, lblDocumento, lblVendedor };
            int columnas = dosColumnas ? 6 : 2;
            int anchoColumna = Math.Max(120, (ancho - 40) / columnas);
            for (int indice = 0; indice < titulos.Length; indice++)
            {
                int fila = indice / columnas;
                int columna = indice % columnas;
                int x = 20 + columna * anchoColumna;
                int y = 16 + fila * 76;
                titulos[indice].Location = new Point(x, y);
                valores[indice].SetBounds(x, y + 24, anchoColumna - 10, 26);
                valores[indice].AutoEllipsis = true;
            }
        }

        // Abre un detalle desde Reportes y conserva la validación de alcance en VentaLogica.
        public FormVentaDetalle(
            FormPrincipal formPrincipal,
            int idVenta,
            FormReportesGeneral.SeccionReporte? seccionReportesOrigen = null)
            : this(
                formPrincipal,
                OrigenHistorialVentas.Reportes,
                idVenta,
                TipoHistorialVentas.Vendedor,
                SesionActual.IdUsuario,
                string.Empty,
                true,
                seccionReportesOrigen)
        {
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


            if (consultaDesdeReportes)
            {
                resultado = ventaLogica.ObtenerDetalleParaReporte(idVenta);
            }
            else if (tipoOrigen == TipoHistorialVentas.Vendedor)
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


            using SaveFileDialog dialogo = new SaveFileDialog
            {
                Title = "Guardar comprobante de venta",
                Filter = "Documento PDF (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                AddExtension = true,
                FileName = ComprobanteVentaPdf.NombreSugerido(ventaActual)
            };

            if (dialogo.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                ComprobanteVentaPdf.Generar(dialogo.FileName, ventaActual);

                MostrarMensaje(
                    "Comprobante generado",
                    $"El comprobante de la venta N° {ventaActual.IdVenta} se guardó correctamente."
                );
            }
            catch (Exception ex) when (
                ex is IOException
                || ex is UnauthorizedAccessException
                || ex is InvalidOperationException)
            {
                MostrarMensaje(
                    "No se pudo generar el PDF",
                    "No se pudo guardar el archivo. Verificá que la ubicación sea válida y que el archivo no esté abierto en otro programa."
                );
            }
        }


        // ========================================================
        // CERRAR
        // ========================================================

        private void BtnCerrar_Click(
            object? sender,
            EventArgs e)
        {
            if (formPrincipal == null)
            {
                Close();

                return;
            }

            if (origenHistorial == OrigenHistorialVentas.Reportes)
            {
                formPrincipal.AbrirFormularioEnPanel(
                    new FormReportesGeneral(formPrincipal, seccionReportesOrigen),
                    formPrincipal.BotonReportes
                );
                return;
            }


            Button botonOrigen =
                origenHistorial == OrigenHistorialVentas.Ventas
                    ? formPrincipal.BotonVentas
                    : origenHistorial == OrigenHistorialVentas.Clientes
                        ? formPrincipal.BotonClientes
                        : formPrincipal.BotonUsuarios;


            formPrincipal.AbrirFormularioEnPanel(
                new FormListadoVentas(
                    formPrincipal,
                    origenHistorial,
                    tipoOrigen,
                    idReferencia,
                    nombreReferencia
                ),
                botonOrigen
            );
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
                (Form?)formPrincipal ?? this
            );
        }
    }
}
