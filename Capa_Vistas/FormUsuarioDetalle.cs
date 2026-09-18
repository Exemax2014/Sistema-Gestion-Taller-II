using System.Net.Mail;
using System.Text.RegularExpressions;
using Capa_Logica;

namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormUsuarioDetalle
    //
    // Alta y modificación de usuarios.
    //
    // El Designer contiene solamente estructura visual.
    // La carga dinámica, eventos, validaciones y comportamiento
    // se gestionan en este archivo.
    // ============================================================

    public partial class FormUsuarioDetalle : Form
    {
        private readonly UsuarioLogica usuarioLogica;
        private readonly FormPrincipal formPrincipal;

        private readonly int? idUsuario;
        private bool debeVolverAUsuarios;

        private List<PerfilUsuarioModelo> perfiles =
            new List<PerfilUsuarioModelo>();

        private List<SucursalUsuarioModelo> sucursales =
            new List<SucursalUsuarioModelo>();


        // ========================================================
        // CONSTRUCTORES
        // ========================================================

        public FormUsuarioDetalle(
            FormPrincipal formPrincipal)
            : this(
                formPrincipal,
                null
            )
        {
        }


        public FormUsuarioDetalle(
            FormPrincipal formPrincipal,
            int? idUsuario)
        {
            InitializeComponent();

            this.formPrincipal =
                formPrincipal;

            usuarioLogica =
                new UsuarioLogica();

            this.idUsuario =
                idUsuario;

            ConfigurarEventos();

            ConfigurarValidacionesVisuales();

            CargarSexo();

            CargarPerfiles();

            CargarSucursales();

            PrepararVista();

            AjustarDisenoResponsivo();
        }


        // ========================================================
        // ESTADO DEL FORMULARIO
        // ========================================================

        private bool EsEdicion =>
            idUsuario.HasValue;


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            btnGuardar.Click +=
                BtnGuardar_Click;

            btnCancelar.Click +=
                BtnCancelar_Click;

            btnDarBaja.Click +=
                BtnDarBaja_Click;

            cmbPerfil.SelectedIndexChanged +=
                CmbPerfil_SelectedIndexChanged;

            txtNombre.KeyPress +=
                TxtNombre_KeyPress;

            txtApellido.KeyPress +=
                TxtNombre_KeyPress;

            txtDni.KeyPress +=
                TxtDni_KeyPress;

            txtTelefono.KeyPress +=
                TxtTelefono_KeyPress;

            txtUsuario.KeyPress +=
                TxtUsuario_KeyPress;

            Resize +=
                FormUsuarioDetalle_Resize;

            Load +=
                FormUsuarioDetalle_Load;
        }

        // Si la carga falló por una condición excepcional entre la validación
        // previa y la navegación, vuelve al listado sin dejar un detalle vacío.
        private void FormUsuarioDetalle_Load(
            object? sender,
            EventArgs e)
        {
            if (debeVolverAUsuarios)
            {
                VolverAUsuarios();
            }
        }


        // ========================================================
        // VALIDACIONES VISUALES
        // ========================================================

        private void ConfigurarValidacionesVisuales()
        {
            txtNombre.MaxLength =
                100;

            txtApellido.MaxLength =
                100;

            txtDni.MaxLength =
                20;

            txtTelefono.MaxLength =
                30;

            txtUsuario.MaxLength =
                50;

            txtCorreo.MaxLength =
                150;

            txtContrasena.MaxLength =
                100;

            txtConfirmarContrasena.MaxLength =
                100;


            dtpFechaNacimiento.MaxDate =
                DateTime.Today;
        }


        private void TxtNombre_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (
                char.IsControl(
                    e.KeyChar)
                ||
                char.IsLetter(
                    e.KeyChar)
                ||
                char.IsWhiteSpace(
                    e.KeyChar)
                ||
                e.KeyChar == '\''
                ||
                e.KeyChar == '-')
            {
                return;
            }


            e.Handled =
                true;
        }


        private void TxtDni_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (
                char.IsControl(
                    e.KeyChar)
                ||
                char.IsDigit(
                    e.KeyChar))
            {
                return;
            }


            e.Handled =
                true;
        }


        private void TxtTelefono_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (
                char.IsControl(
                    e.KeyChar)
                ||
                char.IsDigit(
                    e.KeyChar)
                ||
                char.IsWhiteSpace(
                    e.KeyChar)
                ||
                e.KeyChar == '+'
                ||
                e.KeyChar == '-'
                ||
                e.KeyChar == '('
                ||
                e.KeyChar == ')')
            {
                return;
            }


            e.Handled =
                true;
        }


        private void TxtUsuario_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (
                char.IsControl(
                    e.KeyChar)
                ||
                char.IsLetterOrDigit(
                    e.KeyChar)
                ||
                e.KeyChar == '.'
                ||
                e.KeyChar == '_'
                ||
                e.KeyChar == '-')
            {
                return;
            }


            e.Handled =
                true;
        }


        // ========================================================
        // VISTA INICIAL
        // ========================================================

        private void PrepararVista()
        {
            if (EsEdicion)
            {
                lblTitulo.Text =
                    "Editar usuario";

                lblSubtitulo.Text =
                    "Modifique los datos permitidos del usuario.";

                lblContrasena.Visible =
                    false;

                txtContrasena.Visible =
                    false;

                lblConfirmarContrasena.Visible =
                    false;

                txtConfirmarContrasena.Visible =
                    false;


                if (!CargarUsuario())
                {
                    debeVolverAUsuarios = true;
                    return;
                }
            }
            else
            {
                lblTitulo.Text =
                    "Nuevo usuario";

                lblSubtitulo.Text =
                    "Complete los datos del usuario y asigne su perfil y sucursal.";


                dtpFechaNacimiento.Checked =
                    false;
            }


            ConfigurarPermisos();

            AjustarDisenoResponsivo();
        }


        private void ConfigurarPermisos()
        {
            bool puedeGuardar =
                EsEdicion
                    ? usuarioLogica.PuedeModificarUsuario()
                    : usuarioLogica.PuedeCrearUsuario();


            btnGuardar.Enabled =
                puedeGuardar;

            bool puedeDarBaja =
                EsEdicion
                &&
                usuarioLogica.PuedeEliminarUsuario()
                &&
                idUsuario.HasValue
                &&
                (
                    !SesionActual.SesionIniciada
                    ||
                    SesionActual.IdUsuario !=
                        idUsuario.Value
                );


            btnDarBaja.Visible =
                puedeDarBaja;

            btnDarBaja.Enabled =
                puedeDarBaja;

        }


        // ========================================================
        // DISEÑO RESPONSIVO
        //
        // El Designer conserva únicamente los controles y una
        // distribución base. Acá se adapta el formulario al tamaño
        // disponible del panel principal.
        // ========================================================

        private void FormUsuarioDetalle_Resize(
            object? sender,
            EventArgs e)
        {
            AjustarDisenoResponsivo();
        }


        private void AjustarDisenoResponsivo()
        {
            if (
                pnlPrincipal.ClientSize.Width <= 0
                ||
                pnlPrincipal.ClientSize.Height <= 0)
            {
                return;
            }


            int anchoDisponible =
                pnlPrincipal.ClientSize.Width;

            int altoDisponible =
                pnlPrincipal.ClientSize.Height;


            int margenHorizontal =
                anchoDisponible >= 1200
                    ? 42
                    : anchoDisponible >= 900
                        ? 30
                        : 20;


            int anchoContenido =
                Math.Max(
                    620,
                    anchoDisponible -
                    (margenHorizontal * 2)
                );


            int anchoMaximo =
                1320;


            if (anchoContenido > anchoMaximo)
            {
                anchoContenido =
                    anchoMaximo;
            }


            int izquierdaContenido =
                Math.Max(
                    margenHorizontal,
                    (anchoDisponible - anchoContenido) / 2
                );


            // CABECERA
            pnlCabecera.Location =
                new Point(
                    izquierdaContenido,
                    18
                );

            pnlCabecera.Size =
                new Size(
                    anchoContenido,
                    82
                );


            // DATOS
            int yDatos =
                114;

            int altoAcciones =
                68;

            int separacionAcciones =
                12;

            int altoDatosDisponible =
                Math.Max(
                    430,
                    altoDisponible -
                    yDatos -
                    altoAcciones -
                    separacionAcciones -
                    18
                );


            pnlDatos.Location =
                new Point(
                    izquierdaContenido,
                    yDatos
                );

            pnlDatos.Size =
                new Size(
                    anchoContenido,
                    altoDatosDisponible
                );


            // ACCIONES
            pnlAcciones.Location =
                new Point(
                    izquierdaContenido,
                    yDatos +
                    altoDatosDisponible +
                    separacionAcciones
                );

            pnlAcciones.Size =
                new Size(
                    anchoContenido,
                    altoAcciones
                );


            AjustarCamposDatos();


            btnCancelar.Location =
                new Point(
                    0,
                    8
                );


            btnGuardar.Location =
                new Point(
                    Math.Max(
                        0,
                        pnlAcciones.ClientSize.Width -
                        btnGuardar.Width
                    ),
                    8
                );


            btnDarBaja.Location =
                new Point(
                    Math.Max(
                        0,
                        btnGuardar.Left -
                        btnDarBaja.Width -
                        12
                    ),
                    8
                );
        }


        private void AjustarCamposDatos()
        {
            int anchoPanel =
                pnlDatos.ClientSize.Width;


            int margen =
                anchoPanel >= 1000
                    ? 30
                    : 22;

            int separacionHorizontal =
                anchoPanel >= 1000
                    ? 28
                    : 20;

            int columnas =
                anchoPanel >= 980
                    ? 3
                    : anchoPanel >= 660
                        ? 2
                        : 1;


            int anchoCampo =
                (
                    anchoPanel
                    -
                    (margen * 2)
                    -
                    (separacionHorizontal * (columnas - 1))
                )
                /
                columnas;


            anchoCampo =
                Math.Max(
                    220,
                    anchoCampo
                );


            int altoFila =
                104;

            int yInicial =
                58;


            Control[] etiquetas =
            {
                lblNombre,
                lblApellido,
                lblDni,
                lblTelefono,
                lblUsuario,
                lblCorreo,
                lblSexo,
                lblFechaNacimiento,
                lblPerfil,
                lblSucursal,
                lblContrasena,
                lblConfirmarContrasena
            };


            Control[] campos =
            {
                txtNombre,
                txtApellido,
                txtDni,
                txtTelefono,
                txtUsuario,
                txtCorreo,
                cmbSexo,
                dtpFechaNacimiento,
                cmbPerfil,
                cmbSucursal,
                txtContrasena,
                txtConfirmarContrasena
            };


            for (int i = 0; i < campos.Length; i++)
            {
                int fila =
                    i / columnas;

                int columna =
                    i % columnas;


                int x =
                    margen +
                    columna *
                    (
                        anchoCampo +
                        separacionHorizontal
                    );


                int yEtiqueta =
                    yInicial +
                    fila * altoFila;


                etiquetas[i].Location =
                    new Point(
                        x,
                        yEtiqueta
                    );


                campos[i].Location =
                    new Point(
                        x,
                        yEtiqueta + 27
                    );


                campos[i].Width =
                    anchoCampo;
            }
        }


        // ========================================================
        // SEXO
        //
        // Es una opción fija de interfaz porque actualmente no
        // existe una tabla catálogo SEXO en la base de datos.
        // ========================================================

        private void CargarSexo()
        {
            cmbSexo.Items.Clear();

            cmbSexo.Items.Add(
                "No especificado"
            );

            cmbSexo.Items.Add(
                "Masculino"
            );

            cmbSexo.Items.Add(
                "Femenino"
            );

            cmbSexo.Items.Add(
                "Otro"
            );


            cmbSexo.SelectedIndex =
                0;
        }


        // ========================================================
        // PERFILES
        // ========================================================

        private void CargarPerfiles()
        {
            try
            {
                perfiles =
                    usuarioLogica.ObtenerPerfiles();


                cmbPerfil.DataSource =
                    perfiles.ToList();

                cmbPerfil.DisplayMember =
                    nameof(
                        PerfilUsuarioModelo.Nombre
                    );

                cmbPerfil.ValueMember =
                    nameof(
                        PerfilUsuarioModelo.IdPerfil
                    );


                if (cmbPerfil.Items.Count > 0)
                {
                    cmbPerfil.SelectedIndex =
                        0;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudieron cargar los perfiles",
                    ex.Message
                );
            }
        }


        // ========================================================
        // SUCURSALES
        // ========================================================

        private void CargarSucursales()
        {
            try
            {
                sucursales =
                    usuarioLogica
                        .ObtenerSucursalesDisponibles();


                cmbSucursal.DataSource =
                    sucursales.ToList();

                cmbSucursal.DisplayMember =
                    nameof(
                        SucursalUsuarioModelo.Nombre
                    );

                cmbSucursal.ValueMember =
                    nameof(
                        SucursalUsuarioModelo.IdSucursal
                    );


                if (cmbSucursal.Items.Count > 0)
                {
                    cmbSucursal.SelectedIndex =
                        0;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudieron cargar las sucursales",
                    ex.Message
                );
            }
        }


        // ========================================================
        // PERFIL / SUCURSAL
        // ========================================================

        private void CmbPerfil_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            ActualizarSucursalSegunPerfil();
        }


        private void ActualizarSucursalSegunPerfil()
        {
            if (
                cmbPerfil.SelectedItem
                is not PerfilUsuarioModelo perfil)
            {
                cmbSucursal.Enabled =
                    false;

                return;
            }


            bool requiereSucursal =
                !perfil.AlcanceGlobal;


            cmbSucursal.Enabled =
                requiereSucursal;

            lblSucursal.Enabled =
                requiereSucursal;


            if (!requiereSucursal)
            {
                cmbSucursal.SelectedIndex =
                    -1;
            }
            else if (
                cmbSucursal.SelectedIndex < 0
                &&
                cmbSucursal.Items.Count > 0)
            {
                cmbSucursal.SelectedIndex =
                    0;
            }
        }


        // ========================================================
        // CARGAR USUARIO
        // ========================================================

        // Carga los datos de edición y traduce fallos técnicos a un mensaje
        // entendible; el llamador decide volver al listado si no hay datos.
        private bool CargarUsuario()
        {
            if (!idUsuario.HasValue)
            {
                return false;
            }


            try
            {
                UsuarioDetalleModelo? usuario =
                    usuarioLogica.ObtenerPorId(
                        idUsuario.Value
                    );


                if (usuario == null)
                {
                    MostrarMensaje(
                        "No se pudo cargar el usuario",
                        "El usuario seleccionado no está disponible."
                    );

                    return false;
                }


                txtNombre.Text =
                    usuario.Nombre;

                txtApellido.Text =
                    usuario.Apellido;

                txtDni.Text =
                    usuario.Dni;

                txtTelefono.Text =
                    usuario.Telefono;

                txtUsuario.Text =
                    usuario.NombreUsuario;

                txtCorreo.Text =
                    usuario.Correo;


                SeleccionarSexo(
                    usuario.Sexo
                );


                if (usuario.FechaNacimiento.HasValue)
                {
                    dtpFechaNacimiento.Value =
                        usuario.FechaNacimiento.Value;

                    dtpFechaNacimiento.Checked =
                        true;
                }
                else
                {
                    dtpFechaNacimiento.Checked =
                        false;
                }


                SeleccionarPerfil(
                    usuario.IdPerfil
                );


                ActualizarSucursalSegunPerfil();


                if (usuario.IdSucursal.HasValue)
                {
                    SeleccionarSucursal(
                        usuario.IdSucursal.Value
                    );
                }

                return true;
            }
            catch
            {
                MostrarMensaje(
                    "No se pudo cargar el usuario",
                    "Ocurrió un problema al cargar los datos. Intentá nuevamente."
                );

                return false;
            }
        }


        private void SeleccionarPerfil(
            int idPerfil)
        {
            for (
                int i = 0;
                i < cmbPerfil.Items.Count;
                i++)
            {
                if (
                    cmbPerfil.Items[i]
                    is PerfilUsuarioModelo perfil
                    &&
                    perfil.IdPerfil == idPerfil)
                {
                    cmbPerfil.SelectedIndex =
                        i;

                    return;
                }
            }
        }


        private void SeleccionarSucursal(
            int idSucursal)
        {
            for (
                int i = 0;
                i < cmbSucursal.Items.Count;
                i++)
            {
                if (
                    cmbSucursal.Items[i]
                    is SucursalUsuarioModelo sucursal
                    &&
                    sucursal.IdSucursal == idSucursal)
                {
                    cmbSucursal.SelectedIndex =
                        i;

                    return;
                }
            }
        }


        private void SeleccionarSexo(
            string sexo)
        {
            if (string.IsNullOrWhiteSpace(
                sexo))
            {
                cmbSexo.SelectedIndex =
                    0;

                return;
            }


            for (
                int i = 0;
                i < cmbSexo.Items.Count;
                i++)
            {
                if (
                    string.Equals(
                        cmbSexo.Items[i]?.ToString(),
                        sexo,
                        StringComparison.OrdinalIgnoreCase
                    ))
                {
                    cmbSexo.SelectedIndex =
                        i;

                    return;
                }
            }


            cmbSexo.SelectedIndex =
                0;
        }


        // ========================================================
        // BAJA LÓGICA
        // ========================================================

        private void BtnDarBaja_Click(
            object? sender,
            EventArgs e)
        {
            if (
                !EsEdicion
                ||
                !idUsuario.HasValue)
            {
                return;
            }


            using FormMensaje confirmacion =
                new FormMensaje(
                    "Dar de baja usuario",
                    "¿Desea dar de baja este usuario? El usuario dejará de poder ingresar al sistema.",
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


            ResultadoUsuario resultado;


            try
            {
                resultado =
                    usuarioLogica.Eliminar(
                        idUsuario.Value
                    );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo dar de baja",
                    ex.Message
                );

                return;
            }


            MostrarMensaje(
                resultado.Exitoso
                    ? "Usuario dado de baja"
                    : "No se pudo dar de baja",
                resultado.Mensaje
            );


            if (!resultado.Exitoso)
            {
                return;
            }


            VolverAUsuarios();
        }


        // ========================================================
        // GUARDAR
        // ========================================================

        private void BtnGuardar_Click(
            object? sender,
            EventArgs e)
        {
            if (!ValidarVista())
            {
                return;
            }


            UsuarioGuardarModelo usuario =
                ConstruirUsuario();


            ResultadoUsuario resultado =
                EsEdicion
                    ? usuarioLogica.Modificar(
                        idUsuario!.Value,
                        usuario
                    )
                    : usuarioLogica.Crear(
                        usuario
                    );


            MostrarMensaje(
                resultado.Exitoso
                    ? "Operación realizada"
                    : "No se pudo guardar",
                resultado.Mensaje
            );


            if (!resultado.Exitoso)
            {
                return;
            }


            VolverAUsuarios();
        }


        // ========================================================
        // VALIDACIÓN PREVENTIVA DE VISTA
        // ========================================================

        private bool ValidarVista()
        {
            if (string.IsNullOrWhiteSpace(
                txtNombre.Text))
            {
                return ErrorCampo(
                    txtNombre,
                    "Debe ingresar el nombre."
                );
            }


            if (!Regex.IsMatch(
                txtNombre.Text.Trim(),
                @"^[\p{L}\s'-]+$"))
            {
                return ErrorCampo(
                    txtNombre,
                    "El nombre contiene caracteres no válidos."
                );
            }


            if (string.IsNullOrWhiteSpace(
                txtApellido.Text))
            {
                return ErrorCampo(
                    txtApellido,
                    "Debe ingresar el apellido."
                );
            }


            if (!Regex.IsMatch(
                txtApellido.Text.Trim(),
                @"^[\p{L}\s'-]+$"))
            {
                return ErrorCampo(
                    txtApellido,
                    "El apellido contiene caracteres no válidos."
                );
            }


            string dni =
                txtDni.Text.Trim();


            if (string.IsNullOrWhiteSpace(
                dni))
            {
                return ErrorCampo(
                    txtDni,
                    "Debe ingresar el DNI."
                );
            }


            if (!dni.All(char.IsDigit))
            {
                return ErrorCampo(
                    txtDni,
                    "El DNI debe contener solamente números."
                );
            }


            if (
                !string.IsNullOrWhiteSpace(
                    txtTelefono.Text)
                &&
                !Regex.IsMatch(
                    txtTelefono.Text.Trim(),
                    @"^[0-9+\-\s()]+$"))
            {
                return ErrorCampo(
                    txtTelefono,
                    "El teléfono contiene caracteres no válidos."
                );
            }


            if (string.IsNullOrWhiteSpace(
                txtUsuario.Text))
            {
                return ErrorCampo(
                    txtUsuario,
                    "Debe ingresar el nombre de usuario."
                );
            }


            if (!Regex.IsMatch(
                txtUsuario.Text.Trim(),
                @"^[A-Za-z0-9._-]+$"))
            {
                return ErrorCampo(
                    txtUsuario,
                    "El usuario solo puede contener letras, números, punto, guion y guion bajo."
                );
            }


            if (string.IsNullOrWhiteSpace(
                txtCorreo.Text))
            {
                return ErrorCampo(
                    txtCorreo,
                    "Debe ingresar el correo."
                );
            }


            if (!EsCorreoValido(
                txtCorreo.Text.Trim()))
            {
                return ErrorCampo(
                    txtCorreo,
                    "El correo ingresado no es válido."
                );
            }


            if (
                dtpFechaNacimiento.Checked
                &&
                dtpFechaNacimiento.Value.Date >
                DateTime.Today)
            {
                return ErrorCampo(
                    dtpFechaNacimiento,
                    "La fecha de nacimiento no puede ser futura."
                );
            }


            if (
                cmbPerfil.SelectedItem
                is not PerfilUsuarioModelo perfil)
            {
                return ErrorCampo(
                    cmbPerfil,
                    "Debe seleccionar un perfil."
                );
            }


            if (
                !perfil.AlcanceGlobal
                &&
                cmbSucursal.SelectedItem
                is not SucursalUsuarioModelo)
            {
                return ErrorCampo(
                    cmbSucursal,
                    "Debe seleccionar una sucursal."
                );
            }


            if (!EsEdicion)
            {
                if (string.IsNullOrWhiteSpace(
                    txtContrasena.Text))
                {
                    return ErrorCampo(
                        txtContrasena,
                        "Debe ingresar una contraseña."
                    );
                }


                if (
                    txtContrasena.Text.Length < 8
                    ||
                    txtContrasena.Text.Length > 100)
                {
                    return ErrorCampo(
                        txtContrasena,
                        "La contraseña debe tener entre 8 y 100 caracteres."
                    );
                }


                if (
                    txtContrasena.Text
                    !=
                    txtConfirmarContrasena.Text)
                {
                    return ErrorCampo(
                        txtConfirmarContrasena,
                        "Las contraseñas no coinciden."
                    );
                }
            }


            return true;
        }


        private UsuarioGuardarModelo ConstruirUsuario()
        {
            PerfilUsuarioModelo perfil =
                (PerfilUsuarioModelo)
                    cmbPerfil.SelectedItem!;


            int? idSucursal =
                perfil.AlcanceGlobal
                    ? null
                    : (
                        cmbSucursal.SelectedItem
                        as SucursalUsuarioModelo
                    )?.IdSucursal;


            string sexo =
                cmbSexo.SelectedIndex <= 0
                    ? string.Empty
                    : cmbSexo.SelectedItem?.ToString()
                        ?? string.Empty;


            return new UsuarioGuardarModelo
            {
                IdPerfil =
                    perfil.IdPerfil,

                IdSucursal =
                    idSucursal,

                Nombre =
                    txtNombre.Text.Trim(),

                Apellido =
                    txtApellido.Text.Trim(),

                Dni =
                    txtDni.Text.Trim(),

                Telefono =
                    txtTelefono.Text.Trim(),

                NombreUsuario =
                    txtUsuario.Text.Trim(),

                Correo =
                    txtCorreo.Text.Trim(),

                Sexo =
                    sexo,

                FechaNacimiento =
                    dtpFechaNacimiento.Checked
                        ? dtpFechaNacimiento.Value.Date
                        : null,

                Contrasena =
                    EsEdicion
                        ? string.Empty
                        : txtContrasena.Text
            };
        }


        // ========================================================
        // CANCELAR
        // ========================================================

        private void BtnCancelar_Click(
            object? sender,
            EventArgs e)
        {
            VolverAUsuarios();
        }


        private void VolverAUsuarios()
        {
            formPrincipal.AbrirFormularioEnPanel(
                new FormUsuarios(
                    formPrincipal
                ),
                formPrincipal.BotonUsuarios
            );
        }


        // ========================================================
        // AUXILIARES
        // ========================================================

        private bool ErrorCampo(
            Control control,
            string mensaje)
        {
            MostrarMensaje(
                "Datos incompletos o inválidos",
                mensaje
            );


            if (control.CanFocus)
            {
                control.Focus();
            }


            return false;
        }


        private static bool EsCorreoValido(
            string correo)
        {
            try
            {
                MailAddress direccion =
                    new MailAddress(
                        correo
                    );


                return string.Equals(
                    direccion.Address,
                    correo,
                    StringComparison.OrdinalIgnoreCase
                );
            }
            catch
            {
                return false;
            }
        }


        // Muestra mensajes del detalle con FormPrincipal como owner para
        // conservar el centrado dentro de la navegación embebida.
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
    }
}
