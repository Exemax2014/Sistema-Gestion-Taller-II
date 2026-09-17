using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormUsuarios
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlCabecera;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Panel pnlLineaTitulo;
        private Button btnNuevoUsuario;

        private Panel pnlNavegacion;
        private Button btnVistaUsuarios;
        private Button btnVistaPerfiles;

        private Panel pnlVistaUsuarios;
        private Panel pnlFiltros;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Label lblPerfil;
        private ComboBox cmbPerfil;
        private Label lblEstado;
        private ComboBox cmbEstado;
        private Button btnBuscar;
        private Button btnLimpiarFiltros;

        private Panel pnlListado;
        private Label lblListadoTitulo;
        private Label lblListadoDescripcion;
        private DataGridView dgvUsuarios;
        private Label lblCantidad;

        private Panel pnlVistaPerfiles;
        private Panel pnlListaPerfiles;
        private Label lblPerfilesTitulo;
        private Label lblPerfilesDescripcion;
        private ListBox lstPerfiles;
        private Button btnNuevoPerfil;

        private Panel pnlDetallePerfil;
        private Label lblDetallePerfilTitulo;
        private Label lblNombrePerfil;
        private TextBox txtNombrePerfil;
        private Label lblDescripcionPerfil;
        private TextBox txtDescripcionPerfil;
        private Label lblPermisosTitulo;
        private Label lblPermisosDescripcion;
        private FlowLayoutPanel flpPermisos;
        private Button btnGuardarPerfil;
        private Button btnEliminarPerfil;

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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();

            pnlCabecera = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaTitulo = new Panel();
            btnNuevoUsuario = new Button();

            pnlNavegacion = new Panel();
            btnVistaUsuarios = new Button();
            btnVistaPerfiles = new Button();

            pnlVistaUsuarios = new Panel();
            pnlFiltros = new Panel();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            lblPerfil = new Label();
            cmbPerfil = new ComboBox();
            lblEstado = new Label();
            cmbEstado = new ComboBox();
            btnBuscar = new Button();
            btnLimpiarFiltros = new Button();

            pnlListado = new Panel();
            lblListadoTitulo = new Label();
            lblListadoDescripcion = new Label();
            dgvUsuarios = new DataGridView();
            lblCantidad = new Label();

            pnlVistaPerfiles = new Panel();
            pnlListaPerfiles = new Panel();
            lblPerfilesTitulo = new Label();
            lblPerfilesDescripcion = new Label();
            lstPerfiles = new ListBox();
            btnNuevoPerfil = new Button();

            pnlDetallePerfil = new Panel();
            lblDetallePerfilTitulo = new Label();
            lblNombrePerfil = new Label();
            txtNombrePerfil = new TextBox();
            lblDescripcionPerfil = new Label();
            txtDescripcionPerfil = new TextBox();
            lblPermisosTitulo = new Label();
            lblPermisosDescripcion = new Label();
            flpPermisos = new FlowLayoutPanel();
            btnGuardarPerfil = new Button();
            btnEliminarPerfil = new Button();

            pnlCabecera.SuspendLayout();
            pnlNavegacion.SuspendLayout();
            pnlVistaUsuarios.SuspendLayout();
            pnlFiltros.SuspendLayout();
            pnlListado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            pnlVistaPerfiles.SuspendLayout();
            pnlListaPerfiles.SuspendLayout();
            pnlDetallePerfil.SuspendLayout();
            SuspendLayout();

            pnlCabecera.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Controls.Add(lblSubtitulo);
            pnlCabecera.Controls.Add(pnlLineaTitulo);
            pnlCabecera.Controls.Add(btnNuevoUsuario);
            pnlCabecera.Location = new Point(32, 12);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(1243, 88);
            pnlCabecera.TabIndex = 0;

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(45, 49, 54);
            lblTitulo.Location = new Point(0, 4);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(172, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Usuarios";

            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(3, 50);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(328, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Administración de usuarios, perfiles y sucursales.";

            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(2, 74);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(95, 3);
            pnlLineaTitulo.TabIndex = 2;

            btnNuevoUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevoUsuario.BackColor = Color.FromArgb(45, 49, 54);
            btnNuevoUsuario.Cursor = Cursors.Hand;
            btnNuevoUsuario.FlatAppearance.BorderSize = 0;
            btnNuevoUsuario.FlatStyle = FlatStyle.Flat;
            btnNuevoUsuario.ForeColor = Color.White;
            btnNuevoUsuario.Location = new Point(1124, 30);
            btnNuevoUsuario.Name = "btnNuevoUsuario";
            btnNuevoUsuario.Size = new Size(100, 40);
            btnNuevoUsuario.TabIndex = 3;
            btnNuevoUsuario.Text = "+ Nuevo";
            btnNuevoUsuario.UseVisualStyleBackColor = false;

            pnlNavegacion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlNavegacion.Controls.Add(btnVistaUsuarios);
            pnlNavegacion.Controls.Add(btnVistaPerfiles);
            pnlNavegacion.Location = new Point(32, 103);
            pnlNavegacion.Name = "pnlNavegacion";
            pnlNavegacion.Size = new Size(1243, 43);
            pnlNavegacion.TabIndex = 1;

            btnVistaUsuarios.BackColor = Color.FromArgb(190, 137, 45);
            btnVistaUsuarios.Cursor = Cursors.Hand;
            btnVistaUsuarios.FlatAppearance.BorderSize = 0;
            btnVistaUsuarios.FlatStyle = FlatStyle.Flat;
            btnVistaUsuarios.ForeColor = Color.White;
            btnVistaUsuarios.Location = new Point(0, 0);
            btnVistaUsuarios.Name = "btnVistaUsuarios";
            btnVistaUsuarios.Size = new Size(145, 40);
            btnVistaUsuarios.TabIndex = 0;
            btnVistaUsuarios.Text = "Usuarios";
            btnVistaUsuarios.UseVisualStyleBackColor = false;

            btnVistaPerfiles.BackColor = Color.FromArgb(235, 237, 240);
            btnVistaPerfiles.Cursor = Cursors.Hand;
            btnVistaPerfiles.FlatAppearance.BorderSize = 0;
            btnVistaPerfiles.FlatStyle = FlatStyle.Flat;
            btnVistaPerfiles.ForeColor = Color.FromArgb(55, 59, 64);
            btnVistaPerfiles.Location = new Point(151, 0);
            btnVistaPerfiles.Name = "btnVistaPerfiles";
            btnVistaPerfiles.Size = new Size(230, 40);
            btnVistaPerfiles.TabIndex = 1;
            btnVistaPerfiles.Text = "Tipos de usuario y permisos";
            btnVistaPerfiles.UseVisualStyleBackColor = false;

            pnlVistaUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlVistaUsuarios.Controls.Add(pnlFiltros);
            pnlVistaUsuarios.Controls.Add(pnlListado);
            pnlVistaUsuarios.Location = new Point(32, 152);
            pnlVistaUsuarios.Name = "pnlVistaUsuarios";
            pnlVistaUsuarios.Size = new Size(1243, 576);
            pnlVistaUsuarios.TabIndex = 2;

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
            pnlFiltros.Location = new Point(0, 0);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Size = new Size(1243, 73);
            pnlFiltros.TabIndex = 0;

            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(18, 25);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(104, 20);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar usuario";

            txtBuscar.Location = new Point(126, 21);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Usuario, nombre o apellido";
            txtBuscar.Size = new Size(300, 27);
            txtBuscar.TabIndex = 1;

            lblPerfil.AutoSize = true;
            lblPerfil.Location = new Point(473, 25);
            lblPerfil.Name = "lblPerfil";
            lblPerfil.Size = new Size(42, 20);
            lblPerfil.TabIndex = 2;
            lblPerfil.Text = "Perfil";

            cmbPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPerfil.Location = new Point(521, 20);
            cmbPerfil.Name = "cmbPerfil";
            cmbPerfil.Size = new Size(180, 28);
            cmbPerfil.TabIndex = 3;

            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(729, 25);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(54, 20);
            lblEstado.TabIndex = 4;
            lblEstado.Text = "Estado";

            cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstado.Location = new Point(789, 21);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(160, 28);
            cmbEstado.TabIndex = 5;

            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.BackColor = Color.FromArgb(190, 137, 45);
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(990, 15);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(110, 40);
            btnBuscar.TabIndex = 6;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;

            btnLimpiarFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLimpiarFiltros.BackColor = Color.White;
            btnLimpiarFiltros.Cursor = Cursors.Hand;
            btnLimpiarFiltros.FlatAppearance.BorderColor = Color.FromArgb(160, 165, 170);
            btnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
            btnLimpiarFiltros.Location = new Point(1120, 14);
            btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            btnLimpiarFiltros.Size = new Size(105, 40);
            btnLimpiarFiltros.TabIndex = 7;
            btnLimpiarFiltros.Text = "Limpiar";
            btnLimpiarFiltros.UseVisualStyleBackColor = false;

            pnlListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlListado.BackColor = Color.White;
            pnlListado.BorderStyle = BorderStyle.FixedSingle;
            pnlListado.Controls.Add(lblListadoTitulo);
            pnlListado.Controls.Add(lblListadoDescripcion);
            pnlListado.Controls.Add(dgvUsuarios);
            pnlListado.Controls.Add(lblCantidad);
            pnlListado.Location = new Point(0, 79);
            pnlListado.Name = "pnlListado";
            pnlListado.Size = new Size(1243, 497);
            pnlListado.TabIndex = 1;

            lblListadoTitulo.AutoSize = true;
            lblListadoTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblListadoTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblListadoTitulo.Location = new Point(18, 13);
            lblListadoTitulo.Name = "lblListadoTitulo";
            lblListadoTitulo.Size = new Size(185, 25);
            lblListadoTitulo.TabIndex = 0;
            lblListadoTitulo.Text = "Usuarios registrados";

            lblListadoDescripcion.AutoSize = true;
            lblListadoDescripcion.Font = new Font("Segoe UI", 8F);
            lblListadoDescripcion.ForeColor = Color.FromArgb(105, 110, 116);
            lblListadoDescripcion.Location = new Point(210, 18);
            lblListadoDescripcion.Name = "lblListadoDescripcion";
            lblListadoDescripcion.Size = new Size(297, 19);
            lblListadoDescripcion.TabIndex = 1;
            lblListadoDescripcion.Text = "Consulta los usuarios registrados en el sistema.";

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
            dgvUsuarios.Location = new Point(18, 51);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.RowTemplate.Height = 38;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(1205, 406);
            dgvUsuarios.TabIndex = 2;

            lblCantidad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCantidad.AutoSize = true;
            lblCantidad.Font = new Font("Segoe UI", 8.5F);
            lblCantidad.ForeColor = Color.FromArgb(105, 110, 116);
            lblCantidad.Location = new Point(18, 461);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(85, 20);
            lblCantidad.TabIndex = 3;
            lblCantidad.Text = "0 usuario(s)";

            pnlVistaPerfiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlVistaPerfiles.Controls.Add(pnlListaPerfiles);
            pnlVistaPerfiles.Controls.Add(pnlDetallePerfil);
            pnlVistaPerfiles.Location = new Point(32, 152);
            pnlVistaPerfiles.Name = "pnlVistaPerfiles";
            pnlVistaPerfiles.Size = new Size(1243, 576);
            pnlVistaPerfiles.TabIndex = 3;
            pnlVistaPerfiles.Visible = false;

            pnlListaPerfiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlListaPerfiles.BackColor = Color.White;
            pnlListaPerfiles.BorderStyle = BorderStyle.FixedSingle;
            pnlListaPerfiles.Controls.Add(lblPerfilesTitulo);
            pnlListaPerfiles.Controls.Add(lblPerfilesDescripcion);
            pnlListaPerfiles.Controls.Add(lstPerfiles);
            pnlListaPerfiles.Controls.Add(btnNuevoPerfil);
            pnlListaPerfiles.Location = new Point(0, 0);
            pnlListaPerfiles.Name = "pnlListaPerfiles";
            pnlListaPerfiles.Size = new Size(355, 576);
            pnlListaPerfiles.TabIndex = 0;

            lblPerfilesTitulo.AutoSize = true;
            lblPerfilesTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPerfilesTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblPerfilesTitulo.Location = new Point(18, 16);
            lblPerfilesTitulo.Name = "lblPerfilesTitulo";
            lblPerfilesTitulo.Size = new Size(153, 25);
            lblPerfilesTitulo.TabIndex = 0;
            lblPerfilesTitulo.Text = "Tipos de usuario";

            lblPerfilesDescripcion.Font = new Font("Segoe UI", 8.5F);
            lblPerfilesDescripcion.ForeColor = Color.FromArgb(105, 110, 116);
            lblPerfilesDescripcion.Location = new Point(18, 47);
            lblPerfilesDescripcion.Name = "lblPerfilesDescripcion";
            lblPerfilesDescripcion.Size = new Size(315, 42);
            lblPerfilesDescripcion.TabIndex = 1;
            lblPerfilesDescripcion.Text = "Seleccioná un tipo para consultar o modificar sus permisos.";

            lstPerfiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstPerfiles.BorderStyle = BorderStyle.FixedSingle;
            lstPerfiles.Font = new Font("Segoe UI", 9.5F);
            lstPerfiles.FormattingEnabled = true;
            lstPerfiles.ItemHeight = 21;
            lstPerfiles.Location = new Point(18, 103);
            lstPerfiles.Name = "lstPerfiles";
            lstPerfiles.Size = new Size(315, 401);
            lstPerfiles.TabIndex = 2;

            btnNuevoPerfil.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnNuevoPerfil.BackColor = Color.FromArgb(45, 49, 54);
            btnNuevoPerfil.Cursor = Cursors.Hand;
            btnNuevoPerfil.FlatAppearance.BorderSize = 0;
            btnNuevoPerfil.FlatStyle = FlatStyle.Flat;
            btnNuevoPerfil.ForeColor = Color.White;
            btnNuevoPerfil.Location = new Point(18, 518);
            btnNuevoPerfil.Name = "btnNuevoPerfil";
            btnNuevoPerfil.Size = new Size(315, 40);
            btnNuevoPerfil.TabIndex = 3;
            btnNuevoPerfil.Text = "+ Nuevo tipo de usuario";
            btnNuevoPerfil.UseVisualStyleBackColor = false;

            pnlDetallePerfil.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlDetallePerfil.BackColor = Color.White;
            pnlDetallePerfil.BorderStyle = BorderStyle.FixedSingle;
            pnlDetallePerfil.Controls.Add(lblDetallePerfilTitulo);
            pnlDetallePerfil.Controls.Add(lblNombrePerfil);
            pnlDetallePerfil.Controls.Add(txtNombrePerfil);
            pnlDetallePerfil.Controls.Add(lblDescripcionPerfil);
            pnlDetallePerfil.Controls.Add(txtDescripcionPerfil);
            pnlDetallePerfil.Controls.Add(lblPermisosTitulo);
            pnlDetallePerfil.Controls.Add(lblPermisosDescripcion);
            pnlDetallePerfil.Controls.Add(flpPermisos);
            pnlDetallePerfil.Controls.Add(btnGuardarPerfil);
            pnlDetallePerfil.Controls.Add(btnEliminarPerfil);
            pnlDetallePerfil.Location = new Point(367, 0);
            pnlDetallePerfil.Name = "pnlDetallePerfil";
            pnlDetallePerfil.Size = new Size(876, 576);
            pnlDetallePerfil.TabIndex = 1;

            lblDetallePerfilTitulo.AutoSize = true;
            lblDetallePerfilTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblDetallePerfilTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblDetallePerfilTitulo.Location = new Point(22, 16);
            lblDetallePerfilTitulo.Name = "lblDetallePerfilTitulo";
            lblDetallePerfilTitulo.Size = new Size(193, 25);
            lblDetallePerfilTitulo.TabIndex = 0;
            lblDetallePerfilTitulo.Text = "Datos del tipo de usuario";

            lblNombrePerfil.AutoSize = true;
            lblNombrePerfil.Location = new Point(22, 60);
            lblNombrePerfil.Name = "lblNombrePerfil";
            lblNombrePerfil.Size = new Size(64, 20);
            lblNombrePerfil.TabIndex = 1;
            lblNombrePerfil.Text = "Nombre";

            txtNombrePerfil.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNombrePerfil.Location = new Point(22, 84);
            txtNombrePerfil.MaxLength = 50;
            txtNombrePerfil.Name = "txtNombrePerfil";
            txtNombrePerfil.Size = new Size(831, 27);
            txtNombrePerfil.TabIndex = 2;

            lblDescripcionPerfil.AutoSize = true;
            lblDescripcionPerfil.Location = new Point(22, 125);
            lblDescripcionPerfil.Name = "lblDescripcionPerfil";
            lblDescripcionPerfil.Size = new Size(87, 20);
            lblDescripcionPerfil.TabIndex = 3;
            lblDescripcionPerfil.Text = "Descripción";

            txtDescripcionPerfil.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDescripcionPerfil.Location = new Point(22, 149);
            txtDescripcionPerfil.MaxLength = 200;
            txtDescripcionPerfil.Multiline = true;
            txtDescripcionPerfil.Name = "txtDescripcionPerfil";
            txtDescripcionPerfil.ScrollBars = ScrollBars.Vertical;
            txtDescripcionPerfil.Size = new Size(831, 62);
            txtDescripcionPerfil.TabIndex = 4;

            lblPermisosTitulo.AutoSize = true;
            lblPermisosTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPermisosTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblPermisosTitulo.Location = new Point(22, 228);
            lblPermisosTitulo.Name = "lblPermisosTitulo";
            lblPermisosTitulo.Size = new Size(81, 23);
            lblPermisosTitulo.TabIndex = 5;
            lblPermisosTitulo.Text = "Permisos";

            lblPermisosDescripcion.AutoSize = true;
            lblPermisosDescripcion.Font = new Font("Segoe UI", 8F);
            lblPermisosDescripcion.ForeColor = Color.FromArgb(105, 110, 116);
            lblPermisosDescripcion.Location = new Point(112, 232);
            lblPermisosDescripcion.Name = "lblPermisosDescripcion";
            lblPermisosDescripcion.Size = new Size(329, 19);
            lblPermisosDescripcion.TabIndex = 6;
            lblPermisosDescripcion.Text = "Las opciones se cargarán dinámicamente desde la base.";

            flpPermisos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flpPermisos.AutoScroll = true;
            flpPermisos.BackColor = Color.FromArgb(248, 249, 250);
            flpPermisos.BorderStyle = BorderStyle.FixedSingle;
            flpPermisos.FlowDirection = FlowDirection.TopDown;
            flpPermisos.Location = new Point(22, 264);
            flpPermisos.Name = "flpPermisos";
            flpPermisos.Padding = new Padding(12);
            flpPermisos.Size = new Size(831, 235);
            flpPermisos.TabIndex = 7;
            flpPermisos.WrapContents = false;

            btnGuardarPerfil.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGuardarPerfil.BackColor = Color.FromArgb(190, 137, 45);
            btnGuardarPerfil.Cursor = Cursors.Hand;
            btnGuardarPerfil.FlatAppearance.BorderSize = 0;
            btnGuardarPerfil.FlatStyle = FlatStyle.Flat;
            btnGuardarPerfil.ForeColor = Color.White;
            btnGuardarPerfil.Location = new Point(693, 516);
            btnGuardarPerfil.Name = "btnGuardarPerfil";
            btnGuardarPerfil.Size = new Size(160, 40);
            btnGuardarPerfil.TabIndex = 8;
            btnGuardarPerfil.Text = "Guardar cambios";
            btnGuardarPerfil.UseVisualStyleBackColor = false;

            btnEliminarPerfil.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEliminarPerfil.BackColor = Color.White;
            btnEliminarPerfil.Cursor = Cursors.Hand;
            btnEliminarPerfil.FlatAppearance.BorderColor = Color.FromArgb(170, 75, 75);
            btnEliminarPerfil.FlatStyle = FlatStyle.Flat;
            btnEliminarPerfil.ForeColor = Color.FromArgb(150, 55, 55);
            btnEliminarPerfil.Location = new Point(22, 516);
            btnEliminarPerfil.Name = "btnEliminarPerfil";
            btnEliminarPerfil.Size = new Size(150, 40);
            btnEliminarPerfil.TabIndex = 9;
            btnEliminarPerfil.Text = "Dar de baja";
            btnEliminarPerfil.UseVisualStyleBackColor = false;

            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1307, 760);
            Controls.Add(pnlCabecera);
            Controls.Add(pnlNavegacion);
            Controls.Add(pnlVistaUsuarios);
            Controls.Add(pnlVistaPerfiles);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormUsuarios";
            Text = "Usuarios";

            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlNavegacion.ResumeLayout(false);
            pnlVistaUsuarios.ResumeLayout(false);
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            pnlListado.ResumeLayout(false);
            pnlListado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            pnlVistaPerfiles.ResumeLayout(false);
            pnlListaPerfiles.ResumeLayout(false);
            pnlListaPerfiles.PerformLayout();
            pnlDetallePerfil.ResumeLayout(false);
            pnlDetallePerfil.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
