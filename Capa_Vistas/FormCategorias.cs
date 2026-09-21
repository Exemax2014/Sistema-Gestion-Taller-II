using Capa_Logica;

namespace Capa_Vistas;

// Presenta el mantenimiento embebido de categorías y sus referencias históricas.
public partial class FormCategorias : Form
{
    private readonly FormPrincipal principal;
    private readonly CatalogoLogica logica = new();
    private int? idSeleccionado;
    private bool estadoSeleccionado;

    // Inicializa el listado y habilita acciones según funcionalidades de sesión.
    public FormCategorias(FormPrincipal principal)
    {
        InitializeComponent();
        this.principal = principal;
        ConfigurarGrilla();
        btnProductos.Click += (_, _) => AbrirProductos();
        btnCategorias.Click += (_, _) => CargarListado();
        btnMarcas.Click += (_, _) => AbrirMarcas();
        btnNuevo.Click += (_, _) => NuevaCategoria();
        btnGuardar.Click += (_, _) => GuardarCategoria();
        btnEstado.Click += (_, _) => CambiarEstado();
        Resize += (_, _) => AjustarNavegacionResponsive();
        tlpNavegacion.SizeChanged += (_, _) => AjustarNavegacionResponsive();
        dgvCategorias.SelectionChanged += (_, _) => SeleccionarCategoria();
        ConfigurarPermisos();
        ConfigurarNavegacion();
        CargarListado();
    }

