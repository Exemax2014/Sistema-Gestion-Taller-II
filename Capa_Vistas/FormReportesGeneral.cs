using System.Globalization;
using Capa_Logica;

namespace Capa_Vistas
{
    // Organiza las consultas autorizadas de Reportes en secciones internas sin crear módulos paralelos.
    public partial class FormReportesGeneral : Form
    {
        private enum SeccionReporte { General, PorUsuario, VentasDetalladas, StockBajo }
        private readonly FormPrincipal formPrincipal;
        private readonly ReporteLogica reporteLogica = new();
        private readonly SucursalLogica sucursalLogica = new();
        private readonly TabControl tabContenido = new();
        private readonly DataGridView dgvUsuarioVentas = CrearGrilla();
        private readonly DataGridView dgvActividadUsuario = CrearGrilla();
        private readonly DataGridView dgvDetalleVentas = CrearGrilla();
        private readonly DataGridView dgvStockBajo = CrearGrilla();
        private readonly Button btnExportar = new();
        private Panel? pnlActividadUsuario;
        private SplitContainer? divisorUsuario;
        private readonly Dictionary<SeccionReporte, TabPage> paginas = new();
        private readonly Dictionary<SeccionReporte, Label> estadosVacios = new();
        private List<ReporteDiaModelo> dias = new();
        private SeccionReporte seccionActual = SeccionReporte.General;

        // Mantiene la navegación embebida y los mensajes centrados en el formulario principal.
        public FormReportesGeneral(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            AutoScroll = true;
            ConfigurarGrillas();
            ConfigurarContenidoInterno();
            ConfigurarEventos();
            PrepararVistaInicial();
        }

        // Define columnas, formatos y acciones de las grillas sin mezclar reglas de consulta.
        private void ConfigurarGrillas()
        {
            ConfigurarColumnas(dgvActividadUsuario, new[] { "Fecha", "Acción", "Entidad", "Detalle", "Sucursal" });
            ConfigurarColumnas(dgvProductosVendidos, new[] { "Producto", "Categoría", "Marca", "Unidades", "Importe" });
            ConfigurarColumnas(dgvUsuarioVentas, new[] { "N°", "Fecha", "Cliente", "Sucursal", "Subtotal", "Descuento", "Total", "Ver detalle" });
            ConfigurarColumnas(dgvDetalleVentas, new[] { "N°", "Fecha", "Cliente", "Vendedor", "Sucursal", "Subtotal", "Descuento", "Total", "Pagos", "Ver detalle" });
            ConfigurarColumnas(dgvStockBajo, new[] { "Código", "Producto", "Categoría", "Sucursal", "Stock", "Stock mínimo", "Diferencia" });
            dgvStockBajo.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdProducto", Visible = false });
            dgvStockBajo.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdSucursal", Visible = false });
        }

        // Crea las páginas reutilizables para las secciones que ocupan el área completa de resultados.
        private void ConfigurarContenidoInterno()
        {
            tabContenido.Appearance = TabAppearance.FlatButtons;
            tabContenido.ItemSize = new Size(0, 1);
            tabContenido.SizeMode = TabSizeMode.Fixed;
            tabContenido.Font = new Font("Segoe UI", 9F);
            tabContenido.Visible = false;
            CrearPagina(SeccionReporte.PorUsuario, "Historial resumido", dgvUsuarioVentas);
            ConfigurarPaginaPorUsuario();
            CrearPagina(SeccionReporte.VentasDetalladas, "Ventas detalladas", dgvDetalleVentas);
            CrearPagina(SeccionReporte.StockBajo, "Stock bajo", dgvStockBajo);
            Controls.Add(tabContenido);
            btnExportar.Text = "Exportar CSV";
            btnExportar.BackColor = Color.FromArgb(190, 137, 45);
            btnExportar.ForeColor = Color.White;
            btnExportar.FlatStyle = FlatStyle.Flat;
            btnExportar.FlatAppearance.BorderSize = 0;
            pnlFiltros.Controls.Add(btnExportar);
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
            TabPage pagina = paginas[SeccionReporte.PorUsuario];
            Label estadoVentas = estadosVacios[SeccionReporte.PorUsuario];
            pagina.Controls.Clear();

            divisorUsuario = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterWidth = 6,
                Panel1MinSize = 40,
                Panel2MinSize = 40
            };
            divisorUsuario.SizeChanged += (_, _) => AjustarDivisorUsuario();

