namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormReportesGeneral
    //
    // Vista general de reportes.
    //
    // El Designer solamente maneja controles y posiciones.
    // Las columnas, eventos y comportamiento se configuran acá.
    // ============================================================

    public partial class FormReportesGeneral : Form
    {
        public FormReportesGeneral()
        {
            InitializeComponent();

            ConfigurarGrilla();
            ConfigurarEventos();
            PrepararVistaInicial();
        }


        // ========================================================
        // GRILLA
        //
        // Las columnas se crean fuera del Designer.
        // ========================================================

        private void ConfigurarGrilla()
        {
            dgvProductosVendidos.AutoGenerateColumns =
                false;

            dgvProductosVendidos.Columns.Clear();


            DataGridViewTextBoxColumn colProducto =
                new DataGridViewTextBoxColumn();

            colProducto.Name =
                "colProducto";

            colProducto.HeaderText =
                "Producto";

            colProducto.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;

            colProducto.MinimumWidth =
                220;


            DataGridViewTextBoxColumn colCategoria =
                new DataGridViewTextBoxColumn();

            colCategoria.Name =
                "colCategoria";

            colCategoria.HeaderText =
                "Categoría";

            colCategoria.Width =
                180;


            DataGridViewTextBoxColumn colCantidad =
                new DataGridViewTextBoxColumn();

            colCantidad.Name =
                "colCantidad";

            colCantidad.HeaderText =
                "Cantidad vendida";

            colCantidad.Width =
                150;


            DataGridViewTextBoxColumn colImporte =
                new DataGridViewTextBoxColumn();

            colImporte.Name =
                "colImporte";

            colImporte.HeaderText =
                "Importe";

            colImporte.Width =
                150;

            colImporte.DefaultCellStyle.Format =
                "C2";


            dgvProductosVendidos.Columns.AddRange(
                colProducto,
                colCategoria,
                colCantidad,
                colImporte
            );
        }


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            btnAplicarFiltros.Click +=
                BtnAplicarFiltros_Click;

            btnLimpiarFiltros.Click +=
                BtnLimpiarFiltros_Click;
        }


        // ========================================================
        // VISTA INICIAL
        // ========================================================

        private void PrepararVistaInicial()
        {
            cmbSucursal.Items.Clear();

            cmbSucursal.Items.Add(
                "Todas"
            );

            cmbSucursal.SelectedIndex =
                0;


            dtpDesde.Value =
                DateTime.Today.AddDays(-30);

            dtpHasta.Value =
                DateTime.Today;


            lblVentasValor.Text =
                "0";

            lblIngresosValor.Text =
                "$ 0,00";

            lblProductosValor.Text =
                "0";

            lblStockBajoValor.Text =
                "0";


            lblPeriodoGrafico.Text =
                "Últimos 30 días";
        }


        // ========================================================
        // APLICAR FILTROS
        // ========================================================

        private void BtnAplicarFiltros_Click(
            object? sender,
            EventArgs e)
        {
            lblPeriodoGrafico.Text =
                $"Período: {dtpDesde.Value:dd/MM/yyyy} - {dtpHasta.Value:dd/MM/yyyy}";
        }


        // ========================================================
        // LIMPIAR
        // ========================================================

        private void BtnLimpiarFiltros_Click(
            object? sender,
            EventArgs e)
        {
            dtpDesde.Value =
                DateTime.Today.AddDays(-30);

            dtpHasta.Value =
                DateTime.Today;


            if (cmbSucursal.Items.Count > 0)
            {
                cmbSucursal.SelectedIndex =
                    0;
            }


            lblPeriodoGrafico.Text =
                "Últimos 30 días";
        }
    }
}