using System.Globalization;
using Capa_Logica;

namespace Capa_Vistas
{
    // Presenta el módulo unificado de Reportes con filtros ya resueltos por la lógica.
    public partial class FormReportesGeneral : Form
    {
        private readonly FormPrincipal formPrincipal;
        private readonly ReporteLogica reporteLogica = new();
        private readonly SucursalLogica sucursalLogica = new();
        private readonly TabControl tabResultados = new();
        private readonly DataGridView dgvVendedores = CrearGrilla();
        private readonly DataGridView dgvStock = CrearGrilla();
        private readonly DataGridView dgvDetalle = CrearGrilla();
        private readonly Button btnExportar = new();
        private readonly Dictionary<string, TabPage> paginasResultados = new();
        private List<ReporteDiaModelo> dias = new();

        // Recibe el principal para centrar mensajes y conservar navegación embebida.
        public FormReportesGeneral(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            AutoScroll = true;
            ConfigurarGrilla();
            ConfigurarResultados();
            ConfigurarEventos();
            PrepararVistaInicial();
        }

        // Configura columnas y alineación del ranking principal sin mezclar datos de negocio.
        private void ConfigurarGrilla()
        {
            dgvProductosVendidos.AutoGenerateColumns = false;
            dgvProductosVendidos.Columns.Clear();
            dgvProductosVendidos.Columns.Add("Producto", "Producto");
            dgvProductosVendidos.Columns.Add("Categoria", "Categoría");
            dgvProductosVendidos.Columns.Add("Unidades", "Unidades");
            int importe = dgvProductosVendidos.Columns.Add("Importe", "Importe");
            dgvProductosVendidos.Columns["Producto"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProductosVendidos.Columns["Unidades"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProductosVendidos.Columns[importe].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProductosVendidos.Columns[importe].DefaultCellStyle.Format = "C2";
            AplicarEstiloGrilla(dgvProductosVendidos);
        }

        // Enlaza eventos visuales fuera del Designer para mantenerlo como estructura fija.
        private void ConfigurarEventos()
        {
            btnAplicarFiltros.Click += BtnAplicarFiltros_Click;
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;
            btnExportar.Click += BtnExportar_Click;
            pnlGraficoContenido.Paint += PnlGraficoContenido_Paint;
            Load += FormReportesGeneral_Load;
            Resize += FormReportesGeneral_Resize;
        }

        // Prepara resultados adicionales como pestañas visuales reutilizables y consistentes.
        private void ConfigurarResultados()
        {
            tabResultados.Appearance = TabAppearance.Normal;
            tabResultados.Location = new Point(32, 760);
            tabResultados.Size = new Size(1116, 270);
            tabResultados.Visible = false;
            tabResultados.Font = new Font("Segoe UI", 9F);

            AgregarPestana("Vendedores", dgvVendedores, new[] { "Vendedor", "Ventas", "Total" });
            AgregarPestana("Stock bajo", dgvStock, new[] { "Producto", "Sucursal", "Stock", "Stock mínimo" });
            AgregarPestana("Ventas detalladas", dgvDetalle, new[] { "N°", "Fecha", "Cliente", "Vendedor", "Sucursal", "Subtotal", "Descuento", "Total" });
            Controls.Add(tabResultados);

            btnExportar.Text = "Exportar CSV";
            btnExportar.BackColor = Color.FromArgb(190, 137, 45);
            btnExportar.ForeColor = Color.White;
            btnExportar.FlatAppearance.BorderSize = 0;
            btnExportar.FlatStyle = FlatStyle.Flat;
            btnExportar.Size = new Size(128, 40);
            pnlFiltros.Controls.Add(btnExportar);
        }

        // Crea grillas de solo lectura con el mismo estilo que las secciones declaradas en Designer.
        private static DataGridView CrearGrilla()
        {
            DataGridView grilla = new()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            AplicarEstiloGrilla(grilla);
            return grilla;
        }

        // Normaliza encabezados, filas y alineación numérica para todas las grillas de Reportes.
        private static void AplicarEstiloGrilla(DataGridView grilla)
        {
            grilla.EnableHeadersVisualStyles = false;
            grilla.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grilla.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(82, 88, 94);
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            grilla.ColumnHeadersHeight = 38;
            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grilla.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grilla.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            grilla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 229, 232);
            grilla.DefaultCellStyle.SelectionForeColor = Color.FromArgb(40, 44, 48);
            grilla.RowTemplate.Height = 36;
            grilla.GridColor = Color.FromArgb(224, 227, 230);
        }

        // Inserta una pestaña con una grilla ya estilizada y guarda la referencia para alternarla visualmente.
        private void AgregarPestana(string titulo, DataGridView grilla, IEnumerable<string> columnas)
        {
            TabPage pagina = new(titulo) { BackColor = Color.White, Padding = new Padding(8) };
            foreach (string columna in columnas)
            {
                int indice = grilla.Columns.Add(columna, columna);
                if (columna is "Ventas" or "Stock" or "Stock mínimo" or "N°")
                    grilla.Columns[indice].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                if (columna is "Total" or "Subtotal" or "Descuento")
                {
                    grilla.Columns[indice].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    grilla.Columns[indice].DefaultCellStyle.Format = "C2";
                }
            }
            if (grilla.Columns.Count > 0) grilla.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            pagina.Controls.Add(grilla);
            paginasResultados.Add(titulo, pagina);
        }

        // Configura filtros disponibles y evita consultas cuando la sesión no tiene alcance válido.
        private void PrepararVistaInicial()
        {
            dtpDesde.Value = DateTime.Today.AddDays(-30);
            dtpHasta.Value = DateTime.Today;
            if (!reporteLogica.PuedeVerReportes()) { MostrarMensaje("Sin permiso", "No tiene permiso para acceder a Reportes."); return; }
            if (reporteLogica.ObtenerAlcanceReportes() == AlcanceReportes.Ninguno) { MostrarMensaje("Sin alcance", reporteLogica.ObtenerMensajeSinAlcance()); return; }
            CargarSucursales();
            CargarReporte();
        }

        // Carga el selector solamente para alcance global; los demás alcances permanecen fijos.
        private void CargarSucursales()
        {
            if (reporteLogica.ObtenerAlcanceReportes() != AlcanceReportes.Global)
            {
                cmbSucursal.Visible = false;
                lblSucursal.Visible = false;
                return;
            }

            List<OpcionSucursal> opciones = sucursalLogica.ObtenerSucursalesDisponibles()
                .Select(s => new OpcionSucursal { IdSucursal = s.IdSucursal, Nombre = s.Nombre }).ToList();
            opciones.Insert(0, new OpcionSucursal { IdSucursal = null, Nombre = "Todas las sucursales" });
            cmbSucursal.DataSource = opciones;
            cmbSucursal.DisplayMember = nameof(OpcionSucursal.Nombre);
            cmbSucursal.ValueMember = nameof(OpcionSucursal.IdSucursal);
        }

        // Carga tarjetas, gráfico y secciones usando exclusivamente los datos preparados por ReporteLogica.
        private void CargarReporte()
        {
            try
            {
                if (dtpDesde.Value.Date > dtpHasta.Value.Date)
                {
                    MostrarMensaje("Fechas inválidas", "La fecha Desde no puede ser posterior a la fecha Hasta.");
                    return;
                }

                int? sucursal = cmbSucursal.Visible && cmbSucursal.SelectedValue is int id ? id : null;
                bool ventas = reporteLogica.PuedeVerVentas();
                bool recaudacion = reporteLogica.PuedeVerRecaudacion();
                pnlTarjetaVentas.Visible = ventas;
                pnlTarjetaIngresos.Visible = recaudacion;
                pnlTarjetaProductos.Visible = reporteLogica.PuedeVerProductos();
                pnlTarjetaStock.Visible = reporteLogica.PuedeVerStock();

                if (ventas || recaudacion)
                {
                    var resumen = reporteLogica.ObtenerResumen(dtpDesde.Value, dtpHasta.Value, sucursal, null);
                    lblVentasValor.Text = resumen.Ventas.ToString(CultureInfo.CurrentCulture);
                    lblIngresosValor.Text = resumen.Recaudacion.ToString("C2", CultureInfo.CurrentCulture);
                }

                if (reporteLogica.PuedeVerProductos())
                {
                    var productos = reporteLogica.ObtenerProductos(dtpDesde.Value, dtpHasta.Value, sucursal, null);
                    lblProductosValor.Text = productos.Sum(producto => producto.Unidades).ToString(CultureInfo.CurrentCulture);
                    dgvProductosVendidos.Rows.Clear();
                    foreach (var producto in productos)
                        dgvProductosVendidos.Rows.Add(producto.Producto, producto.Categoria, producto.Unidades, producto.Importe);
                    lblProductosVendidosDescripcion.Text = productos.Count == 0
                        ? "No se registraron productos vendidos en el período seleccionado."
                        : "Ranking correspondiente al período seleccionado.";
                }

                if (reporteLogica.PuedeVerStock())
                    lblStockBajoValor.Text = reporteLogica.ObtenerStock(sucursal).Count.ToString(CultureInfo.CurrentCulture);

                dias = ventas || recaudacion
                    ? reporteLogica.ObtenerVentasPorDia(dtpDesde.Value, dtpHasta.Value, sucursal, null)
                    : new List<ReporteDiaModelo>();
                lblGraficoPlaceholder.Visible = dias.Count == 0;
                lblGraficoPlaceholder.Text = "No se registraron ventas en el período seleccionado.";
                pnlGraficoContenido.Invalidate();
                CargarResultadosAdicionales(sucursal);
                lblPeriodoGrafico.Text = $"Período: {dtpDesde.Value:dd/MM/yyyy} - {dtpHasta.Value:dd/MM/yyyy}";
                AjustarLayout();
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudieron cargar los reportes", ex.Message);
            }
        }

        // Muestra únicamente resultados autorizados y deja el estado sin datos dentro de cada pestaña.
        private void CargarResultadosAdicionales(int? sucursal)
        {
            tabResultados.TabPages.Clear();

            if (reporteLogica.PuedeVerRendimientoVendedores() && reporteLogica.ObtenerAlcanceReportes() != AlcanceReportes.Propio)
            {
                dgvVendedores.Rows.Clear();
                foreach (var vendedor in reporteLogica.ObtenerRendimiento(dtpDesde.Value, dtpHasta.Value, sucursal, null))
                    dgvVendedores.Rows.Add(vendedor.Vendedor, vendedor.Ventas, vendedor.Total);
                AgregarResultadoVisible("Vendedores", dgvVendedores.Rows.Count);
            }

            if (reporteLogica.PuedeVerStock())
            {
                dgvStock.Rows.Clear();
                foreach (var stock in reporteLogica.ObtenerStock(sucursal))
                    dgvStock.Rows.Add(stock.Producto, stock.Sucursal, stock.Stock, stock.StockMinimo);
                AgregarResultadoVisible("Stock bajo", dgvStock.Rows.Count);
            }

            if (reporteLogica.PuedeVerDetalleVentas())
            {
                dgvDetalle.Rows.Clear();
                foreach (var venta in reporteLogica.ObtenerDetalleVentas(dtpDesde.Value, dtpHasta.Value, sucursal, null))
                    dgvDetalle.Rows.Add(venta.IdVenta, venta.Fecha, venta.Cliente, venta.Vendedor, venta.Sucursal, venta.Subtotal, venta.Descuento, venta.Total);
                AgregarResultadoVisible("Ventas detalladas", dgvDetalle.Rows.Count);
            }

            tabResultados.Visible = tabResultados.TabPages.Count > 0;
            btnExportar.Visible = reporteLogica.PuedeExportar();
        }

        // Agrega una pestaña y explicita en su propio título cuando no existen registros para mostrar.
        private void AgregarResultadoVisible(string clave, int cantidad)
        {
            TabPage pagina = paginasResultados[clave];
            pagina.Text = cantidad == 0 ? $"{clave} — sin datos" : clave;
            tabResultados.TabPages.Add(pagina);
        }

        // Dibuja series reales con escalas independientes para ventas e ingresos y evita el placeholder con datos.
        private void PnlGraficoContenido_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(248, 249, 250));
            if (dias.Count == 0) return;

