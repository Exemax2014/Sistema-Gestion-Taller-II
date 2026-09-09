using System;
using System.ComponentModel;
using System.Windows.Forms;
using Capa_Datos;
using Capa_Logica;

namespace Capa_Vistas
{
    // =====================================================================
    // Formulario: FormVentas
    //
    // Responsabilidad:
    // Permite registrar una nueva venta: buscar cliente, buscar productos,
    // agregarlos a la grilla con su cantidad y calcular el precio total.
    //
    // La estructura visual se encuentra en:
    // FormVentas.Designer.cs
    //
    // Este archivo contiene únicamente:
    // - carga de datos;
    // - navegación;
    // - interacción con Capa_Logica.
    //
    // Regla crítica de seguridad (Vendedor):
    // - El campo Vendedor se autocompleta con el usuario logueado y
    //   queda bloqueado: no puede vender "a nombre de" otro vendedor.
    // - El campo Precio Venta queda bloqueado para el perfil Vendedor:
    //   no puede modificar precios, solo la cantidad.
    //
    // Se carga dentro de FormPrincipal -> pnlContenido y no repite
    // cabecera, menú lateral ni cierre de sesión (ya los pone FormPrincipal).
    // =====================================================================
    public partial class FormVentas : Form
    {
        private readonly ClienteLogica clienteLogica;
        private readonly ProductoLogica productoLogica;

        // Cliente actualmente seleccionado en el buscador.
        private ClienteInfo? clienteSeleccionado;

        // Producto actualmente seleccionado en el buscador
        // (todavía no agregado a la grilla).
        private ProductoInfo? productoSeleccionado;

        // Ítems ya agregados a la venta. BindingList permite que
        // el DataGridView se actualice solo al agregar o quitar filas.
        private readonly BindingList<ItemVentaVista> itemsVenta;

        public FormVentas()
        {
            InitializeComponent();

            clienteLogica = new ClienteLogica();
            productoLogica = new ProductoLogica();

            itemsVenta = new BindingList<ItemVentaVista>();
            dgvItems.AutoGenerateColumns = false;
            dgvItems.DataSource = itemsVenta;

            ConfigurarVendedor();
            ConfigurarPermisosPrecio();
            ConfigurarValoresPorDefecto();
        }

        // ========================================================
        // Autocompleta el campo Vendedor con el usuario logueado
        // y lo deja bloqueado (no editable).
        // ========================================================
        private void ConfigurarVendedor()
        {
            txtVendedorId.Text = SesionActual.IdUsuario.ToString();
            txtVendedorNombre.Text =
                $"{SesionActual.Nombre} {SesionActual.Apellido}";
        }

        // ========================================================
        // El Vendedor no puede modificar el precio de venta:
        // se autocompleta al elegir el producto, pero queda
        // bloqueado. Otros perfiles sí podrían editarlo.
        // ========================================================
        private void ConfigurarPermisosPrecio()
        {
            txtPrecioVenta.ReadOnly =
                SesionActual.Perfil == "Vendedor";
        }

        private void ConfigurarValoresPorDefecto()
        {
            dtpFecha.Value = DateTime.Today;
            cmbTipoFactura.SelectedIndex = 0;
            RecalcularTotal();
        }

