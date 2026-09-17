using Capa_Logica;

namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormUsuarios
    //
    // Vista de listado y búsqueda de usuarios.
    //
    // El Designer contiene únicamente estructura y diseño visual.
    // Las columnas, eventos, filtros y carga dinámica viven acá.
    // ============================================================

    public partial class FormUsuarios : Form
    {
        private readonly UsuarioLogica usuarioLogica;


        public FormUsuarios()
        {
            InitializeComponent();

            usuarioLogica =
                new UsuarioLogica();

            ConfigurarGrilla();
            ConfigurarEventos();
            ConfigurarPermisos();

            CargarPerfiles();
            CargarEstados();
            CargarUsuarios();
        }


        // ========================================================
        // GRILLA
        // ========================================================

        private void ConfigurarGrilla()
        {
            dgvUsuarios.AutoGenerateColumns =
                false;

            dgvUsuarios.Columns.Clear();


            DataGridViewTextBoxColumn colIdUsuario =
                new DataGridViewTextBoxColumn
                {
                    Name = "colIdUsuario",
                    DataPropertyName = "IdUsuario",
                    HeaderText = "ID",
                    Visible = false
                };


            DataGridViewTextBoxColumn colUsuario =
                new DataGridViewTextBoxColumn
                {
                    Name = "colUsuario",
                    DataPropertyName = "NombreUsuario",
                    HeaderText = "Usuario",
                    Width = 140
                };


            DataGridViewTextBoxColumn colNombre =
                new DataGridViewTextBoxColumn
                {
                    Name = "colNombre",
                    DataPropertyName = "Nombre",
                    HeaderText = "Nombre",
                    Width = 145
                };


            DataGridViewTextBoxColumn colApellido =
                new DataGridViewTextBoxColumn
                {
                    Name = "colApellido",
                    DataPropertyName = "Apellido",
                    HeaderText = "Apellido",
                    Width = 145
                };


            DataGridViewTextBoxColumn colPerfil =
                new DataGridViewTextBoxColumn
                {
                    Name = "colPerfil",
                    DataPropertyName = "Perfil",
                    HeaderText = "Perfil",
                    Width = 120
                };


            DataGridViewTextBoxColumn colSucursal =
                new DataGridViewTextBoxColumn
                {
                    Name = "colSucursal",
                    DataPropertyName = "Sucursal",
                    HeaderText = "Sucursal",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 150
                };


            DataGridViewTextBoxColumn colEstado =
                new DataGridViewTextBoxColumn
                {
                    Name = "colEstado",
                    DataPropertyName = "Estado",
                    HeaderText = "Estado",
                    Width = 95
                };


            DataGridViewButtonColumn colDetalle =
                new DataGridViewButtonColumn
                {
                    Name = "colDetalle",
                    HeaderText = "",
                    Text = "Ver detalle",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 105
                };


            DataGridViewButtonColumn colVentas =
                new DataGridViewButtonColumn
                {
                    Name = "colVentas",
                    HeaderText = "",
                    Text = "Ventas",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 90
                };


            dgvUsuarios.Columns.AddRange(
                colIdUsuario,
                colUsuario,
                colNombre,
                colApellido,
                colPerfil,
                colSucursal,
                colEstado,
                colDetalle,
                colVentas
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

            cmbPerfil.SelectedIndexChanged +=
                CmbFiltro_SelectedIndexChanged;

            cmbEstado.SelectedIndexChanged +=
                CmbFiltro_SelectedIndexChanged;

            dgvUsuarios.CellContentClick +=
                DgvUsuarios_CellContentClick;
        }


        // ========================================================
        // PERMISOS
        // ========================================================

        private void ConfigurarPermisos()
        {
            btnNuevoUsuario.Visible =
                usuarioLogica.PuedeCrearUsuario();

            btnBuscar.Enabled =
                usuarioLogica.PuedeVerUsuarios();

            btnLimpiarFiltros.Enabled =
                usuarioLogica.PuedeVerUsuarios();

            txtBuscar.Enabled =
                usuarioLogica.PuedeVerUsuarios();

            cmbPerfil.Enabled =
                usuarioLogica.PuedeVerUsuarios();

            cmbEstado.Enabled =
                usuarioLogica.PuedeVerUsuarios();

            dgvUsuarios.Enabled =
                usuarioLogica.PuedeVerUsuarios();
        }


        // ========================================================
        // PERFILES
        // ========================================================

        private void CargarPerfiles()
        {
            List<OpcionPerfil> opciones =
                new List<OpcionPerfil>
                {
                    new OpcionPerfil
                    {
                        IdPerfil = null,
                        Nombre = "Todos"
                    }
                };


            List<PerfilUsuarioModelo> perfiles =
                usuarioLogica.ObtenerPerfiles();


            opciones.AddRange(
                perfiles.Select(
                    perfil =>
                        new OpcionPerfil
                        {
                            IdPerfil =
                                perfil.IdPerfil,

                            Nombre =
                                perfil.Nombre
                        }
                )
            );


            cmbPerfil.DataSource =
                opciones;

            cmbPerfil.DisplayMember =
                nameof(OpcionPerfil.Nombre);

            cmbPerfil.ValueMember =
                nameof(OpcionPerfil.IdPerfil);

            cmbPerfil.SelectedIndex =
                0;
        }


        // ========================================================
        // ESTADOS
        // ========================================================

        private void CargarEstados()
        {
            cmbEstado.DataSource =
                new List<OpcionEstado>
                {
                    new OpcionEstado
                    {
                        Estado =
                            EstadoUsuarioFiltro.Todos,

                        Nombre =
                            "Todos"
                    },

                    new OpcionEstado
                    {
                        Estado =
                            EstadoUsuarioFiltro.Activos,

                        Nombre =
                            "Activos"
                    },

                    new OpcionEstado
                    {
                        Estado =
                            EstadoUsuarioFiltro.Inactivos,

                        Nombre =
                            "Inactivos"
                    }
                };


            cmbEstado.DisplayMember =
                nameof(OpcionEstado.Nombre);

            cmbEstado.ValueMember =
                nameof(OpcionEstado.Estado);

            cmbEstado.SelectedIndex =
                0;
        }


        // ========================================================
        // CARGA DE USUARIOS
        // ========================================================

        private void CargarUsuarios()
        {
            if (!usuarioLogica.PuedeVerUsuarios())
            {
                dgvUsuarios.DataSource =
                    null;

                lblCantidad.Text =
                    "0 usuario(s)";

                return;
            }


            try
            {
                int? idPerfil =
                    ObtenerPerfilSeleccionado();


                EstadoUsuarioFiltro estado =
                    ObtenerEstadoSeleccionado();


                List<UsuarioListadoModelo> usuarios =
                    usuarioLogica.Listar(
                        txtBuscar.Text,
                        idPerfil,
                        estado
                    );


                dgvUsuarios.DataSource =
                    usuarios;


                lblCantidad.Text =
                    $"{usuarios.Count} usuario(s)";
            }
            catch (Exception ex)
            {
                dgvUsuarios.DataSource =
                    null;

                lblCantidad.Text =
                    "0 usuario(s)";


                MostrarMensaje(
                    "No se pudieron cargar los usuarios",
                    ex.Message
                );
            }
        }


        // ========================================================
        // BUSCAR
        // ========================================================

        private void BtnBuscar_Click(
            object? sender,
            EventArgs e)
        {
            CargarUsuarios();
        }


        private void TxtBuscar_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }


            CargarUsuarios();

            e.SuppressKeyPress =
                true;
        }


        private void CmbFiltro_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (!IsHandleCreated)
            {
                return;
            }


            CargarUsuarios();
        }


        // ========================================================
        // LIMPIAR FILTROS
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


            CargarUsuarios();
        }


        // ========================================================
        // NUEVO USUARIO
        // ========================================================

        private void BtnNuevoUsuario_Click(
            object? sender,
            EventArgs e)
        {
            if (!usuarioLogica.PuedeCrearUsuario())
            {
                MostrarMensaje(
                    "Acceso no permitido",
                    "No tiene permiso para registrar usuarios."
                );

                return;
            }


            using FormUsuarioDetalle detalle =
                new FormUsuarioDetalle();


            if (
                detalle.ShowDialog(this)
                ==
                DialogResult.OK)
            {
                CargarUsuarios();
            }
        }


        // ========================================================
        // ACCIONES DE LA GRILLA
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
                    .Rows[e.RowIndex]
                    .DataBoundItem
                is not UsuarioListadoModelo usuario)
            {
                return;
            }


            string nombreColumna =
                dgvUsuarios
                    .Columns[e.ColumnIndex]
                    .Name;


            if (nombreColumna == "colDetalle")
            {
                AbrirDetalleUsuario(
                    usuario
                );

                return;
            }


            if (nombreColumna == "colVentas")
            {
                AbrirHistorialVentas(
                    usuario
                );
            }
        }


        // ========================================================
        // DETALLE / EDICIÓN
        // ========================================================

        private void AbrirDetalleUsuario(
            UsuarioListadoModelo usuario)
        {
            using FormUsuarioDetalle detalle =
                new FormUsuarioDetalle(
                    usuario.IdUsuario
                );


            if (
                detalle.ShowDialog(this)
                ==
                DialogResult.OK)
            {
                CargarUsuarios();
            }
        }


        // ========================================================
        // HISTORIAL DE VENTAS
        //
        // FormListadoVentas y VentaLogica vuelven a validar
        // permisos. La Vista no decide qué ventas puede consultar.
        // ========================================================

        private void AbrirHistorialVentas(
            UsuarioListadoModelo usuario)
        {
            string nombreCompleto =
                $"{usuario.Nombre} {usuario.Apellido}"
                    .Trim();


            using FormListadoVentas historial =
                new FormListadoVentas(
                    TipoHistorialVentas.Vendedor,
                    usuario.IdUsuario,
                    nombreCompleto
                );


            historial.ShowDialog(
                this
            );
        }


        // ========================================================
        // FILTROS
        // ========================================================

        private int? ObtenerPerfilSeleccionado()
        {
            if (
                cmbPerfil.SelectedItem
                is not OpcionPerfil opcion)
            {
                return null;
            }


            return opcion.IdPerfil;
        }


        private EstadoUsuarioFiltro ObtenerEstadoSeleccionado()
        {
            if (
                cmbEstado.SelectedItem
                is not OpcionEstado opcion)
            {
                return EstadoUsuarioFiltro.Todos;
            }


            return opcion.Estado;
        }


        // ========================================================
        // MENSAJES
        // ========================================================

        private void MostrarMensaje(
            string titulo,
            string mensaje)
        {
            using FormMensaje formMensaje =
                new FormMensaje(
                    titulo,
                    mensaje
                );


            formMensaje.ShowDialog(
                this
            );
        }


        // ========================================================
        // OPCIONES VISUALES DE FILTRO
        // ========================================================

        private class OpcionPerfil
        {
            public int? IdPerfil { get; set; }

            public string Nombre { get; set; } =
                string.Empty;
        }


        private class OpcionEstado
        {
            public EstadoUsuarioFiltro Estado { get; set; }

            public string Nombre { get; set; } =
                string.Empty;
        }

        private void pnlCabecera_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
