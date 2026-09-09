using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormProductos
    {
        private System.ComponentModel.IContainer components =
            null;


        private TableLayoutPanel tlpPrincipal;

        private TableLayoutPanel tlpCabecera;

        private Panel pnlTitulo;

        private Label lblTitulo;

        private Label lblSubtitulo;

        private Panel pnlLineaTitulo;

        private Button btnNuevoProducto;


        private Panel pnlFiltros;

        private TableLayoutPanel tlpFiltros;

        private Label lblBuscar;

        private TextBox txtBuscar;

        private Label lblCategoriaFiltro;

        private ComboBox cmbCategoriaFiltro;

        private Label lblMarcaFiltro;

        private ComboBox cmbMarcaFiltro;

        private Label lblEstadoFiltro;

        private ComboBox cmbEstadoFiltro;

        private Button btnBuscar;

        private Button btnLimpiarFiltros;


        private Label lblCantidad;

        private DataGridView dgvProductos;


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
            tlpPrincipal = new TableLayoutPanel();
            tlpCabecera = new TableLayoutPanel();
            pnlTitulo = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaTitulo = new Panel();
            btnNuevoProducto = new Button();
            pnlFiltros = new Panel();
            tlpFiltros = new TableLayoutPanel();
            lblBuscar = new Label();
            lblCategoriaFiltro = new Label();
            lblMarcaFiltro = new Label();
            lblEstadoFiltro = new Label();
            txtBuscar = new TextBox();
            cmbCategoriaFiltro = new ComboBox();
            cmbMarcaFiltro = new ComboBox();
            cmbEstadoFiltro = new ComboBox();
            btnBuscar = new Button();
            btnLimpiarFiltros = new Button();
            lblCantidad = new Label();
            dgvProductos = new DataGridView();
            tlpPrincipal.SuspendLayout();
            tlpCabecera.SuspendLayout();
            pnlTitulo.SuspendLayout();
            pnlFiltros.SuspendLayout();
            tlpFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.BackColor = Color.FromArgb(241, 243, 245);
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(tlpCabecera, 0, 0);
            tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
            tlpPrincipal.Controls.Add(lblCantidad, 0, 2);
            tlpPrincipal.Controls.Add(dgvProductos, 0, 3);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(32, 20, 32, 25);
            tlpPrincipal.RowCount = 4;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.Size = new Size(1180, 700);
            tlpPrincipal.TabIndex = 0;
            // 
            // tlpCabecera
            // 
            tlpCabecera.ColumnCount = 2;
            tlpCabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 256F));
            tlpCabecera.Controls.Add(pnlTitulo, 0, 0);
            tlpCabecera.Controls.Add(btnNuevoProducto, 1, 0);
            tlpCabecera.Dock = DockStyle.Fill;
            tlpCabecera.Location = new Point(32, 20);
            tlpCabecera.Margin = new Padding(0);
            tlpCabecera.Name = "tlpCabecera";
            tlpCabecera.RowCount = 1;
            tlpCabecera.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCabecera.Size = new Size(1116, 100);
            tlpCabecera.TabIndex = 0;
            // 
            // pnlTitulo
            // 
            pnlTitulo.Controls.Add(lblTitulo);
            pnlTitulo.Controls.Add(lblSubtitulo);
            pnlTitulo.Controls.Add(pnlLineaTitulo);
            pnlTitulo.Dock = DockStyle.Fill;
            pnlTitulo.Location = new Point(0, 0);
            pnlTitulo.Margin = new Padding(0);
            pnlTitulo.Name = "pnlTitulo";
            pnlTitulo.Size = new Size(860, 100);
            pnlTitulo.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(45, 49, 54);
            lblTitulo.Location = new Point(0, 2);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(199, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Productos";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(2, 54);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(347, 21);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Catálogo de productos disponibles en el sistema.";
            // 
            // pnlLineaTitulo
            // 
            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(2, 84);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(92, 3);
            pnlLineaTitulo.TabIndex = 2;
            // 
            // btnNuevoProducto
            // 
            btnNuevoProducto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevoProducto.BackColor = Color.FromArgb(190, 137, 45);
            btnNuevoProducto.Cursor = Cursors.Hand;
            btnNuevoProducto.FlatAppearance.BorderSize = 0;
            btnNuevoProducto.FlatAppearance.MouseOverBackColor = Color.FromArgb(168, 119, 35);
            btnNuevoProducto.FlatStyle = FlatStyle.Flat;
            btnNuevoProducto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevoProducto.ForeColor = Color.White;
            btnNuevoProducto.Location = new Point(931, 17);
            btnNuevoProducto.Margin = new Padding(10, 17, 0, 0);
            btnNuevoProducto.Name = "btnNuevoProducto";
            btnNuevoProducto.Size = new Size(185, 42);
            btnNuevoProducto.TabIndex = 1;
            btnNuevoProducto.Text = "+ Nuevo producto";
            btnNuevoProducto.UseVisualStyleBackColor = false;
            // 
            // pnlFiltros
            // 
            pnlFiltros.BackColor = Color.White;
            pnlFiltros.BorderStyle = BorderStyle.FixedSingle;
            pnlFiltros.Controls.Add(tlpFiltros);
            pnlFiltros.Dock = DockStyle.Fill;
            pnlFiltros.Location = new Point(32, 125);
            pnlFiltros.Margin = new Padding(0, 5, 0, 10);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(18, 12, 18, 12);
            pnlFiltros.Size = new Size(1116, 105);
            pnlFiltros.TabIndex = 1;
            // 
            // tlpFiltros
            // 
            tlpFiltros.ColumnCount = 6;
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30.333952F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.0686455F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.9758816F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1317253F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.0593691F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.2448978F));
            tlpFiltros.Controls.Add(lblBuscar, 0, 0);
            tlpFiltros.Controls.Add(lblCategoriaFiltro, 1, 0);
            tlpFiltros.Controls.Add(lblMarcaFiltro, 2, 0);
            tlpFiltros.Controls.Add(lblEstadoFiltro, 3, 0);
            tlpFiltros.Controls.Add(txtBuscar, 0, 1);
            tlpFiltros.Controls.Add(btnBuscar, 4, 1);
            tlpFiltros.Controls.Add(btnLimpiarFiltros, 5, 1);
            tlpFiltros.Controls.Add(cmbCategoriaFiltro, 1, 1);
            tlpFiltros.Controls.Add(cmbMarcaFiltro, 2, 1);
            tlpFiltros.Controls.Add(cmbEstadoFiltro, 3, 1);
            tlpFiltros.Dock = DockStyle.Fill;
            tlpFiltros.Location = new Point(18, 12);
            tlpFiltros.Margin = new Padding(0);
            tlpFiltros.Name = "tlpFiltros";
            tlpFiltros.RowCount = 2;
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFiltros.Size = new Size(1078, 79);
            tlpFiltros.TabIndex = 0;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Dock = DockStyle.Fill;
            lblBuscar.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblBuscar.ForeColor = Color.FromArgb(70, 75, 80);
            lblBuscar.Location = new Point(3, 3);
            lblBuscar.Margin = new Padding(3, 3, 8, 0);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(316, 25);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar";
            lblBuscar.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblCategoriaFiltro
            // 
            lblCategoriaFiltro.AutoSize = true;
            lblCategoriaFiltro.Dock = DockStyle.Fill;
            lblCategoriaFiltro.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblCategoriaFiltro.ForeColor = Color.FromArgb(70, 75, 80);
            lblCategoriaFiltro.Location = new Point(330, 3);
            lblCategoriaFiltro.Margin = new Padding(3, 3, 8, 0);
            lblCategoriaFiltro.Name = "lblCategoriaFiltro";
            lblCategoriaFiltro.Size = new Size(173, 25);
            lblCategoriaFiltro.TabIndex = 1;
            lblCategoriaFiltro.Text = "Categoría";
            lblCategoriaFiltro.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblMarcaFiltro
            // 
            lblMarcaFiltro.AutoSize = true;
            lblMarcaFiltro.Dock = DockStyle.Fill;
            lblMarcaFiltro.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblMarcaFiltro.ForeColor = Color.FromArgb(70, 75, 80);
            lblMarcaFiltro.Location = new Point(514, 3);
            lblMarcaFiltro.Margin = new Padding(3, 3, 8, 0);
            lblMarcaFiltro.Name = "lblMarcaFiltro";
            lblMarcaFiltro.Size = new Size(172, 25);
            lblMarcaFiltro.TabIndex = 2;
            lblMarcaFiltro.Text = "Marca";
            lblMarcaFiltro.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblEstadoFiltro
            // 
            lblEstadoFiltro.AutoSize = true;
            lblEstadoFiltro.Dock = DockStyle.Fill;
            lblEstadoFiltro.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblEstadoFiltro.ForeColor = Color.FromArgb(70, 75, 80);
            lblEstadoFiltro.Location = new Point(697, 3);
            lblEstadoFiltro.Margin = new Padding(3, 3, 8, 0);
            lblEstadoFiltro.Name = "lblEstadoFiltro";
            lblEstadoFiltro.Size = new Size(109, 25);
            lblEstadoFiltro.TabIndex = 3;
            lblEstadoFiltro.Text = "Estado";
            lblEstadoFiltro.TextAlign = ContentAlignment.BottomLeft;
            // 
            // txtBuscar
            // 
            txtBuscar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Font = new Font("Segoe UI", 9.5F);
            txtBuscar.Location = new Point(3, 39);
            txtBuscar.Margin = new Padding(3, 3, 12, 3);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Nombre o código de barras";
            txtBuscar.Size = new Size(312, 29);
            txtBuscar.TabIndex = 4;
            // 
            // cmbCategoriaFiltro
            // 
            cmbCategoriaFiltro.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbCategoriaFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoriaFiltro.FlatStyle = FlatStyle.Flat;
            cmbCategoriaFiltro.Font = new Font("Segoe UI", 9F);
            cmbCategoriaFiltro.Location = new Point(330, 39);
            cmbCategoriaFiltro.Margin = new Padding(3, 3, 12, 3);
            cmbCategoriaFiltro.Name = "cmbCategoriaFiltro";
            cmbCategoriaFiltro.Size = new Size(169, 28);
            cmbCategoriaFiltro.TabIndex = 5;
            // 
            // cmbMarcaFiltro
            // 
            cmbMarcaFiltro.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbMarcaFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMarcaFiltro.FlatStyle = FlatStyle.Flat;
            cmbMarcaFiltro.Font = new Font("Segoe UI", 9F);
            cmbMarcaFiltro.Location = new Point(514, 39);
            cmbMarcaFiltro.Margin = new Padding(3, 3, 12, 3);
            cmbMarcaFiltro.Name = "cmbMarcaFiltro";
            cmbMarcaFiltro.Size = new Size(168, 28);
            cmbMarcaFiltro.TabIndex = 6;
            // 
            // cmbEstadoFiltro
            // 
            cmbEstadoFiltro.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbEstadoFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstadoFiltro.FlatStyle = FlatStyle.Flat;
            cmbEstadoFiltro.Font = new Font("Segoe UI", 9F);
            cmbEstadoFiltro.Location = new Point(697, 39);
            cmbEstadoFiltro.Margin = new Padding(3, 3, 12, 3);
            cmbEstadoFiltro.Name = "cmbEstadoFiltro";
            cmbEstadoFiltro.Size = new Size(105, 28);
            cmbEstadoFiltro.TabIndex = 7;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnBuscar.BackColor = Color.FromArgb(82, 88, 94);
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatAppearance.MouseOverBackColor = Color.FromArgb(67, 72, 78);
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(817, 36);
            btnBuscar.Margin = new Padding(3, 3, 6, 3);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(121, 34);
            btnBuscar.TabIndex = 8;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            // 
            // btnLimpiarFiltros
            // 
            btnLimpiarFiltros.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnLimpiarFiltros.BackColor = Color.White;
            btnLimpiarFiltros.Cursor = Cursors.Hand;
            btnLimpiarFiltros.FlatAppearance.BorderColor = Color.FromArgb(180, 184, 188);
            btnLimpiarFiltros.FlatAppearance.MouseOverBackColor = Color.FromArgb(237, 239, 241);
            btnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
            btnLimpiarFiltros.Font = new Font("Segoe UI", 8.5F);
            btnLimpiarFiltros.ForeColor = Color.FromArgb(70, 75, 80);
            btnLimpiarFiltros.Location = new Point(950, 36);
            btnLimpiarFiltros.Margin = new Padding(6, 3, 3, 3);
            btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            btnLimpiarFiltros.Size = new Size(125, 34);
            btnLimpiarFiltros.TabIndex = 9;
            btnLimpiarFiltros.Text = "Limpiar";
            btnLimpiarFiltros.UseVisualStyleBackColor = false;
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.Font = new Font("Segoe UI", 8.5F);
            lblCantidad.ForeColor = Color.FromArgb(105, 110, 116);
            lblCantidad.Location = new Point(34, 246);
            lblCantidad.Margin = new Padding(2, 6, 0, 4);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(1114, 28);
            lblCantidad.TabIndex = 2;
            lblCantidad.Text = "0 producto(s)";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvProductos
            // 
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.AllowUserToResizeRows = false;
            dgvProductos.BackgroundColor = Color.White;
            dgvProductos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProductos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvProductos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvProductos.ColumnHeadersHeight = 42;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(55, 59, 64);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(226, 229, 232);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(40, 44, 48);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvProductos.DefaultCellStyle = dataGridViewCellStyle2;
            dgvProductos.Dock = DockStyle.Fill;
            dgvProductos.EnableHeadersVisualStyles = false;
            dgvProductos.GridColor = Color.FromArgb(224, 227, 230);
            dgvProductos.Location = new Point(32, 278);
            dgvProductos.Margin = new Padding(0);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.RowHeadersWidth = 51;
            dgvProductos.RowTemplate.Height = 40;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(1116, 397);
            dgvProductos.TabIndex = 3;
            // 
            // FormProductos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 243, 245);
            ClientSize = new Size(1180, 700);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormProductos";
            Text = "Productos";
            tlpPrincipal.ResumeLayout(false);
            tlpPrincipal.PerformLayout();
            tlpCabecera.ResumeLayout(false);
            pnlTitulo.ResumeLayout(false);
            pnlTitulo.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            tlpFiltros.ResumeLayout(false);
            tlpFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}