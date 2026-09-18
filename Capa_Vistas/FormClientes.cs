using Capa_Logica;

namespace Capa_Vistas
{
    public partial class FormClientes : Form
    {
        private readonly ClienteLogica clienteLogica = new ClienteLogica();
        private readonly VentaLogica ventaLogica = new VentaLogica();
        private readonly FormPrincipal formPrincipal;

        private DataGridViewButtonColumn colDetalle = null!;
        private DataGridViewButtonColumn colHistorial = null!;
        private DataGridViewButtonColumn colEstado = null!;

        public FormClientes(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;

            ConfigurarGrilla();
            ConfigurarPermisos();
            ConfigurarEventos();
            EjecutarBusqueda();
        }

        private void ConfigurarPermisos()
        {
            btnNuevoCliente.Visible = clienteLogica.PuedeCrearCliente();
            colDetalle.Visible = clienteLogica.PuedeVerClientes();
            colHistorial.Visible = clienteLogica.PuedeVerHistorialCompras();
            colEstado.Visible =
                clienteLogica.PuedeEliminarCliente() ||
                clienteLogica.PuedeReactivarCliente();
        }

        private void ConfigurarGrilla()
        {
            dgvClientes.AutoGenerateColumns = false;
            dgvClientes.Columns.Clear();

            dgvClientes.Columns.AddRange(
                CrearColumnaTexto("colDni", "DNI", 80F),
                CrearColumnaTexto("colNombre", "Nombre"),
                CrearColumnaTexto("colApellido", "Apellido"),
                CrearColumnaTexto("colTelefono", "Teléfono"),
                CrearColumnaTexto("colEmail", "Email", 130F),
                CrearColumnaTexto("colLocalidad", "Localidad"),
                CrearBoton("colDetalle", "Ver detalle", 85F, Color.FromArgb(242, 243, 245), Color.FromArgb(45, 50, 55)),
                CrearBoton("colHistorial", "Compras", 75F, Color.FromArgb(235, 241, 247), Color.FromArgb(40, 90, 130)),
                CrearBoton("colEstado", "Dar de baja", 90F, Color.FromArgb(245, 235, 235), Color.FromArgb(150, 50, 50))
            );

            colDetalle = (DataGridViewButtonColumn)dgvClientes.Columns["colDetalle"]!;
            colHistorial = (DataGridViewButtonColumn)dgvClientes.Columns["colHistorial"]!;
            colEstado = (DataGridViewButtonColumn)dgvClientes.Columns["colEstado"]!;
        }

