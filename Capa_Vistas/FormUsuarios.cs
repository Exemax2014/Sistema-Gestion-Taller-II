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
        private readonly SucursalLogica sucursalLogica;
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

            sucursalLogica =
                new SucursalLogica();

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

        // Configura las columnas dinámicas y normaliza la presentación
        // para mantener una lectura consistente de datos, estados y acciones.
        private void ConfigurarGrilla()
        {
            dgvUsuarios.AutoGenerateColumns =
                false;

            dgvUsuarios.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvUsuarios.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;

            dgvUsuarios.DefaultCellStyle.Padding =
                new Padding(8, 0, 8, 0);

            dgvUsuarios.RowTemplate.Height =
                40;

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
                    Width = 140,
                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Alignment =
                                DataGridViewContentAlignment.MiddleCenter
                        }
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
                    Width = 120,
                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Alignment =
                                DataGridViewContentAlignment.MiddleCenter
                        }
                };


            DataGridViewTextBoxColumn colSucursal =
                new DataGridViewTextBoxColumn
                {
                    Name = "colSucursal",
                    DataPropertyName = "Sucursal",
                    HeaderText = "Sucursal",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 150,
                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Alignment =
                                DataGridViewContentAlignment.MiddleCenter
                        }
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

                            Padding =
                                Padding.Empty,

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
                                DataGridViewContentAlignment.MiddleCenter,

                            Padding =
                                Padding.Empty
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

            btnVistaSucursales.Click +=
                BtnVistaSucursales_Click;

            btnNuevaSucursal.Click +=
                BtnNuevaSucursal_Click;

            btnCancelarSucursal.Click +=
                BtnCancelarSucursal_Click;

            btnGuardarSucursal.Click +=
                BtnGuardarSucursal_Click;

            cmbSucursalProvincia.SelectedIndexChanged +=
                CmbSucursalProvincia_SelectedIndexChanged;

            flpSucursales.Resize +=
                FlpSucursales_Resize;

            lstPerfiles.SelectedIndexChanged +=
                LstPerfiles_SelectedIndexChanged;

            btnNuevoPerfil.Click +=
                BtnNuevoPerfil_Click;

            btnGuardarPerfil.Click +=
                BtnGuardarPerfil_Click;

            btnEliminarPerfil.Click +=
                BtnEliminarPerfil_Click;

            txtNombrePerfil.KeyPress +=
                NombrePerfil_KeyPress;

            txtDescripcionPerfil.KeyPress +=
                DescripcionPerfil_KeyPress;
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

            bool puedeVerSucursales =
                sucursalLogica.PuedeVerSucursales();

            btnVistaSucursales.Enabled =
                puedeVerSucursales;

            btnNuevaSucursal.Visible =
                sucursalLogica.PuedeCrearSucursal();
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


        // Abre la gestión de sucursales solo cuando la sesión posee el permiso de consulta.
        private void BtnVistaSucursales_Click(
            object? sender,
            EventArgs e)
        {
            if (!sucursalLogica.PuedeVerSucursales())
            {
                MostrarMensaje(
                    "Acceso no permitido",
                    "No tiene permiso para consultar sucursales."
                );

                return;
            }

            MostrarVistaSucursales();
        }


        private void MostrarVistaUsuarios()
        {
            pnlVistaUsuarios.Visible =
                true;

            pnlVistaPerfiles.Visible =
                false;

            pnlVistaSucursales.Visible =
                false;

            btnVistaUsuarios.BackColor =
                Color.FromArgb(190, 137, 45);

            btnVistaUsuarios.ForeColor =
                Color.White;

            btnVistaPerfiles.BackColor =
                Color.FromArgb(235, 237, 240);

            btnVistaPerfiles.ForeColor =
                Color.FromArgb(55, 59, 64);

            btnVistaSucursales.BackColor =
                Color.FromArgb(235, 237, 240);

            btnVistaSucursales.ForeColor =
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

            pnlVistaSucursales.Visible =
                false;

            btnVistaUsuarios.BackColor =
                Color.FromArgb(235, 237, 240);

            btnVistaUsuarios.ForeColor =
                Color.FromArgb(55, 59, 64);

            btnVistaPerfiles.BackColor =
                Color.FromArgb(190, 137, 45);

            btnVistaPerfiles.ForeColor =
                Color.White;

            btnVistaSucursales.BackColor =
                Color.FromArgb(235, 237, 240);

            btnVistaSucursales.ForeColor =
                Color.FromArgb(55, 59, 64);

            lblTitulo.Text =
                "Tipos de usuario y permisos";

            lblSubtitulo.Text =
                "Configuración de perfiles y funcionalidades habilitadas.";

            btnNuevoUsuario.Visible =
                false;

            CargarPerfilesGestion();
        }


        // Alterna a la vista de sucursales y refresca el resumen por perfil.
        private void MostrarVistaSucursales()
        {
            pnlVistaUsuarios.Visible = false;
            pnlVistaPerfiles.Visible = false;
            pnlVistaSucursales.Visible = true;

            btnVistaUsuarios.BackColor = Color.FromArgb(235, 237, 240);
            btnVistaUsuarios.ForeColor = Color.FromArgb(55, 59, 64);
            btnVistaPerfiles.BackColor = Color.FromArgb(235, 237, 240);
            btnVistaPerfiles.ForeColor = Color.FromArgb(55, 59, 64);
            btnVistaSucursales.BackColor = Color.FromArgb(190, 137, 45);
            btnVistaSucursales.ForeColor = Color.White;

            lblTitulo.Text = "Sucursales";
            lblSubtitulo.Text = "Consulta las sucursales activas y la distribución de usuarios por perfil.";
            btnNuevoUsuario.Visible = false;

            MostrarListadoSucursales();
            CargarSucursales();
        }


        // Carga bloques de sucursales y sus perfiles sin asumir una lista fija de roles.
        private void CargarSucursales()
        {
            flpSucursales.SuspendLayout();
            flpSucursales.Controls.Clear();

            if (!sucursalLogica.PuedeVerSucursales())
            {
                lblCantidadSucursales.Text = "No tiene permiso para consultar sucursales.";
                flpSucursales.ResumeLayout();
                return;
            }

            try
            {
                List<SucursalResumenModelo> sucursales =
                    sucursalLogica.ObtenerResumenUsuariosPorPerfil();

                foreach (SucursalResumenModelo sucursal in sucursales)
                {
                    flpSucursales.Controls.Add(CrearBloqueSucursal(sucursal));
                }

                lblCantidadSucursales.Text =
                    $"{sucursales.Count} sucursal(es) activa(s)";

                AjustarAnchoBloquesSucursales();
            }
            catch (Exception ex)
            {
                lblCantidadSucursales.Text = "No se pudieron cargar las sucursales.";
                MostrarMensaje("No se pudieron cargar las sucursales", ex.Message);
            }
            finally
            {
                flpSucursales.ResumeLayout();
            }
        }


        // Crea el bloque visual de una sucursal con los perfiles obtenidos dinámicamente.
        private Panel CrearBloqueSucursal(SucursalResumenModelo sucursal)
        {
            int altoResumen = Math.Max(1, sucursal.UsuariosPorPerfil.Count) * 24;
            Panel bloque = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Height = 105 + altoResumen,
                Margin = new Padding(0, 0, 0, 12),
                Tag = sucursal.IdSucursal
            };

            Label lblNombre = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 59, 64),
                Location = new Point(18, 15),
                Text = sucursal.Nombre
            };

            string ubicacion = string.Join(", ", new[]
            {
                sucursal.Calle,
                sucursal.Localidad,
                sucursal.Provincia
            }.Where(texto => !string.IsNullOrWhiteSpace(texto)));

            Label lblUbicacion = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(105, 110, 116),
                Location = new Point(18, 45),
                Text = string.IsNullOrWhiteSpace(ubicacion)
                    ? "Sin dirección registrada"
                    : ubicacion
            };

            Label lblResumen = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(190, 137, 45),
                Location = new Point(18, 73),
                Text = "Usuarios activos por perfil"
            };

            FlowLayoutPanel perfiles = new FlowLayoutPanel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                FlowDirection = FlowDirection.LeftToRight,
                Location = new Point(18, 99),
                Size = new Size(Math.Max(300, flpSucursales.ClientSize.Width - 55), altoResumen),
                WrapContents = true
            };

            foreach (PerfilSucursalResumenModelo perfil in sucursal.UsuariosPorPerfil)
            {
                perfiles.Controls.Add(new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.FromArgb(55, 59, 64),
                    Margin = new Padding(0, 0, 28, 4),
                    Text = $"{perfil.Perfil}: {perfil.CantidadUsuarios}"
                });
            }

            bloque.Controls.Add(lblNombre);
            bloque.Controls.Add(lblUbicacion);
            bloque.Controls.Add(lblResumen);
            bloque.Controls.Add(perfiles);
            return bloque;
        }


        // Ajusta el ancho de los bloques al área visible sin trasladar layout dinámico al Designer.
        private void FlpSucursales_Resize(object? sender, EventArgs e)
        {
            AjustarAnchoBloquesSucursales();
        }


        // Mantiene las tarjetas alineadas y evita desbordes al cambiar el tamaño del formulario.
        private void AjustarAnchoBloquesSucursales()
        {
            int ancho = Math.Max(360, flpSucursales.ClientSize.Width - 24);

            foreach (Control control in flpSucursales.Controls)
            {
                if (control is Panel bloque)
                {
                    bloque.Width = ancho;
                }
            }
        }


        // Abre el formulario interno de alta y carga sus catálogos dinámicos de ubicación.
        private void BtnNuevaSucursal_Click(object? sender, EventArgs e)
        {
            if (!sucursalLogica.PuedeCrearSucursal())
            {
                MostrarMensaje("Acceso no permitido", "No tiene permiso para registrar sucursales.");
                return;
            }

            LimpiarFormularioSucursal();
            pnlListadoSucursales.Visible = false;
            pnlFormularioSucursal.Visible = true;
            CargarProvinciasSucursal();
            txtNombreSucursal.Focus();
        }


        // Regresa al listado sin persistir los cambios ingresados en el formulario interno.
        private void BtnCancelarSucursal_Click(object? sender, EventArgs e)
        {
            MostrarListadoSucursales();
        }


        // Valida y registra la nueva sucursal después de resolver la localidad elegida o creada.
        private void BtnGuardarSucursal_Click(object? sender, EventArgs e)
        {
            int? idProvincia = ObtenerIdSucursalSeleccionado(cmbSucursalProvincia);

            if (!ValidarSucursalEnVista(idProvincia) || !idProvincia.HasValue)
            {
                return;
            }

            if (!IntentarResolverLocalidadSucursal(idProvincia.Value, out int idLocalidad))
            {
                return;
            }

            ResultadoSucursal resultado;
            try
            {
                resultado = sucursalLogica.Alta(
                    txtNombreSucursal.Text,
                    idProvincia,
                    idLocalidad,
                    txtDireccionSucursal.Text
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudo crear la sucursal", ex.Message);
                return;
            }

            MostrarMensaje(
                resultado.Exitoso ? "Sucursal creada" : "No se pudo crear la sucursal",
                resultado.Mensaje
            );

            if (resultado.Exitoso)
            {
                MostrarListadoSucursales();
                CargarSucursales();
            }
        }


        // Recarga las localidades y limpia la selección al cambiar la provincia de la sucursal.
        private void CmbSucursalProvincia_SelectedIndexChanged(object? sender, EventArgs e)
        {
            CargarLocalidadesSucursal(ObtenerIdSucursalSeleccionado(cmbSucursalProvincia) ?? 0, true);
        }


        // Obtiene el catálogo cerrado de provincias para el alta de sucursal.
        private void CargarProvinciasSucursal()
        {
            List<OpcionSucursalModelo> provincias = sucursalLogica.ObtenerProvincias();
            provincias.Insert(0, new OpcionSucursalModelo { Id = 0, Nombre = "Seleccioná una provincia" });

            cmbSucursalProvincia.DataSource = provincias;
            cmbSucursalProvincia.DisplayMember = nameof(OpcionSucursalModelo.Nombre);
            cmbSucursalProvincia.ValueMember = nameof(OpcionSucursalModelo.Id);
            cmbSucursalProvincia.SelectedIndex = 0;
        }


        // Carga localidades de la provincia y configura sugerencias sin permitir cruces entre provincias.
        private void CargarLocalidadesSucursal(int idProvincia, bool limpiarSeleccion)
        {
            List<OpcionSucursalModelo> localidades = idProvincia > 0
                ? sucursalLogica.ObtenerLocalidades(idProvincia)
                : new List<OpcionSucursalModelo>();

            cmbSucursalLocalidad.DataSource = null;
            cmbSucursalLocalidad.DisplayMember = nameof(OpcionSucursalModelo.Nombre);
            cmbSucursalLocalidad.ValueMember = nameof(OpcionSucursalModelo.Id);
            cmbSucursalLocalidad.DataSource = localidades;

            AutoCompleteStringCollection sugerencias = new();
            sugerencias.AddRange(localidades.Select(localidad => localidad.Nombre).ToArray());
            cmbSucursalLocalidad.AutoCompleteCustomSource = sugerencias;
            cmbSucursalLocalidad.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbSucursalLocalidad.AutoCompleteSource = AutoCompleteSource.CustomSource;

            if (limpiarSeleccion)
            {
                cmbSucursalLocalidad.SelectedIndex = -1;
                cmbSucursalLocalidad.Text = string.Empty;
            }
        }


        // Reutiliza una localidad existente o solicita confirmación antes de crearla por la capa lógica.
        private bool IntentarResolverLocalidadSucursal(int idProvincia, out int idLocalidad)
        {
            idLocalidad = 0;
            string nombre = cmbSucursalLocalidad.Text.Trim();
            OpcionSucursalModelo? existente = sucursalLogica.BuscarLocalidad(idProvincia, nombre);

            if (existente != null)
            {
                idLocalidad = existente.Id;
                cmbSucursalLocalidad.SelectedValue = idLocalidad;
                return true;
            }

            using FormMensaje confirmacion = new FormMensaje(
                "Nueva localidad",
                $"La localidad \"{nombre}\" no existe en la provincia seleccionada. ¿Deseás crearla?",
                "Crear localidad",
                true
            );

            confirmacion.StartPosition = FormStartPosition.CenterParent;
            if (confirmacion.ShowDialog(formPrincipal) != DialogResult.OK)
            {
                return false;
            }

            ResultadoSucursal resultado = sucursalLogica.ObtenerOCrearLocalidad(idProvincia, nombre);
            if (!resultado.Exitoso)
            {
                MostrarMensaje("Localidad", resultado.Mensaje);
                return false;
            }

            CargarLocalidadesSucursal(idProvincia, false);
            cmbSucursalLocalidad.SelectedValue = resultado.IdGenerado;
            idLocalidad = resultado.IdGenerado;
            return true;
        }


        // Repite en la Vista los obligatorios y longitudes antes de delegar la validación autoritativa.
        private bool ValidarSucursalEnVista(int? idProvincia)
        {
            string nombre = NormalizarEspaciosSucursal(txtNombreSucursal.Text);
            string direccion = NormalizarEspaciosSucursal(txtDireccionSucursal.Text);
            string localidad = NormalizarEspaciosSucursal(cmbSucursalLocalidad.Text);
            string? error = string.IsNullOrWhiteSpace(nombre)
                ? "El nombre de la sucursal es obligatorio."
                : nombre.Length > 100
                    ? "El nombre de la sucursal no puede superar los 100 caracteres."
                    : !idProvincia.HasValue || idProvincia.Value <= 0
                        ? "Seleccioná una provincia válida."
                        : string.IsNullOrWhiteSpace(localidad) || localidad.Length > 100
                            ? "Seleccioná o ingresá una localidad válida."
                            : string.IsNullOrWhiteSpace(direccion) || direccion.Length > 150
                                ? "La dirección es obligatoria y no puede superar los 150 caracteres."
                                : null;

            if (error == null)
            {
                txtNombreSucursal.Text = NormalizarEspaciosSucursal(txtNombreSucursal.Text);
                txtDireccionSucursal.Text = NormalizarEspaciosSucursal(txtDireccionSucursal.Text);
                return true;
            }

            MostrarMensaje("Sucursal", error);
            return false;
        }


        // Restablece el formulario para evitar reutilizar una ubicación de una alta anterior.
        private void LimpiarFormularioSucursal()
        {
            txtNombreSucursal.Clear();
            txtDireccionSucursal.Clear();
            chkSucursalActiva.Checked = true;
            cmbSucursalProvincia.DataSource = null;
            cmbSucursalLocalidad.DataSource = null;
            cmbSucursalLocalidad.Text = string.Empty;
        }


        // Muestra el listado y oculta el formulario de alta dentro de la misma pestaña.
        private void MostrarListadoSucursales()
        {
            pnlFormularioSucursal.Visible = false;
            pnlListadoSucursales.Visible = true;
        }


        // Obtiene el ID actual del ComboBox sin depender de posiciones o IDs fijos.
        private static int? ObtenerIdSucursalSeleccionado(ComboBox combo)
        {
            return combo.SelectedValue is int id ? id : null;
        }


        // Reduce espacios repetidos para reflejar el valor que validará y almacenará la lógica.
        private static string NormalizarEspaciosSucursal(string? valor)
        {
            return string.Join(' ', (valor ?? string.Empty).Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
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

                CargarDestinosAvisoPerfil(perfil);
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

            flpDestinosAviso.Controls.Clear();
        }


        // Carga perfiles destino configurables sin asumir roles ni nombres predefinidos.
        private void CargarDestinosAvisoPerfil(PerfilGestionModelo perfil)
        {
            flpDestinosAviso.Controls.Clear();

            foreach (PerfilAvisoDestinoModelo destino in perfilLogica.ObtenerDestinosAviso(perfil.IdPerfil))
            {
                flpDestinosAviso.Controls.Add(new CheckBox
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.FromArgb(55, 59, 64),
                    Text = destino.Nombre,
                    Tag = destino.IdPerfil,
                    Checked = destino.Asignado,
                    Enabled = !perfil.AlcanceGlobal,
                    Margin = new Padding(4, 2, 4, 2)
                });
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

        // Valida y normaliza el perfil antes de conservar sus permisos ya seleccionados.
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

            if (!ValidarPerfilEnVista())
            {
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


                List<int> permisosSeleccionados =
                    ObtenerPermisosSeleccionados();


                ResultadoPerfil resultado =
                    perfilLogica.Guardar(
                        idPerfilEdicion,
                        txtNombrePerfil.Text,
                        txtDescripcionPerfil.Text,
                        permisosSeleccionados
                    );


                if (!resultado.Exitoso)
                {
                    MostrarMensaje(
                        "No se pudo guardar",
                        resultado.Mensaje
                    );

                    return;
                }

                ResultadoPerfil destinosAviso = perfilLogica.GuardarDestinosAviso(
                    resultado.IdGenerado,
                    ObtenerDestinosAvisoSeleccionados()
                );

                if (!destinosAviso.Exitoso)
                {
                    MostrarMensaje("No se pudieron guardar los destinos de avisos", destinosAviso.Mensaje);
                    return;
                }


                int idPerfil = resultado.IdGenerado;


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


        // Previene caracteres ajenos al nombre sin sustituir la validación autoritativa de Lógica.
        private static void NombrePerfil_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || char.IsLetterOrDigit(e.KeyChar) ||
                e.KeyChar == ' ' || e.KeyChar == '-' || e.KeyChar == '\'')
            {
                return;
            }

            e.Handled = true;
        }


        // Evita saltos de línea y otros controles en una descripción que se guarda como texto simple.
        private static void DescripcionPerfil_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        // Repite en la Vista las reglas de formato y normaliza el texto antes de solicitar el guardado.
        private bool ValidarPerfilEnVista()
        {
            string nombreOriginal = txtNombrePerfil.Text;
            string descripcionOriginal = txtDescripcionPerfil.Text;
            string? error = perfilLogica.ValidarDatosPerfil(nombreOriginal, descripcionOriginal);

            if (error != null)
            {
                MostrarMensaje("Perfil", error);
                return false;
            }

            txtNombrePerfil.Text = NormalizarNombrePerfil(nombreOriginal);
            txtDescripcionPerfil.Text = descripcionOriginal.Trim();
            return true;
        }


        // Reduce espacios repetidos para que la Vista presente el mismo valor que guardará la Lógica.
        private static string NormalizarNombrePerfil(string? valor)
        {
            return string.Join(
                ' ',
                (valor ?? string.Empty)
                    .Trim()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            );
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


        // Reúne los perfiles destino seleccionados para persistir la matriz de avisos.
        private List<int> ObtenerDestinosAvisoSeleccionados()
        {
            return flpDestinosAviso.Controls
                .OfType<CheckBox>()
                .Where(check => check.Checked)
                .Select(check => check.Tag)
                .OfType<int>()
                .ToList();
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

            flpDestinosAviso.Controls.Clear();

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


        // Conserva los colores de estado y acción, y elimina el padding general
        // para que esas celdas queden centradas visualmente.
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

                e.CellStyle.Padding =
                    Padding.Empty;

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

                e.CellStyle.Padding =
                    Padding.Empty;

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

        // Comprueba la disponibilidad del usuario antes de reemplazar el listado
        // por el detalle, evitando una vista vacía si la carga no es posible.
        private void AbrirDetalleUsuario(
            UsuarioListadoModelo usuario)
        {
            try
            {
                if (usuarioLogica.ObtenerPorId(usuario.IdUsuario) == null)
                {
                    MostrarMensaje(
                        "No se pudo cargar el usuario",
                        "El usuario seleccionado ya no está disponible."
                    );

                    return;
                }
            }
            catch
            {
                MostrarMensaje(
                    "No se pudo cargar el usuario",
                    "Ocurrió un problema al obtener los datos del usuario. Intentá nuevamente."
                );

                return;
            }

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