    // Define columnas descriptivas y mantiene el estado visual de la categoría.
    private void ConfigurarGrilla()
    {
        dgvCategorias.AutoGenerateColumns = false;
        dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn { Name="id", DataPropertyName="Id", Visible=false });
        dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn { Name="nombre", HeaderText="Categoría", DataPropertyName="Nombre", AutoSizeMode=DataGridViewAutoSizeColumnMode.Fill });
        dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn { Name="productos", HeaderText="Productos asociados", DataPropertyName="ProductosAsociados", Width=180 });
        dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn { Name="estado", HeaderText="Estado", DataPropertyName="Activo", Width=120 });
        dgvCategorias.CellFormatting += (_, e) =>
        {
            if (e.RowIndex >= 0 && dgvCategorias.Columns[e.ColumnIndex].Name == "estado" && dgvCategorias.Rows[e.RowIndex].DataBoundItem is CategoriaCatalogoModelo item)
            {
                e.Value = item.Activo ? "Activa" : "Inactiva";
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                e.CellStyle.BackColor = item.Activo ? Color.FromArgb(46,125,74) : Color.FromArgb(165,55,55);
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.Font = new Font("Segoe UI",8.5F,FontStyle.Bold);
            }
        };
    }

    // Oculta las vistas sin permiso y restringe las acciones de mantenimiento.
    private void ConfigurarPermisos()
    {
        btnProductos.Visible = logica.Puede("PRODUCTOS_VER");
        btnCategorias.Visible = logica.Puede("CATEGORIAS_VER");
        btnMarcas.Visible = logica.Puede("MARCAS_VER");
        btnNuevo.Visible = logica.Puede("CATEGORIAS_ALTA");
        btnGuardar.Visible = logica.Puede("CATEGORIAS_ALTA") || logica.Puede("CATEGORIAS_MODIFICAR");
        btnEstado.Visible = logica.Puede("CATEGORIAS_BAJA");
    }

    // Resalta la vista actual y conserva los accesos autorizados dinámicamente.
    private void ConfigurarNavegacion()
    {
        AplicarEstiloNavegacion(btnProductos, false);
        AplicarEstiloNavegacion(btnCategorias, true);
        AplicarEstiloNavegacion(btnMarcas, false);
        AjustarNavegacionResponsive();
    }

    // Ajusta la fila de pestañas si el ancho disponible obliga a envolverlas.
    private void AjustarNavegacionResponsive()
    {
        if (tlpNavegacion.ClientSize.Width <= 0 || tlpPrincipal.RowStyles.Count < 2) return;
        List<Button> botonesVisibles = new();
        if (logica.Puede("PRODUCTOS_VER")) botonesVisibles.Add(btnProductos);
        if (logica.Puede("CATEGORIAS_VER")) botonesVisibles.Add(btnCategorias);
        if (logica.Puede("MARCAS_VER")) botonesVisibles.Add(btnMarcas);
        int filas = CalcularFilasNavegacion(botonesVisibles, tlpNavegacion.ClientSize.Width);
        tlpPrincipal.RowStyles[1].Height = filas * 43;
    }

    // Calcula el alto requerido usando solo pestañas permitidas para esta sesión.
    private static int CalcularFilasNavegacion(IEnumerable<Button> botones, int anchoDisponible)
    {
        int filas = 1;
        int anchoFila = 0;
        foreach (Button boton in botones)
        {
            int anchoBoton = boton.Width + boton.Margin.Horizontal;
            if (anchoFila > 0 && anchoFila + anchoBoton > anchoDisponible)
            {
                filas++;
                anchoFila = 0;
            }
            anchoFila += anchoBoton;
        }
        return filas;
    }

    // Aplica el estado activo dorado y el fondo neutro de las demás pestañas.
    private static void AplicarEstiloNavegacion(Button boton, bool activo)
    {
        boton.BackColor = activo ? Color.FromArgb(190, 137, 45) : Color.FromArgb(235, 237, 240);
        boton.ForeColor = activo ? Color.White : Color.FromArgb(55, 59, 64);
    }

    // Carga categorías activas e inactivas conservando el recuento histórico.
    private void CargarListado()
    {
        dgvCategorias.DataSource = logica.ListarCategorias();
        idSeleccionado = null;
        txtNombre.Clear();
        txtDescripcion.Clear();
        btnEstado.Enabled = false;
        btnGuardar.Enabled = logica.Puede("CATEGORIAS_ALTA");
    }

    // Carga el registro seleccionado y prepara su acción disponible.
    private void SeleccionarCategoria()
    {
        if (dgvCategorias.CurrentRow?.DataBoundItem is not CategoriaCatalogoModelo item) return;
        idSeleccionado = item.Id;
        estadoSeleccionado = item.Activo;
        txtNombre.Text = item.Nombre;
        txtDescripcion.Text = item.Descripcion;
        btnEstado.Text = item.Activo ? "Dar de baja" : "Dar de alta";
        btnEstado.BackColor = item.Activo ? Color.FromArgb(165, 55, 55) : Color.FromArgb(46, 125, 74);
        btnEstado.Enabled = logica.Puede("CATEGORIAS_BAJA");
        btnGuardar.Enabled = item.Activo && logica.Puede("CATEGORIAS_MODIFICAR");
    }

    // Limpia el editor para iniciar un alta nueva.
    private void NuevaCategoria()
    {
        idSeleccionado = null;
        txtNombre.Clear();
        txtDescripcion.Clear();
        btnEstado.Enabled = false;
        btnGuardar.Enabled = logica.Puede("CATEGORIAS_ALTA");
        txtNombre.Focus();
    }

    // Guarda una categoría y muestra el resultado devuelto por la capa lógica.
    private void GuardarCategoria()
    {
        if (idSeleccionado.HasValue && !estadoSeleccionado) return;
        ResultadoCatalogo resultado = logica.GuardarCategoria(idSeleccionado ?? 0, txtNombre.Text, txtDescripcion.Text);
        MostrarMensaje(resultado.Exitoso ? "Categorías" : "No se pudo guardar", resultado.Mensaje);
        if (resultado.Exitoso) CargarListado();
    }

    // Confirma una baja o reactivación lógica sin eliminar categorías históricas.
    private void CambiarEstado()
    {
        if (!idSeleccionado.HasValue) return;
        using FormMensaje confirmacion = new("Confirmar cambio de estado", "¿Querés cambiar el estado de la categoría seleccionada?", "Confirmar", true);
        if (confirmacion.ShowDialog(principal) != DialogResult.OK) return;
        ResultadoCatalogo resultado = logica.CambiarEstadoCategoria(idSeleccionado.Value, !estadoSeleccionado);
        MostrarMensaje(resultado.Exitoso ? "Categorías" : "No se pudo actualizar", resultado.Mensaje);
        if (resultado.Exitoso) CargarListado();
    }

    // Regresa a la lista principal de productos dentro del mismo contenedor.
    private void AbrirProductos()
    {
        if (!logica.Puede("PRODUCTOS_VER")) return;
        principal.AbrirFormularioEnPanel(new FormProductos(principal), principal.BotonProductos);
    }

    // Cambia a la gestión interna de marcas sin abrir una ventana modal.
    private void AbrirMarcas()
    {
        if (logica.Puede("MARCAS_VER")) principal.AbrirFormularioEnPanel(new FormMarcas(principal), principal.BotonProductos);
    }

    // Centra el mensaje respecto del formulario principal.
    private void MostrarMensaje(string titulo, string mensaje)
    {
        using FormMensaje form = new(titulo, mensaje);
        form.ShowDialog(principal);
    }
}
