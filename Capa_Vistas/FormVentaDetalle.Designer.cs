using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormVentaDetalle
    {
        private System.ComponentModel.IContainer components =
            null;


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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            pnlPrincipal = new Panel();
            pnlAcciones = new Panel();
            btnGenerarPdf = new Button();
            btnCerrar = new Button();
            pnlTotales = new Panel();
            lblSubtotalTitulo = new Label();
            lblSubtotal = new Label();
            lblDescuentoTitulo = new Label();
            lblDescuento = new Label();
            lblTotalTitulo = new Label();
            lblTotal = new Label();
            pnlPagos = new Panel();
            lblPagosTitulo = new Label();
            dgvPagos = new DataGridView();
            pnlProductos = new Panel();
            lblProductosTitulo = new Label();
            dgvProductos = new DataGridView();
            pnlDatosVenta = new Panel();
            lblFechaTitulo = new Label();
            lblFecha = new Label();
            lblSucursalTitulo = new Label();
            lblSucursal = new Label();
            lblTipoFacturaTitulo = new Label();
            lblTipoFactura = new Label();
            lblClienteTitulo = new Label();
            lblCliente = new Label();
            lblDocumentoTitulo = new Label();
            lblDocumento = new Label();
            lblVendedorTitulo = new Label();
            lblVendedor = new Label();
            pnlCabecera = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaTitulo = new Panel();
            pnlPrincipal.SuspendLayout();
            pnlAcciones.SuspendLayout();
            pnlTotales.SuspendLayout();
            pnlPagos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
            pnlProductos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            pnlDatosVenta.SuspendLayout();
            pnlCabecera.SuspendLayout();
            SuspendLayout();
            // 
            // pnlPrincipal
            // 
            pnlPrincipal.BackColor = Color.FromArgb(245, 246, 248);
            pnlPrincipal.Controls.Add(pnlAcciones);
            pnlPrincipal.Controls.Add(pnlTotales);
            pnlPrincipal.Controls.Add(pnlPagos);
            pnlPrincipal.Controls.Add(pnlProductos);
            pnlPrincipal.Controls.Add(pnlDatosVenta);
            pnlPrincipal.Controls.Add(pnlCabecera);
            pnlPrincipal.Dock = DockStyle.Fill;
            pnlPrincipal.Location = new Point(0, 0);
            pnlPrincipal.Name = "pnlPrincipal";
            pnlPrincipal.Padding = new Padding(32, 18, 32, 24);
            pnlPrincipal.Size = new Size(1180, 760);
            pnlPrincipal.TabIndex = 0;
            // 
            // pnlAcciones
            // 
            pnlAcciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlAcciones.Controls.Add(btnGenerarPdf);
            pnlAcciones.Controls.Add(btnCerrar);
            pnlAcciones.Location = new Point(32, 667);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(1116, 61);
            pnlAcciones.TabIndex = 5;
            // 
            // btnGenerarPdf
            // 
            btnGenerarPdf.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGenerarPdf.BackColor = Color.FromArgb(190, 137, 45);
            btnGenerarPdf.Cursor = Cursors.Hand;
            btnGenerarPdf.FlatAppearance.BorderSize = 0;
            btnGenerarPdf.FlatStyle = FlatStyle.Flat;
            btnGenerarPdf.ForeColor = Color.White;
            btnGenerarPdf.Location = new Point(753, 13);
            btnGenerarPdf.Name = "btnGenerarPdf";
            btnGenerarPdf.Size = new Size(140, 40);
            btnGenerarPdf.TabIndex = 0;
            btnGenerarPdf.Text = "Generar PDF";
            btnGenerarPdf.UseVisualStyleBackColor = false;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCerrar.BackColor = Color.FromArgb(45, 49, 54);
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(0, 13);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(190, 40);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "← Volver al historial";
            btnCerrar.UseVisualStyleBackColor = false;
            // 
            // pnlTotales
            // 
            pnlTotales.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pnlTotales.BackColor = Color.White;
            pnlTotales.BorderStyle = BorderStyle.FixedSingle;
            pnlTotales.Controls.Add(lblSubtotalTitulo);
            pnlTotales.Controls.Add(lblSubtotal);
            pnlTotales.Controls.Add(lblDescuentoTitulo);
            pnlTotales.Controls.Add(lblDescuento);
            pnlTotales.Controls.Add(lblTotalTitulo);
            pnlTotales.Controls.Add(lblTotal);
            pnlTotales.Location = new Point(766, 553);
            pnlTotales.Name = "pnlTotales";
            pnlTotales.Size = new Size(382, 108);
            pnlTotales.TabIndex = 4;
            // 
            // lblSubtotalTitulo
            // 
            lblSubtotalTitulo.AutoSize = true;
            lblSubtotalTitulo.Location = new Point(18, 12);
            lblSubtotalTitulo.Name = "lblSubtotalTitulo";
            lblSubtotalTitulo.Size = new Size(65, 20);
            lblSubtotalTitulo.TabIndex = 0;
            lblSubtotalTitulo.Text = "Subtotal";
            // 
            // lblSubtotal
            // 
            lblSubtotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSubtotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSubtotal.Location = new Point(205, 10);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(155, 23);
            lblSubtotal.TabIndex = 1;
            lblSubtotal.Text = "$0,00";
            lblSubtotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDescuentoTitulo
            // 
            lblDescuentoTitulo.AutoSize = true;
            lblDescuentoTitulo.Location = new Point(18, 41);
            lblDescuentoTitulo.Name = "lblDescuentoTitulo";
            lblDescuentoTitulo.Size = new Size(79, 20);
            lblDescuentoTitulo.TabIndex = 2;
            lblDescuentoTitulo.Text = "Descuento";
            // 
            // lblDescuento
            // 
            lblDescuento.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDescuento.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDescuento.Location = new Point(205, 39);
            lblDescuento.Name = "lblDescuento";
            lblDescuento.Size = new Size(155, 23);
            lblDescuento.TabIndex = 3;
            lblDescuento.Text = "$0,00";
            lblDescuento.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalTitulo
            // 
            lblTotalTitulo.AutoSize = true;
            lblTotalTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotalTitulo.ForeColor = Color.FromArgb(45, 49, 54);
            lblTotalTitulo.Location = new Point(18, 70);
            lblTotalTitulo.Name = "lblTotalTitulo";
            lblTotalTitulo.Size = new Size(68, 25);
            lblTotalTitulo.TabIndex = 4;
            lblTotalTitulo.Text = "TOTAL";
            // 
            // lblTotal
            // 
            lblTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTotal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(190, 137, 45);
            lblTotal.Location = new Point(185, 68);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(175, 28);
            lblTotal.TabIndex = 5;
            lblTotal.Text = "$0,00";
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlPagos
            // 
            pnlPagos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pnlPagos.BackColor = Color.White;
            pnlPagos.BorderStyle = BorderStyle.FixedSingle;
            pnlPagos.Controls.Add(lblPagosTitulo);
            pnlPagos.Controls.Add(dgvPagos);
            pnlPagos.Location = new Point(766, 214);
            pnlPagos.Name = "pnlPagos";
            pnlPagos.Size = new Size(382, 333);
            pnlPagos.TabIndex = 3;
            // 
            // lblPagosTitulo
            // 
            lblPagosTitulo.AutoSize = true;
            lblPagosTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblPagosTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblPagosTitulo.Location = new Point(18, 12);
            lblPagosTitulo.Name = "lblPagosTitulo";
            lblPagosTitulo.Size = new Size(147, 25);
            lblPagosTitulo.TabIndex = 0;
            lblPagosTitulo.Text = "Formas de pago";
            // 
            // dgvPagos
            // 
            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AllowUserToDeleteRows = false;
            dgvPagos.AllowUserToResizeRows = false;
            dgvPagos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPagos.BackgroundColor = Color.White;
            dgvPagos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPagos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvPagos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvPagos.ColumnHeadersHeight = 38;
            dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(55, 59, 64);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(226, 229, 232);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(40, 44, 48);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvPagos.DefaultCellStyle = dataGridViewCellStyle2;
            dgvPagos.EnableHeadersVisualStyles = false;
            dgvPagos.GridColor = Color.FromArgb(224, 227, 230);
            dgvPagos.Location = new Point(18, 47);
            dgvPagos.MultiSelect = false;
            dgvPagos.Name = "dgvPagos";
            dgvPagos.ReadOnly = true;
            dgvPagos.RowHeadersVisible = false;
            dgvPagos.RowHeadersWidth = 51;
            dgvPagos.RowTemplate.Height = 36;
            dgvPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPagos.Size = new Size(344, 267);
            dgvPagos.TabIndex = 1;
            // 
            // pnlProductos
            // 
            pnlProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlProductos.BackColor = Color.White;
            pnlProductos.BorderStyle = BorderStyle.FixedSingle;
            pnlProductos.Controls.Add(lblProductosTitulo);
            pnlProductos.Controls.Add(dgvProductos);
            pnlProductos.Location = new Point(32, 214);
            pnlProductos.Name = "pnlProductos";
            pnlProductos.Size = new Size(720, 447);
            pnlProductos.TabIndex = 2;
            // 
            // lblProductosTitulo
            // 
            lblProductosTitulo.AutoSize = true;
            lblProductosTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblProductosTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblProductosTitulo.Location = new Point(18, 12);
            lblProductosTitulo.Name = "lblProductosTitulo";
            lblProductosTitulo.Size = new Size(197, 25);
            lblProductosTitulo.TabIndex = 0;
            lblProductosTitulo.Text = "Productos de la venta";
            // 
            // dgvProductos
            // 
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.AllowUserToResizeRows = false;
            dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProductos.BackgroundColor = Color.White;
            dgvProductos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProductos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dgvProductos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvProductos.ColumnHeadersHeight = 38;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(55, 59, 64);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(226, 229, 232);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(40, 44, 48);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvProductos.DefaultCellStyle = dataGridViewCellStyle4;
            dgvProductos.EnableHeadersVisualStyles = false;
            dgvProductos.GridColor = Color.FromArgb(224, 227, 230);
            dgvProductos.Location = new Point(18, 47);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.RowHeadersWidth = 51;
            dgvProductos.RowTemplate.Height = 36;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(682, 381);
            dgvProductos.TabIndex = 1;
            // 
            // pnlDatosVenta
            // 
            pnlDatosVenta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlDatosVenta.BackColor = Color.White;
            pnlDatosVenta.BorderStyle = BorderStyle.FixedSingle;
            pnlDatosVenta.Controls.Add(lblFechaTitulo);
            pnlDatosVenta.Controls.Add(lblFecha);
            pnlDatosVenta.Controls.Add(lblSucursalTitulo);
            pnlDatosVenta.Controls.Add(lblSucursal);
            pnlDatosVenta.Controls.Add(lblTipoFacturaTitulo);
            pnlDatosVenta.Controls.Add(lblTipoFactura);
            pnlDatosVenta.Controls.Add(lblClienteTitulo);
            pnlDatosVenta.Controls.Add(lblCliente);
            pnlDatosVenta.Controls.Add(lblDocumentoTitulo);
            pnlDatosVenta.Controls.Add(lblDocumento);
            pnlDatosVenta.Controls.Add(lblVendedorTitulo);
            pnlDatosVenta.Controls.Add(lblVendedor);
            pnlDatosVenta.Location = new Point(32, 106);
            pnlDatosVenta.Name = "pnlDatosVenta";
            pnlDatosVenta.Size = new Size(1116, 102);
            pnlDatosVenta.TabIndex = 1;
            // 
            // lblFechaTitulo
            // 
            lblFechaTitulo.AutoSize = true;
            lblFechaTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblFechaTitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblFechaTitulo.Location = new Point(20, 16);
            lblFechaTitulo.Name = "lblFechaTitulo";
            lblFechaTitulo.Size = new Size(49, 20);
            lblFechaTitulo.TabIndex = 0;
            lblFechaTitulo.Text = "Fecha";
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Font = new Font("Segoe UI", 10F);
            lblFecha.ForeColor = Color.FromArgb(55, 59, 64);
            lblFecha.Location = new Point(20, 40);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(17, 23);
            lblFecha.TabIndex = 1;
            lblFecha.Text = "-";
            // 
            // lblSucursalTitulo
            // 
            lblSucursalTitulo.AutoSize = true;
            lblSucursalTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSucursalTitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSucursalTitulo.Location = new Point(208, 16);
            lblSucursalTitulo.Name = "lblSucursalTitulo";
            lblSucursalTitulo.Size = new Size(67, 20);
            lblSucursalTitulo.TabIndex = 2;
            lblSucursalTitulo.Text = "Sucursal";
            // 
            // lblSucursal
            // 
            lblSucursal.AutoSize = true;
            lblSucursal.Font = new Font("Segoe UI", 10F);
            lblSucursal.ForeColor = Color.FromArgb(55, 59, 64);
            lblSucursal.Location = new Point(208, 40);
            lblSucursal.Name = "lblSucursal";
            lblSucursal.Size = new Size(17, 23);
            lblSucursal.TabIndex = 3;
            lblSucursal.Text = "-";
            // 
            // lblTipoFacturaTitulo
            // 
            lblTipoFacturaTitulo.AutoSize = true;
            lblTipoFacturaTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblTipoFacturaTitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblTipoFacturaTitulo.Location = new Point(396, 16);
            lblTipoFacturaTitulo.Name = "lblTipoFacturaTitulo";
            lblTipoFacturaTitulo.Size = new Size(94, 20);
            lblTipoFacturaTitulo.TabIndex = 4;
            lblTipoFacturaTitulo.Text = "Tipo factura";
            // 
            // lblTipoFactura
            // 
            lblTipoFactura.AutoSize = true;
            lblTipoFactura.Font = new Font("Segoe UI", 10F);
            lblTipoFactura.ForeColor = Color.FromArgb(55, 59, 64);
            lblTipoFactura.Location = new Point(396, 40);
            lblTipoFactura.Name = "lblTipoFactura";
            lblTipoFactura.Size = new Size(17, 23);
            lblTipoFactura.TabIndex = 5;
            lblTipoFactura.Text = "-";
            // 
            // lblClienteTitulo
            // 
            lblClienteTitulo.AutoSize = true;
            lblClienteTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblClienteTitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblClienteTitulo.Location = new Point(584, 16);
            lblClienteTitulo.Name = "lblClienteTitulo";
            lblClienteTitulo.Size = new Size(57, 20);
            lblClienteTitulo.TabIndex = 6;
            lblClienteTitulo.Text = "Cliente";
            // 
            // lblCliente
            // 
            lblCliente.AutoSize = true;
            lblCliente.Font = new Font("Segoe UI", 10F);
            lblCliente.ForeColor = Color.FromArgb(55, 59, 64);
            lblCliente.Location = new Point(584, 40);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(17, 23);
            lblCliente.TabIndex = 7;
            lblCliente.Text = "-";
            // 
            // lblDocumentoTitulo
            // 
            lblDocumentoTitulo.AutoSize = true;
            lblDocumentoTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblDocumentoTitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblDocumentoTitulo.Location = new Point(772, 16);
            lblDocumentoTitulo.Name = "lblDocumentoTitulo";
            lblDocumentoTitulo.Size = new Size(91, 20);
            lblDocumentoTitulo.TabIndex = 8;
            lblDocumentoTitulo.Text = "Documento";
            // 
            // lblDocumento
            // 
            lblDocumento.AutoSize = true;
            lblDocumento.Font = new Font("Segoe UI", 10F);
            lblDocumento.ForeColor = Color.FromArgb(55, 59, 64);
            lblDocumento.Location = new Point(772, 40);
            lblDocumento.Name = "lblDocumento";
            lblDocumento.Size = new Size(17, 23);
            lblDocumento.TabIndex = 9;
            lblDocumento.Text = "-";
            // 
            // lblVendedorTitulo
            // 
            lblVendedorTitulo.AutoSize = true;
            lblVendedorTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblVendedorTitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblVendedorTitulo.Location = new Point(960, 16);
            lblVendedorTitulo.Name = "lblVendedorTitulo";
            lblVendedorTitulo.Size = new Size(76, 20);
            lblVendedorTitulo.TabIndex = 10;
            lblVendedorTitulo.Text = "Vendedor";
            // 
            // lblVendedor
            // 
            lblVendedor.AutoSize = true;
            lblVendedor.Font = new Font("Segoe UI", 10F);
            lblVendedor.ForeColor = Color.FromArgb(55, 59, 64);
            lblVendedor.Location = new Point(960, 40);
            lblVendedor.Name = "lblVendedor";
            lblVendedor.Size = new Size(17, 23);
            lblVendedor.TabIndex = 11;
            lblVendedor.Text = "-";
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
            lblTitulo.Size = new Size(304, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Detalle de venta";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(2, 48);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(352, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Consulta la información completa de la transacción.";
            // 
            // pnlLineaTitulo
            // 
            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(2, 74);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(95, 3);
            pnlLineaTitulo.TabIndex = 2;
            // 
            // FormVentaDetalle
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1180, 760);
            Controls.Add(pnlPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormVentaDetalle";
            Text = "Detalle de venta";
            pnlPrincipal.ResumeLayout(false);
            pnlAcciones.ResumeLayout(false);
            pnlTotales.ResumeLayout(false);
            pnlTotales.PerformLayout();
            pnlPagos.ResumeLayout(false);
            pnlPagos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
            pnlProductos.ResumeLayout(false);
            pnlProductos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            pnlDatosVenta.ResumeLayout(false);
            pnlDatosVenta.PerformLayout();
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            ResumeLayout(false);
        }

        #endregion


        private Panel pnlPrincipal;

        private Panel pnlCabecera;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Panel pnlLineaTitulo;

        private Panel pnlDatosVenta;

        private Label lblFechaTitulo;
        private Label lblFecha;

        private Label lblSucursalTitulo;
        private Label lblSucursal;

        private Label lblTipoFacturaTitulo;
        private Label lblTipoFactura;

        private Label lblClienteTitulo;
        private Label lblCliente;

        private Label lblDocumentoTitulo;
        private Label lblDocumento;

        private Label lblVendedorTitulo;
        private Label lblVendedor;

        private Panel pnlProductos;
        private Label lblProductosTitulo;
        private DataGridView dgvProductos;

        private Panel pnlPagos;
        private Label lblPagosTitulo;
        private DataGridView dgvPagos;

        private Panel pnlTotales;

        private Label lblSubtotalTitulo;
        private Label lblSubtotal;

        private Label lblDescuentoTitulo;
        private Label lblDescuento;

        private Label lblTotalTitulo;
        private Label lblTotal;

        private Panel pnlAcciones;
        private Button btnGenerarPdf;
        private Button btnCerrar;
    }
}
