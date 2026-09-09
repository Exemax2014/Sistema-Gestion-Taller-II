using Capa_Logica;

namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormProductos
    //
    // Lista y filtra el catálogo de productos.
    // El detalle se abre dentro de FormPrincipal.
    // ============================================================

    public partial class FormProductos : Form
    {
        private readonly ProductoLogica productoLogica =
            new ProductoLogica();


        private readonly FormPrincipal formPrincipal;


        public FormProductos(
            FormPrincipal formPrincipal)
        {
            InitializeComponent();


            this.formPrincipal =
                formPrincipal;


            ConfigurarGrilla();

            ConfigurarEventos();

            ConfigurarPermisos();

            CargarFiltros();

            CargarProductos();
        }


        // ========================================================
        // GRILLA
        // ========================================================

        private void ConfigurarGrilla()
        {
            dgvProductos.AutoGenerateColumns =
                false;


            dgvProductos.Columns.Clear();


            DataGridViewTextBoxColumn colIdProducto =
                new DataGridViewTextBoxColumn();

            colIdProducto.Name =
                "colIdProducto";

            colIdProducto.DataPropertyName =
                "IdProducto";

            colIdProducto.Visible =
                false;


            DataGridViewTextBoxColumn colCodigo =
                new DataGridViewTextBoxColumn();

            colCodigo.Name =
                "colCodigoBarra";

            colCodigo.DataPropertyName =
                "CodigoBarra";

            colCodigo.HeaderText =
                "Código";

            colCodigo.Width =
                135;


            DataGridViewTextBoxColumn colNombre =
                new DataGridViewTextBoxColumn();

            colNombre.Name =
                "colNombre";

            colNombre.DataPropertyName =
                "Nombre";

            colNombre.HeaderText =
                "Producto";

            colNombre.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;

            colNombre.MinimumWidth =
                180;


            DataGridViewTextBoxColumn colCategoria =
                new DataGridViewTextBoxColumn();

            colCategoria.Name =
                "colCategoria";

            colCategoria.DataPropertyName =
                "Categoria";

            colCategoria.HeaderText =
                "Categoría";

            colCategoria.Width =
                160;


            DataGridViewTextBoxColumn colMarca =
                new DataGridViewTextBoxColumn();

            colMarca.Name =
                "colMarca";

            colMarca.DataPropertyName =
                "Marca";

            colMarca.HeaderText =
                "Marca";

            colMarca.Width =
                145;


            DataGridViewTextBoxColumn colPrecio =
                new DataGridViewTextBoxColumn();

            colPrecio.Name =
                "colPrecioVenta";

            colPrecio.DataPropertyName =
                "PrecioVenta";

            colPrecio.HeaderText =
                "Precio venta";

            colPrecio.DefaultCellStyle.Format =
                "C2";

            colPrecio.Width =
                135;


            DataGridViewTextBoxColumn colEstado =
                new DataGridViewTextBoxColumn();

            colEstado.Name =
                "colEstado";

            colEstado.DataPropertyName =
                "Estado";

            colEstado.HeaderText =
                "Estado";

            colEstado.Width =
                100;


            DataGridViewButtonColumn colDetalle =
                new DataGridViewButtonColumn();

            colDetalle.Name =
                "colDetalle";

            colDetalle.HeaderText =
                "";

            colDetalle.Text =
                "Ver detalle";

            colDetalle.UseColumnTextForButtonValue =
                true;

            colDetalle.FlatStyle =
                FlatStyle.Flat;

            colDetalle.Width =
                115;


            dgvProductos.Columns.AddRange(
                colIdProducto,
                colCodigo,
                colNombre,
                colCategoria,
                colMarca,
                colPrecio,
                colEstado,
                colDetalle
            );
        }


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            btnNuevoProducto.Click +=
                BtnNuevoProducto_Click;


            btnBuscar.Click +=
                BtnBuscar_Click;


            btnLimpiarFiltros.Click +=
                BtnLimpiarFiltros_Click;


            txtBuscar.KeyDown +=
                TxtBuscar_KeyDown;


            dgvProductos.CellContentClick +=
                DgvProductos_CellContentClick;
        }


        // ========================================================
        // PERMISOS
        // ========================================================

        private void ConfigurarPermisos()
        {
            // La Vista no decide qué perfil puede crear.
            // Solamente consulta la regla definida en Lógica.
            btnNuevoProducto.Visible =
                productoLogica.PuedeCrearProducto();
        }


        // ========================================================
        // FILTROS
        // ========================================================

        private void CargarFiltros()
        {
            cmbCategoriaFiltro.Items.Clear();

            cmbCategoriaFiltro.Items.Add(
                "Todas"
            );


            foreach (
                OpcionProductoModelo categoria
                in productoLogica.ObtenerCategorias())
            {
                cmbCategoriaFiltro.Items.Add(
                    categoria.Nombre
                );
            }


            cmbCategoriaFiltro.SelectedIndex =
                0;


            cmbMarcaFiltro.Items.Clear();

            cmbMarcaFiltro.Items.Add(
                "Todas"
            );


            foreach (
                OpcionProductoModelo marca
                in productoLogica.ObtenerMarcas())
            {
                cmbMarcaFiltro.Items.Add(
                    marca.Nombre
                );
            }


            cmbMarcaFiltro.SelectedIndex =
                0;


            cmbEstadoFiltro.Items.Clear();

            cmbEstadoFiltro.Items.Add(
                "Todos"
            );

            cmbEstadoFiltro.Items.Add(
                "Activos"
            );

            cmbEstadoFiltro.Items.Add(
                "Inactivos"
            );


            cmbEstadoFiltro.SelectedIndex =
                0;
        }


        // ========================================================
        // CARGAR PRODUCTOS
        // ========================================================

        private void CargarProductos()
        {
            IEnumerable<ProductoListaModelo> productos =
                productoLogica.ObtenerTodos();


            string texto =
                txtBuscar.Text.Trim();


            string categoria =
                cmbCategoriaFiltro.SelectedItem
                    ?.ToString()
                ?? "Todas";


            string marca =
                cmbMarcaFiltro.SelectedItem
                    ?.ToString()
                ?? "Todas";


            string estado =
                cmbEstadoFiltro.SelectedItem
                    ?.ToString()
                ?? "Todos";


            if (!string.IsNullOrWhiteSpace(texto))
            {
                productos =
                    productos.Where(
                        p =>
                            p.Nombre.Contains(
                                texto,
                                StringComparison.OrdinalIgnoreCase
                            )
                            ||
                            p.CodigoBarra.Contains(
                                texto,
                                StringComparison.OrdinalIgnoreCase
                            )
                    );
            }


            if (categoria != "Todas")
            {
                productos =
                    productos.Where(
                        p =>
                            p.Categoria
                            == categoria
                    );
            }


            if (marca != "Todas")
            {
                productos =
                    productos.Where(
                        p =>
                            p.Marca
                            == marca
                    );
            }


            if (estado == "Activos")
            {
                productos =
                    productos.Where(
                        p => p.Activo
                    );
            }
            else if (estado == "Inactivos")
            {
                productos =
                    productos.Where(
                        p => !p.Activo
                    );
            }


            dgvProductos.DataSource =
                null;


            dgvProductos.DataSource =
                productos.ToList();


            lblCantidad.Text =
                $"{dgvProductos.Rows.Count} producto(s)";
        }


        // ========================================================
        // NUEVO PRODUCTO
        // ========================================================

        private void BtnNuevoProducto_Click(
            object? sender,
            EventArgs e)
        {
            if (!productoLogica.PuedeCrearProducto())
            {
                return;
            }


            formPrincipal.AbrirFormularioEnPanel(
                new FormProductoDetalle(
                    formPrincipal
                ),
                formPrincipal.BotonProductos
            );
        }


        // ========================================================
        // BUSCAR
        // ========================================================

        private void BtnBuscar_Click(
            object? sender,
            EventArgs e)
        {
            CargarProductos();
        }


        private void TxtBuscar_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }


            CargarProductos();


            e.SuppressKeyPress =
                true;
        }


        // ========================================================
        // LIMPIAR FILTROS
        // ========================================================

        private void BtnLimpiarFiltros_Click(
            object? sender,
            EventArgs e)
        {
            txtBuscar.Clear();


            cmbCategoriaFiltro.SelectedIndex =
                0;


            cmbMarcaFiltro.SelectedIndex =
                0;


            cmbEstadoFiltro.SelectedIndex =
                0;


            CargarProductos();
        }


        // ========================================================
        // VER DETALLE
        // ========================================================

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
                != "colDetalle")
            {
                return;
            }


            int idProducto =
                Convert.ToInt32(
                    dgvProductos
                        .Rows[e.RowIndex]
                        .Cells["colIdProducto"]
                        .Value
                );


            formPrincipal.AbrirFormularioEnPanel(
                new FormProductoDetalle(
                    formPrincipal,
                    idProducto
                ),
                formPrincipal.BotonProductos
            );
        }
    }
}