            divisorUsuario.Panel1.Controls.Add(CrearPanelSeccion("Historial resumido de ventas", dgvUsuarioVentas, estadoVentas));
            pnlActividadUsuario = CrearPanelSeccion("Actividad del sistema", dgvActividadUsuario, new Label());
            pnlActividadUsuario.Visible = false;
            divisorUsuario.Panel2.Controls.Add(pnlActividadUsuario);
            divisorUsuario.Panel2Collapsed = true;
            pagina.Controls.Add(divisorUsuario);
        }

        // Mantiene la proporción inicial del historial sin asignar una distancia inválida durante la creación.
        private void AjustarDivisorUsuario()
        {
            if (divisorUsuario is null || divisorUsuario.Height <= 0) return;

            int espacioDisponible = divisorUsuario.ClientSize.Height - divisorUsuario.SplitterWidth;
            int minimo = divisorUsuario.Panel1MinSize;
            int maximo = espacioDisponible - divisorUsuario.Panel2MinSize;
            if (maximo < minimo) return;

            int distancia = Math.Clamp(220, minimo, maximo);
            if (divisorUsuario.SplitterDistance != distancia)
                divisorUsuario.SplitterDistance = distancia;
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
            Load += (_, _) => AjustarLayout();
            Resize += (_, _) => AjustarLayout();
        }

        // Inicializa los filtros según las capacidades ya resueltas por ReporteLogica.
        private void PrepararVistaInicial()
        {
            dtpDesde.Value = DateTime.Today.AddDays(-30);
            dtpHasta.Value = DateTime.Today;
            if (!reporteLogica.PuedeVerReportes()) { MostrarMensaje("Sin permiso", "No tiene permiso para acceder a Reportes."); return; }
            if (reporteLogica.ObtenerAlcanceReportes() == AlcanceReportes.Ninguno) { MostrarMensaje("Sin alcance", reporteLogica.ObtenerMensajeSinAlcance()); return; }
            CargarSucursales();
            ConfigurarNavegacion();
            CambiarSeccion(PrimeraSeccionDisponible());
        }

        // Muestra solo pestañas que la sesión puede consultar mediante permisos REPORTES_*.
        private void ConfigurarNavegacion()
        {
            btnGeneral.Visible = reporteLogica.PuedeVerVentas() || reporteLogica.PuedeVerRecaudacion() || reporteLogica.PuedeVerProductos() || reporteLogica.PuedeVerStock();
            btnPorUsuario.Visible = reporteLogica.PuedeVerRendimientoVendedores();
            btnVentasDetalladas.Visible = reporteLogica.PuedeVerDetalleVentas();
            btnStockBajo.Visible = reporteLogica.PuedeVerStock();
        }

        // Elige una sección autorizada si la predeterminada no está disponible para la sesión.
        private SeccionReporte PrimeraSeccionDisponible()
        {
            if (btnGeneral.Visible) return SeccionReporte.General;
            if (btnPorUsuario.Visible) return SeccionReporte.PorUsuario;
            if (btnVentasDetalladas.Visible) return SeccionReporte.VentasDetalladas;
            return SeccionReporte.StockBajo;
        }

        // Carga sucursales activas solo para el alcance global y evita IDs fijos en el selector.
        private void CargarSucursales()
        {
            bool global = reporteLogica.ObtenerAlcanceReportes() == AlcanceReportes.Global;
            lblSucursal.Visible = cmbSucursal.Visible = global;
            if (!global) return;
            List<OpcionSucursal> opciones = sucursalLogica.ObtenerSucursalesDisponibles().Select(s => new OpcionSucursal { IdSucursal = s.IdSucursal, Nombre = s.Nombre }).ToList();
            opciones.Insert(0, new OpcionSucursal { IdSucursal = null, Nombre = "Todas las sucursales" });
            cmbSucursal.DataSource = opciones;
            cmbSucursal.DisplayMember = nameof(OpcionSucursal.Nombre);
            cmbSucursal.ValueMember = nameof(OpcionSucursal.IdSucursal);
        }

        // Alterna el contenido sin abandonar FormReportesGeneral ni duplicar entradas en FormPrincipal.
        private void CambiarSeccion(SeccionReporte seccion)
        {
            seccionActual = seccion;
            ConfigurarVistaSeccion();
            CargarSeccionActual();
        }

