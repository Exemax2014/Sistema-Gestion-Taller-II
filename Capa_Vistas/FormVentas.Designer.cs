using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormVentas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle estiloCabecera1 = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloFila1 = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloCabecera2 = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloFila2 = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloCabecera3 = new DataGridViewCellStyle();
            DataGridViewCellStyle estiloFila3 = new DataGridViewCellStyle();

            pnlPrincipal = new Panel();
            pnlCabecera = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            pnlLineaTitulo = new Panel();
            btnMisVentas = new Button();
            pnlContexto = new Panel();
            lblVendedorTitulo = new Label();
            lblVendedorValor = new Label();
            lblSucursalTitulo = new Label();
            lblSucursalValor = new Label();
            lblFechaTitulo = new Label();
            lblFechaValor = new Label();

            pnlPasoProductos = new Panel();
            pnlBuscarProductos = new Panel();
            lblPasoProductos = new Label();
            lblBuscarProducto = new Label();
            txtBuscarProducto = new TextBox();
            btnBuscarProducto = new Button();
            btnMostrarTodos = new Button();
            lblAyudaBusqueda = new Label();
            dgvProductos = new DataGridView();
            pnlCarrito = new Panel();
            lblCarrito = new Label();
            lblCarritoAyuda = new Label();
            dgvCarrito = new DataGridView();
            lblTotalCarritoTitulo = new Label();
            lblTotalCarrito = new Label();
            btnCancelarVentaPaso1 = new Button();
            btnContinuar = new Button();

            pnlPasoFinalizar = new Panel();
            pnlCliente = new Panel();
            lblPasoFinalizar = new Label();
            lblBuscarCliente = new Label();
            txtBuscarCliente = new TextBox();
            btnBuscarCliente = new Button();
            cmbClienteResultados = new ComboBox();
            lblClienteSeleccionadoTitulo = new Label();
            lblClienteSeleccionado = new Label();
            pnlPagos = new Panel();
            lblPagos = new Label();
            lblMetodoPago = new Label();
            cmbMetodoPago = new ComboBox();
            lblMontoPago = new Label();
            txtMontoPago = new TextBox();
            btnCompletarSaldo = new Button();
            btnAgregarPago = new Button();
            dgvPagos = new DataGridView();
            pnlResumen = new Panel();
            lblResumen = new Label();
            lblResumenProductosTitulo = new Label();
            lblResumenProductos = new Label();
            lblResumenClienteTitulo = new Label();
            lblResumenCliente = new Label();
            lblTotalVentaTitulo = new Label();
            lblTotalVenta = new Label();
            lblTotalPagadoTitulo = new Label();
            lblTotalPagado = new Label();
            lblSaldoTitulo = new Label();
            lblSaldo = new Label();
            btnVolverProductos = new Button();
            btnCancelarVentaPaso2 = new Button();
            btnConfirmarVenta = new Button();

            pnlPrincipal.SuspendLayout();
            pnlCabecera.SuspendLayout();
            pnlContexto.SuspendLayout();
            pnlPasoProductos.SuspendLayout();
            pnlBuscarProductos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            pnlCarrito.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).BeginInit();
            pnlPasoFinalizar.SuspendLayout();
            pnlCliente.SuspendLayout();
            pnlPagos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
            pnlResumen.SuspendLayout();
            SuspendLayout();

            pnlPrincipal.BackColor = Color.FromArgb(246, 248, 250);
            pnlPrincipal.Controls.Add(pnlPasoFinalizar);
            pnlPrincipal.Controls.Add(pnlPasoProductos);
            pnlPrincipal.Controls.Add(pnlContexto);
            pnlPrincipal.Controls.Add(pnlCabecera);
            pnlPrincipal.Dock = DockStyle.Fill;
            pnlPrincipal.Location = new Point(0, 0);
            pnlPrincipal.Name = "pnlPrincipal";
            pnlPrincipal.Size = new Size(1350, 760);

            pnlCabecera.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCabecera.Controls.Add(btnMisVentas);
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Controls.Add(lblSubtitulo);
            pnlCabecera.Controls.Add(pnlLineaTitulo);
            pnlCabecera.Location = new Point(28, 16);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(1294, 84);

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 35, 46);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "Nueva venta";

            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(85, 98, 112);
            lblSubtitulo.Location = new Point(2, 50);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Text = "Buscá productos, armá la venta y luego completá cliente y pagos.";

            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(2, 77);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(95, 3);

            btnMisVentas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMisVentas.BackColor = Color.FromArgb(45, 49, 54);
            btnMisVentas.Cursor = Cursors.Hand;
            btnMisVentas.FlatAppearance.BorderSize = 0;
            btnMisVentas.FlatStyle = FlatStyle.Flat;
            btnMisVentas.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMisVentas.ForeColor = Color.White;
            btnMisVentas.Location = new Point(1134, 12);
            btnMisVentas.Name = "btnMisVentas";
            btnMisVentas.Size = new Size(160, 42);
            btnMisVentas.Text = "Mis ventas";
            btnMisVentas.UseVisualStyleBackColor = false;

            pnlContexto.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlContexto.BackColor = Color.White;
            pnlContexto.BorderStyle = BorderStyle.FixedSingle;
            pnlContexto.Controls.Add(lblVendedorTitulo);
            pnlContexto.Controls.Add(lblVendedorValor);
            pnlContexto.Controls.Add(lblSucursalTitulo);
            pnlContexto.Controls.Add(lblSucursalValor);
            pnlContexto.Controls.Add(lblFechaTitulo);
            pnlContexto.Controls.Add(lblFechaValor);
            pnlContexto.Location = new Point(28, 108);
            pnlContexto.Name = "pnlContexto";
            pnlContexto.Size = new Size(1294, 66);

            lblVendedorTitulo.AutoSize = true;
            lblVendedorTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblVendedorTitulo.ForeColor = Color.FromArgb(100, 108, 116);
            lblVendedorTitulo.Location = new Point(18, 9);
            lblVendedorTitulo.Text = "Vendedor";
            lblVendedorValor.AutoSize = true;
            lblVendedorValor.Font = new Font("Segoe UI", 10F);
            lblVendedorValor.Location = new Point(18, 32);
            lblVendedorValor.Text = "-";

            lblSucursalTitulo.AutoSize = true;
            lblSucursalTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSucursalTitulo.ForeColor = Color.FromArgb(100, 108, 116);
            lblSucursalTitulo.Location = new Point(450, 9);
            lblSucursalTitulo.Text = "Sucursal";
            lblSucursalValor.AutoSize = true;
            lblSucursalValor.Font = new Font("Segoe UI", 10F);
            lblSucursalValor.Location = new Point(450, 32);
            lblSucursalValor.Text = "-";

            lblFechaTitulo.AutoSize = true;
            lblFechaTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblFechaTitulo.ForeColor = Color.FromArgb(100, 108, 116);
            lblFechaTitulo.Location = new Point(885, 9);
            lblFechaTitulo.Text = "Fecha";
            lblFechaValor.AutoSize = true;
            lblFechaValor.Font = new Font("Segoe UI", 10F);
            lblFechaValor.Location = new Point(885, 32);
            lblFechaValor.Text = "-";

            pnlPasoProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlPasoProductos.Controls.Add(pnlCarrito);
            pnlPasoProductos.Controls.Add(pnlBuscarProductos);
            pnlPasoProductos.Location = new Point(28, 188);
            pnlPasoProductos.Name = "pnlPasoProductos";
            pnlPasoProductos.Size = new Size(1294, 548);

            pnlBuscarProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlBuscarProductos.BackColor = Color.White;
            pnlBuscarProductos.BorderStyle = BorderStyle.FixedSingle;
            pnlBuscarProductos.Controls.Add(lblPasoProductos);
            pnlBuscarProductos.Controls.Add(lblBuscarProducto);
            pnlBuscarProductos.Controls.Add(txtBuscarProducto);
            pnlBuscarProductos.Controls.Add(btnBuscarProducto);
            pnlBuscarProductos.Controls.Add(btnMostrarTodos);
            pnlBuscarProductos.Controls.Add(lblAyudaBusqueda);
            pnlBuscarProductos.Controls.Add(dgvProductos);
            pnlBuscarProductos.Location = new Point(0, 0);
            pnlBuscarProductos.Name = "pnlBuscarProductos";
            pnlBuscarProductos.Size = new Size(748, 548);

            lblPasoProductos.AutoSize = true;
            lblPasoProductos.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPasoProductos.ForeColor = Color.FromArgb(27, 42, 58);
            lblPasoProductos.Location = new Point(18, 14);
            lblPasoProductos.Text = "1. Buscar y agregar productos";

            lblBuscarProducto.AutoSize = true;
            lblBuscarProducto.Location = new Point(18, 55);
            lblBuscarProducto.Text = "Buscar producto";

            txtBuscarProducto.Location = new Point(18, 79);
            txtBuscarProducto.PlaceholderText = "Nombre, código, categoría o marca...";
            txtBuscarProducto.Size = new Size(430, 27);

            btnBuscarProducto.BackColor = Color.FromArgb(37, 116, 222);
            btnBuscarProducto.FlatAppearance.BorderSize = 0;
            btnBuscarProducto.FlatStyle = FlatStyle.Flat;
            btnBuscarProducto.ForeColor = Color.White;
            btnBuscarProducto.Location = new Point(458, 76);
            btnBuscarProducto.Size = new Size(112, 34);
            btnBuscarProducto.Text = "Buscar";
            btnBuscarProducto.UseVisualStyleBackColor = false;

            btnMostrarTodos.BackColor = Color.White;
            btnMostrarTodos.FlatStyle = FlatStyle.Flat;
            btnMostrarTodos.Location = new Point(580, 76);
            btnMostrarTodos.Size = new Size(145, 34);
            btnMostrarTodos.Text = "Mostrar todos";

            lblAyudaBusqueda.AutoSize = true;
            lblAyudaBusqueda.Font = new Font("Segoe UI", 8F);
            lblAyudaBusqueda.ForeColor = Color.FromArgb(100, 108, 116);
            lblAyudaBusqueda.Location = new Point(18, 116);
            lblAyudaBusqueda.Text = "La lista muestra stock actual. Seleccioná una fila para agregar.";

            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.AllowUserToResizeRows = false;
            dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProductos.BackgroundColor = Color.White;
            dgvProductos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProductos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            estiloCabecera1.BackColor = Color.FromArgb(48, 65, 82);
            estiloCabecera1.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            estiloCabecera1.ForeColor = Color.White;
            dgvProductos.ColumnHeadersDefaultCellStyle = estiloCabecera1;
            dgvProductos.ColumnHeadersHeight = 40;
            estiloFila1.SelectionBackColor = Color.FromArgb(226, 229, 232);
            estiloFila1.SelectionForeColor = Color.FromArgb(35, 39, 43);
            dgvProductos.DefaultCellStyle = estiloFila1;
            dgvProductos.EnableHeadersVisualStyles = false;
            dgvProductos.Location = new Point(18, 144);
            dgvProductos.MultiSelect = false;
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(707, 385);

            pnlCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlCarrito.BackColor = Color.White;
            pnlCarrito.BorderStyle = BorderStyle.FixedSingle;
            pnlCarrito.Controls.Add(lblCarrito);
            pnlCarrito.Controls.Add(lblCarritoAyuda);
            pnlCarrito.Controls.Add(dgvCarrito);
            pnlCarrito.Controls.Add(lblTotalCarritoTitulo);
            pnlCarrito.Controls.Add(lblTotalCarrito);
            pnlCarrito.Controls.Add(btnCancelarVentaPaso1);
            pnlCarrito.Controls.Add(btnContinuar);
            pnlCarrito.Location = new Point(762, 0);
            pnlCarrito.Name = "pnlCarrito";
            pnlCarrito.Size = new Size(532, 548);

            lblCarrito.AutoSize = true;
            lblCarrito.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCarrito.ForeColor = Color.FromArgb(27, 42, 58);
            lblCarrito.Location = new Point(18, 14);
            lblCarrito.Text = "Productos de la venta";

            lblCarritoAyuda.AutoSize = true;
            lblCarritoAyuda.Font = new Font("Segoe UI", 8F);
            lblCarritoAyuda.ForeColor = Color.FromArgb(100, 108, 116);
            lblCarritoAyuda.Location = new Point(18, 46);
            lblCarritoAyuda.Text = "Podés quitar productos antes de continuar.";

            dgvCarrito.AllowUserToAddRows = false;
            dgvCarrito.AllowUserToDeleteRows = false;
            dgvCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCarrito.BackgroundColor = Color.White;
            dgvCarrito.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCarrito.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            estiloCabecera2.BackColor = Color.FromArgb(48, 65, 82);
            estiloCabecera2.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            estiloCabecera2.ForeColor = Color.White;
            dgvCarrito.ColumnHeadersDefaultCellStyle = estiloCabecera2;
            dgvCarrito.ColumnHeadersHeight = 40;
            estiloFila2.SelectionBackColor = Color.FromArgb(226, 229, 232);
            estiloFila2.SelectionForeColor = Color.FromArgb(35, 39, 43);
            dgvCarrito.DefaultCellStyle = estiloFila2;
            dgvCarrito.EnableHeadersVisualStyles = false;
            dgvCarrito.Location = new Point(18, 76);
            dgvCarrito.MultiSelect = false;
            dgvCarrito.ReadOnly = true;
            dgvCarrito.RowHeadersVisible = false;
            dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarrito.Size = new Size(494, 350);

            lblTotalCarritoTitulo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTotalCarritoTitulo.AutoSize = true;
            lblTotalCarritoTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalCarritoTitulo.Location = new Point(18, 438);
            lblTotalCarritoTitulo.Text = "Total venta";

            lblTotalCarrito.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblTotalCarrito.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTotalCarrito.ForeColor = Color.FromArgb(190, 137, 45);
            lblTotalCarrito.Location = new Point(312, 433);
            lblTotalCarrito.Size = new Size(200, 32);
            lblTotalCarrito.Text = "$0,00";
            lblTotalCarrito.TextAlign = ContentAlignment.MiddleRight;

            btnCancelarVentaPaso1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelarVentaPaso1.BackColor = Color.White;
            btnCancelarVentaPaso1.FlatStyle = FlatStyle.Flat;
            btnCancelarVentaPaso1.ForeColor = Color.FromArgb(150, 35, 35);
            btnCancelarVentaPaso1.Location = new Point(18, 486);
            btnCancelarVentaPaso1.Size = new Size(145, 42);
            btnCancelarVentaPaso1.Text = "Cancelar venta";

            btnContinuar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnContinuar.BackColor = Color.FromArgb(39, 151, 75);
            btnContinuar.FlatAppearance.BorderSize = 0;
            btnContinuar.FlatStyle = FlatStyle.Flat;
            btnContinuar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnContinuar.ForeColor = Color.White;
            btnContinuar.Location = new Point(352, 486);
            btnContinuar.Size = new Size(160, 42);
            btnContinuar.Text = "Continuar";
            btnContinuar.UseVisualStyleBackColor = false;

            pnlPasoFinalizar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlPasoFinalizar.Controls.Add(pnlResumen);
            pnlPasoFinalizar.Controls.Add(pnlPagos);
            pnlPasoFinalizar.Controls.Add(pnlCliente);
            pnlPasoFinalizar.Location = new Point(28, 188);
            pnlPasoFinalizar.Name = "pnlPasoFinalizar";
            pnlPasoFinalizar.Size = new Size(1294, 548);
            pnlPasoFinalizar.Visible = false;

            pnlCliente.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCliente.BackColor = Color.White;
            pnlCliente.BorderStyle = BorderStyle.FixedSingle;
            pnlCliente.Controls.Add(lblPasoFinalizar);
            pnlCliente.Controls.Add(lblBuscarCliente);
            pnlCliente.Controls.Add(txtBuscarCliente);
            pnlCliente.Controls.Add(btnBuscarCliente);
            pnlCliente.Controls.Add(cmbClienteResultados);
            pnlCliente.Controls.Add(lblClienteSeleccionadoTitulo);
            pnlCliente.Controls.Add(lblClienteSeleccionado);
            pnlCliente.Location = new Point(0, 0);
            pnlCliente.Size = new Size(1294, 136);

            lblPasoFinalizar.AutoSize = true;
            lblPasoFinalizar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPasoFinalizar.ForeColor = Color.FromArgb(27, 42, 58);
            lblPasoFinalizar.Location = new Point(18, 12);
            lblPasoFinalizar.Text = "2. Finalizar venta";

            lblBuscarCliente.AutoSize = true;
            lblBuscarCliente.Location = new Point(18, 54);
            lblBuscarCliente.Text = "Buscar cliente";

            txtBuscarCliente.Location = new Point(18, 79);
            txtBuscarCliente.PlaceholderText = "Nombre, apellido o documento...";
            txtBuscarCliente.Size = new Size(385, 27);

            btnBuscarCliente.BackColor = Color.FromArgb(37, 116, 222);
            btnBuscarCliente.FlatAppearance.BorderSize = 0;
            btnBuscarCliente.FlatStyle = FlatStyle.Flat;
            btnBuscarCliente.ForeColor = Color.White;
            btnBuscarCliente.Location = new Point(413, 76);
            btnBuscarCliente.Size = new Size(105, 34);
            btnBuscarCliente.Text = "Buscar";
            btnBuscarCliente.UseVisualStyleBackColor = false;

            cmbClienteResultados.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClienteResultados.Location = new Point(535, 79);
            cmbClienteResultados.Size = new Size(330, 28);

            lblClienteSeleccionadoTitulo.AutoSize = true;
            lblClienteSeleccionadoTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblClienteSeleccionadoTitulo.ForeColor = Color.FromArgb(100, 108, 116);
            lblClienteSeleccionadoTitulo.Location = new Point(895, 55);
            lblClienteSeleccionadoTitulo.Text = "Cliente seleccionado";

            lblClienteSeleccionado.AutoSize = true;
            lblClienteSeleccionado.Font = new Font("Segoe UI", 10F);
            lblClienteSeleccionado.Location = new Point(895, 82);
            lblClienteSeleccionado.Text = "Sin seleccionar";

            pnlPagos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlPagos.BackColor = Color.White;
            pnlPagos.BorderStyle = BorderStyle.FixedSingle;
            pnlPagos.Controls.Add(lblPagos);
            pnlPagos.Controls.Add(lblMetodoPago);
            pnlPagos.Controls.Add(cmbMetodoPago);
            pnlPagos.Controls.Add(lblMontoPago);
            pnlPagos.Controls.Add(txtMontoPago);
            pnlPagos.Controls.Add(btnCompletarSaldo);
            pnlPagos.Controls.Add(btnAgregarPago);
            pnlPagos.Controls.Add(dgvPagos);
            pnlPagos.Location = new Point(0, 150);
            pnlPagos.Size = new Size(748, 398);

            lblPagos.AutoSize = true;
            lblPagos.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPagos.ForeColor = Color.FromArgb(27, 42, 58);
            lblPagos.Location = new Point(18, 14);
            lblPagos.Text = "Formas de pago";

            lblMetodoPago.AutoSize = true;
            lblMetodoPago.Location = new Point(18, 57);
            lblMetodoPago.Text = "Método";

            cmbMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMetodoPago.Location = new Point(18, 82);
            cmbMetodoPago.Size = new Size(230, 28);

            lblMontoPago.AutoSize = true;
            lblMontoPago.Location = new Point(265, 57);
            lblMontoPago.Text = "Monto";

            txtMontoPago.Location = new Point(265, 82);
            txtMontoPago.PlaceholderText = "0,00";
            txtMontoPago.Size = new Size(150, 27);
            txtMontoPago.TextAlign = HorizontalAlignment.Right;

            btnCompletarSaldo.BackColor = Color.White;
            btnCompletarSaldo.FlatStyle = FlatStyle.Flat;
            btnCompletarSaldo.Location = new Point(429, 79);
            btnCompletarSaldo.Size = new Size(128, 34);
            btnCompletarSaldo.Text = "Usar saldo";

            btnAgregarPago.BackColor = Color.FromArgb(37, 116, 222);
            btnAgregarPago.FlatAppearance.BorderSize = 0;
            btnAgregarPago.FlatStyle = FlatStyle.Flat;
            btnAgregarPago.ForeColor = Color.White;
            btnAgregarPago.Location = new Point(570, 79);
            btnAgregarPago.Size = new Size(155, 34);
            btnAgregarPago.Text = "Agregar pago";
            btnAgregarPago.UseVisualStyleBackColor = false;

            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AllowUserToDeleteRows = false;
            dgvPagos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPagos.BackgroundColor = Color.White;
            dgvPagos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPagos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            estiloCabecera3.BackColor = Color.FromArgb(48, 65, 82);
            estiloCabecera3.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            estiloCabecera3.ForeColor = Color.White;
            dgvPagos.ColumnHeadersDefaultCellStyle = estiloCabecera3;
            dgvPagos.ColumnHeadersHeight = 40;
            estiloFila3.SelectionBackColor = Color.FromArgb(226, 229, 232);
            estiloFila3.SelectionForeColor = Color.FromArgb(35, 39, 43);
            dgvPagos.DefaultCellStyle = estiloFila3;
            dgvPagos.EnableHeadersVisualStyles = false;
            dgvPagos.Location = new Point(18, 130);
            dgvPagos.MultiSelect = false;
            dgvPagos.ReadOnly = true;
            dgvPagos.RowHeadersVisible = false;
            dgvPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPagos.Size = new Size(707, 247);

            pnlResumen.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlResumen.BackColor = Color.White;
            pnlResumen.BorderStyle = BorderStyle.FixedSingle;
            pnlResumen.Controls.Add(lblResumen);
            pnlResumen.Controls.Add(lblResumenProductosTitulo);
            pnlResumen.Controls.Add(lblResumenProductos);
            pnlResumen.Controls.Add(lblResumenClienteTitulo);
            pnlResumen.Controls.Add(lblResumenCliente);
            pnlResumen.Controls.Add(lblTotalVentaTitulo);
            pnlResumen.Controls.Add(lblTotalVenta);
            pnlResumen.Controls.Add(lblTotalPagadoTitulo);
            pnlResumen.Controls.Add(lblTotalPagado);
            pnlResumen.Controls.Add(lblSaldoTitulo);
            pnlResumen.Controls.Add(lblSaldo);
            pnlResumen.Controls.Add(btnVolverProductos);
            pnlResumen.Controls.Add(btnCancelarVentaPaso2);
            pnlResumen.Controls.Add(btnConfirmarVenta);
            pnlResumen.Location = new Point(762, 150);
            pnlResumen.Size = new Size(532, 398);

            lblResumen.AutoSize = true;
            lblResumen.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblResumen.ForeColor = Color.FromArgb(27, 42, 58);
            lblResumen.Location = new Point(18, 14);
            lblResumen.Text = "Resumen de venta";

            lblResumenProductosTitulo.AutoSize = true;
            lblResumenProductosTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblResumenProductosTitulo.ForeColor = Color.FromArgb(100, 108, 116);
            lblResumenProductosTitulo.Location = new Point(18, 59);
            lblResumenProductosTitulo.Text = "Productos";
            lblResumenProductos.AutoSize = true;
            lblResumenProductos.Font = new Font("Segoe UI", 10F);
            lblResumenProductos.Location = new Point(18, 82);
            lblResumenProductos.Text = "0 productos";

            lblResumenClienteTitulo.AutoSize = true;
            lblResumenClienteTitulo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblResumenClienteTitulo.ForeColor = Color.FromArgb(100, 108, 116);
            lblResumenClienteTitulo.Location = new Point(18, 120);
            lblResumenClienteTitulo.Text = "Cliente";
            lblResumenCliente.AutoSize = true;
            lblResumenCliente.Font = new Font("Segoe UI", 10F);
            lblResumenCliente.Location = new Point(18, 143);
            lblResumenCliente.Text = "Sin seleccionar";

            lblTotalVentaTitulo.AutoSize = true;
            lblTotalVentaTitulo.Location = new Point(18, 190);
            lblTotalVentaTitulo.Text = "Total venta";
            lblTotalVenta.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTotalVenta.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalVenta.Location = new Point(327, 186);
            lblTotalVenta.Size = new Size(183, 27);
            lblTotalVenta.Text = "$0,00";
            lblTotalVenta.TextAlign = ContentAlignment.MiddleRight;

            lblTotalPagadoTitulo.AutoSize = true;
            lblTotalPagadoTitulo.Location = new Point(18, 223);
            lblTotalPagadoTitulo.Text = "Total pagado";
            lblTotalPagado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTotalPagado.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalPagado.Location = new Point(327, 219);
            lblTotalPagado.Size = new Size(183, 27);
            lblTotalPagado.Text = "$0,00";
            lblTotalPagado.TextAlign = ContentAlignment.MiddleRight;

            lblSaldoTitulo.AutoSize = true;
            lblSaldoTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSaldoTitulo.Location = new Point(18, 259);
            lblSaldoTitulo.Text = "Saldo pendiente";
            lblSaldo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSaldo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSaldo.ForeColor = Color.FromArgb(190, 137, 45);
            lblSaldo.Location = new Point(307, 254);
            lblSaldo.Size = new Size(203, 32);
            lblSaldo.Text = "$0,00";
            lblSaldo.TextAlign = ContentAlignment.MiddleRight;

            btnVolverProductos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnVolverProductos.BackColor = Color.White;
            btnVolverProductos.FlatStyle = FlatStyle.Flat;
            btnVolverProductos.Location = new Point(18, 334);
            btnVolverProductos.Size = new Size(150, 42);
            btnVolverProductos.Text = "Volver";

            btnCancelarVentaPaso2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelarVentaPaso2.BackColor = Color.White;
            btnCancelarVentaPaso2.FlatStyle = FlatStyle.Flat;
            btnCancelarVentaPaso2.ForeColor = Color.FromArgb(150, 35, 35);
            btnCancelarVentaPaso2.Location = new Point(180, 334);
            btnCancelarVentaPaso2.Size = new Size(145, 42);
            btnCancelarVentaPaso2.Text = "Cancelar";

            btnConfirmarVenta.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnConfirmarVenta.BackColor = Color.FromArgb(39, 151, 75);
            btnConfirmarVenta.FlatAppearance.BorderSize = 0;
            btnConfirmarVenta.FlatStyle = FlatStyle.Flat;
            btnConfirmarVenta.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnConfirmarVenta.ForeColor = Color.White;
            btnConfirmarVenta.Location = new Point(342, 334);
            btnConfirmarVenta.Size = new Size(168, 42);
            btnConfirmarVenta.Text = "Confirmar venta";
            btnConfirmarVenta.UseVisualStyleBackColor = false;

            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(246, 248, 250);
            ClientSize = new Size(1350, 760);
            Controls.Add(pnlPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormVentas";
            Text = "Ventas";

            pnlPrincipal.ResumeLayout(false);
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlContexto.ResumeLayout(false);
            pnlContexto.PerformLayout();
            pnlPasoProductos.ResumeLayout(false);
            pnlBuscarProductos.ResumeLayout(false);
            pnlBuscarProductos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            pnlCarrito.ResumeLayout(false);
            pnlCarrito.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).EndInit();
            pnlPasoFinalizar.ResumeLayout(false);
            pnlCliente.ResumeLayout(false);
            pnlCliente.PerformLayout();
            pnlPagos.ResumeLayout(false);
            pnlPagos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
            pnlResumen.ResumeLayout(false);
            pnlResumen.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlPrincipal;
        private Panel pnlCabecera;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Panel pnlLineaTitulo;
        private Button btnMisVentas;
        private Panel pnlContexto;
        private Label lblVendedorTitulo;
        private Label lblVendedorValor;
        private Label lblSucursalTitulo;
        private Label lblSucursalValor;
        private Label lblFechaTitulo;
        private Label lblFechaValor;

        private Panel pnlPasoProductos;
        private Panel pnlBuscarProductos;
        private Label lblPasoProductos;
        private Label lblBuscarProducto;
        private TextBox txtBuscarProducto;
        private Button btnBuscarProducto;
        private Button btnMostrarTodos;
        private Label lblAyudaBusqueda;
        private DataGridView dgvProductos;

        private Panel pnlCarrito;
        private Label lblCarrito;
        private Label lblCarritoAyuda;
        private DataGridView dgvCarrito;
        private Label lblTotalCarritoTitulo;
        private Label lblTotalCarrito;
        private Button btnCancelarVentaPaso1;
        private Button btnContinuar;

        private Panel pnlPasoFinalizar;
        private Panel pnlCliente;
        private Label lblPasoFinalizar;
        private Label lblBuscarCliente;
        private TextBox txtBuscarCliente;
        private Button btnBuscarCliente;
        private ComboBox cmbClienteResultados;
        private Label lblClienteSeleccionadoTitulo;
        private Label lblClienteSeleccionado;

        private Panel pnlPagos;
        private Label lblPagos;
        private Label lblMetodoPago;
        private ComboBox cmbMetodoPago;
        private Label lblMontoPago;
        private TextBox txtMontoPago;
        private Button btnCompletarSaldo;
        private Button btnAgregarPago;
        private DataGridView dgvPagos;

        private Panel pnlResumen;
        private Label lblResumen;
        private Label lblResumenProductosTitulo;
        private Label lblResumenProductos;
        private Label lblResumenClienteTitulo;
        private Label lblResumenCliente;
        private Label lblTotalVentaTitulo;
        private Label lblTotalVenta;
        private Label lblTotalPagadoTitulo;
        private Label lblTotalPagado;
        private Label lblSaldoTitulo;
        private Label lblSaldo;
        private Button btnVolverProductos;
        private Button btnCancelarVentaPaso2;
        private Button btnConfirmarVenta;
    }
}
