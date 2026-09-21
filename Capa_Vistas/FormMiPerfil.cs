using Capa_Logica;

namespace Capa_Vistas
{
    public partial class FormMiPerfil : Form
    {
        private readonly FormPrincipal formPrincipal;
        private readonly UsuarioLogica usuarioLogica = new();
        private UsuarioDetalleModelo? perfilActual;

        // Inicializa la edición personal sin incluir controles administrativos de usuario.
        public FormMiPerfil(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            ConfigurarEstilos();
            ConfigurarEventos();
            AjustarLayout();
            CargarPerfil();
        }

        // Aplica el estilo del sistema a los controles declarados en el Designer.
        private void ConfigurarEstilos()
        {
            foreach (Label etiqueta in new[] { lblDatos, lblNombre, lblApellido, lblDni, lblTelefono, lblUsuario, lblCorreo, lblSexo, lblFecha, lblSeguridad, lblNueva, lblConfirmar, lblActual })
            {
                etiqueta.AutoSize = true;
                etiqueta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                etiqueta.ForeColor = Color.FromArgb(230, 232, 235);
            }
            foreach (TextBox campo in new[] { txtNombre, txtApellido, txtDni, txtTelefono, txtUsuario, txtCorreo, txtNueva, txtConfirmar, txtActual })
            {
                campo.BorderStyle = BorderStyle.FixedSingle;
                campo.Font = new Font("Segoe UI", 10F);
                campo.BackColor = Color.FromArgb(38, 43, 49);
                campo.ForeColor = Color.White;
            }
            cmbSexo.Font = new Font("Segoe UI", 10F);
        }

        // Conecta validaciones preventivas y las acciones del formulario embebido.
        private void ConfigurarEventos()
        {
            Resize += (_, _) => AjustarLayout();
            txtNombre.KeyPress += ValidarNombreKeyPress;
            txtApellido.KeyPress += ValidarNombreKeyPress;
            txtDni.KeyPress += ValidarDniKeyPress;
            txtTelefono.KeyPress += ValidarTelefonoKeyPress;
            txtUsuario.KeyPress += ValidarUsuarioKeyPress;
            btnGuardar.Click += BtnGuardarClick;
            btnCancelar.Click += (_, _) => formPrincipal.VolverAInicioDesdePerfil();
        }

        // Carga los datos propios desde Lógica; el hash permanece fuera de la Vista.
        private void CargarPerfil()
        {
            try
            {
                perfilActual = usuarioLogica.ObtenerMiPerfil();
                if (perfilActual == null)
                {
                    MostrarMensaje("No se pudo cargar el perfil", "La cuenta actual no está disponible.");
                    formPrincipal.VolverAInicioDesdePerfil();
                    return;
                }

                txtNombre.Text = perfilActual.Nombre;
                txtApellido.Text = perfilActual.Apellido;
                txtDni.Text = perfilActual.Dni;
                txtTelefono.Text = perfilActual.Telefono;
                txtUsuario.Text = perfilActual.NombreUsuario;
                txtCorreo.Text = perfilActual.Correo;
                int sexo = cmbSexo.Items.IndexOf(string.IsNullOrWhiteSpace(perfilActual.Sexo) ? "No especificado" : perfilActual.Sexo);
                cmbSexo.SelectedIndex = sexo >= 0 ? sexo : 0;
                if (perfilActual.FechaNacimiento.HasValue)
                {
                    dtpFecha.Value = perfilActual.FechaNacimiento.Value;
                    dtpFecha.Checked = true;
                }
            }
            catch
            {
                MostrarMensaje("No se pudo cargar el perfil", "Ocurrió un problema al consultar los datos de la cuenta.");
                formPrincipal.VolverAInicioDesdePerfil();
            }
        }

