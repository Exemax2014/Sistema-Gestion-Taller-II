namespace Capa_Vistas
{
    partial class FormMensaje
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
            pnlBorde = new Panel();
            pnlPrincipal = new Panel();

            lblTitulo = new Label();
            lblMensaje = new Label();

            btnCancelar = new Button();
            btnAceptar = new Button();

            pnlBorde.SuspendLayout();
            pnlPrincipal.SuspendLayout();

            SuspendLayout();

            // =====================================================
            // FORM
            // =====================================================

            AutoScaleDimensions =
                new SizeF(8F, 20F);

            AutoScaleMode =
                AutoScaleMode.Font;

            BackColor =
                Color.FromArgb(190, 137, 45);

            ClientSize =
                new Size(470, 260);

            Controls.Add(pnlBorde);

            FormBorderStyle =
                FormBorderStyle.None;

            Name =
                "FormMensaje";

            ShowInTaskbar =
                false;

            StartPosition =
                FormStartPosition.CenterParent;

            // =====================================================
            // BORDE DORADO
            // =====================================================

            pnlBorde.BackColor =
                Color.FromArgb(190, 137, 45);

            pnlBorde.Controls.Add(pnlPrincipal);

            pnlBorde.Dock =
                DockStyle.Fill;

            pnlBorde.Name =
                "pnlBorde";

            pnlBorde.Padding =
                new Padding(3);

            // =====================================================
            // PANEL PRINCIPAL
            // =====================================================

            pnlPrincipal.BackColor =
                Color.White;

            pnlPrincipal.Controls.Add(lblTitulo);
            pnlPrincipal.Controls.Add(lblMensaje);
            pnlPrincipal.Controls.Add(btnCancelar);
            pnlPrincipal.Controls.Add(btnAceptar);

            pnlPrincipal.Dock =
                DockStyle.Fill;

            pnlPrincipal.Name =
                "pnlPrincipal";

            // =====================================================
            // TITULO
            // =====================================================

            lblTitulo.Font =
                new Font(
                    "Segoe UI",
                    14F,
                    FontStyle.Bold
                );

            lblTitulo.ForeColor =
                Color.FromArgb(27, 34, 42);

            lblTitulo.Location =
                new Point(35, 35);

            lblTitulo.Name =
                "lblTitulo";

            lblTitulo.Size =
                new Size(394, 38);

            lblTitulo.Text =
                "Mensaje";

            lblTitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            // =====================================================
            // MENSAJE
            // =====================================================

            lblMensaje.Font =
                new Font(
                    "Segoe UI",
                    9.5F
                );

            lblMensaje.ForeColor =
                Color.FromArgb(90, 96, 103);

            lblMensaje.Location =
                new Point(35, 85);

            lblMensaje.Name =
                "lblMensaje";

            lblMensaje.Size =
                new Size(394, 55);

            lblMensaje.Text =
                "Mensaje";

            lblMensaje.TextAlign =
                ContentAlignment.MiddleCenter;

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
                Color.FromArgb(40, 44, 48);

            btnCancelar.Location =
                new Point(70, 175);

            btnCancelar.Name =
                "btnCancelar";

            btnCancelar.Size =
                new Size(150, 42);

            btnCancelar.Text =
                "Cancelar";

            btnCancelar.UseVisualStyleBackColor =
                false;

            // =====================================================
            // ACEPTAR
            // =====================================================

            btnAceptar.BackColor =
                Color.FromArgb(190, 137, 45);

            btnAceptar.Cursor =
                Cursors.Hand;

            btnAceptar.FlatAppearance.BorderSize =
                0;

            btnAceptar.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(210, 153, 50);

            btnAceptar.FlatStyle =
                FlatStyle.Flat;

            btnAceptar.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            btnAceptar.ForeColor =
                Color.White;

            btnAceptar.Location =
                new Point(250, 175);

            btnAceptar.Name =
                "btnAceptar";

            btnAceptar.Size =
                new Size(150, 42);

            btnAceptar.Text =
                "Aceptar";

            btnAceptar.UseVisualStyleBackColor =
                false;

            // =====================================================
            // FINAL
            // =====================================================

            pnlBorde.ResumeLayout(false);
            pnlPrincipal.ResumeLayout(false);

            ResumeLayout(false);
        }

        #endregion

        private Panel pnlBorde;
        private Panel pnlPrincipal;

        private Label lblTitulo;
        private Label lblMensaje;

        private Button btnCancelar;
        private Button btnAceptar;
    }
}