            Rectangle area = new(30, 18, Math.Max(1, pnlGraficoContenido.ClientSize.Width - 50), Math.Max(1, pnlGraficoContenido.ClientSize.Height - 45));
            decimal maxVentas = Math.Max(1, dias.Max(dia => (decimal)dia.Ventas));
            decimal maxIngresos = Math.Max(1, dias.Max(dia => dia.Recaudacion));
            int anchoGrupo = Math.Max(10, area.Width / Math.Max(1, dias.Count));
            int anchoBarra = Math.Max(3, (anchoGrupo - 5) / (reporteLogica.PuedeVerRecaudacion() ? 2 : 1));

            using Font fuente = new("Segoe UI", 7F);
            using Brush texto = new SolidBrush(Color.FromArgb(105, 110, 116));
            using Brush ventas = new SolidBrush(Color.FromArgb(82, 88, 94));
            using Brush ingresos = new SolidBrush(Color.FromArgb(190, 137, 45));

            for (int indice = 0; indice < dias.Count; indice++)
            {
                ReporteDiaModelo dia = dias[indice];
                int x = area.Left + indice * anchoGrupo;
                int altoVentas = (int)(area.Height * (dia.Ventas / maxVentas));
                e.Graphics.FillRectangle(ventas, x, area.Bottom - altoVentas, anchoBarra, altoVentas);

                if (reporteLogica.PuedeVerRecaudacion())
                {
                    int altoIngresos = (int)(area.Height * (dia.Recaudacion / maxIngresos));
                    e.Graphics.FillRectangle(ingresos, x + anchoBarra + 2, area.Bottom - altoIngresos, anchoBarra, altoIngresos);
                }

                if (dias.Count <= 14 || indice % 2 == 0)
                    e.Graphics.DrawString(dia.Fecha.ToString("dd/MM"), fuente, texto, x, area.Bottom + 4);
            }

