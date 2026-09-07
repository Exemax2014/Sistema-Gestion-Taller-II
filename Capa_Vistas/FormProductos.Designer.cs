namespace Capa_Vistas
{
    partial class FormProductos
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
            this.pnlAlta = new System.Windows.Forms.Panel();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.cmbCategoria = new System.Windows.Forms.ComboBox();
            this.lblCodigoBarra = new System.Windows.Forms.Label();
            this.txtCodigoBarra = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblPrecioCosto = new System.Windows.Forms.Label();
            this.txtPrecioCosto = new System.Windows.Forms.TextBox();
            this.lblPorcentajeGanancia = new System.Windows.Forms.Label();
            this.txtPorcentajeGanancia = new System.Windows.Forms.TextBox();
            this.btnAlta = new System.Windows.Forms.Button();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.pnlAlta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(30, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(150, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Productos";
            //
            // pnlAlta
            //
            this.pnlAlta.BackColor = System.Drawing.Color.FromArgb(64, 196, 190);
            this.pnlAlta.Controls.Add(this.lblCategoria);
            this.pnlAlta.Controls.Add(this.cmbCategoria);
            this.pnlAlta.Controls.Add(this.lblCodigoBarra);
            this.pnlAlta.Controls.Add(this.txtCodigoBarra);
            this.pnlAlta.Controls.Add(this.lblNombre);
            this.pnlAlta.Controls.Add(this.txtNombre);
            this.pnlAlta.Controls.Add(this.lblDescripcion);
            this.pnlAlta.Controls.Add(this.txtDescripcion);
            this.pnlAlta.Controls.Add(this.lblPrecioCosto);
            this.pnlAlta.Controls.Add(this.txtPrecioCosto);
            this.pnlAlta.Controls.Add(this.lblPorcentajeGanancia);
            this.pnlAlta.Controls.Add(this.txtPorcentajeGanancia);
            this.pnlAlta.Controls.Add(this.btnAlta);
            this.pnlAlta.Location = new System.Drawing.Point(30, 75);
            this.pnlAlta.Name = "pnlAlta";
            this.pnlAlta.Size = new System.Drawing.Size(600, 200);
            this.pnlAlta.TabIndex = 1;
            //
            // lblCategoria
            //
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCategoria.ForeColor = System.Drawing.Color.White;
            this.lblCategoria.Location = new System.Drawing.Point(20, 15);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(66, 15);
            this.lblCategoria.TabIndex = 0;
            this.lblCategoria.Text = "Categoría";
            //
            // cmbCategoria
            //
            this.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoria.Location = new System.Drawing.Point(20, 35);
            this.cmbCategoria.Name = "cmbCategoria";
            this.cmbCategoria.Size = new System.Drawing.Size(180, 23);
            this.cmbCategoria.TabIndex = 1;
            //
            // lblCodigoBarra
            //
            this.lblCodigoBarra.AutoSize = true;
            this.lblCodigoBarra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCodigoBarra.ForeColor = System.Drawing.Color.White;
            this.lblCodigoBarra.Location = new System.Drawing.Point(220, 15);
            this.lblCodigoBarra.Name = "lblCodigoBarra";
            this.lblCodigoBarra.Size = new System.Drawing.Size(93, 15);
            this.lblCodigoBarra.TabIndex = 2;
            this.lblCodigoBarra.Text = "Código de barra";
            //
            // txtCodigoBarra
            //
            this.txtCodigoBarra.Location = new System.Drawing.Point(220, 35);
            this.txtCodigoBarra.Name = "txtCodigoBarra";
            this.txtCodigoBarra.Size = new System.Drawing.Size(150, 23);
            this.txtCodigoBarra.TabIndex = 3;
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNombre.ForeColor = System.Drawing.Color.White;
            this.lblNombre.Location = new System.Drawing.Point(390, 15);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(55, 15);
            this.lblNombre.TabIndex = 4;
            this.lblNombre.Text = "Nombre";
            //
            // txtNombre
            //
            this.txtNombre.Location = new System.Drawing.Point(390, 35);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(190, 23);
            this.txtNombre.TabIndex = 5;
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescripcion.ForeColor = System.Drawing.Color.White;
            this.lblDescripcion.Location = new System.Drawing.Point(20, 75);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(78, 15);
            this.lblDescripcion.TabIndex = 6;
            this.lblDescripcion.Text = "Descripción";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(20, 95);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(350, 23);
            this.txtDescripcion.TabIndex = 7;
            //
            // lblPrecioCosto
            //
            this.lblPrecioCosto.AutoSize = true;
            this.lblPrecioCosto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPrecioCosto.ForeColor = System.Drawing.Color.White;
            this.lblPrecioCosto.Location = new System.Drawing.Point(390, 75);
            this.lblPrecioCosto.Name = "lblPrecioCosto";
            this.lblPrecioCosto.Size = new System.Drawing.Size(86, 15);
            this.lblPrecioCosto.TabIndex = 8;
            this.lblPrecioCosto.Text = "Precio de costo";
            //
            // txtPrecioCosto
            //
            this.txtPrecioCosto.Location = new System.Drawing.Point(390, 95);
            this.txtPrecioCosto.Name = "txtPrecioCosto";
            this.txtPrecioCosto.Size = new System.Drawing.Size(90, 23);
            this.txtPrecioCosto.TabIndex = 9;
            //
            // lblPorcentajeGanancia
            //
            this.lblPorcentajeGanancia.AutoSize = true;
            this.lblPorcentajeGanancia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPorcentajeGanancia.ForeColor = System.Drawing.Color.White;
            this.lblPorcentajeGanancia.Location = new System.Drawing.Point(490, 75);
            this.lblPorcentajeGanancia.Name = "lblPorcentajeGanancia";
            this.lblPorcentajeGanancia.Size = new System.Drawing.Size(96, 15);
            this.lblPorcentajeGanancia.TabIndex = 10;
            this.lblPorcentajeGanancia.Text = "% Ganancia";
            //
            // txtPorcentajeGanancia
            //
            this.txtPorcentajeGanancia.Location = new System.Drawing.Point(490, 95);
            this.txtPorcentajeGanancia.Name = "txtPorcentajeGanancia";
            this.txtPorcentajeGanancia.Size = new System.Drawing.Size(90, 23);
            this.txtPorcentajeGanancia.TabIndex = 11;
            //
            // btnAlta
            //
            this.btnAlta.BackColor = System.Drawing.Color.White;
            this.btnAlta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlta.Location = new System.Drawing.Point(20, 140);
            this.btnAlta.Name = "btnAlta";
            this.btnAlta.Size = new System.Drawing.Size(180, 40);
            this.btnAlta.TabIndex = 12;
            this.btnAlta.Text = "Agregar producto";
            this.btnAlta.UseVisualStyleBackColor = false;
            this.btnAlta.Click += new System.EventHandler(this.BtnAlta_Click);
            //
            // dgvProductos
            //
            this.dgvProductos.AllowUserToAddRows = false;
            this.dgvProductos.AllowUserToDeleteRows = false;
            this.dgvProductos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProductos.Location = new System.Drawing.Point(30, 300);
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.ReadOnly = true;
            this.dgvProductos.RowHeadersVisible = false;
            this.dgvProductos.Size = new System.Drawing.Size(600, 280);
            this.dgvProductos.TabIndex = 2;
            //
            // FormProductos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(660, 610);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.pnlAlta);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormProductos";
            this.Text = "FormProductos";
            this.pnlAlta.ResumeLayout(false);
            this.pnlAlta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlAlta;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.ComboBox cmbCategoria;
        private System.Windows.Forms.Label lblCodigoBarra;
        private System.Windows.Forms.TextBox txtCodigoBarra;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblPrecioCosto;
        private System.Windows.Forms.TextBox txtPrecioCosto;
        private System.Windows.Forms.Label lblPorcentajeGanancia;
        private System.Windows.Forms.TextBox txtPorcentajeGanancia;
        private System.Windows.Forms.Button btnAlta;
        private System.Windows.Forms.DataGridView dgvProductos;
    }
}