namespace Capa_Vistas
{
    partial class FormVentas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();

            // --- Panel Vendedor ---
            this.pnlVendedor = new System.Windows.Forms.Panel();
            this.lblVendedor = new System.Windows.Forms.Label();
            this.txtVendedorId = new System.Windows.Forms.TextBox();
            this.txtVendedorNombre = new System.Windows.Forms.TextBox();

            // --- Panel Cliente ---
            this.pnlCliente = new System.Windows.Forms.Panel();
            this.lblCliente = new System.Windows.Forms.Label();
            this.txtClienteBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscarCliente = new System.Windows.Forms.Button();
            this.cmbClienteResultados = new System.Windows.Forms.ComboBox();

            // --- Panel Producto ---
            this.pnlProducto = new System.Windows.Forms.Panel();
            this.lblProducto = new System.Windows.Forms.Label();
            this.txtProductoBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscarProducto = new System.Windows.Forms.Button();
            this.cmbProductoResultados = new System.Windows.Forms.ComboBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblStock = new System.Windows.Forms.Label();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.lblPrecioVenta = new System.Windows.Forms.Label();
            this.txtPrecioVenta = new System.Windows.Forms.TextBox();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.nudCantidad = new System.Windows.Forms.NumericUpDown();
            this.btnAgregar = new System.Windows.Forms.Button();

            // --- Panel Fecha / Factura ---
            this.pnlFechaFactura = new System.Windows.Forms.Panel();
            this.lblFecha = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.lblTipoFactura = new System.Windows.Forms.Label();
            this.cmbTipoFactura = new System.Windows.Forms.ComboBox();

            // --- Grilla de ítems ---
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.colIdProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecioUnitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuitar = new System.Windows.Forms.DataGridViewButtonColumn();

            // --- Total y botones ---
            this.lblPrecioTotal = new System.Windows.Forms.Label();
            this.txtPrecioTotal = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();

