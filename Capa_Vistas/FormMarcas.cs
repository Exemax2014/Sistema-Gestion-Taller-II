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
        Resize += (_, _) => AjustarNavegacionResponsive();
        tlpNavegacion.SizeChanged += (_, _) => AjustarNavegacionResponsive();
        flpCategorias.SizeChanged += (_, _) => AjustarColumnasCategorias();
        flpAcciones.SizeChanged += (_, _) => AjustarAccionesResponsive();
        dgvMarcas.SelectionChanged += (_, _) => SeleccionarMarca();
        ConfigurarPermisos();
        ConfigurarNavegacion();
        AjustarAccionesResponsive();
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
        btnProductos.Visible = logica.Puede("PRODUCTOS_VER");
        btnCategorias.Visible = logica.Puede("CATEGORIAS_VER");
        btnMarcas.Visible = logica.Puede("MARCAS_VER");
        btnNuevo.Visible = logica.Puede("MARCAS_ALTA");
        btnGuardar.Visible = logica.Puede("MARCAS_ALTA") || logica.Puede("MARCAS_MODIFICAR");
        btnEstado.Visible = logica.Puede("MARCAS_BAJA");
    }

    // Resalta la vista actual y conserva los accesos autorizados dinámicamente.
    private void ConfigurarNavegacion()
    {
        AplicarEstiloNavegacion(btnProductos, false);
        AplicarEstiloNavegacion(btnCategorias, false);
        AplicarEstiloNavegacion(btnMarcas, true);
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

    // Carga únicamente categorías activas como opciones para nuevas asociaciones.
    private void CargarCategoriasActivas()
    {
        flpCategorias.SuspendLayout();
        flpCategorias.Controls.Clear();
        foreach (CategoriaCatalogoModelo categoria in logica.ListarCategorias().Where(x => x.Activo))
        {
            flpCategorias.Controls.Add(new CheckBox
            {
                Text = categoria.Nombre,
                Tag = categoria.Id,
                AutoSize = false,
                Height = 28,
                Margin = new Padding(3, 2, 3, 2),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F)
            });
        }
        AjustarColumnasCategorias();
        flpCategorias.ResumeLayout();
    }

    // Reparte las categorías dinámicas en columnas según el ancho disponible, sin scroll horizontal.
    private void AjustarColumnasCategorias()
    {
        int anchoDisponible = flpCategorias.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
        if (anchoDisponible <= 0) return;

        int columnas = anchoDisponible >= 600 ? 6
            : anchoDisponible >= 420 ? 4
            : anchoDisponible >= 300 ? 3
            : anchoDisponible >= 180 ? 2
            : 1;
        int anchoColumna = Math.Max(80, (anchoDisponible - columnas * 6) / columnas);

        foreach (CheckBox check in flpCategorias.Controls.OfType<CheckBox>())
            check.Width = anchoColumna;
    }

    // Mantiene las acciones en una fila mientras caben y las envuelve solo en ventanas estrechas.
    private void AjustarAccionesResponsive()
    {
        if (flpAcciones.ClientSize.Width <= 0) return;
        flpAcciones.FlowDirection = FlowDirection.LeftToRight;
        flpAcciones.WrapContents = flpAcciones.ClientSize.Width < 400;
    }

    // Lista marcas activas e inactivas sin borrar sus asociaciones históricas.
    private void CargarListado()
    {
        dgvMarcas.DataSource = logica.ListarMarcas();
        idSeleccionado = null;
        txtNombre.Clear();
        foreach (CheckBox check in flpCategorias.Controls.OfType<CheckBox>()) check.Checked = false;
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
        foreach (CheckBox check in flpCategorias.Controls.OfType<CheckBox>())
            check.Checked = check.Tag is int id && seleccionadas.Contains(id);
        btnEstado.Text = item.Activo ? "Dar de baja" : "Dar de alta";
        btnEstado.BackColor = item.Activo ? Color.FromArgb(165, 55, 55) : Color.FromArgb(46, 125, 74);
        btnEstado.Enabled = logica.Puede("MARCAS_BAJA");
        btnGuardar.Enabled = item.Activo && logica.Puede("MARCAS_MODIFICAR");
    }

    // Limpia el editor para iniciar una marca con categorías elegidas por el usuario.
    private void NuevaMarca()
    {
        idSeleccionado = null;
        txtNombre.Clear();
        foreach (CheckBox check in flpCategorias.Controls.OfType<CheckBox>()) check.Checked = false;
        btnEstado.Enabled = false;
        btnGuardar.Enabled = logica.Puede("MARCAS_ALTA");
        txtNombre.Focus();
    }

    // Guarda nombre y asociaciones como una sola operación lógica transaccional.
    private void GuardarMarca()
    {
        if (idSeleccionado.HasValue && !estadoSeleccionado) return;
        int[] categorias = flpCategorias.Controls.OfType<CheckBox>()
            .Where(check => check.Checked && check.Tag is int)
            .Select(check => (int)check.Tag!)
            .ToArray();
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
    private void AbrirProductos()
    {
        if (!logica.Puede("PRODUCTOS_VER")) return;
        principal.AbrirFormularioEnPanel(new FormProductos(principal), principal.BotonProductos);
    }

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

}
