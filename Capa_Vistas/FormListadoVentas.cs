using Capa_Logica;

namespace Capa_Vistas
{
    // ============================================================
    // Contexto utilizado por FormListadoVentas.
    //
    // La misma vista puede mostrar:
    // - ventas realizadas por un usuario/vendedor;
    // - compras realizadas por un cliente.
    // ============================================================

    public enum TipoHistorialVentas
    {
        Vendedor = 1,
        Cliente = 2
    }


    public enum OrigenHistorialVentas
    {
        Modal = 0,
        Ventas = 1,
        Usuarios = 2,
        Clientes = 3,
        Reportes = 4
    }


    // ============================================================
    // Formulario: FormListadoVentas
    //
    // Vista reutilizable para consultar historiales de ventas.
    //
    // La autorización real se valida en VentaLogica.
    // ============================================================

    public partial class FormListadoVentas : Form
    {
        private readonly Panel pnlGrilla = new() { BackColor = Color.White };
        private readonly VentaLogica ventaLogica;
        private readonly FormPrincipal? formPrincipal;
        private readonly OrigenHistorialVentas origenHistorial;

        private readonly TipoHistorialVentas tipoHistorial;
        private readonly int idReferencia;
        private readonly string nombreReferencia;


        public FormListadoVentas(
            TipoHistorialVentas tipoHistorial,
            int idReferencia,
            string nombreReferencia)
            : this(
                null,
                OrigenHistorialVentas.Modal,
                tipoHistorial,
                idReferencia,
                nombreReferencia
            )
        {
        }


        public FormListadoVentas(
            FormPrincipal? formPrincipal,
            TipoHistorialVentas tipoHistorial,
            int idReferencia,
            string nombreReferencia)
            : this(
                formPrincipal,
                tipoHistorial == TipoHistorialVentas.Cliente
                    ? OrigenHistorialVentas.Clientes
                    : OrigenHistorialVentas.Usuarios,
                tipoHistorial,
                idReferencia,
                nombreReferencia
            )
        {
        }


        public FormListadoVentas(
            FormPrincipal? formPrincipal,
            OrigenHistorialVentas origenHistorial,
            TipoHistorialVentas tipoHistorial,
            int idReferencia,
            string nombreReferencia)
        {
            InitializeComponent();

            this.formPrincipal =
                formPrincipal;

            this.origenHistorial =
                origenHistorial;

            ventaLogica =
                new VentaLogica();

            this.tipoHistorial =
                tipoHistorial;

            this.idReferencia =
                idReferencia;

            this.nombreReferencia =
                nombreReferencia
                ?? string.Empty;


            ConfigurarGrilla();
            pnlListado.Controls.Remove(dgvVentas);
            pnlGrilla.Controls.Add(dgvVentas);
            pnlListado.Controls.Add(pnlGrilla);
            dgvVentas.Dock = DockStyle.Fill;
            ConfigurarEventos();
            ConfigurarVista();
            PrepararFechas();

            pnlPrincipal.AutoScroll = false;
            dgvVentas.ScrollBars = ScrollBars.Both;
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            pnlAcciones.Visible = false;
            pnlCabecera.Controls.Add(btnCerrar);
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Resize += (_, _) => AjustarLayoutResponsive();
            Load += (_, _) => AjustarLayoutResponsive();

            CargarVentas();
        }

        // Conserva la distribución actual en pantallas amplias y apila el filtro
        // al bajar de ancho, manteniendo la grilla totalmente accesible.
        private void AjustarLayoutResponsive()
        {
            const int margen = 24;
            int ancho = Math.Max(0, pnlPrincipal.ClientSize.Width - margen * 2);
            int altoCabecera = 86;
            int yFiltros = 112;
            bool compacto = ancho < 850;
            int altoFiltros = compacto ? 112 : 68;
            int yListado = yFiltros + altoFiltros + 14;
            int altoDisponible = Math.Max(180, pnlPrincipal.ClientSize.Height - yListado - 24);

            pnlCabecera.SetBounds(margen, 16, ancho, altoCabecera);
            btnCerrar.SetBounds(Math.Max(0, ancho - 150), 8, 140, 38);
            pnlFiltros.SetBounds(margen, yFiltros, ancho, altoFiltros);
            if (!compacto)
            {
                int x = 18;
                lblDesde.SetBounds(x, 22, 52, 24);
                dtpDesde.SetBounds(x + 58, 18, 150, 28);
                lblHasta.SetBounds(x + 230, 22, 48, 24);
                dtpHasta.SetBounds(x + 282, 18, 150, 28);
                btnBuscar.SetBounds(Math.Max(x + 450, ancho - 250), 14, 105, 38);
                btnLimpiar.SetBounds(Math.Max(x + 565, ancho - 132), 14, 105, 38);
            }
            else
            {
                int anchoFecha = Math.Max(120, Math.Min(190, ancho - 140));
                lblDesde.SetBounds(18, 12, 55, 24);
                dtpDesde.SetBounds(78, 10, anchoFecha, 28);
                lblHasta.SetBounds(18, 54, 55, 24);
                dtpHasta.SetBounds(78, 52, anchoFecha, 28);
                int xBotones = Math.Min(ancho - 220, 78 + anchoFecha + 14);
                btnBuscar.SetBounds(Math.Max(18, xBotones), 10, 95, 36);
                btnLimpiar.SetBounds(Math.Max(18, xBotones), 54, 95, 36);
            }
            pnlListado.SetBounds(margen, yListado, ancho, altoDisponible);
            pnlGrilla.SetBounds(18, 50, Math.Max(0, ancho - 36), Math.Max(0, altoDisponible - 76));
            lblCantidad.SetBounds(18, Math.Max(50, altoDisponible - 24), Math.Max(0, ancho - 36), 20);
            pnlPrincipal.AutoScrollMinSize = Size.Empty;
        }


