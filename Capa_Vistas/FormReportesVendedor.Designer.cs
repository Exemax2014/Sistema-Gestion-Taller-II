namespace Capa_Vistas
{
    partial class FormReportesVendedor
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
            this.pnlFiltro = new System.Windows.Forms.Panel();
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnMisVentas = new System.Windows.Forms.Button();
            this.dgvMisVentas = new System.Windows.Forms.DataGridView();
            this.pnlFiltro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMisVentas)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(30, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(280, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Reportes del Vendedor";
            //
            // pnlFiltro
            //
            this.pnlFiltro.BackColor = System.Drawing.Color.FromArgb(64, 196, 190);
            this.pnlFiltro.Controls.Add(this.lblDesde);
            this.pnlFiltro.Controls.Add(this.dtpDesde);
            this.pnlFiltro.Controls.Add(this.lblHasta);
            this.pnlFiltro.Controls.Add(this.dtpHasta);
            this.pnlFiltro.Controls.Add(this.btnMisVentas);
            this.pnlFiltro.Location = new System.Drawing.Point(30, 75);
            this.pnlFiltro.Name = "pnlFiltro";
            this.pnlFiltro.Size = new System.Drawing.Size(260, 130);
            this.pnlFiltro.TabIndex = 1;
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
            this.dtpDesde.Size = new System.Drawing.Size(100, 23);
            this.dtpDesde.TabIndex = 1;
            //
            // lblHasta
            //
            this.lblHasta.AutoSize = true;
            this.lblHasta.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHasta.ForeColor = System.Drawing.Color.White;
            this.lblHasta.Location = new System.Drawing.Point(135, 15);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(40, 15);
            this.lblHasta.TabIndex = 2;
            this.lblHasta.Text = "Hasta";
            //
            // dtpHasta
            //
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(135, 35);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(100, 23);
            this.dtpHasta.TabIndex = 3;
            //
            // btnMisVentas
            //
            this.btnMisVentas.BackColor = System.Drawing.Color.White;
            this.btnMisVentas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMisVentas.Location = new System.Drawing.Point(20, 75);
            this.btnMisVentas.Name = "btnMisVentas";
            this.btnMisVentas.Size = new System.Drawing.Size(220, 35);
            this.btnMisVentas.TabIndex = 4;
            this.btnMisVentas.Text = "Mis ventas";
            this.btnMisVentas.UseVisualStyleBackColor = false;
            this.btnMisVentas.Click += new System.EventHandler(this.BtnMisVentas_Click);
            //
            // dgvMisVentas
            //
            this.dgvMisVentas.AllowUserToAddRows = false;
            this.dgvMisVentas.AllowUserToDeleteRows = false;
            this.dgvMisVentas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMisVentas.Location = new System.Drawing.Point(30, 225);
            this.dgvMisVentas.Name = "dgvMisVentas";
            this.dgvMisVentas.ReadOnly = true;
            this.dgvMisVentas.RowHeadersVisible = false;
            this.dgvMisVentas.Size = new System.Drawing.Size(600, 300);
            this.dgvMisVentas.TabIndex = 2;
            //
            // FormReportesVendedor
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(660, 550);
            this.Controls.Add(this.dgvMisVentas);
            this.Controls.Add(this.pnlFiltro);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormReportesVendedor";
            this.Text = "FormReportesVendedor";
            this.pnlFiltro.ResumeLayout(false);
            this.pnlFiltro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMisVentas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlFiltro;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnMisVentas;
        private System.Windows.Forms.DataGridView dgvMisVentas;
    }
}
