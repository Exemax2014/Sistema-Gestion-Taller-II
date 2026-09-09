namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormClientes
    //
    // Lista y permite acceder al detalle de clientes.
    //
    // Las columnas, eventos y comportamiento se configuran acá.
    // El Designer queda solamente para diseño visual.
    // ============================================================

    public partial class FormClientes : Form
    {
        private readonly FormPrincipal formPrincipal;


        public FormClientes(
            FormPrincipal formPrincipal)
        {
            InitializeComponent();


            this.formPrincipal =
                formPrincipal;


            ConfigurarGrilla();

            ConfigurarEventos();

            PrepararVistaInicial();
        }


        // ========================================================
        // GRILLA
        //
        // Las columnas se crean fuera del Designer para evitar
        // que Visual Studio las elimine al editar visualmente.
        // ========================================================

        private void ConfigurarGrilla()
        {
            dgvClientes.AutoGenerateColumns =
                false;


            dgvClientes.Columns.Clear();


            // DNI
            DataGridViewTextBoxColumn colDni =
                new DataGridViewTextBoxColumn();

            colDni.Name =
                "colDni";

            colDni.HeaderText =
                "DNI";

            colDni.ReadOnly =
                true;

            colDni.FillWeight =
                80F;


            // NOMBRE
            DataGridViewTextBoxColumn colNombre =
                new DataGridViewTextBoxColumn();

            colNombre.Name =
                "colNombre";

            colNombre.HeaderText =
                "Nombre";

            colNombre.ReadOnly =
                true;


            // APELLIDO
            DataGridViewTextBoxColumn colApellido =
                new DataGridViewTextBoxColumn();

            colApellido.Name =
                "colApellido";

            colApellido.HeaderText =
                "Apellido";

            colApellido.ReadOnly =
                true;


            // TELÉFONO
            DataGridViewTextBoxColumn colTelefono =
                new DataGridViewTextBoxColumn();

            colTelefono.Name =
                "colTelefono";

            colTelefono.HeaderText =
                "Teléfono";

            colTelefono.ReadOnly =
                true;


            // EMAIL
            DataGridViewTextBoxColumn colEmail =
                new DataGridViewTextBoxColumn();

            colEmail.Name =
                "colEmail";

            colEmail.HeaderText =
                "Email";

            colEmail.ReadOnly =
                true;

            colEmail.FillWeight =
                130F;


            // LOCALIDAD
            DataGridViewTextBoxColumn colLocalidad =
                new DataGridViewTextBoxColumn();

            colLocalidad.Name =
                "colLocalidad";

            colLocalidad.HeaderText =
                "Localidad";

            colLocalidad.ReadOnly =
                true;


            // EDITAR
            DataGridViewButtonColumn colEditar =
                new DataGridViewButtonColumn();

            colEditar.Name =
                "colEditar";

            colEditar.HeaderText =
                "";

            colEditar.Text =
                "Editar";

            colEditar.UseColumnTextForButtonValue =
                true;

            colEditar.ReadOnly =
                true;

            colEditar.FillWeight =
                65F;

            colEditar.FlatStyle =
                FlatStyle.Flat;

            colEditar.DefaultCellStyle.BackColor =
                Color.FromArgb(
                    242,
                    243,
                    245
                );

            colEditar.DefaultCellStyle.ForeColor =
                Color.FromArgb(
                    45,
                    50,
                    55
                );


            // ELIMINAR
            DataGridViewButtonColumn colEliminar =
                new DataGridViewButtonColumn();

            colEliminar.Name =
                "colEliminar";

            colEliminar.HeaderText =
                "";

            colEliminar.Text =
                "Eliminar";

            colEliminar.UseColumnTextForButtonValue =
                true;

            colEliminar.ReadOnly =
                true;

            colEliminar.FillWeight =
                70F;

            colEliminar.FlatStyle =
                FlatStyle.Flat;

            colEliminar.DefaultCellStyle.BackColor =
                Color.FromArgb(
                    245,
                    235,
                    235
                );

            colEliminar.DefaultCellStyle.ForeColor =
                Color.FromArgb(
                    150,
                    50,
                    50
                );


            dgvClientes.Columns.AddRange(
                colDni,
                colNombre,
                colApellido,
                colTelefono,
                colEmail,
                colLocalidad,
                colEditar,
                colEliminar
            );
        }


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            btnNuevoCliente.Click +=
                BtnNuevoCliente_Click;


            btnBuscar.Click +=
                BtnBuscar_Click;


            txtBuscar.KeyDown +=
                TxtBuscar_KeyDown;


            dgvClientes.CellContentClick +=
                DgvClientes_CellContentClick;
        }


        // ========================================================
        // VISTA INICIAL
        // ========================================================

        private void PrepararVistaInicial()
        {
            lblCantidad.Text =
                "0 clientes encontrados";
        }


        // ========================================================
        // BUSCAR
        //
        // Luego se conectará con Capa_Logica.
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
        // NUEVO CLIENTE
        // ========================================================

        private void BtnNuevoCliente_Click(
            object? sender,
            EventArgs e)
        {
            FormClienteDetalle detalle =
                new FormClienteDetalle(
                    formPrincipal
                );


            formPrincipal.AbrirFormularioEnPanel(
                detalle,
                formPrincipal.BotonClientes
            );
        }


        // ========================================================
        // ACCIONES DE LA GRILLA
        // ========================================================

        private void DgvClientes_CellContentClick(
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


            string nombreColumna =
                dgvClientes
                    .Columns[e.ColumnIndex]
                    .Name;


            if (nombreColumna == "colEditar")
            {
                AbrirDetalleCliente(
                    e.RowIndex
                );

                return;
            }


            if (nombreColumna == "colEliminar")
            {
                // Se conectará posteriormente
                // con Capa_Logica.
                return;
            }
        }


        // ========================================================
        // ABRIR DETALLE
        // ========================================================

        private void AbrirDetalleCliente(
            int indiceFila)
        {
            DataGridViewRow fila =
                dgvClientes.Rows[indiceFila];


            string dni =
                fila.Cells["colDni"]
                    .Value?.ToString()
                ?? string.Empty;


            string nombre =
                fila.Cells["colNombre"]
                    .Value?.ToString()
                ?? string.Empty;


            string apellido =
                fila.Cells["colApellido"]
                    .Value?.ToString()
                ?? string.Empty;


            string telefono =
                fila.Cells["colTelefono"]
                    .Value?.ToString()
                ?? string.Empty;


            string email =
                fila.Cells["colEmail"]
                    .Value?.ToString()
                ?? string.Empty;


            string localidad =
                fila.Cells["colLocalidad"]
                    .Value?.ToString()
                ?? string.Empty;


            FormClienteDetalle detalle =
                new FormClienteDetalle(
                    formPrincipal,
                    dni,
                    nombre,
                    apellido,
                    telefono,
                    email,
                    localidad
                );


            formPrincipal.AbrirFormularioEnPanel(
                detalle,
                formPrincipal.BotonClientes
            );
        }


        // ========================================================
        // CANTIDAD
        // ========================================================

        private void ActualizarCantidad()
        {
            lblCantidad.Text =
                $"{dgvClientes.Rows.Count} clientes encontrados";
        }
    }
}