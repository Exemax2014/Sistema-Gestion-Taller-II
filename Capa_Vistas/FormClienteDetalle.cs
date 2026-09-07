namespace Capa_Vistas
{
    public partial class FormClienteDetalle : Form
    {
        private readonly FormPrincipal formPrincipal;

        private bool hayCambios;
        private bool cargandoDatos;


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

            ConfigurarEventos();

            hayCambios = false;
        }


        // =========================================================
        // EDITAR CLIENTE
        // =========================================================
        public FormClienteDetalle(
            FormPrincipal formPrincipal,
            string dni,
            string nombre,
            string apellido,
            string telefono,
            string email,
            string localidad)
        {
            InitializeComponent();

            this.formPrincipal = formPrincipal;

            ConfigurarCentrado();

            lblTitulo.Text = "Editar cliente";

            ConfigurarEventos();

            cargandoDatos = true;

            txtDni.Text = dni;
            txtNombre.Text = nombre;
            txtApellido.Text = apellido;
            txtTelefono.Text = telefono;
            txtEmail.Text = email;

            // Provincia, localidad y dirección
            // se cargarán más adelante desde Capa_Logica.

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
        // EVENTOS
        // =========================================================
        private void ConfigurarEventos()
        {
            btnVolver.Click +=
                BtnVolver_Click;

            btnCancelar.Click +=
                BtnVolver_Click;

            btnGuardar.Click +=
                BtnGuardar_Click;


            txtDni.TextChanged +=
                ControlModificado;

            txtNombre.TextChanged +=
                ControlModificado;

            txtApellido.TextChanged +=
                ControlModificado;

            txtTelefono.TextChanged +=
                ControlModificado;

            txtEmail.TextChanged +=
                ControlModificado;

            txtDireccion.TextChanged +=
                ControlModificado;

            cmbProvincia.SelectedIndexChanged +=
                ControlModificado;

            cmbLocalidad.SelectedIndexChanged +=
                ControlModificado;
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

            return mensaje.ShowDialog(this) ==
                   DialogResult.OK;
        }


        // =========================================================
        // GUARDAR
        // =========================================================
        private void BtnGuardar_Click(
            object? sender,
            EventArgs e)
        {
            // Por ahora solamente simulamos el guardado.
            // Después lo conectaremos a ClienteLogica.

            hayCambios = false;

            using FormMensaje mensaje =
                new FormMensaje(
                    "Cliente",
                    "Los datos del cliente están listos para ser guardados."
                );

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
    }
}