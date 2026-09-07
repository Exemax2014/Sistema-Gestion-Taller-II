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

            lblDireccion = new Label();
            txtDireccion = new TextBox();

            btnCancelar = new Button();
            btnGuardar = new Button();

            pnlPrincipal.SuspendLayout();
            pnlContenido.SuspendLayout();

            SuspendLayout();

            // =====================================================
            // FORM
            // =====================================================

            AutoScaleDimensions =
                new SizeF(8F, 20F);

            AutoScaleMode =
                AutoScaleMode.Font;

            BackColor =
                Color.FromArgb(244, 245, 247);

            ClientSize =
                new Size(1200, 720);

            Controls.Add(pnlPrincipal);

            FormBorderStyle =
                FormBorderStyle.None;

            Name =
                "FormClienteDetalle";

            Text =
                "Detalle de cliente";


            // =====================================================
            // PANEL PRINCIPAL
            // =====================================================

            pnlPrincipal.BackColor =
                Color.FromArgb(244, 245, 247);

            pnlPrincipal.Controls.Add(pnlContenido);

            pnlPrincipal.Dock =
                DockStyle.Fill;

            pnlPrincipal.Name =
                "pnlPrincipal";


            // =====================================================
            // PANEL CONTENIDO
            // =====================================================

            pnlContenido.Anchor =
                AnchorStyles.Top;

            pnlContenido.BackColor =
                Color.White;

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

            pnlContenido.Controls.Add(lblDireccion);
            pnlContenido.Controls.Add(txtDireccion);

            pnlContenido.Controls.Add(btnCancelar);
            pnlContenido.Controls.Add(btnGuardar);

            pnlContenido.Location =
                new Point(110, 30);

            pnlContenido.Name =
                "pnlContenido";

            pnlContenido.Size =
                new Size(980, 650);


            // =====================================================
            // VOLVER
            // =====================================================

            btnVolver.BackColor =
                Color.Transparent;

            btnVolver.Cursor =
                Cursors.Hand;

            btnVolver.FlatAppearance.BorderSize =
                0;

            btnVolver.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(245, 245, 245);

            btnVolver.FlatStyle =
                FlatStyle.Flat;

            btnVolver.Font =
                new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Bold
                );

            btnVolver.ForeColor =
                Color.FromArgb(75, 80, 85);

            btnVolver.Location =
                new Point(45, 22);

            btnVolver.Name =
                "btnVolver";

            btnVolver.Size =
                new Size(110, 35);

            btnVolver.Text =
                "← Volver";

            btnVolver.TextAlign =
                ContentAlignment.MiddleLeft;

            btnVolver.UseVisualStyleBackColor =
                false;


            // =====================================================
            // TITULO
            // =====================================================

            lblTitulo.AutoSize =
                true;

            lblTitulo.Font =
                new Font(
                    "Segoe UI",
                    24F,
                    FontStyle.Bold
                );

            lblTitulo.ForeColor =
                Color.FromArgb(27, 34, 42);

            lblTitulo.Location =
                new Point(65, 75);

            lblTitulo.Name =
                "lblTitulo";

            lblTitulo.Text =
                "Nuevo cliente";


            // =====================================================
            // LINEA DORADA
            // =====================================================

            pnlLineaDorada.BackColor =
                Color.FromArgb(190, 137, 45);

            pnlLineaDorada.Location =
                new Point(68, 132);

            pnlLineaDorada.Name =
                "pnlLineaDorada";

            pnlLineaDorada.Size =
                new Size(115, 4);


            // =====================================================
            // DNI
            // =====================================================

            lblDni.AutoSize =
                true;

            lblDni.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            lblDni.ForeColor =
                Color.FromArgb(55, 60, 66);

            lblDni.Location =
                new Point(68, 170);

            lblDni.Name =
                "lblDni";

            lblDni.Text =
                "DNI";


            txtDni.BorderStyle =
                BorderStyle.FixedSingle;

            txtDni.Font =
                new Font(
                    "Segoe UI",
                    10.5F
                );

            txtDni.Location =
                new Point(68, 198);

            txtDni.Name =
                "txtDni";

            txtDni.Size =
                new Size(844, 31);

            txtDni.TabIndex =
                0;


            // =====================================================
            // NOMBRE
            // =====================================================

            lblNombre.AutoSize =
                true;

            lblNombre.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            lblNombre.ForeColor =
                Color.FromArgb(55, 60, 66);

            lblNombre.Location =
                new Point(68, 255);

            lblNombre.Name =
                "lblNombre";

            lblNombre.Text =
                "Nombre";


            txtNombre.BorderStyle =
                BorderStyle.FixedSingle;

            txtNombre.Font =
                new Font(
                    "Segoe UI",
                    10.5F
                );

            txtNombre.Location =
                new Point(68, 283);

            txtNombre.Name =
                "txtNombre";

            txtNombre.Size =
                new Size(405, 31);

            txtNombre.TabIndex =
                1;


            // =====================================================
            // APELLIDO
            // =====================================================

            lblApellido.AutoSize =
                true;

            lblApellido.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            lblApellido.ForeColor =
                Color.FromArgb(55, 60, 66);

            lblApellido.Location =
                new Point(507, 255);

            lblApellido.Name =
                "lblApellido";

            lblApellido.Text =
                "Apellido";


            txtApellido.BorderStyle =
                BorderStyle.FixedSingle;

            txtApellido.Font =
                new Font(
                    "Segoe UI",
                    10.5F
                );

            txtApellido.Location =
                new Point(507, 283);

            txtApellido.Name =
                "txtApellido";

            txtApellido.Size =
                new Size(405, 31);

            txtApellido.TabIndex =
                2;


            // =====================================================
            // TELEFONO
            // =====================================================

            lblTelefono.AutoSize =
                true;

            lblTelefono.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            lblTelefono.ForeColor =
                Color.FromArgb(55, 60, 66);

            lblTelefono.Location =
                new Point(68, 345);

            lblTelefono.Name =
                "lblTelefono";

            lblTelefono.Text =
                "Teléfono";


            txtTelefono.BorderStyle =
                BorderStyle.FixedSingle;

            txtTelefono.Font =
                new Font(
                    "Segoe UI",
                    10.5F
                );

            txtTelefono.Location =
                new Point(68, 373);

            txtTelefono.Name =
                "txtTelefono";

            txtTelefono.Size =
                new Size(405, 31);

            txtTelefono.TabIndex =
                3;


            // =====================================================
            // EMAIL
            // =====================================================

            lblEmail.AutoSize =
                true;

            lblEmail.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            lblEmail.ForeColor =
                Color.FromArgb(55, 60, 66);

            lblEmail.Location =
                new Point(507, 345);

            lblEmail.Name =
                "lblEmail";

            lblEmail.Text =
                "Email";


            txtEmail.BorderStyle =
                BorderStyle.FixedSingle;

            txtEmail.Font =
                new Font(
                    "Segoe UI",
                    10.5F
                );

            txtEmail.Location =
                new Point(507, 373);

            txtEmail.Name =
                "txtEmail";

            txtEmail.Size =
                new Size(405, 31);

            txtEmail.TabIndex =
                4;


            // =====================================================
            // PROVINCIA
            // =====================================================

            lblProvincia.AutoSize =
                true;

            lblProvincia.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            lblProvincia.ForeColor =
                Color.FromArgb(55, 60, 66);

            lblProvincia.Location =
                new Point(68, 435);

            lblProvincia.Name =
                "lblProvincia";

            lblProvincia.Text =
                "Provincia";


            cmbProvincia.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbProvincia.Font =
                new Font(
                    "Segoe UI",
                    10.5F
                );

            cmbProvincia.Location =
                new Point(68, 463);

            cmbProvincia.Name =
                "cmbProvincia";

            cmbProvincia.Size =
                new Size(405, 31);

            cmbProvincia.TabIndex =
                5;


            // =====================================================
            // LOCALIDAD
            // =====================================================

            lblLocalidad.AutoSize =
                true;

            lblLocalidad.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            lblLocalidad.ForeColor =
                Color.FromArgb(55, 60, 66);

            lblLocalidad.Location =
                new Point(507, 435);

            lblLocalidad.Name =
                "lblLocalidad";

            lblLocalidad.Text =
                "Localidad";


            cmbLocalidad.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbLocalidad.Font =
                new Font(
                    "Segoe UI",
                    10.5F
                );

            cmbLocalidad.Location =
                new Point(507, 463);

            cmbLocalidad.Name =
                "cmbLocalidad";

            cmbLocalidad.Size =
                new Size(405, 31);

            cmbLocalidad.TabIndex =
                6;


            // =====================================================
            // DIRECCION
            // =====================================================

            lblDireccion.AutoSize =
                true;

            lblDireccion.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            lblDireccion.ForeColor =
                Color.FromArgb(55, 60, 66);

            lblDireccion.Location =
                new Point(68, 525);

            lblDireccion.Name =
                "lblDireccion";

            lblDireccion.Text =
                "Dirección";


            txtDireccion.BorderStyle =
                BorderStyle.FixedSingle;

            txtDireccion.Font =
                new Font(
                    "Segoe UI",
                    10.5F
                );

            txtDireccion.Location =
                new Point(68, 553);

            txtDireccion.Name =
                "txtDireccion";

            txtDireccion.Size =
                new Size(844, 31);

            txtDireccion.TabIndex =
                7;


            // =====================================================
            // CANCELAR
            // =====================================================

            btnCancelar.BackColor =
                Color.FromArgb(242, 243, 245);

            btnCancelar.Cursor =
                Cursors.Hand;

            btnCancelar.FlatAppearance.BorderColor =
                Color.FromArgb(205, 205, 205);

            btnCancelar.FlatStyle =
                FlatStyle.Flat;

            btnCancelar.Font =
                new Font(
                    "Segoe UI",
                    9.5F
                );

            btnCancelar.ForeColor =
                Color.FromArgb(45, 50, 55);

            btnCancelar.Location =
                new Point(68, 605);

            btnCancelar.Name =
                "btnCancelar";

            btnCancelar.Size =
                new Size(205, 42);

            btnCancelar.TabIndex =
                9;

            btnCancelar.Text =
                "Cancelar";

            btnCancelar.UseVisualStyleBackColor =
                false;


            // =====================================================
            // GUARDAR
            // =====================================================

            btnGuardar.BackColor =
                Color.FromArgb(190, 137, 45);

            btnGuardar.Cursor =
                Cursors.Hand;

            btnGuardar.FlatAppearance.BorderSize =
                0;

            btnGuardar.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(210, 153, 50);

            btnGuardar.FlatStyle =
                FlatStyle.Flat;

            btnGuardar.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            btnGuardar.ForeColor =
                Color.White;

            btnGuardar.Location =
                new Point(707, 605);

            btnGuardar.Name =
                "btnGuardar";

            btnGuardar.Size =
                new Size(205, 42);

            btnGuardar.TabIndex =
                8;

            btnGuardar.Text =
                "Guardar cliente";

            btnGuardar.UseVisualStyleBackColor =
                false;


            // =====================================================
            // FINAL
            // =====================================================

            pnlContenido.ResumeLayout(false);
            pnlContenido.PerformLayout();

            pnlPrincipal.ResumeLayout(false);

            ResumeLayout(false);
        }

        #endregion


        private Panel pnlPrincipal;
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

        private Label lblDireccion;
        private TextBox txtDireccion;

        private Button btnCancelar;
        private Button btnGuardar;
    }
}