            this.pnlVendedor.SuspendLayout();
            this.pnlCliente.SuspendLayout();
            this.pnlProducto.SuspendLayout();
            this.pnlFechaFactura.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();

            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(150, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Nueva Venta";

            //
            // pnlVendedor
            //
            this.pnlVendedor.Controls.Add(this.lblVendedor);
            this.pnlVendedor.Controls.Add(this.txtVendedorId);
            this.pnlVendedor.Controls.Add(this.txtVendedorNombre);
            this.pnlVendedor.Location = new System.Drawing.Point(20, 55);
            this.pnlVendedor.Name = "pnlVendedor";
            this.pnlVendedor.Size = new System.Drawing.Size(260, 40);
            this.pnlVendedor.TabIndex = 1;
            //
            // lblVendedor
            //
            this.lblVendedor.AutoSize = true;
            this.lblVendedor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblVendedor.Location = new System.Drawing.Point(0, 10);
            this.lblVendedor.Name = "lblVendedor";
            this.lblVendedor.Size = new System.Drawing.Size(65, 15);
            this.lblVendedor.TabIndex = 0;
            this.lblVendedor.Text = "Vendedor:";
            //
            // txtVendedorId
            //
            this.txtVendedorId.Location = new System.Drawing.Point(75, 7);
            this.txtVendedorId.Name = "txtVendedorId";
            this.txtVendedorId.ReadOnly = true;
            this.txtVendedorId.Size = new System.Drawing.Size(40, 23);
            this.txtVendedorId.TabIndex = 1;
            //
            // txtVendedorNombre
            //
            this.txtVendedorNombre.Location = new System.Drawing.Point(120, 7);
            this.txtVendedorNombre.Name = "txtVendedorNombre";
            this.txtVendedorNombre.ReadOnly = true;
            this.txtVendedorNombre.Size = new System.Drawing.Size(135, 23);
            this.txtVendedorNombre.TabIndex = 2;

            //
            // pnlCliente
            //
            this.pnlCliente.Controls.Add(this.lblCliente);
            this.pnlCliente.Controls.Add(this.txtClienteBuscar);
            this.pnlCliente.Controls.Add(this.btnBuscarCliente);
            this.pnlCliente.Controls.Add(this.cmbClienteResultados);
            this.pnlCliente.Location = new System.Drawing.Point(300, 55);
            this.pnlCliente.Name = "pnlCliente";
            this.pnlCliente.Size = new System.Drawing.Size(340, 75);
            this.pnlCliente.TabIndex = 2;
            //
            // lblCliente
            //
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCliente.Location = new System.Drawing.Point(0, 10);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(50, 15);
            this.lblCliente.TabIndex = 0;
            this.lblCliente.Text = "Cliente:";
            //
            // txtClienteBuscar
            //
            this.txtClienteBuscar.Location = new System.Drawing.Point(60, 7);
            this.txtClienteBuscar.Name = "txtClienteBuscar";
            this.txtClienteBuscar.Size = new System.Drawing.Size(160, 23);
            this.txtClienteBuscar.TabIndex = 1;
            //
            // btnBuscarCliente
            //
            this.btnBuscarCliente.Location = new System.Drawing.Point(230, 6);
            this.btnBuscarCliente.Name = "btnBuscarCliente";
            this.btnBuscarCliente.Size = new System.Drawing.Size(90, 25);
            this.btnBuscarCliente.TabIndex = 2;
            this.btnBuscarCliente.Text = "Buscar";
            this.btnBuscarCliente.UseVisualStyleBackColor = true;
            this.btnBuscarCliente.Click += new System.EventHandler(this.BtnBuscarCliente_Click);
            //
            // cmbClienteResultados
            //
            this.cmbClienteResultados.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClienteResultados.Location = new System.Drawing.Point(0, 40);
            this.cmbClienteResultados.Name = "cmbClienteResultados";
            this.cmbClienteResultados.Size = new System.Drawing.Size(320, 23);
            this.cmbClienteResultados.TabIndex = 3;
            this.cmbClienteResultados.SelectedIndexChanged += new System.EventHandler(this.CmbClienteResultados_SelectedIndexChanged);

            //
            // pnlProducto
            //
            this.pnlProducto.Controls.Add(this.lblProducto);
            this.pnlProducto.Controls.Add(this.txtProductoBuscar);
            this.pnlProducto.Controls.Add(this.btnBuscarProducto);
            this.pnlProducto.Controls.Add(this.cmbProductoResultados);
            this.pnlProducto.Controls.Add(this.lblDescripcion);
            this.pnlProducto.Controls.Add(this.txtDescripcion);
            this.pnlProducto.Controls.Add(this.lblStock);
            this.pnlProducto.Controls.Add(this.txtStock);
            this.pnlProducto.Controls.Add(this.lblPrecioVenta);
            this.pnlProducto.Controls.Add(this.txtPrecioVenta);
            this.pnlProducto.Controls.Add(this.lblCantidad);
            this.pnlProducto.Controls.Add(this.nudCantidad);
            this.pnlProducto.Controls.Add(this.btnAgregar);
            this.pnlProducto.Location = new System.Drawing.Point(20, 145);
            this.pnlProducto.Name = "pnlProducto";
            this.pnlProducto.Size = new System.Drawing.Size(620, 195);
            this.pnlProducto.TabIndex = 3;
            //
            // lblProducto
            //
            this.lblProducto.AutoSize = true;
            this.lblProducto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblProducto.Location = new System.Drawing.Point(0, 10);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(65, 15);
            this.lblProducto.TabIndex = 0;
            this.lblProducto.Text = "Producto:";
            //
            // txtProductoBuscar
            //
            this.txtProductoBuscar.Location = new System.Drawing.Point(80, 7);
            this.txtProductoBuscar.Name = "txtProductoBuscar";
            this.txtProductoBuscar.Size = new System.Drawing.Size(220, 23);
            this.txtProductoBuscar.TabIndex = 1;
            //
            // btnBuscarProducto
            //
            this.btnBuscarProducto.Location = new System.Drawing.Point(310, 6);
            this.btnBuscarProducto.Name = "btnBuscarProducto";
            this.btnBuscarProducto.Size = new System.Drawing.Size(90, 25);
            this.btnBuscarProducto.TabIndex = 2;
            this.btnBuscarProducto.Text = "Buscar";
            this.btnBuscarProducto.UseVisualStyleBackColor = true;
            this.btnBuscarProducto.Click += new System.EventHandler(this.BtnBuscarProducto_Click);
            //
            // cmbProductoResultados
            //
            this.cmbProductoResultados.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductoResultados.Location = new System.Drawing.Point(0, 40);
            this.cmbProductoResultados.Name = "cmbProductoResultados";
            this.cmbProductoResultados.Size = new System.Drawing.Size(400, 23);
            this.cmbProductoResultados.TabIndex = 3;
            this.cmbProductoResultados.SelectedIndexChanged += new System.EventHandler(this.CmbProductoResultados_SelectedIndexChanged);
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(0, 78);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(75, 15);
            this.lblDescripcion.TabIndex = 4;
            this.lblDescripcion.Text = "Descripción:";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(90, 75);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.ReadOnly = true;
            this.txtDescripcion.Size = new System.Drawing.Size(310, 23);
            this.txtDescripcion.TabIndex = 5;
            //
            // lblStock
            //
            this.lblStock.AutoSize = true;
            this.lblStock.Location = new System.Drawing.Point(0, 115);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(40, 15);
            this.lblStock.TabIndex = 6;
            this.lblStock.Text = "Stock:";
            //
            // txtStock
            //
            this.txtStock.Location = new System.Drawing.Point(50, 112);
            this.txtStock.Name = "txtStock";
            this.txtStock.ReadOnly = true;
            this.txtStock.Size = new System.Drawing.Size(60, 23);
            this.txtStock.TabIndex = 7;
            //
            // lblPrecioVenta
            //
            this.lblPrecioVenta.AutoSize = true;
            this.lblPrecioVenta.Location = new System.Drawing.Point(130, 115);
            this.lblPrecioVenta.Name = "lblPrecioVenta";
            this.lblPrecioVenta.Size = new System.Drawing.Size(80, 15);
            this.lblPrecioVenta.TabIndex = 8;
            this.lblPrecioVenta.Text = "Precio Venta:";
            //
            // txtPrecioVenta
            //
            this.txtPrecioVenta.Location = new System.Drawing.Point(215, 112);
            this.txtPrecioVenta.Name = "txtPrecioVenta";
            this.txtPrecioVenta.Size = new System.Drawing.Size(80, 23);
            this.txtPrecioVenta.TabIndex = 9;
            //
            // lblCantidad
            //
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(310, 115);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(60, 15);
            this.lblCantidad.TabIndex = 10;
            this.lblCantidad.Text = "Cantidad:";
            //
            // nudCantidad
            //
            this.nudCantidad.Location = new System.Drawing.Point(375, 112);
            this.nudCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudCantidad.Name = "nudCantidad";
            this.nudCantidad.Size = new System.Drawing.Size(60, 23);
            this.nudCantidad.TabIndex = 11;
            this.nudCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // btnAgregar
            //
            this.btnAgregar.BackColor = System.Drawing.Color.LightGreen;
            this.btnAgregar.Location = new System.Drawing.Point(0, 150);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(150, 32);
            this.btnAgregar.TabIndex = 12;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.BtnAgregar_Click);

