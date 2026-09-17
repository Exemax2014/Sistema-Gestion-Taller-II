using Capa_Logica;
using System.ComponentModel;
using System.Globalization;

namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormVentas
    //
    // Flujo:
    // 1. Buscar productos y armar la venta.
    // 2. Seleccionar cliente y cargar pagos.
    // 3. Confirmar mediante VentaLogica.
    //
    // La Vista NO registra directamente en la base de datos.
    // ============================================================

    public partial class FormVentas : Form
    {
        private readonly VentaLogica ventaLogica;
        private readonly FormPrincipal? formPrincipal;

        private readonly BindingList<ItemVentaVista> itemsVenta;
        private readonly BindingList<PagoVentaVista> pagosVenta;

        private ClienteVentaModelo? clienteSeleccionado;


        public FormVentas()
            : this(null)
        {
        }


        public FormVentas(
            FormPrincipal? formPrincipal)
        {
            InitializeComponent();

            this.formPrincipal =
                formPrincipal;

            ventaLogica =
                new VentaLogica();

            itemsVenta =
                new BindingList<ItemVentaVista>();

            pagosVenta =
                new BindingList<PagoVentaVista>();

            ConfigurarGrillas();
            ConfigurarEventos();
            ConfigurarContexto();
            ConfigurarPermisos();

            CargarMetodosPago();
            ReiniciarVenta();
        }


        // ========================================================
        // CONFIGURACIÓN GENERAL
        // ========================================================

        private void ConfigurarContexto()
        {
            lblVendedorValor.Text =
                $"{SesionActual.Nombre} {SesionActual.Apellido}"
                    .Trim();

            lblSucursalValor.Text =
                string.IsNullOrWhiteSpace(
                    SesionActual.SucursalOperativa)
                    ? "-"
                    : SesionActual.SucursalOperativa;

            lblFechaValor.Text =
                DateTime.Now.ToString(
                    "dd/MM/yyyy HH:mm"
                );
        }


        private void ConfigurarPermisos()
        {
            bool puedeRealizar =
                ventaLogica.PuedeRealizarVentas();

            bool tieneSucursal =
                SesionActual.IdSucursalOperativa.HasValue;


            pnlPasoProductos.Enabled =
                puedeRealizar
                &&
                tieneSucursal;

            pnlPasoFinalizar.Enabled =
                puedeRealizar
                &&
                tieneSucursal;

            btnMisVentas.Visible =
                ventaLogica.PuedeVerVentas();


            if (!puedeRealizar)
            {
                lblSubtitulo.Text =
                    "No tiene permiso para registrar ventas.";

                return;
            }


            if (!tieneSucursal)
            {
                lblSubtitulo.Text =
                    "Seleccione una sucursal específica para comenzar una venta.";
            }
        }


        // ========================================================
        // GRILLAS
        // ========================================================

        private void ConfigurarGrillas()
        {
            ConfigurarGrillaProductos();
            ConfigurarGrillaCarrito();
            ConfigurarGrillaPagos();
        }


        private void ConfigurarGrillaProductos()
        {
            dgvProductos.AutoGenerateColumns =
                false;

            dgvProductos.ReadOnly =
                false;

            dgvProductos.Columns.Clear();


            DataGridViewTextBoxColumn colCodigo =
                new DataGridViewTextBoxColumn
                {
                    Name = "colProductoCodigo",
                    DataPropertyName = "CodigoBarra",
                    HeaderText = "Código",
                    Width = 110,
                    ReadOnly = true
                };


            DataGridViewTextBoxColumn colProducto =
                new DataGridViewTextBoxColumn
                {
                    Name = "colProductoNombre",
                    DataPropertyName = "Nombre",
                    HeaderText = "Producto",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 180,
                    ReadOnly = true
                };


            DataGridViewTextBoxColumn colPrecio =
                new DataGridViewTextBoxColumn
                {
                    Name = "colProductoPrecio",
                    DataPropertyName = "PrecioVenta",
                    HeaderText = "Precio",
                    Width = 100,
                    ReadOnly = true,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            DataGridViewTextBoxColumn colStock =
                new DataGridViewTextBoxColumn
                {
                    Name = "colProductoStock",
                    DataPropertyName = "Stock",
                    HeaderText = "Stock",
                    Width = 70,
                    ReadOnly = true
                };


            DataGridViewTextBoxColumn colCantidad =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCantidadAgregar",
                    HeaderText = "Cant.",
                    Width = 65,
                    ReadOnly = false,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Alignment =
                                DataGridViewContentAlignment.MiddleCenter
                        }
                };


            DataGridViewButtonColumn colAgregar =
                new DataGridViewButtonColumn
                {
                    Name = "colAgregarProducto",
                    HeaderText = "",
                    Text = "Agregar",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 90,
                    ReadOnly = true
                };


            dgvProductos.Columns.AddRange(
                colCodigo,
                colProducto,
                colPrecio,
                colStock,
                colCantidad,
                colAgregar
            );
        }


        private void ConfigurarGrillaCarrito()
        {
            dgvCarrito.AutoGenerateColumns =
                false;

            dgvCarrito.Columns.Clear();


            DataGridViewTextBoxColumn colProducto =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCarritoProducto",
                    DataPropertyName = "Producto",
                    HeaderText = "Producto",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 150
                };


            DataGridViewTextBoxColumn colCantidad =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCarritoCantidad",
                    DataPropertyName = "Cantidad",
                    HeaderText = "Cant.",
                    Width = 55
                };


            DataGridViewTextBoxColumn colPrecio =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCarritoPrecio",
                    DataPropertyName = "PrecioUnitario",
                    HeaderText = "Precio",
                    Width = 95,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            DataGridViewTextBoxColumn colSubtotal =
                new DataGridViewTextBoxColumn
                {
                    Name = "colCarritoSubtotal",
                    DataPropertyName = "Subtotal",
                    HeaderText = "Subtotal",
                    Width = 105,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            DataGridViewButtonColumn colQuitar =
                new DataGridViewButtonColumn
                {
                    Name = "colQuitarProducto",
                    HeaderText = "",
                    Text = "Quitar",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 70
                };


            dgvCarrito.Columns.AddRange(
                colProducto,
                colCantidad,
                colPrecio,
                colSubtotal,
                colQuitar
            );


            dgvCarrito.DataSource =
                itemsVenta;
        }


        private void ConfigurarGrillaPagos()
        {
            dgvPagos.AutoGenerateColumns =
                false;

            dgvPagos.Columns.Clear();


            DataGridViewTextBoxColumn colMetodo =
                new DataGridViewTextBoxColumn
                {
                    Name = "colPagoMetodo",
                    DataPropertyName = "MetodoPago",
                    HeaderText = "Método",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 180
                };


            DataGridViewTextBoxColumn colMonto =
                new DataGridViewTextBoxColumn
                {
                    Name = "colPagoMonto",
                    DataPropertyName = "Monto",
                    HeaderText = "Monto",
                    Width = 130,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "C2",
                            Alignment =
                                DataGridViewContentAlignment.MiddleRight
                        }
                };


            DataGridViewButtonColumn colQuitar =
                new DataGridViewButtonColumn
                {
                    Name = "colQuitarPago",
                    HeaderText = "",
                    Text = "Quitar",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat,
                    Width = 80
                };


            dgvPagos.Columns.AddRange(
                colMetodo,
                colMonto,
                colQuitar
            );


            dgvPagos.DataSource =
                pagosVenta;
        }


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            btnBuscarProducto.Click +=
                BtnBuscarProducto_Click;

            btnMostrarTodos.Click +=
                BtnMostrarTodos_Click;

            txtBuscarProducto.KeyDown +=
                TxtBuscarProducto_KeyDown;

            dgvProductos.CellContentClick +=
                DgvProductos_CellContentClick;

            dgvProductos.EditingControlShowing +=
                DgvProductos_EditingControlShowing;

            dgvProductos.CellValidating +=
                DgvProductos_CellValidating;

            dgvProductos.DataBindingComplete +=
                DgvProductos_DataBindingComplete;


            dgvCarrito.CellContentClick +=
                DgvCarrito_CellContentClick;


            btnContinuar.Click +=
                BtnContinuar_Click;

            btnCancelarVentaPaso1.Click +=
                BtnCancelarVenta_Click;


            btnBuscarCliente.Click +=
                BtnBuscarCliente_Click;

            txtBuscarCliente.KeyDown +=
                TxtBuscarCliente_KeyDown;

            cmbClienteResultados.SelectedIndexChanged +=
                CmbClienteResultados_SelectedIndexChanged;


            txtMontoPago.KeyPress +=
                TxtMontoPago_KeyPress;

            btnCompletarSaldo.Click +=
                BtnCompletarSaldo_Click;

            btnAgregarPago.Click +=
                BtnAgregarPago_Click;

            dgvPagos.CellContentClick +=
                DgvPagos_CellContentClick;


            btnVolverProductos.Click +=
                BtnVolverProductos_Click;

            btnCancelarVentaPaso2.Click +=
                BtnCancelarVenta_Click;

            btnConfirmarVenta.Click +=
                BtnConfirmarVenta_Click;


            btnMisVentas.Click +=
                BtnMisVentas_Click;
        }


        // ========================================================
        // PRODUCTOS
        // ========================================================

        private void CargarProductos(
            string texto)
        {
            if (!SesionActual.IdSucursalOperativa.HasValue)
            {
                dgvProductos.DataSource =
                    null;

                MostrarMensaje(
                    "Sucursal requerida",
                    "Debe seleccionar una sucursal específica para buscar productos."
                );

                return;
            }


            try
            {
                List<ProductoVentaModelo> productos =
                    ventaLogica
                        .BuscarProductosParaVenta(
                            texto.Trim()
                        );


                dgvProductos.DataSource =
                    productos;
            }
            catch (Exception ex)
            {
                dgvProductos.DataSource =
                    null;

                MostrarMensaje(
                    "No se pudieron cargar los productos",
                    ex.Message
                );
            }
        }


        private void BtnBuscarProducto_Click(
            object? sender,
            EventArgs e)
        {
            CargarProductos(
                txtBuscarProducto.Text
            );
        }


        private void BtnMostrarTodos_Click(
            object? sender,
            EventArgs e)
        {
            txtBuscarProducto.Clear();

            CargarProductos(
                string.Empty
            );
        }


        private void TxtBuscarProducto_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }


            CargarProductos(
                txtBuscarProducto.Text
            );

            e.SuppressKeyPress =
                true;
        }


        private void DgvProductos_DataBindingComplete(
            object? sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            foreach (
                DataGridViewRow fila
                in dgvProductos.Rows)
            {
                fila.Cells["colCantidadAgregar"].Value =
                    1;
            }
        }


        private void DgvProductos_CellContentClick(
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
                dgvProductos
                    .Columns[e.ColumnIndex]
                    .Name
                !=
                "colAgregarProducto")
            {
                return;
            }


            if (
                dgvProductos
                    .Rows[e.RowIndex]
                    .DataBoundItem
                is not ProductoVentaModelo producto)
            {
                return;
            }


            AgregarProducto(
                producto,
                dgvProductos.Rows[e.RowIndex]
            );
        }


        private void AgregarProducto(
            ProductoVentaModelo producto,
            DataGridViewRow fila)
        {
            if (producto.Stock <= 0)
            {
                MostrarMensaje(
                    "Sin stock",
                    $"El producto {producto.Nombre} no tiene stock disponible."
                );

                return;
            }


            if (
                !int.TryParse(
                    Convert.ToString(
                        fila.Cells["colCantidadAgregar"].Value
                    ),
                    out int cantidad)
                ||
                cantidad <= 0)
            {
                MostrarMensaje(
                    "Cantidad inválida",
                    "Ingrese una cantidad mayor que cero."
                );

                return;
            }


            ItemVentaVista? existente =
                itemsVenta
                    .FirstOrDefault(
                        item =>
                            item.IdProducto
                            ==
                            producto.IdProducto
                    );


            int cantidadActual =
                existente?.Cantidad
                ??
                0;

            int nuevaCantidad =
                cantidadActual
                +
                cantidad;


            if (nuevaCantidad > producto.Stock)
            {
                MostrarMensaje(
                    "Stock insuficiente",
                    $"Solo hay {producto.Stock} unidad(es) disponibles de {producto.Nombre}. " +
                    $"Ya agregó {cantidadActual} a la venta."
                );

                return;
            }


            if (existente != null)
            {
                existente.Cantidad =
                    nuevaCantidad;

                dgvCarrito.Refresh();
            }
            else
            {
                itemsVenta.Add(
                    new ItemVentaVista
                    {
                        IdProducto =
                            producto.IdProducto,

                        CodigoBarra =
                            producto.CodigoBarra,

                        Producto =
                            producto.Nombre,

                        Cantidad =
                            cantidad,

                        PrecioUnitario =
                            producto.PrecioVenta,

                        StockDisponible =
                            producto.Stock
                    }
                );
            }


            fila.Cells["colCantidadAgregar"].Value =
                1;


            ActualizarResumen();
        }


        // ========================================================
        // VALIDACIÓN PREVENTIVA DE CANTIDAD
        // ========================================================

        private void DgvProductos_EditingControlShowing(
            object? sender,
            DataGridViewEditingControlShowingEventArgs e)
        {
            if (
                dgvProductos.CurrentCell
                ==
                null
                ||
                dgvProductos
                    .Columns[
                        dgvProductos.CurrentCell.ColumnIndex
                    ]
                    .Name
                !=
                "colCantidadAgregar")
            {
                return;
            }


            if (e.Control is TextBox caja)
            {
                caja.KeyPress -=
                    Cantidad_KeyPress;

                caja.KeyPress +=
                    Cantidad_KeyPress;
            }
        }


        private void Cantidad_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (
                char.IsControl(
                    e.KeyChar
                )
                ||
                char.IsDigit(
                    e.KeyChar
                ))
            {
                return;
            }


            e.Handled =
                true;
        }


        private void DgvProductos_CellValidating(
            object? sender,
            DataGridViewCellValidatingEventArgs e)
        {
            if (
                e.RowIndex < 0
                ||
                dgvProductos
                    .Columns[e.ColumnIndex]
                    .Name
                !=
                "colCantidadAgregar")
            {
                return;
            }


            if (
                dgvProductos
                    .Rows[e.RowIndex]
                    .DataBoundItem
                is not ProductoVentaModelo producto)
            {
                return;
            }


            if (
                !int.TryParse(
                    Convert.ToString(
                        e.FormattedValue
                    ),
                    out int cantidad)
                ||
                cantidad <= 0
                ||
                cantidad > producto.Stock)
            {
                dgvProductos
                    .Rows[e.RowIndex]
                    .Cells["colCantidadAgregar"]
                    .Value =
                        producto.Stock > 0
                            ? 1
                            : 0;

                e.Cancel =
                    false;
            }
        }


        // ========================================================
        // CARRITO
        // ========================================================

        private void DgvCarrito_CellContentClick(
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
                dgvCarrito
                    .Columns[e.ColumnIndex]
                    .Name
                !=
                "colQuitarProducto")
            {
                return;
            }


            if (
                e.RowIndex
                <
                itemsVenta.Count)
            {
                itemsVenta.RemoveAt(
                    e.RowIndex
                );

                ActualizarResumen();
            }
        }


        private void BtnContinuar_Click(
            object? sender,
            EventArgs e)
        {
            if (itemsVenta.Count == 0)
            {
                MostrarMensaje(
                    "Venta incompleta",
                    "Debe agregar al menos un producto antes de continuar."
                );

                return;
            }


            MostrarPasoFinalizar();
        }


        private void MostrarPasoProductos()
        {
            pnlPasoFinalizar.Visible =
                false;

            pnlPasoProductos.Visible =
                true;

            lblSubtitulo.Text =
                "Buscá productos, armá la venta y luego completá cliente y pagos.";

            txtBuscarProducto.Focus();
        }


        private void MostrarPasoFinalizar()
        {
            pnlPasoProductos.Visible =
                false;

            pnlPasoFinalizar.Visible =
                true;

            lblSubtitulo.Text =
                "Seleccioná el cliente, completá los pagos y confirmá la operación.";

            ActualizarResumen();

            txtBuscarCliente.Focus();
        }


        // ========================================================
        // CLIENTE
        // ========================================================

        private void BuscarClientes()
        {
            string texto =
                txtBuscarCliente.Text.Trim();


            if (texto.Length < 2)
            {
                MostrarMensaje(
                    "Buscar cliente",
                    "Ingrese al menos 2 caracteres para buscar un cliente."
                );

                return;
            }


            try
            {
                List<ClienteVentaModelo> clientes =
                    ventaLogica
                        .BuscarClientesParaVenta(
                            texto
                        );


                cmbClienteResultados.DataSource =
                    null;

                clienteSeleccionado =
                    null;

                lblClienteSeleccionado.Text =
                    "Sin seleccionar";


                if (clientes.Count == 0)
                {
                    MostrarMensaje(
                        "Buscar cliente",
                        "No se encontraron clientes con ese criterio."
                    );

                    return;
                }


                cmbClienteResultados.DisplayMember =
                    nameof(
                        ClienteVentaModelo.Nombre
                    );

                cmbClienteResultados.ValueMember =
                    nameof(
                        ClienteVentaModelo.IdCliente
                    );

                cmbClienteResultados.DataSource =
                    clientes;


                if (clientes.Count > 0)
                {
                    cmbClienteResultados.SelectedIndex =
                        0;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudieron buscar clientes",
                    ex.Message
                );
            }
        }


        private void BtnBuscarCliente_Click(
            object? sender,
            EventArgs e)
        {
            BuscarClientes();
        }


        private void TxtBuscarCliente_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }


            BuscarClientes();

            e.SuppressKeyPress =
                true;
        }


        private void CmbClienteResultados_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            clienteSeleccionado =
                cmbClienteResultados.SelectedItem
                as ClienteVentaModelo;


            lblClienteSeleccionado.Text =
                clienteSeleccionado?.Nombre
                ??
                "Sin seleccionar";


            ActualizarResumen();
        }


        // ========================================================
        // MÉTODOS DE PAGO
        // ========================================================

        private void CargarMetodosPago()
        {
            try
            {
                List<MetodoPagoModelo> metodos =
                    ventaLogica
                        .ObtenerMetodosPago();


                cmbMetodoPago.DataSource =
                    null;

                cmbMetodoPago.DisplayMember =
                    nameof(
                        MetodoPagoModelo.Nombre
                    );

                cmbMetodoPago.ValueMember =
                    nameof(
                        MetodoPagoModelo.IdMetodoPago
                    );

                cmbMetodoPago.DataSource =
                    metodos;
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudieron cargar los métodos de pago",
                    ex.Message
                );
            }
        }


        private void TxtMontoPago_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (
                char.IsControl(
                    e.KeyChar
                )
                ||
                char.IsDigit(
                    e.KeyChar
                ))
            {
                return;
            }


            if (
                e.KeyChar == ','
                ||
                e.KeyChar == '.')
            {
                TextBox caja =
                    (TextBox)sender!;


                if (
                    !caja.Text.Contains(',')
                    &&
                    !caja.Text.Contains('.'))
                {
                    return;
                }
            }


            e.Handled =
                true;
        }


        private void BtnCompletarSaldo_Click(
            object? sender,
            EventArgs e)
        {
            decimal saldo =
                ObtenerSaldoPendiente();


            txtMontoPago.Text =
                saldo > 0
                    ? saldo.ToString(
                        "0.00",
                        CultureInfo.CurrentCulture
                    )
                    : string.Empty;
        }


        private void BtnAgregarPago_Click(
            object? sender,
            EventArgs e)
        {
            if (
                cmbMetodoPago.SelectedItem
                is not MetodoPagoModelo metodo)
            {
                MostrarMensaje(
                    "Pago incompleto",
                    "Seleccione un método de pago."
                );

                return;
            }


            if (
                !TryObtenerMonto(
                    txtMontoPago.Text,
                    out decimal monto)
                ||
                monto <= 0)
            {
                MostrarMensaje(
                    "Monto inválido",
                    "Ingrese un monto mayor que cero."
                );

                return;
            }


            decimal saldo =
                ObtenerSaldoPendiente();


            if (saldo <= 0)
            {
                MostrarMensaje(
                    "Venta pagada",
                    "La venta ya no tiene saldo pendiente."
                );

                return;
            }


            if (monto > saldo)
            {
                MostrarMensaje(
                    "Monto inválido",
                    $"El monto no puede superar el saldo pendiente de {saldo:C2}."
                );

                return;
            }


            PagoVentaVista? existente =
                pagosVenta
                    .FirstOrDefault(
                        pago =>
                            pago.IdMetodoPago
                            ==
                            metodo.IdMetodoPago
                    );


            if (existente != null)
            {
                existente.Monto +=
                    monto;

                dgvPagos.Refresh();
            }
            else
            {
                pagosVenta.Add(
                    new PagoVentaVista
                    {
                        IdMetodoPago =
                            metodo.IdMetodoPago,

                        MetodoPago =
                            metodo.Nombre,

                        Monto =
                            monto
                    }
                );
            }


            txtMontoPago.Clear();

            ActualizarResumen();
        }


        private void DgvPagos_CellContentClick(
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
                dgvPagos
                    .Columns[e.ColumnIndex]
                    .Name
                !=
                "colQuitarPago")
            {
                return;
            }


            if (
                e.RowIndex
                <
                pagosVenta.Count)
            {
                pagosVenta.RemoveAt(
                    e.RowIndex
                );

                ActualizarResumen();
            }
        }


        // ========================================================
        // RESUMEN
        // ========================================================

        private decimal ObtenerTotalVenta()
        {
            return itemsVenta.Sum(
                item =>
                    item.Subtotal
            );
        }


        private decimal ObtenerTotalPagado()
        {
            return pagosVenta.Sum(
                pago =>
                    pago.Monto
            );
        }


        private decimal ObtenerSaldoPendiente()
        {
            decimal saldo =
                ObtenerTotalVenta()
                -
                ObtenerTotalPagado();


            return saldo < 0
                ? 0
                : saldo;
        }


        private void ActualizarResumen()
        {
            decimal total =
                ObtenerTotalVenta();

            decimal pagado =
                ObtenerTotalPagado();

            decimal saldo =
                total - pagado;


            lblTotalCarrito.Text =
                total.ToString("C2");

            lblTotalVenta.Text =
                total.ToString("C2");

            lblTotalPagado.Text =
                pagado.ToString("C2");

            lblSaldo.Text =
                saldo.ToString("C2");


            int unidades =
                itemsVenta.Sum(
                    item =>
                        item.Cantidad
                );


            lblResumenProductos.Text =
                $"{itemsVenta.Count} producto(s) - {unidades} unidad(es)";

            lblResumenCliente.Text =
                clienteSeleccionado?.Nombre
                ??
                "Sin seleccionar";


            btnContinuar.Enabled =
                itemsVenta.Count > 0;

            btnConfirmarVenta.Enabled =
                clienteSeleccionado != null
                &&
                itemsVenta.Count > 0
                &&
                pagosVenta.Count > 0
                &&
                saldo == 0;
        }


        // ========================================================
        // CONFIRMAR VENTA
        // ========================================================

        private void BtnConfirmarVenta_Click(
            object? sender,
            EventArgs e)
        {
            if (clienteSeleccionado == null)
            {
                MostrarMensaje(
                    "Venta incompleta",
                    "Debe seleccionar un cliente."
                );

                return;
            }


            if (itemsVenta.Count == 0)
            {
                MostrarMensaje(
                    "Venta incompleta",
                    "Debe agregar al menos un producto."
                );

                return;
            }


            if (pagosVenta.Count == 0)
            {
                MostrarMensaje(
                    "Venta incompleta",
                    "Debe agregar al menos un pago."
                );

                return;
            }


            decimal total =
                ObtenerTotalVenta();

            decimal pagado =
                ObtenerTotalPagado();


            if (pagado != total)
            {
                MostrarMensaje(
                    "Venta incompleta",
                    $"El total pagado debe coincidir con el total de la venta. " +
                    $"Pendiente: {(total - pagado):C2}."
                );

                return;
            }


            using FormMensaje confirmacion =
                new FormMensaje(
                    "Confirmar venta",
                    $"¿Desea registrar la venta por {total:C2}?",
                    "Confirmar",
                    true
                );


            if (
                confirmacion.ShowDialog(this)
                !=
                DialogResult.OK)
            {
                return;
            }


            VentaRegistrarModelo venta =
                new VentaRegistrarModelo
                {
                    IdCliente =
                        clienteSeleccionado.IdCliente,

                    Items =
                        itemsVenta
                            .Select(
                                item =>
                                    new VentaItemGuardarModelo
                                    {
                                        IdProducto =
                                            item.IdProducto,

                                        Cantidad =
                                            item.Cantidad
                                    }
                            )
                            .ToList(),

                    Pagos =
                        pagosVenta
                            .Select(
                                pago =>
                                    new VentaPagoGuardarModelo
                                    {
                                        IdMetodoPago =
                                            pago.IdMetodoPago,

                                        Monto =
                                            pago.Monto
                                    }
                            )
                            .ToList()
                };


            ResultadoVenta resultado;


            try
            {
                resultado =
                    ventaLogica.Registrar(
                        venta
                    );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo registrar la venta",
                    ex.Message
                );

                return;
            }


            if (!resultado.Exitoso)
            {
                MostrarMensaje(
                    "No se pudo registrar la venta",
                    resultado.Mensaje
                );

                // El stock/precio pudo haber cambiado mientras se
                // armaba la operación. Volvemos a consultar productos.
                CargarProductos(
                    txtBuscarProducto.Text
                );

                return;
            }


            MostrarMensaje(
                "Venta registrada",
                $"La venta N.º {resultado.IdVenta} se registró correctamente."
            );


            ReiniciarVenta();
        }


        // ========================================================
        // VOLVER / CANCELAR
        // ========================================================

        private void BtnVolverProductos_Click(
            object? sender,
            EventArgs e)
        {
            MostrarPasoProductos();
        }


        private void BtnCancelarVenta_Click(
            object? sender,
            EventArgs e)
        {
            bool hayDatos =
                itemsVenta.Count > 0
                ||
                pagosVenta.Count > 0
                ||
                clienteSeleccionado != null;


            if (hayDatos)
            {
                using FormMensaje confirmacion =
                    new FormMensaje(
                        "Cancelar venta",
                        "¿Desea cancelar la venta actual? Se perderán los productos, el cliente y los pagos cargados.",
                        "Cancelar venta",
                        true
                    );


                if (
                    confirmacion.ShowDialog(this)
                    !=
                    DialogResult.OK)
                {
                    return;
                }
            }


            ReiniciarVenta();
        }


        private void ReiniciarVenta()
        {
            itemsVenta.Clear();
            pagosVenta.Clear();

            clienteSeleccionado =
                null;


            txtBuscarProducto.Clear();

            txtBuscarCliente.Clear();

            txtMontoPago.Clear();


            dgvProductos.DataSource =
                null;

            cmbClienteResultados.DataSource =
                null;

            lblClienteSeleccionado.Text =
                "Sin seleccionar";


            ConfigurarContexto();

            MostrarPasoProductos();

            ActualizarResumen();


            if (
                ventaLogica.PuedeRealizarVentas()
                &&
                SesionActual.IdSucursalOperativa.HasValue)
            {
                CargarProductos(
                    string.Empty
                );
            }
        }


        // ========================================================
        // HISTORIAL PROPIO
        // ========================================================

        private void BtnMisVentas_Click(
            object? sender,
            EventArgs e)
        {
            if (!ventaLogica.PuedeVerVentas())
            {
                MostrarMensaje(
                    "Acceso no permitido",
                    "No tiene permiso para consultar ventas."
                );

                return;
            }


            string nombre =
                $"{SesionActual.Nombre} {SesionActual.Apellido}"
                    .Trim();


            if (formPrincipal == null)
            {
                using FormListadoVentas historial =
                    new FormListadoVentas(
                        TipoHistorialVentas.Vendedor,
                        SesionActual.IdUsuario,
                        nombre
                    );

                historial.ShowDialog(
                    this
                );

                return;
            }


            formPrincipal.AbrirFormularioEnPanel(
                new FormListadoVentas(
                    formPrincipal,
                    OrigenHistorialVentas.Ventas,
                    TipoHistorialVentas.Vendedor,
                    SesionActual.IdUsuario,
                    nombre
                ),
                formPrincipal.BotonVentas
            );
        }


        // ========================================================
        // HELPERS
        // ========================================================

        private static bool TryObtenerMonto(
            string texto,
            out decimal monto)
        {
            texto =
                texto.Trim();


            if (
                decimal.TryParse(
                    texto,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out monto))
            {
                return true;
            }


            return decimal.TryParse(
                texto.Replace(',', '.'),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out monto
            );
        }


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
                this
            );
        }
    }


    // ============================================================
    // MODELOS EXCLUSIVOS DE LA VISTA
    // ============================================================

    public class ItemVentaVista
    {
        public int IdProducto { get; set; }
        public string CodigoBarra { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int StockDisponible { get; set; }

        public decimal Subtotal =>
            PrecioUnitario
            *
            Cantidad;
    }


    public class PagoVentaVista
    {
        public int IdMetodoPago { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }
}