        // ========================================================
        // GRILLA
        // ========================================================

        private void ConfigurarGrilla()
        {
            dgvVentas.AutoGenerateColumns =
                false;

            dgvVentas.Columns.Clear();


            DataGridViewTextBoxColumn colIdVenta =
                new DataGridViewTextBoxColumn
                {
                    Name = "colIdVenta",
                    DataPropertyName = "IdVenta",
                    HeaderText = "Venta",
                    Width = 80
                };


            DataGridViewTextBoxColumn colFecha =
                new DataGridViewTextBoxColumn
                {
                    Name = "colFecha",
                    DataPropertyName = "FechaHora",
                    HeaderText = "Fecha",
                    Width = 145,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "dd/MM/yyyy HH:mm"
                        }
                };


            DataGridViewTextBoxColumn colCliente =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCliente",
                    DataPropertyName = "Cliente",
                    HeaderText = "Cliente",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,

                    MinimumWidth = 160
                };


            DataGridViewTextBoxColumn colVendedor =
                new DataGridViewTextBoxColumn
                {
                    Name = "colVendedor",
                    DataPropertyName = "Vendedor",
                    HeaderText = "Vendedor",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,

                    MinimumWidth = 160
                };


            DataGridViewTextBoxColumn colSucursal =
                new DataGridViewTextBoxColumn
                {
                    Name = "colSucursal",
                    DataPropertyName = "Sucursal",
                    HeaderText = "Sucursal",
                    Width = 170,
                    MinimumWidth = 150
                };


            DataGridViewTextBoxColumn colTipoFactura =
                new DataGridViewTextBoxColumn
                {
                    Name = "colTipoFactura",
                    DataPropertyName = "TipoFactura",
                    HeaderText = "Factura",
                    Width = 100,
                    MinimumWidth = 90
                };


            DataGridViewTextBoxColumn colTotal =
                new DataGridViewTextBoxColumn
                {
                    Name = "colTotal",
                    DataPropertyName = "Total",
                    HeaderText = "Total",
                    Width = 130,
                    MinimumWidth = 115,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            DataGridViewButtonColumn colDetalle =
                new DataGridViewButtonColumn
                {
                    Name = "colDetalle",
                    HeaderText = "",
                    Text = "Ver detalle",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 120,
                    MinimumWidth = 110
                };


            dgvVentas.Columns.AddRange(
                colIdVenta,
                colFecha,
                colCliente,
                colVendedor,
                colSucursal,
                colTipoFactura,
                colTotal,
                colDetalle
            );
        }


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            btnBuscar.Click +=
                BtnBuscar_Click;

            btnLimpiar.Click +=
                BtnLimpiar_Click;

            btnCerrar.Click +=
                BtnCerrar_Click;