            //
            // pnlFechaFactura
            //
            this.pnlFechaFactura.Controls.Add(this.lblFecha);
            this.pnlFechaFactura.Controls.Add(this.dtpFecha);
            this.pnlFechaFactura.Controls.Add(this.lblTipoFactura);
            this.pnlFechaFactura.Controls.Add(this.cmbTipoFactura);
            this.pnlFechaFactura.Location = new System.Drawing.Point(660, 145);
            this.pnlFechaFactura.Name = "pnlFechaFactura";
            this.pnlFechaFactura.Size = new System.Drawing.Size(220, 100);
            this.pnlFechaFactura.TabIndex = 4;
            //
            // lblFecha
            //
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFecha.Location = new System.Drawing.Point(0, 10);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(45, 15);
            this.lblFecha.TabIndex = 0;
            this.lblFecha.Text = "Fecha:";
            //
            // dtpFecha
            //
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(70, 7);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(130, 23);
            this.dtpFecha.TabIndex = 1;
            //
            // lblTipoFactura
            //
            this.lblTipoFactura.AutoSize = true;
            this.lblTipoFactura.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTipoFactura.Location = new System.Drawing.Point(0, 45);
            this.lblTipoFactura.Name = "lblTipoFactura";
            this.lblTipoFactura.Size = new System.Drawing.Size(35, 15);
            this.lblTipoFactura.TabIndex = 2;
            this.lblTipoFactura.Text = "Tipo:";
            //
            // cmbTipoFactura
            //
            this.cmbTipoFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoFactura.Items.AddRange(new object[] { "A", "B", "C" });
            this.cmbTipoFactura.Location = new System.Drawing.Point(70, 42);
            this.cmbTipoFactura.Name = "cmbTipoFactura";
            this.cmbTipoFactura.Size = new System.Drawing.Size(130, 23);
            this.cmbTipoFactura.TabIndex = 3;

