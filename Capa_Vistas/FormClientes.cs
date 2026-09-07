namespace Capa_Vistas
{
    public partial class FormClientes : Form
    {
        private readonly FormPrincipal formPrincipal;


        public FormClientes(
            FormPrincipal formPrincipal)
        {
            InitializeComponent();

            this.formPrincipal =
                formPrincipal;

            ConfigurarEventos();
        }


        // =========================================================
        // EVENTOS
        // =========================================================
        private void ConfigurarEventos()
        {
            btnNuevoCliente.Click +=
                BtnNuevoCliente_Click;

            dgvClientes.CellContentClick +=
                DgvClientes_CellContentClick;
        }


        // =========================================================
        // NUEVO CLIENTE
        // =========================================================
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


        // =========================================================
        // EDITAR CLIENTE
        // =========================================================
        private void DgvClientes_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }


            if (e.ColumnIndex < 0)
            {
                return;
            }


            if (dgvClientes.Columns[e.ColumnIndex]
                .Name != "colEditar")
            {
                return;
            }


            DataGridViewRow fila =
                dgvClientes.Rows[e.RowIndex];


            string dni =
                fila.Cells["colDni"]
                    .Value?.ToString() ?? "";


            string nombre =
                fila.Cells["colNombre"]
                    .Value?.ToString() ?? "";


            string apellido =
                fila.Cells["colApellido"]
                    .Value?.ToString() ?? "";


            string telefono =
                fila.Cells["colTelefono"]
                    .Value?.ToString() ?? "";


            string email =
                fila.Cells["colEmail"]
                    .Value?.ToString() ?? "";


            string localidad =
                fila.Cells["colLocalidad"]
                    .Value?.ToString() ?? "";


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
    }
}