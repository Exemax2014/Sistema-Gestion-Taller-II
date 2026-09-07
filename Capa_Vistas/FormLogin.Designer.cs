namespace Capa_Vistas
{
    partial class FormLogin
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
            pnlIzquierda = new Panel();
            picLogoCompleto = new PictureBox();

            pnlDerecha = new Panel();

            btnCerrar = new Button();
            btnMinimizar = new Button();

            lblTitulo = new Label();
            pnlLineaDorada = new Panel();
            lblSubtitulo = new Label();

            lblUsuario = new Label();
            txtUsuario = new TextBox();

            lblContrasena = new Label();
            txtContrasena = new TextBox();

            btnIngresar = new Button();

            pnlIzquierda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoCompleto).BeginInit();

            pnlDerecha.SuspendLayout();

            SuspendLayout();


            // =====================================================
            // FORM LOGIN
            // =====================================================

            AutoScaleDimensions =
                new SizeF(8F, 20F);

            AutoScaleMode =
                AutoScaleMode.Font;

            BackColor =
                Color.FromArgb(17, 21, 26);

            ClientSize =
                new Size(1040, 390);

            Controls.Add(pnlDerecha);
            Controls.Add(pnlIzquierda);

            FormBorderStyle =
                FormBorderStyle.None;

            MaximizeBox =
                false;

            MinimizeBox =
                false;

            Name =
                "FormLogin";

            StartPosition =
                FormStartPosition.CenterScreen;

            Text =
                "Hierro y Forja";


            // =====================================================
            // PANEL IZQUIERDO
            // =====================================================

            pnlIzquierda.BackColor =
                Color.FromArgb(17, 21, 26);

            pnlIzquierda.Controls.Add(
                picLogoCompleto
            );

            pnlIzquierda.Dock =
                DockStyle.Left;

            pnlIzquierda.Location =
                new Point(0, 0);

            pnlIzquierda.Name =
                "pnlIzquierda";

            pnlIzquierda.Size =
                new Size(560, 390);

            pnlIzquierda.TabIndex =
                0;


            // =====================================================
            // LOGO COMPLETO
            // =====================================================

            picLogoCompleto.BackColor =
                Color.Transparent;

            picLogoCompleto.Image =
                Properties.Resource.LogoHierroForja;

            picLogoCompleto.Location =
                new Point(28, 22);

            picLogoCompleto.Name =
                "picLogoCompleto";

            picLogoCompleto.Size =
                new Size(505, 345);

            picLogoCompleto.SizeMode =
                PictureBoxSizeMode.Zoom;

            picLogoCompleto.TabIndex =
                0;

            picLogoCompleto.TabStop =
                false;


            // =====================================================
            // PANEL DERECHO
            // =====================================================

            pnlDerecha.BackColor =
                Color.White;

            pnlDerecha.Controls.Add(btnCerrar);
            pnlDerecha.Controls.Add(btnMinimizar);

            pnlDerecha.Controls.Add(lblTitulo);
            pnlDerecha.Controls.Add(pnlLineaDorada);
            pnlDerecha.Controls.Add(lblSubtitulo);

            pnlDerecha.Controls.Add(lblUsuario);
            pnlDerecha.Controls.Add(txtUsuario);

            pnlDerecha.Controls.Add(lblContrasena);
            pnlDerecha.Controls.Add(txtContrasena);

            pnlDerecha.Controls.Add(btnIngresar);

            pnlDerecha.Dock =
                DockStyle.Fill;

            pnlDerecha.Location =
                new Point(560, 0);

            pnlDerecha.Name =
                "pnlDerecha";

            pnlDerecha.Size =
                new Size(480, 390);

            pnlDerecha.TabIndex =
                1;


            // =====================================================
            // MINIMIZAR
            // =====================================================

            btnMinimizar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnMinimizar.BackColor =
                Color.Transparent;

            btnMinimizar.Cursor =
                Cursors.Hand;

            btnMinimizar.FlatAppearance.BorderSize =
                0;

            btnMinimizar.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(235, 235, 235);

            btnMinimizar.FlatStyle =
                FlatStyle.Flat;

            btnMinimizar.Font =
                new Font(
                    "Segoe UI Symbol",
                    11F
                );

            btnMinimizar.ForeColor =
                Color.FromArgb(40, 40, 40);

            btnMinimizar.Location =
                new Point(392, 0);

            btnMinimizar.Name =
                "btnMinimizar";

            btnMinimizar.Size =
                new Size(42, 32);

            btnMinimizar.TabIndex =
                10;

            btnMinimizar.Text =
                "─";

            btnMinimizar.UseVisualStyleBackColor =
                false;


            // =====================================================
            // CERRAR
            // =====================================================

            btnCerrar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnCerrar.BackColor =
                Color.Transparent;

            btnCerrar.Cursor =
                Cursors.Hand;

            btnCerrar.FlatAppearance.BorderSize =
                0;

            btnCerrar.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(180, 50, 50);

            btnCerrar.FlatStyle =
                FlatStyle.Flat;

            btnCerrar.Font =
                new Font(
                    "Segoe UI",
                    12F
                );

            btnCerrar.ForeColor =
                Color.FromArgb(40, 40, 40);

            btnCerrar.Location =
                new Point(435, 0);

            btnCerrar.Name =
                "btnCerrar";

            btnCerrar.Size =
                new Size(45, 32);

            btnCerrar.TabIndex =
                11;

            btnCerrar.Text =
                "×";

            btnCerrar.UseVisualStyleBackColor =
                false;


            // =====================================================
            // TITULO
            // =====================================================

            lblTitulo.AutoSize =
                true;

            lblTitulo.Font =
                new Font(
                    "Segoe UI",
                    20F,
                    FontStyle.Bold
                );

            lblTitulo.ForeColor =
                Color.FromArgb(27, 34, 42);

            lblTitulo.Location =
                new Point(70, 45);

            lblTitulo.Name =
                "lblTitulo";

            lblTitulo.Text =
                "Iniciar sesión";


            // =====================================================
            // LINEA DORADA
            // =====================================================

            pnlLineaDorada.BackColor =
                Color.FromArgb(190, 137, 45);

            pnlLineaDorada.Location =
                new Point(72, 95);

            pnlLineaDorada.Name =
                "pnlLineaDorada";

            pnlLineaDorada.Size =
                new Size(90, 3);


            // =====================================================
            // SUBTITULO
            // =====================================================

            lblSubtitulo.AutoSize =
                true;

            lblSubtitulo.Font =
                new Font(
                    "Segoe UI",
                    9.2F
                );

            lblSubtitulo.ForeColor =
                Color.FromArgb(100, 106, 113);

            lblSubtitulo.Location =
                new Point(72, 112);

            lblSubtitulo.Name =
                "lblSubtitulo";

            lblSubtitulo.Text =
                "Ingresá tus credenciales para acceder al sistema.";


            // =====================================================
            // USUARIO
            // =====================================================

            lblUsuario.AutoSize =
                true;

            lblUsuario.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold
                );

            lblUsuario.ForeColor =
                Color.FromArgb(55, 60, 66);

            lblUsuario.Location =
                new Point(72, 162);

            lblUsuario.Name =
                "lblUsuario";

            lblUsuario.Text =
                "Usuario";


            txtUsuario.BorderStyle =
                BorderStyle.FixedSingle;

            txtUsuario.Font =
                new Font(
                    "Segoe UI",
                    10F
                );

            txtUsuario.Location =
                new Point(72, 188);

            txtUsuario.Name =
                "txtUsuario";

            txtUsuario.Size =
                new Size(335, 30);

            txtUsuario.TabIndex =
                0;


            // =====================================================
            // CONTRASEÑA
            // =====================================================

            lblContrasena.AutoSize =
                true;

            lblContrasena.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold
                );

            lblContrasena.ForeColor =
                Color.FromArgb(55, 60, 66);

            lblContrasena.Location =
                new Point(72, 238);

            lblContrasena.Name =
                "lblContrasena";

            lblContrasena.Text =
                "Contraseña";


            txtContrasena.BorderStyle =
                BorderStyle.FixedSingle;

            txtContrasena.Font =
                new Font(
                    "Segoe UI",
                    10F
                );

            txtContrasena.Location =
                new Point(72, 264);

            txtContrasena.Name =
                "txtContrasena";

            txtContrasena.Size =
                new Size(335, 30);

            txtContrasena.TabIndex =
                1;

            txtContrasena.UseSystemPasswordChar =
                true;


            // =====================================================
            // INGRESAR
            // =====================================================

            btnIngresar.BackColor =
                Color.FromArgb(190, 137, 45);

            btnIngresar.Cursor =
                Cursors.Hand;

            btnIngresar.FlatAppearance.BorderSize =
                0;

            btnIngresar.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(210, 153, 50);

            btnIngresar.FlatStyle =
                FlatStyle.Flat;

            btnIngresar.Font =
                new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Bold
                );

            btnIngresar.ForeColor =
                Color.White;

            btnIngresar.Location =
                new Point(72, 320);

            btnIngresar.Name =
                "btnIngresar";

            btnIngresar.Size =
                new Size(335, 45);

            btnIngresar.TabIndex =
                2;

            btnIngresar.Text =
                "INGRESAR";

            btnIngresar.UseVisualStyleBackColor =
                false;

            btnIngresar.Click +=
                btnIngresar_Click;


            // =====================================================
            // FINAL
            // =====================================================

            pnlIzquierda.ResumeLayout(false);

            ((System.ComponentModel.ISupportInitialize)picLogoCompleto)
                .EndInit();

            pnlDerecha.ResumeLayout(false);
            pnlDerecha.PerformLayout();

            ResumeLayout(false);
        }

        #endregion


        private Panel pnlIzquierda;
        private PictureBox picLogoCompleto;

        private Panel pnlDerecha;

        private Button btnMinimizar;
        private Button btnCerrar;

        private Label lblTitulo;
        private Panel pnlLineaDorada;
        private Label lblSubtitulo;

        private Label lblUsuario;
        private TextBox txtUsuario;

        private Label lblContrasena;
        private TextBox txtContrasena;

        private Button btnIngresar;
    }
}