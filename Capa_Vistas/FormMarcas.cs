using Capa_Logica;

namespace Capa_Vistas;

// Presenta el mantenimiento de marcas y asociaciones configurables por categoría.
public partial class FormMarcas : Form
{
    private readonly FormPrincipal principal;
    private readonly CatalogoLogica logica = new();
    private int? idSeleccionado;
    private bool estadoSeleccionado;

    // Inicializa el editor, la selección múltiple y las acciones autorizadas.
    public FormMarcas(FormPrincipal principal)
    {
        InitializeComponent();
        this.principal = principal;
        ConfigurarGrilla();
        btnProductos.Click += (_, _) => AbrirProductos();
        btnCategorias.Click += (_, _) => AbrirCategorias();
        btnMarcas.Click += (_, _) => CargarListado();
        btnNuevo.Click += (_, _) => NuevaMarca();
        btnGuardar.Click += (_, _) => GuardarMarca();
        btnEstado.Click += (_, _) => CambiarEstado();
        dgvMarcas.SelectionChanged += (_, _) => SeleccionarMarca();
        ConfigurarPermisos();
        CargarCategoriasActivas();
        CargarListado();
    }

    // Define las columnas del listado y la presentación del estado lógico.
    private void ConfigurarGrilla()
    {
        dgvMarcas.AutoGenerateColumns = false;
        dgvMarcas.Columns.Add(new DataGridViewTextBoxColumn { Name="id", DataPropertyName="Id", Visible=false });
        dgvMarcas.Columns.Add(new DataGridViewTextBoxColumn { Name="nombre", HeaderText="Marca", DataPropertyName="Nombre", AutoSizeMode=DataGridViewAutoSizeColumnMode.Fill });
        dgvMarcas.Columns.Add(new DataGridViewTextBoxColumn { Name="categorias", HeaderText="Categorías", DataPropertyName="CategoriasAsociadas", Width=140 });
        dgvMarcas.Columns.Add(new DataGridViewTextBoxColumn { Name="estado", HeaderText="Estado", DataPropertyName="Activo", Width=120 });
        dgvMarcas.CellFormatting += (_, e) =>
        {
            if (e.RowIndex >= 0 && dgvMarcas.Columns[e.ColumnIndex].Name == "estado" && dgvMarcas.Rows[e.RowIndex].DataBoundItem is MarcaCatalogoModelo item)
            {
                e.Value = item.Activo ? "Activa" : "Inactiva";
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                e.CellStyle.BackColor = item.Activo ? Color.FromArgb(46,125,74) : Color.FromArgb(165,55,55);
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.Font = new Font("Segoe UI",8.5F,FontStyle.Bold);
            }
        };
    }

    // Oculta navegación y acciones que la sesión no tiene permitidas.
    private void ConfigurarPermisos()
    {
        btnCategorias.Visible = logica.Puede("CATEGORIAS_VER");
        btnMarcas.Visible = logica.Puede("MARCAS_VER");
        btnNuevo.Visible = logica.Puede("MARCAS_ALTA");
        btnGuardar.Visible = logica.Puede("MARCAS_ALTA") || logica.Puede("MARCAS_MODIFICAR");
        btnEstado.Visible = logica.Puede("MARCAS_BAJA");
    }

    // Carga únicamente categorías activas como opciones para nuevas asociaciones.
    private void CargarCategoriasActivas()
    {
        clbCategorias.Items.Clear();
        foreach (CategoriaCatalogoModelo categoria in logica.ListarCategorias().Where(x => x.Activo))
            clbCategorias.Items.Add(new CategoriaSeleccion(categoria.Id, categoria.Nombre));
    }

    // Lista marcas activas e inactivas sin borrar sus asociaciones históricas.
    private void CargarListado()
    {
        dgvMarcas.DataSource = logica.ListarMarcas();
        idSeleccionado = null;
        txtNombre.Clear();
        for (int i = 0; i < clbCategorias.Items.Count; i++) clbCategorias.SetItemChecked(i, false);
        btnEstado.Enabled = false;
        btnGuardar.Enabled = logica.Puede("MARCAS_ALTA");
    }

