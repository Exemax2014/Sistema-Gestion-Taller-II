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
        private bool puedePublicarAvisos;
        private bool puedeVerVentas;
        private bool puedeVerProductos;
        private bool puedeVerClientes;
        private bool puedeRealizarVentas;
        private bool puedeVerAvisos;

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
            ConfigurarGrilla(dgvAvisos, new[] { "Fecha", "Título", "Mensaje", "Autor", "Alcance" });
            dgvAvisos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvAvisos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAvisos.Columns[0].FillWeight = 65;
            dgvAvisos.Columns[1].FillWeight = 100;
            dgvAvisos.Columns[2].FillWeight = 230;
            dgvAvisos.Columns[3].FillWeight = 100;
            dgvAvisos.Columns[4].FillWeight = 135;
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
            pnlGraficoIngresos.Visible = puedeVerVentas;
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

            txtAvisoTitulo.Visible = puedePublicarAvisos;
            txtAvisoMensaje.Visible = puedePublicarAvisos;
            lblAvisoMensaje.Visible = puedePublicarAvisos;
            lblAvisoAlcance.Visible = puedePublicarAvisos && alcanceGlobal;
            cmbAvisoSucursal.Visible = puedePublicarAvisos && alcanceGlobal;
            lblAvisoSucursalFija.Visible = puedePublicarAvisos && !alcanceGlobal;
            lblAvisoSucursalFija.Text = $"Sucursal: {SesionActual.Sucursal}";
            btnPublicarAviso.Visible = puedePublicarAvisos;
            AjustarLayoutResponsive();
        }

        // Describe el alcance realmente seleccionado, sin inferirlo desde el nombre del perfil.
        private string ObtenerDescripcionSesion()
        {
            string perfil = string.IsNullOrWhiteSpace(SesionActual.Perfil) ? "Usuario" : SesionActual.Perfil;
            string alcance = SesionActual.IdSucursalOperativa.HasValue ? SesionActual.SucursalOperativa : "Todo el negocio";
            return $"{perfil} · {alcance}";
        }

        // Redistribuye solo los bloques autorizados y adapta accesos al espacio disponible.
        private void AjustarLayoutResponsive()
        {
            int margen = 32, ancho = Math.Max(320, pnlPrincipal.ClientSize.Width - margen * 2);
            int separacion = 16;
            pnlPrincipal.AutoScroll = true;
            int altoEncabezado = ancho >= 700 ? 100 : 122;
            pnlEncabezado.SetBounds(margen, 20, ancho, altoEncabezado);
            lblBienvenida.AutoSize = false;
            lblBienvenida.AutoEllipsis = true;
            lblBienvenida.SetBounds(0, 2, ancho >= 700 ? Math.Max(260, ancho - 360) : ancho - 4, 50);
            lblSubtitulo.AutoSize = false;
            lblSubtitulo.SetBounds(2, 54, ancho - 4, 22);
            lblPerfilSucursal.AutoSize = false;
            lblPerfilSucursal.AutoEllipsis = true;
            lblPerfilSucursal.SetBounds(ancho >= 700 ? ancho - 340 : 2, ancho >= 700 ? 26 : 78,
                ancho >= 700 ? 330 : ancho - 4, 22);
            pnlLineaDorada.SetBounds(2, altoEncabezado == 100 ? 84 : 104, 92, 3);
            List<Panel> tarjetasVisibles = new();
            if (puedeVerVentas) tarjetasVisibles.AddRange(new[] { pnlTarjetaVentas, pnlTarjetaIngresos });
            if (puedeVerProductos) tarjetasVisibles.AddRange(new[] { pnlTarjetaStock, pnlTarjetaProductos });
            int maxColumnas = ancho >= 1000 ? 4 : ancho >= 620 ? 2 : 1;
            int columnas = Math.Max(1, Math.Min(maxColumnas, tarjetasVisibles.Count));
            int anchoTarjeta = (ancho - separacion * (columnas - 1)) / columnas;
            int inicioContenido = pnlEncabezado.Bottom;
            for (int i = 0; i < tarjetasVisibles.Count; i++)
                tarjetasVisibles[i].SetBounds(margen + (i % columnas) * (anchoTarjeta + separacion), inicioContenido + (i / columnas) * 126, anchoTarjeta, 110);

            int filasTarjetas = (tarjetasVisibles.Count + columnas - 1) / columnas;
            int y = inicioContenido + filasTarjetas * 126;
            bool hayAccesos = puedeRealizarVentas || puedeVerProductos || puedeVerClientes;
            pnlAccesos.Visible = hayAccesos;
            if (hayAccesos)
            {
                pnlAccesos.SetBounds(margen, y, ancho, 90);
                int altoAccesos = AjustarAccesosRapidos(ancho);
                pnlAccesos.Height = altoAccesos;
                y += altoAccesos + 20;
            }

            List<Panel> graficosVisibles = new();
            if (puedeVerVentas) graficosVisibles.AddRange(new[] { pnlGraficoVentas, pnlGraficoIngresos });
            if (puedeVerVentas && puedeVerProductos) graficosVisibles.Add(pnlGraficoProductos);
            pnlGraficos.Visible = graficosVisibles.Count > 0;
            if (pnlGraficos.Visible)
            {
                int columnasGraficos = ancho >= 920 ? Math.Min(3, graficosVisibles.Count) : 1;
                int anchoGrafico = (ancho - separacion * (columnasGraficos - 1)) / columnasGraficos;
                int filasGraficos = (graficosVisibles.Count + columnasGraficos - 1) / columnasGraficos;
                pnlGraficos.SetBounds(margen, y, ancho, filasGraficos * 185);
                for (int i = 0; i < graficosVisibles.Count; i++)
                    graficosVisibles[i].SetBounds((i % columnasGraficos) * (anchoGrafico + separacion), (i / columnasGraficos) * 185, anchoGrafico, 170);
                y += pnlGraficos.Height + 20;
            }

            if (puedeVerVentas)
            {
                pnlActividad.SetBounds(margen, y, ancho, 270);
                y += 290;
            }

            if (!puedePublicarAvisos && !puedeVerAvisos)
            {
                pnlPrincipal.AutoScrollMinSize = new Size(0, y + 25);
                return;
            }
            int altoAvisos = puedePublicarAvisos ? 465 : 330;
            pnlAvisos.SetBounds(margen, y, ancho, altoAvisos);
            AjustarLayoutAvisos();
            pnlPrincipal.AutoScrollMinSize = new Size(0, y + altoAvisos + 25);
        }

        // Distribuye los accesos autorizados en columnas responsivas y adapta el subtítulo.
        private int AjustarAccesosRapidos(int ancho)
        {
            int margen = 18;
            bool encabezadoEnUnaFila = ancho >= 560;
            lblAccesosTitulo.AutoSize = false;
            lblAccesosTitulo.SetBounds(margen, 11, encabezadoEnUnaFila ? 155 : ancho - margen * 2, 24);
            lblAccesosDescripcion.AutoSize = false;
            lblAccesosDescripcion.AutoEllipsis = true;
            lblAccesosDescripcion.SetBounds(encabezadoEnUnaFila ? 180 : margen, encabezadoEnUnaFila ? 15 : 36,
                Math.Max(40, ancho - (encabezadoEnUnaFila ? 198 : margen * 2)), 20);

            List<Button> botones = new();
            if (puedeRealizarVentas) botones.Add(btnNuevaVenta);
            if (puedeVerProductos) botones.Add(btnProductos);
            if (puedeVerClientes) botones.Add(btnClientes);
            int columnas = ancho >= 900 ? Math.Min(3, botones.Count) : ancho >= 560 ? Math.Min(2, botones.Count) : 1;
            if (columnas == 0) return encabezadoEnUnaFila ? 54 : 70;

            int espacio = ancho - margen * 2;
            int separacion = 12;
            int anchoBoton = (espacio - separacion * (columnas - 1)) / columnas;
            int yBotones = encabezadoEnUnaFila ? 51 : 65;
            for (int i = 0; i < botones.Count; i++)
            {
                int columna = i % columnas;
                int fila = i / columnas;
                botones[i].SetBounds(margen + columna * (anchoBoton + separacion), yBotones + fila * 50, anchoBoton, 42);
            }
            int filas = (botones.Count + columnas - 1) / columnas;
            return yBotones + filas * 50 + 8;
        }

        // Reorganiza publicación y avisos recibidos para aprovechar el ancho sin ocultar información relevante.
        private void AjustarLayoutAvisos()
        {
            int margen = 18;
            int ancho = Math.Max(280, pnlAvisos.ClientSize.Width - (margen * 2));
            int yContenido = 48;

            if (puedePublicarAvisos)
            {
                if (ancho >= 760)
                {
                    int anchoPublicacion = (int)(ancho * 0.63);
                    int xAlcance = margen + anchoPublicacion + 16;
                    int anchoAlcance = ancho - anchoPublicacion - 16;

                    txtAvisoTitulo.SetBounds(margen, 48, anchoPublicacion, 27);
                    lblAvisoMensaje.SetBounds(margen, 84, 220, 20);
                    txtAvisoMensaje.SetBounds(margen, 106, anchoPublicacion, 100);
                    lblAvisoAlcance.SetBounds(xAlcance, 48, anchoAlcance, 20);
                    cmbAvisoSucursal.SetBounds(xAlcance, 70, anchoAlcance, 28);
                    lblAvisoSucursalFija.SetBounds(xAlcance, 70, anchoAlcance, 28);
                    btnPublicarAviso.SetBounds(
                        Math.Max(xAlcance, xAlcance + anchoAlcance - 150),
                        116,
                        150,
                        36
                    );
                    yContenido = 280;
                }
                else
                {
                    txtAvisoTitulo.SetBounds(margen, 48, ancho, 27);
                    lblAvisoMensaje.SetBounds(margen, 84, ancho, 20);
                    txtAvisoMensaje.SetBounds(margen, 106, ancho, 100);
                    lblAvisoAlcance.SetBounds(margen, 216, ancho, 20);
                    cmbAvisoSucursal.SetBounds(margen, 238, ancho, 28);
                    lblAvisoSucursalFija.SetBounds(margen, 238, ancho, 28);
                    btnPublicarAviso.SetBounds(Math.Max(margen, ancho - 132 + margen), 280, 132, 36);
                    yContenido = 335;
                }
            }

            int altoListado = Math.Max(120, pnlAvisos.ClientSize.Height - yContenido - 18);
            dgvAvisos.SetBounds(margen, yContenido, ancho, altoListado);
            lblAvisosVacio.SetBounds(margen, yContenido, ancho, altoListado);
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

        // Revalida permiso y sucursal operativa antes de iniciar una venta desde Inicio.
        private void BtnNuevaVenta_Click(object? sender, EventArgs e)
        {
            if (!SesionActual.TienePermiso("VENTAS_REALIZAR")) return;
            if (!SesionActual.IdSucursalOperativa.HasValue) { MostrarMensaje("Sucursal operativa requerida", "Para realizar una venta primero seleccioná una sucursal operativa."); return; }
            formPrincipal.AbrirFormularioEnPanel(new FormVentas(), formPrincipal.BotonVentas);
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
                txtAvisoTitulo.Clear();
                txtAvisoMensaje.Clear();
                CargarAvisos();
            }
            catch (Exception ex) { MostrarMensaje("No se pudo publicar el aviso", ex.Message); }
        }

        // Recarga avisos y alcance de publicación cuando FormPrincipal cambia la sucursal operativa.
        public void ActualizarPorSucursalOperativa()
        {
            CargarAvisos();
            ConfigurarAlcanceAviso();
        }

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
