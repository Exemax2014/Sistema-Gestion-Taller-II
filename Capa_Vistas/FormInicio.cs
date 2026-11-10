using System.Globalization;
using Capa_Logica;

namespace Capa_Vistas
{
    // Muestra el resumen operativo y las comunicaciones internas del usuario actual.
    public partial class FormInicio : Form
    {
        private readonly TableLayoutPanel tlpDashboard = new() { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 1, GrowStyle = TableLayoutPanelGrowStyle.FixedSize, BackColor = Color.Transparent };
        private readonly TableLayoutPanel tlpEncabezado = new() { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 4, GrowStyle = TableLayoutPanelGrowStyle.FixedSize, BackColor = Color.Transparent };
        private readonly TableLayoutPanel tlpTarjetas = new() { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, GrowStyle = TableLayoutPanelGrowStyle.FixedSize, BackColor = Color.Transparent };
        private readonly TableLayoutPanel tlpAccesos = new() { Dock = DockStyle.Top, Height = 54, GrowStyle = TableLayoutPanelGrowStyle.FixedSize, BackColor = Color.Transparent };
        private readonly TableLayoutPanel tlpGraficos = new()
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            BackColor = Color.Transparent,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };
        private readonly FormPrincipal formPrincipal;
        private readonly DashboardLogica dashboardLogica = new DashboardLogica();
        private List<DashboardSerieModelo> serieSemanal = new();
        private List<DashboardProductoModelo> productosMasVendidos = new();
        private bool puedePublicarAvisos;
        private bool puedeVerVentas;
        private bool puedeVerProductos;
        private bool puedeVerClientes;
        private bool puedeRealizarVentas;
        private bool puedeVerAvisos;
        private bool editandoAviso;
        private bool vistaCargada;
        private readonly Button btnNuevoAviso = new();
        private readonly Button btnVolverAviso = new();
        private readonly Panel pnlAvisosCabecera = new() { Dock = DockStyle.Top, Height = 36, BackColor = Color.White };
        private readonly Panel pnlAvisosListado = new() { Dock = DockStyle.Fill, BackColor = Color.White };
        private readonly Panel pnlAvisoAlcance = new() { Dock = DockStyle.Fill, BackColor = Color.White };
        private readonly TableLayoutPanel tlpAvisos = new() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 1, GrowStyle = TableLayoutPanelGrowStyle.FixedSize, BackColor = Color.White };
        private readonly TableLayoutPanel tlpActividad = new() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, GrowStyle = TableLayoutPanelGrowStyle.FixedSize, BackColor = Color.White };
        private readonly FlowLayoutPanel pnlActividadCabecera = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, BackColor = Color.White };
        private readonly FlowLayoutPanel pnlAccesosCabecera = new() { Dock = DockStyle.Top, Height = 28, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, BackColor = Color.White };

        // Inicializa la vista con el principal como dueño de la navegación y de los mensajes.
        public FormInicio(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            ConfigurarEstructuraDashboard();
            ConfigurarContenedorGraficos();
            foreach (Control control in new[] { pnlPrincipal, pnlGraficos, pnlGraficoVentas, pnlGraficoProductos })
                HabilitarDobleBuffer(control);
            this.formPrincipal = formPrincipal;
            ConfigurarEventos();
            ConfigurarGrillas();
            PrepararVistaInicial();
        }

        private void ConfigurarEstructuraDashboard()
        {
            pnlPrincipal.AutoScroll = true;
            pnlPrincipal.Padding = new Padding(24);

            foreach (Control control in new Control[]
            {
                pnlEncabezado, pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaStock, pnlTarjetaProductos,
                pnlAccesos, pnlGraficos, pnlActividad, pnlAvisos
            })
                pnlPrincipal.Controls.Remove(control);

            foreach (Control control in new Control[] { lblBienvenida, lblPerfilSucursal, lblSubtitulo, pnlLineaDorada })
                pnlEncabezado.Controls.Remove(control);
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, 3F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            foreach (Control control in new Control[] { lblBienvenida, lblPerfilSucursal, lblSubtitulo, pnlLineaDorada })
                control.Dock = DockStyle.Fill;
            lblBienvenida.AutoSize = false;
            lblBienvenida.AutoEllipsis = true;
            lblBienvenida.TextAlign = ContentAlignment.MiddleLeft;
            lblPerfilSucursal.AutoSize = false;
            lblPerfilSucursal.AutoEllipsis = true;
            lblPerfilSucursal.TextAlign = ContentAlignment.MiddleRight;
            lblSubtitulo.AutoSize = false;
            lblSubtitulo.TextAlign = ContentAlignment.MiddleLeft;
            tlpEncabezado.Controls.Add(lblBienvenida, 0, 0);
            tlpEncabezado.Controls.Add(lblPerfilSucursal, 1, 0);
            tlpEncabezado.Controls.Add(lblSubtitulo, 0, 1);
            tlpEncabezado.SetColumnSpan(lblSubtitulo, 2);
            tlpEncabezado.Controls.Add(pnlLineaDorada, 0, 2);
            tlpEncabezado.SetColumnSpan(pnlLineaDorada, 2);
            pnlEncabezado.Controls.Add(tlpEncabezado);

            tlpDashboard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDashboard.RowCount = 6;
            for (int i = 0; i < 6; i++) tlpDashboard.RowStyles.Add(new RowStyle(i is 0 or 3 or 4 or 5 ? SizeType.Absolute : SizeType.AutoSize, i switch { 0 => 100, 3 => 285, 4 => 300, 5 => 380, _ => 0 }));
            tlpDashboard.Controls.Add(pnlEncabezado, 0, 0);
            tlpDashboard.Controls.Add(tlpTarjetas, 0, 1);
            tlpDashboard.Controls.Add(pnlAccesos, 0, 2);
            tlpDashboard.Controls.Add(pnlGraficos, 0, 3);
            tlpDashboard.Controls.Add(pnlActividad, 0, 4);
            tlpDashboard.Controls.Add(pnlAvisos, 0, 5);
            foreach (Control bloque in new Control[] { pnlEncabezado, tlpTarjetas, pnlAccesos, pnlGraficos, pnlActividad, pnlAvisos })
                bloque.Margin = new Padding(0, 0, 0, 14);
            pnlEncabezado.Dock = DockStyle.Fill;
            tlpTarjetas.Dock = DockStyle.Fill;
            pnlAccesos.Dock = DockStyle.Fill;
            pnlGraficos.Dock = DockStyle.Fill;
            pnlActividad.Dock = DockStyle.Fill;
            pnlAvisos.Dock = DockStyle.Fill;

            tlpTarjetas.ColumnCount = 4;
            tlpTarjetas.RowCount = 2;
            for (int i = 0; i < 4; i++) tlpTarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpTarjetas.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            tlpTarjetas.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            foreach (Panel tarjeta in new[] { pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaStock, pnlTarjetaProductos })
            {
                tarjeta.Dock = DockStyle.Fill;
                tarjeta.Margin = new Padding(0, 0, 12, 0);
                tlpTarjetas.Controls.Add(tarjeta);
            }

            pnlAccesos.AutoSize = false;
            pnlAccesos.Padding = new Padding(10);
            pnlAccesos.Controls.Remove(lblAccesosTitulo);
            pnlAccesos.Controls.Remove(lblAccesosDescripcion);
            pnlAccesos.Controls.Remove(btnNuevaVenta);
            pnlAccesos.Controls.Remove(btnProductos);
            pnlAccesos.Controls.Remove(btnClientes);
            pnlAccesosCabecera.Controls.Add(lblAccesosTitulo);
            pnlAccesosCabecera.Controls.Add(lblAccesosDescripcion);
            pnlAccesos.Controls.Add(tlpAccesos);
            pnlAccesos.Controls.Add(pnlAccesosCabecera);
            tlpAccesos.Dock = DockStyle.Top;
            tlpAccesos.ColumnCount = 3;
            tlpAccesos.RowCount = 3;
            for (int i = 0; i < 3; i++) tlpAccesos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            tlpAccesos.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpAccesos.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tlpAccesos.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            foreach (Button boton in new[] { btnNuevaVenta, btnProductos, btnClientes })
            {
                boton.Dock = DockStyle.Fill;
                boton.Margin = new Padding(4, 6, 8, 0);
                tlpAccesos.Controls.Add(boton);
            }

            pnlActividad.Padding = new Padding(14, 10, 14, 14);
            pnlActividad.Controls.Remove(lblActividadTitulo);
            pnlActividad.Controls.Remove(lblActividadDescripcion);
            pnlActividad.Controls.Remove(pnlActividadContenido);
            pnlActividadCabecera.Controls.Add(lblActividadTitulo);
            pnlActividadCabecera.Controls.Add(lblActividadDescripcion);
            tlpActividad.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpActividad.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpActividad.Controls.Add(pnlActividadCabecera, 0, 0);
            tlpActividad.Controls.Add(pnlActividadContenido, 0, 1);
            pnlActividadContenido.Dock = DockStyle.Fill;
            pnlActividad.Controls.Add(tlpActividad);

            ConfigurarEstructuraAvisos();
            pnlPrincipal.Controls.Add(tlpDashboard);
            // El encabezado permanece disponible; el dashboard dependiente de datos
            // se expande al terminar Load y aplicar el layout del host.
            for (int fila = 1; fila < tlpDashboard.RowCount; fila++)
            {
                tlpDashboard.RowStyles[fila].SizeType = SizeType.Absolute;
                tlpDashboard.RowStyles[fila].Height = 0F;
            }
        }

        private void ConfigurarEstructuraAvisos()
        {
            foreach (Control control in new Control[] { dgvAvisos, lblAvisosVacio, lblAvisosTitulo, txtAvisoTitulo, txtAvisoMensaje, lblAvisoMensaje, lblAvisoAlcance, lblAvisoSucursalFija, cmbAvisoSucursal, btnPublicarAviso })
                pnlAvisos.Controls.Remove(control);

            pnlAvisos.Padding = new Padding(14, 10, 14, 14);
            pnlAvisosCabecera.Controls.Add(lblAvisosTitulo);
            pnlAvisosCabecera.Controls.Add(btnNuevoAviso);
            lblAvisosTitulo.Location = new Point(0, 6);
            btnNuevoAviso.Dock = DockStyle.Right;
            btnNuevoAviso.Width = 120;
            pnlAvisosListado.Controls.Add(dgvAvisos);
            pnlAvisosListado.Controls.Add(lblAvisosVacio);
            dgvAvisos.Dock = DockStyle.Fill;
            lblAvisosVacio.Dock = DockStyle.Fill;
            pnlAvisoAlcance.Controls.Add(cmbAvisoSucursal);
            pnlAvisoAlcance.Controls.Add(lblAvisoSucursalFija);
            cmbAvisoSucursal.Dock = DockStyle.Fill;
            lblAvisoSucursalFija.Dock = DockStyle.Fill;

            tlpAvisos.ColumnCount = 1;
            tlpAvisos.RowCount = 6;
            tlpAvisos.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpAvisos.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpAvisos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAvisos.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpAvisos.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tlpAvisos.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpAvisos.Controls.Add(txtAvisoTitulo, 0, 0);
            tlpAvisos.Controls.Add(lblAvisoMensaje, 0, 1);
            tlpAvisos.Controls.Add(txtAvisoMensaje, 0, 2);
            tlpAvisos.Controls.Add(lblAvisoAlcance, 0, 3);
            tlpAvisos.Controls.Add(pnlAvisoAlcance, 0, 4);
            Panel acciones = new() { Dock = DockStyle.Fill };
            acciones.Controls.Add(btnVolverAviso);
            acciones.Controls.Add(btnPublicarAviso);
            btnVolverAviso.Dock = DockStyle.Left;
            btnPublicarAviso.Dock = DockStyle.Right;
            btnVolverAviso.Width = 100;
            btnPublicarAviso.Width = 140;
            tlpAvisos.Controls.Add(acciones, 0, 5);
            txtAvisoTitulo.Dock = DockStyle.Fill;
            txtAvisoMensaje.Dock = DockStyle.Fill;
            lblAvisoMensaje.Dock = DockStyle.Fill;
            lblAvisoAlcance.Dock = DockStyle.Fill;
            pnlAvisosListado.Dock = DockStyle.Fill;
            tlpAvisos.Dock = DockStyle.Fill;
            pnlAvisosCabecera.Dock = DockStyle.Top;
            pnlAvisos.Controls.Add(tlpAvisos);
            pnlAvisos.Controls.Add(pnlAvisosListado);
            pnlAvisos.Controls.Add(pnlAvisosCabecera);
            pnlAvisosListado.Visible = true;
            tlpAvisos.Visible = false;
        }

        private void ConfigurarContenedorGraficos()
        {
            pnlGraficos.Controls.Remove(pnlGraficoVentas);
            pnlGraficos.Controls.Remove(pnlGraficoProductos);
            pnlGraficos.Controls.Remove(pnlGraficoIngresos);
            tlpGraficos.ColumnCount = 2;
            tlpGraficos.RowCount = 2;
            tlpGraficos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpGraficos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpGraficos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpGraficos.RowStyles.Add(new RowStyle(SizeType.Percent, 0F));
            pnlGraficoVentas.Dock = DockStyle.Fill;
            pnlGraficoProductos.Dock = DockStyle.Fill;
            pnlGraficoVentas.Margin = new Padding(0, 0, 8, 0);
            pnlGraficoProductos.Margin = new Padding(8, 0, 0, 0);
            pnlGraficoIngresos.Visible = false;
            if (!ReferenceEquals(pnlGraficoVentas.Parent, tlpGraficos))
                tlpGraficos.Controls.Add(pnlGraficoVentas);
            if (!ReferenceEquals(pnlGraficoProductos.Parent, tlpGraficos))
                tlpGraficos.Controls.Add(pnlGraficoProductos);
            tlpGraficos.SetColumn(pnlGraficoVentas, 0);
            tlpGraficos.SetRow(pnlGraficoVentas, 0);
            tlpGraficos.SetColumn(pnlGraficoProductos, 1);
            tlpGraficos.SetRow(pnlGraficoProductos, 0);
            pnlGraficos.Controls.Add(tlpGraficos);
        }

        // Enlaza interacción, recarga y pintura de los mini informes fuera del Designer.
        private void ConfigurarEventos()
        {
            Load += FormInicio_Load;
            Resize += FormInicio_Resize;
            btnNuevaVenta.Click += BtnNuevaVenta_Click;
            btnProductos.Click += BtnProductos_Click;
            btnClientes.Click += BtnClientes_Click;
            btnPublicarAviso.Click += BtnPublicarAviso_Click;
            btnNuevoAviso.Click += (_, _) => CambiarModoAvisos(true);
            btnVolverAviso.Click += (_, _) => CambiarModoAvisos(false);
            pnlGraficoVentas.Paint += PnlGraficoVentas_Paint;
            pnlGraficoIngresos.Paint += PnlGraficoIngresos_Paint;
            pnlGraficoProductos.Paint += PnlGraficoProductos_Paint;
        }

        private static void HabilitarDobleBuffer(Control control)
        {
            typeof(Control)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(control, true);
        }

        // Define grillas de sólo lectura para que Inicio no edite datos persistentes.
        private void ConfigurarGrillas()
        {
            ConfigurarGrilla(dgvActividad, new[] { "Fecha", "Cliente", "Vendedor", "Total" });
            ConfigurarGrilla(dgvAvisos, new[] { "Fecha", "Título", "Mensaje", "Autor", "Alcance" });
            dgvAvisos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvAvisos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAvisos.Columns[0].FillWeight = 65;
            dgvAvisos.Columns[1].FillWeight = 100;
            dgvAvisos.Columns[2].FillWeight = 230;
            dgvAvisos.Columns[3].FillWeight = 100;
            dgvAvisos.Columns[4].FillWeight = 135;
            lblActividadPlaceholder.Visible = false;
            btnNuevoAviso.Text = "Nuevo aviso";
            btnNuevoAviso.BackColor = Color.FromArgb(45, 49, 54);
            btnNuevoAviso.ForeColor = Color.White;
            btnNuevoAviso.FlatStyle = FlatStyle.Flat;
            btnNuevoAviso.FlatAppearance.BorderSize = 0;
            btnVolverAviso.Text = "Volver";
            btnVolverAviso.FlatStyle = FlatStyle.Flat;
        }

        // Aplica una presentación común y compacta a los dos listados del dashboard.
        private static void ConfigurarGrilla(DataGridView grilla, IEnumerable<string> columnas)
        {
            grilla.Columns.Clear();
            foreach (string columna in columnas) grilla.Columns.Add(columna, columna);
            grilla.EnableHeadersVisualStyles = false;
            grilla.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grilla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 228, 232);
            grilla.DefaultCellStyle.SelectionForeColor = Color.FromArgb(45, 49, 54);
            grilla.RowTemplate.Height = 30;
        }

        // Carga encabezado, permisos de accesos rápidos y los datos autorizados para la sesión.
        private void PrepararVistaInicial()
        {
            lblBienvenida.Text = $"Bienvenido, {SesionActual.Nombre}";
            lblPerfilSucursal.Text = ObtenerDescripcionSesion();
            puedeVerVentas = dashboardLogica.PuedeVerVentas();
            puedeVerProductos = dashboardLogica.PuedeVerProductos();
            puedeVerClientes = SesionActual.TienePermiso("CLIENTES_VER");
            puedeRealizarVentas = SesionActual.TienePermiso("VENTAS_REALIZAR");
            puedeVerAvisos = SesionActual.TienePermiso("AVISOS_VER");
            btnNuevaVenta.Visible = puedeRealizarVentas;
            btnProductos.Visible = puedeVerProductos;
            btnClientes.Visible = puedeVerClientes;
            AplicarPermisosDashboard();
            CargarDashboard();
        }

        // Ajusta la visibilidad de métricas y gráficos al permiso de consulta de cada módulo.
        private void AplicarPermisosDashboard()
        {
            pnlTarjetaVentas.Visible = puedeVerVentas;
            pnlTarjetaIngresos.Visible = puedeVerVentas;
            pnlTarjetaStock.Visible = puedeVerProductos;
            pnlTarjetaProductos.Visible = puedeVerProductos;
            pnlGraficoVentas.Visible = puedeVerVentas;
            pnlGraficoIngresos.Visible = false;
            pnlGraficoProductos.Visible = puedeVerVentas && puedeVerProductos;
            pnlActividad.Visible = puedeVerVentas;
            pnlAvisos.Visible = puedePublicarAvisos || puedeVerAvisos;
        }

        // Consulta la fachada lógica y proyecta únicamente métricas autorizadas para la sesión.
        private void CargarDashboard()
        {
            try
            {
                DashboardResumenModelo resumen = dashboardLogica.ObtenerResumen();
                lblVentasHoyValor.Text = puedeVerVentas ? resumen.VentasHoy.ToString() : "—";
                lblIngresosHoyValor.Text = puedeVerVentas ? resumen.IngresosHoy.ToString("C2", CultureInfo.CurrentCulture) : "—";
                lblStockBajoValor.Text = puedeVerProductos ? resumen.StockBajo.ToString() : "—";
                lblProductosValor.Text = puedeVerProductos ? resumen.ProductosActivos.ToString() : "—";
                serieSemanal = dashboardLogica.ObtenerSerieSemanal();
                productosMasVendidos = dashboardLogica.ObtenerProductosMasVendidos();
                CargarActividad(); CargarAvisos(); ConfigurarAlcanceAviso();
                AplicarPermisosDashboard();
            }
            catch (Exception)
            {
                MostrarMensaje("No se pudo cargar el inicio", "No fue posible obtener el resumen del sistema. Intentá nuevamente.");
            }
        }

        // Proyecta los campos útiles de las últimas cinco operaciones para el listado visual.
        private void CargarActividad()
        {
            dgvActividad.Rows.Clear();
            foreach (DashboardVentaRecienteModelo venta in dashboardLogica.ObtenerActividadReciente())
                dgvActividad.Rows.Add(venta.FechaHora.ToString("dd/MM HH:mm"), venta.Cliente, venta.Vendedor, venta.Total.ToString("C2", CultureInfo.CurrentCulture));
        }

        // Muestra los avisos que SQL autoriza para el permiso y alcance actuales de la sesión.
        private void CargarAvisos()
        {
            dgvAvisos.Rows.Clear();
            List<AvisoModelo> avisos = dashboardLogica.ListarAvisos();

            foreach (AvisoModelo aviso in avisos)
            {
                dgvAvisos.Rows.Add(
                    aviso.FechaCreacion.ToString("dd/MM HH:mm"),
                    aviso.Titulo,
                    aviso.Mensaje,
                    aviso.Autor,
                    aviso.Alcance
                );
            }

            dgvAvisos.Visible = avisos.Count > 0;
            lblAvisosVacio.Visible = avisos.Count == 0;
        }

        // Configura el alcance publicable según la sucursal fija o el alcance global de la sesión.
        private void ConfigurarAlcanceAviso()
        {
            bool alcanceGlobal = SesionActual.AlcanceGlobal;
            puedePublicarAvisos = dashboardLogica.PuedePublicarAvisos();

            List<SucursalAvisoModelo> opciones = new()
            {
                new SucursalAvisoModelo { IdSucursal = null, Nombre = "Todas las sucursales" }
            };

            if (puedePublicarAvisos && alcanceGlobal)
            {
                opciones.AddRange(dashboardLogica.ObtenerSucursalesParaAviso());
                cmbAvisoSucursal.DataSource = opciones;
                cmbAvisoSucursal.DisplayMember = nameof(SucursalAvisoModelo.Nombre);
                cmbAvisoSucursal.ValueMember = nameof(SucursalAvisoModelo.IdSucursal);
                cmbAvisoSucursal.SelectedIndex = opciones.FindIndex(opcion => opcion.IdSucursal == SesionActual.IdSucursalOperativa);
                if (cmbAvisoSucursal.SelectedIndex < 0) cmbAvisoSucursal.SelectedIndex = 0;
            }

            txtAvisoTitulo.Visible = puedePublicarAvisos && editandoAviso;
            txtAvisoMensaje.Visible = puedePublicarAvisos && editandoAviso;
            lblAvisoMensaje.Visible = puedePublicarAvisos && editandoAviso;
            lblAvisoAlcance.Visible = puedePublicarAvisos && alcanceGlobal && editandoAviso;
            cmbAvisoSucursal.Visible = puedePublicarAvisos && alcanceGlobal && editandoAviso;
            lblAvisoSucursalFija.Visible = puedePublicarAvisos && !alcanceGlobal && editandoAviso;
            lblAvisoSucursalFija.Text = $"Sucursal: {SesionActual.Sucursal}";
            btnPublicarAviso.Visible = puedePublicarAvisos && editandoAviso;
            btnNuevoAviso.Visible = puedePublicarAvisos && !editandoAviso;
            btnVolverAviso.Visible = puedePublicarAvisos && editandoAviso;
        }

        // Describe el alcance realmente seleccionado, sin inferirlo desde el nombre del perfil.
        private string ObtenerDescripcionSesion()
        {
            string perfil = string.IsNullOrWhiteSpace(SesionActual.Perfil) ? "Usuario" : SesionActual.Perfil;
            string alcance = SesionActual.IdSucursalOperativa.HasValue ? SesionActual.SucursalOperativa : "Todo el negocio";
            return $"{perfil} · {alcance}";
        }

        // Cambia únicamente la estructura de filas y columnas; los controles permanecen montados.
        private void AjustarLayoutResponsive()
        {
            int ancho = Math.Max(0, pnlPrincipal.ClientSize.Width - pnlPrincipal.Padding.Horizontal);
            lblAccesosDescripcion.Visible = ancho >= 560;
            lblActividadDescripcion.Visible = ancho >= 700;
            bool encabezadoCompacto = ancho < 700;
            tlpEncabezado.SuspendLayout();
            tlpEncabezado.ColumnStyles.Clear();
            tlpEncabezado.RowStyles.Clear();
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, encabezadoCompacto ? 100F : 70F));
            tlpEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, encabezadoCompacto ? 0F : 30F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, encabezadoCompacto ? 22F : 3F));
            tlpEncabezado.RowStyles.Add(new RowStyle(SizeType.Absolute, encabezadoCompacto ? 3F : 0F));
            if (encabezadoCompacto)
            {
                tlpEncabezado.SetCellPosition(lblPerfilSucursal, new TableLayoutPanelCellPosition(0, 1));
                tlpEncabezado.SetCellPosition(lblSubtitulo, new TableLayoutPanelCellPosition(0, 2));
                tlpEncabezado.SetColumnSpan(lblSubtitulo, 1);
                tlpEncabezado.SetCellPosition(pnlLineaDorada, new TableLayoutPanelCellPosition(0, 3));
                tlpEncabezado.SetColumnSpan(pnlLineaDorada, 1);
                tlpDashboard.RowStyles[0].Height = 97F;
            }
            else
            {
                tlpEncabezado.SetCellPosition(lblPerfilSucursal, new TableLayoutPanelCellPosition(1, 0));
                tlpEncabezado.SetCellPosition(lblSubtitulo, new TableLayoutPanelCellPosition(0, 1));
                tlpEncabezado.SetColumnSpan(lblSubtitulo, 2);
                tlpEncabezado.SetCellPosition(pnlLineaDorada, new TableLayoutPanelCellPosition(0, 2));
                tlpEncabezado.SetColumnSpan(pnlLineaDorada, 2);
                tlpDashboard.RowStyles[0].Height = 75F;
            }
            tlpEncabezado.ResumeLayout(true);
            bool cuatroColumnasTarjetas = ancho >= 1000;
            int columnasTarjetas = cuatroColumnasTarjetas ? 4 : 2;
            List<Panel> tarjetas = new() { pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaStock, pnlTarjetaProductos };
            int filasTarjetas = cuatroColumnasTarjetas ? 1 : 2;

            tlpTarjetas.SuspendLayout();
            tlpTarjetas.ColumnStyles.Clear();
            for (int columna = 0; columna < 4; columna++)
                tlpTarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpTarjetas.RowStyles[0].Height = 110F;
            tlpTarjetas.RowStyles[1].Height = cuatroColumnasTarjetas ? 0F : 110F;
            for (int i = 0; i < tarjetas.Count; i++)
            {
                int columna = cuatroColumnasTarjetas ? i : (i % 2) * 2;
                int fila = cuatroColumnasTarjetas ? 0 : i / 2;
                tlpTarjetas.SetColumn(tarjetas[i], columna);
                tlpTarjetas.SetRow(tarjetas[i], fila);
                tlpTarjetas.SetColumnSpan(tarjetas[i], cuatroColumnasTarjetas ? 1 : 2);
            }
            tlpTarjetas.ResumeLayout(true);

            List<Button> botones = new();
            if (puedeRealizarVentas) botones.Add(btnNuevaVenta);
            if (puedeVerProductos) botones.Add(btnProductos);
            if (puedeVerClientes) botones.Add(btnClientes);
            int columnasAccesos = ancho >= 900 ? 3 : ancho >= 560 ? 2 : 1;
            int filasAccesos = botones.Count == 0 ? 0 : (botones.Count + columnasAccesos - 1) / columnasAccesos;
            tlpAccesos.SuspendLayout();
            tlpAccesos.ColumnStyles.Clear();
            for (int columna = 0; columna < 3; columna++)
                tlpAccesos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, columna < columnasAccesos ? 100F / columnasAccesos : 0F));
            for (int fila = 0; fila < 3; fila++)
                tlpAccesos.RowStyles[fila].Height = fila < filasAccesos ? 50F : 0F;
            foreach (Button boton in new[] { btnNuevaVenta, btnProductos, btnClientes })
            {
                int indice = botones.IndexOf(boton);
                tlpAccesos.SetColumn(boton, indice < 0 ? 0 : indice % columnasAccesos);
                tlpAccesos.SetRow(boton, indice < 0 ? 0 : indice / columnasAccesos);
                tlpAccesos.SetColumnSpan(boton, 1);
            }
            tlpAccesos.ResumeLayout(true);
            tlpAccesos.Height = filasAccesos * 50;

            bool mostrarAccesos = botones.Count > 0;
            bool mostrarGraficos = puedeVerVentas;
            pnlAccesos.Visible = mostrarAccesos;
            pnlGraficos.Visible = mostrarGraficos;
            pnlActividad.Visible = puedeVerVentas;
            pnlAvisos.Visible = puedePublicarAvisos || puedeVerAvisos;

            bool dosColumnasGraficos = ancho >= 760 && pnlGraficoVentas.Visible && pnlGraficoProductos.Visible;
            bool unGrafico = pnlGraficoVentas.Visible != pnlGraficoProductos.Visible;
            int filasGraficos = dosColumnasGraficos || unGrafico ? 1 : 2;
            tlpGraficos.SuspendLayout();
            tlpGraficos.ColumnStyles.Clear();
            tlpGraficos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, dosColumnasGraficos ? 50F : 100F));
            tlpGraficos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, dosColumnasGraficos ? 50F : 0F));
            tlpGraficos.RowStyles[0].SizeType = SizeType.Percent;
            tlpGraficos.RowStyles[1].SizeType = SizeType.Percent;
            tlpGraficos.RowStyles[0].Height = filasGraficos == 1 ? 100F : 50F;
            tlpGraficos.RowStyles[1].Height = filasGraficos == 1 ? 0F : 50F;
            tlpGraficos.SetColumn(pnlGraficoVentas, 0);
            tlpGraficos.SetRow(pnlGraficoVentas, 0);
            tlpGraficos.SetColumn(pnlGraficoProductos, dosColumnasGraficos ? 1 : 0);
            tlpGraficos.SetRow(pnlGraficoProductos, dosColumnasGraficos ? 0 : unGrafico ? 0 : 1);
            tlpGraficos.SetColumnSpan(pnlGraficoProductos, dosColumnasGraficos ? 1 : 2);
            tlpGraficos.SetColumnSpan(pnlGraficoVentas, dosColumnasGraficos ? 1 : 2);
            tlpGraficos.ResumeLayout(true);

            int altoGraficos = !mostrarGraficos ? 0 : filasGraficos == 1 ? 299 : 598;
            tlpDashboard.RowStyles[1].SizeType = SizeType.Absolute;
            tlpDashboard.RowStyles[1].Height = filasTarjetas * 110 + 12;
            tlpDashboard.RowStyles[2].SizeType = SizeType.Absolute;
            tlpDashboard.RowStyles[2].Height = mostrarAccesos ? tlpAccesos.Height + 66 : 0;
            tlpDashboard.RowStyles[3].Height = altoGraficos;
            tlpDashboard.RowStyles[4].Height = pnlActividad.Visible ? 314 : 0;
            tlpDashboard.RowStyles[5].Height = pnlAvisos.Visible ? 394 : 0;
        }

        private void CambiarModoAvisos(bool editar)
        {
            editandoAviso = editar;
            if (!editar) { txtAvisoTitulo.Clear(); txtAvisoMensaje.Clear(); }
            pnlAvisosListado.Visible = !editar;
            tlpAvisos.Visible = editar;
            txtAvisoTitulo.Visible = editar;
            txtAvisoMensaje.Visible = editar;
            lblAvisoMensaje.Visible = editar;
            lblAvisoAlcance.Visible = editar && SesionActual.AlcanceGlobal;
            cmbAvisoSucursal.Visible = editar && SesionActual.AlcanceGlobal;
            lblAvisoSucursalFija.Visible = editar && !SesionActual.AlcanceGlobal;
            btnPublicarAviso.Visible = editar;
            btnVolverAviso.Visible = editar;
            btnNuevoAviso.Visible = !editar;
            dgvAvisos.Visible = !editar && dgvAvisos.Rows.Count > 0;
            lblAvisosVacio.Visible = !editar && dgvAvisos.Rows.Count == 0;
        }

        // Dibuja barras de ventas con la misma escala para el período semanal.
        private void PnlGraficoVentas_Paint(object? sender, PaintEventArgs e) => DibujarBarras(e.Graphics, pnlGraficoVentas.ClientRectangle, "Ventas últimos 7 días", serieSemanal.Select(x => (x.Fecha.ToString("dd"), (decimal)x.Ventas)).ToList(), Color.FromArgb(45, 49, 54));

        // Dibuja importes diarios sin reemplazar los reportes detallados del módulo Reportes.
        private void PnlGraficoIngresos_Paint(object? sender, PaintEventArgs e) => DibujarBarras(e.Graphics, pnlGraficoIngresos.ClientRectangle, "Ingresos últimos 7 días", serieSemanal.Select(x => (x.Fecha.ToString("dd"), x.Ingresos)).ToList(), Color.FromArgb(190, 137, 45));

        // Dibuja el ranking breve de productos vendidos dentro del alcance operativo.
        private void PnlGraficoProductos_Paint(object? sender, PaintEventArgs e) => DibujarBarras(e.Graphics, pnlGraficoProductos.ClientRectangle, "Productos más vendidos", productosMasVendidos.Select(x => (x.Producto, (decimal)x.UnidadesVendidas)).ToList(), Color.FromArgb(74, 120, 88));

        // Renderiza un gráfico liviano sin incorporar dependencias externas de gráficos.
        private static readonly Color AzulDatos = Color.FromArgb(78, 121, 167);
        private static readonly Color VerdeDatos = Color.FromArgb(89, 161, 79);
        private static readonly Color[] PaletaDatos = { AzulDatos, Color.FromArgb(242, 142, 43), VerdeDatos, Color.FromArgb(225, 87, 89), Color.FromArgb(176, 122, 161), Color.FromArgb(118, 183, 178) };

        private static void DibujarBarras(Graphics graphics, Rectangle area, string titulo, List<(string Etiqueta, decimal Valor)> datos, Color color)
        {
            if (titulo.StartsWith("Ventas")) { DibujarGraficoBarras(graphics, area, titulo, datos, AzulDatos); return; }
            if (titulo.StartsWith("Productos")) { DibujarProductosHorizontales(graphics, area, titulo, datos); return; }
            DibujarGraficoBarras(graphics, area, titulo, datos, VerdeDatos);
        }

        private static void DibujarGraficoLinea(Graphics graphics, Rectangle area, string titulo, List<(string Etiqueta, decimal Valor)> datos)
        {
            using Font tituloFuente = new("Segoe UI", 9F, FontStyle.Bold); using Font fuente = new("Segoe UI", 7F); using Brush texto = new SolidBrush(Color.FromArgb(55, 59, 64));
            graphics.DrawString(titulo, tituloFuente, texto, 12, 10);
            if (!datos.Any(x => x.Valor > 0)) { graphics.DrawString("Sin datos para el período.", fuente, texto, 12, 75); return; }
            Rectangle plot = new(30, 34, Math.Max(30, area.Width - 52), Math.Max(30, area.Height - 64));
            DibujarGuias(graphics, plot); decimal maximo = Math.Max(1, datos.Max(x => x.Valor));
            PointF[] puntos = datos.Select((dato, i) => new PointF(datos.Count == 1 ? plot.Left + plot.Width / 2F : plot.Left + i * plot.Width / (float)(datos.Count - 1), plot.Bottom - (float)(dato.Valor / maximo) * plot.Height)).ToArray();
            using Pen linea = new(AzulDatos, 2.4F); using Brush punto = new SolidBrush(AzulDatos);
            if (puntos.Length > 1) graphics.DrawLines(linea, puntos);
            for (int i = 0; i < puntos.Length; i++) { graphics.FillEllipse(punto, puntos[i].X - 3, puntos[i].Y - 3, 6, 6); string etiqueta = datos[i].Etiqueta; SizeF medida = graphics.MeasureString(etiqueta, fuente); graphics.DrawString(etiqueta, fuente, texto, Math.Clamp(puntos[i].X - medida.Width / 2, plot.Left, plot.Right - medida.Width), plot.Bottom + 5); if (plot.Width / datos.Count >= 50 && puntos[i].Y > 48) { string valor = datos[i].Valor.ToString("0"); SizeF valorMedida = graphics.MeasureString(valor, fuente); graphics.DrawString(valor, fuente, texto, puntos[i].X - valorMedida.Width / 2, puntos[i].Y - 16); } }
        }

        private static void DibujarGraficoBarras(Graphics graphics, Rectangle area, string titulo, List<(string Etiqueta, decimal Valor)> datos, Color color)
        {
            using Font tituloFuente = new("Segoe UI", 9F, FontStyle.Bold); using Font fuente = new("Segoe UI", 7F); using Brush texto = new SolidBrush(Color.FromArgb(55, 59, 64));
            graphics.DrawString(titulo, tituloFuente, texto, 12, 10);
            if (!datos.Any(x => x.Valor > 0)) { graphics.DrawString("Sin datos para el período.", fuente, texto, 12, 75); return; }
            Rectangle plot = new(54, 34, Math.Max(30, area.Width - 76), Math.Max(30, area.Height - 64));
            decimal maximo = Math.Max(1, datos.Max(x => x.Valor));
            bool esMoneda = titulo.StartsWith("Ingresos");
            DibujarGuias(graphics, plot);
            DibujarEjeY(graphics, plot, maximo, esMoneda, fuente, texto);
            int paso = Math.Max(1, plot.Width / datos.Count), ancho = Math.Max(5, Math.Min(30, paso - 8)); using Brush barra = new SolidBrush(color);
            for (int i = 0; i < datos.Count; i++) { int alto = (int)(plot.Height * datos[i].Valor / maximo), x = plot.Left + i * paso + (paso - ancho) / 2, y = plot.Bottom - alto; graphics.FillRectangle(barra, x, y, ancho, alto); string etiqueta = datos[i].Etiqueta; SizeF medida = graphics.MeasureString(etiqueta, fuente); graphics.DrawString(etiqueta, fuente, texto, Math.Clamp(x + ancho / 2F - medida.Width / 2, plot.Left, plot.Right - medida.Width), plot.Bottom + 5); if (paso >= 58 && y > 48) { string valor = esMoneda ? FormatearNumeroCompacto(datos[i].Valor) : datos[i].Valor.ToString("0", CultureInfo.CurrentCulture); SizeF valorMedida = graphics.MeasureString(valor, fuente); graphics.DrawString(valor, fuente, texto, x + ancho / 2F - valorMedida.Width / 2, y - 16); } }
        }

        private static void DibujarGraficoDonut(Graphics graphics, Rectangle area, string titulo, List<(string Etiqueta, decimal Valor)> datos)
        {
            using Font tituloFuente = new("Segoe UI", 9F, FontStyle.Bold); using Font fuente = new("Segoe UI", 7.5F); using Brush texto = new SolidBrush(Color.FromArgb(55, 59, 64)); graphics.DrawString(titulo, tituloFuente, texto, 12, 10);
            List<(string Etiqueta, decimal Valor)> top = datos.OrderByDescending(x => x.Valor).Take(5).ToList(); decimal otros = datos.Skip(5).Sum(x => x.Valor); if (otros > 0) top.Add(("Otros", otros)); if (!top.Any(x => x.Valor > 0)) { graphics.DrawString("Sin datos para el período.", fuente, texto, 12, 75); return; }
            int diametro = Math.Max(70, Math.Min(area.Height - 56, area.Width * 45 / 100)); Rectangle donut = new(16, 40, diametro, diametro); decimal total = top.Sum(x => x.Valor); float inicio = -90;
            for (int i = 0; i < top.Count; i++) { float angulo = (float)(top[i].Valor / total * 360); using Pen segmento = new(PaletaDatos[i % PaletaDatos.Length], Math.Max(18, diametro / 3)); graphics.DrawArc(segmento, donut, inicio, angulo); inicio += angulo; }
            DibujarLeyenda(graphics, top, new Rectangle(donut.Right + 16, 42, Math.Max(75, area.Right - donut.Right - 22), area.Height - 50), total);
        }

        private static void DibujarProductosHorizontales(Graphics graphics, Rectangle area, string titulo, List<(string Etiqueta, decimal Valor)> datos)
        {
            using Font tituloFuente = new("Segoe UI", 9F, FontStyle.Bold); using Font fuente = new("Segoe UI", 8F); using Brush texto = new SolidBrush(Color.FromArgb(55, 59, 64)); using Brush barra = new SolidBrush(AzulDatos);
            graphics.DrawString(titulo, tituloFuente, texto, 12, 10); List<(string Etiqueta, decimal Valor)> top = datos.OrderByDescending(x => x.Valor).Take(5).ToList(); if (!top.Any()) { graphics.DrawString("Sin datos para el período.", fuente, texto, 12, 75); return; }
            decimal maximo = top.Max(x => x.Valor); int y = 44; int etiqueta = Math.Min(230, area.Width / 3); foreach (var dato in top) { string nombre = dato.Etiqueta.Length > 28 ? dato.Etiqueta[..28] + "…" : dato.Etiqueta; graphics.DrawString(nombre, fuente, texto, 12, y); int x = etiqueta + 18; int ancho = (int)((area.Width - x - 48) * dato.Valor / maximo); graphics.FillRectangle(barra, x, y + 2, Math.Max(2, ancho), 13); graphics.DrawString(dato.Valor.ToString("0"), fuente, texto, x + ancho + 6, y); y += 38; }
        }

        private static void DibujarLeyenda(Graphics graphics, List<(string Etiqueta, decimal Valor)> datos, Rectangle area, decimal total)
        {
            using Font fuente = new("Segoe UI", 7.5F); using Brush texto = new SolidBrush(Color.FromArgb(55, 59, 64)); for (int i = 0; i < datos.Count && area.Top + i * 22 + 18 <= area.Bottom; i++) { int y = area.Top + i * 22; using Brush color = new SolidBrush(PaletaDatos[i % PaletaDatos.Length]); graphics.FillEllipse(color, area.Left, y + 3, 10, 10); string nombre = datos[i].Etiqueta.Length > 18 ? datos[i].Etiqueta[..18] + "…" : datos[i].Etiqueta; graphics.DrawString($"{nombre}  {datos[i].Valor / total:P0}", fuente, texto, area.Left + 15, y); }
        }

        private static void DibujarGuias(Graphics graphics, Rectangle plot) { using Pen guia = new(Color.FromArgb(226, 229, 232)); for (int i = 0; i < 4; i++) { int y = plot.Top + i * plot.Height / 3; graphics.DrawLine(guia, plot.Left, y, plot.Right, y); } }
        private static void DibujarEjeY(Graphics graphics, Rectangle plot, decimal maximo, bool esMoneda, Font fuente, Brush texto)
        {
            for (int marca = 0; marca <= 4; marca++)
            {
                decimal valor = maximo * (4 - marca) / 4m;
                string etiqueta = esMoneda ? FormatearNumeroCompacto(valor) : Math.Round(valor).ToString(CultureInfo.CurrentCulture);
                SizeF medida = graphics.MeasureString(etiqueta, fuente);
                int y = plot.Top + plot.Height * marca / 4;
                graphics.DrawString(etiqueta, fuente, texto, Math.Max(0, plot.Left - medida.Width - 6), y - medida.Height / 2);
            }
        }
        private static string FormatearNumeroCompacto(decimal valor) => Math.Abs(valor) >= 1000000m ? "$" + (valor / 1000000m).ToString("0.##", CultureInfo.CurrentCulture) + "M" : Math.Abs(valor) >= 1000m ? "$" + (valor / 1000m).ToString("0.#", CultureInfo.CurrentCulture) + "k" : valor.ToString("C0", CultureInfo.CurrentCulture);
        // Revalida permiso y sucursal operativa antes de iniciar una venta desde Inicio.
        private void BtnNuevaVenta_Click(object? sender, EventArgs e)
        {
            if (!SesionActual.TienePermiso("VENTAS_REALIZAR")) return;
            if (!SesionActual.IdSucursalOperativa.HasValue) { MostrarMensaje("Sucursal operativa requerida", "Para realizar una venta primero seleccioná una sucursal operativa."); return; }
            formPrincipal.AbrirFormularioEnPanel(new FormVentas(formPrincipal), formPrincipal.BotonVentas);
        }

        // Navega al módulo de productos desde el acceso rápido autorizado.
        private void BtnProductos_Click(object? sender, EventArgs e)
        {
            if (!SesionActual.TienePermiso("PRODUCTOS_VER")) return;
            formPrincipal.AbrirFormularioEnPanel(new FormProductos(formPrincipal), formPrincipal.BotonProductos);
        }

        // Navega al módulo de clientes desde el acceso rápido autorizado.
        private void BtnClientes_Click(object? sender, EventArgs e)
        {
            if (!SesionActual.TienePermiso("CLIENTES_VER")) return;
            formPrincipal.AbrirFormularioEnPanel(new FormClientes(formPrincipal), formPrincipal.BotonClientes);
        }

        // Publica el aviso para el alcance seleccionado tras validar la autorización en Lógica y SQL.
        private void BtnPublicarAviso_Click(object? sender, EventArgs e)
        {
            try
            {
                int? idSucursal = cmbAvisoSucursal.SelectedItem is SucursalAvisoModelo opcion
                    ? opcion.IdSucursal
                    : null;
                dashboardLogica.PublicarAviso(idSucursal, txtAvisoTitulo.Text, txtAvisoMensaje.Text);
                CargarAvisos();
                CambiarModoAvisos(false);
            }
            catch (Exception ex) { MostrarMensaje("No se pudo publicar el aviso", ex.Message); }
        }

        // Recarga avisos y alcance de publicación cuando FormPrincipal cambia la sucursal operativa.
        public void ActualizarPorSucursalOperativa()
        {
            CargarAvisos();
            ConfigurarAlcanceAviso();
        }

        private void FormInicio_Load(object? sender, EventArgs e)
        {
            AjustarLayoutResponsive();
            vistaCargada = true;
            InvalidarGraficosVisibles();
        }

        // Recalcula únicamente la geometría y el dibujo al cambiar el tamaño; no vuelve a consultar datos.
        private void FormInicio_Resize(object? sender, EventArgs e)
        {
            if (!vistaCargada) return;
            AjustarLayoutResponsive();
            InvalidarGraficosVisibles();
        }

        private void InvalidarGraficosVisibles()
        {
            if (pnlGraficoVentas.Visible) pnlGraficoVentas.Invalidate();
            if (pnlGraficoProductos.Visible) pnlGraficoProductos.Invalidate();
        }

        // Usa FormPrincipal como owner para mantener la ubicación coherente de los mensajes.
        private void MostrarMensaje(string titulo, string mensaje)
        {
            using FormMensaje formMensaje = new(titulo, mensaje);
            formMensaje.ShowDialog(formPrincipal);
        }
    }
}
