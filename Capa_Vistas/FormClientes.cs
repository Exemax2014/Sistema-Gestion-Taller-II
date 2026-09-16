using Capa_Logica;

namespace Capa_Vistas
{
    public partial class FormClientes : Form
    {
        private readonly ClienteLogica clienteLogica = new ClienteLogica();

        private readonly FormPrincipal formPrincipal;


        public FormClientes(
            FormPrincipal formPrincipal)
        {
            InitializeComponent();

            this.formPrincipal = formPrincipal;

            ConfigurarPermisos();

            ConfigurarEventos();

            EjecutarBusqueda();
        }


        // =========================================================
        // PERMISOS
        // =========================================================
        private void ConfigurarPermisos()
        {
            btnNuevoCliente.Visible =
                clienteLogica.PuedeCrearCliente();

            colEditar.Visible =
                clienteLogica.PuedeModificarCliente();

            // La columna de acción (Eliminar/Reactivar) usa el
            // mismo permiso para ambas operaciones.
            colEliminar.Visible =
                clienteLogica.PuedeEliminarCliente();
        }


        // =========================================================
        // EVENTOS
        // =========================================================
        private void ConfigurarEventos()
        {
            btnNuevoCliente.Click +=
                BtnNuevoCliente_Click;

            btnBuscar.Click +=
                BtnBuscar_Click;

            txtBuscar.KeyDown +=
                TxtBuscar_KeyDown;

            cmbEstado.SelectedIndexChanged +=
                CmbEstado_SelectedIndexChanged;

            dgvClientes.CellContentClick +=
                DgvClientes_CellContentClick;

            dgvClientes.CellFormatting +=
                DgvClientes_CellFormatting;
        }


        // =========================================================
        // ESTADO SELECCIONADO -> texto que espera Capa_Logica
        // =========================================================
        private string ObtenerEstadoSeleccionado()
        {
            return cmbEstado.SelectedIndex switch
            {
                1 => "BAJA",
                2 => "TODOS",
                _ => "ACTIVOS"
            };
        }


        private void CmbEstado_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            EjecutarBusqueda();
        }


        // =========================================================
        // BUSCAR
        // =========================================================
        private void BtnBuscar_Click(
            object? sender,
            EventArgs e)
        {
            EjecutarBusqueda();
        }


        private void TxtBuscar_KeyDown(
            object? sender,
            KeyEventArgs e)
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

            List<ClienteListaModelo> resultado =
                string.IsNullOrWhiteSpace(texto)
                    ? clienteLogica.ObtenerTodos(estado)
                    : clienteLogica.Buscar(texto, estado);

