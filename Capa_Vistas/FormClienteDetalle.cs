using Capa_Logica;

namespace Capa_Vistas
{
    public partial class FormClienteDetalle : Form
    {
        private readonly ClienteLogica clienteLogica = new ClienteLogica();

        private readonly FormPrincipal formPrincipal;

        private int? idCliente;
        private int? idDireccionActual;

        private bool hayCambios;
        private bool cargandoDatos = true;
        private List<OpcionClienteModelo> localidadesActuales = new();

        private bool EsAlta => !idCliente.HasValue;


        // =========================================================
        // NUEVO CLIENTE
        // =========================================================
        public FormClienteDetalle(
            FormPrincipal formPrincipal)
        {
            InitializeComponent();

            this.formPrincipal = formPrincipal;

            ConfigurarLayoutResponsive();

            lblTitulo.Text = "Nuevo cliente";

            CargarProvincias();

            ConfigurarEventos();
            ConfigurarValidacionesVisuales();

            cargandoDatos = false;

            hayCambios = false;
        }


        // =========================================================
        // EDITAR CLIENTE
        //
        // Ahora recibe solo el idCliente y carga TODO (incluida
        // la dirección real) desde Capa_Logica, igual que hace
        // FormProductoDetalle con idProducto. Evita pasar datos
        // sueltos que podrían estar desactualizados.
        // =========================================================
        public FormClienteDetalle(
            FormPrincipal formPrincipal,
            int idCliente)
        {
            InitializeComponent();

            this.formPrincipal = formPrincipal;

            this.idCliente = idCliente;

            ConfigurarLayoutResponsive();

            lblTitulo.Text = "Editar cliente";

            CargarProvincias();

            ConfigurarEventos();
            ConfigurarValidacionesVisuales();

            CargarCliente();
            ConfigurarModoEdicion();

            cargandoDatos = false;

            hayCambios = false;
        }


        // =========================================================
        // LAYOUT RESPONSIVE
        // =========================================================
        // Registra los eventos que recalculan el formulario dentro de pnlContenido.
        private void ConfigurarLayoutResponsive()
        {
            Load += FormClienteDetalle_Load;
            Resize += FormClienteDetalle_Resize;
            pnlContenedor.Resize += FormClienteDetalle_Resize;
        }

        // Aplica el primer cálculo cuando el contenedor ya dispone de su tamaño real.
        private void FormClienteDetalle_Load(object? sender, EventArgs e)
        {
            AjustarLayoutResponsive();
        }

        // Recalcula la distribución al cambiar el tamaño del formulario o contenedor.
        private void FormClienteDetalle_Resize(object? sender, EventArgs e)
        {
            AjustarLayoutResponsive();
        }

        // Ajusta tarjeta, márgenes y columnas según el espacio disponible;
        // el contenedor conserva scroll si el contenido no entra verticalmente.
        private void AjustarLayoutResponsive()
        {
            if (pnlContenedor.ClientSize.Width <= 0)
            {
                return;
            }

            const int anchoMinimo = 560;
            const int anchoMaximo = 1120;
            const int altoContenido = 680;
            const int margenVertical = 20;

            int anchoDisponible = pnlContenedor.ClientSize.Width - 64;
            int anchoContenido = Math.Clamp(anchoDisponible, anchoMinimo, anchoMaximo);
            int margenLateral = Math.Max(32, (pnlContenedor.ClientSize.Width - anchoContenido) / 2);
            int relleno = Math.Max(42, (int)(anchoContenido * 0.06));
            int anchoCampos = anchoContenido - (relleno * 2);
            int separacionColumnas = 34;
            int anchoColumna = (anchoCampos - separacionColumnas) / 2;
            int segundaColumna = relleno + anchoColumna + separacionColumnas;

            pnlContenedor.SuspendLayout();
            pnlContenido.SuspendLayout();

            pnlContenido.Size = new Size(anchoContenido, altoContenido);
            pnlContenido.Location = new Point(margenLateral, margenVertical);
            pnlContenedor.AutoScrollMinSize = new Size(
                anchoContenido + (margenLateral * 2),
                altoContenido + (margenVertical * 2));

            btnVolver.Location = new Point(relleno - 22, 22);
            lblTitulo.Location = new Point(relleno, 75);
            pnlLineaDorada.Location = new Point(relleno + 3, 132);

            AjustarFilaCompleta(lblDni, txtDni, relleno, 170, anchoCampos);
            AjustarFilaDoble(lblNombre, txtNombre, relleno, lblApellido, txtApellido, segundaColumna, 255, anchoColumna);
            AjustarFilaDoble(lblTelefono, txtTelefono, relleno, lblEmail, txtEmail, segundaColumna, 345, anchoColumna);
            AjustarFilaDoble(lblProvincia, cmbProvincia, relleno, lblLocalidad, cmbLocalidad, segundaColumna, 435, anchoColumna);
            AjustarFilaCompleta(lblDireccion, txtDireccion, relleno, 525, anchoCampos);

            btnCancelar.Location = new Point(relleno, 610);
            btnGuardar.Location = new Point(anchoContenido - relleno - btnGuardar.Width, 610);

            pnlContenido.ResumeLayout(false);
            pnlContenedor.ResumeLayout(true);
        }

