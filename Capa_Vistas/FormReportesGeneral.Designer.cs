using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormReportesGeneral
    {
        private System.ComponentModel.IContainer components =
            null;


        private Panel pnlCabecera;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Panel pnlLineaTitulo;


        private Panel pnlFiltros;
        private Label lblFiltrosTitulo;

        private Label lblDesde;
        private DateTimePicker dtpDesde;

        private Label lblHasta;
        private DateTimePicker dtpHasta;

        private Label lblSucursal;
        private ComboBox cmbSucursal;

        private Button btnAplicarFiltros;
        private Button btnLimpiarFiltros;


        private Panel pnlTarjetaVentas;
        private Label lblVentasTitulo;
        private Label lblVentasValor;
        private Label lblVentasDescripcion;


        private Panel pnlTarjetaIngresos;
        private Label lblIngresosTitulo;
        private Label lblIngresosValor;
        private Label lblIngresosDescripcion;


        private Panel pnlTarjetaProductos;
        private Label lblProductosTitulo;
        private Label lblProductosValor;
        private Label lblProductosDescripcion;


        private Panel pnlTarjetaStock;
        private Label lblStockBajoTitulo;
        private Label lblStockBajoValor;
        private Label lblStockBajoDescripcion;


        private Panel pnlGrafico;
        private Label lblGraficoTitulo;
        private Label lblPeriodoGrafico;
        private Panel pnlGraficoContenido;
        private Label lblGraficoPlaceholder;


        private Panel pnlProductosVendidos;
        private Label lblProductosVendidosTitulo;
        private Label lblProductosVendidosDescripcion;
        private DataGridView dgvProductosVendidos;


        protected override void Dispose(
            bool disposing)
        {
            if (
                disposing
                &&
                components != null)
            {
                components.Dispose();
            }

            base.Dispose(
                disposing
            );
        }


        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlCabecera = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaTitulo = new Panel();
            pnlFiltros = new Panel();
            lblFiltrosTitulo = new Label();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            lblSucursal = new Label();
            cmbSucursal = new ComboBox();
            btnAplicarFiltros = new Button();
            btnLimpiarFiltros = new Button();
            pnlTarjetaVentas = new Panel();
            lblVentasTitulo = new Label();
            lblVentasValor = new Label();
            lblVentasDescripcion = new Label();
            pnlTarjetaIngresos = new Panel();
            lblIngresosTitulo = new Label();
            lblIngresosValor = new Label();
            lblIngresosDescripcion = new Label();
            pnlTarjetaProductos = new Panel();
            lblProductosTitulo = new Label();
            lblProductosValor = new Label();
            lblProductosDescripcion = new Label();
            pnlTarjetaStock = new Panel();
            lblStockBajoTitulo = new Label();
            lblStockBajoValor = new Label();
            lblStockBajoDescripcion = new Label();
            pnlGrafico = new Panel();
            lblGraficoTitulo = new Label();
            lblPeriodoGrafico = new Label();
            pnlGraficoContenido = new Panel();
            lblGraficoPlaceholder = new Label();
            pnlProductosVendidos = new Panel();
            lblProductosVendidosTitulo = new Label();
            lblProductosVendidosDescripcion = new Label();
            dgvProductosVendidos = new DataGridView();
            pnlCabecera.SuspendLayout();
            pnlFiltros.SuspendLayout();
            pnlTarjetaVentas.SuspendLayout();
            pnlTarjetaIngresos.SuspendLayout();
            pnlTarjetaProductos.SuspendLayout();
            pnlTarjetaStock.SuspendLayout();
            pnlGrafico.SuspendLayout();
            pnlGraficoContenido.SuspendLayout();
            pnlProductosVendidos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductosVendidos).BeginInit();
            SuspendLayout();
            // 
            // pnlCabecera
            // 
            pnlCabecera.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Controls.Add(lblSubtitulo);
            pnlCabecera.Controls.Add(pnlLineaTitulo);
            pnlCabecera.Location = new Point(32, 18);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(1116, 82);
            pnlCabecera.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(45, 49, 54);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(177, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reportes";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(2, 48);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(307, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Resumen general de la actividad del sistema.";
            // 
            // pnlLineaTitulo
            // 
            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(2, 74);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(95, 3);
            pnlLineaTitulo.TabIndex = 2;
            // 
            // pnlFiltros
            // 
            pnlFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFiltros.BackColor = Color.White;
            pnlFiltros.BorderStyle = BorderStyle.FixedSingle;
            pnlFiltros.Controls.Add(lblFiltrosTitulo);
            pnlFiltros.Controls.Add(lblDesde);
            pnlFiltros.Controls.Add(dtpDesde);
            pnlFiltros.Controls.Add(lblHasta);
            pnlFiltros.Controls.Add(dtpHasta);
            pnlFiltros.Controls.Add(lblSucursal);
            pnlFiltros.Controls.Add(cmbSucursal);
            pnlFiltros.Controls.Add(btnAplicarFiltros);
            pnlFiltros.Controls.Add(btnLimpiarFiltros);
            pnlFiltros.Location = new Point(32, 106);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Size = new Size(1116, 105);
            pnlFiltros.TabIndex = 1;
            // 
            // lblFiltrosTitulo
            // 
            lblFiltrosTitulo.AutoSize = true;
            lblFiltrosTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFiltrosTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblFiltrosTitulo.Location = new Point(18, 10);
            lblFiltrosTitulo.Name = "lblFiltrosTitulo";
            lblFiltrosTitulo.Size = new Size(60, 23);
            lblFiltrosTitulo.TabIndex = 0;
            lblFiltrosTitulo.Text = "Filtros";
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(20, 41);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(51, 20);
            lblDesde.TabIndex = 1;
            lblDesde.Text = "Desde";
            // 
            // dtpDesde
            // 
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(20, 64);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(150, 27);
            dtpDesde.TabIndex = 2;
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(190, 41);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(47, 20);
            lblHasta.TabIndex = 3;
            lblHasta.Text = "Hasta";
            // 
            // dtpHasta
            // 
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(190, 64);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(150, 27);
            dtpHasta.TabIndex = 4;
            // 
            // lblSucursal
            // 
            lblSucursal.AutoSize = true;
            lblSucursal.Location = new Point(360, 41);
            lblSucursal.Name = "lblSucursal";
            lblSucursal.Size = new Size(63, 20);
            lblSucursal.TabIndex = 5;
            lblSucursal.Text = "Sucursal";
            // 
            // cmbSucursal
            // 
            cmbSucursal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSucursal.Location = new Point(360, 64);
            cmbSucursal.Name = "cmbSucursal";
            cmbSucursal.Size = new Size(230, 28);
            cmbSucursal.TabIndex = 6;
            // 
            // btnAplicarFiltros
            // 
            btnAplicarFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAplicarFiltros.BackColor = Color.FromArgb(190, 137, 45);
            btnAplicarFiltros.Cursor = Cursors.Hand;
            btnAplicarFiltros.FlatAppearance.BorderSize = 0;
            btnAplicarFiltros.FlatStyle = FlatStyle.Flat;
            btnAplicarFiltros.ForeColor = Color.White;
            btnAplicarFiltros.Location = new Point(776, 41);
            btnAplicarFiltros.Name = "btnAplicarFiltros";
            btnAplicarFiltros.Size = new Size(155, 40);
            btnAplicarFiltros.TabIndex = 7;
            btnAplicarFiltros.Text = "Aplicar filtros";
            btnAplicarFiltros.UseVisualStyleBackColor = false;
            // 
            // btnLimpiarFiltros
            // 
            btnLimpiarFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLimpiarFiltros.BackColor = Color.White;
            btnLimpiarFiltros.Cursor = Cursors.Hand;
            btnLimpiarFiltros.FlatAppearance.BorderColor = Color.FromArgb(160, 165, 170);
            btnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
            btnLimpiarFiltros.Location = new Point(954, 52);
            btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            btnLimpiarFiltros.Size = new Size(140, 40);
            btnLimpiarFiltros.TabIndex = 8;
            btnLimpiarFiltros.Text = "Limpiar";
            btnLimpiarFiltros.UseVisualStyleBackColor = false;
            // 
            // pnlTarjetaVentas
            // 
            pnlTarjetaVentas.BackColor = Color.White;
            pnlTarjetaVentas.BorderStyle = BorderStyle.FixedSingle;
            pnlTarjetaVentas.Controls.Add(lblVentasTitulo);
            pnlTarjetaVentas.Controls.Add(lblVentasValor);
            pnlTarjetaVentas.Controls.Add(lblVentasDescripcion);
            pnlTarjetaVentas.Location = new Point(32, 225);
            pnlTarjetaVentas.Name = "pnlTarjetaVentas";
            pnlTarjetaVentas.Size = new Size(260, 110);
            pnlTarjetaVentas.TabIndex = 2;
            // 
            // lblVentasTitulo
            // 
            lblVentasTitulo.AutoSize = true;
            lblVentasTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVentasTitulo.ForeColor = Color.FromArgb(80, 85, 90);
            lblVentasTitulo.Location = new Point(16, 13);
            lblVentasTitulo.Name = "lblVentasTitulo";
            lblVentasTitulo.Size = new Size(56, 20);
            lblVentasTitulo.TabIndex = 0;
            lblVentasTitulo.Text = "Ventas";
            // 
            // lblVentasValor
            // 
            lblVentasValor.AutoSize = true;
            lblVentasValor.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblVentasValor.ForeColor = Color.FromArgb(45, 49, 54);
            lblVentasValor.Location = new Point(14, 31);
            lblVentasValor.Name = "lblVentasValor";
            lblVentasValor.Size = new Size(44, 52);
            lblVentasValor.TabIndex = 1;
            lblVentasValor.Text = "0";
            // 
            // lblVentasDescripcion
            // 
            lblVentasDescripcion.AutoSize = true;
            lblVentasDescripcion.Font = new Font("Segoe UI", 8F);
            lblVentasDescripcion.ForeColor = Color.FromArgb(115, 120, 125);
            lblVentasDescripcion.Location = new Point(17, 82);
            lblVentasDescripcion.Name = "lblVentasDescripcion";
            lblVentasDescripcion.Size = new Size(158, 19);
            lblVentasDescripcion.TabIndex = 2;
            lblVentasDescripcion.Text = "Operaciones del período";
            // 
            // pnlTarjetaIngresos
            // 
            pnlTarjetaIngresos.Anchor = AnchorStyles.Top;
            pnlTarjetaIngresos.BackColor = Color.White;
            pnlTarjetaIngresos.BorderStyle = BorderStyle.FixedSingle;
            pnlTarjetaIngresos.Controls.Add(lblIngresosTitulo);
            pnlTarjetaIngresos.Controls.Add(lblIngresosValor);
            pnlTarjetaIngresos.Controls.Add(lblIngresosDescripcion);
            pnlTarjetaIngresos.Location = new Point(317, 225);
            pnlTarjetaIngresos.Name = "pnlTarjetaIngresos";
            pnlTarjetaIngresos.Size = new Size(260, 110);
            pnlTarjetaIngresos.TabIndex = 3;
            // 
            // lblIngresosTitulo
            // 
            lblIngresosTitulo.AutoSize = true;
            lblIngresosTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblIngresosTitulo.ForeColor = Color.FromArgb(80, 85, 90);
            lblIngresosTitulo.Location = new Point(16, 13);
            lblIngresosTitulo.Name = "lblIngresosTitulo";
            lblIngresosTitulo.Size = new Size(69, 20);
            lblIngresosTitulo.TabIndex = 0;
            lblIngresosTitulo.Text = "Ingresos";
            // 
            // lblIngresosValor
            // 
            lblIngresosValor.AutoSize = true;
            lblIngresosValor.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblIngresosValor.ForeColor = Color.FromArgb(190, 137, 45);
            lblIngresosValor.Location = new Point(14, 31);
            lblIngresosValor.Name = "lblIngresosValor";
            lblIngresosValor.Size = new Size(132, 52);
            lblIngresosValor.TabIndex = 1;
            lblIngresosValor.Text = "$ 0,00";
            // 
            // lblIngresosDescripcion
            // 
            lblIngresosDescripcion.AutoSize = true;
            lblIngresosDescripcion.Font = new Font("Segoe UI", 8F);
            lblIngresosDescripcion.ForeColor = Color.FromArgb(115, 120, 125);
            lblIngresosDescripcion.Location = new Point(17, 82);
            lblIngresosDescripcion.Name = "lblIngresosDescripcion";
            lblIngresosDescripcion.Size = new Size(111, 19);
            lblIngresosDescripcion.TabIndex = 2;
            lblIngresosDescripcion.Text = "Total del período";
            // 
            // pnlTarjetaProductos
            // 
            pnlTarjetaProductos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlTarjetaProductos.BackColor = Color.White;
            pnlTarjetaProductos.BorderStyle = BorderStyle.FixedSingle;
            pnlTarjetaProductos.Controls.Add(lblProductosTitulo);
            pnlTarjetaProductos.Controls.Add(lblProductosValor);
            pnlTarjetaProductos.Controls.Add(lblProductosDescripcion);
            pnlTarjetaProductos.Location = new Point(602, 225);
            pnlTarjetaProductos.Name = "pnlTarjetaProductos";
            pnlTarjetaProductos.Size = new Size(260, 110);
            pnlTarjetaProductos.TabIndex = 4;
            // 
            // lblProductosTitulo
            // 
            lblProductosTitulo.AutoSize = true;
            lblProductosTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProductosTitulo.ForeColor = Color.FromArgb(80, 85, 90);
            lblProductosTitulo.Location = new Point(16, 13);
            lblProductosTitulo.Name = "lblProductosTitulo";
            lblProductosTitulo.Size = new Size(80, 20);
            lblProductosTitulo.TabIndex = 0;
            lblProductosTitulo.Text = "Productos";
            // 
            // lblProductosValor
            // 
            lblProductosValor.AutoSize = true;
            lblProductosValor.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblProductosValor.ForeColor = Color.FromArgb(45, 49, 54);
            lblProductosValor.Location = new Point(14, 31);
            lblProductosValor.Name = "lblProductosValor";
            lblProductosValor.Size = new Size(44, 52);
            lblProductosValor.TabIndex = 1;
            lblProductosValor.Text = "0";
            // 
            // lblProductosDescripcion
            // 
            lblProductosDescripcion.AutoSize = true;
            lblProductosDescripcion.Font = new Font("Segoe UI", 8F);
            lblProductosDescripcion.ForeColor = Color.FromArgb(115, 120, 125);
            lblProductosDescripcion.Location = new Point(17, 82);
            lblProductosDescripcion.Name = "lblProductosDescripcion";
            lblProductosDescripcion.Size = new Size(130, 19);
            lblProductosDescripcion.TabIndex = 2;
            lblProductosDescripcion.Text = "Productos vendidos";
            // 
            // pnlTarjetaStock
            // 
            pnlTarjetaStock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlTarjetaStock.BackColor = Color.White;
            pnlTarjetaStock.BorderStyle = BorderStyle.FixedSingle;
            pnlTarjetaStock.Controls.Add(lblStockBajoTitulo);
            pnlTarjetaStock.Controls.Add(lblStockBajoValor);
            pnlTarjetaStock.Controls.Add(lblStockBajoDescripcion);
            pnlTarjetaStock.Location = new Point(887, 225);
            pnlTarjetaStock.Name = "pnlTarjetaStock";
            pnlTarjetaStock.Size = new Size(261, 110);
            pnlTarjetaStock.TabIndex = 5;
            // 
            // lblStockBajoTitulo
            // 
            lblStockBajoTitulo.AutoSize = true;
            lblStockBajoTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStockBajoTitulo.ForeColor = Color.FromArgb(80, 85, 90);
            lblStockBajoTitulo.Location = new Point(16, 13);
            lblStockBajoTitulo.Name = "lblStockBajoTitulo";
            lblStockBajoTitulo.Size = new Size(81, 20);
            lblStockBajoTitulo.TabIndex = 0;
            lblStockBajoTitulo.Text = "Stock bajo";
            // 
            // lblStockBajoValor
            // 
            lblStockBajoValor.AutoSize = true;
            lblStockBajoValor.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblStockBajoValor.ForeColor = Color.FromArgb(175, 65, 65);
            lblStockBajoValor.Location = new Point(14, 31);
            lblStockBajoValor.Name = "lblStockBajoValor";
            lblStockBajoValor.Size = new Size(44, 52);
            lblStockBajoValor.TabIndex = 1;
            lblStockBajoValor.Text = "0";
            // 
            // lblStockBajoDescripcion
            // 
            lblStockBajoDescripcion.AutoSize = true;
            lblStockBajoDescripcion.Font = new Font("Segoe UI", 8F);
            lblStockBajoDescripcion.ForeColor = Color.FromArgb(115, 120, 125);
            lblStockBajoDescripcion.Location = new Point(17, 82);
            lblStockBajoDescripcion.Name = "lblStockBajoDescripcion";
            lblStockBajoDescripcion.Size = new Size(216, 19);
            lblStockBajoDescripcion.TabIndex = 2;
            lblStockBajoDescripcion.Text = "Productos que requieren atención";
            // 
            // pnlGrafico
            // 
            pnlGrafico.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlGrafico.BackColor = Color.White;
            pnlGrafico.BorderStyle = BorderStyle.FixedSingle;
            pnlGrafico.Controls.Add(lblGraficoTitulo);
            pnlGrafico.Controls.Add(lblPeriodoGrafico);
            pnlGrafico.Controls.Add(pnlGraficoContenido);
            pnlGrafico.Location = new Point(32, 349);
            pnlGrafico.Name = "pnlGrafico";
            pnlGrafico.Size = new Size(1116, 180);
            pnlGrafico.TabIndex = 6;
            // 
            // lblGraficoTitulo
            // 
            lblGraficoTitulo.AutoSize = true;
            lblGraficoTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblGraficoTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblGraficoTitulo.Location = new Point(18, 13);
            lblGraficoTitulo.Name = "lblGraficoTitulo";
            lblGraficoTitulo.Size = new Size(171, 25);
            lblGraficoTitulo.TabIndex = 0;
            lblGraficoTitulo.Text = "Ventas del período";
            // 
            // lblPeriodoGrafico
            // 
            lblPeriodoGrafico.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPeriodoGrafico.Font = new Font("Segoe UI", 8.5F);
            lblPeriodoGrafico.ForeColor = Color.FromArgb(105, 110, 116);
            lblPeriodoGrafico.Location = new Point(800, 16);
            lblPeriodoGrafico.Name = "lblPeriodoGrafico";
            lblPeriodoGrafico.Size = new Size(290, 20);
            lblPeriodoGrafico.TabIndex = 1;
            lblPeriodoGrafico.Text = "Últimos 30 días";
            lblPeriodoGrafico.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlGraficoContenido
            // 
            pnlGraficoContenido.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlGraficoContenido.BackColor = Color.FromArgb(248, 249, 250);
            pnlGraficoContenido.Controls.Add(lblGraficoPlaceholder);
            pnlGraficoContenido.Location = new Point(18, 47);
            pnlGraficoContenido.Name = "pnlGraficoContenido";
            pnlGraficoContenido.Size = new Size(1074, 112);
            pnlGraficoContenido.TabIndex = 2;
            // 
            // lblGraficoPlaceholder
            // 
            lblGraficoPlaceholder.Dock = DockStyle.Fill;
            lblGraficoPlaceholder.Font = new Font("Segoe UI", 9F);
            lblGraficoPlaceholder.ForeColor = Color.FromArgb(130, 135, 140);
            lblGraficoPlaceholder.Location = new Point(0, 0);
            lblGraficoPlaceholder.Name = "lblGraficoPlaceholder";
            lblGraficoPlaceholder.Size = new Size(1074, 112);
            lblGraficoPlaceholder.TabIndex = 0;
            lblGraficoPlaceholder.Text = "El gráfico de ventas se mostrará aquí al conectar la capa lógica.";
            lblGraficoPlaceholder.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlProductosVendidos
            // 
            pnlProductosVendidos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlProductosVendidos.BackColor = Color.White;
            pnlProductosVendidos.BorderStyle = BorderStyle.FixedSingle;
            pnlProductosVendidos.Controls.Add(lblProductosVendidosTitulo);
            pnlProductosVendidos.Controls.Add(lblProductosVendidosDescripcion);
            pnlProductosVendidos.Controls.Add(dgvProductosVendidos);
            pnlProductosVendidos.Location = new Point(32, 543);
            pnlProductosVendidos.Name = "pnlProductosVendidos";
            pnlProductosVendidos.Size = new Size(1116, 185);
            pnlProductosVendidos.TabIndex = 7;
            // 
            // lblProductosVendidosTitulo
            // 
            lblProductosVendidosTitulo.AutoSize = true;
            lblProductosVendidosTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblProductosVendidosTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblProductosVendidosTitulo.Location = new Point(18, 12);
            lblProductosVendidosTitulo.Name = "lblProductosVendidosTitulo";
            lblProductosVendidosTitulo.Size = new Size(219, 25);
            lblProductosVendidosTitulo.TabIndex = 0;
            lblProductosVendidosTitulo.Text = "Productos más vendidos";
            // 
            // lblProductosVendidosDescripcion
            // 
            lblProductosVendidosDescripcion.AutoSize = true;
            lblProductosVendidosDescripcion.Font = new Font("Segoe UI", 8F);
            lblProductosVendidosDescripcion.ForeColor = Color.FromArgb(105, 110, 116);
            lblProductosVendidosDescripcion.Location = new Point(195, 17);
            lblProductosVendidosDescripcion.Name = "lblProductosVendidosDescripcion";
            lblProductosVendidosDescripcion.Size = new Size(310, 19);
            lblProductosVendidosDescripcion.TabIndex = 1;
            lblProductosVendidosDescripcion.Text = "Ranking correspondiente al período seleccionado.";
            // 
            // dgvProductosVendidos
            // 
            dgvProductosVendidos.AllowUserToAddRows = false;
            dgvProductosVendidos.AllowUserToDeleteRows = false;
            dgvProductosVendidos.AllowUserToResizeRows = false;
            dgvProductosVendidos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProductosVendidos.BackgroundColor = Color.White;
            dgvProductosVendidos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProductosVendidos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvProductosVendidos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvProductosVendidos.ColumnHeadersHeight = 38;
            dgvProductosVendidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(55, 59, 64);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(226, 229, 232);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(40, 44, 48);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvProductosVendidos.DefaultCellStyle = dataGridViewCellStyle2;
            dgvProductosVendidos.EnableHeadersVisualStyles = false;
            dgvProductosVendidos.GridColor = Color.FromArgb(224, 227, 230);
            dgvProductosVendidos.Location = new Point(18, 47);
            dgvProductosVendidos.MultiSelect = false;
            dgvProductosVendidos.Name = "dgvProductosVendidos";
            dgvProductosVendidos.ReadOnly = true;
            dgvProductosVendidos.RowHeadersVisible = false;
            dgvProductosVendidos.RowHeadersWidth = 51;
            dgvProductosVendidos.RowTemplate.Height = 36;
            dgvProductosVendidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductosVendidos.Size = new Size(1074, 116);
            dgvProductosVendidos.TabIndex = 2;
            // 
            // FormReportesGeneral
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1180, 760);
            Controls.Add(pnlCabecera);
            Controls.Add(pnlFiltros);
            Controls.Add(pnlTarjetaVentas);
            Controls.Add(pnlTarjetaIngresos);
            Controls.Add(pnlTarjetaProductos);
            Controls.Add(pnlTarjetaStock);
            Controls.Add(pnlGrafico);
            Controls.Add(pnlProductosVendidos);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormReportesGeneral";
            Text = "Reportes";
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            pnlTarjetaVentas.ResumeLayout(false);
            pnlTarjetaVentas.PerformLayout();
            pnlTarjetaIngresos.ResumeLayout(false);
            pnlTarjetaIngresos.PerformLayout();
            pnlTarjetaProductos.ResumeLayout(false);
            pnlTarjetaProductos.PerformLayout();
            pnlTarjetaStock.ResumeLayout(false);
            pnlTarjetaStock.PerformLayout();
            pnlGrafico.ResumeLayout(false);
            pnlGrafico.PerformLayout();
            pnlGraficoContenido.ResumeLayout(false);
            pnlProductosVendidos.ResumeLayout(false);
            pnlProductosVendidos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductosVendidos).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}