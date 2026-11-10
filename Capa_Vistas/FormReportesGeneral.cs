using System.Globalization;
using Capa_Logica;

namespace Capa_Vistas
{
    // Organiza las consultas autorizadas de Reportes en secciones internas sin crear módulos paralelos.
    public partial class FormReportesGeneral : Form
    {
        public enum SeccionReporte { General, PorUsuario, Actividad, VentasDetalladas, StockBajo }
        private readonly FormPrincipal formPrincipal;
        private readonly ReporteLogica reporteLogica = new();
        private readonly SucursalLogica sucursalLogica = new();
        private readonly TabControl tabContenido = new();
        private readonly DataGridView dgvUsuarioVentas = CrearGrilla();
        private readonly DataGridView dgvActividad = CrearGrilla();
        private readonly DataGridView dgvDetalleVentas = CrearGrilla();
        private readonly DataGridView dgvStockBajo = CrearGrilla();
        private readonly Button btnExportar = new();
        private readonly Button btnActividad = new();
        private readonly Panel pnlVistaActividad = new() { AutoScroll = true };
        private readonly Dictionary<SeccionReporte, TabPage> paginas = new();
        private readonly Dictionary<SeccionReporte, Label> estadosVacios = new();
        private List<ReporteDiaModelo> dias = new();
        private List<(string Nombre, decimal Valor)> ventasPorSucursal = new();
        private readonly Panel pnlGraficoIngresos = new() { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
        private readonly Panel pnlTopProductos = new() { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
        private readonly Panel pnlGraficoSucursales = new() { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
        private readonly Panel pnlTarjetaTicketPromedio = new() { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Visible = false };
        private readonly Label lblTicketPromedioTitulo = new() { AutoSize = true, Text = "Ticket promedio", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(80, 85, 90), Location = new Point(16, 13) };
        private readonly Label lblTicketPromedioValor = new() { AutoSize = true, Text = "$ 0,00", Font = new Font("Segoe UI", 23F, FontStyle.Bold), ForeColor = Color.FromArgb(45, 49, 54), Location = new Point(14, 31) };
        private readonly Label lblTicketPromedioDescripcion = new() { AutoSize = true, Text = "Promedio por venta", Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(115, 120, 125), Location = new Point(17, 82) };
        private readonly Label lblEstadoUsuarioVacio = new() { AutoSize = false, Text = "Seleccioná un usuario para consultar su rendimiento comercial.\nElegí un usuario y aplicá los filtros.", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 11F), ForeColor = Color.FromArgb(105, 110, 116), Visible = false };
        private decimal subtotalActual;
        private decimal descuentosActuales;
        private decimal recaudacionActual;
        private int ventasActuales;
        private int unidadesActuales;
        private int stockBajoActual;
        private string usuarioReporteActual = string.Empty;
        private SeccionReporte seccionActual = SeccionReporte.General;
        private bool seccionInicializada;
        private bool preparandoCambioSeccion;
        private SeccionReporte? seccionInicialPendiente;
        private readonly SeccionReporte? seccionSolicitada;

        // Mantiene la navegación embebida y los mensajes centrados en el formulario principal.
        public FormReportesGeneral(FormPrincipal formPrincipal, SeccionReporte? seccionInicial = null)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            seccionSolicitada = seccionInicial;
            AutoScroll = false;
            ConfigurarGrillas();
            ConfigurarContenidoInterno();
            ConfigurarEventos();
            PrepararVistaInicial();
        }

        // Define columnas, formatos y acciones de las grillas sin mezclar reglas de consulta.
        private void ConfigurarGrillas()
        {
            ConfigurarColumnas(dgvActividad, new[] { "Fecha", "Usuario", "Acción", "Entidad", "Detalle", "Sucursal" });
            dgvActividad.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvActividad.Columns[0].Width = 150;
            dgvActividad.Columns[1].Width = 180;
            dgvActividad.Columns[2].Width = 150;
            dgvActividad.Columns[3].Width = 150;
            dgvActividad.Columns[4].MinimumWidth = 400;
            dgvActividad.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvActividad.Columns[5].Width = 180;
            dgvActividad.ScrollBars = ScrollBars.Both;
            dgvActividad.ShowCellToolTips = true;
            dgvActividad.CellToolTipTextNeeded += (_, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 4)
                    e.ToolTipText = Convert.ToString(dgvActividad.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) ?? string.Empty;
            };
            ConfigurarColumnas(dgvProductosVendidos, new[] { "Producto", "Categoría", "Marca", "Unidades", "Precio promedio", "Importe", "Participación" });
            ConfigurarColumnas(dgvUsuarioVentas, new[] { "N°", "Fecha", "Cliente", "Sucursal", "Subtotal", "Descuento", "Total", "Ver detalle" });
            ConfigurarColumnas(dgvDetalleVentas, new[] { "N°", "Fecha", "Cliente", "Vendedor", "Sucursal", "Subtotal", "Descuento", "Total", "Pagos", "Ver detalle" });
            ConfigurarColumnas(dgvStockBajo, new[] { "Código", "Producto", "Categoría", "Sucursal", "Stock", "Stock mínimo", "Diferencia" });
            dgvStockBajo.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdProducto", Visible = false });
            dgvStockBajo.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdSucursal", Visible = false });
            dgvStockBajo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvStockBajo.Columns[0].Width = 140;
            dgvStockBajo.Columns[1].MinimumWidth = 220;
            dgvStockBajo.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvStockBajo.Columns[2].Width = 140;
            dgvStockBajo.Columns[3].Width = 180;
            dgvStockBajo.Columns[4].Width = 80;
            dgvStockBajo.Columns[5].Width = 115;
            dgvStockBajo.Columns[6].Width = 90;
            dgvStockBajo.ColumnHeadersHeight = 42;
            dgvStockBajo.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvStockBajo.ScrollBars = ScrollBars.Both;
        }

        // Crea las páginas reutilizables para las secciones que ocupan el área completa de resultados.
        private void ConfigurarContenidoInterno()
        {
            pnlTarjetaTicketPromedio.Controls.AddRange(new Control[] { lblTicketPromedioTitulo, lblTicketPromedioValor, lblTicketPromedioDescripcion });
            pnlGraficoIngresos.Paint += (_, e) => DibujarBarrasIngresos(e.Graphics, pnlGraficoIngresos.ClientRectangle);
            pnlTopProductos.Paint += (_, e) => DibujarTopProductos(e.Graphics, pnlTopProductos.ClientRectangle);
            pnlGraficoSucursales.Paint += (_, e) => DibujarDonutSucursales(e.Graphics, pnlGraficoSucursales.ClientRectangle);
            tabContenido.Appearance = TabAppearance.FlatButtons;
            tabContenido.ItemSize = new Size(0, 1);
            tabContenido.SizeMode = TabSizeMode.Fixed;
            tabContenido.Font = new Font("Segoe UI", 9F);
            tabContenido.Visible = false;
            CrearPagina(SeccionReporte.PorUsuario, "Historial resumido", dgvUsuarioVentas);
            CrearPagina(SeccionReporte.Actividad, "Actividad", dgvActividad);
            CrearPagina(SeccionReporte.VentasDetalladas, "Ventas detalladas", dgvDetalleVentas);
            CrearPagina(SeccionReporte.StockBajo, "Stock bajo", dgvStockBajo);
            foreach (TabPage pagina in paginas.Values)
                tabContenido.TabPages.Add(pagina);
            btnExportar.Text = "Exportar Excel";
            btnExportar.BackColor = Color.FromArgb(190, 137, 45);
            btnExportar.ForeColor = Color.White;
            btnExportar.FlatStyle = FlatStyle.Flat;
            btnExportar.FlatAppearance.BorderSize = 0;
            pnlFiltros.Controls.Add(btnExportar);
            btnActividad.Text = "Actividad";
            btnActividad.FlatStyle = FlatStyle.Flat;
            btnActividad.FlatAppearance.BorderSize = 0;
            btnActividad.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            pnlNavegacion.Controls.Add(btnActividad);
            pnlAreaContenido.Controls.Add(pnlVistaActividad);
            MontarControlesPersistentes();
            foreach (Control control in new Control[]
            {
                pnlAreaContenido, pnlVistaGeneral, pnlVistaUsuario, pnlVistaActividad, pnlVistaVentas, pnlVistaStock,
                dgvActividad, dgvDetalleVentas, dgvStockBajo, dgvUsuarioVentas, dgvProductosVendidos
            })
                HabilitarDobleBuffer(control);

            OcultarZonaDatos();
        }

        // Mantiene filtros y navegación disponibles mientras evita mostrar resultados a medio actualizar.
        private void OcultarZonaDatos()
        {
            foreach (Control control in new Control[]
            {
                pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaProductos, pnlTarjetaStock,
                pnlTarjetaTicketPromedio, pnlGrafico, pnlGraficoIngresos, pnlTopProductos,
                pnlGraficoSucursales, pnlProductosVendidos, tabContenido, lblEstadoUsuarioVacio
            })
                control.Visible = false;
        }

        private void MontarControlesPersistentes()
        {
            pnlAreaContenido.AutoScroll = true;
            Control[] controlesCompartidos =
            {
                pnlFiltros, pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaProductos,
                pnlTarjetaStock, pnlTarjetaTicketPromedio, pnlGrafico, pnlGraficoIngresos,
                pnlTopProductos, pnlGraficoSucursales, pnlProductosVendidos, tabContenido,
                lblEstadoUsuarioVacio
            };

            foreach (Control control in controlesCompartidos)
            {
                if (!ReferenceEquals(control.Parent, pnlAreaContenido))
                    pnlAreaContenido.Controls.Add(control);
            }
        }

        private static void HabilitarDobleBuffer(Control control)
        {
            typeof(Control)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(control, true);
        }