            //
            // dgvItems
            //
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colIdProducto,
                this.colDescripcion,
                this.colCantidad,
                this.colPrecioUnitario,
                this.colSubTotal,
                this.colQuitar});
            this.dgvItems.Location = new System.Drawing.Point(20, 355);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.ReadOnly = true;
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.Size = new System.Drawing.Size(860, 190);
            this.dgvItems.TabIndex = 5;
            this.dgvItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvItems_CellContentClick);
            //
            // colIdProducto
            //
            this.colIdProducto.DataPropertyName = "IdProducto";
            this.colIdProducto.HeaderText = "idProducto";
            this.colIdProducto.Name = "colIdProducto";
            this.colIdProducto.ReadOnly = true;
            this.colIdProducto.Width = 80;
            //
            // colDescripcion
            //
            this.colDescripcion.DataPropertyName = "Descripcion";
            this.colDescripcion.HeaderText = "descripcion";
            this.colDescripcion.Name = "colDescripcion";
            this.colDescripcion.ReadOnly = true;
            this.colDescripcion.Width = 300;
            //
            // colCantidad
            //
            this.colCantidad.DataPropertyName = "Cantidad";
            this.colCantidad.HeaderText = "cantidad";
            this.colCantidad.Name = "colCantidad";
            this.colCantidad.ReadOnly = true;
            this.colCantidad.Width = 80;
            //
            // colPrecioUnitario
            //
            this.colPrecioUnitario.DataPropertyName = "PrecioUnitario";
            this.colPrecioUnitario.HeaderText = "precioUnitario";
            this.colPrecioUnitario.Name = "colPrecioUnitario";
            this.colPrecioUnitario.ReadOnly = true;
            this.colPrecioUnitario.Width = 100;
            //
            // colSubTotal
            //
            this.colSubTotal.DataPropertyName = "SubTotal";
            this.colSubTotal.HeaderText = "subTotal";
            this.colSubTotal.Name = "colSubTotal";
            this.colSubTotal.ReadOnly = true;
            this.colSubTotal.Width = 100;
            //
            // colQuitar
            //
            this.colQuitar.HeaderText = "eliminar";
            this.colQuitar.Name = "colQuitar";
            this.colQuitar.Text = "Quitar";
            this.colQuitar.UseColumnTextForButtonValue = true;
            this.colQuitar.Width = 90;

            //
            // lblPrecioTotal
            //
            this.lblPrecioTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrecioTotal.AutoSize = true;
            this.lblPrecioTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrecioTotal.Location = new System.Drawing.Point(650, 555);
            this.lblPrecioTotal.Name = "lblPrecioTotal";
            this.lblPrecioTotal.Size = new System.Drawing.Size(95, 19);
            this.lblPrecioTotal.TabIndex = 6;
            this.lblPrecioTotal.Text = "Precio Total:";
            //
            // txtPrecioTotal
            //
            this.txtPrecioTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrecioTotal.Location = new System.Drawing.Point(750, 553);
            this.txtPrecioTotal.Name = "txtPrecioTotal";
            this.txtPrecioTotal.ReadOnly = true;
            this.txtPrecioTotal.Size = new System.Drawing.Size(130, 23);
            this.txtPrecioTotal.TabIndex = 7;
            this.txtPrecioTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // btnGuardar
            //
            this.btnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGuardar.BackColor = System.Drawing.Color.LightGreen;
            this.btnGuardar.Location = new System.Drawing.Point(300, 590);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(130, 35);
            this.btnGuardar.TabIndex = 8;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelar.BackColor = System.Drawing.Color.LightCoral;
            this.btnCancelar.Location = new System.Drawing.Point(450, 590);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(130, 35);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);

            //
            // FormVentas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtPrecioTotal);
            this.Controls.Add(this.lblPrecioTotal);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.pnlFechaFactura);
            this.Controls.Add(this.pnlProducto);
            this.Controls.Add(this.pnlCliente);
            this.Controls.Add(this.pnlVendedor);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormVentas";
            this.Text = "FormVentas";
            this.pnlVendedor.ResumeLayout(false);
            this.pnlVendedor.PerformLayout();
            this.pnlCliente.ResumeLayout(false);
            this.pnlCliente.PerformLayout();
            this.pnlProducto.ResumeLayout(false);
            this.pnlProducto.PerformLayout();
            this.pnlFechaFactura.ResumeLayout(false);
            this.pnlFechaFactura.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;

        private System.Windows.Forms.Panel pnlVendedor;
        private System.Windows.Forms.Label lblVendedor;
        private System.Windows.Forms.TextBox txtVendedorId;
        private System.Windows.Forms.TextBox txtVendedorNombre;

        private System.Windows.Forms.Panel pnlCliente;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.TextBox txtClienteBuscar;
        private System.Windows.Forms.Button btnBuscarCliente;
        private System.Windows.Forms.ComboBox cmbClienteResultados;

        private System.Windows.Forms.Panel pnlProducto;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.TextBox txtProductoBuscar;
        private System.Windows.Forms.Button btnBuscarProducto;
        private System.Windows.Forms.ComboBox cmbProductoResultados;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.Label lblPrecioVenta;
        private System.Windows.Forms.TextBox txtPrecioVenta;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.NumericUpDown nudCantidad;
        private System.Windows.Forms.Button btnAgregar;

        private System.Windows.Forms.Panel pnlFechaFactura;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblTipoFactura;
        private System.Windows.Forms.ComboBox cmbTipoFactura;

        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecioUnitario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubTotal;
        private System.Windows.Forms.DataGridViewButtonColumn colQuitar;

        private System.Windows.Forms.Label lblPrecioTotal;
        private System.Windows.Forms.TextBox txtPrecioTotal;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
