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
        private readonly PerfilLogica perfilLogica;
        private readonly VentaLogica ventaLogica;
        private readonly FormPrincipal formPrincipal;

        private int? idPerfilEdicion;
        private bool cargandoPerfil;


        public FormUsuarios(
            FormPrincipal formPrincipal)
        {
            InitializeComponent();

            this.formPrincipal =
                formPrincipal;

            usuarioLogica =
                new UsuarioLogica();

            perfilLogica =
                new PerfilLogica();

            ventaLogica =
                new VentaLogica();

            ConfigurarGrilla();
            ConfigurarEventos();
            ConfigurarPermisos();
            MostrarVistaUsuarios();

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
                    Text = string.Empty,
                    UseColumnTextForButtonValue = false,
                    FlatStyle = FlatStyle.Flat,
                    Width = 115,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Alignment =
                                DataGridViewContentAlignment.MiddleCenter,

                            Font =
                                new Font(
                                    "Segoe UI",
                                    8.5F,
                                    FontStyle.Bold
                                ),

                            ForeColor =
                                Color.White,

                            SelectionForeColor =
                                Color.White
                        }
                };


            DataGridViewButtonColumn colVentas =
                new DataGridViewButtonColumn
                {
                    Name = "colVentas",
                    HeaderText = "",
                    Text = "Ventas",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 90,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Alignment =
                                DataGridViewContentAlignment.MiddleCenter
                        }
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

            dgvUsuarios.CellFormatting +=
                DgvUsuarios_CellFormatting;

            btnVistaUsuarios.Click +=
                BtnVistaUsuarios_Click;

            btnVistaPerfiles.Click +=
                BtnVistaPerfiles_Click;

            lstPerfiles.SelectedIndexChanged +=
                LstPerfiles_SelectedIndexChanged;

            btnNuevoPerfil.Click +=
                BtnNuevoPerfil_Click;

            btnGuardarPerfil.Click +=
                BtnGuardarPerfil_Click;

            btnEliminarPerfil.Click +=
                BtnEliminarPerfil_Click;
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

            bool puedeGestionarPermisos =
                perfilLogica.PuedeGestionarPermisos();

            btnVistaPerfiles.Visible =
                puedeGestionarPermisos;

            btnVistaPerfiles.Enabled =
                puedeGestionarPermisos;
        }


        // ========================================================
        // NAVEGACIÓN INTERNA
        // ========================================================

        private void BtnVistaUsuarios_Click(
            object? sender,
            EventArgs e)
        {
            MostrarVistaUsuarios();
        }


        private void BtnVistaPerfiles_Click(
            object? sender,
            EventArgs e)
        {
            if (!perfilLogica.PuedeGestionarPermisos())
            {
                MostrarMensaje(
                    "Acceso no permitido",
                    "No tiene permiso para administrar tipos de usuario y permisos."
                );

                return;
            }

            MostrarVistaPerfiles();
        }


        private void MostrarVistaUsuarios()
        {
            pnlVistaUsuarios.Visible =
                true;

            pnlVistaPerfiles.Visible =
                false;

            btnVistaUsuarios.BackColor =
                Color.FromArgb(190, 137, 45);

            btnVistaUsuarios.ForeColor =
                Color.White;

            btnVistaPerfiles.BackColor =
                Color.FromArgb(235, 237, 240);

            btnVistaPerfiles.ForeColor =
                Color.FromArgb(55, 59, 64);

            lblTitulo.Text =
                "Usuarios";

            lblSubtitulo.Text =
                "Administración de usuarios, perfiles y sucursales.";

            btnNuevoUsuario.Visible =
                usuarioLogica.PuedeCrearUsuario();
        }


        private void MostrarVistaPerfiles()
        {
            pnlVistaUsuarios.Visible =
                false;

            pnlVistaPerfiles.Visible =
                true;

            btnVistaUsuarios.BackColor =
                Color.FromArgb(235, 237, 240);

            btnVistaUsuarios.ForeColor =
                Color.FromArgb(55, 59, 64);

            btnVistaPerfiles.BackColor =
                Color.FromArgb(190, 137, 45);

            btnVistaPerfiles.ForeColor =
                Color.White;

            lblTitulo.Text =
                "Tipos de usuario y permisos";

            lblSubtitulo.Text =
                "Configuración de perfiles y funcionalidades habilitadas.";

            btnNuevoUsuario.Visible =
                false;

            CargarPerfilesGestion();
        }


        // ========================================================
        // GESTIÓN DE PERFILES Y PERMISOS
        // ========================================================

        private void CargarPerfilesGestion(
            int? idSeleccionar = null)
        {
            if (!perfilLogica.PuedeGestionarPermisos())
            {
                lstPerfiles.DataSource =
                    null;

                LimpiarEdicionPerfil();

                return;
            }


            try
            {
                cargandoPerfil =
                    true;


                List<PerfilGestionModelo> perfiles =
                    perfilLogica.ListarPerfiles();


                lstPerfiles.DataSource =
                    perfiles;

                lstPerfiles.DisplayMember =
                    nameof(PerfilGestionModelo.Nombre);

                lstPerfiles.ValueMember =
                    nameof(PerfilGestionModelo.IdPerfil);


                if (perfiles.Count == 0)
                {
                    LimpiarEdicionPerfil();

                    return;
                }


                if (idSeleccionar.HasValue)
                {
                    PerfilGestionModelo? perfil =
                        perfiles.FirstOrDefault(
                            p =>
                                p.IdPerfil ==
                                idSeleccionar.Value
                        );


                    if (perfil != null)
                    {
                        lstPerfiles.SelectedValue =
                            perfil.IdPerfil;
                    }
                }


                if (lstPerfiles.SelectedIndex < 0)
                {
                    lstPerfiles.SelectedIndex =
                        0;
                }
            }
            catch (Exception ex)
            {
                lstPerfiles.DataSource =
                    null;

                LimpiarEdicionPerfil();


                MostrarMensaje(
                    "No se pudieron cargar los tipos de usuario",
                    ex.Message
                );
            }
            finally
            {
                cargandoPerfil =
                    false;
            }


            CargarPerfilSeleccionado();
        }


        private void LstPerfiles_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (cargandoPerfil)
            {
                return;
            }


            CargarPerfilSeleccionado();
        }


        private void CargarPerfilSeleccionado()
        {
            if (
                lstPerfiles.SelectedItem
                is not PerfilGestionModelo perfilListado)
            {
                LimpiarEdicionPerfil();

                return;
            }


            try
            {
                PerfilGestionModelo? perfil =
                    perfilLogica.ObtenerPorId(
                        perfilListado.IdPerfil
                    );


                if (perfil == null)
                {
                    LimpiarEdicionPerfil();

                    return;
                }


                idPerfilEdicion =
                    perfil.IdPerfil;

                txtNombrePerfil.Text =
                    perfil.Nombre;

                txtDescripcionPerfil.Text =
                    perfil.Descripcion;


                bool esPerfilGlobal =
                    perfil.AlcanceGlobal;


                txtNombrePerfil.ReadOnly =
                    esPerfilGlobal;

                txtDescripcionPerfil.ReadOnly =
                    esPerfilGlobal;

                btnGuardarPerfil.Visible =
                    !esPerfilGlobal;

                btnGuardarPerfil.Enabled =
                    !esPerfilGlobal;

                btnEliminarPerfil.Visible =
                    !esPerfilGlobal;

                btnEliminarPerfil.Enabled =
                    !esPerfilGlobal;


                CargarPermisosPerfil(
                    perfil
                );
            }
            catch (Exception ex)
            {
                LimpiarEdicionPerfil();


                MostrarMensaje(
                    "No se pudo cargar el tipo de usuario",
                    ex.Message
                );
            }
        }


        private void CargarPermisosPerfil(
            PerfilGestionModelo perfil)
        {
            flpPermisos.Controls.Clear();


            List<FuncionalidadPerfilModelo> funcionalidades =
                perfilLogica.ObtenerFuncionalidadesPerfil(
                    perfil.IdPerfil
                );


            foreach (FuncionalidadPerfilModelo funcionalidad
                in funcionalidades)
            {
                CheckBox check =
                    CrearCheckPermiso(
                        funcionalidad
                    );


                /*
                    El perfil global conserva todas las funcionalidades.
                    Se muestran seleccionadas y bloqueadas.
                */
                if (perfil.AlcanceGlobal)
                {
                    check.Checked =
                        true;

                    check.Enabled =
                        false;
                }


                flpPermisos.Controls.Add(
                    check
                );
            }
        }


        private void CargarPermisosNuevoPerfil()
        {
            flpPermisos.Controls.Clear();


            List<FuncionalidadPerfilModelo> funcionalidades =
                perfilLogica
                    .ObtenerFuncionalidadesDisponibles();


            foreach (FuncionalidadPerfilModelo funcionalidad
                in funcionalidades)
            {
                CheckBox check =
                    CrearCheckPermiso(
                        funcionalidad
                    );


                check.Checked =
                    false;


                /*
                    PERMISOS_GESTIONAR es exclusivo del Administrador.
                    Se muestra para que la matriz de permisos sea visible,
                    pero no puede seleccionarse para perfiles nuevos.
                */
                if (
                    funcionalidad.Codigo ==
                    "PERMISOS_GESTIONAR")
                {
                    check.Enabled =
                        false;
                }


                flpPermisos.Controls.Add(
                    check
                );
            }
        }


        private CheckBox CrearCheckPermiso(
            FuncionalidadPerfilModelo funcionalidad)
        {
            string texto =
                funcionalidad.Nombre;


            if (!string.IsNullOrWhiteSpace(
                funcionalidad.Descripcion))
            {
                texto +=
                    $" - {funcionalidad.Descripcion}";
            }


            CheckBox check =
                new CheckBox
                {
                    AutoSize = false,
                    Width = Math.Max(
                        250,
                        flpPermisos.ClientSize.Width - 45
                    ),
                    Height = 42,
                    Margin = new Padding(
                        4,
                        3,
                        4,
                        3
                    ),
                    Font =
                        new Font(
                            "Segoe UI",
                            9F
                        ),
                    ForeColor =
                        Color.FromArgb(
                            55,
                            59,
                            64
                        ),
                    Text =
                        texto,
                    Tag =
                        funcionalidad.IdFuncionalidad,
                    Checked =
                        funcionalidad.Asignada,
                    Enabled =
                        !funcionalidad.Bloqueada
                };


            return check;
        }


        private void BtnNuevoPerfil_Click(
            object? sender,
            EventArgs e)
        {
            if (!perfilLogica.PuedeGestionarPermisos())
            {
                MostrarMensaje(
                    "Acceso no permitido",
                    "No tiene permiso para crear tipos de usuario."
                );

                return;
            }


            idPerfilEdicion =
                null;

            cargandoPerfil =
                true;

            lstPerfiles.ClearSelected();

            cargandoPerfil =
                false;


            txtNombrePerfil.ReadOnly =
                false;

            txtDescripcionPerfil.ReadOnly =
                false;

            btnGuardarPerfil.Visible =
                true;

            btnGuardarPerfil.Enabled =
                true;

            txtNombrePerfil.Clear();

            txtDescripcionPerfil.Clear();

            btnEliminarPerfil.Visible =
                false;

            btnEliminarPerfil.Enabled =
                false;


            CargarPermisosNuevoPerfil();


            txtNombrePerfil.Focus();
        }


        private void BtnGuardarPerfil_Click(
            object? sender,
            EventArgs e)
        {
            if (!perfilLogica.PuedeGestionarPermisos())
            {
                MostrarMensaje(
                    "Acceso no permitido",
                    "No tiene permiso para administrar tipos de usuario."
                );

                return;
            }


            try
            {
                if (idPerfilEdicion.HasValue)
                {
                    PerfilGestionModelo? perfilActual =
                        perfilLogica.ObtenerPorId(
                            idPerfilEdicion.Value
                        );


                    if (
                        perfilActual != null
                        &&
                        perfilActual.AlcanceGlobal)
                    {
                        MostrarMensaje(
                            "Perfil protegido",
                            "El perfil global del sistema es de solo lectura."
                        );

                        return;
                    }
                }


                ResultadoPerfil resultado;


                if (idPerfilEdicion.HasValue)
                {
                    resultado =
                        perfilLogica.Modificar(
                            idPerfilEdicion.Value,
                            txtNombrePerfil.Text,
                            txtDescripcionPerfil.Text
                        );
                }
                else
                {
                    resultado =
                        perfilLogica.Crear(
                            txtNombrePerfil.Text,
                            txtDescripcionPerfil.Text
                        );
                }


                if (!resultado.Exitoso)
                {
                    MostrarMensaje(
                        "No se pudo guardar",
                        resultado.Mensaje
                    );

                    return;
                }


                int idPerfil =
                    idPerfilEdicion
                    ??
                    resultado.IdGenerado;


                List<int> permisosSeleccionados =
                    ObtenerPermisosSeleccionados();


                ResultadoPerfil resultadoPermisos =
                    perfilLogica.GuardarPermisos(
                        idPerfil,
                        permisosSeleccionados
                    );


                if (!resultadoPermisos.Exitoso)
                {
                    MostrarMensaje(
                        "Tipo de usuario guardado",
                        "El tipo de usuario se guardó, pero no pudieron actualizarse todos sus permisos. " +
                        resultadoPermisos.Mensaje
                    );


                    CargarPerfilesGestion(
                        idPerfil
                    );

                    CargarPerfiles();

                    return;
                }


                idPerfilEdicion =
                    idPerfil;


                CargarPerfilesGestion(
                    idPerfil
                );

                CargarPerfiles();


                MostrarMensaje(
                    "Cambios guardados",
                    "El tipo de usuario y sus permisos se actualizaron correctamente."
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo guardar",
                    ex.Message
                );
            }
        }


        private void BtnEliminarPerfil_Click(
            object? sender,
            EventArgs e)
        {
            if (!idPerfilEdicion.HasValue)
            {
                return;
            }


            if (!perfilLogica.PuedeGestionarPermisos())
            {
                MostrarMensaje(
                    "Acceso no permitido",
                    "No tiene permiso para dar de baja tipos de usuario."
                );

                return;
            }


            using FormMensaje confirmacion =
                new FormMensaje(
                    "Dar de baja tipo de usuario",
                    "¿Confirma que desea dar de baja el tipo de usuario seleccionado?",
                    "Dar de baja",
                    true
                );


            if (
                confirmacion.ShowDialog(this)
                !=
                DialogResult.OK)
            {
                return;
            }


            try
            {
                ResultadoPerfil resultado =
                    perfilLogica.Eliminar(
                        idPerfilEdicion.Value
                    );


                if (!resultado.Exitoso)
                {
                    MostrarMensaje(
                        "No se pudo dar de baja",
                        resultado.Mensaje
                    );

                    return;
                }


                CargarPerfilesGestion();
                CargarPerfiles();


                MostrarMensaje(
                    "Tipo de usuario dado de baja",
                    resultado.Mensaje
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo dar de baja",
                    ex.Message
                );
            }
        }


        private List<int> ObtenerPermisosSeleccionados()
        {
            List<int> seleccionados =
                new List<int>();


            foreach (Control control
                in flpPermisos.Controls)
            {
                if (
                    control is CheckBox check
                    &&
                    check.Checked
                    &&
                    check.Tag is int idFuncionalidad)
                {
                    seleccionados.Add(
                        idFuncionalidad
                    );
                }
            }


            return seleccionados;
        }


        private void LimpiarEdicionPerfil()
        {
            idPerfilEdicion =
                null;

            txtNombrePerfil.ReadOnly =
                false;

            txtDescripcionPerfil.ReadOnly =
                false;

            btnGuardarPerfil.Visible =
                true;

            btnGuardarPerfil.Enabled =
                true;

            txtNombrePerfil.Clear();

            txtDescripcionPerfil.Clear();

            flpPermisos.Controls.Clear();

            btnEliminarPerfil.Visible =
                false;

            btnEliminarPerfil.Enabled =
                false;
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


        private void DgvUsuarios_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (
                e.RowIndex < 0
                ||
                e.RowIndex >= dgvUsuarios.Rows.Count)
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


            if (nombreColumna == "colEstado")
            {
                e.Value =
                    usuario.Activo
                        ? "Activo"
                        : "Inactivo";

                e.CellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

                e.CellStyle.Font =
                    new Font(
                        "Segoe UI",
                        8.5F,
                        FontStyle.Bold
                    );

                e.CellStyle.ForeColor =
                    Color.White;

                e.CellStyle.SelectionForeColor =
                    Color.White;

                e.CellStyle.BackColor =
                    usuario.Activo
                        ? Color.FromArgb(
                            46,
                            125,
                            74
                        )
                        : Color.FromArgb(
                            165,
                            55,
                            55
                        );

                e.CellStyle.SelectionBackColor =
                    e.CellStyle.BackColor;

                e.FormattingApplied =
                    true;

                return;
            }


            if (nombreColumna == "colDetalle")
            {
                e.Value =
                    usuario.Activo
                        ? "Ver detalle"
                        : "Dar de alta";

                e.CellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

                e.CellStyle.Font =
                    new Font(
                        "Segoe UI",
                        8.5F,
                        FontStyle.Bold
                    );

                e.CellStyle.ForeColor =
                    Color.White;

                e.CellStyle.SelectionForeColor =
                    Color.White;

                e.CellStyle.BackColor =
                    usuario.Activo
                        ? Color.FromArgb(
                            105,
                            110,
                            116
                        )
                        : Color.FromArgb(
                            46,
                            125,
                            74
                        );

                e.CellStyle.SelectionBackColor =
                    e.CellStyle.BackColor;

                e.FormattingApplied =
                    true;
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


            formPrincipal.AbrirFormularioEnPanel(
                new FormUsuarioDetalle(
                    formPrincipal
                ),
                formPrincipal.BotonUsuarios
            );
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
                if (usuario.Activo)
                {
                    AbrirDetalleUsuario(
                        usuario
                    );
                }
                else
                {
                    ReactivarUsuario(
                        usuario
                    );
                }

                return;
            }


            if (nombreColumna == "colVentas")
            {
                AbrirHistorialVentas(
                    usuario
                );
            }
        }


        private void ReactivarUsuario(
            UsuarioListadoModelo usuario)
        {
            if (!usuarioLogica.PuedeReactivarUsuario())
            {
                MostrarMensaje(
                    "Acceso no permitido",
                    "No tiene permiso para dar de alta usuarios."
                );

                return;
            }


            string identificadorUsuario =
                string.IsNullOrWhiteSpace(
                    usuario.NombreUsuario)
                    ? $"{usuario.Nombre} {usuario.Apellido}".Trim()
                    : usuario.NombreUsuario;


            using FormMensaje confirmacion =
                new FormMensaje(
                    "Confirmar alta",
                    $"¿Desea dar de alta nuevamente al usuario \"{identificadorUsuario}\"?",
                    "Dar de alta",
                    true
                );


            confirmacion.StartPosition =
                FormStartPosition.CenterParent;


            if (
                confirmacion.ShowDialog(
                    formPrincipal
                )
                !=
                DialogResult.OK)
            {
                return;
            }


            ResultadoUsuario resultado;


            try
            {
                resultado =
                    usuarioLogica.Reactivar(
                        usuario.IdUsuario
                    );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo dar de alta",
                    ex.Message
                );

                return;
            }


            MostrarMensaje(
                resultado.Exitoso
                    ? "Usuario reactivado"
                    : "No se pudo dar de alta",
                resultado.Mensaje
            );


            if (resultado.Exitoso)
            {
                CargarUsuarios();
            }
        }


        // ========================================================
        // DETALLE / EDICIÓN
        // ========================================================

        private void AbrirDetalleUsuario(
            UsuarioListadoModelo usuario)
        {
            formPrincipal.AbrirFormularioEnPanel(
                new FormUsuarioDetalle(
                    formPrincipal,
                    usuario.IdUsuario
                ),
                formPrincipal.BotonUsuarios
            );
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
            ResultadoConsultaVentas resultado;


            try
            {
                /*
                    Se consulta todo el historial para decidir si tiene
                    sentido abrir la vista. Esto no depende del permiso
                    VENTAS_REALIZAR del perfil del usuario consultado:
                    puede tener ventas históricas aunque hoy ya no venda
                    o incluso aunque esté dado de baja.
                */
                resultado =
                    ventaLogica.ListarPorVendedor(
                        usuario.IdUsuario,
                        DateTime.MinValue,
                        DateTime.Today
                    );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo consultar el historial",
                    ex.Message
                );

                return;
            }


            if (!resultado.Exitoso)
            {
                MostrarMensaje(
                    "No se pudo consultar el historial",
                    resultado.Mensaje
                );

                return;
            }


            if (resultado.Ventas.Count == 0)
            {
                MostrarMensaje(
                    "Sin ventas registradas",
                    "Este usuario no tiene ventas registradas."
                );

                return;
            }


            string nombreCompleto =
                $"{usuario.Nombre} {usuario.Apellido}"
                    .Trim();


            formPrincipal.AbrirFormularioEnPanel(
                new FormListadoVentas(
                    formPrincipal,
                    OrigenHistorialVentas.Usuarios,
                    TipoHistorialVentas.Vendedor,
                    usuario.IdUsuario,
                    nombreCompleto
                ),
                formPrincipal.BotonUsuarios
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


            formMensaje.StartPosition =
                FormStartPosition.CenterParent;


            formMensaje.ShowDialog(
                formPrincipal
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