            e.Graphics.DrawString("■ Ventas", fuente, ventas, area.Left, 2);
            if (reporteLogica.PuedeVerRecaudacion())
                e.Graphics.DrawString("■ Recaudación", fuente, ingresos, area.Left + 78, 2);
        }

        // Exporta la sección visible seleccionada sin incorporar dependencias externas.
        private void BtnExportar_Click(object? sender, EventArgs e)
        {
            DataGridView? grilla = tabResultados.SelectedTab?.Controls.OfType<DataGridView>().FirstOrDefault() ?? dgvProductosVendidos;
            if (grilla.Rows.Count == 0) { MostrarMensaje("Sin datos", "No hay datos para exportar."); return; }
            using SaveFileDialog dialogo = new() { Filter = "CSV (*.csv)|*.csv", FileName = "reporte.csv" };
            if (dialogo.ShowDialog(formPrincipal) != DialogResult.OK) return;
            using StreamWriter escritor = new(dialogo.FileName, false, System.Text.Encoding.UTF8);
            escritor.WriteLine(string.Join(";", grilla.Columns.Cast<DataGridViewColumn>().Select(columna => columna.HeaderText)));
            foreach (DataGridViewRow fila in grilla.Rows)
                escritor.WriteLine(string.Join(";", fila.Cells.Cast<DataGridViewCell>().Select(celda => '"' + (celda.Value?.ToString() ?? string.Empty).Replace("\"", "\"\"") + '"')));
        }

