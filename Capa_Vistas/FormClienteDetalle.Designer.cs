namespace Capa_Vistas
{
    partial class FormClienteDetalle
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
            pnlPrincipal = new Panel();
            pnlContenedor = new Panel();
            pnlContenido = new Panel();
            btnVolver = new Button();
            lblTitulo = new Label();
            pnlLineaDorada = new Panel();
            lblDni = new Label();
            txtDni = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblProvincia = new Label();
            cmbProvincia = new ComboBox();
            lblLocalidad = new Label();
            cmbLocalidad = new ComboBox();
            lblCalle = new Label();
            txtCalle = new TextBox();
            lblAltura = new Label();
            txtAltura = new TextBox();
            lblPiso = new Label();
            txtPiso = new TextBox();
            btnCancelar = new Button();
            btnGuardar = new Button();
            pnlPrincipal.SuspendLayout();
            pnlContenedor.SuspendLayout();
            pnlContenido.SuspendLayout();
            SuspendLayout();
            //
            // pnlPrincipal
            //
            pnlPrincipal.BackColor = Color.FromArgb(244, 245, 247);
            pnlPrincipal.Controls.Add(pnlContenedor);
            pnlPrincipal.Dock = DockStyle.Fill;
            pnlPrincipal.Location = new Point(0, 0);
            pnlPrincipal.Name = "pnlPrincipal";
            pnlPrincipal.Size = new Size(1366, 881);
            pnlPrincipal.TabIndex = 0;
            //
            // pnlContenedor
            //
            pnlContenedor.AutoScroll = true;
            pnlContenedor.BackColor = Color.FromArgb(244, 245, 247);
            pnlContenedor.Controls.Add(pnlContenido);
            pnlContenedor.Dock = DockStyle.Fill;
            pnlContenedor.Location = new Point(0, 0);
            pnlContenedor.Name = "pnlContenedor";
            pnlContenedor.Size = new Size(1366, 881);
            pnlContenedor.TabIndex = 0;
            //
            // pnlContenido
            //
            pnlContenido.Anchor = AnchorStyles.Top;
            pnlContenido.BackColor = Color.White;
            pnlContenido.Controls.Add(btnVolver);
            pnlContenido.Controls.Add(lblTitulo);
            pnlContenido.Controls.Add(pnlLineaDorada);
            pnlContenido.Controls.Add(lblDni);
            pnlContenido.Controls.Add(txtDni);
            pnlContenido.Controls.Add(lblNombre);
            pnlContenido.Controls.Add(txtNombre);
            pnlContenido.Controls.Add(lblApellido);
            pnlContenido.Controls.Add(txtApellido);
            pnlContenido.Controls.Add(lblTelefono);
            pnlContenido.Controls.Add(txtTelefono);
            pnlContenido.Controls.Add(lblEmail);
            pnlContenido.Controls.Add(txtEmail);
            pnlContenido.Controls.Add(lblProvincia);
            pnlContenido.Controls.Add(cmbProvincia);
            pnlContenido.Controls.Add(lblLocalidad);
            pnlContenido.Controls.Add(cmbLocalidad);
            pnlContenido.Controls.Add(lblCalle);
            pnlContenido.Controls.Add(txtCalle);
            pnlContenido.Controls.Add(lblAltura);
            pnlContenido.Controls.Add(txtAltura);
            pnlContenido.Controls.Add(lblPiso);
            pnlContenido.Controls.Add(txtPiso);
            pnlContenido.Controls.Add(btnCancelar);
            pnlContenido.Controls.Add(btnGuardar);
            pnlContenido.Location = new Point(164, 44);
            pnlContenido.Name = "pnlContenido";
            pnlContenido.Size = new Size(980, 734);
            pnlContenido.TabIndex = 0;
            //
            // btnVolver
            //
            btnVolver.BackColor = Color.Transparent;
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 245, 245);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnVolver.ForeColor = Color.FromArgb(75, 80, 85);
            btnVolver.Location = new Point(45, 22);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(110, 35);
            btnVolver.TabIndex = 0;
            btnVolver.Text = "← Volver";
            btnVolver.TextAlign = ContentAlignment.MiddleLeft;
            btnVolver.UseVisualStyleBackColor = false;
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(27, 34, 42);
            lblTitulo.Location = new Point(65, 75);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(283, 54);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Nuevo cliente";
            //
            // pnlLineaDorada
            //
            pnlLineaDorada.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaDorada.Location = new Point(68, 132);
            pnlLineaDorada.Name = "pnlLineaDorada";
            pnlLineaDorada.Size = new Size(115, 4);
            pnlLineaDorada.TabIndex = 2;
            //
            // lblDni
            //
            lblDni.AutoSize = true;
            lblDni.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblDni.ForeColor = Color.FromArgb(55, 60, 66);
            lblDni.Location = new Point(68, 170);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(40, 21);
            lblDni.TabIndex = 3;
            lblDni.Text = "DNI";
            //
            // txtDni
            //
            txtDni.BorderStyle = BorderStyle.FixedSingle;
            txtDni.Font = new Font("Segoe UI", 10.5F);
            txtDni.Location = new Point(68, 198);
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(844, 31);
            txtDni.TabIndex = 0;
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblNombre.ForeColor = Color.FromArgb(55, 60, 66);
            lblNombre.Location = new Point(68, 255);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(73, 21);
            lblNombre.TabIndex = 4;
            lblNombre.Text = "Nombre";
            //
            // txtNombre
            //
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 10.5F);
            txtNombre.Location = new Point(68, 283);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(405, 31);
            txtNombre.TabIndex = 1;
            //
            // lblApellido
            //
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblApellido.ForeColor = Color.FromArgb(55, 60, 66);
            lblApellido.Location = new Point(507, 255);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(75, 21);
            lblApellido.TabIndex = 5;
            lblApellido.Text = "Apellido";
            //
            // txtApellido
            //
            txtApellido.BorderStyle = BorderStyle.FixedSingle;
            txtApellido.Font = new Font("Segoe UI", 10.5F);
            txtApellido.Location = new Point(507, 283);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(405, 31);
            txtApellido.TabIndex = 2;
            //
            // lblTelefono
            //
            lblTelefono.AutoSize = true;
            lblTelefono.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTelefono.ForeColor = Color.FromArgb(55, 60, 66);
            lblTelefono.Location = new Point(68, 345);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(77, 21);
            lblTelefono.TabIndex = 6;
            lblTelefono.Text = "Teléfono";
            //
            // txtTelefono
            //
            txtTelefono.BorderStyle = BorderStyle.FixedSingle;
            txtTelefono.Font = new Font("Segoe UI", 10.5F);
            txtTelefono.Location = new Point(68, 373);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(405, 31);
            txtTelefono.TabIndex = 3;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(55, 60, 66);
            lblEmail.Location = new Point(507, 345);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(53, 21);
            lblEmail.TabIndex = 7;
            lblEmail.Text = "Email";
            //
            // txtEmail
            //
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 10.5F);
            txtEmail.Location = new Point(507, 373);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(405, 31);
            txtEmail.TabIndex = 4;
            //
            // lblProvincia
            //
            lblProvincia.AutoSize = true;
            lblProvincia.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblProvincia.ForeColor = Color.FromArgb(55, 60, 66);
            lblProvincia.Location = new Point(68, 435);
            lblProvincia.Name = "lblProvincia";
            lblProvincia.Size = new Size(82, 21);
            lblProvincia.TabIndex = 8;
            lblProvincia.Text = "Provincia";
            //
            // cmbProvincia
            //
            cmbProvincia.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProvincia.Font = new Font("Segoe UI", 10.5F);
            cmbProvincia.Location = new Point(68, 463);
            cmbProvincia.Name = "cmbProvincia";
            cmbProvincia.Size = new Size(405, 31);
            cmbProvincia.TabIndex = 5;
            //
            // lblLocalidad
            //
            lblLocalidad.AutoSize = true;
            lblLocalidad.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblLocalidad.ForeColor = Color.FromArgb(55, 60, 66);
            lblLocalidad.Location = new Point(507, 435);
            lblLocalidad.Name = "lblLocalidad";
            lblLocalidad.Size = new Size(84, 21);
            lblLocalidad.TabIndex = 9;
            lblLocalidad.Text = "Localidad";
            //
            // cmbLocalidad
            //
            cmbLocalidad.DropDownStyle = ComboBoxStyle.DropDown;
            cmbLocalidad.Font = new Font("Segoe UI", 10.5F);
            cmbLocalidad.Location = new Point(507, 463);
            cmbLocalidad.Name = "cmbLocalidad";
            cmbLocalidad.Size = new Size(405, 31);
            cmbLocalidad.TabIndex = 6;
            //
            // lblCalle
            //
            lblCalle.AutoSize = true;
            lblCalle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCalle.ForeColor = Color.FromArgb(55, 60, 66);
            lblCalle.Location = new Point(68, 525);
            lblCalle.Name = "lblCalle";
            lblCalle.Size = new Size(45, 21);
            lblCalle.TabIndex = 10;
            lblCalle.Text = "Calle";
            //
            // txtCalle
            //
            txtCalle.BorderStyle = BorderStyle.FixedSingle;
            txtCalle.Font = new Font("Segoe UI", 10.5F);
            txtCalle.Location = new Point(68, 553);
            txtCalle.Name = "txtCalle";
            txtCalle.Size = new Size(500, 31);
            txtCalle.TabIndex = 7;
            //
            // lblAltura
            //
            lblAltura.AutoSize = true;
            lblAltura.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblAltura.ForeColor = Color.FromArgb(55, 60, 66);
            lblAltura.Location = new Point(590, 525);
            lblAltura.Name = "lblAltura";
            lblAltura.Size = new Size(52, 21);
            lblAltura.TabIndex = 11;
            lblAltura.Text = "Altura";
            //
            // txtAltura
            //
            txtAltura.BorderStyle = BorderStyle.FixedSingle;
            txtAltura.Font = new Font("Segoe UI", 10.5F);
            txtAltura.Location = new Point(590, 553);
            txtAltura.Name = "txtAltura";
            txtAltura.Size = new Size(160, 31);
            txtAltura.TabIndex = 8;
            //
            // lblPiso
            //
            lblPiso.AutoSize = true;
            lblPiso.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblPiso.ForeColor = Color.FromArgb(55, 60, 66);
            lblPiso.Location = new Point(770, 525);
            lblPiso.Name = "lblPiso";
            lblPiso.Size = new Size(105, 21);
            lblPiso.TabIndex = 12;
            lblPiso.Text = "Piso (opcional)";
            //
            // txtPiso
            //
            txtPiso.BorderStyle = BorderStyle.FixedSingle;
            txtPiso.Font = new Font("Segoe UI", 10.5F);
            txtPiso.Location = new Point(770, 553);
            txtPiso.Name = "txtPiso";
            txtPiso.Size = new Size(142, 31);
            txtPiso.TabIndex = 9;
            //
            // btnCancelar
            //
            btnCancelar.BackColor = Color.FromArgb(242, 243, 245);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(205, 205, 205);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9.5F);
            btnCancelar.ForeColor = Color.FromArgb(45, 50, 55);
            btnCancelar.Location = new Point(68, 659);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(205, 42);
            btnCancelar.TabIndex = 10;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            //
            // btnGuardar
            //
            btnGuardar.BackColor = Color.FromArgb(190, 137, 45);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.MouseOverBackColor = Color.FromArgb(210, 153, 50);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(707, 659);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(205, 42);
            btnGuardar.TabIndex = 11;
            btnGuardar.Text = "Guardar cliente";
            btnGuardar.UseVisualStyleBackColor = false;
            //
            // FormClienteDetalle
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(1366, 881);
            Controls.Add(pnlPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormClienteDetalle";
            Text = "Detalle de cliente";
            pnlPrincipal.ResumeLayout(false);
            pnlContenedor.ResumeLayout(false);
            pnlContenido.ResumeLayout(false);
            pnlContenido.PerformLayout();
            ResumeLayout(false);
        }

        #endregion


        private Panel pnlPrincipal;
        private Panel pnlContenedor;
        private Panel pnlContenido;

        private Button btnVolver;

        private Label lblTitulo;
        private Panel pnlLineaDorada;

        private Label lblDni;
        private TextBox txtDni;

        private Label lblNombre;
        private TextBox txtNombre;

        private Label lblApellido;
        private TextBox txtApellido;

        private Label lblTelefono;
        private TextBox txtTelefono;

        private Label lblEmail;
        private TextBox txtEmail;

        private Label lblProvincia;
        private ComboBox cmbProvincia;

        private Label lblLocalidad;
        private ComboBox cmbLocalidad;

        private Label lblCalle;
        private TextBox txtCalle;
        private Label lblAltura;
        private TextBox txtAltura;
        private Label lblPiso;
        private TextBox txtPiso;

        private Button btnCancelar;
        private Button btnGuardar;
    }
}
