using System.Globalization;
using Capa_Logica;

namespace Capa_Vistas
{
    // Muestra el resumen operativo y las comunicaciones internas del usuario actual.
    public partial class FormInicio : Form
    {
        private readonly FormPrincipal formPrincipal;
        private readonly DashboardLogica dashboardLogica = new DashboardLogica();
        private List<DashboardSerieModelo> serieSemanal = new();
        private List<DashboardProductoModelo> productosMasVendidos = new();

        // Inicializa la vista con el principal como dueño de la navegación y de los mensajes.
        public FormInicio(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            ConfigurarEventos();
            ConfigurarGrillas();
            PrepararVistaInicial();
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
            pnlGraficoVentas.Paint += PnlGraficoVentas_Paint;
            pnlGraficoIngresos.Paint += PnlGraficoIngresos_Paint;
            pnlGraficoProductos.Paint += PnlGraficoProductos_Paint;
        }

        // Define grillas de sólo lectura para que Inicio no edite datos persistentes.
        private void ConfigurarGrillas()
        {
            ConfigurarGrilla(dgvActividad, new[] { "Fecha", "Cliente", "Vendedor", "Total" });
            ConfigurarGrilla(dgvAvisos, new[] { "Fecha", "Título", "Mensaje", "Autor", "Destino" });
            lblActividadPlaceholder.Visible = false;
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
            btnNuevaVenta.Enabled = SesionActual.TienePermiso("VENTAS_REALIZAR");
            btnProductos.Enabled = SesionActual.TienePermiso("PRODUCTOS_VER");
            btnClientes.Enabled = SesionActual.TienePermiso("CLIENTES_VER");
            CargarDashboard();
        }

