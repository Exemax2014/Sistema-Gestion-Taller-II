using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormListadoVentas
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
            pnlAcciones = new Panel();
            btnCerrar = new Button();
            pnlListado = new Panel();
            lblListadoTitulo = new Label();
            lblListadoDescripcion = new Label();
            dgvVentas = new DataGridView();
            lblCantidad = new Label();
            pnlFiltros = new Panel();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            btnBuscar = new Button();
            btnLimpiar = new Button();
            pnlCabecera = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaTitulo = new Panel();
            pnlPrincipal.SuspendLayout();
            pnlAcciones.SuspendLayout();
            pnlListado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVentas).BeginInit();
            pnlFiltros.SuspendLayout();
            pnlCabecera.SuspendLayout();
            SuspendLayout();
            // 
            // pnlPrincipal
            // 
            pnlPrincipal.BackColor = Color.FromArgb(245, 246, 248);
            pnlPrincipal.Controls.Add(pnlAcciones);
            pnlPrincipal.Controls.Add(pnlListado);
            pnlPrincipal.Controls.Add(pnlFiltros);
            pnlPrincipal.Controls.Add(pnlCabecera);
            pnlPrincipal.Dock = DockStyle.Fill;
            pnlPrincipal.Location = new Point(0, 0);
            pnlPrincipal.Name = "pnlPrincipal";
            pnlPrincipal.Padding = new Padding(32, 18, 32, 24);
            pnlPrincipal.Size = new Size(1180, 760);
            pnlPrincipal.TabIndex = 0;
            // 
            // pnlAcciones
            // 
            pnlAcciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlAcciones.Controls.Add(btnCerrar);
            pnlAcciones.Location = new Point(32, 680);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(1116, 48);
            pnlAcciones.TabIndex = 3;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnCerrar.BackColor = Color.FromArgb(45, 49, 54);
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(0, 4);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(175, 40);
            btnCerrar.TabIndex = 0;
            btnCerrar.Text = "← Volver";
            btnCerrar.UseVisualStyleBackColor = false;
            // 
            // pnlListado
            // 
            pnlListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlListado.BackColor = Color.White;
            pnlListado.BorderStyle = BorderStyle.FixedSingle;
            pnlListado.Controls.Add(lblListadoTitulo);
            pnlListado.Controls.Add(lblListadoDescripcion);
            pnlListado.Controls.Add(dgvVentas);
            pnlListado.Controls.Add(lblCantidad);
            pnlListado.Location = new Point(32, 184);
            pnlListado.Name = "pnlListado";
            pnlListado.Size = new Size(1116, 490);
            pnlListado.TabIndex = 2;
            // 
            // lblListadoTitulo
            // 
            lblListadoTitulo.AutoSize = true;
            lblListadoTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblListadoTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblListadoTitulo.Location = new Point(18, 13);
            lblListadoTitulo.Name = "lblListadoTitulo";
            lblListadoTitulo.Size = new Size(217, 25);
            lblListadoTitulo.TabIndex = 0;
            lblListadoTitulo.Text = "Operaciones registradas";
            // 
            // lblListadoDescripcion
            // 
            lblListadoDescripcion.AutoSize = true;
            lblListadoDescripcion.Font = new Font("Segoe UI", 8F);
            lblListadoDescripcion.ForeColor = Color.FromArgb(105, 110, 116);
            lblListadoDescripcion.Location = new Point(241, 17);
            lblListadoDescripcion.Name = "lblListadoDescripcion";
            lblListadoDescripcion.Size = new Size(292, 19);
            lblListadoDescripcion.TabIndex = 1;
            lblListadoDescripcion.Text = "Seleccioná una venta para consultar su detalle.";
            // 
            // dgvVentas
            // 
            dgvVentas.AllowUserToAddRows = false;
            dgvVentas.AllowUserToDeleteRows = false;
            dgvVentas.AllowUserToResizeRows = false;
            dgvVentas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvVentas.BackgroundColor = Color.White;
            dgvVentas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvVentas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvVentas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvVentas.ColumnHeadersHeight = 40;
            dgvVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(55, 59, 64);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(226, 229, 232);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(40, 44, 48);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvVentas.DefaultCellStyle = dataGridViewCellStyle2;
            dgvVentas.EnableHeadersVisualStyles = false;
            dgvVentas.GridColor = Color.FromArgb(224, 227, 230);
            dgvVentas.Location = new Point(18, 50);
            dgvVentas.MultiSelect = false;
            dgvVentas.Name = "dgvVentas";
            dgvVentas.ReadOnly = true;
            dgvVentas.RowHeadersVisible = false;
            dgvVentas.RowHeadersWidth = 51;
            dgvVentas.RowTemplate.Height = 38;
            dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentas.Size = new Size(1078, 400);
            dgvVentas.TabIndex = 2;
            // 
            // lblCantidad
            // 
            lblCantidad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCantidad.AutoSize = true;
            lblCantidad.Font = new Font("Segoe UI", 8.5F);
            lblCantidad.ForeColor = Color.FromArgb(105, 110, 116);
            lblCantidad.Location = new Point(18, 453);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(73, 20);
            lblCantidad.TabIndex = 3;
            lblCantidad.Text = "0 venta(s)";
            // 
            // pnlFiltros
            // 
            pnlFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFiltros.BackColor = Color.White;
            pnlFiltros.BorderStyle = BorderStyle.FixedSingle;
            pnlFiltros.Controls.Add(lblDesde);
            pnlFiltros.Controls.Add(dtpDesde);
            pnlFiltros.Controls.Add(lblHasta);
            pnlFiltros.Controls.Add(dtpHasta);
            pnlFiltros.Controls.Add(btnBuscar);
            pnlFiltros.Controls.Add(btnLimpiar);
            pnlFiltros.Location = new Point(32, 106);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Size = new Size(1116, 72);
            pnlFiltros.TabIndex = 1;
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(35, 21);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(51, 20);
            lblDesde.TabIndex = 0;
            lblDesde.Text = "Desde";
            // 
            // dtpDesde
            // 
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(92, 21);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(210, 27);
            dtpDesde.TabIndex = 1;
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(361, 21);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(47, 20);
            lblHasta.TabIndex = 2;
            lblHasta.Text = "Hasta";
            // 
            // dtpHasta
            // 
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(414, 21);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(210, 27);
            dtpHasta.TabIndex = 3;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.BackColor = Color.FromArgb(190, 137, 45);
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(809, 16);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(110, 40);
            btnBuscar.TabIndex = 4;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLimpiar.BackColor = Color.White;
            btnLimpiar.Cursor = Cursors.Hand;
            btnLimpiar.FlatAppearance.BorderColor = Color.FromArgb(160, 165, 170);
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Location = new Point(985, 16);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(104, 40);
            btnLimpiar.TabIndex = 5;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
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
            lblTitulo.Size = new Size(346, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Historial de ventas";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(2, 48);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(325, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Consulta el historial de operaciones registradas.";
            // 
            // pnlLineaTitulo
            // 
            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(2, 74);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(95, 3);
            pnlLineaTitulo.TabIndex = 2;
            // 
            // FormListadoVentas
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1180, 760);
            Controls.Add(pnlPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormListadoVentas";
            Text = "Historial de ventas";
            pnlPrincipal.ResumeLayout(false);
            pnlAcciones.ResumeLayout(false);
            pnlListado.ResumeLayout(false);
            pnlListado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVentas).EndInit();
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
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

        private Panel pnlFiltros;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnBuscar;
        private Button btnLimpiar;

        private Panel pnlListado;
        private Label lblListadoTitulo;
        private Label lblListadoDescripcion;
        private DataGridView dgvVentas;
        private Label lblCantidad;

        private Panel pnlAcciones;
        private Button btnCerrar;
    }
}
