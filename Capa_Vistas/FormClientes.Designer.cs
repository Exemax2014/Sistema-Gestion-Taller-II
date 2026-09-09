using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormClientes
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlPrincipal = new Panel();
            pnlTabla = new Panel();
            dgvClientes = new DataGridView();
            pnlBusqueda = new Panel();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            btnNuevoCliente = new Button();
            pnlEncabezado = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaDorada = new Panel();
            pnlPie = new Panel();
            lblCantidad = new Label();
            pnlPrincipal.SuspendLayout();
            pnlTabla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            pnlBusqueda.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            pnlPie.SuspendLayout();
            SuspendLayout();
            // 
            // pnlPrincipal
            // 
            pnlPrincipal.BackColor = Color.FromArgb(245, 246, 248);
            pnlPrincipal.Controls.Add(pnlTabla);
            pnlPrincipal.Controls.Add(pnlBusqueda);
            pnlPrincipal.Controls.Add(pnlEncabezado);
            pnlPrincipal.Controls.Add(pnlPie);
            pnlPrincipal.Dock = DockStyle.Fill;
            pnlPrincipal.Location = new Point(0, 0);
            pnlPrincipal.Name = "pnlPrincipal";
            pnlPrincipal.Padding = new Padding(32);
            pnlPrincipal.Size = new Size(1180, 760);
            pnlPrincipal.TabIndex = 0;
            // 
            // pnlTabla
            // 
            pnlTabla.BackColor = Color.White;
            pnlTabla.BorderStyle = BorderStyle.FixedSingle;
            pnlTabla.Controls.Add(dgvClientes);
            pnlTabla.Dock = DockStyle.Fill;
            pnlTabla.Location = new Point(32, 200);
            pnlTabla.Name = "pnlTabla";
            pnlTabla.Padding = new Padding(18, 18, 18, 10);
            pnlTabla.Size = new Size(1116, 480);
            pnlTabla.TabIndex = 0;
            // 
            // dgvClientes
            // 
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dgvClientes.AllowUserToResizeRows = false;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.BackgroundColor = Color.White;
            dgvClientes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvClientes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvClientes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvClientes.ColumnHeadersHeight = 40;
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(55, 59, 64);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(226, 229, 232);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(40, 44, 48);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvClientes.DefaultCellStyle = dataGridViewCellStyle2;
            dgvClientes.Dock = DockStyle.Fill;
            dgvClientes.EnableHeadersVisualStyles = false;
            dgvClientes.GridColor = Color.FromArgb(224, 227, 230);
            dgvClientes.Location = new Point(18, 18);
            dgvClientes.MultiSelect = false;
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersVisible = false;
            dgvClientes.RowHeadersWidth = 51;
            dgvClientes.RowTemplate.Height = 38;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new Size(1078, 450);
            dgvClientes.TabIndex = 0;
            // 
            // pnlBusqueda
            // 
            pnlBusqueda.BackColor = Color.White;
            pnlBusqueda.BorderStyle = BorderStyle.FixedSingle;
            pnlBusqueda.Controls.Add(txtBuscar);
            pnlBusqueda.Controls.Add(btnBuscar);
            pnlBusqueda.Controls.Add(btnNuevoCliente);
            pnlBusqueda.Dock = DockStyle.Top;
            pnlBusqueda.Location = new Point(32, 114);
            pnlBusqueda.Name = "pnlBusqueda";
            pnlBusqueda.Size = new Size(1116, 86);
            pnlBusqueda.TabIndex = 1;
            // 
            // txtBuscar
            // 
            txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Font = new Font("Segoe UI", 10F);
            txtBuscar.Location = new Point(18, 26);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar por nombre, apellido, DNI o teléfono...";
            txtBuscar.Size = new Size(625, 30);
            txtBuscar.TabIndex = 0;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.BackColor = Color.FromArgb(45, 49, 54);
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(666, 20);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(140, 40);
            btnBuscar.TabIndex = 1;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            // 
            // btnNuevoCliente
            // 
            btnNuevoCliente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevoCliente.BackColor = Color.FromArgb(190, 137, 45);
            btnNuevoCliente.Cursor = Cursors.Hand;
            btnNuevoCliente.FlatAppearance.BorderSize = 0;
            btnNuevoCliente.FlatStyle = FlatStyle.Flat;
            btnNuevoCliente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevoCliente.ForeColor = Color.White;
            btnNuevoCliente.Location = new Point(831, 21);
            btnNuevoCliente.Name = "btnNuevoCliente";
            btnNuevoCliente.Size = new Size(263, 40);
            btnNuevoCliente.TabIndex = 2;
            btnNuevoCliente.Text = "+ Nuevo cliente";
            btnNuevoCliente.UseVisualStyleBackColor = false;
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.Transparent;
            pnlEncabezado.Controls.Add(lblTitulo);
            pnlEncabezado.Controls.Add(lblSubtitulo);
            pnlEncabezado.Controls.Add(pnlLineaDorada);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Location = new Point(32, 32);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(1116, 82);
            pnlEncabezado.TabIndex = 2;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(45, 49, 54);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(159, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Clientes";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(2, 48);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(250, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Gestión y administración de clientes.";
            // 
            // pnlLineaDorada
            // 
            pnlLineaDorada.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaDorada.Location = new Point(2, 74);
            pnlLineaDorada.Name = "pnlLineaDorada";
            pnlLineaDorada.Size = new Size(95, 3);
            pnlLineaDorada.TabIndex = 2;
            // 
            // pnlPie
            // 
            pnlPie.BackColor = Color.White;
            pnlPie.Controls.Add(lblCantidad);
            pnlPie.Dock = DockStyle.Bottom;
            pnlPie.Location = new Point(32, 680);
            pnlPie.Name = "pnlPie";
            pnlPie.Size = new Size(1116, 48);
            pnlPie.TabIndex = 3;
            // 
            // lblCantidad
            // 
            lblCantidad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCantidad.Font = new Font("Segoe UI", 8.5F);
            lblCantidad.ForeColor = Color.FromArgb(105, 110, 116);
            lblCantidad.Location = new Point(820, 14);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(275, 22);
            lblCantidad.TabIndex = 0;
            lblCantidad.Text = "0 clientes encontrados";
            lblCantidad.TextAlign = ContentAlignment.MiddleRight;
            // 
            // FormClientes
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1180, 760);
            Controls.Add(pnlPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormClientes";
            Text = "Clientes";
            pnlPrincipal.ResumeLayout(false);
            pnlTabla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            pnlBusqueda.ResumeLayout(false);
            pnlBusqueda.PerformLayout();
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            pnlPie.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion


        // =========================================================
        // CONTROLES
        //
        // Solo controles visuales.
        // Las columnas del DataGridView están en FormClientes.cs.
        // =========================================================

        private Panel pnlPrincipal;

        private Panel pnlEncabezado;

        private Label lblTitulo;

        private Label lblSubtitulo;

        private Panel pnlLineaDorada;


        private Panel pnlBusqueda;

        private TextBox txtBuscar;

        private Button btnBuscar;

        private Button btnNuevoCliente;


        private Panel pnlTabla;

        private DataGridView dgvClientes;


        private Panel pnlPie;

        private Label lblCantidad;
    }
}