        // Ajusta filtros y elementos visibles para que cada vista muestre únicamente sus datos pertinentes.
        private void ConfigurarVistaSeccion()
        {
            bool esGeneral = seccionActual == SeccionReporte.General;
            bool esUsuario = seccionActual == SeccionReporte.PorUsuario;
            bool esDetalle = seccionActual == SeccionReporte.VentasDetalladas;
            bool esStock = seccionActual == SeccionReporte.StockBajo;
            bool puedeElegirUsuario = (esUsuario || esDetalle) && reporteLogica.ObtenerAlcanceReportes() != AlcanceReportes.Propio;
            lblDesde.Visible = dtpDesde.Visible = !esStock;
            lblHasta.Visible = dtpHasta.Visible = !esStock;
            lblVendedor.Visible = cmbVendedor.Visible = puedeElegirUsuario;
            lblVendedor.Text = esUsuario ? "Usuario" : "Vendedor";
            pnlTarjetaVentas.Visible = (esGeneral || esUsuario) && reporteLogica.PuedeVerVentas();
            pnlTarjetaIngresos.Visible = (esGeneral || esUsuario) && reporteLogica.PuedeVerRecaudacion();
            pnlTarjetaProductos.Visible = (esGeneral || esUsuario) && reporteLogica.PuedeVerProductos();
            pnlTarjetaStock.Visible = esGeneral && reporteLogica.PuedeVerStock();
            pnlGrafico.Visible = (esGeneral || esUsuario) && (reporteLogica.PuedeVerVentas() || reporteLogica.PuedeVerRecaudacion());
            pnlProductosVendidos.Visible = (esGeneral || esUsuario) && reporteLogica.PuedeVerProductos();
            tabContenido.Visible = esUsuario || esDetalle || esStock;
            btnExportar.Visible = reporteLogica.PuedeExportar();
            lblTitulo.Text = esGeneral ? "Reportes" : esUsuario ? "Reportes por usuario" : esDetalle ? "Ventas detalladas" : "Stock bajo";
            lblSubtitulo.Text = esGeneral ? "Resumen comercial del alcance autorizado." : esUsuario ? "Actividad comercial registrada de un usuario." : esDetalle ? "Consulta completa de ventas registradas." : "Productos que requieren reposición.";
            lblFiltrosTitulo.Text = esStock ? "Filtros de stock" : "Filtros del período";
            lblProductosVendidosTitulo.Text = esUsuario ? "Productos vendidos por usuario" : "Productos más vendidos";
            if (puedeElegirUsuario) CargarVendedores();
            ActualizarEstiloNavegacion();
        }

        // Destaca la sección activa y conserva la estética oscura y dorada del sistema.
        private void ActualizarEstiloNavegacion()
        {
            AplicarEstiloPestana(btnGeneral, seccionActual == SeccionReporte.General);
            AplicarEstiloPestana(btnPorUsuario, seccionActual == SeccionReporte.PorUsuario);
            AplicarEstiloPestana(btnVentasDetalladas, seccionActual == SeccionReporte.VentasDetalladas);
            AplicarEstiloPestana(btnStockBajo, seccionActual == SeccionReporte.StockBajo);
        }

        // Aplica el color activo sin cambiar los permisos que determinan si la pestaña existe.
        private static void AplicarEstiloPestana(Button boton, bool activa) { boton.BackColor = activa ? Color.FromArgb(190, 137, 45) : Color.FromArgb(45, 49, 54); boton.ForeColor = Color.White; }

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
        private void CargarSeccionActual()
        {
            if (dtpDesde.Value.Date > dtpHasta.Value.Date) { MostrarMensaje("Fechas inválidas", "La fecha Desde no puede ser posterior a la fecha Hasta."); return; }
            try
            {
                int? sucursal = ObtenerSucursalSeleccionada(), usuario = ObtenerUsuarioSeleccionado();
                if (seccionActual == SeccionReporte.General) CargarGeneral(sucursal);
                else if (seccionActual == SeccionReporte.PorUsuario) CargarPorUsuario(sucursal, usuario);
                else if (seccionActual == SeccionReporte.VentasDetalladas) CargarVentasDetalladas(sucursal, usuario);
                else CargarStockBajo(sucursal);
                AjustarLayout();
            }
            catch (Exception ex) { MostrarMensaje("No se pudieron cargar los reportes", ex.Message); }
        }