        // Ubica una etiqueta y su control ocupando todo el ancho disponible.
        private static void AjustarFilaCompleta(
            Label etiqueta,
            Control campo,
            int izquierda,
            int superiorEtiqueta,
            int ancho)
        {
            etiqueta.Location = new Point(izquierda, superiorEtiqueta);
            campo.Location = new Point(izquierda, superiorEtiqueta + 28);
            campo.Width = ancho;
        }

        // Mantiene dos campos alineados en una misma fila responsive.
        private static void AjustarFilaDoble(
            Label etiquetaIzquierda,
            Control campoIzquierdo,
            int izquierda,
            Label etiquetaDerecha,
            Control campoDerecho,
            int derecha,
            int superiorEtiqueta,
            int ancho)
        {
            etiquetaIzquierda.Location = new Point(izquierda, superiorEtiqueta);
            campoIzquierdo.Location = new Point(izquierda, superiorEtiqueta + 28);
            campoIzquierdo.Width = ancho;

            etiquetaDerecha.Location = new Point(derecha, superiorEtiqueta);
            campoDerecho.Location = new Point(derecha, superiorEtiqueta + 28);
            campoDerecho.Width = ancho;
        }


        // =========================================================
        // PROVINCIAS Y LOCALIDADES
        // =========================================================
        // Carga el catálogo cerrado de provincias desde la lógica para impedir altas libres.
        private void CargarProvincias()
        {
            List<OpcionClienteModelo> provincias =
                clienteLogica.ObtenerProvincias();

            provincias.Insert(
                0,
                new OpcionClienteModelo { Id = 0, Nombre = "Seleccioná una provincia" }
            );

            cmbProvincia.DataSource = provincias;
            cmbProvincia.DisplayMember = nameof(OpcionClienteModelo.Nombre);
            cmbProvincia.ValueMember = nameof(OpcionClienteModelo.Id);
        }


        // Al cambiar de provincia descarta la localidad previa y carga solo
        // las opciones correspondientes a la nueva selección.
        private void CmbProvincia_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            int idProvincia =
                ObtenerIdSeleccionado(cmbProvincia) ?? 0;

            CargarLocalidades(idProvincia, true);

            ControlModificado(sender, e);
        }

        // Carga las localidades activas de la provincia seleccionada
        // y configura el autocompletado del ComboBox editable.
        private void CargarLocalidades(int idProvincia, bool limpiarSeleccion)
        {
            localidadesActuales = idProvincia > 0
                ? clienteLogica.ObtenerLocalidades(idProvincia)
                : new List<OpcionClienteModelo>();

            cmbLocalidad.DataSource = null;
            cmbLocalidad.DisplayMember = nameof(OpcionClienteModelo.Nombre);
            cmbLocalidad.ValueMember = nameof(OpcionClienteModelo.Id);
            cmbLocalidad.DataSource = localidadesActuales;

            AutoCompleteStringCollection sugerencias = new AutoCompleteStringCollection();
            sugerencias.AddRange(localidadesActuales.Select(l => l.Nombre).ToArray());
            cmbLocalidad.AutoCompleteCustomSource = sugerencias;
            cmbLocalidad.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbLocalidad.AutoCompleteSource = AutoCompleteSource.CustomSource;

            if (limpiarSeleccion)
            {
                cmbLocalidad.SelectedIndex = -1;
                cmbLocalidad.Text = string.Empty;
            }
        }


