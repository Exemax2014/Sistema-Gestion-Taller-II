namespace Capa_Vistas
{
    partial class FormReportesGerente
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
            this.pnlFiltroFecha = new System.Windows.Forms.Panel();
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnRecaudacion = new System.Windows.Forms.Button();
            this.btnProductosMasVendidos = new System.Windows.Forms.Button();
            this.btnVentas = new System.Windows.Forms.Button();
            this.pnlVendedor = new System.Windows.Forms.Panel();
            this.lblVendedor = new System.Windows.Forms.Label();
            this.txtVendedorId = new System.Windows.Forms.TextBox();
            this.txtVendedorNombre = new System.Windows.Forms.TextBox();
            this.btnBuscarVendedor = new System.Windows.Forms.Button();
            this.btnVentasPorVendedor = new System.Windows.Forms.Button();
            this.dgvResultado = new System.Windows.Forms.DataGridView();
            this.pnlFiltroFecha.SuspendLayout();
            this.pnlVendedor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultado)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(30, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(260, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Reportes del Gerente";
            //
            // pnlFiltroFecha
            //
            this.pnlFiltroFecha.BackColor = System.Drawing.Color.FromArgb(64, 196, 190);
            this.pnlFiltroFecha.Controls.Add(this.lblDesde);
            this.pnlFiltroFecha.Controls.Add(this.dtpDesde);
            this.pnlFiltroFecha.Controls.Add(this.lblHasta);
            this.pnlFiltroFecha.Controls.Add(this.dtpHasta);
            this.pnlFiltroFecha.Controls.Add(this.btnRecaudacion);
            this.pnlFiltroFecha.Controls.Add(this.btnProductosMasVendidos);
            this.pnlFiltroFecha.Controls.Add(this.btnVentas);
            this.pnlFiltroFecha.Location = new System.Drawing.Point(30, 75);
            this.pnlFiltroFecha.Name = "pnlFiltroFecha";
            this.pnlFiltroFecha.Size = new System.Drawing.Size(420, 150);
            this.pnlFiltroFecha.TabIndex = 1;
            //
            // lblDesde
            //
            this.lblDesde.AutoSize = true;
            this.lblDesde.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDesde.ForeColor = System.Drawing.Color.White;
            this.lblDesde.Location = new System.Drawing.Point(20, 15);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(45, 15);
            this.lblDesde.TabIndex = 0;
            this.lblDesde.Text = "Desde";
            //
            // dtpDesde
            //
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(20, 35);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(120, 23);
            this.dtpDesde.TabIndex = 1;
            //
            // lblHasta
            //
            this.lblHasta.AutoSize = true;
            this.lblHasta.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHasta.ForeColor = System.Drawing.Color.White;
            this.lblHasta.Location = new System.Drawing.Point(160, 15);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(40, 15);
            this.lblHasta.TabIndex = 2;
            this.lblHasta.Text = "Hasta";
            //
            // dtpHasta
            //
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(160, 35);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(120, 23);
            this.dtpHasta.TabIndex = 3;
            //
            // btnRecaudacion
            //
            this.btnRecaudacion.BackColor = System.Drawing.Color.White;
            this.btnRecaudacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecaudacion.Location = new System.Drawing.Point(20, 75);
            this.btnRecaudacion.Name = "btnRecaudacion";
            this.btnRecaudacion.Size = new System.Drawing.Size(120, 60);
            this.btnRecaudacion.TabIndex = 4;
            this.btnRecaudacion.Text = "Recaudación";
            this.btnRecaudacion.UseVisualStyleBackColor = false;
            this.btnRecaudacion.Click += new System.EventHandler(this.BtnRecaudacion_Click);
            //
            // btnProductosMasVendidos
            //
            this.btnProductosMasVendidos.BackColor = System.Drawing.Color.White;
            this.btnProductosMasVendidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductosMasVendidos.Location = new System.Drawing.Point(150, 75);
            this.btnProductosMasVendidos.Name = "btnProductosMasVendidos";
            this.btnProductosMasVendidos.Size = new System.Drawing.Size(130, 60);
            this.btnProductosMasVendidos.TabIndex = 5;
            this.btnProductosMasVendidos.Text = "Productos más vendidos";
            this.btnProductosMasVendidos.UseVisualStyleBackColor = false;
            this.btnProductosMasVendidos.Click += new System.EventHandler(this.BtnProductosMasVendidos_Click);
            //
            // btnVentas
            //
            this.btnVentas.BackColor = System.Drawing.Color.White;
            this.btnVentas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVentas.Location = new System.Drawing.Point(290, 75);
            this.btnVentas.Name = "btnVentas";
            this.btnVentas.Size = new System.Drawing.Size(110, 60);
            this.btnVentas.TabIndex = 6;
            this.btnVentas.Text = "Ventas";
            this.btnVentas.UseVisualStyleBackColor = false;
            this.btnVentas.Click += new System.EventHandler(this.BtnVentas_Click);
            //
            // pnlVendedor
            //
            this.pnlVendedor.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlVendedor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVendedor.Controls.Add(this.lblVendedor);
            this.pnlVendedor.Controls.Add(this.txtVendedorId);
            this.pnlVendedor.Controls.Add(this.txtVendedorNombre);
            this.pnlVendedor.Controls.Add(this.btnBuscarVendedor);
            this.pnlVendedor.Location = new System.Drawing.Point(30, 235);
            this.pnlVendedor.Name = "pnlVendedor";
            this.pnlVendedor.Size = new System.Drawing.Size(300, 70);
            this.pnlVendedor.TabIndex = 2;
            //
            // lblVendedor
            //
            this.lblVendedor.AutoSize = true;
            this.lblVendedor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblVendedor.Location = new System.Drawing.Point(10, 8);
            this.lblVendedor.Name = "lblVendedor";
            this.lblVendedor.Size = new System.Drawing.Size(60, 15);
            this.lblVendedor.TabIndex = 0;
            this.lblVendedor.Text = "Vendedor:";
            //
            // txtVendedorId
            //
            this.txtVendedorId.Location = new System.Drawing.Point(10, 30);
            this.txtVendedorId.Name = "txtVendedorId";
            this.txtVendedorId.Size = new System.Drawing.Size(50, 23);
            this.txtVendedorId.TabIndex = 1;
            //
            // txtVendedorNombre
            //
            this.txtVendedorNombre.Location = new System.Drawing.Point(65, 30);
            this.txtVendedorNombre.Name = "txtVendedorNombre";
            this.txtVendedorNombre.ReadOnly = true;
            this.txtVendedorNombre.Size = new System.Drawing.Size(140, 23);
            this.txtVendedorNombre.TabIndex = 2;
            //
            // btnBuscarVendedor
            //
            this.btnBuscarVendedor.Location = new System.Drawing.Point(212, 30);
            this.btnBuscarVendedor.Name = "btnBuscarVendedor";
            this.btnBuscarVendedor.Size = new System.Drawing.Size(75, 23);
            this.btnBuscarVendedor.TabIndex = 3;
            this.btnBuscarVendedor.Text = "Buscar";
            this.btnBuscarVendedor.UseVisualStyleBackColor = true;
            this.btnBuscarVendedor.Click += new System.EventHandler(this.BtnBuscarVendedor_Click);
            //
            // btnVentasPorVendedor
            //
            this.btnVentasPorVendedor.BackColor = System.Drawing.Color.FromArgb(64, 196, 190);
            this.btnVentasPorVendedor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVentasPorVendedor.ForeColor = System.Drawing.Color.White;
            this.btnVentasPorVendedor.Location = new System.Drawing.Point(340, 235);
            this.btnVentasPorVendedor.Name = "btnVentasPorVendedor";
            this.btnVentasPorVendedor.Size = new System.Drawing.Size(110, 70);
            this.btnVentasPorVendedor.TabIndex = 3;
            this.btnVentasPorVendedor.Text = "Ventas por vendedor";
            this.btnVentasPorVendedor.UseVisualStyleBackColor = false;
            this.btnVentasPorVendedor.Click += new System.EventHandler(this.BtnVentasPorVendedor_Click);
            //
            // dgvResultado
            //
            this.dgvResultado.AllowUserToAddRows = false;
            this.dgvResultado.AllowUserToDeleteRows = false;
            this.dgvResultado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvResultado.Location = new System.Drawing.Point(30, 320);
            this.dgvResultado.Name = "dgvResultado";
            this.dgvResultado.ReadOnly = true;
            this.dgvResultado.RowHeadersVisible = false;
            this.dgvResultado.Size = new System.Drawing.Size(600, 260);
            this.dgvResultado.TabIndex = 4;
            //
            // FormReportesGerente
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(660, 610);
            this.Controls.Add(this.dgvResultado);
            this.Controls.Add(this.btnVentasPorVendedor);
            this.Controls.Add(this.pnlVendedor);
            this.Controls.Add(this.pnlFiltroFecha);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormReportesGerente";
            this.Text = "FormReportesGerente";
            this.pnlFiltroFecha.ResumeLayout(false);
            this.pnlFiltroFecha.PerformLayout();
            this.pnlVendedor.ResumeLayout(false);
            this.pnlVendedor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultado)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlFiltroFecha;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnRecaudacion;
        private System.Windows.Forms.Button btnProductosMasVendidos;
        private System.Windows.Forms.Button btnVentas;
        private System.Windows.Forms.Panel pnlVendedor;
        private System.Windows.Forms.Label lblVendedor;
        private System.Windows.Forms.TextBox txtVendedorId;
        private System.Windows.Forms.TextBox txtVendedorNombre;
        private System.Windows.Forms.Button btnBuscarVendedor;
        private System.Windows.Forms.Button btnVentasPorVendedor;
        private System.Windows.Forms.DataGridView dgvResultado;
    }
}