            CargarClientes(resultado);
        }


        // =========================================================
        // CARGAR GRILLA
        //
        // Guarda el ClienteListaModelo completo en fila.Tag
        // (no solo el id), porque necesitamos saber si esa fila
        // está activa o de baja para dibujar el botón correcto.
        // =========================================================
        private void CargarClientes(List<ClienteListaModelo> clientes)
        {
            dgvClientes.Rows.Clear();

            foreach (ClienteListaModelo cliente in clientes)
            {
                int fila = dgvClientes.Rows.Add(
                    cliente.Documento,
                    cliente.Nombre,
                    cliente.Apellido,
                    cliente.Telefono,
                    cliente.Correo,
                    cliente.Localidad
                );

                dgvClientes.Rows[fila].Tag = cliente;
            }

            lblCantidad.Text =
                $"{clientes.Count} clientes encontrados";
        }


        // =========================================================
        // PINTAR EL BOTÓN SEGÚN EL ESTADO DE LA FILA
        //
        // Activo  -> "Eliminar" en rojo
        // De baja -> "Reactivar" en verde
        // =========================================================
        private void DgvClientes_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dgvClientes.Columns[e.ColumnIndex].Name != "colEliminar")
            {
                return;
            }

            if (dgvClientes.Rows[e.RowIndex].Tag is not ClienteListaModelo cliente)
            {
                return;
            }

            DataGridViewCell celda = dgvClientes.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (cliente.Activo)
            {
                celda.Value = "Eliminar";
                celda.Style.BackColor = System.Drawing.Color.FromArgb(245, 235, 235);
                celda.Style.ForeColor = System.Drawing.Color.FromArgb(150, 50, 50);
            }
            else
            {
                celda.Value = "Reactivar";
                celda.Style.BackColor = System.Drawing.Color.FromArgb(230, 245, 230);
                celda.Style.ForeColor = System.Drawing.Color.FromArgb(45, 130, 60);
            }
        }


        // =========================================================
        // NUEVO CLIENTE
        // =========================================================
        private void BtnNuevoCliente_Click(
            object? sender,
            EventArgs e)
        {
            FormClienteDetalle detalle =
                new FormClienteDetalle(formPrincipal);

            formPrincipal.AbrirFormularioEnPanel(
                detalle,
                formPrincipal.BotonClientes
            );
        }


        // =========================================================
        // EDITAR / ELIMINAR / REACTIVAR (botones de la grilla)
        // =========================================================
        private void DgvClientes_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvClientes.Rows[e.RowIndex];

            if (fila.Tag is not ClienteListaModelo cliente)
            {
                return;
            }

            string nombreColumna = dgvClientes.Columns[e.ColumnIndex].Name;

            if (nombreColumna == "colEditar")
            {
                AbrirEdicion(cliente.IdCliente);
            }
            else if (nombreColumna == "colEliminar")
            {
                if (cliente.Activo)
                {
                    ConfirmarYEliminar(cliente);
                }
                else
                {
                    ConfirmarYReactivar(cliente);
                }
            }
        }


        private void AbrirEdicion(int idCliente)
        {
            FormClienteDetalle detalle =
                new FormClienteDetalle(formPrincipal, idCliente);

            formPrincipal.AbrirFormularioEnPanel(
                detalle,
                formPrincipal.BotonClientes
            );
        }


        // =========================================================
        // ELIMINAR (baja lógica, con confirmación)
        // =========================================================
        private void ConfirmarYEliminar(ClienteListaModelo cliente)
        {
            using FormMensaje confirmacion =
                new FormMensaje(
                    "Eliminar cliente",
                    $"¿Deseás dar de baja a {cliente.Nombre} {cliente.Apellido}? " +
                    "Vas a poder reactivarlo más adelante desde el filtro \"Dados de baja\".",
                    "Sí, eliminar",
                    true
                );

            if (confirmacion.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            ResultadoCliente resultado = clienteLogica.Baja(cliente.IdCliente);

            if (!resultado.Exitoso)
            {
                using FormMensaje error =
                    new FormMensaje("No se pudo eliminar", resultado.Mensaje);

                error.ShowDialog(this);

                return;
            }

            using FormMensaje exito =
                new FormMensaje("Cliente eliminado", resultado.Mensaje);

            exito.ShowDialog(this);

            EjecutarBusqueda();
        }


        // =========================================================
        // REACTIVAR (con confirmación, misma lógica que eliminar)
        // =========================================================
        private void ConfirmarYReactivar(ClienteListaModelo cliente)
        {
            using FormMensaje confirmacion =
                new FormMensaje(
                    "Reactivar cliente",
                    $"¿Deseás reactivar a {cliente.Nombre} {cliente.Apellido}?",
                    "Sí, reactivar",
                    true
                );

            if (confirmacion.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            ResultadoCliente resultado = clienteLogica.Reactivar(cliente.IdCliente);

            if (!resultado.Exitoso)
            {
                using FormMensaje error =
                    new FormMensaje("No se pudo reactivar", resultado.Mensaje);

                error.ShowDialog(this);

                return;
            }

            using FormMensaje exito =
                new FormMensaje("Cliente reactivado", resultado.Mensaje);

            exito.ShowDialog(this);

            EjecutarBusqueda();
        }
    }
}