        // Carga tarjetas, gráfico y ranking comercial sin duplicar detalle ni stock bajo completo.
        private void CargarGeneral(int? sucursal)
        {
            CargarResumenYGrafico(sucursal, null);
            if (reporteLogica.PuedeVerProductos()) CargarProductos(sucursal, null);
            if (reporteLogica.PuedeVerStock()) lblStockBajoValor.Text = reporteLogica.ObtenerStock(sucursal).Count.ToString(CultureInfo.CurrentCulture);
            tabContenido.TabPages.Clear();
        }

        // Reutiliza las consultas por usuario para mostrar solo ventas realmente registradas y su historial resumido.
        private void CargarPorUsuario(int? sucursal, int? usuario)
        {
            if (reporteLogica.ObtenerAlcanceReportes() != AlcanceReportes.Propio && !usuario.HasValue)
            {
                dgvUsuarioVentas.Rows.Clear();
                dgvActividadUsuario.Rows.Clear();
                if (pnlActividadUsuario != null) pnlActividadUsuario.Visible = false;
                if (divisorUsuario != null) divisorUsuario.Panel2Collapsed = true;
                dgvProductosVendidos.Rows.Clear();
                dias = new List<ReporteDiaModelo>();
                lblVentasValor.Text = "0";
                lblIngresosValor.Text = 0m.ToString("C2", CultureInfo.CurrentCulture);
                lblProductosValor.Text = "0";
                lblProductosVendidosDescripcion.Text = "Seleccioná un usuario para consultar sus productos vendidos.";
                lblGraficoPlaceholder.Visible = true;
                lblGraficoPlaceholder.Text = "Seleccioná un usuario para consultar su evolución.";
                pnlGraficoContenido.Invalidate();
                MostrarPagina(SeccionReporte.PorUsuario, 0, "Seleccioná un usuario para consultar su actividad comercial.");
                return;
            }
            CargarResumenYGrafico(sucursal, usuario);
            if (reporteLogica.PuedeVerProductos()) CargarProductos(sucursal, usuario);
            dgvUsuarioVentas.Rows.Clear();
            if (reporteLogica.PuedeVerDetalleVentas())
            {
                foreach (var venta in reporteLogica.ObtenerDetalleVentas(dtpDesde.Value, dtpHasta.Value, sucursal, usuario)) dgvUsuarioVentas.Rows.Add(venta.IdVenta, venta.Fecha.ToString("dd/MM/yyyy HH:mm"), venta.Cliente, venta.Sucursal, venta.Subtotal, venta.Descuento, venta.Total, "Ver detalle");
                MostrarPagina(SeccionReporte.PorUsuario, dgvUsuarioVentas.Rows.Count, "El usuario seleccionado no registró ventas en el período.");
            }
            else MostrarPagina(SeccionReporte.PorUsuario, 0, "No tiene permiso para consultar el historial detallado de ventas.");

            CargarActividadUsuario(sucursal, usuario);
        }

        // Muestra acciones administrativas reales separadas del historial comercial cuando existen para el usuario consultado.
        private void CargarActividadUsuario(int? sucursal, int? usuario)
        {
            List<AuditoriaReporteModelo> actividad = reporteLogica.ObtenerActividadUsuario(
                dtpDesde.Value,
                dtpHasta.Value,
                sucursal,
                usuario);

            dgvActividadUsuario.Rows.Clear();
            foreach (AuditoriaReporteModelo evento in actividad)
            {
                dgvActividadUsuario.Rows.Add(
                    evento.Fecha.ToString("dd/MM/yyyy HH:mm"),
                    evento.Accion,
                    evento.Entidad,
                    evento.Detalle,
                    evento.Sucursal);
            }

            if (pnlActividadUsuario != null)
            {
                pnlActividadUsuario.Visible = actividad.Count > 0;
            }
            if (divisorUsuario != null)
            {
                divisorUsuario.Panel2Collapsed = actividad.Count == 0;
            }
        }

