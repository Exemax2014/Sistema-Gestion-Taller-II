using Capa_Logica;
using System.ComponentModel;
using System.Globalization;

namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormVentas
    //
    // Flujo:
    // 1. Buscar productos y armar la venta.
    // 2. Seleccionar cliente y cargar pagos.
    // 3. Confirmar mediante VentaLogica.
    //
    // La Vista NO registra directamente en la base de datos.
    // ============================================================

    public partial class FormVentas : Form, IControlaCambios
    {
        private readonly VentaLogica ventaLogica;
        // La venta se hospeda dentro de esta instancia; conservarla evita depender
        // de la jerarquía de controles para abrir diálogos modales.
        private readonly FormPrincipal formPrincipal;

        private readonly BindingList<ItemVentaVista> itemsVenta;
        private readonly BindingList<PagoVentaVista> pagosVenta;
        private const int MaximoProductosResumen = 4;
        private readonly TableLayoutPanel tlpResumenProductos = new()
        {
            ColumnCount = 2,
            RowCount = 0,
            BackColor = Color.White,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        private readonly Label lblResumenProductosRestantes = new()
        {
            AutoSize = false,
            Font = new Font("Segoe UI", 8F),
            ForeColor = Color.FromArgb(100, 108, 116),
            TextAlign = ContentAlignment.MiddleLeft,
            Visible = false
        };
        private readonly ToolTip tooltipResumenProductos = new();

        private ClienteVentaModelo? clienteSeleccionado;
        private List<ClienteVentaModelo> clientesDisponibles = new();
        private bool actualizandoSelectorCliente;
        private readonly Button btnNuevoCliente = new();

        // Informa a FormPrincipal si abandonar el módulo descartaría una venta
        // todavía no confirmada. El estado existe únicamente en memoria.
        public bool PuedeCerrar()
        {
            if (itemsVenta.Count == 0 && pagosVenta.Count == 0 && clienteSeleccionado == null)
                return true;

            using FormMensaje confirmacion = new(
                "Venta en curso",
                "Hay una venta en curso. Si salís, los datos cargados se perderán. ¿Deseás continuar?",
                "Sí, salir",
                true);

            return confirmacion.ShowDialog(formPrincipal) == DialogResult.OK;
        }

        public FormVentas(
            FormPrincipal formPrincipal)
        {
            ArgumentNullException.ThrowIfNull(formPrincipal);
            InitializeComponent();

            this.formPrincipal = formPrincipal;

            ventaLogica =
                new VentaLogica();

            itemsVenta =
                new BindingList<ItemVentaVista>();

            pagosVenta =
                new BindingList<PagoVentaVista>();

            ConfigurarListaResumenProductos();
            ConfigurarGrillas();
            ConfigurarAccionNuevoCliente();
            ConfigurarEventos();
            ConfigurarValidacionesVisuales();
            ConfigurarContexto();
            ConfigurarPermisos();

            CargarMetodosPago();
            ReiniciarVenta();
            AjustarLayoutResponsive();

            if (
                ventaLogica.PuedeRealizarVentas()
                &&
                !SesionActual.IdSucursalOperativa.HasValue)
            {
                MostrarAvisoSucursalOperativa();
            }
        }


        // ========================================================
        // CONFIGURACIÓN GENERAL
        // ========================================================

        private void ConfigurarContexto()
        {
            lblVendedorValor.Text =
                $"{SesionActual.Nombre} {SesionActual.Apellido}"
                    .Trim();

            lblSucursalValor.Text =
                string.IsNullOrWhiteSpace(
                    SesionActual.SucursalOperativa)
                    ? "-"
                    : SesionActual.SucursalOperativa;

            lblFechaValor.Text =
                DateTime.Now.ToString(
                    "dd/MM/yyyy HH:mm"
                );
        }

        // Redistribuye la cabecera y el flujo de productos según el espacio real del panel embebido.
        private void AjustarLayoutResponsive()
        {
            if (pnlPrincipal.ClientSize.Width <= 0 || pnlPrincipal.ClientSize.Height <= 0) return;

            pnlPrincipal.AutoScroll = true;
            int margen = 32;
            int ancho = Math.Max(320, pnlPrincipal.ClientSize.Width - margen * 2);
            int yContexto = 120;
            int altoContexto = ancho < 620 ? 112 : 66;
            int yContenido = yContexto + altoContexto + 14;
            int altoVisible = Math.Max(500, pnlPrincipal.ClientSize.Height - yContenido - 24);

            pnlCabecera.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlContexto.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlPasoProductos.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlPasoFinalizar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlCabecera.SetBounds(margen, 20, ancho, 100);
            btnMisVentas.SetBounds(Math.Max(0, ancho - btnMisVentas.Width - 12), 12, btnMisVentas.Width, btnMisVentas.Height);
            pnlContexto.SetBounds(margen, yContexto, ancho, altoContexto);
            AjustarDatosContexto(ancho, altoContexto);

            pnlPasoProductos.SetBounds(margen, yContenido, ancho, altoVisible);
            pnlPasoFinalizar.SetBounds(margen, yContenido, ancho, altoVisible);
            if (ancho >= 1000)
                AjustarDosColumnasProductos(ancho, altoVisible);
            else
                AjustarProductosApilados(ancho);

            if (pnlPasoFinalizar.Visible)
                AjustarPasoFinalizar(ancho, altoVisible);

            Panel pasoActivo = pnlPasoFinalizar.Visible ? pnlPasoFinalizar : pnlPasoProductos;
            pnlPrincipal.AutoScrollMinSize = new Size(0, yContenido + pasoActivo.Height + 24);
        }

        // Da al segundo paso el mismo comportamiento adaptable que al carrito:
        // cliente arriba, pagos y resumen en columnas o apilados según el ancho.
        private void AjustarPasoFinalizar(int ancho, int alto)
        {
            int interior = Math.Max(260, ancho - 36);
            bool apilado = ancho < 700;
            bool botonesEnFila = ancho >= 480;
            int altoCliente = !apilado ? 126 : botonesEnFila ? 172 : 214;
            pnlCliente.SetBounds(0, 0, ancho, altoCliente);
            lblPasoFinalizar.SetBounds(18, 12, interior, 28);
            if (!apilado)
            {
                int anchoSelector = Math.Max(180, interior - 140);
                cmbClienteResultados.SetBounds(18, 62, anchoSelector, 32);
                btnNuevoCliente.SetBounds(cmbClienteResultados.Right + 10, 61, 130, 34);
            }
            else
            {
                if (botonesEnFila)
                {
                    int anchoSelector = Math.Max(160, interior - 140);
                    cmbClienteResultados.SetBounds(18, 62, anchoSelector, 32);
                    btnNuevoCliente.SetBounds(cmbClienteResultados.Right + 10, 61, 130, 34);
                }
                else
                {
                    cmbClienteResultados.SetBounds(18, 62, interior, 32);
                    btnNuevoCliente.SetBounds(18, 108, interior, 34);
                }
            }

            int y = pnlCliente.Bottom + 14;
            bool columnas = ancho >= 900;
            int altoPanel = columnas ? Math.Max(330, alto - y) : 390;
            if (columnas)
            {
                int pagos = (ancho - 14) * 3 / 5;
                pnlPagos.SetBounds(0, y, pagos, altoPanel);
                pnlResumen.SetBounds(pagos + 14, y, ancho - pagos - 14, altoPanel);
            }
            else
            {
                pnlPagos.SetBounds(0, y, ancho, 360);
                pnlResumen.SetBounds(0, y + 374, ancho, 390);
                pnlPasoFinalizar.Height = 778;
            }

            AjustarControlesPagos();
            AjustarControlesResumen();
        }

        private void AjustarControlesPagos()
        {
            int ancho = Math.Max(220, pnlPagos.ClientSize.Width - 36);
            int combo = Math.Min(190, Math.Max(130, ancho / 4));
            int monto = Math.Min(130, Math.Max(90, ancho / 6));
            cmbMetodoPago.SetBounds(18, 82, combo, 28);
            lblMontoPago.SetBounds(cmbMetodoPago.Right + 14, 57, monto, 20);
            txtMontoPago.SetBounds(cmbMetodoPago.Right + 14, 82, monto, 27);
            btnCompletarSaldo.SetBounds(txtMontoPago.Right + 12, 79, Math.Min(128, Math.Max(92, ancho - txtMontoPago.Right - 18 - 115)), 34);
            btnAgregarPago.SetBounds(Math.Max(btnCompletarSaldo.Right + 8, pnlPagos.ClientSize.Width - 155 - 18), 79, 155, 34);
            dgvPagos.SetBounds(18, 130, ancho, Math.Max(120, pnlPagos.ClientSize.Height - 151));
        }

        private void AjustarControlesResumen()
        {
            int ancho = Math.Max(220, pnlResumen.ClientSize.Width - 36);
            int xValor = Math.Max(140, pnlResumen.ClientSize.Width - Math.Min(190, ancho / 2) - 18);
            int anchoValor = Math.Max(100, pnlResumen.ClientSize.Width - xValor - 18);
            int yBotones = pnlResumen.ClientSize.Height - 64;
            int yTotales = Math.Max(158, yBotones - 92);
            int filasProductos = ObtenerCantidadProductosResumenVisibles(yTotales);
            int altoLista = filasProductos * 20;
            int yLista = 82;
            bool hayRestantes = itemsVenta.Count > filasProductos;
            int yCliente = yLista + altoLista + (hayRestantes ? 28 : 10);

            lblResumenProductosTitulo.SetBounds(18, 59, 88, 20);
            lblResumenProductos.SetBounds(110, 59, Math.Max(100, ancho - 92), 20);
            tlpResumenProductos.SetBounds(18, yLista, ancho, altoLista);
            lblResumenProductosRestantes.SetBounds(18, yLista + altoLista, ancho, 20);
            lblResumenClienteTitulo.SetBounds(18, yCliente, ancho, 20);
            lblResumenCliente.SetBounds(18, yCliente + 20, ancho, 20);

            lblTotalVentaTitulo.SetBounds(18, yTotales, xValor - 24, 27);
            lblTotalVenta.SetBounds(xValor, yTotales, anchoValor, 27);
            lblTotalPagadoTitulo.SetBounds(18, yTotales + 30, xValor - 24, 27);
            lblTotalPagado.SetBounds(xValor, yTotales + 30, anchoValor, 27);
            lblSaldoTitulo.SetBounds(18, yTotales + 60, xValor - 24, 32);
            lblSaldo.SetBounds(xValor, yTotales + 60, anchoValor, 32);
            btnVolverProductos.SetBounds(18, yBotones, 120, 42);
            btnCancelarVentaPaso2.SetBounds(146, yBotones, 120, 42);
            btnConfirmarVenta.SetBounds(Math.Max(274, pnlResumen.ClientSize.Width - 168 - 18), yBotones, 168, 42);
            ActualizarProductosResumen(filasProductos);
        }

        private void ConfigurarListaResumenProductos()
        {
            tlpResumenProductos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpResumenProductos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 48F));
            pnlResumen.Controls.Add(tlpResumenProductos);
            pnlResumen.Controls.Add(lblResumenProductosRestantes);

            lblResumenProductos.AutoSize = false;
            lblResumenProductos.Font = new Font("Segoe UI", 8F);
            lblResumenProductos.ForeColor = Color.FromArgb(100, 108, 116);
            lblResumenProductos.TextAlign = ContentAlignment.MiddleRight;
            lblResumenCliente.AutoSize = false;
            lblResumenCliente.AutoEllipsis = true;
            lblResumenCliente.TextAlign = ContentAlignment.MiddleLeft;
        }

        private int ObtenerCantidadProductosResumenVisibles(int yTotales)
        {
            const int yLista = 82;
            const int altoFila = 20;
            const int reservaCliente = 56;
            int altoDisponible = Math.Max(altoFila, yTotales - reservaCliente - yLista);
            int filasDisponibles = Math.Max(1, altoDisponible / altoFila);
            return Math.Min(MaximoProductosResumen, Math.Min(itemsVenta.Count, filasDisponibles));
        }

        private void ActualizarProductosResumen(int cantidadVisible)
        {
            tlpResumenProductos.SuspendLayout();
            tlpResumenProductos.Controls.Clear();
            tlpResumenProductos.RowStyles.Clear();
            tlpResumenProductos.RowCount = cantidadVisible;

            foreach (ItemVentaVista item in itemsVenta.Take(cantidadVisible))
            {
                int fila = tlpResumenProductos.RowStyles.Count;
                tlpResumenProductos.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));

                Label nombre = new()
                {
                    AutoSize = false,
                    AutoEllipsis = true,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 8.5F),
                    Text = item.Producto,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                Label cantidad = new()
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(80, 85, 90),
                    Text = $"x{item.Cantidad}",
                    TextAlign = ContentAlignment.MiddleRight
                };

                tooltipResumenProductos.SetToolTip(nombre, item.Producto);
                tlpResumenProductos.Controls.Add(nombre, 0, fila);
                tlpResumenProductos.Controls.Add(cantidad, 1, fila);
            }

            int restantes = itemsVenta.Count - cantidadVisible;
            lblResumenProductosRestantes.Visible = restantes > 0;
            lblResumenProductosRestantes.Text = restantes > 0
                ? $"+ {restantes} producto{(restantes == 1 ? string.Empty : "s")} más"
                : string.Empty;
            tlpResumenProductos.ResumeLayout(true);
        }

        private void ConfigurarAccionNuevoCliente()
        {
            btnNuevoCliente.Text = "Nuevo cliente";
            btnNuevoCliente.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnNuevoCliente.Cursor = Cursors.Hand;
            btnNuevoCliente.BackColor = Color.White;
            btnNuevoCliente.ForeColor = Color.FromArgb(37, 116, 222);
            btnNuevoCliente.FlatStyle = FlatStyle.Flat;
            btnNuevoCliente.FlatAppearance.BorderColor = Color.FromArgb(37, 116, 222);
            btnNuevoCliente.Visible = SesionActual.TienePermiso("CLIENTES_ALTA");
            btnNuevoCliente.Click += BtnNuevoCliente_Click;
            pnlCliente.Controls.Remove(txtBuscarCliente);
            pnlCliente.Controls.Remove(btnBuscarCliente);
            pnlCliente.Controls.Remove(lblBuscarCliente);
            pnlCliente.Controls.Remove(lblClienteSeleccionadoTitulo);
            pnlCliente.Controls.Remove(lblClienteSeleccionado);
            cmbClienteResultados.DropDownStyle = ComboBoxStyle.DropDown;
            cmbClienteResultados.AutoCompleteMode = AutoCompleteMode.None;
            cmbClienteResultados.IntegralHeight = false;
            cmbClienteResultados.ItemHeight = 24;
            cmbClienteResultados.MaxDropDownItems = 6;
            cmbClienteResultados.Text = "Escribí nombre, apellido o documento...";
            pnlCliente.Controls.Add(btnNuevoCliente);
        }

        // Reubica vendedor, sucursal y fecha en tres columnas o en filas compactas.
        private void AjustarDatosContexto(int ancho, int alto)
        {
            if (ancho >= 620)
            {
                int columna = (ancho - 36) / 3;
                int[] posiciones = { 18, 18 + columna, 18 + columna * 2 };
                Label[] titulos = { lblVendedorTitulo, lblSucursalTitulo, lblFechaTitulo };
                Label[] valores = { lblVendedorValor, lblSucursalValor, lblFechaValor };
                for (int i = 0; i < titulos.Length; i++)
                {
                    titulos[i].SetBounds(posiciones[i], 8, columna - 12, 20);
                    valores[i].SetBounds(posiciones[i], 32, columna - 12, 24);
                }
            }
            else
            {
                Label[] titulos = { lblVendedorTitulo, lblSucursalTitulo, lblFechaTitulo };
                Label[] valores = { lblVendedorValor, lblSucursalValor, lblFechaValor };
                for (int i = 0; i < titulos.Length; i++)
                {
                    titulos[i].SetBounds(14, 5 + i * 34, 82, 22);
                    valores[i].SetBounds(100, 5 + i * 34, Math.Max(150, ancho - 116), 22);
                }
            }
        }

        // Distribuye búsqueda y carrito en dos columnas equilibradas en anchos grandes.
        private void AjustarDosColumnasProductos(int ancho, int alto)
        {
            const int separacion = 16;
            int anchoIzquierdo = (ancho - separacion) / 2;
            int anchoDerecho = ancho - separacion - anchoIzquierdo;
            pnlBuscarProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlBuscarProductos.SetBounds(0, 0, anchoIzquierdo, alto);
            pnlCarrito.SetBounds(anchoIzquierdo + separacion, 0, anchoDerecho, alto);
            AjustarControlesBusqueda(anchoIzquierdo, alto, false);
            AjustarControlesCarrito(anchoDerecho, alto);
        }

        // Apila los paneles en ventanas angostas para evitar recortes y desplazamiento horizontal.
        private void AjustarProductosApilados(int ancho)
        {
            int altoBusqueda = 440;
            int altoCarrito = 470;
            pnlBuscarProductos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlBuscarProductos.SetBounds(0, 0, ancho, altoBusqueda);
            pnlCarrito.SetBounds(0, altoBusqueda + 14, ancho, altoCarrito);
            pnlPasoProductos.Height = altoBusqueda + 14 + altoCarrito;
            AjustarControlesBusqueda(ancho, altoBusqueda, true);
            AjustarControlesCarrito(ancho, altoCarrito);
        }

        // Ajusta los campos de búsqueda y la grilla al ancho de su panel sin alterar el filtrado.
        private void AjustarControlesBusqueda(int ancho, int alto, bool apilado)
        {
            int anchoInterior = Math.Max(120, ancho - 36);
            lblPasoProductos.SetBounds(18, 14, anchoInterior, 28);
            lblBuscarProducto.SetBounds(18, 55, anchoInterior, 20);
            bool controlesEnFila = ancho >= 690 && !apilado;
            if (controlesEnFila)
            {
                int anchoTexto = Math.Max(150, anchoInterior - 112 - 10 - 132 - 8);
                txtBuscarProducto.SetBounds(18, 79, anchoTexto, 27);
                btnBuscarProducto.SetBounds(18 + anchoTexto + 10, 76, 112, 34);
                btnMostrarTodos.SetBounds(18 + anchoTexto + 130, 76, 132, 34);
                lblAyudaBusqueda.SetBounds(18, 116, anchoInterior, 20);
                dgvProductos.SetBounds(18, 144, anchoInterior, Math.Max(120, alto - 162));
            }
            else
            {
                txtBuscarProducto.SetBounds(18, 79, anchoInterior, 27);
                int anchoBotonMostrar = Math.Min(132, Math.Max(112, (anchoInterior - 8 - 96) / 2));
                btnBuscarProducto.SetBounds(18, 114, 96, 34);
                btnMostrarTodos.SetBounds(18 + 96 + 8, 114, anchoBotonMostrar, 34);
                lblAyudaBusqueda.SetBounds(18, 153, anchoInterior, 34);
                dgvProductos.SetBounds(18, 190, anchoInterior, Math.Max(120, alto - 208));
            }
            dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        // Mantiene grilla, total y acciones del carrito alineados a sus márgenes internos.
        private void AjustarControlesCarrito(int ancho, int alto)
        {
            int anchoInterior = Math.Max(120, ancho - 36);
            lblCarrito.SetBounds(18, 14, anchoInterior, 26);
            lblCarritoAyuda.SetBounds(18, 46, anchoInterior, 22);
            dgvCarrito.SetBounds(18, 76, anchoInterior, Math.Max(120, alto - 200));
            lblTotalCarritoTitulo.SetBounds(18, alto - 96, Math.Max(120, anchoInterior / 2), 28);
            lblTotalCarrito.SetBounds(Math.Max(150, ancho - 218), alto - 100, Math.Max(100, anchoInterior / 2), 34);
            int anchoCancelar = Math.Min(145, Math.Max(120, (anchoInterior - 12) / 2));
            int anchoContinuar = Math.Min(160, Math.Max(120, anchoInterior - anchoCancelar - 12));
            btnCancelarVentaPaso1.SetBounds(18, alto - 62, anchoCancelar, 42);
            btnContinuar.SetBounds(ancho - 18 - anchoContinuar, alto - 62, anchoContinuar, 42);
            dgvCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }


        private void ConfigurarPermisos()
        {
            bool puedeRealizar =
                ventaLogica.PuedeRealizarVentas();

            bool tieneSucursal =
                SesionActual.IdSucursalOperativa.HasValue;


            pnlPasoProductos.Enabled =
                puedeRealizar
                &&
                tieneSucursal;

            pnlPasoFinalizar.Enabled =
                puedeRealizar
                &&
                tieneSucursal;

            btnMisVentas.Visible =
                ventaLogica.PuedeVerVentas();


            if (!puedeRealizar)
            {
                lblSubtitulo.Text =
                    "No tiene permiso para registrar ventas.";

                return;
            }


            if (!tieneSucursal)
            {
                lblSubtitulo.Text =
                    "Seleccione una sucursal específica para comenzar una venta.";
            }
        }


        // ========================================================
        // GRILLAS
        // ========================================================

        private void ConfigurarGrillas()
        {
            ConfigurarGrillaProductos();
            ConfigurarGrillaCarrito();
            ConfigurarGrillaPagos();
        }


        private void ConfigurarGrillaProductos()
        {
            dgvProductos.AutoGenerateColumns =
                false;

            dgvProductos.ReadOnly =
                false;

            dgvProductos.Columns.Clear();


            DataGridViewTextBoxColumn colCodigo =
                new DataGridViewTextBoxColumn
                {
                    Name = "colProductoCodigo",
                    DataPropertyName = "CodigoBarra",
                    HeaderText = "Código",
                    Width = 110,
                    ReadOnly = true
                };


            DataGridViewTextBoxColumn colProducto =
                new DataGridViewTextBoxColumn
                {
                    Name = "colProductoNombre",
                    DataPropertyName = "Nombre",
                    HeaderText = "Producto",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 180,
                    ReadOnly = true
                };


            DataGridViewTextBoxColumn colPrecio =
                new DataGridViewTextBoxColumn
                {
                    Name = "colProductoPrecio",
                    DataPropertyName = "PrecioVenta",
                    HeaderText = "Precio",
                    Width = 100,
                    ReadOnly = true,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            DataGridViewTextBoxColumn colStock =
                new DataGridViewTextBoxColumn
                {
                    Name = "colProductoStock",
                    DataPropertyName = "Stock",
                    HeaderText = "Stock",
                    Width = 70,
                    ReadOnly = true
                };


            DataGridViewTextBoxColumn colCantidad =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCantidadAgregar",
                    HeaderText = "Cant.",
                    Width = 65,
                    ReadOnly = false,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Alignment =
                                DataGridViewContentAlignment.MiddleCenter
                        }
                };


            DataGridViewButtonColumn colAgregar =
                new DataGridViewButtonColumn
                {
                    Name = "colAgregarProducto",
                    HeaderText = "",
                    Text = "Agregar",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 90,
                    ReadOnly = true
                };


            dgvProductos.Columns.AddRange(
                colCodigo,
                colProducto,
                colPrecio,
                colStock,
                colCantidad,
                colAgregar
            );
            DataGridViewLayoutHelper.Normalizar(dgvProductos, "colProductoNombre");
        }


        private void ConfigurarGrillaCarrito()
        {
            dgvCarrito.AutoGenerateColumns =
                false;

            dgvCarrito.Columns.Clear();


            DataGridViewTextBoxColumn colProducto =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCarritoProducto",
                    DataPropertyName = "Producto",
                    HeaderText = "Producto",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 150
                };


            DataGridViewTextBoxColumn colCantidad =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCarritoCantidad",
                    DataPropertyName = "Cantidad",
                    HeaderText = "Cant.",
                    Width = 55
                };


            DataGridViewTextBoxColumn colPrecio =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCarritoPrecio",
                    DataPropertyName = "PrecioUnitario",
                    HeaderText = "Precio",
                    Width = 95,

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
                    Name = "colCarritoSubtotal",
                    DataPropertyName = "Subtotal",
                    HeaderText = "Subtotal",
                    Width = 105,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            DataGridViewButtonColumn colQuitar =
                new DataGridViewButtonColumn
                {
                    Name = "colQuitarProducto",
                    HeaderText = "",
                    Text = "Quitar",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 70
                };


            dgvCarrito.Columns.AddRange(
                colProducto,
                colCantidad,
                colPrecio,
                colSubtotal,
                colQuitar
            );
            DataGridViewLayoutHelper.Normalizar(dgvCarrito, "colCarritoProducto");


            dgvCarrito.DataSource =
                itemsVenta;
        }


        private void ConfigurarGrillaPagos()
        {
            dgvPagos.AutoGenerateColumns =
                false;

            dgvPagos.Columns.Clear();


            DataGridViewTextBoxColumn colMetodo =
                new DataGridViewTextBoxColumn
                {
                    Name = "colPagoMetodo",
                    DataPropertyName = "MetodoPago",
                    HeaderText = "Método",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 180
                };


            DataGridViewTextBoxColumn colMonto =
                new DataGridViewTextBoxColumn
                {
                    Name = "colPagoMonto",
                    DataPropertyName = "Monto",
                    HeaderText = "Monto",
                    Width = 130,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            DataGridViewButtonColumn colQuitar =
                new DataGridViewButtonColumn
                {
                    Name = "colQuitarPago",
                    HeaderText = "",
                    Text = "Quitar",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 80
                };


            dgvPagos.Columns.AddRange(
                colMetodo,
                colMonto,
                colQuitar
            );
            DataGridViewLayoutHelper.Normalizar(dgvPagos, "colPagoMetodo");


            dgvPagos.DataSource =
                pagosVenta;
        }


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            pnlPrincipal.Resize += (_, _) => AjustarLayoutResponsive();

            btnBuscarProducto.Click +=
                BtnBuscarProducto_Click;

            btnMostrarTodos.Click +=
                BtnMostrarTodos_Click;

            txtBuscarProducto.KeyDown +=
                TxtBuscarProducto_KeyDown;

            dgvProductos.CellContentClick +=
                DgvProductos_CellContentClick;

            dgvProductos.EditingControlShowing +=
                DgvProductos_EditingControlShowing;

            dgvProductos.CellValidating +=
                DgvProductos_CellValidating;

            dgvProductos.DataBindingComplete +=
                DgvProductos_DataBindingComplete;


            dgvCarrito.CellContentClick +=
                DgvCarrito_CellContentClick;


            btnContinuar.Click +=
                BtnContinuar_Click;

            btnCancelarVentaPaso1.Click +=
                BtnCancelarVenta_Click;


            cmbClienteResultados.SelectedIndexChanged +=
                CmbClienteResultados_SelectedIndexChanged;
            cmbClienteResultados.TextUpdate += CmbClienteResultados_TextUpdate;


            txtMontoPago.KeyPress +=
                TxtMontoPago_KeyPress;

            btnCompletarSaldo.Click +=
                BtnCompletarSaldo_Click;

            btnAgregarPago.Click +=
                BtnAgregarPago_Click;

            dgvPagos.CellContentClick +=
                DgvPagos_CellContentClick;


            btnVolverProductos.Click +=
                BtnVolverProductos_Click;

            btnCancelarVentaPaso2.Click +=
                BtnCancelarVenta_Click;

            btnConfirmarVenta.Click +=
                BtnConfirmarVenta_Click;


            btnMisVentas.Click +=
                BtnMisVentas_Click;
        }


        // Configura límites preventivos para el importe sin reemplazar la validación de VentaLogica.
        private void ConfigurarValidacionesVisuales()
        {
            txtMontoPago.MaxLength = 19;
        }


        // ========================================================
        // PRODUCTOS
        // ========================================================

        // Carga productos de la sucursal operativa y evita iniciar la venta
        // en un contexto global que no permite descontar stock.
        private void CargarProductos(
            string texto)
        {
            if (!ValidarSucursalOperativa())
            {
                dgvProductos.DataSource =
                    null;

                return;
            }


            try
            {
                List<ProductoVentaModelo> productos =
                    ventaLogica
                        .BuscarProductosParaVenta(
                            texto.Trim()
                        );


                dgvProductos.DataSource =
                    productos;
            }
            catch (Exception ex)
            {
                dgvProductos.DataSource =
                    null;

                MostrarMensaje(
                    "No se pudieron cargar los productos",
                    ex.Message
                );
            }
        }


        private void BtnBuscarProducto_Click(
            object? sender,
            EventArgs e)
        {
            CargarProductos(
                txtBuscarProducto.Text
            );
        }


        private void BtnMostrarTodos_Click(
            object? sender,
            EventArgs e)
        {
            txtBuscarProducto.Clear();

            CargarProductos(
                string.Empty
            );
        }


        private void TxtBuscarProducto_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }


            CargarProductos(
                txtBuscarProducto.Text
            );

            e.SuppressKeyPress =
                true;
        }


        private void DgvProductos_DataBindingComplete(
            object? sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            foreach (
                DataGridViewRow fila
                in dgvProductos.Rows)
            {
                fila.Cells["colCantidadAgregar"].Value =
                    1;
            }
        }


        private void DgvProductos_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (
                e.RowIndex < 0
                ||
                e.ColumnIndex < 0)
            {
                return;
            }


            if (
                dgvProductos
                    .Columns[e.ColumnIndex]
                    .Name
                !=
                "colAgregarProducto")
            {
                return;
            }


            if (
                dgvProductos
                    .Rows[e.RowIndex]
                    .DataBoundItem
                is not ProductoVentaModelo producto)
            {
                return;
            }


            AgregarProducto(
                producto,
                dgvProductos.Rows[e.RowIndex]
            );
        }


        // Agrega productos al carrito solo cuando existe una sucursal concreta
        // que permite validar stock y registrar la futura venta.
        private void AgregarProducto(
            ProductoVentaModelo producto,
            DataGridViewRow fila)
        {
            if (!ValidarSucursalOperativa())
            {
                return;
            }


            if (producto.Stock <= 0)
            {
                MostrarMensaje(
                    "Sin stock",
                    $"El producto {producto.Nombre} no tiene stock disponible."
                );

                return;
            }


            if (
                !int.TryParse(
                    Convert.ToString(
                        fila.Cells["colCantidadAgregar"].Value
                    ),
                    out int cantidad)
                ||
                cantidad <= 0)
            {
                MostrarMensaje(
                    "Cantidad inválida",
                    "Ingrese una cantidad mayor que cero."
                );

                return;
            }


            ItemVentaVista? existente =
                itemsVenta
                    .FirstOrDefault(
                        item =>
                            item.IdProducto
                            ==
                            producto.IdProducto
                    );


            int cantidadActual =
                existente?.Cantidad
                ??
                0;

            int nuevaCantidad =
                cantidadActual
                +
                cantidad;


            if (nuevaCantidad > producto.Stock)
            {
                MostrarMensaje(
                    "Stock insuficiente",
                    $"Solo hay {producto.Stock} unidad(es) disponibles de {producto.Nombre}. " +
                    $"Ya agregó {cantidadActual} a la venta."
                );

                return;
            }


            if (existente != null)
            {
                existente.Cantidad =
                    nuevaCantidad;

                dgvCarrito.Refresh();
            }
            else
            {
                itemsVenta.Add(
                    new ItemVentaVista
                    {
                        IdProducto =
                            producto.IdProducto,

                        CodigoBarra =
                            producto.CodigoBarra,

                        Producto =
                            producto.Nombre,

                        Cantidad =
                            cantidad,

                        PrecioUnitario =
                            producto.PrecioVenta,

                        StockDisponible =
                            producto.Stock
                    }
                );
            }


            fila.Cells["colCantidadAgregar"].Value =
                1;


            ActualizarResumen();
        }


        // ========================================================
        // VALIDACIÓN PREVENTIVA DE CANTIDAD
        // ========================================================

        private void DgvProductos_EditingControlShowing(
            object? sender,
            DataGridViewEditingControlShowingEventArgs e)
        {
            if (
                dgvProductos.CurrentCell
                ==
                null
                ||
                dgvProductos
                    .Columns[
                        dgvProductos.CurrentCell.ColumnIndex
                    ]
                    .Name
                !=
                "colCantidadAgregar")
            {
                return;
            }


            if (e.Control is TextBox caja)
            {
                caja.KeyPress -=
                    Cantidad_KeyPress;

                caja.KeyPress +=
                    Cantidad_KeyPress;
            }
        }


        // Permite solo enteros durante la edición; la cantidad positiva se confirma al validar la celda.
        private void Cantidad_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (
                char.IsControl(
                    e.KeyChar
                )
                ||
                char.IsDigit(
                    e.KeyChar
                ))
            {
                return;
            }


            e.Handled =
                true;
        }


        // Mantiene el valor editado y muestra el motivo para que no se reemplace silenciosamente.
        private void DgvProductos_CellValidating(
            object? sender,
            DataGridViewCellValidatingEventArgs e)
        {
            if (
                e.RowIndex < 0
                ||
                dgvProductos
                    .Columns[e.ColumnIndex]
                    .Name
                !=
                "colCantidadAgregar")
            {
                return;
            }


            if (
                dgvProductos
                    .Rows[e.RowIndex]
                    .DataBoundItem
                is not ProductoVentaModelo producto)
            {
                return;
            }


            if (
                !int.TryParse(
                    Convert.ToString(
                        e.FormattedValue
                    ),
                    out int cantidad)
                ||
                cantidad <= 0
                ||
                cantidad > producto.Stock)
            {
                string mensaje = producto.Stock <= 0
                    ? "El producto no tiene stock disponible."
                    : cantidad <= 0
                        ? "Ingresá una cantidad entera mayor que cero."
                        : $"La cantidad no puede superar el stock disponible ({producto.Stock}).";

                MostrarMensaje("Cantidad inválida", mensaje);
                e.Cancel = true;
            }
        }


        // ========================================================
        // CARRITO
        // ========================================================

        private void DgvCarrito_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (
                e.RowIndex < 0
                ||
                e.ColumnIndex < 0)
            {
                return;
            }


            if (
                dgvCarrito
                    .Columns[e.ColumnIndex]
                    .Name
                !=
                "colQuitarProducto")
            {
                return;
            }


            if (
                e.RowIndex
                <
                itemsVenta.Count)
            {
                itemsVenta.RemoveAt(
                    e.RowIndex
                );

                ActualizarResumen();
            }
        }


        // Impide avanzar al paso de cliente y pagos si la venta no tiene
        // una sucursal operativa específica asociada.
        private void BtnContinuar_Click(
            object? sender,
            EventArgs e)
        {
            if (!ValidarSucursalOperativa())
            {
                return;
            }


            if (itemsVenta.Count == 0)
            {
                MostrarMensaje(
                    "Venta incompleta",
                    "Debe agregar al menos un producto antes de continuar."
                );

                return;
            }


            MostrarPasoFinalizar();
        }


        private void MostrarPasoProductos()
        {
            pnlPasoFinalizar.Visible =
                false;

            pnlPasoProductos.Visible =
                true;

            lblSubtitulo.Text =
                "Buscá productos, armá la venta y luego completá cliente y pagos.";

            txtBuscarProducto.Focus();
        }


        private void MostrarPasoFinalizar()
        {
            pnlPasoProductos.Visible =
                false;

            pnlPasoFinalizar.Visible =
                true;

            AjustarLayoutResponsive();

            lblSubtitulo.Text =
                "Seleccioná el cliente, completá los pagos y confirmá la operación.";

            ActualizarResumen();

            if (clientesDisponibles.Count == 0)
                CargarClientesDisponibles();
            cmbClienteResultados.Focus();
        }


        // ========================================================
        // CLIENTE
        // ========================================================

        // Texto vacío: lista todos los clientes activos para elegir.
        // mostrarAvisos=false evita mensajes al cargar el listado inicial.
        private void CargarClientesDisponibles()
        {
            const string texto = "";


            try
            {
                List<ClienteVentaModelo> clientes =
                    ventaLogica
                        .BuscarClientesParaVenta(
                            texto
                        );

                clientesDisponibles = clientes;


                cmbClienteResultados.DataSource =
                    null;

                clienteSeleccionado =
                    null;

                if (clientes.Count == 0)
                {
                    ActualizarResumen();

                    return;
                }


                cmbClienteResultados.DisplayMember =
                    nameof(
                        ClienteVentaModelo.Etiqueta
                    );

                cmbClienteResultados.ValueMember =
                    nameof(
                        ClienteVentaModelo.IdCliente
                    );

                cmbClienteResultados.DataSource =
                    clientes;

                AjustarAlturaDesplegableClientes(clientes.Count);


                // Con varios resultados el usuario elige; con uno solo se
                // selecciona automáticamente.
                cmbClienteResultados.SelectedIndex =
                    clientes.Count == 1
                        ? 0
                        : -1;

            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudieron buscar clientes",
                    ex.Message
                );
            }
        }


        private void CmbClienteResultados_TextUpdate(object? sender, EventArgs e)
        {
            if (actualizandoSelectorCliente) return;
            string texto = cmbClienteResultados.Text.Trim();
            List<ClienteVentaModelo> coincidencias = clientesDisponibles
                .Where(cliente => cliente.Nombre.Contains(texto, StringComparison.CurrentCultureIgnoreCase)
                    || cliente.Documento.Contains(texto, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
            ActualizarOpcionesCliente(coincidencias, texto);
        }

        private void ActualizarOpcionesCliente(List<ClienteVentaModelo> opciones, string texto)
        {
            actualizandoSelectorCliente = true;
            cmbClienteResultados.DataSource = null;
            cmbClienteResultados.DisplayMember = nameof(ClienteVentaModelo.Etiqueta);
            cmbClienteResultados.ValueMember = nameof(ClienteVentaModelo.IdCliente);
            cmbClienteResultados.DataSource = opciones;
            cmbClienteResultados.Text = texto;
            cmbClienteResultados.SelectionStart = texto.Length;
            actualizandoSelectorCliente = false;
            AjustarAlturaDesplegableClientes(opciones.Count);
            if (opciones.Count > 0 && texto.Length > 0)
                cmbClienteResultados.DroppedDown = true;
            else
                cmbClienteResultados.DroppedDown = false;
        }

        // Limita el desplegable a las coincidencias reales y deja scroll interno
        // cuando supera el máximo visual, evitando espacios vacíos enormes.
        private void AjustarAlturaDesplegableClientes(int cantidadResultados)
        {
            const int maximoFilas = 6;
            int filasVisibles = Math.Min(maximoFilas, Math.Max(1, cantidadResultados));
            cmbClienteResultados.DropDownHeight = filasVisibles * cmbClienteResultados.ItemHeight + 4;
        }

        // Abre el alta como diálogo: no reemplaza el formulario embebido de venta
        // y por eso conserva carrito, pagos y el paso actual en memoria.
        private void BtnNuevoCliente_Click(object? sender, EventArgs e)
        {
            if (!SesionActual.TienePermiso("CLIENTES_ALTA"))
                return;

            try
            {
                FormClienteDetalle alta = new(formPrincipal, SeleccionarClienteCreado);
                formPrincipal.AbrirFormularioTemporalEnPanel(
                    alta,
                    this,
                    formPrincipal.BotonVentas);
            }
            catch (Exception ex)
            {
                // El diálogo es auxiliar: un error al abrirlo o al refrescar el
                // cliente no debe descartar ni cerrar la venta que sigue en memoria.
                MostrarMensaje("No se pudo crear el cliente", ex.Message);
            }
        }

        // Selecciona por ID generado, nunca por nombre, para evitar ambigüedad.
        private void SeleccionarClienteCreado(int idCliente)
        {
            try
            {
                List<ClienteVentaModelo> clientes = ventaLogica.BuscarClientesParaVenta(string.Empty);
                ClienteVentaModelo? creado = clientes.FirstOrDefault(cliente => cliente.IdCliente == idCliente);
                if (creado == null)
                {
                    MostrarMensaje("Cliente", "El cliente se guardó, pero no pudo cargarse para esta venta.");
                    return;
                }

                clientesDisponibles = clientes;
                ActualizarOpcionesCliente(clientes, creado.Etiqueta);
                cmbClienteResultados.SelectedItem = creado;
                clienteSeleccionado = creado;
                ActualizarResumen();
                AjustarLayoutResponsive();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Cliente", $"El cliente se guardó, pero no pudo cargarse para esta venta. {ex.Message}");
            }
        }


        private void CmbClienteResultados_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            clienteSeleccionado =
                cmbClienteResultados.SelectedItem
                as ClienteVentaModelo;


            ActualizarResumen();
        }


        // ========================================================
        // MÉTODOS DE PAGO
        // ========================================================

        private void CargarMetodosPago()
        {
            try
            {
                List<MetodoPagoModelo> metodos =
                    ventaLogica
                        .ObtenerMetodosPago();


                cmbMetodoPago.DataSource =
                    null;

                cmbMetodoPago.DisplayMember =
                    nameof(
                        MetodoPagoModelo.Nombre
                    );

                cmbMetodoPago.ValueMember =
                    nameof(
                        MetodoPagoModelo.IdMetodoPago
                    );

                cmbMetodoPago.DataSource =
                    metodos;
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudieron cargar los métodos de pago",
                    ex.Message
                );
            }
        }


        // Restringe el importe a dígitos y un único separador decimal con hasta dos decimales.
        private void TxtMontoPago_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            TextBox caja = (TextBox)sender!;
            int indiceSeparador = caja.Text.IndexOfAny([',', '.']);

            if (char.IsDigit(e.KeyChar))
            {
                if (indiceSeparador >= 0 && caja.SelectionStart > indiceSeparador &&
                    caja.SelectionLength == 0 && caja.Text.Length - indiceSeparador - 1 >= 2)
                {
                    e.Handled = true;
                }

                return;
            }

            if ((e.KeyChar == ',' || e.KeyChar == '.') && indiceSeparador < 0)
            {
                return;
            }


            e.Handled =
                true;
        }


        private void BtnCompletarSaldo_Click(
            object? sender,
            EventArgs e)
        {
            decimal saldo =
                ObtenerSaldoPendiente();


            txtMontoPago.Text =
                saldo > 0
                    ? saldo.ToString(
                        "0.00",
                        CultureInfo.CurrentCulture
                    )
                    : string.Empty;
        }


        private void BtnAgregarPago_Click(
            object? sender,
            EventArgs e)
        {
            if (
                cmbMetodoPago.SelectedItem
                is not MetodoPagoModelo metodo)
            {
                MostrarMensaje(
                    "Pago incompleto",
                    "Seleccione un método de pago."
                );

                return;
            }


            if (!TryObtenerMonto(txtMontoPago.Text, out decimal monto, out string errorMonto))
            {
                MostrarMensaje(
                    "Monto inválido",
                    errorMonto
                );

                return;
            }


            decimal saldo =
                ObtenerSaldoPendiente();


            if (saldo <= 0)
            {
                MostrarMensaje(
                    "Venta pagada",
                    "La venta ya no tiene saldo pendiente."
                );

                return;
            }


            if (monto > saldo)
            {
                MostrarMensaje(
                    "Monto inválido",
                    $"El monto no puede superar el saldo pendiente de {saldo:C2}."
                );

                return;
            }


            PagoVentaVista? existente =
                pagosVenta
                    .FirstOrDefault(
                        pago =>
                            pago.IdMetodoPago
                            ==
                            metodo.IdMetodoPago
                    );


            if (existente != null)
            {
                string? errorMontoAcumulado = VentaLogica.ValidarMontoPago(existente.Monto + monto);

                if (errorMontoAcumulado != null)
                {
                    MostrarMensaje("Monto inválido", errorMontoAcumulado);
                    return;
                }

                existente.Monto +=
                    monto;

                dgvPagos.Refresh();
            }
            else
            {
                pagosVenta.Add(
                    new PagoVentaVista
                    {
                        IdMetodoPago =
                            metodo.IdMetodoPago,

                        MetodoPago =
                            metodo.Nombre,

                        Monto =
                            monto
                    }
                );
            }


            txtMontoPago.Clear();

            ActualizarResumen();
        }


        private void DgvPagos_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (
                e.RowIndex < 0
                ||
                e.ColumnIndex < 0)
            {
                return;
            }


            if (
                dgvPagos
                    .Columns[e.ColumnIndex]
                    .Name
                !=
                "colQuitarPago")
            {
                return;
            }


            if (
                e.RowIndex
                <
                pagosVenta.Count)
            {
                pagosVenta.RemoveAt(
                    e.RowIndex
                );

                ActualizarResumen();
            }
        }


        // ========================================================
        // RESUMEN
        // ========================================================

        private decimal ObtenerTotalVenta()
        {
            return itemsVenta.Sum(
                item =>
                    item.Subtotal
            );
        }


        private decimal ObtenerTotalPagado()
        {
            return pagosVenta.Sum(
                pago =>
                    pago.Monto
            );
        }


        private decimal ObtenerSaldoPendiente()
        {
            decimal saldo =
                ObtenerTotalVenta()
                -
                ObtenerTotalPagado();


            return saldo < 0
                ? 0
                : saldo;
        }


        private void ActualizarResumen()
        {
            decimal total =
                ObtenerTotalVenta();

            decimal pagado =
                ObtenerTotalPagado();

            decimal saldo =
                total - pagado;


            lblTotalCarrito.Text =
                total.ToString("C2");

            lblTotalVenta.Text =
                total.ToString("C2");

            lblTotalPagado.Text =
                pagado.ToString("C2");

            lblSaldo.Text =
                saldo.ToString("C2");


            int unidades =
                itemsVenta.Sum(
                    item =>
                        item.Cantidad
                );


            lblResumenProductos.Text =
                $"{itemsVenta.Count} producto{(itemsVenta.Count == 1 ? string.Empty : "s")} · " +
                $"{unidades} unidade{(unidades == 1 ? string.Empty : "s")}";

            lblResumenCliente.Text =
                clienteSeleccionado?.Nombre
                ??
                "Sin seleccionar";

            AjustarControlesResumen();


            btnContinuar.Enabled =
                itemsVenta.Count > 0
                &&
                SesionActual.IdSucursalOperativa.HasValue;

            btnConfirmarVenta.Enabled =
                clienteSeleccionado != null
                &&
                itemsVenta.Count > 0
                &&
                pagosVenta.Count > 0
                && saldo == 0
                && SesionActual.IdSucursalOperativa.HasValue;
        }


        // ========================================================
        // CONFIRMAR VENTA
        // ========================================================

        // Da feedback inmediato antes de confirmar y mantiene en Lógica
        // la validación autoritativa de la sucursal y la venta completa.
        private void BtnConfirmarVenta_Click(
            object? sender,
            EventArgs e)
        {
            if (!ValidarSucursalOperativa())
            {
                return;
            }


            if (clienteSeleccionado == null)
            {
                MostrarMensaje(
                    "Venta incompleta",
                    "Debe seleccionar un cliente."
                );

                return;
            }


            if (itemsVenta.Count == 0)
            {
                MostrarMensaje(
                    "Venta incompleta",
                    "Debe agregar al menos un producto."
                );

                return;
            }


            if (pagosVenta.Count == 0)
            {
                MostrarMensaje(
                    "Venta incompleta",
                    "Debe agregar al menos un pago."
                );

                return;
            }


            decimal total =
                ObtenerTotalVenta();

            decimal pagado =
                ObtenerTotalPagado();


            if (pagado != total)
            {
                MostrarMensaje(
                    "Venta incompleta",
                    $"El total pagado debe coincidir con el total de la venta. " +
                    $"Pendiente: {(total - pagado):C2}."
                );

                return;
            }


            using FormMensaje confirmacion =
                new FormMensaje(
                    "Confirmar venta",
                    $"¿Desea registrar la venta por {total:C2}?",
                    "Confirmar",
                    true
                );


            if (
                confirmacion.ShowDialog((Form?)formPrincipal ?? this)
                !=
                DialogResult.OK)
            {
                return;
            }


            VentaRegistrarModelo venta =
                new VentaRegistrarModelo
                {
                    IdCliente =
                        clienteSeleccionado.IdCliente,

                    Items =
                        itemsVenta
                            .Select(
                                item =>
                                    new VentaItemGuardarModelo
                                    {
                                        IdProducto =
                                            item.IdProducto,

                                        Cantidad =
                                            item.Cantidad
                                    }
                            )
                            .ToList(),

                    Pagos =
                        pagosVenta
                            .Select(
                                pago =>
                                    new VentaPagoGuardarModelo
                                    {
                                        IdMetodoPago =
                                            pago.IdMetodoPago,

                                        Monto =
                                            pago.Monto
                                    }
                            )
                            .ToList()
                };


            ResultadoVenta resultado;


            try
            {
                resultado =
                    ventaLogica.Registrar(
                        venta
                    );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo registrar la venta",
                    ex.Message
                );

                return;
            }


            if (!resultado.Exitoso)
            {
                MostrarMensaje(
                    "No se pudo registrar la venta",
                    resultado.Mensaje
                );

                // El stock/precio pudo haber cambiado mientras se
                // armaba la operación. Volvemos a consultar productos.
                CargarProductos(
                    txtBuscarProducto.Text
                );

                return;
            }


            MostrarMensaje(
                "Venta registrada",
                $"La venta N.º {resultado.IdVenta} se registró correctamente."
            );


            ReiniciarVenta();
        }


        // ========================================================
        // VOLVER / CANCELAR
        // ========================================================

        private void BtnVolverProductos_Click(
            object? sender,
            EventArgs e)
        {
            MostrarPasoProductos();
        }


        private void BtnCancelarVenta_Click(
            object? sender,
            EventArgs e)
        {
            bool hayDatos =
                itemsVenta.Count > 0
                ||
                pagosVenta.Count > 0
                ||
                clienteSeleccionado != null;


            if (hayDatos)
            {
                using FormMensaje confirmacion =
                    new FormMensaje(
                        "Cancelar venta",
                        "¿Desea cancelar la venta actual? Se perderán los productos, el cliente y los pagos cargados.",
                        "Cancelar venta",
                        true
                    );


                if (
                    confirmacion.ShowDialog((Form?)formPrincipal ?? this)
                    !=
                    DialogResult.OK)
                {
                    return;
                }
            }


            ReiniciarVenta();
        }


        private void ReiniciarVenta()
        {
            itemsVenta.Clear();
            pagosVenta.Clear();

            clienteSeleccionado =
                null;


            txtBuscarProducto.Clear();

            txtMontoPago.Clear();


            dgvProductos.DataSource =
                null;

            actualizandoSelectorCliente = true;
            cmbClienteResultados.DataSource = null;
            cmbClienteResultados.Text = "Escribí nombre, apellido o documento...";
            actualizandoSelectorCliente = false;


            ConfigurarContexto();

            MostrarPasoProductos();

            ActualizarResumen();


            if (
                ventaLogica.PuedeRealizarVentas()
                &&
                SesionActual.IdSucursalOperativa.HasValue)
            {
                CargarProductos(
                    string.Empty
                );
            }
        }


        // ========================================================
        // HISTORIAL PROPIO
        // ========================================================

        private void BtnMisVentas_Click(
            object? sender,
            EventArgs e)
        {
            if (!ventaLogica.PuedeVerVentas())
            {
                MostrarMensaje(
                    "Acceso no permitido",
                    "No tiene permiso para consultar ventas."
                );

                return;
            }


            string nombre =
                $"{SesionActual.Nombre} {SesionActual.Apellido}"
                    .Trim();


            if (formPrincipal == null)
            {
                using FormListadoVentas historial =
                    new FormListadoVentas(
                        TipoHistorialVentas.Vendedor,
                        SesionActual.IdUsuario,
                        nombre
                    );

                historial.ShowDialog(
                    this
                );

                return;
            }


            formPrincipal.AbrirFormularioEnPanel(
                new FormListadoVentas(
                    formPrincipal,
                    OrigenHistorialVentas.Ventas,
                    TipoHistorialVentas.Vendedor,
                    SesionActual.IdUsuario,
                    nombre
                ),
                formPrincipal.BotonVentas
            );
        }


        // ========================================================
        // HELPERS
        // ========================================================

        // Acepta coma o punto según la cultura y rechaza importes que SQL redondearía o desbordaría.
        private static bool TryObtenerMonto(
            string texto,
            out decimal monto,
            out string error)
        {
            texto = texto.Trim();
            monto = 0;
            error = "Ingresá un monto positivo válido.";

            bool convertido = decimal.TryParse(
                texto,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.CurrentCulture,
                out monto
            );

            if (!convertido)
            {
                convertido = decimal.TryParse(
                    texto.Replace(',', '.'),
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out monto
                );
            }

            if (!convertido)
            {
                return false;
            }

            error = VentaLogica.ValidarMontoPago(monto) ?? string.Empty;
            return string.IsNullOrEmpty(error);
        }


        // Verifica el requisito operativo antes de acciones de venta y evita
        // que la Vista avance cuando la sesión representa todas las sucursales.
        private bool ValidarSucursalOperativa()
        {
            if (SesionActual.IdSucursalOperativa.HasValue)
            {
                return true;
            }


            MostrarAvisoSucursalOperativa();

            return false;
        }


        // Muestra un único aviso claro para orientar la selección previa
        // de sucursal sin exponer el detalle técnico de la validación lógica.
        private void MostrarAvisoSucursalOperativa()
        {
            MostrarMensaje(
                "Sucursal requerida",
                "Para realizar una venta primero seleccioná una sucursal operativa."
            );
        }


        // Muestra los mensajes de Ventas con FormPrincipal como propietario
        // para centrarlos sobre el contenido principal de la aplicación.
        private void MostrarMensaje(
            string titulo,
            string mensaje)
        {
            using FormMensaje formMensaje =
                new FormMensaje(
                    titulo,
                    mensaje
                );


            formMensaje.StartPosition =
                FormStartPosition.CenterParent;

            IWin32Window propietario =
                formPrincipal != null
                    ? formPrincipal
                    : this;

            formMensaje.ShowDialog(
                propietario
            );
        }
    }


    // ============================================================
    // MODELOS EXCLUSIVOS DE LA VISTA
    // ============================================================

    public class ItemVentaVista
    {
        public int IdProducto { get; set; }
        public string CodigoBarra { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int StockDisponible { get; set; }

        public decimal Subtotal =>
            PrecioUnitario
            *
            Cantidad;
    }


    public class PagoVentaVista
    {
        public int IdMetodoPago { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }
}