        // Recalcula posiciones, altura y scroll vertical para tarjetas, filtros y secciones en cualquier ancho.
        private void FormReportesGeneral_Load(object? sender, EventArgs e) => AjustarLayout();

        // Recalcula el diseño al cambiar el tamaño disponible dentro de pnlContenido.
        private void FormReportesGeneral_Resize(object? sender, EventArgs e) => AjustarLayout();

        // Distribuye el contenido en 4, 2 o 1 tarjetas por fila y mantiene acciones del filtro alineadas.
        private void AjustarLayout()
        {
            int margen = 32;
            int ancho = Math.Max(340, ClientSize.Width - margen * 2);
            int columnas = ancho >= 1040 ? 4 : ancho >= 650 ? 2 : 1;
            int separacion = 16;
            int anchoTarjeta = (ancho - separacion * (columnas - 1)) / columnas;
            int y = 18;

            pnlCabecera.SetBounds(margen, y, ancho, 82);
            y += 94;
            AjustarFiltros(ancho, margen, ref y);

            Panel[] tarjetas = { pnlTarjetaVentas, pnlTarjetaIngresos, pnlTarjetaProductos, pnlTarjetaStock };
            List<Panel> visibles = tarjetas.Where(tarjeta => tarjeta.Visible).ToList();
            for (int indice = 0; indice < visibles.Count; indice++)
                visibles[indice].SetBounds(margen + (indice % columnas) * (anchoTarjeta + separacion), y + (indice / columnas) * 126, anchoTarjeta, 110);
            AjustarContenidoTarjetas(visibles);
            y += Math.Max(1, (visibles.Count + columnas - 1) / columnas) * 126 + 10;

            pnlGrafico.SetBounds(margen, y, ancho, 230);
            y += 246;
            pnlProductosVendidos.SetBounds(margen, y, ancho, 230);
            y += 246;

            tabResultados.SetBounds(margen, y, ancho, 290);
            if (tabResultados.Visible) y += 306;
            AutoScrollMinSize = new Size(0, y + 24);
        }

