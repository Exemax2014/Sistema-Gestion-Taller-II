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

        private bool EsAlta => !idCliente.HasValue;


        // =========================================================
        // NUEVO CLIENTE
        // =========================================================
        public FormClienteDetalle(
            FormPrincipal formPrincipal)
        {
            InitializeComponent();

            this.formPrincipal = formPrincipal;

            ConfigurarCentrado();

            lblTitulo.Text = "Nuevo cliente";

            CargarProvincias();

            ConfigurarEventos();

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

            ConfigurarCentrado();

            lblTitulo.Text = "Editar cliente";

            CargarProvincias();

            ConfigurarEventos();

            CargarCliente();

            cargandoDatos = false;

            hayCambios = false;
        }


        // =========================================================
        // CENTRADO DEL CONTENIDO
        // =========================================================
        private void ConfigurarCentrado()
        {
            CentrarContenido();

            Resize +=
                FormClienteDetalle_Resize;
        }


        private void FormClienteDetalle_Resize(
            object? sender,
            EventArgs e)
        {
            CentrarContenido();
        }


        private void CentrarContenido()
        {
            int margenHorizontal = 30;
            int margenSuperior = 25;

            int posicionX =
                (ClientSize.Width -
                 pnlContenido.Width) / 2;

            if (posicionX < margenHorizontal)
            {
                posicionX =
                    margenHorizontal;
            }

            pnlContenido.Left =
                posicionX;

            pnlContenido.Top =
                margenSuperior;
        }


        // =========================================================
        // PROVINCIAS Y LOCALIDADES
        // =========================================================
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


        private void CmbProvincia_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            int idProvincia =
                ObtenerIdSeleccionado(cmbProvincia) ?? 0;

            List<OpcionClienteModelo> localidades =
                idProvincia > 0
                    ? clienteLogica.ObtenerLocalidades(idProvincia)
                    : new List<OpcionClienteModelo>();

            localidades.Insert(
                0,
                new OpcionClienteModelo { Id = 0, Nombre = "Seleccioná una localidad" }
            );

            cmbLocalidad.DataSource = localidades;
            cmbLocalidad.DisplayMember = nameof(OpcionClienteModelo.Nombre);
            cmbLocalidad.ValueMember = nameof(OpcionClienteModelo.Id);

            ControlModificado(sender, e);
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
            int? idLocalidad = ObtenerIdSeleccionado(cmbLocalidad);

            if (idProvincia == 0) idProvincia = null;
            if (idLocalidad == 0) idLocalidad = null;

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