        private static DataGridViewTextBoxColumn CrearColumnaTexto(string nombre, string titulo, float? peso = null)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = nombre,
                HeaderText = titulo,
                ReadOnly = true,
                FillWeight = peso ?? 100F
            };
        }

        private static DataGridViewButtonColumn CrearBoton(
            string nombre, string texto, float peso, Color fondo, Color textoColor)
        {
            return new DataGridViewButtonColumn
            {
                Name = nombre,
                HeaderText = string.Empty,
                Text = texto,
                UseColumnTextForButtonValue = true,
                ReadOnly = true,
                FillWeight = peso,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = fondo,
                    ForeColor = textoColor
                }
            };
        }

        private void ConfigurarEventos()
        {
            btnNuevoCliente.Click += BtnNuevoCliente_Click;
            btnBuscar.Click += (_, _) => EjecutarBusqueda();
            txtBuscar.KeyDown += TxtBuscar_KeyDown;
            cmbEstado.SelectedIndexChanged += (_, _) => EjecutarBusqueda();
            dgvClientes.CellContentClick += DgvClientes_CellContentClick;
            dgvClientes.CellFormatting += DgvClientes_CellFormatting;
        }

        private string ObtenerEstadoSeleccionado() => cmbEstado.SelectedIndex switch
        {
            1 => "BAJA",
            2 => "TODOS",
            _ => "ACTIVOS"
        };

        private void TxtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                EjecutarBusqueda();
            }
        }

        private void EjecutarBusqueda()
        {
            string texto = txtBuscar.Text.Trim();
            string estado = ObtenerEstadoSeleccionado();
            List<ClienteListaModelo> resultado = string.IsNullOrWhiteSpace(texto)
                ? clienteLogica.ObtenerTodos(estado)
                : clienteLogica.Buscar(texto, estado);

            dgvClientes.Rows.Clear();
            foreach (ClienteListaModelo cliente in resultado)
            {
                int fila = dgvClientes.Rows.Add(
                    cliente.Documento, cliente.Nombre, cliente.Apellido, cliente.Telefono,
                    cliente.Correo, cliente.Localidad);
                dgvClientes.Rows[fila].Tag = cliente;
            }

            lblCantidad.Text = $"{resultado.Count} clientes encontrados";
        }

        private void DgvClientes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvClientes.Rows[e.RowIndex].Tag is not ClienteListaModelo cliente)
            {
                return;
            }

            string columna = dgvClientes.Columns[e.ColumnIndex].Name;
            if (columna == "colDetalle" && !cliente.Activo)
            {
                e.Value = string.Empty;
                e.FormattingApplied = true;
                return;
            }

            if (columna != "colEstado")
            {
                return;
            }

            DataGridViewCell celda = dgvClientes.Rows[e.RowIndex].Cells[e.ColumnIndex];
            celda.Value = cliente.Activo ? "Dar de baja" : "Dar de alta";
            celda.Style.BackColor = cliente.Activo
                ? Color.FromArgb(245, 235, 235)
                : Color.FromArgb(230, 245, 230);
            celda.Style.ForeColor = cliente.Activo
                ? Color.FromArgb(150, 50, 50)
                : Color.FromArgb(45, 130, 60);
        }

        private void BtnNuevoCliente_Click(object? sender, EventArgs e)
        {
            formPrincipal.AbrirFormularioEnPanel(
                new FormClienteDetalle(formPrincipal),
                formPrincipal.BotonClientes);
        }

        private void DgvClientes_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                dgvClientes.Rows[e.RowIndex].Tag is not ClienteListaModelo cliente)
            {
                return;
            }

            switch (dgvClientes.Columns[e.ColumnIndex].Name)
            {
                case "colDetalle" when cliente.Activo:
                    AbrirDetalle(cliente.IdCliente);
                    break;
                case "colHistorial":
                    AbrirHistorial(cliente);
                    break;
                case "colEstado" when cliente.Activo:
                    ConfirmarCambioEstado(cliente, false);
                    break;
                case "colEstado":
                    ConfirmarCambioEstado(cliente, true);
                    break;
            }
        }

        private void AbrirDetalle(int idCliente)
        {
            formPrincipal.AbrirFormularioEnPanel(
                new FormClienteDetalle(formPrincipal, idCliente),
                formPrincipal.BotonClientes);
        }

        private void AbrirHistorial(ClienteListaModelo cliente)
        {
            ResultadoConsultaVentas resultado = ventaLogica.ListarPorCliente(
                cliente.IdCliente,
                DateTimePicker.MinimumDateTime,
                DateTime.Today);

            if (!resultado.Exitoso)
            {
                MostrarMensaje("Historial de compras", resultado.Mensaje);
                return;
            }

            if (resultado.Ventas.Count == 0)
            {
                MostrarMensaje(
                    "Historial de compras",
                    $"{cliente.Nombre} {cliente.Apellido} no tiene compras registradas.");
                return;
            }

            formPrincipal.AbrirFormularioEnPanel(
                new FormListadoVentas(
                    formPrincipal,
                    OrigenHistorialVentas.Clientes,
                    TipoHistorialVentas.Cliente,
                    cliente.IdCliente,
                    $"{cliente.Nombre} {cliente.Apellido}"),
                formPrincipal.BotonClientes);
        }

        private void ConfirmarCambioEstado(ClienteListaModelo cliente, bool reactivar)
        {
            string accion = reactivar ? "dar de alta" : "dar de baja";
            using FormMensaje confirmacion = new FormMensaje(
                reactivar ? "Dar de alta cliente" : "Dar de baja cliente",
                $"¿Deseás {accion} a {cliente.Nombre} {cliente.Apellido}?",
                reactivar ? "Sí, dar de alta" : "Sí, dar de baja",
                true);

            if (confirmacion.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            ResultadoCliente resultado = reactivar
                ? clienteLogica.Reactivar(cliente.IdCliente)
                : clienteLogica.Baja(cliente.IdCliente);

            if (!resultado.Exitoso)
            {
                MostrarMensaje("Clientes", resultado.Mensaje);
                return;
            }

            MostrarMensaje("Clientes", resultado.Mensaje);
            EjecutarBusqueda();
        }

        private void MostrarMensaje(string titulo, string mensaje)
        {
            using FormMensaje dialogo = new FormMensaje(titulo, mensaje);
            dialogo.ShowDialog(this);
        }
    }
}