        // Reordena los campos de filtros y las acciones para ocupar el ancho disponible sin espacios flotantes.
        private void AjustarFiltros(int ancho, int margen, ref int y)
        {
            int columnas = ancho >= 950 ? 3 : ancho >= 610 ? 2 : 1;
            int anchoCampo = Math.Min(230, (ancho - 36 - (columnas - 1) * 16) / columnas);
            int alto = columnas == 1 ? 205 : 135;
            pnlFiltros.SetBounds(margen, y, ancho, alto);

            Control[] etiquetas = { lblDesde, lblHasta, lblSucursal };
            Control[] campos = { dtpDesde, dtpHasta, cmbSucursal };
            int indiceCampo = 0;
            for (int indice = 0; indice < campos.Length; indice++)
            {
                if (!campos[indice].Visible) continue;
                int columna = indiceCampo % columnas;
                int fila = indiceCampo / columnas;
                int izquierda = 18 + columna * (anchoCampo + 16);
                int superior = 40 + fila * 58;
                etiquetas[indice].SetBounds(izquierda, superior, anchoCampo, 20);
                campos[indice].SetBounds(izquierda, superior + 22, anchoCampo, 28);
                indiceCampo++;
            }

            int accionesY = columnas == 1 ? 145 : 82;
            btnAplicarFiltros.SetBounds(ancho - 18 - 420, accionesY, 140, 40);
            btnLimpiarFiltros.SetBounds(ancho - 18 - 270, accionesY, 120, 40);
            btnExportar.SetBounds(ancho - 18 - 140, accionesY, 140, 40);
            y += alto + 14;
        }

        // Ajusta la tipografía de ingresos y el padding de cada tarjeta para que importes extensos no se recorten.
        private void AjustarContenidoTarjetas(IEnumerable<Panel> tarjetas)
        {
            foreach (Panel tarjeta in tarjetas)
            {
                Label? titulo = tarjeta.Controls.OfType<Label>().FirstOrDefault(etiqueta => etiqueta.Name.EndsWith("Titulo"));
                Label? valor = tarjeta.Controls.OfType<Label>().FirstOrDefault(etiqueta => etiqueta.Name.EndsWith("Valor"));
                Label? descripcion = tarjeta.Controls.OfType<Label>().FirstOrDefault(etiqueta => etiqueta.Name.EndsWith("Descripcion"));
                if (titulo == null || valor == null || descripcion == null) continue;
                titulo.SetBounds(16, 12, tarjeta.ClientSize.Width - 32, 20);
                valor.AutoSize = false;
                valor.TextAlign = ContentAlignment.MiddleLeft;
                valor.SetBounds(14, 33, tarjeta.ClientSize.Width - 28, 42);
                descripcion.SetBounds(17, 82, tarjeta.ClientSize.Width - 34, 20);
                descripcion.AutoEllipsis = true;
                AjustarFuenteValor(valor);
            }
        }

        // Reduce progresivamente la fuente hasta que el valor monetario completo entra en la tarjeta.
        private static void AjustarFuenteValor(Label etiqueta)
        {
            float tamano = 23F;
            using Graphics graficos = etiqueta.CreateGraphics();
            while (tamano > 11F)
            {
                using Font prueba = new("Segoe UI", tamano, FontStyle.Bold);
                if (graficos.MeasureString(etiqueta.Text, prueba).Width <= etiqueta.ClientSize.Width) break;
                tamano -= 1F;
            }
            etiqueta.Font = new Font("Segoe UI", tamano, FontStyle.Bold);
        }

        // Aplica las fechas y alcance seleccionados después de la prevención visual.
        private void BtnAplicarFiltros_Click(object? sender, EventArgs e) => CargarReporte();

        // Restablece el período inicial y la sucursal global cuando corresponde.
        private void BtnLimpiarFiltros_Click(object? sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Today.AddDays(-30);
            dtpHasta.Value = DateTime.Today;
            if (cmbSucursal.Visible) cmbSucursal.SelectedIndex = 0;
            CargarReporte();
        }

        // Centra mensajes funcionales en FormPrincipal en lugar de usar MessageBox.
        private void MostrarMensaje(string titulo, string mensaje)
        {
            using FormMensaje dialogo = new(titulo, mensaje);
            dialogo.ShowDialog(formPrincipal);
        }

        // Representa una sucursal del selector sin depender de posiciones o IDs fijos.
        private class OpcionSucursal
        {
            public int? IdSucursal { get; set; }
            public string Nombre { get; set; } = string.Empty;
        }
    }
}
