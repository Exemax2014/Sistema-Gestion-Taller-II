using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormProductoDetalle
    {
        private System.ComponentModel.IContainer components =
            null;


        private Panel pnlCabecera;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Panel pnlLineaTitulo;
        private Button btnVolver;


        private Panel pnlDatos;

        private Label lblDatosTitulo;

        private Label lblCategoria;
        private ComboBox cmbCategoria;

        private Label lblMarca;
        private ComboBox cmbMarca;

        private Label lblCodigoBarra;
        private TextBox txtCodigoBarra;

        private Label lblNombre;
        private TextBox txtNombre;

        private Label lblDescripcion;
        private TextBox txtDescripcion;

        private Label lblPrecioCosto;
        private TextBox txtPrecioCosto;

        private Label lblGanancia;
        private TextBox txtPorcentajeGanancia;

        private Label lblPrecioVenta;
        private Label lblPrecioVentaValor;

        private CheckBox chkActivo;


        private Panel pnlStock;

        private Label lblStockTitulo;
        private Label lblStockDescripcion;

        private DataGridView dgvStockSucursales;


        private Panel pnlEditarStock;

        private Label lblEditarStockSucursal;

        private Label lblNuevoStock;
        private TextBox txtNuevoStock;

        private Label lblStockMinimoEditar;
        private TextBox txtStockMinimo;

        private Button btnGuardarStock;
        private Button btnCancelarStock;


        private Panel pnlAcciones;

        private Button btnGuardar;
        private Button btnCancelar;
        private Button btnEliminar;


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
            btnVolver = new Button();
            pnlDatos = new Panel();
            lblDatosTitulo = new Label();
            lblCategoria = new Label();
            cmbCategoria = new ComboBox();
            lblMarca = new Label();
            cmbMarca = new ComboBox();
            lblCodigoBarra = new Label();
            txtCodigoBarra = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblPrecioCosto = new Label();
            txtPrecioCosto = new TextBox();
            lblGanancia = new Label();
            txtPorcentajeGanancia = new TextBox();
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            lblPrecioVenta = new Label();
            lblPrecioVentaValor = new Label();
            chkActivo = new CheckBox();
            pnlStock = new Panel();
            lblStockTitulo = new Label();
            lblStockDescripcion = new Label();
            dgvStockSucursales = new DataGridView();
            pnlEditarStock = new Panel();
            lblEditarStockSucursal = new Label();
            lblNuevoStock = new Label();
            txtNuevoStock = new TextBox();
            lblStockMinimoEditar = new Label();
            txtStockMinimo = new TextBox();
            btnGuardarStock = new Button();
            btnCancelarStock = new Button();
            pnlAcciones = new Panel();
            btnGuardar = new Button();
            btnCancelar = new Button();
            btnEliminar = new Button();
            pnlCabecera.SuspendLayout();
            pnlDatos.SuspendLayout();
            pnlStock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStockSucursales).BeginInit();
            pnlEditarStock.SuspendLayout();
            pnlAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // pnlCabecera
            // 
            pnlCabecera.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Controls.Add(lblSubtitulo);
            pnlCabecera.Controls.Add(pnlLineaTitulo);
            pnlCabecera.Controls.Add(btnVolver);
            pnlCabecera.Location = new Point(32, 18);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(1116, 86);
            pnlCabecera.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(45, 49, 54);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(380, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Detalle del producto";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(2, 50);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(311, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Información general e inventario por sucursal.";
            // 
            // pnlLineaTitulo
            // 
            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(2, 76);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(95, 3);
            pnlLineaTitulo.TabIndex = 2;
            // 
            // btnVolver
            // 
            btnVolver.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnVolver.BackColor = Color.White;
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatAppearance.BorderColor = Color.FromArgb(190, 137, 45);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = Color.FromArgb(70, 75, 80);
            btnVolver.Location = new Point(941, 14);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(150, 40);
            btnVolver.TabIndex = 3;
            btnVolver.Text = "← Volver";
            btnVolver.UseVisualStyleBackColor = false;
            // 
            // pnlDatos
            // 
            pnlDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlDatos.BackColor = Color.WhiteSmoke;
            pnlDatos.BorderStyle = BorderStyle.FixedSingle;
            pnlDatos.Controls.Add(lblDatosTitulo);
            pnlDatos.Controls.Add(lblCategoria);
            pnlDatos.Controls.Add(cmbCategoria);
            pnlDatos.Controls.Add(lblMarca);
            pnlDatos.Controls.Add(cmbMarca);
            pnlDatos.Controls.Add(lblCodigoBarra);
            pnlDatos.Controls.Add(txtCodigoBarra);
            pnlDatos.Controls.Add(lblNombre);
            pnlDatos.Controls.Add(txtNombre);
            pnlDatos.Controls.Add(lblPrecioCosto);
            pnlDatos.Controls.Add(txtPrecioCosto);
            pnlDatos.Controls.Add(lblGanancia);
            pnlDatos.Controls.Add(txtPorcentajeGanancia);
            pnlDatos.Controls.Add(lblDescripcion);
            pnlDatos.Controls.Add(txtDescripcion);
            pnlDatos.Controls.Add(lblPrecioVenta);
            pnlDatos.Controls.Add(lblPrecioVentaValor);
            pnlDatos.Controls.Add(chkActivo);
            pnlDatos.Location = new Point(32, 110);
            pnlDatos.Name = "pnlDatos";
            pnlDatos.Size = new Size(1116, 305);
            pnlDatos.TabIndex = 1;
            // 
            // lblDatosTitulo
            // 
            lblDatosTitulo.AutoSize = true;
            lblDatosTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblDatosTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblDatosTitulo.Location = new Point(20, 12);
            lblDatosTitulo.Name = "lblDatosTitulo";
            lblDatosTitulo.Size = new Size(194, 25);
            lblDatosTitulo.TabIndex = 0;
            lblDatosTitulo.Text = "Información general";
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(20, 50);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(74, 20);
            lblCategoria.TabIndex = 1;
            lblCategoria.Text = "Categoría";
            // 
            // cmbCategoria
            // 
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.Location = new Point(20, 73);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(240, 28);
            cmbCategoria.TabIndex = 2;
            // 
            // lblMarca
            // 
            lblMarca.AutoSize = true;
            lblMarca.Location = new Point(280, 50);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(50, 20);
            lblMarca.TabIndex = 3;
            lblMarca.Text = "Marca";
            // 
            // cmbMarca
            // 
            cmbMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMarca.Location = new Point(280, 73);
            cmbMarca.Name = "cmbMarca";
            cmbMarca.Size = new Size(240, 28);
            cmbMarca.TabIndex = 4;
            // 
            // lblCodigoBarra
            // 
            lblCodigoBarra.AutoSize = true;
            lblCodigoBarra.Location = new Point(20, 115);
            lblCodigoBarra.Name = "lblCodigoBarra";
            lblCodigoBarra.Size = new Size(124, 20);
            lblCodigoBarra.TabIndex = 5;
            lblCodigoBarra.Text = "Código de barras";
            // 
            // txtCodigoBarra
            // 
            txtCodigoBarra.Location = new Point(20, 138);
            txtCodigoBarra.Name = "txtCodigoBarra";
            txtCodigoBarra.Size = new Size(240, 27);
            txtCodigoBarra.TabIndex = 6;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(280, 115);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(64, 20);
            lblNombre.TabIndex = 7;
            lblNombre.Text = "Nombre";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(280, 138);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(240, 27);
            txtNombre.TabIndex = 8;
            // 
            // lblPrecioCosto
            // 
            lblPrecioCosto.AutoSize = true;
            lblPrecioCosto.Location = new Point(20, 185);
            lblPrecioCosto.Name = "lblPrecioCosto";
            lblPrecioCosto.Size = new Size(111, 20);
            lblPrecioCosto.TabIndex = 9;
            lblPrecioCosto.Text = "Precio de costo";
            // 
            // txtPrecioCosto
            // 
            txtPrecioCosto.Location = new Point(20, 208);
            txtPrecioCosto.Name = "txtPrecioCosto";
            txtPrecioCosto.Size = new Size(240, 27);
            txtPrecioCosto.TabIndex = 10;
            // 
            // lblGanancia
            // 
            lblGanancia.AutoSize = true;
            lblGanancia.Location = new Point(280, 185);
            lblGanancia.Name = "lblGanancia";
            lblGanancia.Size = new Size(106, 20);
            lblGanancia.TabIndex = 11;
            lblGanancia.Text = "% de ganancia";
            // 
            // txtPorcentajeGanancia
            // 
            txtPorcentajeGanancia.Location = new Point(280, 208);
            txtPorcentajeGanancia.Name = "txtPorcentajeGanancia";
            txtPorcentajeGanancia.Size = new Size(240, 27);
            txtPorcentajeGanancia.TabIndex = 12;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(570, 50);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(87, 20);
            lblDescripcion.TabIndex = 13;
            lblDescripcion.Text = "Descripción";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDescripcion.Location = new Point(570, 73);
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Size = new Size(520, 202);
            txtDescripcion.TabIndex = 14;
            // 
            // lblPrecioVenta
            // 
            lblPrecioVenta.AutoSize = true;
            lblPrecioVenta.Font = new Font("Segoe UI", 12F);
            lblPrecioVenta.Location = new Point(20, 250);
            lblPrecioVenta.Name = "lblPrecioVenta";
            lblPrecioVenta.Size = new Size(146, 28);
            lblPrecioVenta.TabIndex = 15;
            lblPrecioVenta.Text = "Precio de venta";
            // 
            // lblPrecioVentaValor
            // 
            lblPrecioVentaValor.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblPrecioVentaValor.ForeColor = Color.FromArgb(190, 137, 45);
            lblPrecioVentaValor.Location = new Point(189, 250);
            lblPrecioVentaValor.Name = "lblPrecioVentaValor";
            lblPrecioVentaValor.Size = new Size(190, 32);
            lblPrecioVentaValor.TabIndex = 16;
            lblPrecioVentaValor.Text = "$ 0,00";
            // 
            // chkActivo
            // 
            chkActivo.AutoSize = true;
            chkActivo.Location = new Point(385, 258);
            chkActivo.Name = "chkActivo";
            chkActivo.Size = new Size(135, 24);
            chkActivo.TabIndex = 17;
            chkActivo.Text = "Producto activo";
            // 
            // pnlStock
            // 
            pnlStock.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlStock.BackColor = Color.White;
            pnlStock.BorderStyle = BorderStyle.FixedSingle;
            pnlStock.Controls.Add(lblStockTitulo);
            pnlStock.Controls.Add(lblStockDescripcion);
            pnlStock.Controls.Add(dgvStockSucursales);
            pnlStock.Controls.Add(pnlEditarStock);
            pnlStock.Location = new Point(32, 425);
            pnlStock.Name = "pnlStock";
            pnlStock.Size = new Size(1116, 258);
            pnlStock.TabIndex = 2;
            // 
            // lblStockTitulo
            // 
            lblStockTitulo.AutoSize = true;
            lblStockTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblStockTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblStockTitulo.Location = new Point(20, 10);
            lblStockTitulo.Name = "lblStockTitulo";
            lblStockTitulo.Size = new Size(176, 25);
            lblStockTitulo.TabIndex = 0;
            lblStockTitulo.Text = "Stock por sucursal";
            // 
            // lblStockDescripcion
            // 
            lblStockDescripcion.AutoSize = true;
            lblStockDescripcion.Font = new Font("Segoe UI", 8F);
            lblStockDescripcion.ForeColor = Color.FromArgb(105, 110, 116);
            lblStockDescripcion.Location = new Point(200, 16);
            lblStockDescripcion.Name = "lblStockDescripcion";
            lblStockDescripcion.Size = new Size(406, 19);
            lblStockDescripcion.TabIndex = 1;
            lblStockDescripcion.Text = "Todos los perfiles pueden consultar la disponibilidad por sucursal.";
            // 
            // dgvStockSucursales
            // 
            dgvStockSucursales.AllowUserToAddRows = false;
            dgvStockSucursales.AllowUserToDeleteRows = false;
            dgvStockSucursales.AllowUserToResizeRows = false;
            dgvStockSucursales.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvStockSucursales.BackgroundColor = Color.White;
            dgvStockSucursales.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvStockSucursales.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvStockSucursales.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvStockSucursales.ColumnHeadersHeight = 42;
            dgvStockSucursales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(55, 59, 64);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(226, 229, 232);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(40, 44, 48);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvStockSucursales.DefaultCellStyle = dataGridViewCellStyle2;
            dgvStockSucursales.EnableHeadersVisualStyles = false;
            dgvStockSucursales.GridColor = Color.FromArgb(224, 227, 230);
            dgvStockSucursales.Location = new Point(20, 45);
            dgvStockSucursales.MultiSelect = false;
            dgvStockSucursales.Name = "dgvStockSucursales";
            dgvStockSucursales.ReadOnly = true;
            dgvStockSucursales.RowHeadersVisible = false;
            dgvStockSucursales.RowHeadersWidth = 51;
            dgvStockSucursales.RowTemplate.Height = 40;
            dgvStockSucursales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStockSucursales.Size = new Size(1074, 120);
            dgvStockSucursales.TabIndex = 2;
            // 
            // pnlEditarStock
            // 
            pnlEditarStock.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlEditarStock.BackColor = Color.FromArgb(246, 247, 248);
            pnlEditarStock.BorderStyle = BorderStyle.FixedSingle;
            pnlEditarStock.Controls.Add(lblEditarStockSucursal);
            pnlEditarStock.Controls.Add(lblNuevoStock);
            pnlEditarStock.Controls.Add(txtNuevoStock);
            pnlEditarStock.Controls.Add(lblStockMinimoEditar);
            pnlEditarStock.Controls.Add(txtStockMinimo);
            pnlEditarStock.Controls.Add(btnGuardarStock);
            pnlEditarStock.Controls.Add(btnCancelarStock);
            pnlEditarStock.Location = new Point(20, 175);
            pnlEditarStock.Name = "pnlEditarStock";
            pnlEditarStock.Size = new Size(1074, 62);
            pnlEditarStock.TabIndex = 3;
            // 
            // lblEditarStockSucursal
            // 
            lblEditarStockSucursal.AutoSize = true;
            lblEditarStockSucursal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEditarStockSucursal.Location = new Point(20, 21);
            lblEditarStockSucursal.Name = "lblEditarStockSucursal";
            lblEditarStockSucursal.Size = new Size(67, 20);
            lblEditarStockSucursal.TabIndex = 0;
            lblEditarStockSucursal.Text = "Sucursal";
            // 
            // lblNuevoStock
            // 
            lblNuevoStock.AutoSize = true;
            lblNuevoStock.Location = new Point(163, 24);
            lblNuevoStock.Name = "lblNuevoStock";
            lblNuevoStock.Size = new Size(90, 20);
            lblNuevoStock.TabIndex = 1;
            lblNuevoStock.Text = "Nuevo stock";
            // 
            // txtNuevoStock
            // 
            txtNuevoStock.Location = new Point(259, 21);
            txtNuevoStock.Name = "txtNuevoStock";
            txtNuevoStock.Size = new Size(110, 27);
            txtNuevoStock.TabIndex = 2;
            // 
            // lblStockMinimoEditar
            // 
            lblStockMinimoEditar.AutoSize = true;
            lblStockMinimoEditar.Location = new Point(395, 24);
            lblStockMinimoEditar.Name = "lblStockMinimoEditar";
            lblStockMinimoEditar.Size = new Size(100, 20);
            lblStockMinimoEditar.TabIndex = 3;
            lblStockMinimoEditar.Text = "Stock mínimo";
            // 
            // txtStockMinimo
            // 
            txtStockMinimo.Location = new Point(501, 21);
            txtStockMinimo.Name = "txtStockMinimo";
            txtStockMinimo.Size = new Size(110, 27);
            txtStockMinimo.TabIndex = 4;
            // 
            // btnGuardarStock
            // 
            btnGuardarStock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGuardarStock.BackColor = Color.FromArgb(190, 137, 45);
            btnGuardarStock.Cursor = Cursors.Hand;
            btnGuardarStock.FlatAppearance.BorderSize = 0;
            btnGuardarStock.FlatStyle = FlatStyle.Flat;
            btnGuardarStock.ForeColor = Color.White;
            btnGuardarStock.Location = new Point(754, 12);
            btnGuardarStock.Name = "btnGuardarStock";
            btnGuardarStock.Size = new Size(145, 38);
            btnGuardarStock.TabIndex = 5;
            btnGuardarStock.Text = "Guardar stock";
            btnGuardarStock.UseVisualStyleBackColor = false;
            // 
            // btnCancelarStock
            // 
            btnCancelarStock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelarStock.BackColor = Color.White;
            btnCancelarStock.Cursor = Cursors.Hand;
            btnCancelarStock.FlatStyle = FlatStyle.Flat;
            btnCancelarStock.Location = new Point(919, 12);
            btnCancelarStock.Name = "btnCancelarStock";
            btnCancelarStock.Size = new Size(120, 38);
            btnCancelarStock.TabIndex = 6;
            btnCancelarStock.Text = "Cancelar";
            btnCancelarStock.UseVisualStyleBackColor = false;
            // 
            // pnlAcciones
            // 
            pnlAcciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlAcciones.Controls.Add(btnGuardar);
            pnlAcciones.Controls.Add(btnCancelar);
            pnlAcciones.Controls.Add(btnEliminar);
            pnlAcciones.Location = new Point(32, 692);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(1116, 55);
            pnlAcciones.TabIndex = 3;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(190, 137, 45);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(20, 5);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(175, 42);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar cambios";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.White;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Location = new Point(215, 5);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(130, 42);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            btnEliminar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEliminar.BackColor = Color.White;
            btnEliminar.Cursor = Cursors.Hand;
            btnEliminar.FlatAppearance.BorderColor = Color.FromArgb(180, 70, 70);
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.ForeColor = Color.FromArgb(175, 55, 55);
            btnEliminar.Location = new Point(920, 5);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(175, 42);
            btnEliminar.TabIndex = 2;
            btnEliminar.Text = "Eliminar producto";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // FormProductoDetalle
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(241, 243, 245);
            ClientSize = new Size(1180, 760);
            ControlBox = false;
            Controls.Add(pnlCabecera);
            Controls.Add(pnlDatos);
            Controls.Add(pnlStock);
            Controls.Add(pnlAcciones);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormProductoDetalle";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Detalle de producto";
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlDatos.ResumeLayout(false);
            pnlDatos.PerformLayout();
            pnlStock.ResumeLayout(false);
            pnlStock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStockSucursales).EndInit();
            pnlEditarStock.ResumeLayout(false);
            pnlEditarStock.PerformLayout();
            pnlAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}