using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormUsuarios
    {
        private System.ComponentModel.IContainer components =
            null;


        private Panel pnlCabecera;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Panel pnlLineaTitulo;


        private Panel pnlFiltros;
        private Label lblBuscar;
        private TextBox txtBuscar;

        private Label lblPerfil;
        private ComboBox cmbPerfil;

        private Label lblEstado;
        private ComboBox cmbEstado;

        private Button btnBuscar;
        private Button btnLimpiarFiltros;
        private Button btnNuevoUsuario;


        private Panel pnlListado;
        private Label lblListadoTitulo;
        private Label lblListadoDescripcion;

        private DataGridView dgvUsuarios;

        private Label lblCantidad;


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
            pnlCabecera = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaTitulo = new Panel();
            pnlFiltros = new Panel();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            lblPerfil = new Label();
            cmbPerfil = new ComboBox();
            lblEstado = new Label();
            cmbEstado = new ComboBox();
            btnBuscar = new Button();
            btnLimpiarFiltros = new Button();
            btnNuevoUsuario = new Button();
            pnlListado = new Panel();
            lblListadoTitulo = new Label();
            lblListadoDescripcion = new Label();
            dgvUsuarios = new DataGridView();
            lblCantidad = new Label();
            pnlCabecera.SuspendLayout();
            pnlFiltros.SuspendLayout();
            pnlListado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
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
            lblTitulo.Size = new Size(172, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Usuarios";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(2, 48);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(328, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Administración de usuarios, perfiles y sucursales.";
            // 
            // pnlLineaTitulo
            // 
            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(2, 74);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(95, 3);
            pnlLineaTitulo.TabIndex = 2;
            // 
            // pnlFiltros
            // 
            pnlFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFiltros.BackColor = Color.White;
            pnlFiltros.BorderStyle = BorderStyle.FixedSingle;
            pnlFiltros.Controls.Add(lblBuscar);
            pnlFiltros.Controls.Add(txtBuscar);
            pnlFiltros.Controls.Add(lblPerfil);
            pnlFiltros.Controls.Add(cmbPerfil);
            pnlFiltros.Controls.Add(lblEstado);
            pnlFiltros.Controls.Add(cmbEstado);
            pnlFiltros.Controls.Add(btnBuscar);
            pnlFiltros.Controls.Add(btnLimpiarFiltros);
            pnlFiltros.Controls.Add(btnNuevoUsuario);
            pnlFiltros.Location = new Point(32, 106);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Size = new Size(1116, 116);
            pnlFiltros.TabIndex = 1;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(20, 17);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(104, 20);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar usuario";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(20, 44);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Usuario, nombre o apellido";
            txtBuscar.Size = new Size(300, 27);
            txtBuscar.TabIndex = 1;
            // 
            // lblPerfil
            // 
            lblPerfil.AutoSize = true;
            lblPerfil.Location = new Point(345, 17);
            lblPerfil.Name = "lblPerfil";
            lblPerfil.Size = new Size(42, 20);
            lblPerfil.TabIndex = 2;
            lblPerfil.Text = "Perfil";
            // 
            // cmbPerfil
            // 
            cmbPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPerfil.Location = new Point(345, 44);
            cmbPerfil.Name = "cmbPerfil";
            cmbPerfil.Size = new Size(180, 28);
            cmbPerfil.TabIndex = 3;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(550, 17);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(54, 20);
            lblEstado.TabIndex = 4;
            lblEstado.Text = "Estado";
            // 
            // cmbEstado
            // 
            cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstado.Location = new Point(550, 44);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(160, 28);
            cmbEstado.TabIndex = 5;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.BackColor = Color.FromArgb(190, 137, 45);
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(748, 41);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(110, 40);
            btnBuscar.TabIndex = 6;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            // 
            // btnLimpiarFiltros
            // 
            btnLimpiarFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLimpiarFiltros.BackColor = Color.White;
            btnLimpiarFiltros.Cursor = Cursors.Hand;
            btnLimpiarFiltros.FlatAppearance.BorderColor = Color.FromArgb(160, 165, 170);
            btnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
            btnLimpiarFiltros.Location = new Point(875, 41);
            btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            btnLimpiarFiltros.Size = new Size(105, 40);
            btnLimpiarFiltros.TabIndex = 7;
            btnLimpiarFiltros.Text = "Limpiar";
            btnLimpiarFiltros.UseVisualStyleBackColor = false;
            // 
            // btnNuevoUsuario
            // 
            btnNuevoUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevoUsuario.BackColor = Color.FromArgb(45, 49, 54);
            btnNuevoUsuario.Cursor = Cursors.Hand;
            btnNuevoUsuario.FlatAppearance.BorderSize = 0;
            btnNuevoUsuario.FlatStyle = FlatStyle.Flat;
            btnNuevoUsuario.ForeColor = Color.White;
            btnNuevoUsuario.Location = new Point(995, 41);
            btnNuevoUsuario.Name = "btnNuevoUsuario";
            btnNuevoUsuario.Size = new Size(100, 40);
            btnNuevoUsuario.TabIndex = 8;
            btnNuevoUsuario.Text = "+ Nuevo";
            btnNuevoUsuario.UseVisualStyleBackColor = false;
            // 
            // pnlListado
            // 
            pnlListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlListado.BackColor = Color.White;
            pnlListado.BorderStyle = BorderStyle.FixedSingle;
            pnlListado.Controls.Add(lblListadoTitulo);
            pnlListado.Controls.Add(lblListadoDescripcion);
            pnlListado.Controls.Add(dgvUsuarios);
            pnlListado.Controls.Add(lblCantidad);
            pnlListado.Location = new Point(32, 236);
            pnlListado.Name = "pnlListado";
            pnlListado.Size = new Size(1116, 492);
            pnlListado.TabIndex = 2;
            // 
            // lblListadoTitulo
            // 
            lblListadoTitulo.AutoSize = true;
            lblListadoTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblListadoTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblListadoTitulo.Location = new Point(18, 13);
            lblListadoTitulo.Name = "lblListadoTitulo";
            lblListadoTitulo.Size = new Size(185, 25);
            lblListadoTitulo.TabIndex = 0;
            lblListadoTitulo.Text = "Usuarios registrados";
            // 
            // lblListadoDescripcion
            // 
            lblListadoDescripcion.AutoSize = true;
            lblListadoDescripcion.Font = new Font("Segoe UI", 8F);
            lblListadoDescripcion.ForeColor = Color.FromArgb(105, 110, 116);
            lblListadoDescripcion.Location = new Point(180, 18);
            lblListadoDescripcion.Name = "lblListadoDescripcion";
            lblListadoDescripcion.Size = new Size(295, 19);
            lblListadoDescripcion.TabIndex = 1;
            lblListadoDescripcion.Text = "Consulta los usuarios habilitados en el sistema.";
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeRows = false;
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsuarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(82, 88, 94);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsuarios.ColumnHeadersHeight = 40;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(55, 59, 64);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(226, 229, 232);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(40, 44, 48);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvUsuarios.DefaultCellStyle = dataGridViewCellStyle2;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.GridColor = Color.FromArgb(224, 227, 230);
            dgvUsuarios.Location = new Point(18, 50);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.RowTemplate.Height = 38;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(1078, 392);
            dgvUsuarios.TabIndex = 2;
            // 
            // lblCantidad
            // 
            lblCantidad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCantidad.AutoSize = true;
            lblCantidad.Font = new Font("Segoe UI", 8.5F);
            lblCantidad.ForeColor = Color.FromArgb(105, 110, 116);
            lblCantidad.Location = new Point(18, 455);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(85, 20);
            lblCantidad.TabIndex = 3;
            lblCantidad.Text = "0 usuario(s)";
            // 
            // FormUsuarios
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1180, 760);
            Controls.Add(pnlCabecera);
            Controls.Add(pnlFiltros);
            Controls.Add(pnlListado);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormUsuarios";
            Text = "Usuarios";
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            pnlListado.ResumeLayout(false);
            pnlListado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}