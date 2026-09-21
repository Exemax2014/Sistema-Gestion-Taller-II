#nullable disable

namespace Capa_Vistas;

partial class FormCategorias
{
    private TableLayoutPanel tlpPrincipal = null!;
    private Panel pnlCabecera = null!;
    private Label lblTitulo = null!;
    private Label lblSubtitulo = null!;
    private Panel pnlLineaTitulo = null!;
    private FlowLayoutPanel tlpNavegacion = null!;
    private Button btnProductos = null!;
    private Button btnCategorias = null!;
    private Button btnMarcas = null!;
    private FlowLayoutPanel flpEdicion = null!;
    private TextBox txtNombre = null!;
    private TextBox txtDescripcion = null!;
    private Button btnNuevo = null!;
    private Button btnGuardar = null!;
    private Button btnEstado = null!;
    private DataGridView dgvCategorias = null!;

    private void InitializeComponent()
    {
        tlpPrincipal = new TableLayoutPanel();
        pnlCabecera = new Panel();
        lblTitulo = new Label();
        lblSubtitulo = new Label();
        pnlLineaTitulo = new Panel();
        tlpNavegacion = new FlowLayoutPanel();
        btnProductos = new Button();
        btnCategorias = new Button();
        btnMarcas = new Button();
        flpEdicion = new FlowLayoutPanel();
        txtNombre = new TextBox();
        txtDescripcion = new TextBox();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnEstado = new Button();
        dgvCategorias = new DataGridView();
        tlpPrincipal.SuspendLayout();
        pnlCabecera.SuspendLayout();
        tlpNavegacion.SuspendLayout();
        flpEdicion.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCategorias).BeginInit();
        SuspendLayout();

        tlpPrincipal.BackColor = Color.FromArgb(241, 243, 245);
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPrincipal.RowCount = 4;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 94F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpPrincipal.Padding = new Padding(32, 20, 32, 24);
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
        tlpPrincipal.Controls.Add(tlpNavegacion, 0, 1);
        tlpPrincipal.Controls.Add(flpEdicion, 0, 2);
        tlpPrincipal.Controls.Add(dgvCategorias, 0, 3);

        pnlCabecera.Dock = DockStyle.Fill;
        pnlCabecera.Controls.Add(lblTitulo);
        pnlCabecera.Controls.Add(lblSubtitulo);
        pnlCabecera.Controls.Add(pnlLineaTitulo);
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(45, 49, 54);
        lblTitulo.Location = new Point(0, 2);
        lblTitulo.AutoSize = true;
        lblTitulo.Text = "Categorías";
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
        lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
        lblSubtitulo.Location = new Point(2, 54);
        lblSubtitulo.Text = "Administración del catálogo de categorías.";
        pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
        pnlLineaTitulo.Location = new Point(2, 84);
        pnlLineaTitulo.Size = new Size(92, 3);

        tlpNavegacion.Dock = DockStyle.Fill;
        tlpNavegacion.FlowDirection = FlowDirection.LeftToRight;
        tlpNavegacion.WrapContents = true;
        tlpNavegacion.Margin = Padding.Empty;
        ConfigurarBotonNavegacion(btnProductos, "Productos", 120);
        ConfigurarBotonNavegacion(btnCategorias, "Categorías", 145);
        ConfigurarBotonNavegacion(btnMarcas, "Marcas", 110);
        tlpNavegacion.Controls.Add(btnProductos);
        tlpNavegacion.Controls.Add(btnCategorias);
        tlpNavegacion.Controls.Add(btnMarcas);

        flpEdicion.BackColor = Color.White;
        flpEdicion.Dock = DockStyle.Fill;
        flpEdicion.Padding = new Padding(10);
        flpEdicion.WrapContents = true;
        flpEdicion.AutoScroll = true;
        txtNombre.MaxLength = 100;
        txtNombre.Width = 230;
        txtNombre.PlaceholderText = "Nombre de categoría";
        txtNombre.Font = new Font("Segoe UI", 9.5F);
        txtDescripcion.MaxLength = 200;
        txtDescripcion.Width = 280;
        txtDescripcion.PlaceholderText = "Descripción (opcional)";
        txtDescripcion.Font = new Font("Segoe UI", 9.5F);
        ConfigurarBotonAccion(btnNuevo, "+ Nueva", Color.FromArgb(82, 88, 94));
        ConfigurarBotonAccion(btnGuardar, "Guardar", Color.FromArgb(190, 137, 45));
        ConfigurarBotonAccion(btnEstado, "Dar de baja", Color.FromArgb(165, 55, 55));
        flpEdicion.Controls.Add(txtNombre);
        flpEdicion.Controls.Add(txtDescripcion);
        flpEdicion.Controls.Add(btnNuevo);
        flpEdicion.Controls.Add(btnGuardar);
        flpEdicion.Controls.Add(btnEstado);

        dgvCategorias.Dock = DockStyle.Fill;
        dgvCategorias.ReadOnly = true;
        dgvCategorias.AllowUserToAddRows = false;
        dgvCategorias.AllowUserToDeleteRows = false;
        dgvCategorias.MultiSelect = false;
        dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCategorias.BackgroundColor = Color.White;
        dgvCategorias.RowHeadersVisible = false;
        dgvCategorias.EnableHeadersVisualStyles = false;
        dgvCategorias.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(82, 88, 94);
        dgvCategorias.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvCategorias.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvCategorias.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        dgvCategorias.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        dgvCategorias.RowTemplate.Height = 38;

        Controls.Add(tlpPrincipal);
        BackColor = Color.FromArgb(241, 243, 245);
        FormBorderStyle = FormBorderStyle.None;
        Name = "FormCategorias";
        Text = "Categorías";
        ClientSize = new Size(1180, 700);
        tlpPrincipal.ResumeLayout(false);
        pnlCabecera.ResumeLayout(false);
        pnlCabecera.PerformLayout();
        tlpNavegacion.ResumeLayout(false);
        flpEdicion.ResumeLayout(false);
        flpEdicion.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCategorias).EndInit();
        ResumeLayout(false);
    }

    // Define la apariencia fija de cada pestaña de navegación.
    private static void ConfigurarBotonNavegacion(Button boton, string texto, int ancho)
    {
        boton.Text = texto;
        boton.Size = new Size(ancho, 39);
        boton.Margin = new Padding(0, 2, 6, 2);
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.BackColor = Color.FromArgb(235, 237, 240);
        boton.ForeColor = Color.FromArgb(55, 59, 64);
        boton.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
    }

    // Aplica el estilo común a las acciones del editor de categorías.
    private static void ConfigurarBotonAccion(Button boton, string texto, Color color)
    {
        boton.Text = texto;
        boton.Size = new Size(120, 38);
        boton.Margin = new Padding(6, 8, 6, 0);
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.BackColor = color;
        boton.ForeColor = Color.White;
        boton.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
    }
}
