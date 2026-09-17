using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormUsuarioDetalle
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
            pnlPrincipal = new Panel();
            pnlDatos = new Panel();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblDni = new Label();
            txtDni = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblCorreo = new Label();
            txtCorreo = new TextBox();
            lblSexo = new Label();
            cmbSexo = new ComboBox();
            lblFechaNacimiento = new Label();
            dtpFechaNacimiento = new DateTimePicker();
            lblPerfil = new Label();
            cmbPerfil = new ComboBox();
            lblSucursal = new Label();
            cmbSucursal = new ComboBox();
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            lblConfirmarContrasena = new Label();
            txtConfirmarContrasena = new TextBox();
            pnlAcciones = new Panel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            pnlCabecera = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaTitulo = new Panel();
            pnlPrincipal.SuspendLayout();
            pnlDatos.SuspendLayout();
            pnlAcciones.SuspendLayout();
            pnlCabecera.SuspendLayout();
            SuspendLayout();
            // 
            // pnlPrincipal
            // 
            pnlPrincipal.BackColor = Color.FromArgb(245, 246, 248);
            pnlPrincipal.Controls.Add(pnlDatos);
            pnlPrincipal.Controls.Add(pnlAcciones);
            pnlPrincipal.Controls.Add(pnlCabecera);
            pnlPrincipal.Dock = DockStyle.Fill;
            pnlPrincipal.Location = new Point(0, 0);
            pnlPrincipal.Name = "pnlPrincipal";
            pnlPrincipal.Padding = new Padding(32, 18, 32, 24);
            pnlPrincipal.Size = new Size(1180, 760);
            pnlPrincipal.TabIndex = 0;
            // 
            // pnlDatos
            // 
            pnlDatos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlDatos.BackColor = Color.White;
            pnlDatos.BorderStyle = BorderStyle.FixedSingle;
            pnlDatos.Controls.Add(lblNombre);
            pnlDatos.Controls.Add(txtNombre);
            pnlDatos.Controls.Add(lblApellido);
            pnlDatos.Controls.Add(txtApellido);
            pnlDatos.Controls.Add(lblDni);
            pnlDatos.Controls.Add(txtDni);
            pnlDatos.Controls.Add(lblTelefono);
            pnlDatos.Controls.Add(txtTelefono);
            pnlDatos.Controls.Add(lblUsuario);
            pnlDatos.Controls.Add(txtUsuario);
            pnlDatos.Controls.Add(lblCorreo);
            pnlDatos.Controls.Add(txtCorreo);
            pnlDatos.Controls.Add(lblSexo);
            pnlDatos.Controls.Add(cmbSexo);
            pnlDatos.Controls.Add(lblFechaNacimiento);
            pnlDatos.Controls.Add(dtpFechaNacimiento);
            pnlDatos.Controls.Add(lblPerfil);
            pnlDatos.Controls.Add(cmbPerfil);
            pnlDatos.Controls.Add(lblSucursal);
            pnlDatos.Controls.Add(cmbSucursal);
            pnlDatos.Controls.Add(lblContrasena);
            pnlDatos.Controls.Add(txtContrasena);
            pnlDatos.Controls.Add(lblConfirmarContrasena);
            pnlDatos.Controls.Add(txtConfirmarContrasena);
            pnlDatos.Location = new Point(32, 114);
            pnlDatos.Name = "pnlDatos";
            pnlDatos.Size = new Size(1116, 522);
            pnlDatos.TabIndex = 1;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(30, 61);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(64, 20);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(30, 88);
            txtNombre.MaxLength = 100;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(325, 27);
            txtNombre.TabIndex = 1;
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(387, 61);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(66, 20);
            lblApellido.TabIndex = 2;
            lblApellido.Text = "Apellido";
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(387, 88);
            txtApellido.MaxLength = 100;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(325, 27);
            txtApellido.TabIndex = 3;
            // 
            // lblDni
            // 
            lblDni.AutoSize = true;
            lblDni.Location = new Point(744, 61);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(35, 20);
            lblDni.TabIndex = 4;
            lblDni.Text = "DNI";
            // 
            // txtDni
            // 
            txtDni.Location = new Point(744, 88);
            txtDni.MaxLength = 20;
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(325, 27);
            txtDni.TabIndex = 5;
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Location = new Point(30, 164);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(67, 20);
            lblTelefono.TabIndex = 6;
            lblTelefono.Text = "Teléfono";
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(30, 191);
            txtTelefono.MaxLength = 30;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(325, 27);
            txtTelefono.TabIndex = 7;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(387, 164);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(137, 20);
            lblUsuario.TabIndex = 8;
            lblUsuario.Text = "Nombre de usuario";
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(387, 191);
            txtUsuario.MaxLength = 50;
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(325, 27);
            txtUsuario.TabIndex = 9;
            // 
            // lblCorreo
            // 
            lblCorreo.AutoSize = true;
            lblCorreo.Location = new Point(744, 164);
            lblCorreo.Name = "lblCorreo";
            lblCorreo.Size = new Size(54, 20);
            lblCorreo.TabIndex = 10;
            lblCorreo.Text = "Correo";
            // 
            // txtCorreo
            // 
            txtCorreo.Location = new Point(744, 191);
            txtCorreo.MaxLength = 150;
            txtCorreo.Name = "txtCorreo";
            txtCorreo.Size = new Size(325, 27);
            txtCorreo.TabIndex = 11;
            // 
            // lblSexo
            // 
            lblSexo.AutoSize = true;
            lblSexo.Location = new Point(30, 280);
            lblSexo.Name = "lblSexo";
            lblSexo.Size = new Size(41, 20);
            lblSexo.TabIndex = 12;
            lblSexo.Text = "Sexo";
            // 
            // cmbSexo
            // 
            cmbSexo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSexo.FormattingEnabled = true;
            cmbSexo.Location = new Point(30, 306);
            cmbSexo.Name = "cmbSexo";
            cmbSexo.Size = new Size(325, 28);
            cmbSexo.TabIndex = 13;
            // 
            // lblFechaNacimiento
            // 
            lblFechaNacimiento.AutoSize = true;
            lblFechaNacimiento.Location = new Point(387, 280);
            lblFechaNacimiento.Name = "lblFechaNacimiento";
            lblFechaNacimiento.Size = new Size(146, 20);
            lblFechaNacimiento.TabIndex = 14;
            lblFechaNacimiento.Text = "Fecha de nacimiento";
            // 
            // dtpFechaNacimiento
            // 
            dtpFechaNacimiento.Checked = false;
            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            dtpFechaNacimiento.Location = new Point(387, 307);
            dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            dtpFechaNacimiento.ShowCheckBox = true;
            dtpFechaNacimiento.Size = new Size(325, 27);
            dtpFechaNacimiento.TabIndex = 15;
            // 
            // lblPerfil
            // 
            lblPerfil.AutoSize = true;
            lblPerfil.Location = new Point(744, 280);
            lblPerfil.Name = "lblPerfil";
            lblPerfil.Size = new Size(42, 20);
            lblPerfil.TabIndex = 16;
            lblPerfil.Text = "Perfil";
            // 
            // cmbPerfil
            // 
            cmbPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPerfil.FormattingEnabled = true;
            cmbPerfil.Location = new Point(744, 307);
            cmbPerfil.Name = "cmbPerfil";
            cmbPerfil.Size = new Size(325, 28);
            cmbPerfil.TabIndex = 17;
            // 
            // lblSucursal
            // 
            lblSucursal.AutoSize = true;
            lblSucursal.Location = new Point(30, 393);
            lblSucursal.Name = "lblSucursal";
            lblSucursal.Size = new Size(63, 20);
            lblSucursal.TabIndex = 18;
            lblSucursal.Text = "Sucursal";
            // 
            // cmbSucursal
            // 
            cmbSucursal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSucursal.FormattingEnabled = true;
            cmbSucursal.Location = new Point(30, 420);
            cmbSucursal.Name = "cmbSucursal";
            cmbSucursal.Size = new Size(325, 28);
            cmbSucursal.TabIndex = 19;
            // 
            // lblContrasena
            // 
            lblContrasena.AutoSize = true;
            lblContrasena.Location = new Point(387, 393);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(83, 20);
            lblContrasena.TabIndex = 20;
            lblContrasena.Text = "Contraseña";
            // 
            // txtContrasena
            // 
            txtContrasena.Location = new Point(387, 420);
            txtContrasena.MaxLength = 100;
            txtContrasena.Name = "txtContrasena";
            txtContrasena.PasswordChar = '●';
            txtContrasena.Size = new Size(325, 27);
            txtContrasena.TabIndex = 21;
            // 
            // lblConfirmarContrasena
            // 
            lblConfirmarContrasena.AutoSize = true;
            lblConfirmarContrasena.Location = new Point(744, 393);
            lblConfirmarContrasena.Name = "lblConfirmarContrasena";
            lblConfirmarContrasena.Size = new Size(151, 20);
            lblConfirmarContrasena.TabIndex = 22;
            lblConfirmarContrasena.Text = "Confirmar contraseña";
            // 
            // txtConfirmarContrasena
            // 
            txtConfirmarContrasena.Location = new Point(744, 420);
            txtConfirmarContrasena.MaxLength = 100;
            txtConfirmarContrasena.Name = "txtConfirmarContrasena";
            txtConfirmarContrasena.PasswordChar = '●';
            txtConfirmarContrasena.Size = new Size(325, 27);
            txtConfirmarContrasena.TabIndex = 23;
            // 
            // pnlAcciones
            // 
            pnlAcciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlAcciones.Controls.Add(btnCancelar);
            pnlAcciones.Controls.Add(btnGuardar);
            pnlAcciones.Location = new Point(32, 642);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(1116, 86);
            pnlAcciones.TabIndex = 2;
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelar.BackColor = Color.White;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(160, 165, 170);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Location = new Point(876, 8);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 40);
            btnCancelar.TabIndex = 0;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGuardar.BackColor = Color.FromArgb(190, 137, 45);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(1002, 8);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 40);
            btnGuardar.TabIndex = 1;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
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
            lblTitulo.Size = new Size(275, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Nuevo usuario";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(2, 48);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(400, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Complete los datos del usuario y asigne su perfil y sucursal.";
            // 
            // pnlLineaTitulo
            // 
            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(2, 74);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(95, 3);
            pnlLineaTitulo.TabIndex = 2;
            // 
            // FormUsuarioDetalle
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1180, 760);
            Controls.Add(pnlPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormUsuarioDetalle";
            Text = "Detalle de usuario";
            pnlPrincipal.ResumeLayout(false);
            pnlDatos.ResumeLayout(false);
            pnlDatos.PerformLayout();
            pnlAcciones.ResumeLayout(false);
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

        private Panel pnlDatos;

        private Label lblNombre;
        private TextBox txtNombre;

        private Label lblApellido;
        private TextBox txtApellido;

        private Label lblDni;
        private TextBox txtDni;

        private Label lblTelefono;
        private TextBox txtTelefono;

        private Label lblUsuario;
        private TextBox txtUsuario;

        private Label lblCorreo;
        private TextBox txtCorreo;

        private Label lblSexo;
        private ComboBox cmbSexo;

        private Label lblFechaNacimiento;
        private DateTimePicker dtpFechaNacimiento;

        private Label lblPerfil;
        private ComboBox cmbPerfil;

        private Label lblSucursal;
        private ComboBox cmbSucursal;

        private Label lblContrasena;
        private TextBox txtContrasena;

        private Label lblConfirmarContrasena;
        private TextBox txtConfirmarContrasena;

        private Panel pnlAcciones;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}