        // Crea una página con estado vacío dentro de la sección para evitar mensajes emergentes sin datos.
        private void CrearPagina(SeccionReporte seccion, string titulo, DataGridView grilla)
        {
            TabPage pagina = new(titulo) { BackColor = Color.White, Padding = new Padding(8) };
            Label estado = new() { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.FromArgb(105, 110, 116), Font = new Font("Segoe UI", 10F), Visible = false };
            pagina.Controls.Add(estado);
            pagina.Controls.Add(grilla);
            estado.BringToFront();
            paginas.Add(seccion, pagina);
            estadosVacios.Add(seccion, estado);
        }

        // Enlaza navegación, filtros y acciones de grilla fuera del Designer.
        // Divide el reporte por usuario entre ventas y actividad persistida sin mezclar ambos historiales.
        private void ConfigurarPaginaPorUsuario()
        {
            // La página estándar ya aloja sólo el historial resumido de ventas.
        }

        // Mantiene la proporción inicial del historial sin asignar una distancia inválida durante la creación.
        private void AjustarDivisorUsuario()
        {
        }

        // Construye un bloque de resultados con título y estado vacío dentro de la propia sección.
        private static Panel CrearPanelSeccion(string titulo, DataGridView grilla, Label estado)
        {
            Panel panel = new() { Dock = DockStyle.Fill, Padding = new Padding(0, 0, 0, 8) };
            Label encabezado = new() { Dock = DockStyle.Top, Height = 30, Text = titulo, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(55, 59, 64), Padding = new Padding(4, 6, 0, 0) };
            estado.Dock = DockStyle.Fill;
            panel.Controls.Add(estado);
            panel.Controls.Add(grilla);
            panel.Controls.Add(encabezado);
            estado.BringToFront();
            encabezado.BringToFront();
            return panel;
        }

        // Enlaza navegación, filtros y acciones de grilla fuera del Designer.
        private void ConfigurarEventos()
        {
            btnGeneral.Click += (_, _) => CambiarSeccion(SeccionReporte.General);
            btnPorUsuario.Click += (_, _) => CambiarSeccion(SeccionReporte.PorUsuario);
            btnActividad.Click += (_, _) => CambiarSeccion(SeccionReporte.Actividad);
            btnVentasDetalladas.Click += (_, _) => CambiarSeccion(SeccionReporte.VentasDetalladas);
            btnStockBajo.Click += (_, _) => CambiarSeccion(SeccionReporte.StockBajo);
            btnAplicarFiltros.Click += (_, _) => CargarSeccionActual();
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;
            cmbSucursal.SelectionChangeCommitted += (_, _) => CargarVendedores();
            btnExportar.Click += BtnExportar_Click;
            dgvUsuarioVentas.CellContentClick += DgvVentas_CellContentClick;
            dgvDetalleVentas.CellContentClick += DgvVentas_CellContentClick;
            dgvStockBajo.CellContentClick += DgvStockBajo_CellContentClick;
            pnlGraficoContenido.Paint += PnlGraficoContenido_Paint;
            foreach (Panel tarjeta in new[] { pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaProductos, pnlTarjetaStock, pnlTarjetaTicketPromedio })
                tarjeta.Paint += PnlTarjeta_Paint;
            Load += FormReportesGeneral_Load;
            Resize += (_, _) => AjustarLayout();
        }

        private void FormReportesGeneral_Load(object? sender, EventArgs e)
        {
            if (seccionInicialPendiente is SeccionReporte seccion)
            {
                seccionInicialPendiente = null;
                CambiarSeccion(seccion);
            }
        }

        // Acento dorado lateral común a las tarjetas de indicadores para unificar su jerarquía visual.
        private void PnlTarjeta_Paint(object? sender, PaintEventArgs e)
        {
            using SolidBrush acento = new(Color.FromArgb(190, 137, 45));
            e.Graphics.FillRectangle(acento, 0, 0, 4, ((Control)sender!).ClientSize.Height);
        }

        // Inicializa los filtros según las capacidades ya resueltas por ReporteLogica.
        private void PrepararVistaInicial()
        {
            dtpDesde.Value = DateTime.Today.AddDays(-30);
            dtpHasta.Value = DateTime.Today;
            if (!reporteLogica.PuedeAbrirReportes()) { MostrarMensaje("Sin permiso", "No tiene permiso para acceder a Reportes."); return; }
            if (reporteLogica.ObtenerAlcanceReportes() == AlcanceReportes.Ninguno) { MostrarMensaje("Sin alcance", reporteLogica.ObtenerMensajeSinAlcance()); return; }
            CargarSucursales();
            ConfigurarNavegacion();
            SeccionReporte? seccionInicial = seccionSolicitada.HasValue && SeccionDisponible(seccionSolicitada.Value)
                ? seccionSolicitada
                : PrimeraSeccionDisponible();
            if (!seccionInicial.HasValue)
            {
                MostrarMensaje("Sin acceso", "No tenés permisos para consultar ninguna sección de Reportes.");
                return;
            }

            seccionInicialPendiente = seccionInicial.Value;
        }

        // Muestra solo pestañas que la sesión puede consultar mediante permisos REPORTES_*.
        private void ConfigurarNavegacion()
        {
            btnGeneral.Visible = reporteLogica.PuedeVerVentas() || reporteLogica.PuedeVerRecaudacion() || reporteLogica.PuedeVerProductos();
            btnPorUsuario.Visible = reporteLogica.PuedeVerRendimientoVendedores();
            btnActividad.Visible = reporteLogica.PuedeVerActividad();
            btnVentasDetalladas.Visible = reporteLogica.PuedeVerDetalleVentas();
            btnStockBajo.Visible = reporteLogica.PuedeVerStock();
        }

        // Indica si la sección está autorizada por los permisos REPORTES_* de la sesión.
        private bool SeccionDisponible(SeccionReporte seccion) => seccion switch
        {
            SeccionReporte.General => reporteLogica.PuedeVerVentas() || reporteLogica.PuedeVerRecaudacion() || reporteLogica.PuedeVerProductos(),
            SeccionReporte.PorUsuario => reporteLogica.PuedeVerRendimientoVendedores(),
            SeccionReporte.Actividad => reporteLogica.PuedeVerActividad(),
            SeccionReporte.VentasDetalladas => reporteLogica.PuedeVerDetalleVentas(),
            SeccionReporte.StockBajo => reporteLogica.PuedeVerStock(),
            _ => false
        };

        // Elige una sección autorizada si la predeterminada no está disponible para la sesión.
        private SeccionReporte? PrimeraSeccionDisponible()
        {
            if (reporteLogica.PuedeVerVentas()
                || reporteLogica.PuedeVerRecaudacion()
                || reporteLogica.PuedeVerProductos())
            {
                return SeccionReporte.General;
            }

            if (reporteLogica.PuedeVerRendimientoVendedores())
                return SeccionReporte.PorUsuario;

            if (reporteLogica.PuedeVerActividad()) return SeccionReporte.Actividad;

            if (reporteLogica.PuedeVerDetalleVentas())
                return SeccionReporte.VentasDetalladas;

            if (reporteLogica.PuedeVerStock())
                return SeccionReporte.StockBajo;

            return null;
        }

        // Carga sucursales activas solo para el alcance global y evita IDs fijos en el selector.
        private void CargarSucursales()
        {
            AlcanceReportes alcance = reporteLogica.ObtenerAlcanceReportes();
            bool global = alcance == AlcanceReportes.Global;
            bool sucursalFija = alcance == AlcanceReportes.Sucursal;
            lblSucursal.Visible = global || sucursalFija;
            cmbSucursal.Visible = global;
            lblSucursalFija.Visible = sucursalFija;
            lblSucursalFija.Text = $"Sucursal: {SesionActual.Sucursal}";
            if (!global) return;
            List<OpcionSucursal> opciones = sucursalLogica.ObtenerSucursalesDisponibles().Select(s => new OpcionSucursal { IdSucursal = s.IdSucursal, Nombre = s.Nombre }).ToList();
            opciones.Insert(0, new OpcionSucursal { IdSucursal = null, Nombre = "Todas las sucursales" });
            cmbSucursal.DataSource = opciones;
            cmbSucursal.DisplayMember = nameof(OpcionSucursal.Nombre);
            cmbSucursal.ValueMember = nameof(OpcionSucursal.IdSucursal);
            if (cmbSucursal.Items.Count > 0) cmbSucursal.SelectedIndex = 0;
        }

        // Cambia la sección solo cuando varía, conservando las páginas alojadas y reiniciando su scroll.
        private void CambiarSeccion(SeccionReporte seccion)
        {
            if (seccionInicializada && seccionActual == seccion) return;

            seccionActual = seccion;
            seccionInicializada = true;
            if (seccion != SeccionReporte.General)
                tabContenido.SelectedTab = paginas[seccion];
            Panel vistaActiva = ObtenerVista(seccion);
            Panel[] vistas = { pnlVistaGeneral, pnlVistaUsuario, pnlVistaActividad, pnlVistaVentas, pnlVistaStock };
            pnlAreaContenido.SuspendLayout();
            foreach (Panel vista in vistas) vista.SuspendLayout();
            try
            {
                ConfigurarVistaSeccion();
                pnlAreaContenido.AutoScrollPosition = Point.Empty;
                preparandoCambioSeccion = true;
                OcultarZonaDatos();
                CargarSeccionActual(actualizarRender: false);
                ConfigurarVistaSeccion(cargarVendedores: false);
                AjustarLayout();
                foreach (Panel vista in vistas) vista.Visible = false;
                vistaActiva.Visible = true;
                vistaActiva.BringToFront();
                TraerControlesPersistentesAlFrente();
            }
            finally
            {
                preparandoCambioSeccion = false;
                foreach (Panel vista in vistas) vista.ResumeLayout(true);
                pnlAreaContenido.ResumeLayout(true);
            }

            InvalidarGraficosSeccion(seccion);
        }

