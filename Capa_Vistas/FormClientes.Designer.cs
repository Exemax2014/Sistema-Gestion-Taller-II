namespace Capa_Vistas
{
    partial class FormClientes
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

            pnlEncabezado = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaDorada = new Panel();

            pnlBusqueda = new Panel();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            btnNuevoCliente = new Button();

            pnlTabla = new Panel();
            dgvClientes = new DataGridView();

            colDni = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colTelefono = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colLocalidad = new DataGridViewTextBoxColumn();
            colEditar = new DataGridViewButtonColumn();
            colEliminar = new DataGridViewButtonColumn();

            pnlPie = new Panel();
            lblCantidad = new Label();

            pnlPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            pnlBusqueda.SuspendLayout();
            pnlTabla.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)dgvClientes)
                .BeginInit();

            pnlPie.SuspendLayout();

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
                "FormClientes";

            Text =
                "Clientes";

            // =====================================================
            // PANEL PRINCIPAL
            // =====================================================

            pnlPrincipal.BackColor =
                Color.FromArgb(244, 245, 247);

            pnlPrincipal.Controls.Add(pnlTabla);
            pnlPrincipal.Controls.Add(pnlBusqueda);
            pnlPrincipal.Controls.Add(pnlEncabezado);
            pnlPrincipal.Controls.Add(pnlPie);

            pnlPrincipal.Dock =
                DockStyle.Fill;

            pnlPrincipal.Name =
                "pnlPrincipal";

            pnlPrincipal.Padding =
                new Padding(30);

            // =====================================================
            // ENCABEZADO
            // =====================================================

            pnlEncabezado.BackColor =
                Color.Transparent;

            pnlEncabezado.Controls.Add(lblTitulo);
            pnlEncabezado.Controls.Add(lblSubtitulo);
            pnlEncabezado.Controls.Add(pnlLineaDorada);

            pnlEncabezado.Dock =
                DockStyle.Top;

            pnlEncabezado.Name =
                "pnlEncabezado";

            pnlEncabezado.Size =
                new Size(1140, 105);

            // =====================================================
            // TITULO
            // =====================================================

            lblTitulo.AutoSize =
                true;

            lblTitulo.Font =
                new Font(
                    "Segoe UI",
                    22F,
                    FontStyle.Bold
                );

            lblTitulo.ForeColor =
                Color.FromArgb(27, 34, 42);

            lblTitulo.Location =
                new Point(0, 0);

            lblTitulo.Name =
                "lblTitulo";

            lblTitulo.Text =
                "Clientes";

            // =====================================================
            // SUBTITULO
            // =====================================================

            lblSubtitulo.AutoSize =
                true;

            lblSubtitulo.Font =
                new Font(
                    "Segoe UI",
                    10F
                );

            lblSubtitulo.ForeColor =
                Color.FromArgb(105, 110, 116);

            lblSubtitulo.Location =
                new Point(2, 55);

            lblSubtitulo.Name =
                "lblSubtitulo";

            lblSubtitulo.Text =
                "Gestión y administración de clientes.";

            // =====================================================
            // LINEA DORADA
            // =====================================================

            pnlLineaDorada.BackColor =
                Color.FromArgb(190, 137, 45);

            pnlLineaDorada.Location =
                new Point(2, 86);

            pnlLineaDorada.Name =
                "pnlLineaDorada";

            pnlLineaDorada.Size =
                new Size(95, 3);

            // =====================================================
            // PANEL BUSQUEDA
            // =====================================================

            pnlBusqueda.BackColor =
                Color.White;

            pnlBusqueda.Controls.Add(txtBuscar);
            pnlBusqueda.Controls.Add(btnBuscar);
            pnlBusqueda.Controls.Add(btnNuevoCliente);

            pnlBusqueda.Dock =
                DockStyle.Top;

            pnlBusqueda.Name =
                "pnlBusqueda";

            pnlBusqueda.Padding =
                new Padding(18);

            pnlBusqueda.Size =
                new Size(1140, 82);

            // =====================================================
            // TXT BUSCAR
            // =====================================================

            txtBuscar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left; 

            txtBuscar.BorderStyle =
                BorderStyle.FixedSingle;

            txtBuscar.Font =
                new Font(
                    "Segoe UI",
                    10F
                );

            txtBuscar.Location =
                new Point(18, 24);

            txtBuscar.Name =
                "txtBuscar";

            txtBuscar.PlaceholderText =
                "Buscar por nombre, apellido, DNI o teléfono...";

            txtBuscar.Size =
                new Size(520, 30);

            txtBuscar.TabIndex =
                0;

            // =====================================================
            // BOTON BUSCAR
            // =====================================================

            btnBuscar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnBuscar.BackColor =
                Color.FromArgb(35, 40, 46);

            btnBuscar.Cursor =
                Cursors.Hand;

            btnBuscar.FlatAppearance.BorderSize =
                0;

            btnBuscar.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(50, 56, 63);

            btnBuscar.FlatStyle =
                FlatStyle.Flat;

            btnBuscar.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            btnBuscar.ForeColor =
                Color.White;

            btnBuscar.Location =
                new Point(560, 20);

            btnBuscar.Name =
                "btnBuscar";

            btnBuscar.Size =
                new Size(130, 40);

            btnBuscar.TabIndex =
                1;

            btnBuscar.Text =
                "Buscar";

            btnBuscar.UseVisualStyleBackColor =
                false;

            // =====================================================
            // NUEVO CLIENTE
            // =====================================================

            btnNuevoCliente.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnNuevoCliente.BackColor =
                Color.FromArgb(190, 137, 45);

            btnNuevoCliente.Cursor =
                Cursors.Hand;

            btnNuevoCliente.FlatAppearance.BorderSize =
                0;

            btnNuevoCliente.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(210, 153, 50);

            btnNuevoCliente.FlatStyle =
                FlatStyle.Flat;

            btnNuevoCliente.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            btnNuevoCliente.ForeColor =
                Color.White;

            btnNuevoCliente.Location =
                new Point(710, 20);

            btnNuevoCliente.Name =
                "btnNuevoCliente";

            btnNuevoCliente.Size =
                new Size(175, 40);

            btnNuevoCliente.TabIndex =
                2;

            btnNuevoCliente.Text =
                "+ Nuevo cliente";

            btnNuevoCliente.UseVisualStyleBackColor =
                false;

            // =====================================================
            // PANEL TABLA
            // =====================================================

            pnlTabla.BackColor =
                Color.White;

            pnlTabla.Controls.Add(dgvClientes);

            pnlTabla.Dock =
                DockStyle.Fill;

            pnlTabla.Name =
                "pnlTabla";

            pnlTabla.Padding =
                new Padding(0, 20, 0, 0);

            // =====================================================
            // GRID CLIENTES
            // =====================================================

            dgvClientes.AllowUserToAddRows =
                false;

            dgvClientes.AllowUserToDeleteRows =
                false;

            dgvClientes.AllowUserToResizeRows =
                false;

            dgvClientes.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvClientes.BackgroundColor =
                Color.White;

            dgvClientes.BorderStyle =
                BorderStyle.None;

            dgvClientes.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;

            dgvClientes.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.None;

            dgvClientes.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(27, 34, 42);

            dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvClientes.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold
                );

            dgvClientes.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                Color.FromArgb(27, 34, 42);

            dgvClientes.ColumnHeadersHeight =
                42;

            dgvClientes.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvClientes.Columns.AddRange(
                new DataGridViewColumn[]
                {
                    colDni,
                    colNombre,
                    colApellido,
                    colTelefono,
                    colEmail,
                    colLocalidad,
                    colEditar,
                    colEliminar
                }
            );

            dgvClientes.DefaultCellStyle.BackColor =
                Color.White;

            dgvClientes.DefaultCellStyle.ForeColor =
                Color.FromArgb(45, 50, 55);

            dgvClientes.DefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    9F
                );

            dgvClientes.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(244, 237, 222);

            dgvClientes.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(35, 35, 35);

            dgvClientes.Dock =
                DockStyle.Fill;

            dgvClientes.EnableHeadersVisualStyles =
                false;

            dgvClientes.GridColor =
                Color.FromArgb(230, 232, 235);

            dgvClientes.MultiSelect =
                false;

            dgvClientes.Name =
                "dgvClientes";

            dgvClientes.ReadOnly =
                true;

            dgvClientes.RowHeadersVisible =
                false;

            dgvClientes.RowTemplate.Height =
                38;

            dgvClientes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            // =====================================================
            // COLUMNAS
            // =====================================================

            colDni.HeaderText =
                "DNI";

            colDni.Name =
                "colDni";

            colDni.ReadOnly =
                true;

            colDni.FillWeight =
                80F;


            colNombre.HeaderText =
                "Nombre";

            colNombre.Name =
                "colNombre";

            colNombre.ReadOnly =
                true;


            colApellido.HeaderText =
                "Apellido";

            colApellido.Name =
                "colApellido";

            colApellido.ReadOnly =
                true;


            colTelefono.HeaderText =
                "Teléfono";

            colTelefono.Name =
                "colTelefono";

            colTelefono.ReadOnly =
                true;


            colEmail.HeaderText =
                "Email";

            colEmail.Name =
                "colEmail";

            colEmail.ReadOnly =
                true;

            colEmail.FillWeight =
                130F;


            colLocalidad.HeaderText =
                "Localidad";

            colLocalidad.Name =
                "colLocalidad";

            colLocalidad.ReadOnly =
                true;


            colEditar.HeaderText =
                "";

            colEditar.Name =
                "colEditar";

            colEditar.ReadOnly =
                true;

            colEditar.Text =
                "Editar";

            colEditar.UseColumnTextForButtonValue =
                true;

            colEditar.FillWeight =
                65F;

            colEditar.FlatStyle =
                FlatStyle.Flat;

            colEditar.DefaultCellStyle.BackColor =
                Color.FromArgb(242, 243, 245);

            colEditar.DefaultCellStyle.ForeColor =
                Color.FromArgb(45, 50, 55);


            colEliminar.HeaderText =
                "";

            colEliminar.Name =
                "colEliminar";

            colEliminar.ReadOnly =
                true;

            colEliminar.Text =
                "Eliminar";

            colEliminar.UseColumnTextForButtonValue =
                true;

            colEliminar.FillWeight =
                70F;

            colEliminar.FlatStyle =
                FlatStyle.Flat;

            colEliminar.DefaultCellStyle.BackColor =
                Color.FromArgb(245, 235, 235);

            colEliminar.DefaultCellStyle.ForeColor =
                Color.FromArgb(150, 50, 50);

            // =====================================================
            // PIE
            // =====================================================

            pnlPie.BackColor =
                Color.White;

            pnlPie.Controls.Add(lblCantidad);

            pnlPie.Dock =
                DockStyle.Bottom;

            pnlPie.Name =
                "pnlPie";

            pnlPie.Size =
                new Size(1140, 48);

            // =====================================================
            // CANTIDAD
            // =====================================================

            lblCantidad.Anchor =
                AnchorStyles.Right |
                AnchorStyles.Top;

            lblCantidad.Font =
                new Font(
                    "Segoe UI",
                    9F
                );

            lblCantidad.ForeColor =
                Color.FromArgb(110, 115, 120);

            lblCantidad.Location =
                new Point(850, 14);

            lblCantidad.Name =
                "lblCantidad";

            lblCantidad.Size =
                new Size(260, 22);

            lblCantidad.Text =
                "0 clientes encontrados";

            lblCantidad.TextAlign =
                ContentAlignment.MiddleRight;

            // =====================================================
            // FINAL
            // =====================================================

            pnlPrincipal.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();

            pnlBusqueda.ResumeLayout(false);
            pnlBusqueda.PerformLayout();

            pnlTabla.ResumeLayout(false);

            ((System.ComponentModel.ISupportInitialize)dgvClientes)
                .EndInit();

            pnlPie.ResumeLayout(false);

            ResumeLayout(false);
        }

        #endregion

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

        private DataGridViewTextBoxColumn colDni;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colApellido;
        private DataGridViewTextBoxColumn colTelefono;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewTextBoxColumn colLocalidad;

        private DataGridViewButtonColumn colEditar;
        private DataGridViewButtonColumn colEliminar;

        private Panel pnlPie;
        private Label lblCantidad;
    }
}