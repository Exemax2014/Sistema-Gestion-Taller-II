#nullable disable

namespace Capa_Vistas;

partial class FormMarcas
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
    private TableLayoutPanel tlpEdicion = null!;
    private TextBox txtNombre = null!;
    private Label lblCategorias = null!;
    private FlowLayoutPanel flpCategorias = null!;
    private FlowLayoutPanel flpAcciones = null!;
    private Button btnNuevo = null!;
    private Button btnGuardar = null!;
    private Button btnEstado = null!;
    private DataGridView dgvMarcas = null!;

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
        tlpEdicion = new TableLayoutPanel();
        txtNombre = new TextBox();
        lblCategorias = new Label();
        flpCategorias = new FlowLayoutPanel();
        flpAcciones = new FlowLayoutPanel();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnEstado = new Button();
        dgvMarcas = new DataGridView();
        tlpPrincipal.SuspendLayout();
        pnlCabecera.SuspendLayout();
        tlpNavegacion.SuspendLayout();
        tlpEdicion.SuspendLayout();
        flpAcciones.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMarcas).BeginInit();
        SuspendLayout();

        tlpPrincipal.BackColor = Color.FromArgb(241, 243, 245);
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPrincipal.RowCount = 4;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 215F));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpPrincipal.Padding = new Padding(32, 20, 32, 24);
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
        tlpPrincipal.Controls.Add(tlpNavegacion, 0, 1);
        tlpPrincipal.Controls.Add(tlpEdicion, 0, 2);
        tlpPrincipal.Controls.Add(dgvMarcas, 0, 3);

        pnlCabecera.Dock = DockStyle.Fill;
        pnlCabecera.Controls.Add(lblTitulo);
        pnlCabecera.Controls.Add(lblSubtitulo);
        pnlCabecera.Controls.Add(pnlLineaTitulo);
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(45, 49, 54);
        lblTitulo.Location = new Point(0, 2);
        lblTitulo.Text = "Marcas";
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
        lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
        lblSubtitulo.Location = new Point(2, 54);
        lblSubtitulo.Text = "Administración de marcas y sus categorías.";
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

        tlpEdicion.BackColor = Color.White;
        tlpEdicion.ColumnCount = 2;
        tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        tlpEdicion.RowCount = 2;
        tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpEdicion.Dock = DockStyle.Fill;
        tlpEdicion.Padding = new Padding(10);
        txtNombre.MaxLength = 100;
        txtNombre.PlaceholderText = "Nombre de marca";
        txtNombre.Font = new Font("Segoe UI", 9.5F);
        txtNombre.Dock = DockStyle.Fill;
        txtNombre.Margin = new Padding(4, 3, 16, 3);
        lblCategorias.Text = "Categorías activas asociadas";
        lblCategorias.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblCategorias.ForeColor = Color.FromArgb(70, 75, 80);
        lblCategorias.Dock = DockStyle.Fill;
        lblCategorias.TextAlign = ContentAlignment.MiddleLeft;
        flpCategorias.AutoScroll = true;
        flpCategorias.Dock = DockStyle.Fill;
        flpCategorias.FlowDirection = FlowDirection.LeftToRight;
        flpCategorias.WrapContents = true;
        flpCategorias.Margin = Padding.Empty;
        tlpEdicion.Controls.Add(txtNombre, 0, 0);
        tlpEdicion.Controls.Add(lblCategorias, 1, 0);
        tlpEdicion.Controls.Add(flpCategorias, 1, 1);
        flpAcciones.Dock = DockStyle.Fill;
        flpAcciones.FlowDirection = FlowDirection.LeftToRight;
        flpAcciones.WrapContents = true;
        ConfigurarBotonAccion(btnNuevo, "+ Nueva", Color.FromArgb(82, 88, 94));
        ConfigurarBotonAccion(btnGuardar, "Guardar", Color.FromArgb(190, 137, 45));
        ConfigurarBotonAccion(btnEstado, "Dar de baja", Color.FromArgb(165, 55, 55));
        flpAcciones.Controls.Add(btnNuevo);
        flpAcciones.Controls.Add(btnGuardar);
        flpAcciones.Controls.Add(btnEstado);
        tlpEdicion.Controls.Add(flpAcciones, 0, 1);

        dgvMarcas.Dock = DockStyle.Fill;
        dgvMarcas.ReadOnly = true;
        dgvMarcas.AllowUserToAddRows = false;
        dgvMarcas.AllowUserToDeleteRows = false;
        dgvMarcas.MultiSelect = false;
        dgvMarcas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMarcas.BackgroundColor = Color.White;
        dgvMarcas.RowHeadersVisible = false;
        dgvMarcas.EnableHeadersVisualStyles = false;
        dgvMarcas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(82, 88, 94);
        dgvMarcas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvMarcas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvMarcas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        dgvMarcas.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        dgvMarcas.RowTemplate.Height = 38;

        Controls.Add(tlpPrincipal);
        BackColor = Color.FromArgb(241, 243, 245);
        FormBorderStyle = FormBorderStyle.None;
        Name = "FormMarcas";
        Text = "Marcas";
        ClientSize = new Size(1180, 700);
        tlpPrincipal.ResumeLayout(false);
        pnlCabecera.ResumeLayout(false);
        pnlCabecera.PerformLayout();
        tlpNavegacion.ResumeLayout(false);
        tlpEdicion.ResumeLayout(false);
        tlpEdicion.PerformLayout();
        flpAcciones.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvMarcas).EndInit();
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

    // Aplica el estilo común a las acciones del editor de marcas.
    private static void ConfigurarBotonAccion(Button boton, string texto, Color color)
    {
        boton.Text = texto;
        boton.Size = new Size(120, 38);
        boton.Margin = new Padding(4);
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.BackColor = color;
        boton.ForeColor = Color.White;
        boton.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
    }
}