        // Distribuye el formulario a dos columnas y las apila en ventanas angostas.
        private void AjustarLayout()
        {
            if (pnlPrincipal.ClientSize.Width <= 0) return;
            int margen = pnlPrincipal.ClientSize.Width >= 1200 ? 42 : 24;
            int ancho = Math.Min(1120, pnlPrincipal.ClientSize.Width - margen * 2);
            int x = Math.Max(margen, (pnlPrincipal.ClientSize.Width - ancho) / 2);
            bool dosColumnas = ancho >= 760;
            int separacion = 24;
            int anchoColumna = dosColumnas ? (ancho - separacion) / 2 : ancho;
            lblTitulo.Location = new Point(x, 22);
            lblSubtitulo.Location = new Point(x + 2, 62);
            pnlLinea.SetBounds(x, 96, ancho, 2);
            lblDatos.Location = new Point(x, 126);

            int y = 163;
            ColocarCampo(lblNombre, txtNombre, x, y, anchoColumna);
            ColocarCampo(lblApellido, txtApellido, dosColumnas ? x + anchoColumna + separacion : x, dosColumnas ? y : y + 76, anchoColumna);
            y += dosColumnas ? 82 : 158;
            ColocarCampo(lblDni, txtDni, x, y, anchoColumna);
            ColocarCampo(lblTelefono, txtTelefono, dosColumnas ? x + anchoColumna + separacion : x, dosColumnas ? y : y + 76, anchoColumna);
            y += dosColumnas ? 82 : 158;
            ColocarCampo(lblUsuario, txtUsuario, x, y, anchoColumna);
            ColocarCampo(lblCorreo, txtCorreo, dosColumnas ? x + anchoColumna + separacion : x, dosColumnas ? y : y + 76, anchoColumna);
            y += dosColumnas ? 82 : 158;
            ColocarCampo(lblSexo, cmbSexo, x, y, anchoColumna);
            ColocarCampo(lblFecha, dtpFecha, dosColumnas ? x + anchoColumna + separacion : x, dosColumnas ? y : y + 76, anchoColumna);
            y += dosColumnas ? 105 : 182;
            lblSeguridad.Location = new Point(x, y);
            y += 37;
            ColocarCampo(lblNueva, txtNueva, x, y, anchoColumna);
            ColocarCampo(lblConfirmar, txtConfirmar, dosColumnas ? x + anchoColumna + separacion : x, dosColumnas ? y : y + 76, anchoColumna);
            y += dosColumnas ? 82 : 158;
            ColocarCampo(lblActual, txtActual, x, y, anchoColumna);
            y += 64;
            btnCancelar.SetBounds(x, y, 120, 42);
            btnGuardar.SetBounds(x + ancho - 160, y, 160, 42);
            pnlPrincipal.AutoScrollMinSize = new Size(0, y + 64);
        }

        // Coloca la etiqueta y el campo de una fila respetando el ancho del panel.
        private static void ColocarCampo(Control etiqueta, Control campo, int x, int y, int ancho)
        {
            etiqueta.Location = new Point(x, y);
            campo.SetBounds(x, y + 24, ancho, 31);
        }

        // Valida los datos comunes y confirma credenciales antes de solicitar el guardado atómico.
        private void BtnGuardarClick(object? sender, EventArgs e)
        {
            if (perfilActual == null) return;
            UsuarioGuardarModelo datos = new()
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Dni = txtDni.Text,
                Telefono = txtTelefono.Text,
                NombreUsuario = txtUsuario.Text,
                Correo = txtCorreo.Text,
                Sexo = cmbSexo.SelectedItem?.ToString() == "No especificado" ? string.Empty : cmbSexo.SelectedItem?.ToString() ?? string.Empty,
                FechaNacimiento = dtpFecha.Checked ? dtpFecha.Value.Date : null
            };

            try
            {
                ResultadoUsuario resultado = usuarioLogica.GuardarMiPerfil(datos, txtActual.Text, txtNueva.Text, txtConfirmar.Text);
                if (!resultado.Exitoso)
                {
                    if (resultado.Mensaje == "Contraseña incorrecta")
                        MostrarMensaje("Contraseña incorrecta", "No se pudieron confirmar los cambios.");
                    else
                        MostrarMensaje("No se pudieron guardar los cambios", resultado.Mensaje);
                    return;
                }

                formPrincipal.ActualizarSesionVisibleDesdePerfil();
                MostrarMensaje("Perfil actualizado", "Tus datos se guardaron correctamente.");
                formPrincipal.VolverAInicioDesdePerfil();
            }
            catch
            {
                MostrarMensaje("No se pudieron guardar los cambios", "Ocurrió un problema al actualizar la cuenta. No se aplicaron cambios.");
            }
        }

        // Centra los mensajes del formulario embebido en la ventana principal.
        private void MostrarMensaje(string titulo, string texto)
        {
            using FormMensaje mensaje = new(titulo, texto);
            mensaje.ShowDialog(formPrincipal);
        }

        // Previene caracteres no permitidos en nombres sin sustituir la validación autoritativa de Lógica.
        private static void ValidarNombreKeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar is '\'' or '-') return;
            e.Handled = true;
        }

        // Limita preventivamente el documento a dígitos.
        private static void ValidarDniKeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        // Limita preventivamente el teléfono al formato admitido en administración de usuarios.
        private static void ValidarTelefonoKeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar is '+' or '-' or '(' or ')') return;
            e.Handled = true;
        }

        // Limita preventivamente el nombre de usuario al formato existente.
        private static void ValidarUsuarioKeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetterOrDigit(e.KeyChar) || e.KeyChar is '.' or '_' or '-') return;
            e.Handled = true;
        }
    }
}
