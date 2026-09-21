#nullable enable
namespace Capa_Vistas
{
    partial class FormMiPerfil
    {
        private System.ComponentModel.IContainer? components = null;
        private Panel pnlPrincipal = null!;
        private Label lblTitulo = null!;
        private Label lblSubtitulo = null!;
        private Panel pnlLinea = null!;
        private Label lblDatos = null!;
        private Label lblNombre = null!;
        private Label lblApellido = null!;
        private Label lblDni = null!;
        private Label lblTelefono = null!;
        private Label lblUsuario = null!;
        private Label lblCorreo = null!;
        private Label lblSexo = null!;
        private Label lblFecha = null!;
        private TextBox txtNombre = null!;
        private TextBox txtApellido = null!;
        private TextBox txtDni = null!;
        private TextBox txtTelefono = null!;
        private TextBox txtUsuario = null!;
        private TextBox txtCorreo = null!;
        private ComboBox cmbSexo = null!;
        private DateTimePicker dtpFecha = null!;
        private Label lblSeguridad = null!;
        private Label lblNueva = null!;
        private Label lblConfirmar = null!;
        private Label lblActual = null!;
        private TextBox txtNueva = null!;
        private TextBox txtConfirmar = null!;
        private TextBox txtActual = null!;
        private Button btnGuardar = null!;
        private Button btnCancelar = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlPrincipal = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLinea = new Panel();
            lblDatos = new Label();
            lblNombre = new Label();
            lblApellido = new Label();
            lblDni = new Label();
            lblTelefono = new Label();
            lblUsuario = new Label();
            lblCorreo = new Label();
            lblSexo = new Label();
            lblFecha = new Label();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtDni = new TextBox();
            txtTelefono = new TextBox();
            txtUsuario = new TextBox();
            txtCorreo = new TextBox();
            cmbSexo = new ComboBox();
            dtpFecha = new DateTimePicker();
            lblSeguridad = new Label();
            lblNueva = new Label();
            lblConfirmar = new Label();
            lblActual = new Label();
            txtNueva = new TextBox();
            txtConfirmar = new TextBox();
            txtActual = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            pnlPrincipal.SuspendLayout();
            SuspendLayout();
            //
            // pnlPrincipal
            //
            pnlPrincipal.AutoScroll = true;
            pnlPrincipal.BackColor = Color.FromArgb(24, 28, 33);
            pnlPrincipal.Controls.AddRange(new Control[] { lblTitulo, lblSubtitulo, pnlLinea, lblDatos, lblNombre, lblApellido, lblDni, lblTelefono, lblUsuario, lblCorreo, lblSexo, lblFecha, txtNombre, txtApellido, txtDni, txtTelefono, txtUsuario, txtCorreo, cmbSexo, dtpFecha, lblSeguridad, lblNueva, lblConfirmar, lblActual, txtNueva, txtConfirmar, txtActual, btnGuardar, btnCancelar });
            pnlPrincipal.Dock = DockStyle.Fill;
            pnlPrincipal.Name = "pnlPrincipal";
            pnlPrincipal.TabIndex = 0;
            //
            // Header
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(190, 137, 45);
            lblTitulo.Location = new Point(42, 22);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "Mi perfil";
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 10F);
            lblSubtitulo.ForeColor = Color.WhiteSmoke;
            lblSubtitulo.Location = new Point(44, 62);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Text = "Actualizá tus datos personales y la seguridad de tu cuenta.";
            pnlLinea.BackColor = Color.FromArgb(190, 137, 45);
            pnlLinea.Location = new Point(42, 96);
            pnlLinea.Name = "pnlLinea";
            pnlLinea.Size = new Size(500, 2);
            //
            // Labels and fields
            //
            lblDatos.Name = "lblDatos"; lblDatos.Text = "DATOS PERSONALES";
            lblNombre.Name = "lblNombre"; lblNombre.Text = "Nombre";
            lblApellido.Name = "lblApellido"; lblApellido.Text = "Apellido";
            lblDni.Name = "lblDni"; lblDni.Text = "DNI";
            lblTelefono.Name = "lblTelefono"; lblTelefono.Text = "Teléfono";
            lblUsuario.Name = "lblUsuario"; lblUsuario.Text = "Nombre de usuario";
            lblCorreo.Name = "lblCorreo"; lblCorreo.Text = "Correo electrónico";
            lblSexo.Name = "lblSexo"; lblSexo.Text = "Sexo";
            lblFecha.Name = "lblFecha"; lblFecha.Text = "Fecha de nacimiento";
            lblSeguridad.Name = "lblSeguridad"; lblSeguridad.Text = "SEGURIDAD DE LA CUENTA";
            lblNueva.Name = "lblNueva"; lblNueva.Text = "Nueva contraseña (opcional)";
            lblConfirmar.Name = "lblConfirmar"; lblConfirmar.Text = "Confirmar nueva contraseña";
            lblActual.Name = "lblActual"; lblActual.Text = "Contraseña actual *";
            txtNombre.Name = "txtNombre"; txtNombre.MaxLength = 100;
            txtApellido.Name = "txtApellido"; txtApellido.MaxLength = 100;
            txtDni.Name = "txtDni"; txtDni.MaxLength = 20;
            txtTelefono.Name = "txtTelefono"; txtTelefono.MaxLength = 30;
            txtUsuario.Name = "txtUsuario"; txtUsuario.MaxLength = 50;
            txtCorreo.Name = "txtCorreo"; txtCorreo.MaxLength = 150;
            txtNueva.Name = "txtNueva"; txtNueva.MaxLength = 100; txtNueva.PasswordChar = '●';
            txtConfirmar.Name = "txtConfirmar"; txtConfirmar.MaxLength = 100; txtConfirmar.PasswordChar = '●';
            txtActual.Name = "txtActual"; txtActual.MaxLength = 100; txtActual.PasswordChar = '●';
            cmbSexo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSexo.Font = new Font("Segoe UI", 10F);
            cmbSexo.Items.AddRange(new object[] { "No especificado", "Masculino", "Femenino", "Otro" });
            cmbSexo.SelectedIndex = 0;
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.ShowCheckBox = true;
            dtpFecha.Checked = false;
            dtpFecha.MaxDate = DateTime.Today;
            //
            // Actions
            //
            btnGuardar.BackColor = Color.FromArgb(190, 137, 45);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(160, 42);
            btnGuardar.Text = "Guardar cambios";
            btnGuardar.UseVisualStyleBackColor = false;
            btnCancelar.BackColor = Color.FromArgb(55, 60, 66);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 42);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            //
            // FormMiPerfil
            //
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 33);
            ClientSize = new Size(1200, 800);
            Controls.Add(pnlPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormMiPerfil";
            Text = "Mi perfil";
            pnlPrincipal.ResumeLayout(false);
            pnlPrincipal.PerformLayout();
            ResumeLayout(false);
        }
    }
}
