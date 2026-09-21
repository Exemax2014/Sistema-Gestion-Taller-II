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
        private DataGridViewButtonColumn colCambioEstado = null!;
        private bool actualizandoFiltros;

        public FormClientes(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            ConfigurarGrilla();

            ConfigurarPermisos();

            ConfigurarEventos();
            AjustarLayoutBusqueda();
            EjecutarBusqueda();
        }

        private void ConfigurarPermisos()
        {
            btnNuevoCliente.Visible = clienteLogica.PuedeCrearCliente();
            colDetalle.Visible = clienteLogica.PuedeVerClientes();
            colHistorial.Visible = clienteLogica.PuedeVerHistorialCompras();
            colCambioEstado.Visible =
                clienteLogica.PuedeEliminarCliente() ||
                clienteLogica.PuedeReactivarCliente();
        }

        // Configura las columnas dinámicas y alinea estado y acciones
        // sin alterar las operaciones disponibles para cada cliente.
        private void ConfigurarGrilla()
        {
            dgvClientes.AutoGenerateColumns = false;

            dgvClientes.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvClientes.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;

            dgvClientes.DefaultCellStyle.Padding =
                new Padding(8, 0, 8, 0);

            dgvClientes.RowTemplate.Height = 40;

            dgvClientes.Columns.Clear();

            dgvClientes.Columns.AddRange(
                CrearColumnaTexto("colDni", "DNI", 80F),
                CrearColumnaTexto("colNombre", "Nombre"),
                CrearColumnaTexto("colApellido", "Apellido"),
                CrearColumnaTexto("colTelefono", "Teléfono"),
                CrearColumnaTexto("colEmail", "Email", 130F),
                CrearColumnaTexto("colLocalidad", "Localidad"),
                CrearColumnaTexto("colEstado", "Estado", 90F),
                CrearBoton("colDetalle", string.Empty, 100F, Color.FromArgb(105, 110, 116), Color.White),
                CrearBoton("colHistorial", "Compras", 75F, Color.FromArgb(235, 241, 247), Color.FromArgb(40, 90, 130)),
                CrearBoton("colCambioEstado", "Dar de baja", 90F, Color.FromArgb(165, 55, 55), Color.White)
            );

            colDetalle = (DataGridViewButtonColumn)dgvClientes.Columns["colDetalle"]!;
            colHistorial = (DataGridViewButtonColumn)dgvClientes.Columns["colHistorial"]!;
            colCambioEstado = (DataGridViewButtonColumn)dgvClientes.Columns["colCambioEstado"]!;
            colDetalle.UseColumnTextForButtonValue = false;
            dgvClientes.Columns["colDni"]!.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
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

        // Crea botones de acción con una alineación común para la grilla.
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
                    ForeColor = textoColor,
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = Padding.Empty
                }
            };
        }

        private void ConfigurarEventos()
        {
            btnNuevoCliente.Click += BtnNuevoCliente_Click;
            btnBuscar.Click += (_, _) => EjecutarBusqueda();
            btnLimpiarBusqueda.Click += BtnLimpiarBusqueda_Click;
            txtBuscar.KeyDown += TxtBuscar_KeyDown;
            cmbEstado.SelectedIndexChanged += (_, _) =>
            {
                if (!actualizandoFiltros)
                {
                    EjecutarBusqueda();
                }
            };
            dgvClientes.CellContentClick += DgvClientes_CellContentClick;
            dgvClientes.CellFormatting += DgvClientes_CellFormatting;
            Resize += (_, _) => AjustarLayoutBusqueda();
        }

        // Separa la acción de alta y reacomoda filtros sin superponerlos al reducir el ancho.
        private void AjustarLayoutBusqueda()
        {
            if (pnlBusqueda.ClientSize.Width <= 0 || pnlEncabezado.ClientSize.Width <= 0) return;

            int anchoCabecera = pnlEncabezado.ClientSize.Width;
            btnNuevoCliente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevoCliente.SetBounds(Math.Max(0, anchoCabecera - 180), 16, 180, 42);
            lblTitulo.AutoSize = false;
            lblTitulo.AutoEllipsis = true;
            lblTitulo.SetBounds(0, 2, Math.Max(120, anchoCabecera - 200), 50);

            int ancho = pnlBusqueda.ClientSize.Width;
            const int margen = 18;
            const int espacio = 8;
            int y;
            if (ancho >= 700)
            {
                pnlBusqueda.Height = 78;
                int anchoTexto = Math.Min(430, ancho - margen * 2 - 150 - 100 - 110 - espacio * 3);
                anchoTexto = Math.Max(240, anchoTexto);
                y = 18;
                txtBuscar.SetBounds(margen, y + 5, anchoTexto, 30);
                cmbEstado.SetBounds(margen + anchoTexto + espacio, y + 5, 150, 30);
                btnBuscar.SetBounds(cmbEstado.Right + espacio, y, 100, 40);
                btnLimpiarBusqueda.SetBounds(btnBuscar.Right + espacio, y, 110, 40);
            }
            else if (ancho >= 480)
            {
                pnlBusqueda.Height = 118;
                txtBuscar.SetBounds(margen, 12, ancho - margen * 2, 30);
                cmbEstado.SetBounds(margen, 62, 150, 30);
                btnBuscar.SetBounds(cmbEstado.Right + espacio, 56, 100, 40);
                btnLimpiarBusqueda.SetBounds(btnBuscar.Right + espacio, 56, 110, 40);
            }
            else
            {
                pnlBusqueda.Height = 166;
                int anchoInterno = Math.Max(120, ancho - margen * 2);
                int anchoBoton = Math.Max(90, (anchoInterno - espacio) / 2);
                txtBuscar.SetBounds(margen, 12, anchoInterno, 30);
                cmbEstado.SetBounds(margen, 57, anchoInterno, 30);
                btnBuscar.SetBounds(margen, 102, anchoBoton, 40);
                btnLimpiarBusqueda.SetBounds(margen + anchoBoton + espacio, 102, anchoBoton, 40);
            }
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

        // Consulta clientes y distingue una lista general vacía de una búsqueda sin coincidencias.
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

            if (dgvClientes.Rows.Count == 0)
            {
                bool criteriosActivos = !string.IsNullOrWhiteSpace(texto)
                    || estado != "ACTIVOS";

                lblEstadoVacio.Text = criteriosActivos
                    ? "No se encontraron clientes para la búsqueda ingresada."
                    : "No hay clientes registrados.";
                lblCantidad.Text = string.Empty;
                dgvClientes.Visible = false;
                lblEstadoVacio.Visible = true;
                lblEstadoVacio.BringToFront();
                return;
            }

            lblEstadoVacio.Visible = false;
            dgvClientes.Visible = true;
            dgvClientes.BringToFront();
            lblCantidad.Text = $"{resultado.Count} clientes encontrados";
        }


        // Quita la búsqueda y restaura el estado predeterminado del listado.
        private void BtnLimpiarBusqueda_Click(object? sender, EventArgs e)
        {
            actualizandoFiltros = true;
            txtBuscar.Clear();
            cmbEstado.SelectedIndex = 0;
            actualizandoFiltros = false;
            EjecutarBusqueda();
        }

        // Aplica el código visual de estado y acción principal según
        // el estado lógico del cliente, sin modificar sus permisos ni datos.
        private void DgvClientes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvClientes.Rows[e.RowIndex].Tag is not ClienteListaModelo cliente)
            {
                return;
            }

            string columna = dgvClientes.Columns[e.ColumnIndex].Name;
            if (columna == "colEstado")
            {
                e.Value = cliente.Activo ? "Activo" : "Inactivo";
                AplicarEstiloEstado(e.CellStyle, cliente.Activo);
                e.FormattingApplied = true;
                return;
            }

            if (columna == "colDetalle")
            {
                bool puedeReactivar = clienteLogica.PuedeReactivarCliente();
                e.Value = cliente.Activo ? "Ver detalle" : puedeReactivar ? "Dar de alta" : string.Empty;
                AplicarEstiloAccionPrincipal(e.CellStyle, cliente.Activo || !puedeReactivar);
                e.FormattingApplied = true;
                return;
            }

            if (columna == "colCambioEstado")
            {
                bool mostrarReactivacionAlternativa =
                    !cliente.Activo && !colDetalle.Visible;

                e.Value = cliente.Activo
                    ? "Dar de baja"
                    : mostrarReactivacionAlternativa
                        ? "Dar de alta"
                        : string.Empty;

                if (mostrarReactivacionAlternativa)
                {
                    AplicarEstiloAccionPrincipal(e.CellStyle, false);
                }

                e.FormattingApplied = true;
            }
        }

        // Usa los mismos colores de estado de Usuarios para que las grillas
        // comuniquen de forma consistente si la entidad está disponible.
        private static void AplicarEstiloEstado(DataGridViewCellStyle estilo, bool activo)
        {
            estilo.Alignment = DataGridViewContentAlignment.MiddleCenter;
            estilo.Padding = Padding.Empty;
            estilo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            estilo.ForeColor = Color.White;
            estilo.SelectionForeColor = Color.White;
            estilo.BackColor = activo
                ? Color.FromArgb(46, 125, 74)
                : Color.FromArgb(165, 55, 55);
            estilo.SelectionBackColor = estilo.BackColor;
        }

        // Diferencia la consulta de una entidad activa de la reactivación
        // de una entidad dada de baja mediante el color del botón principal.
        private static void AplicarEstiloAccionPrincipal(DataGridViewCellStyle estilo, bool activo)
        {
            estilo.Alignment = DataGridViewContentAlignment.MiddleCenter;
            estilo.Padding = Padding.Empty;
            estilo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            estilo.ForeColor = Color.White;
            estilo.SelectionForeColor = Color.White;
            estilo.BackColor = activo
                ? Color.FromArgb(105, 110, 116)
                : Color.FromArgb(46, 125, 74);
            estilo.SelectionBackColor = estilo.BackColor;
        }

        private void BtnNuevoCliente_Click(object? sender, EventArgs e)
        {
            formPrincipal.AbrirFormularioEnPanel(
                new FormClienteDetalle(formPrincipal),
                formPrincipal.BotonClientes);
        }

        // Dirige la acción de la fila sin perder el historial ni la baja lógica
        // que siguen disponibles como acciones específicas del módulo.
        private void DgvClientes_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                dgvClientes.Rows[e.RowIndex].Tag is not ClienteListaModelo cliente)
            {
                return;
            }

            switch (dgvClientes.Columns[e.ColumnIndex].Name)
            {
                case "colDetalle":
                    if (cliente.Activo)
                    {
                        AbrirDetalle(cliente.IdCliente);
                    }
                    else
                    {
                        if (!clienteLogica.PuedeReactivarCliente()) return;
                        ConfirmarCambioEstado(cliente, true);
                    }
                    break;
                case "colHistorial":
                    AbrirHistorial(cliente);
                    break;
                case "colCambioEstado" when cliente.Activo:
                    ConfirmarCambioEstado(cliente, false);
                    break;
                case "colCambioEstado":
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

            if (confirmacion.ShowDialog(formPrincipal) != DialogResult.OK)
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
            dialogo.ShowDialog(formPrincipal);
        }
    }
}
