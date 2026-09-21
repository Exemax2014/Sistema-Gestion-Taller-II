using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormBackup
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlCabecera;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Panel pnlLineaTitulo;
        private Panel pnlTarjeta;
        private Label lblBaseDatos;
        private TextBox txtBaseDatos;
        private Label lblUbicacionCopia;
        private TextBox txtUbicacionCopia;
        private Label lblAvisoServidor;
        private Button btnGenerarBackup;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlCabecera = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaTitulo = new Panel();
            pnlTarjeta = new Panel();
            lblBaseDatos = new Label();
            txtBaseDatos = new TextBox();
            lblUbicacionCopia = new Label();
            txtUbicacionCopia = new TextBox();
            lblAvisoServidor = new Label();
            btnGenerarBackup = new Button();
            pnlCabecera.SuspendLayout();
            pnlTarjeta.SuspendLayout();
            SuspendLayout();
            //
            // pnlCabecera
            //
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Controls.Add(lblSubtitulo);
            pnlCabecera.Controls.Add(pnlLineaTitulo);
            pnlCabecera.Location = new Point(32, 20);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(1116, 100);
            pnlCabecera.TabIndex = 0;
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(45, 49, 54);
            lblTitulo.Location = new Point(0, 2);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(110, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Back Up";
            //
            // lblSubtitulo
            //
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(2, 54);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(340, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Generá una copia de seguridad de la base de datos.";
            //
            // pnlLineaTitulo
            //
            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(2, 84);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(92, 3);
            pnlLineaTitulo.TabIndex = 2;
            //
            // pnlTarjeta
            //
            pnlTarjeta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlTarjeta.BackColor = Color.White;
            pnlTarjeta.BorderStyle = BorderStyle.FixedSingle;
            pnlTarjeta.Controls.Add(lblBaseDatos);
            pnlTarjeta.Controls.Add(txtBaseDatos);
            pnlTarjeta.Controls.Add(lblUbicacionCopia);
            pnlTarjeta.Controls.Add(txtUbicacionCopia);
            pnlTarjeta.Controls.Add(lblAvisoServidor);
            pnlTarjeta.Controls.Add(btnGenerarBackup);
            pnlTarjeta.Location = new Point(32, 145);
            pnlTarjeta.Name = "pnlTarjeta";
            pnlTarjeta.Size = new Size(1116, 270);
            pnlTarjeta.TabIndex = 1;
            //
            // lblBaseDatos
            //
            lblBaseDatos.AutoSize = true;
            lblBaseDatos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBaseDatos.ForeColor = Color.FromArgb(55, 59, 64);
            lblBaseDatos.Location = new Point(24, 24);
            lblBaseDatos.Name = "lblBaseDatos";
            lblBaseDatos.Size = new Size(91, 20);
            lblBaseDatos.TabIndex = 0;
            lblBaseDatos.Text = "Base de datos";
            //
            // txtBaseDatos
            //
            txtBaseDatos.BackColor = Color.FromArgb(246, 247, 248);
            txtBaseDatos.Location = new Point(24, 48);
            txtBaseDatos.Name = "txtBaseDatos";
            txtBaseDatos.ReadOnly = true;
            txtBaseDatos.Size = new Size(700, 27);
            txtBaseDatos.TabIndex = 1;
            //
            // lblUbicacionCopia
            //
            lblUbicacionCopia.AutoSize = true;
            lblUbicacionCopia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUbicacionCopia.ForeColor = Color.FromArgb(55, 59, 64);
            lblUbicacionCopia.Location = new Point(24, 99);
            lblUbicacionCopia.Name = "lblUbicacionCopia";
            lblUbicacionCopia.Size = new Size(145, 20);
            lblUbicacionCopia.TabIndex = 2;
            lblUbicacionCopia.Text = "Ubicación de la copia";
            //
            // txtUbicacionCopia
            //
            txtUbicacionCopia.BackColor = Color.FromArgb(246, 247, 248);
            txtUbicacionCopia.Location = new Point(24, 123);
            txtUbicacionCopia.Name = "txtUbicacionCopia";
            txtUbicacionCopia.ReadOnly = true;
            txtUbicacionCopia.Size = new Size(700, 27);
            txtUbicacionCopia.TabIndex = 3;
            //
            // lblAvisoServidor
            //
            lblAvisoServidor.Font = new Font("Segoe UI", 8.5F);
            lblAvisoServidor.ForeColor = Color.FromArgb(105, 110, 116);
            lblAvisoServidor.Location = new Point(24, 166);
            lblAvisoServidor.Name = "lblAvisoServidor";
            lblAvisoServidor.Size = new Size(500, 42);
            lblAvisoServidor.TabIndex = 4;
            lblAvisoServidor.Text = "La copia se guarda en el servidor donde se ejecuta SQL Server.";
            //
            // btnGenerarBackup
            //
            btnGenerarBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerarBackup.BackColor = Color.FromArgb(190, 137, 45);
            btnGenerarBackup.Cursor = Cursors.Hand;
            btnGenerarBackup.FlatAppearance.BorderSize = 0;
            btnGenerarBackup.FlatStyle = FlatStyle.Flat;
            btnGenerarBackup.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGenerarBackup.ForeColor = Color.White;
            btnGenerarBackup.Location = new Point(918, 202);
            btnGenerarBackup.Name = "btnGenerarBackup";
            btnGenerarBackup.Size = new Size(172, 40);
            btnGenerarBackup.TabIndex = 5;
            btnGenerarBackup.Text = "Generar Back Up";
            btnGenerarBackup.UseVisualStyleBackColor = false;
            //
            // FormBackup
            //
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1180, 760);
            Controls.Add(pnlCabecera);
            Controls.Add(pnlTarjeta);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormBackup";
            Text = "Back Up";
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlTarjeta.ResumeLayout(false);
            pnlTarjeta.PerformLayout();
            ResumeLayout(false);
        }
    }
}