            dgvVentas.CellContentClick +=
                DgvVentas_CellContentClick;
        }


        // ========================================================
        // CONFIGURACIÓN SEGÚN CONTEXTO
        // ========================================================

        private void ConfigurarVista()
        {
            if (tipoHistorial == TipoHistorialVentas.Vendedor)
            {
                lblTitulo.Text =
                    "Historial de ventas";

                lblSubtitulo.Text =
                    string.IsNullOrWhiteSpace(
                        nombreReferencia)
                        ? "Consulta las ventas realizadas por el usuario seleccionado."
                        : $"Ventas realizadas por {nombreReferencia}.";

                lblListadoTitulo.Text =
                    "Ventas realizadas";

                if (
                    dgvVentas.Columns.Contains(
                        "colVendedor"))
                {
                    dgvVentas.Columns["colVendedor"]!
                        .Visible = false;
                }
            }
            else
            {
                lblTitulo.Text =
                    "Historial de compras";

                lblSubtitulo.Text =
                    string.IsNullOrWhiteSpace(
                        nombreReferencia)
                        ? "Consulta las compras realizadas por el cliente seleccionado."
                        : $"Compras realizadas por {nombreReferencia}.";

                lblListadoTitulo.Text =
                    "Compras registradas";

                if (
                    dgvVentas.Columns.Contains(
                        "colCliente"))
                {
                    dgvVentas.Columns["colCliente"]!
                        .Visible = false;
                }
            }


            lblListadoDescripcion.Text =
                "Seleccioná una venta para consultar su detalle.";
        }


        // ========================================================
        // FECHAS
        // ========================================================

        private void PrepararFechas()
        {
            dtpHasta.Value =
                DateTime.Today;

            dtpDesde.Value =
                tipoHistorial == TipoHistorialVentas.Cliente
                    ? DateTimePicker.MinimumDateTime
                    : DateTime.Today.AddMonths(-1);
        }


        // ========================================================
        // CARGA
        // ========================================================

        // Valida el rango en la Vista antes de delegar la consulta autoritativa a VentaLogica.
        private void CargarVentas()
        {
            if (!ValidarRangoFechas())
            {
                return;
            }

            ResultadoConsultaVentas resultado;


            if (tipoHistorial == TipoHistorialVentas.Vendedor)
            {
                resultado =
                    ventaLogica.ListarPorVendedor(
                        idReferencia,
                        dtpDesde.Value.Date,
                        dtpHasta.Value.Date
                    );
            }
            else
            {
                resultado =
                    ventaLogica.ListarPorCliente(
                        idReferencia,
                        dtpDesde.Value.Date,
                        dtpHasta.Value.Date
                    );
            }


            if (!resultado.Exitoso)
            {
                dgvVentas.DataSource =
                    null;

                lblCantidad.Text =
                    "0 venta(s)";


                MostrarMensaje(
                    "No se pudo consultar el historial",
                    resultado.Mensaje
                );

                return;
            }


            dgvVentas.DataSource =
                resultado.Ventas;


            lblCantidad.Text =
                $"{resultado.Ventas.Count} venta(s)";
        }


        // Evita consultas con un rango invertido y da feedback inmediato al usuario.
        private bool ValidarRangoFechas()
        {
            if (dtpDesde.Value.Date <= dtpHasta.Value.Date)
            {
                return true;
            }

            MostrarMensaje(
                "Rango de fechas inválido",
                "La fecha Desde no puede ser posterior a la fecha Hasta."
            );

            return false;
        }


        // ========================================================
        // BUSCAR
        // ========================================================

        private void BtnBuscar_Click(
            object? sender,
            EventArgs e)
        {
            CargarVentas();
        }


        // ========================================================
        // LIMPIAR FILTROS
        // ========================================================

        private void BtnLimpiar_Click(
            object? sender,
            EventArgs e)
        {
            PrepararFechas();
            CargarVentas();
        }


        // ========================================================
        // DETALLE
        // ========================================================

        private void DgvVentas_CellContentClick(
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


            if (
                dgvVentas
                    .Columns[e.ColumnIndex]
                    .Name
                !=
                "colDetalle")
            {
                return;
            }


            if (
                dgvVentas
                    .Rows[e.RowIndex]
                    .DataBoundItem
                is not VentaResumenModelo venta)
            {
                return;
            }


            if (formPrincipal == null)
            {
                using FormVentaDetalle detalle =
                    new FormVentaDetalle(
                        venta.IdVenta,
                        tipoHistorial,
                        idReferencia
                    );

                detalle.ShowDialog(
                    this
                );

                return;
            }


            Button botonOrigen =
                origenHistorial == OrigenHistorialVentas.Ventas
                    ? formPrincipal.BotonVentas
                    : origenHistorial == OrigenHistorialVentas.Clientes
                        ? formPrincipal.BotonClientes
                        : formPrincipal.BotonUsuarios;


            formPrincipal.AbrirFormularioEnPanel(
                new FormVentaDetalle(
                    formPrincipal,
                    origenHistorial,
                    venta.IdVenta,
                    tipoHistorial,
                    idReferencia,
                    nombreReferencia
                ),
                botonOrigen
            );
        }


        // ========================================================
        // CERRAR
        // ========================================================

        private void BtnCerrar_Click(
            object? sender,
            EventArgs e)
        {
            if (formPrincipal == null)
            {
                Close();

                return;
            }


            switch (origenHistorial)
            {
                case OrigenHistorialVentas.Ventas:

                    formPrincipal.AbrirFormularioEnPanel(
                        new FormVentas(
                            formPrincipal
                        ),
                        formPrincipal.BotonVentas
                    );

                    break;


                case OrigenHistorialVentas.Clientes:

                    formPrincipal.AbrirFormularioEnPanel(
                        new FormClientes(
                            formPrincipal
                        ),
                        formPrincipal.BotonClientes
                    );

                    break;


                default:

                    formPrincipal.AbrirFormularioEnPanel(
                        new FormUsuarios(
                            formPrincipal
                        ),
                        formPrincipal.BotonUsuarios
                    );

                    break;
            }
        }


        // ========================================================
        // MENSAJES
        // ========================================================

        private void MostrarMensaje(
            string titulo,
            string mensaje)
        {
            using FormMensaje formMensaje =
                new FormMensaje(
                    titulo,
                    mensaje
                );


            formMensaje.ShowDialog(
                (Form?)formPrincipal ?? this
            );
        }
    }
}