        // ========================================================
        // Botón: Buscar cliente
        //
        // Busca clientes mediante Capa_Logica y carga los
        // resultados en el combo para que el usuario elija.
        // ========================================================
        private void BtnBuscarCliente_Click(object? sender, EventArgs e)
        {
            var resultados = clienteLogica.Buscar(txtClienteBuscar.Text);

            cmbClienteResultados.DataSource = null;
            clienteSeleccionado = null;

            if (resultados.Count == 0)
            {
                MessageBox.Show(
                    "No se encontraron clientes con ese criterio.",
                    "Buscar cliente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            cmbClienteResultados.DisplayMember = nameof(ClienteInfo.Nombre);
            cmbClienteResultados.DataSource = resultados;
        }

        private void CmbClienteResultados_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            clienteSeleccionado =
                cmbClienteResultados.SelectedItem as ClienteInfo;
        }

        // ========================================================
        // Botón: Buscar producto
        //
        // Busca productos activos junto con el stock disponible
        // en la sucursal del usuario logueado.
        // ========================================================
        private void BtnBuscarProducto_Click(object? sender, EventArgs e)
        {
            var resultados = productoLogica.Buscar(
                txtProductoBuscar.Text,
                SesionActual.ObtenerIdSucursalOperativa());

            cmbProductoResultados.DataSource = null;
            productoSeleccionado = null;
            LimpiarDatosProducto();

            if (resultados.Count == 0)
            {
                MessageBox.Show(
                    "No se encontraron productos con ese criterio.",
                    "Buscar producto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            cmbProductoResultados.DisplayMember = nameof(ProductoInfo.Nombre);
            cmbProductoResultados.DataSource = resultados;
        }

        private void CmbProductoResultados_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            productoSeleccionado =
                cmbProductoResultados.SelectedItem as ProductoInfo;

            if (productoSeleccionado == null)
            {
                LimpiarDatosProducto();
                return;
            }

            txtDescripcion.Text = productoSeleccionado.Descripcion;
            txtStock.Text = productoSeleccionado.Stock.ToString();
            txtPrecioVenta.Text =
                productoSeleccionado.PrecioVenta.ToString("0.00");

            // La cantidad no puede superar el stock disponible.
            nudCantidad.Maximum =
                productoSeleccionado.Stock > 0 ? productoSeleccionado.Stock : 1;
            nudCantidad.Value = 1;
        }

        private void LimpiarDatosProducto()
        {
            txtDescripcion.Text = string.Empty;
            txtStock.Text = string.Empty;
            txtPrecioVenta.Text = string.Empty;
            nudCantidad.Value = 1;
        }

        // ========================================================
        // Botón: Agregar
        //
        // Agrega el producto seleccionado a la grilla de la venta,
        // usando el precio que se ve en pantalla (respeta el
        // bloqueo de precio para el Vendedor).
        // ========================================================
        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (productoSeleccionado == null)
            {
                MessageBox.Show(
                    "Primero tenés que buscar y elegir un producto.",
                    "Agregar producto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecioVenta.Text, out decimal precio))
            {
                MessageBox.Show(
                    "El precio de venta no es válido.",
                    "Agregar producto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int cantidad = (int)nudCantidad.Value;

            itemsVenta.Add(new ItemVentaVista
            {
                IdProducto = productoSeleccionado.IdProducto,
                Descripcion = productoSeleccionado.Descripcion,
                Cantidad = cantidad,
                PrecioUnitario = precio
            });

            RecalcularTotal();

            // Limpiar el buscador de productos para la próxima carga.
            txtProductoBuscar.Text = string.Empty;
            cmbProductoResultados.DataSource = null;
            productoSeleccionado = null;
            LimpiarDatosProducto();
        }

        // ========================================================
        // Quita un ítem de la grilla al presionar el botón
        // "Quitar" de esa fila.
        // ========================================================
        private void DgvItems_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dgvItems.Columns[e.ColumnIndex].Name == "colQuitar")
            {
                itemsVenta.RemoveAt(e.RowIndex);
                RecalcularTotal();
            }
        }

        private void RecalcularTotal()
        {
            decimal total = 0;

            foreach (ItemVentaVista item in itemsVenta)
            {
                total += item.SubTotal;
            }

            txtPrecioTotal.Text = total.ToString("0.00");
        }

        // ========================================================
        // Botón: Guardar
        //
        // TODO: reemplazar este bloque cuando exista VentaLogica.
        // Debe insertar la venta y el detalle, descontar stock,
        // y usar siempre SesionActual.IdUsuario como vendedor
        // (nunca un valor editable desde la pantalla).
        // ========================================================
        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (clienteSeleccionado == null)
            {
                MessageBox.Show(
                    "Tenés que elegir un cliente antes de guardar.",
                    "Guardar venta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (itemsVenta.Count == 0)
            {
                MessageBox.Show(
                    "Agregá al menos un producto antes de guardar.",
                    "Guardar venta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(
                "Todavía no existe el módulo de Ventas para guardar en la base.\n" +
                "Esta pantalla ya está lista para conectarse en cuanto exista VentaLogica.",
                "Guardar venta",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ========================================================
        // Botón: Cancelar
        //
        // Limpia el formulario para empezar una venta nueva,
        // sin cerrar la pantalla.
        // ========================================================
        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            clienteSeleccionado = null;
            productoSeleccionado = null;

            txtClienteBuscar.Text = string.Empty;
            cmbClienteResultados.DataSource = null;

            txtProductoBuscar.Text = string.Empty;
            cmbProductoResultados.DataSource = null;
            LimpiarDatosProducto();

            itemsVenta.Clear();
            RecalcularTotal();

            ConfigurarValoresPorDefecto();
        }
    }

    // =====================================================================
    // Clase: ItemVentaVista
    //
    // Representa una fila de la grilla de ítems de la venta actual.
    // Es exclusiva de la vista: no se guarda en la base tal cual,
    // sirve solo para mostrar y calcular en pantalla.
    // =====================================================================
    public class ItemVentaVista
    {
        public int IdProducto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal => PrecioUnitario * Cantidad;
    }
}