        // Carga la consulta extensa en su propia sección para no duplicarla dentro del resumen general.
        private void CargarVentasDetalladas(int? sucursal, int? usuario)
        {
            dgvDetalleVentas.Rows.Clear();
            foreach (var venta in reporteLogica.ObtenerDetalleVentas(dtpDesde.Value, dtpHasta.Value, sucursal, usuario)) dgvDetalleVentas.Rows.Add(venta.IdVenta, venta.Fecha.ToString("dd/MM/yyyy HH:mm"), venta.Cliente, venta.Vendedor, venta.Sucursal, venta.Subtotal, venta.Descuento, venta.Total, venta.MetodosPago, "Ver detalle");
            MostrarPagina(SeccionReporte.VentasDetalladas, dgvDetalleVentas.Rows.Count, "No se registraron ventas en el período seleccionado.");
        }

        // Presenta alertas de inventario y deriva la modificación al flujo autorizado de Productos.
        private void CargarStockBajo(int? sucursal)
        {
            dgvStockBajo.Rows.Clear();
            foreach (var stock in reporteLogica.ObtenerStock(sucursal)) dgvStockBajo.Rows.Add(stock.Codigo, stock.Producto, stock.Categoria, stock.Sucursal, stock.Stock, stock.StockMinimo, stock.Stock - stock.StockMinimo, stock.IdProducto, stock.IdSucursal);
            bool puedeGestionar = dgvStockBajo.Rows.Cast<DataGridViewRow>().Any(fila => reporteLogica.PuedeGestionarStockSucursal(Convert.ToInt32(fila.Cells[8].Value)));
            if (puedeGestionar && !dgvStockBajo.Columns.Contains("Gestionar")) dgvStockBajo.Columns.Add(new DataGridViewButtonColumn { Name = "Gestionar", HeaderText = "Acción", Text = "Gestionar stock", UseColumnTextForButtonValue = true });
            if (!puedeGestionar && dgvStockBajo.Columns.Contains("Gestionar")) dgvStockBajo.Columns.Remove("Gestionar");
            MostrarPagina(SeccionReporte.StockBajo, dgvStockBajo.Rows.Count, "No hay productos con stock bajo para el alcance seleccionado.");
        }

        // Actualiza los indicadores y la evolución diaria con el mismo alcance aplicado a la sección actual.
        private void CargarResumenYGrafico(int? sucursal, int? usuario)
        {
            bool ventas = reporteLogica.PuedeVerVentas(), recaudacion = reporteLogica.PuedeVerRecaudacion();
            if (ventas || recaudacion)
            {
                var resumen = reporteLogica.ObtenerResumen(dtpDesde.Value, dtpHasta.Value, sucursal, usuario);
                lblVentasValor.Text = ventas ? resumen.Ventas.ToString(CultureInfo.CurrentCulture) : "—";
                lblIngresosValor.Text = recaudacion ? resumen.Recaudacion.ToString("C2", CultureInfo.CurrentCulture) : "—";
            }
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
            foreach (var producto in productos) dgvProductosVendidos.Rows.Add(producto.Producto, producto.Categoria, producto.Marca, producto.Unidades, producto.Importe);
            lblProductosValor.Text = productos.Sum(producto => producto.Unidades).ToString(CultureInfo.CurrentCulture);
            lblProductosVendidosDescripcion.Text = productos.Count == 0 ? "No se registraron productos vendidos en el período seleccionado." : "Ranking correspondiente al período seleccionado.";
        }

        // Activa una sola página y muestra un estado claro cuando la consulta autorizada no devuelve filas.
        private void MostrarPagina(SeccionReporte seccion, int cantidad, string mensajeVacio)
        {
            tabContenido.TabPages.Clear();
            tabContenido.TabPages.Add(paginas[seccion]);
            Label estado = estadosVacios[seccion];
            estado.Text = mensajeVacio;
            estado.Visible = cantidad == 0;
        }