        // Consulta la fachada lógica y conserva tarjetas sin datos cuando la sesión no está autorizada.
        private void CargarDashboard()
        {
            try
            {
                DashboardResumenModelo resumen = dashboardLogica.ObtenerResumen();
                bool puedeVentas = dashboardLogica.PuedeVerVentas();
                bool puedeProductos = dashboardLogica.PuedeVerProductos();
                lblVentasHoyValor.Text = puedeVentas ? resumen.VentasHoy.ToString() : "—";
                lblIngresosHoyValor.Text = puedeVentas ? resumen.IngresosHoy.ToString("C2", CultureInfo.CurrentCulture) : "—";
                lblStockBajoValor.Text = puedeProductos ? resumen.StockBajo.ToString() : "—";
                lblProductosValor.Text = puedeProductos ? resumen.ProductosActivos.ToString() : "—";
                serieSemanal = dashboardLogica.ObtenerSerieSemanal();
                productosMasVendidos = dashboardLogica.ObtenerProductosMasVendidos();
                CargarActividad(); CargarAvisos(); CargarDestinosAviso();
                pnlGraficoVentas.Invalidate(); pnlGraficoIngresos.Invalidate(); pnlGraficoProductos.Invalidate();
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

        // Muestra únicamente los avisos que SQL asocia al perfil y sucursal del usuario actual.
        private void CargarAvisos()
        {
            dgvAvisos.Rows.Clear();
            foreach (AvisoModelo aviso in dashboardLogica.ListarAvisos())
                dgvAvisos.Rows.Add(aviso.FechaCreacion.ToString("dd/MM HH:mm"), aviso.Titulo, aviso.Mensaje, aviso.Autor, aviso.Destino);
        }

        // Carga perfiles destino autorizados y permite seleccionar varios para un mismo aviso.
        private void CargarDestinosAviso()
        {
            List<AvisoDestinoModelo> destinos = dashboardLogica.ListarDestinosAviso();
            clbAvisoDestinos.DataSource = destinos;
            clbAvisoDestinos.DisplayMember = nameof(AvisoDestinoModelo.Nombre);
            clbAvisoDestinos.ValueMember = nameof(AvisoDestinoModelo.IdPerfil);
            bool puedePublicar = SesionActual.TienePermiso("AVISOS_PUBLICAR") && destinos.Count > 0;
            txtAvisoTitulo.Visible = puedePublicar;
            txtAvisoMensaje.Visible = puedePublicar;
            clbAvisoDestinos.Visible = puedePublicar;
            btnPublicarAviso.Visible = puedePublicar;
        }

        // Describe el alcance realmente seleccionado, sin inferirlo desde el nombre del perfil.
        private string ObtenerDescripcionSesion()
        {
            string perfil = string.IsNullOrWhiteSpace(SesionActual.Perfil) ? "Usuario" : SesionActual.Perfil;
            string alcance = SesionActual.IdSucursalOperativa.HasValue ? SesionActual.SucursalOperativa : "Todo el negocio";
            return $"{perfil} · {alcance}";
        }

        // Ajusta tarjetas, gráficos y listas con scroll cuando la altura disponible no alcanza.
        private void AjustarLayoutResponsive()
        {
            int margen = 24, ancho = Math.Max(320, pnlPrincipal.ClientSize.Width - margen * 2);
            int columnas = ancho >= 1000 ? 4 : ancho >= 620 ? 2 : 1;
            int separacion = 16, anchoTarjeta = (ancho - separacion * (columnas - 1)) / columnas;
            Panel[] tarjetas = { pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaStock, pnlTarjetaProductos };
            pnlPrincipal.AutoScroll = true;
            pnlEncabezado.SetBounds(margen, 18, ancho, 82);
            for (int i = 0; i < tarjetas.Length; i++) tarjetas[i].SetBounds(margen + (i % columnas) * (anchoTarjeta + separacion), 128 + (i / columnas) * 126, anchoTarjeta, 110);
            int y = 128 + ((tarjetas.Length + columnas - 1) / columnas) * 126;
            pnlAccesos.SetBounds(margen, y, ancho, 125); y += 145;
            int columnasGraficos = ancho >= 920 ? 3 : 1;
            int anchoGrafico = (ancho - separacion * (columnasGraficos - 1)) / columnasGraficos;
            pnlGraficos.SetBounds(margen, y, ancho, columnasGraficos == 3 ? 185 : 555);
            Panel[] graficos = { pnlGraficoVentas, pnlGraficoIngresos, pnlGraficoProductos };
            for (int i = 0; i < graficos.Length; i++) graficos[i].SetBounds((i % columnasGraficos) * (anchoGrafico + separacion), (i / columnasGraficos) * 185, anchoGrafico, 170);
            y += pnlGraficos.Height + 20;
            pnlActividad.SetBounds(margen, y, ancho, 270); y += 290;
            pnlAvisos.SetBounds(margen, y, ancho, 300);
            pnlPrincipal.AutoScrollMinSize = new Size(0, y + 325);
        }

        // Dibuja barras de ventas con la misma escala para el período semanal.
        private void PnlGraficoVentas_Paint(object? sender, PaintEventArgs e) => DibujarBarras(e.Graphics, pnlGraficoVentas.ClientRectangle, "Ventas últimos 7 días", serieSemanal.Select(x => (x.Fecha.ToString("dd"), (decimal)x.Ventas)).ToList(), Color.FromArgb(45, 49, 54));

        // Dibuja importes diarios sin reemplazar los reportes detallados del módulo Reportes.
        private void PnlGraficoIngresos_Paint(object? sender, PaintEventArgs e) => DibujarBarras(e.Graphics, pnlGraficoIngresos.ClientRectangle, "Ingresos últimos 7 días", serieSemanal.Select(x => (x.Fecha.ToString("dd"), x.Ingresos)).ToList(), Color.FromArgb(190, 137, 45));

        // Dibuja el ranking breve de productos vendidos dentro del alcance operativo.
        private void PnlGraficoProductos_Paint(object? sender, PaintEventArgs e) => DibujarBarras(e.Graphics, pnlGraficoProductos.ClientRectangle, "Productos más vendidos", productosMasVendidos.Select(x => (x.Producto, (decimal)x.UnidadesVendidas)).ToList(), Color.FromArgb(74, 120, 88));

        // Renderiza un gráfico liviano sin incorporar dependencias externas de gráficos.
        private static void DibujarBarras(Graphics graphics, Rectangle area, string titulo, List<(string Etiqueta, decimal Valor)> datos, Color color)
        {
            using Font tituloFuente = new("Segoe UI", 9F, FontStyle.Bold);
            using Font etiquetaFuente = new("Segoe UI", 7F);
            using Brush texto = new SolidBrush(Color.FromArgb(55, 59, 64));
            graphics.DrawString(titulo, tituloFuente, texto, 12, 10);
            if (datos.Count == 0) { graphics.DrawString("Sin datos para el período.", etiquetaFuente, texto, 12, 75); return; }
            decimal maximo = Math.Max(1, datos.Max(x => x.Valor)); int baseY = area.Height - 28;
            int ancho = Math.Max(12, (area.Width - 24) / datos.Count - 8);
            for (int i = 0; i < datos.Count; i++)
            {
                int alto = (int)((area.Height - 65) * (datos[i].Valor / maximo)); int x = 14 + i * (ancho + 8);
                using Brush barra = new SolidBrush(color); graphics.FillRectangle(barra, x, baseY - alto, ancho, alto);
                string etiqueta = datos[i].Etiqueta.Length > 10 ? datos[i].Etiqueta[..10] : datos[i].Etiqueta;
                graphics.DrawString(etiqueta, etiquetaFuente, texto, x, baseY + 4);
            }
        }

        // Impide abrir ventas sin sucursal operativa y centra el aviso en el formulario principal.
        private void BtnNuevaVenta_Click(object? sender, EventArgs e)
        {
            if (!SesionActual.IdSucursalOperativa.HasValue) { MostrarMensaje("Sucursal operativa requerida", "Para realizar una venta primero seleccioná una sucursal operativa."); return; }
            formPrincipal.AbrirFormularioEnPanel(new FormVentas(), formPrincipal.BotonVentas);
        }

        // Navega al módulo de productos desde el acceso rápido autorizado.
        private void BtnProductos_Click(object? sender, EventArgs e) => formPrincipal.AbrirFormularioEnPanel(new FormProductos(formPrincipal), formPrincipal.BotonProductos);

        // Navega al módulo de clientes desde el acceso rápido autorizado.
        private void BtnClientes_Click(object? sender, EventArgs e) => formPrincipal.AbrirFormularioEnPanel(new FormClientes(formPrincipal), formPrincipal.BotonClientes);

        // Publica para todos los perfiles seleccionados tras validar la autorización en Lógica y SQL.
        private void BtnPublicarAviso_Click(object? sender, EventArgs e)
        {
            try
            {
                List<int> destinos = clbAvisoDestinos.CheckedItems
                    .OfType<AvisoDestinoModelo>()
                    .Select(destino => destino.IdPerfil)
                    .ToList();
                dashboardLogica.PublicarAviso(destinos, txtAvisoTitulo.Text, txtAvisoMensaje.Text);
                txtAvisoTitulo.Clear(); txtAvisoMensaje.Clear(); CargarAvisos();
            }
            catch (Exception ex) { MostrarMensaje("No se pudo publicar el aviso", ex.Message); }
        }

        // Ajusta el diseño cuando la vista ya dispone de su tamaño final.
        private void FormInicio_Load(object? sender, EventArgs e) => AjustarLayoutResponsive();

        // Recalcula el diseño al cambiar el tamaño del contenido principal.
        private void FormInicio_Resize(object? sender, EventArgs e) => AjustarLayoutResponsive();

        // Usa FormPrincipal como owner para mantener la ubicación coherente de los mensajes.
        private void MostrarMensaje(string titulo, string mensaje)
        {
            using FormMensaje formMensaje = new(titulo, mensaje);
            formMensaje.ShowDialog(formPrincipal);
        }
    }
}