    // Precarga nombre y categorías al editar una marca existente.
    private void SeleccionarMarca()
    {
        if (dgvMarcas.CurrentRow?.DataBoundItem is not MarcaCatalogoModelo item) return;
        idSeleccionado = item.Id;
        estadoSeleccionado = item.Activo;
        txtNombre.Text = item.Nombre;
        HashSet<int> seleccionadas = logica.ObtenerCategoriasMarca(item.Id).ToHashSet();
        for (int i = 0; i < clbCategorias.Items.Count; i++)
            clbCategorias.SetItemChecked(i, clbCategorias.Items[i] is CategoriaSeleccion cat && seleccionadas.Contains(cat.Id));
        btnEstado.Text = item.Activo ? "Dar de baja" : "Dar de alta";
        btnEstado.Enabled = logica.Puede("MARCAS_BAJA");
        btnGuardar.Enabled = item.Activo && logica.Puede("MARCAS_MODIFICAR");
    }

    // Limpia el editor para iniciar una marca con categorías elegidas por el usuario.
    private void NuevaMarca()
    {
        idSeleccionado = null;
        txtNombre.Clear();
        for (int i = 0; i < clbCategorias.Items.Count; i++) clbCategorias.SetItemChecked(i, false);
        btnEstado.Enabled = false;
        btnGuardar.Enabled = logica.Puede("MARCAS_ALTA");
        txtNombre.Focus();
    }

    // Guarda nombre y asociaciones como una sola operación lógica transaccional.
    private void GuardarMarca()
    {
        if (idSeleccionado.HasValue && !estadoSeleccionado) return;
        int[] categorias = Enumerable.Range(0, clbCategorias.Items.Count)
            .Where(i => clbCategorias.GetItemChecked(i) && clbCategorias.Items[i] is CategoriaSeleccion)
            .Select(i => ((CategoriaSeleccion)clbCategorias.Items[i]).Id).ToArray();
        ResultadoCatalogo resultado = logica.GuardarMarca(idSeleccionado ?? 0, txtNombre.Text, categorias);
        MostrarMensaje(resultado.Exitoso ? "Marcas" : "No se pudo guardar", resultado.Mensaje);
        if (resultado.Exitoso) CargarListado();
    }

    // Solicita confirmación antes de aplicar baja lógica o reactivación.
    private void CambiarEstado()
    {
        if (!idSeleccionado.HasValue) return;
        using FormMensaje confirmacion = new("Confirmar cambio de estado", "¿Querés cambiar el estado de la marca seleccionada?", "Confirmar", true);
        if (confirmacion.ShowDialog(principal) != DialogResult.OK) return;
        ResultadoCatalogo resultado = logica.CambiarEstadoMarca(idSeleccionado.Value, !estadoSeleccionado);
        MostrarMensaje(resultado.Exitoso ? "Marcas" : "No se pudo actualizar", resultado.Mensaje);
        if (resultado.Exitoso) CargarListado();
    }

    // Regresa a la lista de productos dentro de FormPrincipal.
    private void AbrirProductos() => principal.AbrirFormularioEnPanel(new FormProductos(principal), principal.BotonProductos);

    // Cambia a la gestión embebida de categorías cuando la sesión tiene acceso.
    private void AbrirCategorias()
    {
        if (logica.Puede("CATEGORIAS_VER")) principal.AbrirFormularioEnPanel(new FormCategorias(principal), principal.BotonProductos);
    }

    // Centra los mensajes respecto del formulario principal.
    private void MostrarMensaje(string titulo, string mensaje)
    {
        using FormMensaje form = new(titulo, mensaje);
        form.ShowDialog(principal);
    }

    private sealed record CategoriaSeleccion(int Id, string Nombre)
    {
        // Muestra el nombre sin perder el identificador necesario para guardar la relación.
        public override string ToString() => Nombre;
    }
}