        // Resuelve el panel raíz de una sección para que las cuatro vistas compartan la misma región.
        private Panel ObtenerVista(SeccionReporte seccion) => seccion switch
        {
            SeccionReporte.General => pnlVistaGeneral,
            SeccionReporte.PorUsuario => pnlVistaUsuario,
            SeccionReporte.Actividad => pnlVistaActividad,
            SeccionReporte.VentasDetalladas => pnlVistaVentas,
            _ => pnlVistaStock
        };

        // Los controles de filtros y resultados se alojan una sola vez en el host estable.
        // Las vistas hermanas actúan como superficies permanentes de sección y no reciben controles reparentados.
        private void TraerControlesPersistentesAlFrente()
        {
            foreach (Control control in new Control[]
            {
                pnlFiltros, pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaProductos,
                pnlTarjetaStock, pnlTarjetaTicketPromedio, pnlGrafico, pnlGraficoIngresos, pnlTopProductos, pnlGraficoSucursales, pnlProductosVendidos, tabContenido, lblEstadoUsuarioVacio
            })
                if (ReferenceEquals(control.Parent, pnlAreaContenido)) control.BringToFront();
        }

        private void InvalidarGraficosSeccion(SeccionReporte seccion)
        {
            if (seccion is SeccionReporte.General or SeccionReporte.PorUsuario)
            {
                pnlGraficoContenido.Invalidate();
                if (pnlGraficoIngresos.Visible) pnlGraficoIngresos.Invalidate();
                if (seccion == SeccionReporte.General && pnlTopProductos.Visible) pnlTopProductos.Invalidate();
            }
        }

        // Ajusta filtros y elementos visibles para que cada vista muestre únicamente sus datos pertinentes.
        private void ConfigurarVistaSeccion(bool cargarVendedores = true)
        {
            bool esGeneral = seccionActual == SeccionReporte.General;
            bool esUsuario = seccionActual == SeccionReporte.PorUsuario;
            bool esActividad = seccionActual == SeccionReporte.Actividad;
            bool esDetalle = seccionActual == SeccionReporte.VentasDetalladas;
            bool esStock = seccionActual == SeccionReporte.StockBajo;
            bool puedeElegirUsuario = (esUsuario || esActividad || esDetalle) && reporteLogica.ObtenerAlcanceReportes() != AlcanceReportes.Propio;
            if (puedeElegirUsuario && cargarVendedores) CargarVendedores();
            bool usuarioIndividual = esUsuario && (reporteLogica.ObtenerAlcanceReportes() == AlcanceReportes.Propio || ObtenerUsuarioSeleccionado().HasValue);
            bool mostrarIndicadoresUsuario = esUsuario && usuarioIndividual;
            lblDesde.Visible = dtpDesde.Visible = !esStock;
            lblHasta.Visible = dtpHasta.Visible = !esStock;
            lblVendedor.Visible = cmbVendedor.Visible = puedeElegirUsuario;
            lblVendedor.Text = (esUsuario || esActividad) ? "Usuario" : "Vendedor";
            pnlTarjetaVentas.Visible = (esGeneral || mostrarIndicadoresUsuario) && reporteLogica.PuedeVerVentas();
            pnlTarjetaIngresos.Visible = (esGeneral || mostrarIndicadoresUsuario) && reporteLogica.PuedeVerRecaudacion();
            pnlTarjetaProductos.Visible = (esGeneral || mostrarIndicadoresUsuario) && reporteLogica.PuedeVerProductos();
            pnlTarjetaStock.Visible = esGeneral && reporteLogica.PuedeVerStock();
            pnlTarjetaTicketPromedio.Visible = mostrarIndicadoresUsuario && reporteLogica.PuedeVerVentas() && reporteLogica.PuedeVerRecaudacion();
            pnlGrafico.Visible = (esGeneral || mostrarIndicadoresUsuario) && (reporteLogica.PuedeVerVentas() || reporteLogica.PuedeVerRecaudacion());
            pnlGraficoIngresos.Visible = (esGeneral || mostrarIndicadoresUsuario) && pnlGrafico.Visible && reporteLogica.PuedeVerRecaudacion();
            pnlTopProductos.Visible = esGeneral && pnlGrafico.Visible;
            pnlGraficoSucursales.Visible = false;
            pnlProductosVendidos.Visible = (esGeneral || mostrarIndicadoresUsuario) && reporteLogica.PuedeVerProductos();
            tabContenido.Visible = (esUsuario && usuarioIndividual) || esActividad || esDetalle || esStock;
            lblEstadoUsuarioVacio.Visible = esUsuario && !usuarioIndividual;
            btnExportar.Visible = reporteLogica.PuedeExportar();
            lblTitulo.Text = esGeneral ? "Reportes" : esUsuario ? "Reportes por usuario" : esActividad ? "Actividad" : esDetalle ? "Ventas detalladas" : "Stock bajo";
            lblSubtitulo.Text = esGeneral ? "Resumen comercial del alcance autorizado." : esUsuario ? "Rendimiento comercial del usuario seleccionado." : esActividad ? "Actividad registrada dentro del alcance autorizado." : esDetalle ? "Consulta completa de ventas registradas." : "Productos que requieren reposición.";
            lblFiltrosTitulo.Text = esStock ? "Filtros de stock" : "Filtros del período";
            lblProductosVendidosTitulo.Text = esUsuario ? "Productos vendidos por usuario" : "Productos más vendidos";
            lblGraficoTitulo.Text = esUsuario ? "Ventas del usuario" : "Ventas del período";
            ActualizarEstiloNavegacion();
        }

        // Destaca la sección activa y conserva la estética oscura y dorada del sistema.
        private void ActualizarEstiloNavegacion()
        {
            AplicarEstiloPestana(btnGeneral, seccionActual == SeccionReporte.General);
            AplicarEstiloPestana(btnPorUsuario, seccionActual == SeccionReporte.PorUsuario);
            AplicarEstiloPestana(btnActividad, seccionActual == SeccionReporte.Actividad);
            AplicarEstiloPestana(btnVentasDetalladas, seccionActual == SeccionReporte.VentasDetalladas);
            AplicarEstiloPestana(btnStockBajo, seccionActual == SeccionReporte.StockBajo);
        }

        // Aplica el color activo sin cambiar los permisos que determinan si la pestaña existe.
        private static void AplicarEstiloPestana(Button boton, bool activa)
        {
            boton.BackColor = activa ? Color.FromArgb(190, 137, 45) : Color.FromArgb(235, 237, 240);
            boton.ForeColor = activa ? Color.White : Color.FromArgb(55, 59, 64);
        }

        // Carga usuarios autorizados por alcance para los filtros que consultan datos por persona.
        private void CargarVendedores()
        {
            if (!cmbVendedor.Visible) return;
            List<OpcionUsuario> opciones = reporteLogica.ListarVendedores(ObtenerSucursalSeleccionada()).Select(u => new OpcionUsuario { IdUsuario = u.IdUsuario, Nombre = u.Nombre }).ToList();
            opciones.Insert(0, new OpcionUsuario { IdUsuario = null, Nombre = "Todos los usuarios" });
            cmbVendedor.DataSource = opciones;
            cmbVendedor.DisplayMember = nameof(OpcionUsuario.Nombre);
            cmbVendedor.ValueMember = nameof(OpcionUsuario.IdUsuario);
        }

        // Valida el rango en la Vista antes de que ReporteLogica aplique la validación autoritativa.
        private void CargarSeccionActual(bool actualizarRender = true)
        {
            if (dtpDesde.Value.Date > dtpHasta.Value.Date)
            {
                MostrarMensaje("Fechas inválidas", "La fecha Desde no puede ser posterior a la fecha Hasta.");
                return;
            }

            bool cargaLocal = !preparandoCambioSeccion;
            if (cargaLocal)
            {
                pnlAreaContenido.SuspendLayout();
                OcultarZonaDatos();
            }

            try
            {
                int? sucursal = ObtenerSucursalSeleccionada(), usuario = ObtenerUsuarioSeleccionado();
                if (seccionActual == SeccionReporte.General) CargarGeneral(sucursal);
                else if (seccionActual == SeccionReporte.PorUsuario) CargarPorUsuario(sucursal, usuario);
                else if (seccionActual == SeccionReporte.Actividad) CargarActividad(sucursal, usuario);
                else if (seccionActual == SeccionReporte.VentasDetalladas) CargarVentasDetalladas(sucursal, usuario);
                else CargarStockBajo(sucursal);
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudieron cargar los reportes", ex.Message);
            }
            finally
            {
                if (cargaLocal)
                {
                    try
                    {
                        ConfigurarVistaSeccion(cargarVendedores: false);
                        AjustarLayout();
                    }
                    finally
                    {
                        pnlAreaContenido.ResumeLayout(true);
                    }
                }
            }

            if (actualizarRender) ActualizarRenderGraficosActivos();
        }
        // Carga tarjetas, gráfico y ranking comercial sin duplicar detalle ni stock bajo completo.
        private void CargarGeneral(int? sucursal)
        {
            stockBajoActual = 0;
            unidadesActuales = 0;
            CargarResumenYGrafico(sucursal, null);
            if (reporteLogica.PuedeVerProductos()) CargarProductos(sucursal, null);
            if (reporteLogica.PuedeVerStock())
            {
                stockBajoActual = reporteLogica.ObtenerStock(sucursal).Count;
                lblStockBajoValor.Text = stockBajoActual.ToString(CultureInfo.CurrentCulture);
            }
            ventasPorSucursal = reporteLogica.PuedeVerDetalleVentas()
                ? reporteLogica.ObtenerDetalleVentas(dtpDesde.Value, dtpHasta.Value, sucursal, null)
                    .GroupBy(venta => string.IsNullOrWhiteSpace(venta.Sucursal) ? "Sin sucursal" : venta.Sucursal)
                    .Select(grupo => (grupo.Key, (decimal)grupo.Count())).ToList()
                : new();
            pnlGraficoIngresos.Invalidate();
            pnlTopProductos.Invalidate();
            pnlGraficoSucursales.Invalidate();
        }