        // =========================================================
        // CARGAR CLIENTE (modo edición)
        // =========================================================
        private void CargarCliente()
        {
            if (!idCliente.HasValue)
            {
                return;
            }

            ClienteDetalleModelo? cliente =
                clienteLogica.ObtenerPorId(idCliente.Value);

            if (cliente == null)
            {
                using FormMensaje mensaje =
                    new FormMensaje(
                        "Cliente",
                        "No se encontró el cliente solicitado."
                    );

                mensaje.ShowDialog(this);

                VolverAClientes();

                return;
            }

            idDireccionActual = cliente.IdDireccion;

            txtDni.Text = cliente.Documento;
            txtNombre.Text = cliente.Nombre;
            txtApellido.Text = cliente.Apellido;
            txtTelefono.Text = cliente.Telefono;
            txtEmail.Text = cliente.Correo;

            // La dirección se muestra en un solo campo de texto
            // (no hay altura separada en este diseño).
            txtDireccion.Text =
                string.IsNullOrWhiteSpace(cliente.Altura)
                    ? cliente.Calle
                    : $"{cliente.Calle} {cliente.Altura}";

            if (cliente.IdProvincia.HasValue)
            {
                cmbProvincia.SelectedValue = cliente.IdProvincia.Value;
            }

            if (cliente.IdLocalidad.HasValue)
            {
                cmbLocalidad.SelectedValue = cliente.IdLocalidad.Value;
            }
        }


        // =========================================================
        // EVENTOS
        // =========================================================
        private void ConfigurarEventos()
        {
            btnVolver.Click += BtnVolver_Click;
            btnCancelar.Click += BtnVolver_Click;
            btnGuardar.Click += BtnGuardar_Click;

            cmbProvincia.SelectedIndexChanged += CmbProvincia_SelectedIndexChanged;
            cmbLocalidad.SelectedIndexChanged += ControlModificado;

            txtDni.TextChanged += ControlModificado;
            txtNombre.TextChanged += ControlModificado;
            txtApellido.TextChanged += ControlModificado;
            txtTelefono.TextChanged += ControlModificado;
            txtEmail.TextChanged += ControlModificado;
            txtDireccion.TextChanged += ControlModificado;
            cmbLocalidad.TextChanged += ControlModificado;
        }

        // Configura límites preventivos de los controles sin reemplazar la validación de negocio.
        private void ConfigurarValidacionesVisuales()
        {
            txtDni.MaxLength = 20;
            txtNombre.MaxLength = 100;
            txtApellido.MaxLength = 100;
            txtTelefono.MaxLength = 30;
            txtEmail.MaxLength = 150;
            txtDireccion.MaxLength = 150;
            txtDni.KeyPress += SoloNumeros_KeyPress;
        }

        private void ConfigurarModoEdicion()
        {
            if (EsAlta || clienteLogica.PuedeModificarCliente())
            {
                return;
            }

            lblTitulo.Text = "Detalle de cliente";
            txtDni.ReadOnly = true;
            txtNombre.ReadOnly = true;
            txtApellido.ReadOnly = true;
            txtTelefono.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtDireccion.ReadOnly = true;
            cmbProvincia.Enabled = false;
            cmbLocalidad.Enabled = false;
            btnGuardar.Visible = false;
            btnCancelar.Text = "Volver";
        }

        private static void SoloNumeros_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        // =========================================================
        // DETECTAR CAMBIOS
        // =========================================================
        private void ControlModificado(
            object? sender,
            EventArgs e)
        {
            if (cargandoDatos)
            {
                return;
            }

            hayCambios = true;
        }


        // =========================================================
        // VOLVER
        // =========================================================
        private void BtnVolver_Click(
            object? sender,
            EventArgs e)
        {
            if (!ConfirmarSalida())
            {
                return;
            }

            VolverAClientes();
        }


        private bool ConfirmarSalida()
        {
            if (!hayCambios)
            {
                return true;
            }

            using FormMensaje mensaje =
                new FormMensaje(
                    "Cambios sin guardar",
                    "Hay cambios que todavía no fueron guardados. ¿Deseás salir igualmente?",
                    "Salir sin guardar",
                    true
                );

            return mensaje.ShowDialog(this) == DialogResult.OK;
        }


