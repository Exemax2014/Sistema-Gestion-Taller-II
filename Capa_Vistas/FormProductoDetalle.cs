using Capa_Logica;
using System.Globalization;

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

        private bool cargandoCombos;

        private bool requiereMarcaPorCambioCategoria;


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

            ConfigurarValidacionesVisuales();

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
                CmbCategoria_SelectedIndexChanged;


            cmbMarca.SelectedIndexChanged +=
                CmbMarca_SelectedIndexChanged;


            chkActivo.CheckedChanged +=
                CampoCambiado;
        }


        // Configura límites y filtros preventivos sin reemplazar las validaciones de Lógica.
        private void ConfigurarValidacionesVisuales()
        {
            txtCodigoBarra.MaxLength = 50;
            txtNombre.MaxLength = 100;
            txtPrecioCosto.MaxLength = 19;
            txtPorcentajeGanancia.MaxLength = 6;
            txtNuevoStock.MaxLength = 10;
            txtStockMinimo.MaxLength = 10;

            txtPrecioCosto.KeyPress += DecimalNoNegativo_KeyPress;
            txtPorcentajeGanancia.KeyPress += DecimalNoNegativo_KeyPress;
            txtNuevoStock.KeyPress += EnteroNoNegativo_KeyPress;
            txtStockMinimo.KeyPress += EnteroNoNegativo_KeyPress;
            txtNombre.KeyPress += TextoUnaLinea_KeyPress;
            txtCodigoBarra.KeyPress += CodigoBarra_KeyPress;
        }


        // Previene caracteres no numéricos en el código sin convertirlo a número.
        private static void CodigoBarra_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (e.KeyChar < '0' || e.KeyChar > '9'))
            {
                e.Handled = true;
            }
        }


        // Evita saltos de línea en el nombre sin restringir caracteres habituales de productos.
        private static void TextoUnaLinea_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                e.Handled = true;
            }
        }


        // Permite solo enteros no negativos mientras se edita el stock de una sucursal.
        private static void EnteroNoNegativo_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        // Restringe costo y ganancia al separador decimal local y a dos decimales al escribir.
        private static void DecimalNoNegativo_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
            {
                if (sender is TextBox texto && char.IsDigit(e.KeyChar))
                {
                    string separador = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    int indiceSeparador = texto.Text.IndexOf(separador, StringComparison.Ordinal);

                    if (indiceSeparador >= 0 && texto.SelectionStart > indiceSeparador &&
                        texto.SelectionLength == 0 && texto.Text.Length - indiceSeparador - 1 >= 2)
                    {
                        e.Handled = true;
                    }
                }

                return;
            }

            if (sender is TextBox control)
            {
                string separador = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                if (separador.Length == 1 && e.KeyChar == separador[0] &&
                    !control.Text.Contains(separador, StringComparison.Ordinal))
                {
                    return;
                }
            }

            e.Handled = true;
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
                EsAlta
                    ? productoLogica.PuedeCrearProducto()
                    : productoLogica.PuedeEliminarProducto();


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

        // Carga categorías activas y prepara marcas compatibles con la categoría inicial.
        private void CargarCombos()
        {
            cargandoCombos = true;
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


            CargarMarcasParaCategoria(null, 0);
            cargandoCombos = false;
        }


        // Recarga marcas válidas para la categoría y descarta una selección incompatible.
        private void CmbCategoria_SelectedIndexChanged(object? sender, EventArgs e)
        {
            CampoCambiado(sender, e);
            if (cargandoCombos || cmbCategoria.SelectedValue is not int idCategoria)
            {
                return;
            }

            int idAnterior = ObtenerIdSeleccionado(cmbMarca) ?? 0;
            if (!cargandoDatos && idAnterior > 0
                && !productoLogica.ObtenerMarcasPorCategoria(idCategoria).Any(m => m.Id == idAnterior))
            {
                requiereMarcaPorCambioCategoria = true;
            }
            CargarMarcasParaCategoria(idCategoria, idAnterior);
        }


        // Libera la validación pendiente cuando se elige una marca compatible.
        private void CmbMarca_SelectedIndexChanged(object? sender, EventArgs e)
        {
            CampoCambiado(sender, e);
            if (!cargandoCombos && (ObtenerIdSeleccionado(cmbMarca) ?? 0) > 0)
            {
                requiereMarcaPorCambioCategoria = false;
            }
        }


        // Configura el combo con marcas activas de la categoría seleccionada.
        private void CargarMarcasParaCategoria(int? idCategoria, int idConservar)
        {
            cargandoCombos = true;
            List<OpcionProductoModelo> marcas = idCategoria.HasValue
                ? productoLogica.ObtenerMarcasPorCategoria(idCategoria.Value)
                : new List<OpcionProductoModelo>();
            marcas.Insert(0, new OpcionProductoModelo { Id = 0, Nombre = "Sin marca" });
            cmbMarca.DataSource = marcas;
            cmbMarca.DisplayMember = nameof(OpcionProductoModelo.Nombre);
            cmbMarca.ValueMember = nameof(OpcionProductoModelo.Id);
            cmbMarca.SelectedValue = marcas.Any(m => m.Id == idConservar) ? idConservar : 0;
            cargandoCombos = false;
        }


        // ========================================================
        // PRODUCTO
        // ========================================================

        // Carga el producto y sus marcas válidas antes de permitir modificarlo.
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

            CargarMarcasParaCategoria(producto.IdCategoria, producto.IdMarca ?? 0);


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

            if (requiereMarcaPorCambioCategoria && !idMarca.HasValue)
            {
                MostrarMensaje("Producto", "La marca anterior no corresponde a la categoría seleccionada. Elegí una marca válida para continuar.");
                return;
            }


            if (!IntentarObtenerImportes(out decimal costo, out decimal ganancia))
            {
                return;
            }

            string? error = productoLogica.ValidarProducto(
                idCategoria,
                txtCodigoBarra.Text,
                txtNombre.Text,
                costo,
                ganancia
            );

            if (error != null)
            {
                MostrarMensaje("Producto", error);
                return;
            }


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


        // Comprueba el formato visual de importes, incluso cuando el texto fue pegado.
        private bool IntentarObtenerImportes(out decimal costo, out decimal ganancia)
        {
            bool costoValido = decimal.TryParse(
                txtPrecioCosto.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out costo
            );

            bool gananciaValida = decimal.TryParse(
                txtPorcentajeGanancia.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out ganancia
            );

            if (!costoValido || !gananciaValida)
            {
                MostrarMensaje("Producto", "Ingresá un costo y un porcentaje de ganancia válidos.");
                return false;
            }

            return true;
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
                mensaje.ShowDialog(formPrincipal)
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


            return mensaje.ShowDialog(formPrincipal)
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


            mensaje.ShowDialog(formPrincipal);
        }
    }
}