        // Reutiliza las consultas por usuario para mostrar solo ventas realmente registradas y su historial resumido.
        private void CargarPorUsuario(int? sucursal, int? usuario)
        {
            usuarioReporteActual = cmbVendedor.Visible && cmbVendedor.SelectedItem is OpcionUsuario opcion
                ? opcion.Nombre
                : SesionActual.NombreUsuario;
            if (reporteLogica.ObtenerAlcanceReportes() != AlcanceReportes.Propio && !usuario.HasValue)
            {
                usuarioReporteActual = string.Empty;
                ConfigurarVisibilidadDatosUsuario(false);
                dgvUsuarioVentas.Rows.Clear();
                dgvProductosVendidos.Rows.Clear();
                dias = new List<ReporteDiaModelo>();
                ventasActuales = unidadesActuales = 0;
                subtotalActual = descuentosActuales = recaudacionActual = 0m;
                lblVentasValor.Text = "0";
                lblIngresosValor.Text = 0m.ToString("C2", CultureInfo.CurrentCulture);
                lblProductosValor.Text = "0";
                lblTicketPromedioValor.Text = 0m.ToString("C2", CultureInfo.CurrentCulture);
                lblProductosVendidosDescripcion.Text = "Seleccioná un usuario para consultar sus productos vendidos.";
                lblGraficoPlaceholder.Visible = true;
                lblGraficoPlaceholder.Text = "Seleccioná un usuario para consultar su evolución.";
                pnlGraficoContenido.Invalidate();
                MostrarPagina(SeccionReporte.PorUsuario, 0, "Seleccioná un usuario para consultar su actividad comercial.");
                return;
            }
            ConfigurarVisibilidadDatosUsuario(true);
            CargarResumenYGrafico(sucursal, usuario);
            if (reporteLogica.PuedeVerProductos()) CargarProductos(sucursal, usuario);
            dgvUsuarioVentas.Rows.Clear();
            if (reporteLogica.PuedeVerDetalleVentas())
            {
                foreach (var venta in reporteLogica.ObtenerDetalleVentas(dtpDesde.Value, dtpHasta.Value, sucursal, usuario)) dgvUsuarioVentas.Rows.Add(venta.IdVenta, venta.Fecha, venta.Cliente, venta.Sucursal, venta.Subtotal, venta.Descuento, venta.Total, "Ver detalle");
                MostrarPagina(SeccionReporte.PorUsuario, dgvUsuarioVentas.Rows.Count, "El usuario seleccionado no registró ventas en el período.");
            }
            else MostrarPagina(SeccionReporte.PorUsuario, 0, "No tiene permiso para consultar el historial detallado de ventas.");

        }

        private void ConfigurarVisibilidadDatosUsuario(bool mostrar)
        {
            lblEstadoUsuarioVacio.Visible = !mostrar;
            pnlTarjetaVentas.Visible = mostrar && reporteLogica.PuedeVerVentas();
            pnlTarjetaIngresos.Visible = mostrar && reporteLogica.PuedeVerRecaudacion();
            pnlTarjetaProductos.Visible = mostrar && reporteLogica.PuedeVerProductos();
            pnlTarjetaTicketPromedio.Visible = mostrar && reporteLogica.PuedeVerVentas() && reporteLogica.PuedeVerRecaudacion();
            pnlGrafico.Visible = mostrar && (reporteLogica.PuedeVerVentas() || reporteLogica.PuedeVerRecaudacion());
            pnlGraficoIngresos.Visible = mostrar && pnlGrafico.Visible && reporteLogica.PuedeVerRecaudacion();
            pnlProductosVendidos.Visible = mostrar && reporteLogica.PuedeVerProductos();
            tabContenido.Visible = mostrar && reporteLogica.PuedeVerDetalleVentas();
        }

        // Muestra actividad administrativa persistida en su propia sección.
        private void CargarActividad(int? sucursal, int? usuario)
        {
            List<AuditoriaReporteModelo> actividad = reporteLogica.ObtenerActividadUsuario(
                dtpDesde.Value,
                dtpHasta.Value,
                sucursal,
                usuario);

            dgvActividad.Rows.Clear();
            foreach (AuditoriaReporteModelo evento in actividad)
            {
                dgvActividad.Rows.Add(
                    evento.Fecha,
                    evento.Usuario,
                    evento.Accion,
                    evento.Entidad,
                    evento.Detalle,
                    evento.Sucursal);
            }

            MostrarPagina(SeccionReporte.Actividad, dgvActividad.Rows.Count, "No se registró actividad en el período seleccionado.");
        }

        // Carga la consulta extensa en su propia sección para no duplicarla dentro del resumen general.
        private void CargarVentasDetalladas(int? sucursal, int? usuario)
        {
            dgvDetalleVentas.Rows.Clear();
            foreach (var venta in reporteLogica.ObtenerDetalleVentas(dtpDesde.Value, dtpHasta.Value, sucursal, usuario)) dgvDetalleVentas.Rows.Add(venta.IdVenta, venta.Fecha, venta.Cliente, venta.Vendedor, venta.Sucursal, venta.Subtotal, venta.Descuento, venta.Total, venta.MetodosPago, "Ver detalle");
            MostrarPagina(SeccionReporte.VentasDetalladas, dgvDetalleVentas.Rows.Count, "No se registraron ventas en el período seleccionado.");
        }

        // Presenta alertas de inventario y deriva la modificación al flujo autorizado de Productos.
        private void CargarStockBajo(int? sucursal)
        {
            dgvStockBajo.Rows.Clear();
            foreach (var stock in reporteLogica.ObtenerStock(sucursal)) dgvStockBajo.Rows.Add(stock.Codigo, stock.Producto, stock.Categoria, stock.Sucursal, stock.Stock, stock.StockMinimo, stock.Stock - stock.StockMinimo, stock.IdProducto, stock.IdSucursal);
            bool puedeGestionar = dgvStockBajo.Rows.Cast<DataGridViewRow>().Any(fila => reporteLogica.PuedeGestionarStockSucursal(Convert.ToInt32(fila.Cells[8].Value)));
            if (puedeGestionar && !dgvStockBajo.Columns.Contains("Gestionar"))
                dgvStockBajo.Columns.Add(new DataGridViewButtonColumn { Name = "Gestionar", HeaderText = "Acción", Text = "Gestionar stock", UseColumnTextForButtonValue = true, Width = 140, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            if (!puedeGestionar && dgvStockBajo.Columns.Contains("Gestionar")) dgvStockBajo.Columns.Remove("Gestionar");
            MostrarPagina(SeccionReporte.StockBajo, dgvStockBajo.Rows.Count, "No hay productos con stock bajo para el alcance seleccionado.");
        }

        // Actualiza los indicadores y la evolución diaria con el mismo alcance aplicado a la sección actual.
        private void CargarResumenYGrafico(int? sucursal, int? usuario)
        {
            bool ventas = reporteLogica.PuedeVerVentas(), recaudacion = reporteLogica.PuedeVerRecaudacion();
            ventasActuales = 0;
            subtotalActual = 0;
            descuentosActuales = 0;
            recaudacionActual = 0;
            if (ventas || recaudacion)
            {
                var resumen = reporteLogica.ObtenerResumen(dtpDesde.Value, dtpHasta.Value, sucursal, usuario);
                ventasActuales = resumen.Ventas;
                subtotalActual = resumen.Subtotal;
                descuentosActuales = resumen.Descuentos;
                recaudacionActual = resumen.Recaudacion;
                lblVentasValor.Text = ventas ? resumen.Ventas.ToString(CultureInfo.CurrentCulture) : "—";
                lblIngresosValor.Text = recaudacion ? resumen.Recaudacion.ToString("C2", CultureInfo.CurrentCulture) : "—";
            }
            lblTicketPromedioValor.Text = ventasActuales > 0
                ? (recaudacionActual / ventasActuales).ToString("C2", CultureInfo.CurrentCulture)
                : 0m.ToString("C2", CultureInfo.CurrentCulture);
            dias = ventas || recaudacion ? reporteLogica.ObtenerVentasPorDia(dtpDesde.Value, dtpHasta.Value, sucursal, usuario) : new();
            lblGraficoPlaceholder.Visible = dias.Sum(dia => dia.Ventas) == 0;
            lblGraficoPlaceholder.Text = "No se registraron ventas en el período seleccionado.";
            lblPeriodoGrafico.Text = $"Período: {dtpDesde.Value:dd/MM/yyyy} - {dtpHasta.Value:dd/MM/yyyy}";
            pnlGraficoContenido.Invalidate();
        }

        // Carga el ranking de productos para el resumen o para el usuario elegido sin consultas duplicadas.
        private void CargarProductos(int? sucursal, int? usuario)
        {
            var productos = reporteLogica.ObtenerProductos(dtpDesde.Value, dtpHasta.Value, sucursal, usuario);
            dgvProductosVendidos.Rows.Clear();
            decimal importeTotal = productos.Sum(producto => producto.Importe);
            foreach (var producto in productos)
            {
                decimal precioPromedio = producto.Unidades > 0 ? producto.Importe / producto.Unidades : 0m;
                decimal participacion = importeTotal > 0m ? producto.Importe / importeTotal : 0m;
                dgvProductosVendidos.Rows.Add(producto.Producto, producto.Categoria, producto.Marca, producto.Unidades, precioPromedio, producto.Importe, participacion);
            }
            unidadesActuales = productos.Sum(producto => producto.Unidades);
            lblProductosValor.Text = productos.Sum(producto => producto.Unidades).ToString(CultureInfo.CurrentCulture);
            lblProductosVendidosDescripcion.Text = productos.Count == 0 ? "No se registraron productos vendidos en el período seleccionado." : "Ranking correspondiente al período seleccionado.";
        }

        // Selecciona una página ya alojada y muestra un estado claro cuando no hay filas.
        private void MostrarPagina(SeccionReporte seccion, int cantidad, string mensajeVacio)
        {
            TabPage pagina = paginas[seccion];
            if (tabContenido.SelectedTab != pagina)
                tabContenido.SelectedTab = pagina;
            Label estado = estadosVacios[seccion];
            estado.Text = mensajeVacio;
            estado.Visible = cantidad == 0;
        }

        // Abre el detalle reutilizable, cuya lógica vuelve a validar el alcance de Reportes.
        private void DgvVentas_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (sender is not DataGridView grilla || e.RowIndex < 0 || e.ColumnIndex < 0 || grilla.Columns[e.ColumnIndex].Name != "Ver detalle") return;
            formPrincipal.AbrirFormularioEnPanel(new FormVentaDetalle(formPrincipal, Convert.ToInt32(grilla.Rows[e.RowIndex].Cells[0].Value), seccionActual), formPrincipal.BotonReportes);
        }

        // Envía el producto al detalle existente solo cuando Lógica autoriza gestionar esa sucursal.
        private void DgvStockBajo_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvStockBajo.Columns[e.ColumnIndex].Name != "Gestionar") return;
            int idProducto = Convert.ToInt32(dgvStockBajo.Rows[e.RowIndex].Cells[7].Value), idSucursal = Convert.ToInt32(dgvStockBajo.Rows[e.RowIndex].Cells[8].Value);
            if (reporteLogica.PuedeGestionarStockSucursal(idSucursal))
            {
                string nombreProducto = Convert.ToString(dgvStockBajo.Rows[e.RowIndex].Cells[1].Value) ?? string.Empty;
                formPrincipal.AbrirFormularioEnPanel(
                    // La sucursal de la alerta habilita la acción, pero no debe limitar
                    // el listado: InventarioLogica resuelve el alcance autorizado completo.
                    new FormProductoDetalle(formPrincipal, idProducto, true, nombreProducto, null, seccionActual),
                    formPrincipal.BotonProductos);
            }
        }

