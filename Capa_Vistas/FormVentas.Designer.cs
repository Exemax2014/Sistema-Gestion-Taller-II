using System.Drawing;
using System.Windows.Forms;

namespace Capa_Vistas
{
    partial class FormVentas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlSuperior = new Panel();
            tblEncabezado = new TableLayoutPanel();
            pnlTituloModulo = new Panel();
            lblIconoVentas = new Label();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlFecha = new Panel();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            pnlComprobante = new Panel();
            lblTipoFactura = new Label();
            cmbTipoFactura = new ComboBox();
            pnlSubMenu = new Panel();
            btnNuevaVenta = new Button();
            btnHistorialVentas = new Button();
            tblContenido = new TableLayoutPanel();
            pnlDatosVenta = new Panel();
            tblDatosVenta = new TableLayoutPanel();
            lblVendedor = new Label();
            txtVendedorNombre = new TextBox();
            lblCliente = new Label();
            txtClienteBuscar = new TextBox();
            btnBuscarCliente = new Button();
            cmbClienteResultados = new ComboBox();
            lblProductoBuscar = new Label();
            txtProductoBuscar = new TextBox();
            btnBuscarProducto = new Button();
            cmbProductoResultados = new ComboBox();
            lblDatosVenta = new Label();
            pnlDetalleProducto = new Panel();
            tblDetalleProducto = new TableLayoutPanel();
            lblCodigo = new Label();
            txtCodigoProducto = new TextBox();
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            lblStock = new Label();
            txtStock = new TextBox();
            lblPrecioVenta = new Label();
            txtPrecioVenta = new TextBox();
            lblCantidad = new Label();
            nudCantidad = new NumericUpDown();
            btnAgregar = new Button();
            lblDetalleProducto = new Label();
            pnlProductos = new Panel();
            dgvItems = new DataGridView();
            colQuitar = new DataGridViewButtonColumn();
            lblProductosAgregados = new Label();
            pnlPagos = new Panel();
            tblPagos = new TableLayoutPanel();
            pnlFormaPago = new Panel();
            tblFormaPago = new TableLayoutPanel();
            lblMetodoPago = new Label();
            cmbMetodoPago = new ComboBox();
            lblMontoPago = new Label();
            txtMontoPago = new TextBox();
            btnAgregarPago = new Button();
            lblFormaPago = new Label();
            pnlListaPagos = new Panel();
            dgvPagos = new DataGridView();
            colPagoNumero = new DataGridViewTextBoxColumn();
            colMetodoPago = new DataGridViewTextBoxColumn();
            colMontoPago = new DataGridViewTextBoxColumn();
            colEliminarPago = new DataGridViewButtonColumn();
            lblPagosVenta = new Label();
            pnlCancelar = new Panel();
            btnCancelar = new Button();
            pnlResumen = new Panel();
            tblResumen = new TableLayoutPanel();
            lblTotalVenta = new Label();
            txtPrecioTotal = new TextBox();
            lblTotalPagado = new Label();
            lblTotalPagadoValor = new Label();
            lblSaldoPendiente = new Label();
            lblSaldoPendienteValor = new Label();
            btnGuardar = new Button();
            txtVendedorId = new TextBox();
            pnlSuperior.SuspendLayout();
            tblEncabezado.SuspendLayout();
            pnlTituloModulo.SuspendLayout();
            pnlFecha.SuspendLayout();
            pnlComprobante.SuspendLayout();
            pnlSubMenu.SuspendLayout();
            tblContenido.SuspendLayout();
            pnlDatosVenta.SuspendLayout();
            tblDatosVenta.SuspendLayout();
            pnlDetalleProducto.SuspendLayout();
            tblDetalleProducto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).BeginInit();
            pnlProductos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            pnlPagos.SuspendLayout();
            tblPagos.SuspendLayout();
            pnlFormaPago.SuspendLayout();
            tblFormaPago.SuspendLayout();
            pnlListaPagos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
            pnlCancelar.SuspendLayout();
            pnlResumen.SuspendLayout();
            tblResumen.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSuperior
            // 
            pnlSuperior.BackColor = Color.FromArgb(246, 248, 250);
            pnlSuperior.Controls.Add(tblEncabezado);
            pnlSuperior.Controls.Add(pnlSubMenu);
            pnlSuperior.Dock = DockStyle.Top;
            pnlSuperior.Location = new Point(0, 0);
            pnlSuperior.Name = "pnlSuperior";
            pnlSuperior.Padding = new Padding(20, 10, 20, 0);
            pnlSuperior.Size = new Size(1350, 155);
            pnlSuperior.TabIndex = 1;
            // 
            // tblEncabezado
            // 
            tblEncabezado.ColumnCount = 3;
            tblEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tblEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblEncabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblEncabezado.Controls.Add(pnlTituloModulo, 0, 0);
            tblEncabezado.Controls.Add(pnlFecha, 1, 0);
            tblEncabezado.Controls.Add(pnlComprobante, 2, 0);
            tblEncabezado.Dock = DockStyle.Fill;
            tblEncabezado.Location = new Point(20, 10);
            tblEncabezado.Name = "tblEncabezado";
            tblEncabezado.RowCount = 1;
            tblEncabezado.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblEncabezado.Size = new Size(1310, 87);
            tblEncabezado.TabIndex = 0;
            // 
            // pnlTituloModulo
            // 
            pnlTituloModulo.BackColor = Color.FromArgb(246, 248, 250);
            pnlTituloModulo.Controls.Add(lblIconoVentas);
            pnlTituloModulo.Controls.Add(lblTitulo);
            pnlTituloModulo.Controls.Add(lblSubtitulo);
            pnlTituloModulo.Dock = DockStyle.Fill;
            pnlTituloModulo.Location = new Point(3, 3);
            pnlTituloModulo.Name = "pnlTituloModulo";
            pnlTituloModulo.Size = new Size(780, 81);
            pnlTituloModulo.TabIndex = 0;
            // 
            // lblIconoVentas
            // 
            lblIconoVentas.Font = new Font("Segoe UI Symbol", 28F);
            lblIconoVentas.ForeColor = Color.FromArgb(22, 43, 64);
            lblIconoVentas.Location = new Point(5, 8);
            lblIconoVentas.Name = "lblIconoVentas";
            lblIconoVentas.Size = new Size(65, 65);
            lblIconoVentas.TabIndex = 0;
            lblIconoVentas.Text = "\U0001f6d2";
            lblIconoVentas.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 21F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 35, 46);
            lblTitulo.Location = new Point(75, 7);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(129, 47);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Ventas";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 10F);
            lblSubtitulo.ForeColor = Color.FromArgb(85, 98, 112);
            lblSubtitulo.Location = new Point(78, 52);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(249, 23);
            lblSubtitulo.TabIndex = 2;
            lblSubtitulo.Text = "Gestión de ventas y facturación";
            // 
            // pnlFecha
            // 
            pnlFecha.BackColor = Color.White;
            pnlFecha.BorderStyle = BorderStyle.FixedSingle;
            pnlFecha.Controls.Add(lblFecha);
            pnlFecha.Controls.Add(dtpFecha);
            pnlFecha.Dock = DockStyle.Fill;
            pnlFecha.Location = new Point(792, 6);
            pnlFecha.Margin = new Padding(6);
            pnlFecha.Name = "pnlFecha";
            pnlFecha.Size = new Size(250, 75);
            pnlFecha.TabIndex = 1;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Font = new Font("Segoe UI", 8.5F);
            lblFecha.ForeColor = Color.FromArgb(70, 84, 98);
            lblFecha.Location = new Point(14, 11);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(108, 20);
            lblFecha.TabIndex = 0;
            lblFecha.Text = "Fecha de venta";
            // 
            // dtpFecha
            // 
            dtpFecha.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(14, 36);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(248, 27);
            dtpFecha.TabIndex = 1;
            // 
            // pnlComprobante
            // 
            pnlComprobante.BackColor = Color.White;
            pnlComprobante.BorderStyle = BorderStyle.FixedSingle;
            pnlComprobante.Controls.Add(lblTipoFactura);
            pnlComprobante.Controls.Add(cmbTipoFactura);
            pnlComprobante.Dock = DockStyle.Fill;
            pnlComprobante.Location = new Point(1054, 6);
            pnlComprobante.Margin = new Padding(6);
            pnlComprobante.Name = "pnlComprobante";
            pnlComprobante.Size = new Size(250, 75);
            pnlComprobante.TabIndex = 2;
            // 
            // lblTipoFactura
            // 
            lblTipoFactura.AutoSize = true;
            lblTipoFactura.Font = new Font("Segoe UI", 8.5F);
            lblTipoFactura.ForeColor = Color.FromArgb(70, 84, 98);
            lblTipoFactura.Location = new Point(14, 11);
            lblTipoFactura.Name = "lblTipoFactura";
            lblTipoFactura.Size = new Size(154, 20);
            lblTipoFactura.TabIndex = 0;
            lblTipoFactura.Text = "Tipo de comprobante";
            // 
            // cmbTipoFactura
            // 
            cmbTipoFactura.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbTipoFactura.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoFactura.Items.AddRange(new object[] { "Factura" });
            cmbTipoFactura.Location = new Point(14, 36);
            cmbTipoFactura.Name = "cmbTipoFactura";
            cmbTipoFactura.Size = new Size(248, 28);
            cmbTipoFactura.TabIndex = 1;
            // 
            // pnlSubMenu
            // 
            pnlSubMenu.BackColor = Color.FromArgb(246, 248, 250);
            pnlSubMenu.Controls.Add(btnNuevaVenta);
            pnlSubMenu.Controls.Add(btnHistorialVentas);
            pnlSubMenu.Dock = DockStyle.Bottom;
            pnlSubMenu.Location = new Point(20, 97);
            pnlSubMenu.Name = "pnlSubMenu";
            pnlSubMenu.Size = new Size(1310, 58);
            pnlSubMenu.TabIndex = 1;
            // 
            // btnNuevaVenta
            // 
            btnNuevaVenta.BackColor = Color.FromArgb(37, 116, 222);
            btnNuevaVenta.Cursor = Cursors.Hand;
            btnNuevaVenta.FlatAppearance.BorderSize = 0;
            btnNuevaVenta.FlatStyle = FlatStyle.Flat;
            btnNuevaVenta.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNuevaVenta.ForeColor = Color.White;
            btnNuevaVenta.Location = new Point(0, 7);
            btnNuevaVenta.Name = "btnNuevaVenta";
            btnNuevaVenta.Size = new Size(210, 44);
            btnNuevaVenta.TabIndex = 0;
            btnNuevaVenta.Text = "▣   Nueva venta";
            btnNuevaVenta.UseVisualStyleBackColor = false;
            // 
            // btnHistorialVentas
            // 
            btnHistorialVentas.BackColor = Color.FromArgb(236, 240, 244);
            btnHistorialVentas.Cursor = Cursors.Hand;
            btnHistorialVentas.FlatAppearance.BorderColor = Color.FromArgb(210, 218, 226);
            btnHistorialVentas.FlatStyle = FlatStyle.Flat;
            btnHistorialVentas.Font = new Font("Segoe UI", 10F);
            btnHistorialVentas.ForeColor = Color.FromArgb(38, 52, 67);
            btnHistorialVentas.Location = new Point(210, 7);
            btnHistorialVentas.Name = "btnHistorialVentas";
            btnHistorialVentas.Size = new Size(230, 44);
            btnHistorialVentas.TabIndex = 1;
            btnHistorialVentas.Text = "☷   Historial de ventas";
            btnHistorialVentas.UseVisualStyleBackColor = false;
            // 
            // tblContenido
            // 
            tblContenido.BackColor = Color.FromArgb(246, 248, 250);
            tblContenido.ColumnCount = 2;
            tblContenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));
            tblContenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
            tblContenido.Controls.Add(pnlDatosVenta, 0, 0);
            tblContenido.Controls.Add(pnlDetalleProducto, 1, 0);
            tblContenido.Controls.Add(pnlProductos, 0, 1);
            tblContenido.Controls.Add(pnlPagos, 1, 1);
            tblContenido.Controls.Add(pnlCancelar, 0, 2);
            tblContenido.Controls.Add(pnlResumen, 1, 2);
            tblContenido.Dock = DockStyle.Fill;
            tblContenido.Location = new Point(0, 155);
            tblContenido.Name = "tblContenido";
            tblContenido.Padding = new Padding(20, 0, 20, 15);
            tblContenido.RowCount = 3;
            tblContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 245F));
            tblContenido.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 115F));
            tblContenido.Size = new Size(1350, 605);
            tblContenido.TabIndex = 0;
            // 
            // pnlDatosVenta
            // 
            pnlDatosVenta.BackColor = Color.White;
            pnlDatosVenta.BorderStyle = BorderStyle.FixedSingle;
            pnlDatosVenta.Controls.Add(tblDatosVenta);
            pnlDatosVenta.Controls.Add(lblDatosVenta);
            pnlDatosVenta.Dock = DockStyle.Fill;
            pnlDatosVenta.Location = new Point(20, 0);
            pnlDatosVenta.Margin = new Padding(0, 0, 8, 10);
            pnlDatosVenta.Name = "pnlDatosVenta";
            pnlDatosVenta.Size = new Size(725, 235);
            pnlDatosVenta.TabIndex = 0;
            // 
            // tblDatosVenta
            // 
            tblDatosVenta.ColumnCount = 3;
            tblDatosVenta.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            tblDatosVenta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblDatosVenta.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            tblDatosVenta.Controls.Add(lblVendedor, 0, 0);
            tblDatosVenta.Controls.Add(txtVendedorNombre, 1, 0);
            tblDatosVenta.Controls.Add(lblCliente, 0, 1);
            tblDatosVenta.Controls.Add(txtClienteBuscar, 1, 1);
            tblDatosVenta.Controls.Add(btnBuscarCliente, 2, 1);
            tblDatosVenta.Controls.Add(cmbClienteResultados, 1, 2);
            tblDatosVenta.Controls.Add(lblProductoBuscar, 0, 3);
            tblDatosVenta.Controls.Add(txtProductoBuscar, 1, 3);
            tblDatosVenta.Controls.Add(btnBuscarProducto, 2, 3);
            tblDatosVenta.Controls.Add(cmbProductoResultados, 1, 4);
            tblDatosVenta.Dock = DockStyle.Fill;
            tblDatosVenta.Location = new Point(0, 45);
            tblDatosVenta.Name = "tblDatosVenta";
            tblDatosVenta.Padding = new Padding(15, 8, 15, 10);
            tblDatosVenta.RowCount = 5;
            tblDatosVenta.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblDatosVenta.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblDatosVenta.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblDatosVenta.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblDatosVenta.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblDatosVenta.Size = new Size(723, 188);
            tblDatosVenta.TabIndex = 0;
            // 
            // lblVendedor
            // 
            lblVendedor.Dock = DockStyle.Fill;
            lblVendedor.Location = new Point(18, 8);
            lblVendedor.Name = "lblVendedor";
            lblVendedor.Size = new Size(89, 34);
            lblVendedor.TabIndex = 0;
            lblVendedor.Text = "Vendedor";
            lblVendedor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtVendedorNombre
            // 
            txtVendedorNombre.BackColor = Color.FromArgb(244, 247, 250);
            tblDatosVenta.SetColumnSpan(txtVendedorNombre, 2);
            txtVendedorNombre.Dock = DockStyle.Fill;
            txtVendedorNombre.Location = new Point(114, 12);
            txtVendedorNombre.Margin = new Padding(4);
            txtVendedorNombre.Name = "txtVendedorNombre";
            txtVendedorNombre.ReadOnly = true;
            txtVendedorNombre.Size = new Size(590, 27);
            txtVendedorNombre.TabIndex = 1;
            // 
            // lblCliente
            // 
            lblCliente.Dock = DockStyle.Fill;
            lblCliente.Location = new Point(18, 42);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(89, 34);
            lblCliente.TabIndex = 2;
            lblCliente.Text = "Cliente";
            lblCliente.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtClienteBuscar
            // 
            txtClienteBuscar.Dock = DockStyle.Fill;
            txtClienteBuscar.Location = new Point(114, 46);
            txtClienteBuscar.Margin = new Padding(4);
            txtClienteBuscar.Name = "txtClienteBuscar";
            txtClienteBuscar.PlaceholderText = "Buscar cliente por nombre o documento...";
            txtClienteBuscar.Size = new Size(495, 27);
            txtClienteBuscar.TabIndex = 3;
            // 
            // btnBuscarCliente
            // 
            btnBuscarCliente.BackColor = Color.FromArgb(37, 116, 222);
            btnBuscarCliente.Cursor = Cursors.Hand;
            btnBuscarCliente.Dock = DockStyle.Fill;
            btnBuscarCliente.FlatAppearance.BorderSize = 0;
            btnBuscarCliente.FlatStyle = FlatStyle.Flat;
            btnBuscarCliente.ForeColor = Color.White;
            btnBuscarCliente.Location = new Point(617, 46);
            btnBuscarCliente.Margin = new Padding(4);
            btnBuscarCliente.Name = "btnBuscarCliente";
            btnBuscarCliente.Size = new Size(87, 26);
            btnBuscarCliente.TabIndex = 4;
            btnBuscarCliente.Text = "Buscar";
            btnBuscarCliente.UseVisualStyleBackColor = false;
            btnBuscarCliente.Click += BtnBuscarCliente_Click;
            // 
            // cmbClienteResultados
            // 
            tblDatosVenta.SetColumnSpan(cmbClienteResultados, 2);
            cmbClienteResultados.Dock = DockStyle.Fill;
            cmbClienteResultados.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClienteResultados.Location = new Point(114, 80);
            cmbClienteResultados.Margin = new Padding(4);
            cmbClienteResultados.Name = "cmbClienteResultados";
            cmbClienteResultados.Size = new Size(590, 28);
            cmbClienteResultados.TabIndex = 5;
            cmbClienteResultados.SelectedIndexChanged += CmbClienteResultados_SelectedIndexChanged;
            // 
            // lblProductoBuscar
            // 
            lblProductoBuscar.Dock = DockStyle.Fill;
            lblProductoBuscar.Location = new Point(18, 110);
            lblProductoBuscar.Name = "lblProductoBuscar";
            lblProductoBuscar.Size = new Size(89, 34);
            lblProductoBuscar.TabIndex = 6;
            lblProductoBuscar.Text = "Producto";
            lblProductoBuscar.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtProductoBuscar
            // 
            txtProductoBuscar.Dock = DockStyle.Fill;
            txtProductoBuscar.Location = new Point(114, 114);
            txtProductoBuscar.Margin = new Padding(4);
            txtProductoBuscar.Name = "txtProductoBuscar";
            txtProductoBuscar.PlaceholderText = "Buscar producto por nombre o código...";
            txtProductoBuscar.Size = new Size(495, 27);
            txtProductoBuscar.TabIndex = 7;
            // 
            // btnBuscarProducto
            // 
            btnBuscarProducto.BackColor = Color.FromArgb(37, 116, 222);
            btnBuscarProducto.Cursor = Cursors.Hand;
            btnBuscarProducto.Dock = DockStyle.Fill;
            btnBuscarProducto.FlatAppearance.BorderSize = 0;
            btnBuscarProducto.FlatStyle = FlatStyle.Flat;
            btnBuscarProducto.ForeColor = Color.White;
            btnBuscarProducto.Location = new Point(617, 114);
            btnBuscarProducto.Margin = new Padding(4);
            btnBuscarProducto.Name = "btnBuscarProducto";
            btnBuscarProducto.Size = new Size(87, 26);
            btnBuscarProducto.TabIndex = 8;
            btnBuscarProducto.Text = "Buscar";
            btnBuscarProducto.UseVisualStyleBackColor = false;
            btnBuscarProducto.Click += BtnBuscarProducto_Click;
            // 
            // cmbProductoResultados
            // 
            tblDatosVenta.SetColumnSpan(cmbProductoResultados, 2);
            cmbProductoResultados.Dock = DockStyle.Fill;
            cmbProductoResultados.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProductoResultados.Location = new Point(114, 148);
            cmbProductoResultados.Margin = new Padding(4);
            cmbProductoResultados.Name = "cmbProductoResultados";
            cmbProductoResultados.Size = new Size(590, 28);
            cmbProductoResultados.TabIndex = 9;
            cmbProductoResultados.SelectedIndexChanged += CmbProductoResultados_SelectedIndexChanged;
            // 
            // lblDatosVenta
            // 
            lblDatosVenta.Dock = DockStyle.Top;
            lblDatosVenta.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblDatosVenta.ForeColor = Color.FromArgb(27, 42, 58);
            lblDatosVenta.Location = new Point(0, 0);
            lblDatosVenta.Name = "lblDatosVenta";
            lblDatosVenta.Padding = new Padding(15, 12, 0, 0);
            lblDatosVenta.Size = new Size(723, 45);
            lblDatosVenta.TabIndex = 1;
            lblDatosVenta.Text = "●   Datos de la venta";
            // 
            // pnlDetalleProducto
            // 
            pnlDetalleProducto.BackColor = Color.White;
            pnlDetalleProducto.BorderStyle = BorderStyle.FixedSingle;
            pnlDetalleProducto.Controls.Add(tblDetalleProducto);
            pnlDetalleProducto.Controls.Add(lblDetalleProducto);
            pnlDetalleProducto.Dock = DockStyle.Fill;
            pnlDetalleProducto.Location = new Point(761, 0);
            pnlDetalleProducto.Margin = new Padding(8, 0, 0, 10);
            pnlDetalleProducto.Name = "pnlDetalleProducto";
            pnlDetalleProducto.Size = new Size(569, 235);
            pnlDetalleProducto.TabIndex = 1;
            // 
            // tblDetalleProducto
            // 
            tblDetalleProducto.ColumnCount = 2;
            tblDetalleProducto.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tblDetalleProducto.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblDetalleProducto.Controls.Add(lblCodigo, 0, 0);
            tblDetalleProducto.Controls.Add(txtCodigoProducto, 1, 0);
            tblDetalleProducto.Controls.Add(lblDescripcion, 0, 1);
            tblDetalleProducto.Controls.Add(txtDescripcion, 1, 1);
            tblDetalleProducto.Controls.Add(lblStock, 0, 2);
            tblDetalleProducto.Controls.Add(txtStock, 1, 2);
            tblDetalleProducto.Controls.Add(lblPrecioVenta, 0, 3);
            tblDetalleProducto.Controls.Add(txtPrecioVenta, 1, 3);
            tblDetalleProducto.Controls.Add(lblCantidad, 0, 4);
            tblDetalleProducto.Controls.Add(nudCantidad, 1, 4);
            tblDetalleProducto.Controls.Add(btnAgregar, 1, 5);
            tblDetalleProducto.Dock = DockStyle.Fill;
            tblDetalleProducto.Location = new Point(0, 45);
            tblDetalleProducto.Name = "tblDetalleProducto";
            tblDetalleProducto.Padding = new Padding(15, 5, 15, 10);
            tblDetalleProducto.RowCount = 6;
            tblDetalleProducto.RowStyles.Add(new RowStyle(SizeType.Percent, 16F));
            tblDetalleProducto.RowStyles.Add(new RowStyle(SizeType.Percent, 16F));
            tblDetalleProducto.RowStyles.Add(new RowStyle(SizeType.Percent, 16F));
            tblDetalleProducto.RowStyles.Add(new RowStyle(SizeType.Percent, 16F));
            tblDetalleProducto.RowStyles.Add(new RowStyle(SizeType.Percent, 16F));
            tblDetalleProducto.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblDetalleProducto.Size = new Size(567, 188);
            tblDetalleProducto.TabIndex = 0;
            // 
            // lblCodigo
            // 
            lblCodigo.Dock = DockStyle.Fill;
            lblCodigo.Location = new Point(18, 5);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(124, 27);
            lblCodigo.TabIndex = 0;
            lblCodigo.Text = "Código";
            lblCodigo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtCodigoProducto
            // 
            txtCodigoProducto.BackColor = Color.FromArgb(245, 247, 249);
            txtCodigoProducto.Dock = DockStyle.Fill;
            txtCodigoProducto.Location = new Point(149, 9);
            txtCodigoProducto.Margin = new Padding(4);
            txtCodigoProducto.Name = "txtCodigoProducto";
            txtCodigoProducto.ReadOnly = true;
            txtCodigoProducto.Size = new Size(399, 27);
            txtCodigoProducto.TabIndex = 1;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Dock = DockStyle.Fill;
            lblDescripcion.Location = new Point(18, 32);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(124, 27);
            lblDescripcion.TabIndex = 2;
            lblDescripcion.Text = "Descripción";
            lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDescripcion
            // 
            txtDescripcion.BackColor = Color.FromArgb(245, 247, 249);
            txtDescripcion.Dock = DockStyle.Fill;
            txtDescripcion.Location = new Point(149, 36);
            txtDescripcion.Margin = new Padding(4);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.ReadOnly = true;
            txtDescripcion.Size = new Size(399, 27);
            txtDescripcion.TabIndex = 3;
            // 
            // lblStock
            // 
            lblStock.Dock = DockStyle.Fill;
            lblStock.Location = new Point(18, 59);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(124, 27);
            lblStock.TabIndex = 4;
            lblStock.Text = "Stock disponible";
            lblStock.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtStock
            // 
            txtStock.BackColor = Color.FromArgb(245, 247, 249);
            txtStock.Dock = DockStyle.Fill;
            txtStock.Location = new Point(149, 63);
            txtStock.Margin = new Padding(4);
            txtStock.Name = "txtStock";
            txtStock.ReadOnly = true;
            txtStock.Size = new Size(399, 27);
            txtStock.TabIndex = 5;
            // 
            // lblPrecioVenta
            // 
            lblPrecioVenta.Dock = DockStyle.Fill;
            lblPrecioVenta.Location = new Point(18, 86);
            lblPrecioVenta.Name = "lblPrecioVenta";
            lblPrecioVenta.Size = new Size(124, 27);
            lblPrecioVenta.TabIndex = 6;
            lblPrecioVenta.Text = "Precio unitario";
            lblPrecioVenta.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPrecioVenta
            // 
            txtPrecioVenta.Dock = DockStyle.Fill;
            txtPrecioVenta.Location = new Point(149, 90);
            txtPrecioVenta.Margin = new Padding(4);
            txtPrecioVenta.Name = "txtPrecioVenta";
            txtPrecioVenta.Size = new Size(399, 27);
            txtPrecioVenta.TabIndex = 7;
            txtPrecioVenta.TextAlign = HorizontalAlignment.Right;
            // 
            // lblCantidad
            // 
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.Location = new Point(18, 113);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(124, 27);
            lblCantidad.TabIndex = 8;
            lblCantidad.Text = "Cantidad";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nudCantidad
            // 
            nudCantidad.Dock = DockStyle.Fill;
            nudCantidad.Location = new Point(149, 117);
            nudCantidad.Margin = new Padding(4);
            nudCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCantidad.Name = "nudCantidad";
            nudCantidad.Size = new Size(399, 27);
            nudCantidad.TabIndex = 9;
            nudCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAgregar
            // 
            btnAgregar.BackColor = Color.FromArgb(39, 151, 75);
            btnAgregar.Cursor = Cursors.Hand;
            btnAgregar.Dock = DockStyle.Right;
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.Location = new Point(363, 144);
            btnAgregar.Margin = new Padding(4);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(185, 30);
            btnAgregar.TabIndex = 10;
            btnAgregar.Text = "＋  Agregar a la venta";
            btnAgregar.UseVisualStyleBackColor = false;
            btnAgregar.Click += BtnAgregar_Click;
            // 
            // lblDetalleProducto
            // 
            lblDetalleProducto.Dock = DockStyle.Top;
            lblDetalleProducto.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblDetalleProducto.ForeColor = Color.FromArgb(27, 42, 58);
            lblDetalleProducto.Location = new Point(0, 0);
            lblDetalleProducto.Name = "lblDetalleProducto";
            lblDetalleProducto.Padding = new Padding(15, 12, 0, 0);
            lblDetalleProducto.Size = new Size(567, 45);
            lblDetalleProducto.TabIndex = 1;
            lblDetalleProducto.Text = "◇   Detalle del producto";
            // 
            // pnlProductos
            // 
            pnlProductos.BackColor = Color.White;
            pnlProductos.BorderStyle = BorderStyle.FixedSingle;
            pnlProductos.Controls.Add(dgvItems);
            pnlProductos.Controls.Add(lblProductosAgregados);
            pnlProductos.Dock = DockStyle.Fill;
            pnlProductos.Location = new Point(20, 245);
            pnlProductos.Margin = new Padding(0, 0, 8, 8);
            pnlProductos.Name = "pnlProductos";
            pnlProductos.Size = new Size(725, 222);
            pnlProductos.TabIndex = 2;
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.BackgroundColor = Color.White;
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(48, 65, 82);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvItems.ColumnHeadersHeight = 40;
            dgvItems.Columns.AddRange(new DataGridViewColumn[] { colQuitar });
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.Location = new Point(0, 46);
            dgvItems.MultiSelect = false;
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dgvItems.RowHeadersWidth = 51;
            dgvItems.RowTemplate.Height = 34;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(723, 174);
            dgvItems.TabIndex = 0;
            dgvItems.CellContentClick += DgvItems_CellContentClick;
            // 
            // colQuitar
            // 
            colQuitar.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colQuitar.FillWeight = 18F;
            colQuitar.HeaderText = "Acciones";
            colQuitar.MinimumWidth = 6;
            colQuitar.Name = "colQuitar";
            colQuitar.ReadOnly = true;
            colQuitar.Text = "Quitar";
            colQuitar.UseColumnTextForButtonValue = true;
            // 
            // lblProductosAgregados
            // 
            lblProductosAgregados.Dock = DockStyle.Top;
            lblProductosAgregados.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblProductosAgregados.ForeColor = Color.FromArgb(27, 42, 58);
            lblProductosAgregados.Location = new Point(0, 0);
            lblProductosAgregados.Name = "lblProductosAgregados";
            lblProductosAgregados.Padding = new Padding(15, 12, 0, 0);
            lblProductosAgregados.Size = new Size(723, 46);
            lblProductosAgregados.TabIndex = 1;
            lblProductosAgregados.Text = "☷   Productos agregados";
            // 
            // pnlPagos
            // 
            pnlPagos.BackColor = Color.FromArgb(246, 248, 250);
            pnlPagos.Controls.Add(tblPagos);
            pnlPagos.Dock = DockStyle.Fill;
            pnlPagos.Location = new Point(761, 245);
            pnlPagos.Margin = new Padding(8, 0, 0, 8);
            pnlPagos.Name = "pnlPagos";
            pnlPagos.Size = new Size(569, 222);
            pnlPagos.TabIndex = 3;
            // 
            // tblPagos
            // 
            tblPagos.ColumnCount = 1;
            tblPagos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblPagos.Controls.Add(pnlFormaPago, 0, 0);
            tblPagos.Controls.Add(pnlListaPagos, 0, 1);
            tblPagos.Dock = DockStyle.Fill;
            tblPagos.Location = new Point(0, 0);
            tblPagos.Name = "tblPagos";
            tblPagos.RowCount = 2;
            tblPagos.RowStyles.Add(new RowStyle(SizeType.Absolute, 105F));
            tblPagos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblPagos.Size = new Size(569, 222);
            tblPagos.TabIndex = 0;
            // 
            // pnlFormaPago
            // 
            pnlFormaPago.BackColor = Color.White;
            pnlFormaPago.BorderStyle = BorderStyle.FixedSingle;
            pnlFormaPago.Controls.Add(tblFormaPago);
            pnlFormaPago.Controls.Add(lblFormaPago);
            pnlFormaPago.Dock = DockStyle.Fill;
            pnlFormaPago.Location = new Point(0, 0);
            pnlFormaPago.Margin = new Padding(0, 0, 0, 8);
            pnlFormaPago.Name = "pnlFormaPago";
            pnlFormaPago.Size = new Size(569, 97);
            pnlFormaPago.TabIndex = 0;
            // 
            // tblFormaPago
            // 
            tblFormaPago.ColumnCount = 5;
            tblFormaPago.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 65F));
            tblFormaPago.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tblFormaPago.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55F));
            tblFormaPago.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tblFormaPago.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblFormaPago.Controls.Add(lblMetodoPago, 0, 0);
            tblFormaPago.Controls.Add(cmbMetodoPago, 1, 0);
            tblFormaPago.Controls.Add(lblMontoPago, 2, 0);
            tblFormaPago.Controls.Add(txtMontoPago, 3, 0);
            tblFormaPago.Controls.Add(btnAgregarPago, 4, 0);
            tblFormaPago.Dock = DockStyle.Fill;
            tblFormaPago.Location = new Point(0, 42);
            tblFormaPago.Name = "tblFormaPago";
            tblFormaPago.Padding = new Padding(12, 4, 12, 8);
            tblFormaPago.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblFormaPago.Size = new Size(567, 53);
            tblFormaPago.TabIndex = 0;
            // 
            // lblMetodoPago
            // 
            lblMetodoPago.Dock = DockStyle.Fill;
            lblMetodoPago.Location = new Point(15, 4);
            lblMetodoPago.Name = "lblMetodoPago";
            lblMetodoPago.Size = new Size(59, 41);
            lblMetodoPago.TabIndex = 0;
            lblMetodoPago.Text = "Método";
            lblMetodoPago.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbMetodoPago
            // 
            cmbMetodoPago.Dock = DockStyle.Fill;
            cmbMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMetodoPago.Items.AddRange(new object[] { "Efectivo", "Débito", "Crédito", "Transferencia" });
            cmbMetodoPago.Location = new Point(81, 8);
            cmbMetodoPago.Margin = new Padding(4);
            cmbMetodoPago.Name = "cmbMetodoPago";
            cmbMetodoPago.Size = new Size(182, 28);
            cmbMetodoPago.TabIndex = 1;
            // 
            // lblMontoPago
            // 
            lblMontoPago.Dock = DockStyle.Fill;
            lblMontoPago.Location = new Point(270, 4);
            lblMontoPago.Name = "lblMontoPago";
            lblMontoPago.Size = new Size(49, 41);
            lblMontoPago.TabIndex = 2;
            lblMontoPago.Text = "Monto";
            lblMontoPago.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMontoPago
            // 
            txtMontoPago.Dock = DockStyle.Fill;
            txtMontoPago.Location = new Point(326, 8);
            txtMontoPago.Margin = new Padding(4);
            txtMontoPago.Name = "txtMontoPago";
            txtMontoPago.Size = new Size(118, 27);
            txtMontoPago.TabIndex = 3;
            txtMontoPago.TextAlign = HorizontalAlignment.Right;
            // 
            // btnAgregarPago
            // 
            btnAgregarPago.BackColor = Color.FromArgb(37, 116, 222);
            btnAgregarPago.Cursor = Cursors.Hand;
            btnAgregarPago.Dock = DockStyle.Fill;
            btnAgregarPago.FlatAppearance.BorderSize = 0;
            btnAgregarPago.FlatStyle = FlatStyle.Flat;
            btnAgregarPago.ForeColor = Color.White;
            btnAgregarPago.Location = new Point(452, 8);
            btnAgregarPago.Margin = new Padding(4);
            btnAgregarPago.Name = "btnAgregarPago";
            btnAgregarPago.Size = new Size(99, 33);
            btnAgregarPago.TabIndex = 4;
            btnAgregarPago.Text = "＋ Agregar";
            btnAgregarPago.UseVisualStyleBackColor = false;
            // 
            // lblFormaPago
            // 
            lblFormaPago.Dock = DockStyle.Top;
            lblFormaPago.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFormaPago.ForeColor = Color.FromArgb(27, 42, 58);
            lblFormaPago.Location = new Point(0, 0);
            lblFormaPago.Name = "lblFormaPago";
            lblFormaPago.Padding = new Padding(15, 11, 0, 0);
            lblFormaPago.Size = new Size(567, 42);
            lblFormaPago.TabIndex = 1;
            lblFormaPago.Text = "▣   Forma de pago";
            // 
            // pnlListaPagos
            // 
            pnlListaPagos.BackColor = Color.White;
            pnlListaPagos.BorderStyle = BorderStyle.FixedSingle;
            pnlListaPagos.Controls.Add(dgvPagos);
            pnlListaPagos.Controls.Add(lblPagosVenta);
            pnlListaPagos.Dock = DockStyle.Fill;
            pnlListaPagos.Location = new Point(0, 105);
            pnlListaPagos.Margin = new Padding(0);
            pnlListaPagos.Name = "pnlListaPagos";
            pnlListaPagos.Size = new Size(569, 117);
            pnlListaPagos.TabIndex = 1;
            // 
            // dgvPagos
            // 
            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AllowUserToDeleteRows = false;
            dgvPagos.BackgroundColor = Color.White;
            dgvPagos.BorderStyle = BorderStyle.None;
            dgvPagos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPagos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(48, 65, 82);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvPagos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvPagos.ColumnHeadersHeight = 38;
            dgvPagos.Columns.AddRange(new DataGridViewColumn[] { colPagoNumero, colMetodoPago, colMontoPago, colEliminarPago });
            dgvPagos.Dock = DockStyle.Fill;
            dgvPagos.EnableHeadersVisualStyles = false;
            dgvPagos.Location = new Point(0, 42);
            dgvPagos.Name = "dgvPagos";
            dgvPagos.ReadOnly = true;
            dgvPagos.RowHeadersVisible = false;
            dgvPagos.RowHeadersWidth = 51;
            dgvPagos.Size = new Size(567, 73);
            dgvPagos.TabIndex = 0;
            // 
            // colPagoNumero
            // 
            colPagoNumero.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPagoNumero.FillWeight = 10F;
            colPagoNumero.HeaderText = "#";
            colPagoNumero.MinimumWidth = 6;
            colPagoNumero.Name = "colPagoNumero";
            colPagoNumero.ReadOnly = true;
            // 
            // colMetodoPago
            // 
            colMetodoPago.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colMetodoPago.FillWeight = 45F;
            colMetodoPago.HeaderText = "Método de pago";
            colMetodoPago.MinimumWidth = 6;
            colMetodoPago.Name = "colMetodoPago";
            colMetodoPago.ReadOnly = true;
            // 
            // colMontoPago
            // 
            colMontoPago.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colMontoPago.FillWeight = 25F;
            colMontoPago.HeaderText = "Monto";
            colMontoPago.MinimumWidth = 6;
            colMontoPago.Name = "colMontoPago";
            colMontoPago.ReadOnly = true;
            // 
            // colEliminarPago
            // 
            colEliminarPago.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colEliminarPago.FillWeight = 20F;
            colEliminarPago.HeaderText = "Acciones";
            colEliminarPago.MinimumWidth = 6;
            colEliminarPago.Name = "colEliminarPago";
            colEliminarPago.ReadOnly = true;
            colEliminarPago.Text = "Quitar";
            colEliminarPago.UseColumnTextForButtonValue = true;
            // 
            // lblPagosVenta
            // 
            lblPagosVenta.Dock = DockStyle.Top;
            lblPagosVenta.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPagosVenta.ForeColor = Color.FromArgb(27, 42, 58);
            lblPagosVenta.Location = new Point(0, 0);
            lblPagosVenta.Name = "lblPagosVenta";
            lblPagosVenta.Padding = new Padding(15, 11, 0, 0);
            lblPagosVenta.Size = new Size(567, 42);
            lblPagosVenta.TabIndex = 1;
            lblPagosVenta.Text = "☷   Pagos de la venta";
            // 
            // pnlCancelar
            // 
            pnlCancelar.BackColor = Color.FromArgb(246, 248, 250);
            pnlCancelar.Controls.Add(btnCancelar);
            pnlCancelar.Dock = DockStyle.Fill;
            pnlCancelar.Location = new Point(20, 475);
            pnlCancelar.Margin = new Padding(0, 0, 8, 0);
            pnlCancelar.Name = "pnlCancelar";
            pnlCancelar.Size = new Size(725, 115);
            pnlCancelar.TabIndex = 4;
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelar.BackColor = Color.FromArgb(255, 237, 237);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(225, 100, 100);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.FromArgb(150, 35, 35);
            btnCancelar.Location = new Point(0, 67);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(180, 42);
            btnCancelar.TabIndex = 0;
            btnCancelar.Text = "🗑   Cancelar venta";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += BtnCancelar_Click;
            // 
            // pnlResumen
            // 
            pnlResumen.BackColor = Color.White;
            pnlResumen.BorderStyle = BorderStyle.FixedSingle;
            pnlResumen.Controls.Add(tblResumen);
            pnlResumen.Dock = DockStyle.Fill;
            pnlResumen.Location = new Point(761, 475);
            pnlResumen.Margin = new Padding(8, 0, 0, 0);
            pnlResumen.Name = "pnlResumen";
            pnlResumen.Size = new Size(569, 115);
            pnlResumen.TabIndex = 5;
            // 
            // tblResumen
            // 
            tblResumen.ColumnCount = 3;
            tblResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            tblResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            tblResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            tblResumen.Controls.Add(lblTotalVenta, 0, 0);
            tblResumen.Controls.Add(txtPrecioTotal, 1, 0);
            tblResumen.Controls.Add(lblTotalPagado, 0, 1);
            tblResumen.Controls.Add(lblTotalPagadoValor, 1, 1);
            tblResumen.Controls.Add(lblSaldoPendiente, 0, 2);
            tblResumen.Controls.Add(lblSaldoPendienteValor, 1, 2);
            tblResumen.Controls.Add(btnGuardar, 2, 0);
            tblResumen.Dock = DockStyle.Fill;
            tblResumen.Location = new Point(0, 0);
            tblResumen.Name = "tblResumen";
            tblResumen.Padding = new Padding(15, 10, 15, 10);
            tblResumen.RowCount = 3;
            tblResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tblResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tblResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
            tblResumen.Size = new Size(567, 113);
            tblResumen.TabIndex = 0;
            // 
            // lblTotalVenta
            // 
            lblTotalVenta.Dock = DockStyle.Fill;
            lblTotalVenta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalVenta.Location = new Point(18, 10);
            lblTotalVenta.Name = "lblTotalVenta";
            lblTotalVenta.Size = new Size(198, 30);
            lblTotalVenta.TabIndex = 0;
            lblTotalVenta.Text = "Total de la venta:";
            lblTotalVenta.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPrecioTotal
            // 
            txtPrecioTotal.BorderStyle = BorderStyle.None;
            txtPrecioTotal.Dock = DockStyle.Fill;
            txtPrecioTotal.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            txtPrecioTotal.Location = new Point(222, 13);
            txtPrecioTotal.Name = "txtPrecioTotal";
            txtPrecioTotal.ReadOnly = true;
            txtPrecioTotal.Size = new Size(122, 22);
            txtPrecioTotal.TabIndex = 1;
            txtPrecioTotal.TextAlign = HorizontalAlignment.Right;
            // 
            // lblTotalPagado
            // 
            lblTotalPagado.Dock = DockStyle.Fill;
            lblTotalPagado.Location = new Point(18, 40);
            lblTotalPagado.Name = "lblTotalPagado";
            lblTotalPagado.Size = new Size(198, 30);
            lblTotalPagado.TabIndex = 2;
            lblTotalPagado.Text = "Total pagado:";
            lblTotalPagado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalPagadoValor
            // 
            lblTotalPagadoValor.Dock = DockStyle.Fill;
            lblTotalPagadoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalPagadoValor.Location = new Point(222, 40);
            lblTotalPagadoValor.Name = "lblTotalPagadoValor";
            lblTotalPagadoValor.Size = new Size(122, 30);
            lblTotalPagadoValor.TabIndex = 3;
            lblTotalPagadoValor.Text = "$ 0,00";
            lblTotalPagadoValor.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSaldoPendiente
            // 
            lblSaldoPendiente.Dock = DockStyle.Fill;
            lblSaldoPendiente.Location = new Point(18, 70);
            lblSaldoPendiente.Name = "lblSaldoPendiente";
            lblSaldoPendiente.Size = new Size(198, 33);
            lblSaldoPendiente.TabIndex = 4;
            lblSaldoPendiente.Text = "Saldo pendiente:";
            lblSaldoPendiente.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSaldoPendienteValor
            // 
            lblSaldoPendienteValor.Dock = DockStyle.Fill;
            lblSaldoPendienteValor.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSaldoPendienteValor.ForeColor = Color.FromArgb(26, 82, 220);
            lblSaldoPendienteValor.Location = new Point(222, 70);
            lblSaldoPendienteValor.Name = "lblSaldoPendienteValor";
            lblSaldoPendienteValor.Size = new Size(122, 33);
            lblSaldoPendienteValor.TabIndex = 5;
            lblSaldoPendienteValor.Text = "$ 0,00";
            lblSaldoPendienteValor.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(39, 151, 75);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.Dock = DockStyle.Fill;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(359, 15);
            btnGuardar.Margin = new Padding(12, 5, 0, 5);
            btnGuardar.Name = "btnGuardar";
            tblResumen.SetRowSpan(btnGuardar, 3);
            btnGuardar.Size = new Size(193, 83);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "✓  Confirmar venta";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // txtVendedorId
            // 
            txtVendedorId.Location = new Point(0, 0);
            txtVendedorId.Name = "txtVendedorId";
            txtVendedorId.Size = new Size(100, 27);
            txtVendedorId.TabIndex = 0;
            txtVendedorId.Visible = false;
            // 
            // FormVentas
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 248, 250);
            ClientSize = new Size(1350, 760);
            Controls.Add(tblContenido);
            Controls.Add(pnlSuperior);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(1000, 650);
            Name = "FormVentas";
            Text = "Ventas";
            pnlSuperior.ResumeLayout(false);
            tblEncabezado.ResumeLayout(false);
            pnlTituloModulo.ResumeLayout(false);
            pnlTituloModulo.PerformLayout();
            pnlFecha.ResumeLayout(false);
            pnlFecha.PerformLayout();
            pnlComprobante.ResumeLayout(false);
            pnlComprobante.PerformLayout();
            pnlSubMenu.ResumeLayout(false);
            tblContenido.ResumeLayout(false);
            pnlDatosVenta.ResumeLayout(false);
            tblDatosVenta.ResumeLayout(false);
            tblDatosVenta.PerformLayout();
            pnlDetalleProducto.ResumeLayout(false);
            tblDetalleProducto.ResumeLayout(false);
            tblDetalleProducto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).EndInit();
            pnlProductos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            pnlPagos.ResumeLayout(false);
            tblPagos.ResumeLayout(false);
            pnlFormaPago.ResumeLayout(false);
            tblFormaPago.ResumeLayout(false);
            tblFormaPago.PerformLayout();
            pnlListaPagos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
            pnlCancelar.ResumeLayout(false);
            pnlResumen.ResumeLayout(false);
            tblResumen.ResumeLayout(false);
            tblResumen.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // =========================================================
        // SUPERIOR
        // =========================================================

        private Panel pnlSuperior;
        private TableLayoutPanel tblEncabezado;

        private Panel pnlTituloModulo;
        private Label lblIconoVentas;
        private Label lblTitulo;
        private Label lblSubtitulo;

        private Panel pnlFecha;
        private Label lblFecha;
        private DateTimePicker dtpFecha;

        private Panel pnlComprobante;
        private Label lblTipoFactura;
        private ComboBox cmbTipoFactura;

        private Panel pnlSubMenu;
        private Button btnNuevaVenta;
        private Button btnHistorialVentas;

        // =========================================================
        // GENERAL
        // =========================================================

        private TableLayoutPanel tblContenido;

        // =========================================================
        // DATOS VENTA
        // =========================================================

        private Panel pnlDatosVenta;
        private Label lblDatosVenta;
        private TableLayoutPanel tblDatosVenta;

        private Label lblVendedor;
        private TextBox txtVendedorId;
        private TextBox txtVendedorNombre;

        private Label lblCliente;
        private TextBox txtClienteBuscar;
        private Button btnBuscarCliente;
        private ComboBox cmbClienteResultados;

        private Label lblProductoBuscar;
        private TextBox txtProductoBuscar;
        private Button btnBuscarProducto;
        private ComboBox cmbProductoResultados;

        // =========================================================
        // DETALLE PRODUCTO
        // =========================================================

        private Panel pnlDetalleProducto;
        private Label lblDetalleProducto;
        private TableLayoutPanel tblDetalleProducto;

        private Label lblCodigo;
        private TextBox txtCodigoProducto;

        private Label lblDescripcion;
        private TextBox txtDescripcion;

        private Label lblStock;
        private TextBox txtStock;

        private Label lblPrecioVenta;
        private TextBox txtPrecioVenta;

        private Label lblCantidad;
        private NumericUpDown nudCantidad;

        private Button btnAgregar;

        // =========================================================
        // PRODUCTOS AGREGADOS
        // =========================================================

        private Panel pnlProductos;
        private Label lblProductosAgregados;

        private DataGridView dgvItems;

        private DataGridViewTextBoxColumn colIdProducto;
        private DataGridViewTextBoxColumn colDescripcion;
        private DataGridViewTextBoxColumn colCantidad;
        private DataGridViewTextBoxColumn colPrecioUnitario;
        private DataGridViewTextBoxColumn colSubTotal;
        private DataGridViewButtonColumn colQuitar;

        // =========================================================
        // PAGOS
        // =========================================================

        private Panel pnlPagos;
        private TableLayoutPanel tblPagos;

        private Panel pnlFormaPago;
        private Label lblFormaPago;
        private TableLayoutPanel tblFormaPago;

        private Label lblMetodoPago;
        private ComboBox cmbMetodoPago;

        private Label lblMontoPago;
        private TextBox txtMontoPago;

        private Button btnAgregarPago;

        private Panel pnlListaPagos;
        private Label lblPagosVenta;

        private DataGridView dgvPagos;

        private DataGridViewTextBoxColumn colPagoNumero;
        private DataGridViewTextBoxColumn colMetodoPago;
        private DataGridViewTextBoxColumn colMontoPago;
        private DataGridViewButtonColumn colEliminarPago;

        // =========================================================
        // PARTE INFERIOR
        // =========================================================

        private Panel pnlCancelar;
        private Button btnCancelar;

        private Panel pnlResumen;
        private TableLayoutPanel tblResumen;

        private Label lblTotalVenta;
        private TextBox txtPrecioTotal;

        private Label lblTotalPagado;
        private Label lblTotalPagadoValor;

        private Label lblSaldoPendiente;
        private Label lblSaldoPendienteValor;

        private Button btnGuardar;
    }
}