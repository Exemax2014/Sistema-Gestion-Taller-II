using Capa_Logica;

namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormProductoDetalle
    //
    // Consulta los datos generales del producto
    // y el stock disponible por sucursal.
    //
    // Las reglas de permisos son determinadas
    // por Capa_Logica.
    // ============================================================

    public partial class FormProductoDetalle
        : Form, IControlaCambios
    {
        private readonly ProductoLogica productoLogica =
            new ProductoLogica();


        private readonly InventarioLogica inventarioLogica =
            new InventarioLogica();


        private readonly FormPrincipal formPrincipal;


        private int? idProducto;


        private bool cargandoDatos =
            true;


        private bool hayCambios =
            false;


        private int? idSucursalEditandoStock;


        private bool EsAlta =>
            !idProducto.HasValue;


        // ========================================================
        // CONSTRUCTOR
        // ========================================================

        public FormProductoDetalle(
            FormPrincipal formPrincipal,
            int? idProducto = null)
        {
            InitializeComponent();


            this.formPrincipal =
                formPrincipal;


            this.idProducto =
                idProducto;


            pnlEditarStock.Visible =
                false;


            ConfigurarGrillaStock();

            ConfigurarEventos();

            CargarCombos();

            ConfigurarModo();


            if (idProducto.HasValue)
            {
                CargarProducto();

                CargarStock();
            }
            else
            {
                PrepararAlta();
            }


            cargandoDatos =
                false;


            hayCambios =
                false;


            ActualizarPrecioVenta();
        }


        // ========================================================
        // GRILLA STOCK
        // ========================================================

        private void ConfigurarGrillaStock()
        {
            dgvStockSucursales.AutoGenerateColumns =
                false;


            dgvStockSucursales.Columns.Clear();


            DataGridViewTextBoxColumn colIdSucursal =
                new DataGridViewTextBoxColumn();

            colIdSucursal.Name =
                "colIdSucursal";

            colIdSucursal.DataPropertyName =
                "IdSucursal";

            colIdSucursal.Visible =
                false;


            DataGridViewTextBoxColumn colSucursal =
                new DataGridViewTextBoxColumn();

            colSucursal.Name =
                "colSucursal";

            colSucursal.DataPropertyName =
                "Sucursal";

            colSucursal.HeaderText =
                "Sucursal";

            colSucursal.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;

            colSucursal.MinimumWidth =
                250;


            DataGridViewTextBoxColumn colStock =
                new DataGridViewTextBoxColumn();

            colStock.Name =
                "colStock";

            colStock.DataPropertyName =
                "Stock";

            colStock.HeaderText =
                "Stock";

            colStock.Width =
                100;


            DataGridViewTextBoxColumn colStockMinimo =
                new DataGridViewTextBoxColumn();

            colStockMinimo.Name =
                "colStockMinimo";

            colStockMinimo.DataPropertyName =
                "StockMinimo";

            colStockMinimo.HeaderText =
                "Stock mínimo";

            colStockMinimo.Width =
                130;


            DataGridViewTextBoxColumn colEstado =
                new DataGridViewTextBoxColumn();

            colEstado.Name =
                "colEstadoStock";

            colEstado.DataPropertyName =
                "Estado";

            colEstado.HeaderText =
                "Estado";

            colEstado.Width =
                135;


            DataGridViewButtonColumn colAccion =
                new DataGridViewButtonColumn();

            colAccion.Name =
                "colAccionStock";

            colAccion.HeaderText =
                "";

            colAccion.Text =
                "Modificar";

            colAccion.UseColumnTextForButtonValue =
                true;

            colAccion.FlatStyle =
                FlatStyle.Flat;

            colAccion.Width =
                120;


            dgvStockSucursales.Columns.AddRange(
                colIdSucursal,
                colSucursal,
                colStock,
                colStockMinimo,
                colEstado,
                colAccion
            );
        }


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            btnVolver.Click +=
                BtnVolver_Click;


            btnCancelar.Click +=
                BtnCancelar_Click;


            btnGuardar.Click +=
                BtnGuardar_Click;


            btnEliminar.Click +=
                BtnEliminar_Click;


            dgvStockSucursales.CellContentClick +=
                DgvStockSucursales_CellContentClick;


            dgvStockSucursales.DataBindingComplete +=
                DgvStockSucursales_DataBindingComplete;


            btnGuardarStock.Click +=
                BtnGuardarStock_Click;


            btnCancelarStock.Click +=
                BtnCancelarStock_Click;


            txtPrecioCosto.TextChanged +=
                CampoCambiado;


            txtPorcentajeGanancia.TextChanged +=
                CampoCambiado;


            txtCodigoBarra.TextChanged +=
                CampoCambiado;


            txtNombre.TextChanged +=
                CampoCambiado;


            txtDescripcion.TextChanged +=
                CampoCambiado;


            cmbCategoria.SelectedIndexChanged +=
                CampoCambiado;


            cmbMarca.SelectedIndexChanged +=
                CampoCambiado;


            chkActivo.CheckedChanged +=
                CampoCambiado;
        }


        // ========================================================
        // MODO DEL FORMULARIO
        // ========================================================

        private void ConfigurarModo()
        {
            bool puedeEditarDatos =
                EsAlta
                    ? productoLogica.PuedeCrearProducto()
                    : productoLogica.PuedeModificarProducto();


            cmbCategoria.Enabled =
                puedeEditarDatos;


            cmbMarca.Enabled =
                puedeEditarDatos;


            txtCodigoBarra.ReadOnly =
                !puedeEditarDatos;


            txtNombre.ReadOnly =
                !puedeEditarDatos;


            txtDescripcion.ReadOnly =
                !puedeEditarDatos;


            txtPrecioCosto.ReadOnly =
                !puedeEditarDatos;


            txtPorcentajeGanancia.ReadOnly =
                !puedeEditarDatos;


            chkActivo.Enabled =
                puedeEditarDatos;


            btnGuardar.Visible =
                puedeEditarDatos;


            btnCancelar.Visible =
                puedeEditarDatos;


            btnEliminar.Visible =
                !EsAlta
                &&
                productoLogica.PuedeEliminarProducto();


            lblTitulo.Text =
                EsAlta
                    ? "Nuevo producto"
                    : "Detalle del producto";
        }


        // ========================================================
        // ALTA
        // ========================================================

        private void PrepararAlta()
        {
            chkActivo.Checked =
                true;


            pnlStock.Visible =
                false;


            lblSubtitulo.Text =
                "Completá los datos generales del nuevo producto.";
        }


        // ========================================================
        // COMBOS
        // ========================================================

        private void CargarCombos()
        {
            cmbCategoria.DataSource =
                productoLogica.ObtenerCategorias();


            cmbCategoria.DisplayMember =
                nameof(
                    OpcionProductoModelo.Nombre
                );


            cmbCategoria.ValueMember =
                nameof(
                    OpcionProductoModelo.Id
                );


            List<OpcionProductoModelo> marcas =
                productoLogica.ObtenerMarcas();


            marcas.Insert(
                0,
                new OpcionProductoModelo
                {
                    Id = 0,
                    Nombre = "Sin marca"
                }
            );


            cmbMarca.DataSource =
                marcas;


            cmbMarca.DisplayMember =
                nameof(
                    OpcionProductoModelo.Nombre
                );


            cmbMarca.ValueMember =
                nameof(
                    OpcionProductoModelo.Id
                );
        }


        // ========================================================
        // PRODUCTO
        // ========================================================

        private void CargarProducto()
        {
            if (!idProducto.HasValue)
            {
                return;
            }


            ProductoDetalleModelo? producto =
                productoLogica.ObtenerPorId(
                    idProducto.Value
                );


            if (producto == null)
            {
                MostrarMensaje(
                    "Producto",
                    "No se encontró el producto solicitado."
                );


                VolverListadoSinPreguntar();

                return;
            }


            cmbCategoria.SelectedValue =
                producto.IdCategoria;


            cmbMarca.SelectedValue =
                producto.IdMarca
                ?? 0;


            txtCodigoBarra.Text =
                producto.CodigoBarra;


            txtNombre.Text =
                producto.Nombre;


            txtDescripcion.Text =
                producto.Descripcion;


            txtPrecioCosto.Text =
                producto.PrecioCosto.ToString(
                    "0.00"
                );


            txtPorcentajeGanancia.Text =
                producto.PorcentajeGanancia.ToString(
                    "0.00"
                );


            chkActivo.Checked =
                producto.Activo;


            lblSubtitulo.Text =
                producto.Nombre;
        }


        // ========================================================
        // STOCK
        // ========================================================

        private void CargarStock()
        {
            if (!idProducto.HasValue)
            {
                return;
            }


            List<StockSucursalModelo> stocks =
                inventarioLogica.ObtenerStockProducto(
                    idProducto.Value
                );


            dgvStockSucursales.DataSource =
                null;


            dgvStockSucursales.DataSource =
                stocks;
        }


        // ========================================================
        // GRILLA TERMINÓ DE CARGAR
        // ========================================================

        private void DgvStockSucursales_DataBindingComplete(
            object? sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            ConfigurarBotonesStock();
        }


        // ========================================================
        // BOTONES DE STOCK
        //
        // La Vista NO decide permisos.
        // Solamente utiliza stock.PuedeModificar,
        // calculado por InventarioLogica.
        // ========================================================

        private void ConfigurarBotonesStock()
        {
            foreach (
                DataGridViewRow fila
                in dgvStockSucursales.Rows)
            {
                if (
                    fila.DataBoundItem
                    is not StockSucursalModelo stock)
                {
                    continue;
                }


                if (stock.PuedeModificar)
                {
                    DataGridViewButtonCell boton =
                        new DataGridViewButtonCell();


                    boton.Value =
                        "Modificar";


                    boton.FlatStyle =
                        FlatStyle.Flat;


                    boton.Style.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;


                    fila.Cells["colAccionStock"] =
                        boton;
                }
                else
                {
                    DataGridViewTextBoxCell celdaVacia =
                        new DataGridViewTextBoxCell();


                    celdaVacia.Value =
                        string.Empty;


                    fila.Cells["colAccionStock"] =
                        celdaVacia;
                }
            }
        }


        // ========================================================
        // MODIFICAR STOCK
        // ========================================================

        private void DgvStockSucursales_CellContentClick(
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
                dgvStockSucursales
                    .Columns[e.ColumnIndex]
                    .Name
                != "colAccionStock")
            {
                return;
            }


            DataGridViewRow fila =
                dgvStockSucursales.Rows[e.RowIndex];


            if (
                fila.DataBoundItem
                is not StockSucursalModelo stock)
            {
                return;
            }


            // La regla ya fue calculada por InventarioLogica.
            if (!stock.PuedeModificar)
            {
                return;
            }


            idSucursalEditandoStock =
                stock.IdSucursal;


            lblEditarStockSucursal.Text =
                stock.Sucursal;


            txtNuevoStock.Text =
                stock.Stock.ToString();


            txtStockMinimo.Text =
                stock.StockMinimo.ToString();


            pnlEditarStock.Visible =
                true;


            pnlEditarStock.BringToFront();


            txtNuevoStock.Focus();
        }


        // ========================================================
        // GUARDAR STOCK
        // ========================================================

        private void BtnGuardarStock_Click(
            object? sender,
            EventArgs e)
        {
            if (
                !idProducto.HasValue
                ||
                !idSucursalEditandoStock.HasValue)
            {
                return;
            }


            if (
                !int.TryParse(
                    txtNuevoStock.Text,
                    out int stock
                )
                ||
                stock < 0)
            {
                MostrarMensaje(
                    "Stock",
                    "Ingresá un stock válido."
                );


                return;
            }


            if (
                !int.TryParse(
                    txtStockMinimo.Text,
                    out int stockMinimo
                )
                ||
                stockMinimo < 0)
            {
                MostrarMensaje(
                    "Stock",
                    "Ingresá un stock mínimo válido."
                );


                return;
            }


            ResultadoInventario resultado =
                inventarioLogica.EstablecerStock(
                    idProducto.Value,
                    idSucursalEditandoStock.Value,
                    stock,
                    stockMinimo
                );


            if (!resultado.Exitoso)
            {
                MostrarMensaje(
                    "No se pudo actualizar",
                    resultado.Mensaje
                );


                return;
            }


            CerrarEdicionStock();


            CargarStock();


            MostrarMensaje(
                "Stock actualizado",
                "El stock de la sucursal se actualizó correctamente."
            );
        }


        // ========================================================
        // CANCELAR STOCK
        // ========================================================

        private void BtnCancelarStock_Click(
            object? sender,
            EventArgs e)
        {
            CerrarEdicionStock();
        }


        private void CerrarEdicionStock()
        {
            idSucursalEditandoStock =
                null;


            pnlEditarStock.Visible =
                false;


            txtNuevoStock.Clear();


            txtStockMinimo.Clear();
        }


        // ========================================================
        // CAMBIOS GENERALES
        // ========================================================

        private void CampoCambiado(
            object? sender,
            EventArgs e)
        {
            if (cargandoDatos)
            {
                return;
            }


            hayCambios =
                true;


            ActualizarPrecioVenta();
        }


        // ========================================================
        // PRECIO DE VENTA
        // ========================================================

        private void ActualizarPrecioVenta()
        {
            if (
                !decimal.TryParse(
                    txtPrecioCosto.Text,
                    out decimal costo
                )
                ||
                !decimal.TryParse(
                    txtPorcentajeGanancia.Text,
                    out decimal ganancia
                ))
            {
                lblPrecioVentaValor.Text =
                    "$ 0,00";


                return;
            }


            decimal precioVenta =
                costo
                +
                (
                    costo
                    *
                    ganancia
                    /
                    100m
                );


            lblPrecioVentaValor.Text =
                precioVenta.ToString(
                    "C2"
                );
        }


        // ========================================================
        // GUARDAR PRODUCTO
        // ========================================================

        private void BtnGuardar_Click(
            object? sender,
            EventArgs e)
        {
            int? idCategoria =
                ObtenerIdSeleccionado(
                    cmbCategoria
                );


            int? idMarca =
                ObtenerIdSeleccionado(
                    cmbMarca
                );


            if (idMarca == 0)
            {
                idMarca =
                    null;
            }


            string? error =
                productoLogica.ValidarProducto(
                    idCategoria,
                    txtNombre.Text,
                    txtPrecioCosto.Text,
                    txtPorcentajeGanancia.Text
                );


            if (error != null)
            {
                MostrarMensaje(
                    "Producto",
                    error
                );


                return;
            }


            decimal costo =
                decimal.Parse(
                    txtPrecioCosto.Text
                );


            decimal ganancia =
                decimal.Parse(
                    txtPorcentajeGanancia.Text
                );


            ResultadoProducto resultado;


            if (EsAlta)
            {
                resultado =
                    productoLogica.Alta(
                        idCategoria!.Value,
                        idMarca,
                        txtCodigoBarra.Text,
                        txtNombre.Text,
                        txtDescripcion.Text,
                        costo,
                        ganancia,
                        chkActivo.Checked
                    );
            }
            else
            {
                resultado =
                    productoLogica.Modificar(
                        idProducto!.Value,
                        idCategoria!.Value,
                        idMarca,
                        txtCodigoBarra.Text,
                        txtNombre.Text,
                        txtDescripcion.Text,
                        costo,
                        ganancia,
                        chkActivo.Checked
                    );
            }


            if (!resultado.Exitoso)
            {
                MostrarMensaje(
                    "No se pudo guardar",
                    resultado.Mensaje
                );


                return;
            }


            hayCambios =
                false;


            MostrarMensaje(
                "Producto guardado",
                resultado.Mensaje
            );


            VolverListadoSinPreguntar();
        }


        // ========================================================
        // ELIMINAR PRODUCTO
        // ========================================================

        private void BtnEliminar_Click(
            object? sender,
            EventArgs e)
        {
            if (!idProducto.HasValue)
            {
                return;
            }


            using FormMensaje mensaje =
                new FormMensaje(
                    "Eliminar producto",
                    "¿Deseás dar de baja este producto?",
                    "Sí, eliminar",
                    true
                );


            if (
                mensaje.ShowDialog(this)
                != DialogResult.OK)
            {
                return;
            }


            ResultadoProducto resultado =
                productoLogica.Baja(
                    idProducto.Value
                );


            if (!resultado.Exitoso)
            {
                MostrarMensaje(
                    "No se pudo eliminar",
                    resultado.Mensaje
                );


                return;
            }


            hayCambios =
                false;


            MostrarMensaje(
                "Producto eliminado",
                resultado.Mensaje
            );


            VolverListadoSinPreguntar();
        }


        // ========================================================
        // CAMBIOS SIN GUARDAR
        // ========================================================

        public bool PuedeCerrar()
        {
            if (!hayCambios)
            {
                return true;
            }


            using FormMensaje mensaje =
                new FormMensaje(
                    "Cambios sin guardar",
                    "Hay cambios sin guardar. ¿Deseás salir sin guardarlos?",
                    "Salir sin guardar",
                    true
                );


            return mensaje.ShowDialog(this)
                == DialogResult.OK;
        }


        // ========================================================
        // VOLVER
        // ========================================================

        private void BtnVolver_Click(
            object? sender,
            EventArgs e)
        {
            VolverListado();
        }


        private void BtnCancelar_Click(
            object? sender,
            EventArgs e)
        {
            VolverListado();
        }


        private void VolverListado()
        {
            formPrincipal.AbrirFormularioEnPanel(
                new FormProductos(
                    formPrincipal
                ),
                formPrincipal.BotonProductos
            );
        }


        private void VolverListadoSinPreguntar()
        {
            hayCambios =
                false;


            VolverListado();
        }


        // ========================================================
        // AUXILIARES
        // ========================================================

        private static int? ObtenerIdSeleccionado(
            ComboBox combo)
        {
            if (
                combo.SelectedValue
                is int id)
            {
                return id;
            }


            return null;
        }


        private void MostrarMensaje(
            string titulo,
            string texto)
        {
            using FormMensaje mensaje =
                new FormMensaje(
                    titulo,
                    texto
                );


            mensaje.ShowDialog(this);
        }
    }
}