        // =========================================================
        // GUARDAR
        // =========================================================
        private void BtnGuardar_Click(
            object? sender,
            EventArgs e)
        {
            int? idProvincia = ObtenerIdSeleccionado(cmbProvincia);

            if (idProvincia == 0) idProvincia = null;

            if (!idProvincia.HasValue)
            {
                MostrarMensaje("Cliente", "Seleccioná una provincia.");
                return;
            }

            if (!IntentarResolverLocalidad(idProvincia.Value, out int idLocalidad))
            {
                return;
            }

            string? error =
                clienteLogica.ValidarCliente(
                    txtNombre.Text,
                    txtApellido.Text,
                    txtDni.Text,
                    txtEmail.Text,
                    idProvincia,
                    idLocalidad,
                    txtDireccion.Text
                );

            if (error != null)
            {
                using FormMensaje mensajeError =
                    new FormMensaje("Cliente", error);

                mensajeError.ShowDialog(this);

                return;
            }

            ResultadoCliente resultado;

            if (EsAlta)
            {
                resultado =
                    clienteLogica.Alta(
                        txtNombre.Text,
                        txtApellido.Text,
                        txtDni.Text,
                        txtEmail.Text,
                        txtTelefono.Text,
                        idLocalidad,
                        txtDireccion.Text,
                        null // no hay campo de altura separado
                    );
            }
            else
            {
                resultado =
                    clienteLogica.Modificar(
                        idCliente!.Value,
                        idDireccionActual,
                        txtNombre.Text,
                        txtApellido.Text,
                        txtDni.Text,
                        txtEmail.Text,
                        txtTelefono.Text,
                        idLocalidad,
                        txtDireccion.Text,
                        null
                    );
            }

            if (!resultado.Exitoso)
            {
                using FormMensaje mensajeError =
                    new FormMensaje("No se pudo guardar", resultado.Mensaje);

                mensajeError.ShowDialog(this);

                return;
            }

            hayCambios = false;

            using FormMensaje mensaje =
                new FormMensaje("Cliente guardado", resultado.Mensaje);

            mensaje.ShowDialog(this);

            VolverAClientes();
        }

        // Reutiliza una localidad existente o pide confirmación antes de crearla;
        // al finalizar deja seleccionado el ID devuelto por la lógica.
        private bool IntentarResolverLocalidad(int idProvincia, out int idLocalidad)
        {
            idLocalidad = 0;
            string nombreLocalidad = cmbLocalidad.Text.Trim();

            OpcionClienteModelo? existente =
                clienteLogica.BuscarLocalidad(idProvincia, nombreLocalidad);

            if (existente != null)
            {
                idLocalidad = existente.Id;
                cmbLocalidad.SelectedValue = idLocalidad;
                return true;
            }

            if (string.IsNullOrWhiteSpace(nombreLocalidad))
            {
                MostrarMensaje("Cliente", "Seleccioná o ingresá una localidad.");
                return false;
            }

            using FormMensaje confirmacion = new FormMensaje(
                "Nueva localidad",
                $"La localidad \"{nombreLocalidad}\" no existe en la provincia seleccionada. ¿Deseás crearla?",
                "Crear localidad",
                true);

            if (confirmacion.ShowDialog(this) != DialogResult.OK)
            {
                return false;
            }

            ResultadoCliente resultado =
                clienteLogica.ObtenerOCrearLocalidad(idProvincia, nombreLocalidad);

            if (!resultado.Exitoso)
            {
                MostrarMensaje("Localidad", resultado.Mensaje);
                return false;
            }

            CargarLocalidades(idProvincia, false);
            cmbLocalidad.SelectedValue = resultado.IdGenerado;
            idLocalidad = resultado.IdGenerado;
            return true;
        }

        // Centraliza los mensajes visuales de este formulario.
        private void MostrarMensaje(string titulo, string mensaje)
        {
            using FormMensaje dialogo = new FormMensaje(titulo, mensaje);
            dialogo.ShowDialog(this);
        }


        // =========================================================
        // VOLVER A CLIENTES
        // =========================================================
        private void VolverAClientes()
        {
            formPrincipal.AbrirFormularioEnPanel(
                new FormClientes(formPrincipal),
                formPrincipal.BotonClientes
            );
        }


        // =========================================================
        // AUXILIARES
        // =========================================================
        private static int? ObtenerIdSeleccionado(ComboBox combo)
        {
            if (combo.SelectedValue is int id)
            {
                return id;
            }

            return null;
        }
    }
}
