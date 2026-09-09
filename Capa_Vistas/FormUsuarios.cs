namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormUsuarios
    //
    // Vista de listado de usuarios.
    //
    // El Designer maneja solamente el diseño.
    // Las columnas, eventos y comportamiento viven acá.
    // ============================================================

    public partial class FormUsuarios : Form
    {
        public FormUsuarios()
        {
            InitializeComponent();

            ConfigurarGrilla();
            ConfigurarEventos();
            PrepararVistaInicial();
        }


        // ========================================================
        // GRILLA
        // ========================================================

        private void ConfigurarGrilla()
        {
            dgvUsuarios.AutoGenerateColumns =
                false;

            dgvUsuarios.Columns.Clear();


            // ID
            DataGridViewTextBoxColumn colIdUsuario =
                new DataGridViewTextBoxColumn();

            colIdUsuario.Name =
                "colIdUsuario";

            colIdUsuario.HeaderText =
                "ID";

            colIdUsuario.Visible =
                false;


            // USUARIO
            DataGridViewTextBoxColumn colUsuario =
                new DataGridViewTextBoxColumn();

            colUsuario.Name =
                "colUsuario";

            colUsuario.HeaderText =
                "Usuario";

            colUsuario.Width =
                150;


            // NOMBRE
            DataGridViewTextBoxColumn colNombre =
                new DataGridViewTextBoxColumn();

            colNombre.Name =
                "colNombre";

            colNombre.HeaderText =
                "Nombre";

            colNombre.Width =
                160;


            // APELLIDO
            DataGridViewTextBoxColumn colApellido =
                new DataGridViewTextBoxColumn();

            colApellido.Name =
                "colApellido";

            colApellido.HeaderText =
                "Apellido";

            colApellido.Width =
                160;


            // PERFIL
            DataGridViewTextBoxColumn colPerfil =
                new DataGridViewTextBoxColumn();

            colPerfil.Name =
                "colPerfil";

            colPerfil.HeaderText =
                "Perfil";

            colPerfil.Width =
                140;


            // SUCURSAL
            DataGridViewTextBoxColumn colSucursal =
                new DataGridViewTextBoxColumn();

            colSucursal.Name =
                "colSucursal";

            colSucursal.HeaderText =
                "Sucursal";

            colSucursal.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;

            colSucursal.MinimumWidth =
                170;


            // ESTADO
            DataGridViewTextBoxColumn colEstado =
                new DataGridViewTextBoxColumn();

            colEstado.Name =
                "colEstado";

            colEstado.HeaderText =
                "Estado";

            colEstado.Width =
                110;


            // DETALLE
            DataGridViewButtonColumn colDetalle =
                new DataGridViewButtonColumn();

            colDetalle.Name =
                "colDetalle";

            colDetalle.HeaderText =
                "";

            colDetalle.Text =
                "Ver detalle";

            colDetalle.UseColumnTextForButtonValue =
                true;

            colDetalle.FlatStyle =
                FlatStyle.Flat;

            colDetalle.Width =
                120;


            dgvUsuarios.Columns.AddRange(
                colIdUsuario,
                colUsuario,
                colNombre,
                colApellido,
                colPerfil,
                colSucursal,
                colEstado,
                colDetalle
            );
        }


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            btnBuscar.Click +=
                BtnBuscar_Click;

            btnLimpiarFiltros.Click +=
                BtnLimpiarFiltros_Click;

            btnNuevoUsuario.Click +=
                BtnNuevoUsuario_Click;

            txtBuscar.KeyDown +=
                TxtBuscar_KeyDown;

            dgvUsuarios.CellContentClick +=
                DgvUsuarios_CellContentClick;
        }


        // ========================================================
        // VISTA INICIAL
        // ========================================================

        private void PrepararVistaInicial()
        {
            cmbPerfil.Items.Clear();

            cmbPerfil.Items.Add(
                "Todos"
            );

            cmbPerfil.Items.Add(
                "Administrador"
            );

            cmbPerfil.Items.Add(
                "Gerente"
            );

            cmbPerfil.Items.Add(
                "Vendedor"
            );

            cmbPerfil.SelectedIndex =
                0;


            cmbEstado.Items.Clear();

            cmbEstado.Items.Add(
                "Todos"
            );

            cmbEstado.Items.Add(
                "Activos"
            );

            cmbEstado.Items.Add(
                "Inactivos"
            );

            cmbEstado.SelectedIndex =
                0;


            lblCantidad.Text =
                "0 usuario(s)";
        }


        // ========================================================
        // BUSCAR
        // ========================================================

        private void BtnBuscar_Click(
            object? sender,
            EventArgs e)
        {
            ActualizarCantidad();
        }


        private void TxtBuscar_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }


            ActualizarCantidad();

            e.SuppressKeyPress =
                true;
        }


        // ========================================================
        // LIMPIAR
        // ========================================================

        private void BtnLimpiarFiltros_Click(
            object? sender,
            EventArgs e)
        {
            txtBuscar.Clear();


            if (cmbPerfil.Items.Count > 0)
            {
                cmbPerfil.SelectedIndex =
                    0;
            }


            if (cmbEstado.Items.Count > 0)
            {
                cmbEstado.SelectedIndex =
                    0;
            }


            ActualizarCantidad();
        }


        // ========================================================
        // NUEVO USUARIO
        // ========================================================

        private void BtnNuevoUsuario_Click(
            object? sender,
            EventArgs e)
        {
            // Se conectará posteriormente
            // con la vista de detalle.
        }


        // ========================================================
        // DETALLE
        // ========================================================

        private void DgvUsuarios_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (
                e.RowIndex < 0
                ||
                e.ColumnIndex < 0)
            {
                return;
            }


            if (
                dgvUsuarios
                    .Columns[e.ColumnIndex]
                    .Name
                != "colDetalle")
            {
                return;
            }


            // Se conectará posteriormente
            // con FormUsuarioDetalle.
        }


        // ========================================================
        // CANTIDAD
        // ========================================================

        private void ActualizarCantidad()
        {
            lblCantidad.Text =
                $"{dgvUsuarios.Rows.Count} usuario(s)";
        }
    }
}