        // Dibuja ventas y recaudación reales con series independientes según permisos concedidos.
        private void PnlGraficoContenido_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(248, 249, 250));
            if (dias.Sum(dia => dia.Ventas) == 0) return;
            DibujarBarrasVerticales(
                e.Graphics,
                pnlGraficoContenido.ClientRectangle,
                dias.Select(dia => (dia.Fecha.ToString("dd/MM"), (decimal)dia.Ventas)).ToList(),
                Color.FromArgb(48, 52, 57),
                Color.FromArgb(190, 137, 45),
                false);
        }

        // Exporta la información visible de la sección activa a un libro XLSX con valores tipados.
        private void BtnExportar_Click(object? sender, EventArgs e)
        {
            if (!reporteLogica.PuedeExportar())
            {
                MostrarMensaje("Sin permiso", "No tiene permiso para exportar reportes.");
                return;
            }

            List<ExcelHojaExportacion> hojas = PrepararHojasExportacion();
            if (hojas.Count == 0)
            {
                MostrarMensaje("Sin datos", "No hay datos para exportar.");
                return;
            }

            string nombreArchivo = ObtenerNombreArchivoExportacion();
            using SaveFileDialog dialogo = new()
            {
                Filter = "Archivo Excel (*.xlsx)|*.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true,
                FileName = nombreArchivo
            };
            if (dialogo.ShowDialog(formPrincipal) != DialogResult.OK) return;

            try
            {
                ExcelExportHelper.Exportar(dialogo.FileName, hojas);
                MostrarMensaje("Exportación completa", "El reporte se exportó correctamente.");
            }
            catch
            {
                MostrarMensaje("Error al exportar", "No se pudo generar el archivo Excel. Verificá que la ubicación esté disponible y que el archivo no esté abierto.");
            }
        }

        // Construye hojas únicamente con los datos y secciones autorizados que están cargados actualmente.
        private List<ExcelHojaExportacion> PrepararHojasExportacion()
        {
            List<ExcelHojaExportacion> hojas = new();
            if (seccionActual == SeccionReporte.General)
            {
                if (dgvProductosVendidos.Rows.Count == 0 && stockBajoActual == 0 && ventasActuales == 0 && recaudacionActual == 0 && unidadesActuales == 0 && !dias.Any(d => d.Ventas > 0 || d.Recaudacion > 0)) return hojas;
                hojas.Add(CrearHojaResumen(false));
                if (dgvProductosVendidos.Rows.Count > 0) hojas.Add(CrearHojaGrilla("Productos más vendidos", dgvProductosVendidos));
                if (dias.Any(d => d.Ventas > 0 || d.Recaudacion > 0))
                    hojas.Add(new ExcelHojaExportacion
                    {
                        Nombre = "Ventas por día",
                        Encabezados = new[] { "Fecha", "Cantidad de ventas", "Recaudación" },
                        Filas = dias.Select(d => new object?[] { d.Fecha, d.Ventas, d.Recaudacion }).ToList(),
                        FormatosColumnas = new Dictionary<int, string> { [0] = "dd/mm/yyyy", [2] = "$#,##0.00" }
                    });
            }
            else if (seccionActual == SeccionReporte.PorUsuario)
            {
                if (string.IsNullOrWhiteSpace(usuarioReporteActual) ||
                    (dgvUsuarioVentas.Rows.Count == 0 && dgvProductosVendidos.Rows.Count == 0 && ventasActuales == 0 && recaudacionActual == 0 && unidadesActuales == 0 && !dias.Any(d => d.Ventas > 0 || d.Recaudacion > 0))) return hojas;
                hojas.Add(CrearHojaResumen(true));
                if (dgvUsuarioVentas.Rows.Count > 0) hojas.Add(CrearHojaGrilla("Ventas", dgvUsuarioVentas));
                if (dgvProductosVendidos.Rows.Count > 0) hojas.Add(CrearHojaGrilla("Productos", dgvProductosVendidos));
            }
            else if (seccionActual == SeccionReporte.Actividad && dgvActividad.Rows.Count > 0)
            {
                hojas.Add(CrearHojaGrilla("Actividad", dgvActividad));
            }
            else if (seccionActual == SeccionReporte.VentasDetalladas && dgvDetalleVentas.Rows.Count > 0)
            {
                hojas.Add(CrearHojaGrilla("Ventas", dgvDetalleVentas));
            }
            else if (seccionActual == SeccionReporte.StockBajo && dgvStockBajo.Rows.Count > 0)
            {
                hojas.Add(CrearHojaGrilla("Stock bajo", dgvStockBajo));
            }
            return hojas;
        }

        // Prepara el resumen del período sin exponer métricas para las que no existe permiso.
        private ExcelHojaExportacion CrearHojaResumen(bool porUsuario)
        {
            List<object?[]> filas = new()
            {
                new object?[] { porUsuario ? "Usuario" : "Reporte", porUsuario ? usuarioReporteActual : "General" },
                new object?[] { "Período", $"{dtpDesde.Value:dd/MM/yyyy} - {dtpHasta.Value:dd/MM/yyyy}" },
                new object?[] { "Sucursal / alcance", ObtenerAlcanceExportacion() }
            };
            if (reporteLogica.PuedeVerVentas()) filas.Add(new object?[] { "Cantidad de ventas", ventasActuales });
            if (reporteLogica.PuedeVerRecaudacion())
            {
                filas.Add(new object?[] { "Subtotal", subtotalActual });
                filas.Add(new object?[] { "Descuentos", descuentosActuales });
                filas.Add(new object?[] { "Ingresos", recaudacionActual });
            }
            if (reporteLogica.PuedeVerProductos()) filas.Add(new object?[] { "Unidades vendidas", unidadesActuales });
            if (!porUsuario && reporteLogica.PuedeVerStock()) filas.Add(new object?[] { "Productos con stock bajo", stockBajoActual });
            Dictionary<(int Fila, int Columna), string> formatos = new();
            for (int i = 0; i < filas.Count; i++)
                if (filas[i][0] is string indicador && indicador is "Subtotal" or "Descuentos" or "Ingresos")
                    formatos[(i, 1)] = "$#,##0.00";

            return new ExcelHojaExportacion
            {
                Nombre = "Resumen",
                Encabezados = new[] { "Indicador", "Valor" },
                Filas = filas,
                CrearTabla = false,
                FormatosCeldas = formatos
            };
        }

        // Convierte una grilla visible a una hoja tabular omitiendo botones y columnas técnicas ocultas.
        private static ExcelHojaExportacion CrearHojaGrilla(string nombre, DataGridView grilla)
        {
            List<DataGridViewColumn> columnas = grilla.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible && c is not DataGridViewButtonColumn && c.Name is not "IdProducto" and not "IdSucursal")
                .ToList();
            if (nombre == "Stock bajo")
                columnas = columnas.OrderBy(c => c.Name == "Producto" ? 0 : c.Name == "Código" ? 1 : c.Index + 2).ToList();

            string[] encabezados = columnas.Select(c => c.Name == "N°" ? "Venta" : c.HeaderText).ToArray();
            List<object?[]> filas = grilla.Rows.Cast<DataGridViewRow>()
                .Where(f => !f.IsNewRow)
                .Select(f => columnas.Select(c => f.Cells[c.Index].Value).ToArray())
                .ToList();
            Dictionary<int, string> formatos = new();
            for (int i = 0; i < columnas.Count; i++)
            {
                if (columnas[i].Name == "Fecha") formatos[i] = "dd/mm/yyyy hh:mm";
                else if (columnas[i].Name is "Subtotal" or "Descuento" or "Total" or "Importe" or "Precio promedio") formatos[i] = "$#,##0.00";
                else if (columnas[i].Name == "Participación") formatos[i] = "0.00%";
                else if (columnas[i].Name is "N°" or "Stock" or "Stock mínimo" or "Diferencia" or "Unidades") formatos[i] = "#,##0";
            }

            return new ExcelHojaExportacion { Nombre = nombre, Encabezados = encabezados, Filas = filas, FormatosColumnas = formatos };
        }

        // Genera un nombre descriptivo y seguro de archivo según la sección actualmente abierta.
        private string ObtenerNombreArchivoExportacion()
        {
            string fecha = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string baseNombre = seccionActual switch
            {
                SeccionReporte.General => $"Reporte_General_{fecha}",
                SeccionReporte.PorUsuario => $"Reporte_Usuario_{LimpiarNombreArchivo(usuarioReporteActual)}_{fecha}",
                SeccionReporte.Actividad => $"Actividad_{fecha}",
                SeccionReporte.VentasDetalladas => $"Ventas_Detalladas_{fecha}",
                _ => $"Stock_Bajo_{fecha}"
            };
            return baseNombre + ".xlsx";
        }

        // Sustituye caracteres no válidos para que el nombre del usuario pueda formar parte del archivo.
        private static string LimpiarNombreArchivo(string valor)
        {
            char[] invalidos = Path.GetInvalidFileNameChars();
            string resultado = new string(valor.Select(c => invalidos.Contains(c) ? '_' : c).ToArray()).Trim();
            return string.IsNullOrWhiteSpace(resultado) ? "Usuario" : resultado;
        }

        // Describe la sucursal seleccionada o el alcance efectivo de la sesión para incluirlo en el resumen.
        private string ObtenerAlcanceExportacion()
        {
            if (cmbSucursal.Visible && cmbSucursal.SelectedItem is OpcionSucursal opcion) return opcion.Nombre;
            if (!string.IsNullOrWhiteSpace(SesionActual.SucursalOperativa)) return SesionActual.SucursalOperativa;
            if (!string.IsNullOrWhiteSpace(SesionActual.Sucursal)) return SesionActual.Sucursal;
            return reporteLogica.ObtenerAlcanceReportes() == AlcanceReportes.Propio ? "Usuario actual" : "Sucursal autorizada";
        }

        // Restablece los filtros disponibles de la sección sin alterar el alcance resuelto en sesión.
        private void BtnLimpiarFiltros_Click(object? sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Today.AddDays(-30); dtpHasta.Value = DateTime.Today;
            if (cmbSucursal.Visible) cmbSucursal.SelectedIndex = 0;
            if (cmbVendedor.Visible) cmbVendedor.SelectedIndex = 0;
            CargarSeccionActual();
        }

        // Mantiene fija la cabecera y distribuye el área scrolleable independientemente debajo de la navegación.
        private void AjustarLayout()
        {
            int margen = 32, ancho = Math.Max(0, ClientSize.Width - margen * 2), y = 20;
            pnlCabecera.SetBounds(margen, y, ancho, 100);
            y += 100;
            pnlNavegacion.SetBounds(margen, y, ancho, 43);
            pnlNavegacion.Height = AjustarBotonesNavegacion(ancho);
            y += pnlNavegacion.Height + 8;
            pnlAreaContenido.SetBounds(margen, y, ancho, Math.Max(140, ClientSize.Height - y - 20));

            foreach (Panel vista in new[] { pnlVistaGeneral, pnlVistaUsuario, pnlVistaActividad, pnlVistaVentas, pnlVistaStock })
            {
                vista.AutoScroll = false;
                vista.SetBounds(0, 0, pnlAreaContenido.ClientSize.Width, Math.Max(140, pnlAreaContenido.ClientSize.Height));
            }

            Panel vistaActiva = ObtenerVista(seccionActual);
            pnlAreaContenido.PerformLayout();
            vistaActiva.PerformLayout();

            // Repite el cálculo si el layout vertical cambia el ancho del viewport visible.
            int anchoViewportAnterior = -1;
            for (int intento = 0; intento < 3; intento++)
            {
                int anchoViewport = Math.Max(0, pnlAreaContenido.ClientSize.Width - pnlAreaContenido.Padding.Horizontal);
                int altoViewport = Math.Max(0, pnlAreaContenido.ClientSize.Height - pnlAreaContenido.Padding.Vertical);
                if (anchoViewport == anchoViewportAnterior)
                    break;

                anchoViewportAnterior = anchoViewport;
                AjustarContenidoVista(vistaActiva, anchoViewport, altoViewport);
                pnlAreaContenido.PerformLayout();
            }
        }

        // Actualiza únicamente las superficies de gráfico; CargarSeccionActual no cambia su geometría.
        private void ActualizarRenderGraficosActivos()
        {
            InvalidarGraficosSeccion(seccionActual);
        }

        // Distribuye la sección activa dentro del área que queda después del scrollbar vertical y el padding.
        private void AjustarContenidoVista(Panel vistaActiva, int anchoContenido, int altoContenido)
        {
            int origenX = vistaActiva.Padding.Left;
            int yContenido = 0;
            AjustarFiltros(anchoContenido, origenX, ref yContenido);
            List<Panel> tarjetas = new[] { pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaProductos, pnlTarjetaStock, pnlTarjetaTicketPromedio }.Where(t => t.Visible).ToList();
            if (tarjetas.Count > 0)
            {
                int columnas = anchoContenido >= 1040 ? 4 : anchoContenido >= 650 ? 2 : 1, separacion = 16, anchoTarjeta = (anchoContenido - separacion * (columnas - 1)) / columnas;
                for (int i = 0; i < tarjetas.Count; i++) tarjetas[i].SetBounds(origenX + (i % columnas) * (anchoTarjeta + separacion), yContenido + (i / columnas) * 126, anchoTarjeta, 110);
                yContenido += ((tarjetas.Count + columnas - 1) / columnas) * 126 + 10;
            }

            if (seccionActual == SeccionReporte.General)
            {
                int separacionGrafico = 16;
                bool dosColumnas = anchoContenido >= 820;
                int anchoGrafico = dosColumnas ? (anchoContenido - separacionGrafico) / 2 : anchoContenido;
                const int altoGrafico = 250;
                if (pnlGrafico.Visible)
                {
                    pnlGrafico.SetBounds(origenX, yContenido, anchoContenido, altoGrafico);
                    AjustarContenidoGrafico();
                    yContenido += altoGrafico + separacionGrafico;
                }
                pnlGraficoIngresos.SetBounds(origenX, yContenido, anchoGrafico, altoGrafico);
                pnlTopProductos.SetBounds(origenX + (dosColumnas ? anchoGrafico + separacionGrafico : 0), dosColumnas ? yContenido : yContenido + altoGrafico + separacionGrafico, anchoGrafico, altoGrafico);
                yContenido += dosColumnas ? altoGrafico + separacionGrafico : altoGrafico * 2 + separacionGrafico * 2;
                if (pnlProductosVendidos.Visible)
                {
                    pnlProductosVendidos.SetBounds(origenX, yContenido, anchoContenido, 360);
                    AjustarEncabezadoProductosVendidos();
                    yContenido += 376;
                }
                EstablecerScrollVista(yContenido);
            }
            else if (seccionActual == SeccionReporte.PorUsuario)
            {
                if (lblEstadoUsuarioVacio.Visible)
                {
                    int altoEstado = Math.Min(170, Math.Max(120, altoContenido / 3));
                    lblEstadoUsuarioVacio.SetBounds(origenX, yContenido + Math.Max(0, (altoContenido - altoEstado) / 2), anchoContenido, altoEstado);
                    EstablecerScrollVista(lblEstadoUsuarioVacio.Bottom);
                }
                else
                {
                    const int separacion = 16;
                    bool dosColumnas = anchoContenido >= 820;
                    int anchoGrafico = dosColumnas ? (anchoContenido - separacion) / 2 : anchoContenido;
                    const int altoGrafico = 250;
                    if (pnlGrafico.Visible)
                    {
                        pnlGrafico.SetBounds(origenX, yContenido, anchoGrafico, altoGrafico);
                        AjustarContenidoGrafico();
                    }
                    if (pnlGraficoIngresos.Visible)
                    {
                        int xIngresos = dosColumnas ? origenX + anchoGrafico + separacion : origenX;
                        int yIngresos = dosColumnas ? yContenido : yContenido + altoGrafico + separacion;
                        pnlGraficoIngresos.SetBounds(xIngresos, yIngresos, anchoGrafico, altoGrafico);
                    }
                    yContenido += (dosColumnas ? altoGrafico : (pnlGrafico.Visible && pnlGraficoIngresos.Visible ? altoGrafico * 2 + separacion : altoGrafico)) + separacion;
                    if (pnlProductosVendidos.Visible)
                    {
                        pnlProductosVendidos.SetBounds(origenX, yContenido, anchoContenido, 360);
                        AjustarEncabezadoProductosVendidos();
                        yContenido += 376;
                    }
                    if (tabContenido.Visible)
                    {
                        tabContenido.SetBounds(origenX, yContenido, anchoContenido, Math.Max(280, Math.Min(440, altoContenido - yContenido - 16)));
                        yContenido += tabContenido.Height + separacion;
                    }
                    EstablecerScrollVista(yContenido);
                }
            }
            else
            {
                tabContenido.SetBounds(origenX, yContenido, anchoContenido, Math.Max(340, altoContenido - yContenido - 12));
                yContenido += tabContenido.Height + 16;
                EstablecerScrollVista(yContenido);
            }
        }

        // Limita el desplazamiento al panel de la sección activa, nunca al host compartido.
        private void EstablecerScrollVista(int altoContenido)
        {
            HashSet<Control> superficies = new() { pnlVistaGeneral, pnlVistaUsuario, pnlVistaActividad, pnlVistaVentas, pnlVistaStock };
            Control? ultimoControlVisible = pnlAreaContenido.Controls
                .Cast<Control>()
                .Where(control => control.Visible && !superficies.Contains(control))
                .OrderByDescending(control => control.Bottom)
                .FirstOrDefault();
            pnlAreaContenido.AutoScrollMinSize = new Size(0, Math.Max(altoContenido, ultimoControlVisible?.Bottom ?? 0) + 24);
        }

        // Reacomoda los elementos del gráfico dentro del ancho de su panel, incluso en la columna compacta de Por usuario.
        private void AjustarContenidoGrafico()
        {
            int anchoInterior = Math.Max(0, pnlGrafico.ClientSize.Width - 36);
            int anchoTitulo = Math.Min(lblGraficoTitulo.PreferredSize.Width, anchoInterior);
            int anchoPeriodo = Math.Min(290, Math.Max(0, anchoInterior - anchoTitulo - 12));
            lblGraficoTitulo.AutoSize = false;
            lblGraficoTitulo.AutoEllipsis = true;
            lblGraficoTitulo.SetBounds(18, 13, anchoTitulo, 25);
            lblPeriodoGrafico.SetBounds(18 + anchoInterior - anchoPeriodo, 16, anchoPeriodo, 20);
            pnlGraficoContenido.SetBounds(18, 47, anchoInterior, Math.Max(0, pnlGrafico.ClientSize.Height - 68));
        }

        // Separa título, descripción y grilla para que el encabezado no se solape al cambiar de sección o tamaño.
        private void AjustarEncabezadoProductosVendidos()
        {
            int anchoPanel = pnlProductosVendidos.ClientSize.Width;
            int margen = Math.Min(18, anchoPanel / 2);
            int ancho = Math.Max(0, anchoPanel - margen * 2);
            lblProductosVendidosTitulo.AutoSize = false;
            lblProductosVendidosTitulo.AutoEllipsis = true;
            lblProductosVendidosTitulo.SetBounds(margen, 8, ancho, 26);
            lblProductosVendidosDescripcion.AutoSize = false;
            lblProductosVendidosDescripcion.AutoEllipsis = true;
            lblProductosVendidosDescripcion.SetBounds(margen, 34, ancho, 34);
            dgvProductosVendidos.SetBounds(0, 72, pnlProductosVendidos.ClientSize.Width, Math.Max(180, pnlProductosVendidos.ClientSize.Height - 73));
            dgvProductosVendidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Distribuye pestañas visibles en todo el ancho sin dejar espacios de una sección sin permiso.
        private int AjustarBotonesNavegacion(int anchoDisponible)
        {
            List<(Button Boton, int Ancho)> botones = new();
            if (reporteLogica.PuedeVerVentas() || reporteLogica.PuedeVerRecaudacion() || reporteLogica.PuedeVerProductos()) botones.Add((btnGeneral, 120));
            if (reporteLogica.PuedeVerRendimientoVendedores()) botones.Add((btnPorUsuario, 120));
            if (reporteLogica.PuedeVerActividad()) botones.Add((btnActividad, 110));
            if (reporteLogica.PuedeVerDetalleVentas()) botones.Add((btnVentasDetalladas, 170));
            if (reporteLogica.PuedeVerStock()) botones.Add((btnStockBajo, 110));

            int fila = 0;
            int x = 0;
            foreach ((Button boton, int ancho) in botones)
            {
                if (x > 0 && x + ancho > anchoDisponible)
                {
                    fila++;
                    x = 0;
                }

                boton.SetBounds(x, fila * 43 + 2, ancho, 39);
                boton.TextAlign = ContentAlignment.MiddleCenter;
                x += ancho + 6;
            }

            return Math.Max(1, fila + 1) * 43;
        }

        // Reacomoda filtros y acciones sin superponerlos y mantiene un margen interior constante.
        private void AjustarFiltros(int ancho, int margen, ref int y)
        {
            List<(Control Etiqueta, Control Campo)> campos = new();
            if (lblDesde.Visible) campos.Add((lblDesde, dtpDesde));
            if (lblHasta.Visible) campos.Add((lblHasta, dtpHasta));
            if (lblSucursal.Visible) campos.Add((lblSucursal, cmbSucursal.Visible ? cmbSucursal : lblSucursalFija));
            if (lblVendedor.Visible) campos.Add((lblVendedor, cmbVendedor));
            // En escritorio los filtros y acciones comparten una única franja;
            // en vistas angostas se ordenan en dos líneas compactas.
            bool formatoAmplio = ancho >= 1080;
            int columnas = formatoAmplio ? 4 : ancho >= 680 ? 2 : 1;
            int margenCampos = Math.Min(18, ancho / 2);
            lblFiltrosTitulo.Visible = !formatoAmplio;
            bool puedeExportar = reporteLogica.PuedeExportar();
            int anchoAccionesAmplio = puedeExportar ? 420 : 270;
            int anchoParaCampos = formatoAmplio
                ? Math.Max(260, ancho - margenCampos * 2 - anchoAccionesAmplio - 20)
                : Math.Max(0, ancho - margenCampos * 2 - (columnas - 1) * 16);
            int anchoInternoCampos = Math.Max(0, anchoParaCampos - (columnas - 1) * 12);
            int anchoCampo = Math.Min(formatoAmplio ? 175 : 230, anchoInternoCampos / columnas);
            int filas = (campos.Count + columnas - 1) / columnas;
            int finCampos = formatoAmplio ? 30 + filas * 52 : 40 + filas * 52;
            int margenAcciones = Math.Min(18, ancho / 2);
            int anchoAccionesDisponible = Math.Max(0, ancho - margenAcciones * 2);
            bool accionesEnUnaFila = anchoAccionesDisponible >= (puedeExportar ? 420 : 270);
            bool aplicarYLimpiarEnFila = accionesEnUnaFila || anchoAccionesDisponible >= 270;
            int cantidadFilasAcciones = aplicarYLimpiarEnFila ? (accionesEnUnaFila || !puedeExportar ? 1 : 2) : (puedeExportar ? 3 : 2);
            int alto = formatoAmplio ? 82 : finCampos + 12 + cantidadFilasAcciones * 42;
            pnlFiltros.SetBounds(margen, y, ancho, alto);
            for (int i = 0; i < campos.Count; i++)
            {
                int col = i % columnas, fila = i / columnas;
                int x = margenCampos + col * (anchoCampo + 12), top = formatoAmplio ? 10 : 40 + fila * 52;
                if (campos[i].Etiqueta is Label etiqueta)
                {
                    etiqueta.AutoSize = false;
                    etiqueta.AutoEllipsis = true;
                }
                campos[i].Etiqueta.SetBounds(x, top, anchoCampo, 20);
                campos[i].Campo.SetBounds(x, top + 22, anchoCampo, 28);
            }

            int accionesY = formatoAmplio ? 24 : finCampos + 6;
            if (accionesEnUnaFila)
            {
                int anchoGrupo = puedeExportar ? 420 : 270;
                int accionesX = formatoAmplio
                    ? ancho - margenAcciones - anchoGrupo
                    : margenAcciones + anchoAccionesDisponible - anchoGrupo;
                btnAplicarFiltros.SetBounds(accionesX, accionesY, 140, 36);
                btnLimpiarFiltros.SetBounds(accionesX + 150, accionesY, 120, 36);
                if (puedeExportar) btnExportar.SetBounds(accionesX + 280, accionesY, 140, 36);
            }
            else if (aplicarYLimpiarEnFila)
            {
                int anchoBotonAplicar = Math.Min(140, anchoAccionesDisponible);
                int anchoBotonLimpiar = Math.Min(120, Math.Max(0, anchoAccionesDisponible - anchoBotonAplicar - 10));
                int accionesX = margenAcciones;
                btnAplicarFiltros.SetBounds(accionesX, accionesY, anchoBotonAplicar, 36);
                btnLimpiarFiltros.SetBounds(accionesX + anchoBotonAplicar + 10, accionesY, anchoBotonLimpiar, 36);
                if (puedeExportar)
                    btnExportar.SetBounds(margenAcciones, accionesY + 42, Math.Min(140, anchoAccionesDisponible), 36);
            }
            else
            {
                int anchoBoton = Math.Min(140, anchoAccionesDisponible);
                btnAplicarFiltros.SetBounds(margenAcciones, accionesY, anchoBoton, 36);
                btnLimpiarFiltros.SetBounds(margenAcciones, accionesY + 42, Math.Min(120, anchoAccionesDisponible), 36);
                if (puedeExportar)
                    btnExportar.SetBounds(margenAcciones, accionesY + 84, anchoBoton, 36);
            }
            y += alto + 12;
        }

        // Construye una grilla de solo lectura con encabezados y números alineados de forma consistente.
        private void DibujarBarrasIngresos(Graphics g, Rectangle area)
        {
            DibujarTitulo(g, seccionActual == SeccionReporte.PorUsuario ? "Ingresos del usuario" : "Ingresos por día");
            List<(string Etiqueta, decimal Valor)> datos = dias
                .OrderBy(dia => dia.Fecha)
                .TakeLast(14)
                .Select(dia => (dia.Fecha.ToString("dd/MM"), dia.Recaudacion))
                .ToList();
            DibujarBarrasVerticales(g, area, datos, Color.FromArgb(89, 161, 79), null, true);
        }

        private void DibujarTopProductos(Graphics g, Rectangle area)
        {
            DibujarTitulo(g, "Top productos");
            List<(string Nombre, decimal Valor)> datos = dgvProductosVendidos.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).Take(8).Select(r => (Convert.ToString(r.Cells[0].Value) ?? "Producto", Convert.ToDecimal(r.Cells[3].Value ?? 0))).ToList();
            if (!datos.Any()) { DibujarVacio(g, area); return; }
            decimal maximo = Math.Max(1, datos.Max(d => d.Valor)); using Brush barra = new SolidBrush(Color.FromArgb(78, 121, 167)); using Font fuente = new("Segoe UI", 7.5F); using Brush texto = new SolidBrush(Color.FromArgb(70, 75, 80));
            int y = 40; foreach (var dato in datos) { string nombre = dato.Nombre.Length > 18 ? dato.Nombre[..18] + "…" : dato.Nombre; g.DrawString(nombre, fuente, texto, 12, y); int x = Math.Min(145, area.Width / 2); int ancho = (int)((area.Width - x - 42) * dato.Valor / maximo); g.FillRectangle(barra, x, y + 2, Math.Max(2, ancho), 12); g.DrawString(dato.Valor.ToString("0"), fuente, texto, x + ancho + 5, y); y += 24; }
        }

        private void DibujarDonutSucursales(Graphics g, Rectangle area)
        {
            DibujarTitulo(g, "Ventas por sucursal"); if (!ventasPorSucursal.Any()) { DibujarVacio(g, area); return; }
            Color[] colores = { Color.FromArgb(78, 121, 167), Color.FromArgb(242, 142, 43), Color.FromArgb(89, 161, 79), Color.FromArgb(225, 87, 89) }; decimal total = ventasPorSucursal.Sum(x => x.Valor); int d = Math.Min(area.Height - 60, area.Width / 2); Rectangle donut = new(16, 40, d, d); float inicio = -90;
            for (int i = 0; i < ventasPorSucursal.Count; i++) { float angulo = (float)(ventasPorSucursal[i].Valor / total * 360); using Pen p = new(colores[i % colores.Length], Math.Max(18, d / 3)); g.DrawArc(p, donut, inicio, angulo); inicio += angulo; }
            using Font fuente = new("Segoe UI", 8F); using Brush texto = new SolidBrush(Color.FromArgb(70, 75, 80)); for (int i = 0; i < ventasPorSucursal.Count; i++) { int y = 48 + i * 24; using Brush b = new SolidBrush(colores[i % colores.Length]); g.FillEllipse(b, donut.Right + 16, y + 3, 10, 10); string nombre = ventasPorSucursal[i].Nombre.Length > 18 ? ventasPorSucursal[i].Nombre[..18] + "…" : ventasPorSucursal[i].Nombre; g.DrawString($"{nombre}  {ventasPorSucursal[i].Valor / total:P0}", fuente, texto, donut.Right + 31, y); }
        }

        private static void DibujarBarrasVerticales(Graphics g, Rectangle area, List<(string Etiqueta, decimal Valor)> datos, Color colorBarra, Color? colorBorde, bool esMoneda)
        {
            if (!datos.Any(dato => dato.Valor > 0)) { DibujarVacio(g, area); return; }
            const int margenIzquierdo = 58;
            Rectangle plot = new(margenIzquierdo, 34, Math.Max(30, area.Width - margenIzquierdo - 18), Math.Max(30, area.Height - 72));
            decimal maximo = Math.Max(1m, datos.Max(dato => dato.Valor));
            using Font fuente = new("Segoe UI", 7F);
            using Brush texto = new SolidBrush(Color.FromArgb(105, 110, 116));
            using Brush barra = new SolidBrush(colorBarra);
            using Pen guia = new(Color.FromArgb(222, 225, 228));
            using Pen? borde = colorBorde.HasValue ? new Pen(colorBorde.Value) : null;

            for (int marca = 0; marca <= 4; marca++)
            {
                decimal valor = maximo * (4 - marca) / 4m;
                int y = plot.Top + plot.Height * marca / 4;
                g.DrawLine(guia, plot.Left, y, plot.Right, y);
                string etiqueta = esMoneda ? FormatearNumeroCompacto(valor) : Math.Round(valor).ToString(CultureInfo.CurrentCulture);
                SizeF medida = g.MeasureString(etiqueta, fuente);
                g.DrawString(etiqueta, fuente, texto, Math.Max(0, plot.Left - medida.Width - 7), y - medida.Height / 2);
            }

            int paso = Math.Max(1, plot.Width / datos.Count);
            int anchoBarra = Math.Max(3, Math.Min(Math.Max(3, paso - 8), 34));
            for (int i = 0; i < datos.Count; i++)
            {
                int alto = (int)Math.Round(plot.Height * datos[i].Valor / maximo);
                int x = plot.Left + i * paso + Math.Max(1, (paso - anchoBarra) / 2);
                Rectangle rectangulo = new(x, plot.Bottom - alto, anchoBarra, alto);
                g.FillRectangle(barra, rectangulo);
                if (borde != null) g.DrawRectangle(borde, rectangulo);
                SizeF medida = g.MeasureString(datos[i].Etiqueta, fuente);
                if (medida.Width <= paso + 3)
                    g.DrawString(datos[i].Etiqueta, fuente, texto, x + (anchoBarra - medida.Width) / 2, plot.Bottom + 5);
            }
        }

        private static string FormatearNumeroCompacto(decimal valor)
        {
            if (valor >= 1000000m) return "$" + (valor / 1000000m).ToString("0.#", CultureInfo.CurrentCulture) + "M";
            if (valor >= 1000m) return "$" + (valor / 1000m).ToString("0.#", CultureInfo.CurrentCulture) + "k";
            return "$" + valor.ToString("0", CultureInfo.CurrentCulture);
        }
        private static void DibujarTitulo(Graphics g, string titulo) { using Font f = new("Segoe UI", 9F, FontStyle.Bold); using Brush b = new SolidBrush(Color.FromArgb(55, 59, 64)); g.DrawString(titulo, f, b, 12, 10); }
        private static void DibujarVacio(Graphics g, Rectangle area) { using Font f = new("Segoe UI", 8F); using Brush b = new SolidBrush(Color.Gray); g.DrawString("Sin datos para el período.", f, b, 12, area.Height / 2); }

        private static DataGridView CrearGrilla() => new() { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AllowUserToResizeRows = false, RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal };

        // Configura columnas dinámicas y separa los importes de los datos descriptivos.
        private static void ConfigurarColumnas(DataGridView grilla, IEnumerable<string> columnas)
        {
            grilla.Columns.Clear();
            foreach (string nombre in columnas)
            {
                DataGridViewColumn columna = nombre is "Ver detalle" ? new DataGridViewButtonColumn { Name = nombre, HeaderText = "Acción", Text = nombre, UseColumnTextForButtonValue = true } : new DataGridViewTextBoxColumn { Name = nombre, HeaderText = nombre };
                int indice = grilla.Columns.Add(columna);
                if (nombre is "N°" or "Stock" or "Stock mínimo" or "Diferencia" or "Unidades") grilla.Columns[indice].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                if (nombre is "Subtotal" or "Descuento" or "Total" or "Importe" or "Precio promedio") { grilla.Columns[indice].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; grilla.Columns[indice].DefaultCellStyle.Format = "C2"; }
                if (nombre == "Participación") { grilla.Columns[indice].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; grilla.Columns[indice].DefaultCellStyle.Format = "P2"; }
                if (nombre == "Fecha") grilla.Columns[indice].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                if (nombre == "Producto") grilla.Columns[indice].FillWeight = 30;
                else if (nombre is "Categoría" or "Marca") grilla.Columns[indice].FillWeight = 15;
                else if (nombre == "Unidades") grilla.Columns[indice].FillWeight = 10;
                else if (nombre == "Precio promedio" || nombre == "Importe") grilla.Columns[indice].FillWeight = 15;
                else if (nombre == "Participación") grilla.Columns[indice].FillWeight = 14;
            }
            AplicarEstiloGrilla(grilla);
        }

        // Mantiene encabezados, padding y altura de fila coherentes con el resto del sistema.
        private static void AplicarEstiloGrilla(DataGridView grilla)
        {
            grilla.EnableHeadersVisualStyles = false; grilla.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; grilla.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(82, 88, 94); grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold); grilla.ColumnHeadersHeight = 38; grilla.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0); grilla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 229, 232); grilla.DefaultCellStyle.SelectionForeColor = Color.FromArgb(40, 44, 48); grilla.RowTemplate.Height = 36; grilla.GridColor = Color.FromArgb(224, 227, 230);
        }

        // Resuelve filtros opcionales sin dejar que la Vista altere el alcance que valida la lógica.
        private int? ObtenerSucursalSeleccionada() => cmbSucursal.Visible && cmbSucursal.SelectedItem is OpcionSucursal opcion ? opcion.IdSucursal : null;
        private int? ObtenerUsuarioSeleccionado() => cmbVendedor.Visible && cmbVendedor.SelectedItem is OpcionUsuario opcion ? opcion.IdUsuario : null;
        // Centraliza mensajes visuales y evita MessageBox fuera del patrón del sistema.
        private void MostrarMensaje(string titulo, string mensaje) { using FormMensaje dialogo = new(titulo, mensaje); dialogo.ShowDialog(formPrincipal); }
        private class OpcionSucursal { public int? IdSucursal { get; set; } public string Nombre { get; set; } = string.Empty; }
        private class OpcionUsuario { public int? IdUsuario { get; set; } public string Nombre { get; set; } = string.Empty; }
    }
}