        // Abre el detalle reutilizable, cuya lógica vuelve a validar el alcance de Reportes.
        private void DgvVentas_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (sender is not DataGridView grilla || e.RowIndex < 0 || e.ColumnIndex < 0 || grilla.Columns[e.ColumnIndex].Name != "Ver detalle") return;
            formPrincipal.AbrirFormularioEnPanel(new FormVentaDetalle(formPrincipal, Convert.ToInt32(grilla.Rows[e.RowIndex].Cells[0].Value)), formPrincipal.BotonReportes);
        }

        // Envía el producto al detalle existente solo cuando Lógica autoriza gestionar esa sucursal.
        private void DgvStockBajo_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvStockBajo.Columns[e.ColumnIndex].Name != "Gestionar") return;
            int idProducto = Convert.ToInt32(dgvStockBajo.Rows[e.RowIndex].Cells[7].Value), idSucursal = Convert.ToInt32(dgvStockBajo.Rows[e.RowIndex].Cells[8].Value);
            if (reporteLogica.PuedeGestionarStockSucursal(idSucursal)) formPrincipal.AbrirFormularioEnPanel(new FormProductoDetalle(formPrincipal, idProducto), formPrincipal.BotonProductos);
        }

        // Dibuja ventas y recaudación reales con series independientes según permisos concedidos.
        private void PnlGraficoContenido_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(248, 249, 250));
            if (dias.Sum(dia => dia.Ventas) == 0) return;
            Rectangle area = new(30, 18, Math.Max(1, pnlGraficoContenido.ClientSize.Width - 50), Math.Max(1, pnlGraficoContenido.ClientSize.Height - 45));
            decimal maxVentas = Math.Max(1, dias.Max(dia => (decimal)dia.Ventas)), maxIngresos = Math.Max(1, dias.Max(dia => dia.Recaudacion));
            int grupo = Math.Max(10, area.Width / Math.Max(1, dias.Count)), barra = Math.Max(3, (grupo - 5) / (reporteLogica.PuedeVerRecaudacion() ? 2 : 1));
            using Font fuente = new("Segoe UI", 7F); using Brush texto = new SolidBrush(Color.FromArgb(105, 110, 116)); using Brush ventas = new SolidBrush(Color.FromArgb(82, 88, 94)); using Brush ingresos = new SolidBrush(Color.FromArgb(190, 137, 45));
            for (int i = 0; i < dias.Count; i++)
            {
                ReporteDiaModelo dia = dias[i]; int x = area.Left + i * grupo;
                if (reporteLogica.PuedeVerVentas()) { int alto = (int)(area.Height * (dia.Ventas / maxVentas)); e.Graphics.FillRectangle(ventas, x, area.Bottom - alto, barra, alto); }
                if (reporteLogica.PuedeVerRecaudacion()) { int alto = (int)(area.Height * (dia.Recaudacion / maxIngresos)); e.Graphics.FillRectangle(ingresos, x + barra + 2, area.Bottom - alto, barra, alto); }
                if (dias.Count <= 14 || i % 2 == 0) e.Graphics.DrawString(dia.Fecha.ToString("dd/MM"), fuente, texto, x, area.Bottom + 4);
            }
        }

        // Exporta la grilla correspondiente a la sección activa con el permiso ya validado por Lógica.
        private void BtnExportar_Click(object? sender, EventArgs e)
        {
            DataGridView grilla = seccionActual == SeccionReporte.General ? dgvProductosVendidos : seccionActual == SeccionReporte.PorUsuario ? dgvUsuarioVentas : seccionActual == SeccionReporte.VentasDetalladas ? dgvDetalleVentas : dgvStockBajo;
            if (grilla.Rows.Count == 0) { MostrarMensaje("Sin datos", "No hay datos para exportar."); return; }
            using SaveFileDialog dialogo = new() { Filter = "CSV (*.csv)|*.csv", FileName = "reporte.csv" };
            if (dialogo.ShowDialog(formPrincipal) != DialogResult.OK) return;
            using StreamWriter escritor = new(dialogo.FileName, false, System.Text.Encoding.UTF8);
            escritor.WriteLine(string.Join(";", grilla.Columns.Cast<DataGridViewColumn>().Where(c => c is not DataGridViewButtonColumn).Select(c => c.HeaderText)));
            foreach (DataGridViewRow fila in grilla.Rows) escritor.WriteLine(string.Join(";", fila.Cells.Cast<DataGridViewCell>().Where(c => grilla.Columns[c.ColumnIndex] is not DataGridViewButtonColumn).Select(c => '"' + (c.Value?.ToString() ?? string.Empty).Replace("\"", "\"\"") + '"')));
        }

        // Restablece los filtros disponibles de la sección sin alterar el alcance resuelto en sesión.
        private void BtnLimpiarFiltros_Click(object? sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Today.AddDays(-30); dtpHasta.Value = DateTime.Today;
            if (cmbSucursal.Visible) cmbSucursal.SelectedIndex = 0;
            if (cmbVendedor.Visible) cmbVendedor.SelectedIndex = 0;
            CargarSeccionActual();
        }

        // Distribuye cabecera, pestañas, filtros y resultados para evitar desbordes dentro de pnlContenido.
        private void AjustarLayout()
        {
            int margen = 32, ancho = Math.Max(340, ClientSize.Width - margen * 2), y = 18;
            pnlCabecera.SetBounds(margen, y, ancho, 82); y += 92;
            pnlNavegacion.SetBounds(margen, y, ancho, 46); AjustarBotonesNavegacion(); y += 58;
            AjustarFiltros(ancho, margen, ref y);
            List<Panel> tarjetas = new[] { pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaProductos, pnlTarjetaStock }.Where(t => t.Visible).ToList();
            if (tarjetas.Count > 0)
            {
                int columnas = ancho >= 1040 ? 4 : ancho >= 650 ? 2 : 1, separacion = 16, anchoTarjeta = (ancho - separacion * (columnas - 1)) / columnas;
                for (int i = 0; i < tarjetas.Count; i++) tarjetas[i].SetBounds(margen + (i % columnas) * (anchoTarjeta + separacion), y + (i / columnas) * 126, anchoTarjeta, 110);
                y += ((tarjetas.Count + columnas - 1) / columnas) * 126 + 10;
            }
            if (pnlGrafico.Visible) { pnlGrafico.SetBounds(margen, y, ancho, 230); y += 246; }
            if (pnlProductosVendidos.Visible) { pnlProductosVendidos.SetBounds(margen, y, ancho, 230); y += 246; }
            if (tabContenido.Visible) { tabContenido.SetBounds(margen, y, ancho, Math.Max(260, ClientSize.Height - y - 32)); y += tabContenido.Height + 16; }
            AutoScrollMinSize = new Size(0, y + 24);
        }

        // Distribuye pestañas visibles en todo el ancho sin dejar espacios de una sección sin permiso.
        private void AjustarBotonesNavegacion()
        {
            List<Button> botones = new[] { btnGeneral, btnPorUsuario, btnVentasDetalladas, btnStockBajo }.Where(b => b.Visible).ToList();
            int ancho = Math.Max(120, (pnlNavegacion.ClientSize.Width - 16 - Math.Max(0, botones.Count - 1) * 6) / Math.Max(1, botones.Count));
            for (int i = 0; i < botones.Count; i++) botones[i].SetBounds(8 + i * (ancho + 6), 5, ancho, 36);
        }

        // Reacomoda filtros y acciones según los controles autorizados de la sección activa.
        private void AjustarFiltros(int ancho, int margen, ref int y)
        {
            List<(Control Etiqueta, Control Campo)> campos = new();
            if (lblDesde.Visible) campos.Add((lblDesde, dtpDesde));
            if (lblHasta.Visible) campos.Add((lblHasta, dtpHasta));
            if (lblSucursal.Visible) campos.Add((lblSucursal, cmbSucursal));
            if (lblVendedor.Visible) campos.Add((lblVendedor, cmbVendedor));
            int columnas = ancho >= 980 ? 4 : ancho >= 680 ? 2 : 1, anchoCampo = Math.Min(230, (ancho - 36 - (columnas - 1) * 16) / columnas), filas = (campos.Count + columnas - 1) / columnas, alto = Math.Max(105, 40 + filas * 58 + 14);
            pnlFiltros.SetBounds(margen, y, ancho, alto);
            for (int i = 0; i < campos.Count; i++) { int col = i % columnas, fila = i / columnas, x = 18 + col * (anchoCampo + 16), top = 40 + fila * 58; campos[i].Etiqueta.SetBounds(x, top, anchoCampo, 20); campos[i].Campo.SetBounds(x, top + 22, anchoCampo, 28); }
            int accionesX = Math.Max(18, ancho - (reporteLogica.PuedeExportar() ? 420 : 270));
            btnAplicarFiltros.SetBounds(accionesX, alto - 52, 140, 36); btnLimpiarFiltros.SetBounds(accionesX + 150, alto - 52, 120, 36); btnExportar.SetBounds(accionesX + 280, alto - 52, 140, 36);
            y += alto + 14;
        }

        // Construye una grilla de solo lectura con encabezados y números alineados de forma consistente.
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
                if (nombre is "Subtotal" or "Descuento" or "Total" or "Importe") { grilla.Columns[indice].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; grilla.Columns[indice].DefaultCellStyle.Format = "C2"; }
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
