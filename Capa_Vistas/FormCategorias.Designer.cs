#nullable disable

namespace Capa_Vistas;

partial class FormCategorias
{
    private TableLayoutPanel tlpPrincipal = null!;
    private TableLayoutPanel tlpNavegacion = null!;
    private Label lblTitulo = null!;
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
        tlpPrincipal = new TableLayoutPanel(); tlpNavegacion = new TableLayoutPanel(); lblTitulo = new Label();
        btnProductos = new Button(); btnCategorias = new Button(); btnMarcas = new Button(); flpEdicion = new FlowLayoutPanel();
        txtNombre = new TextBox(); txtDescripcion = new TextBox(); btnNuevo = new Button(); btnGuardar = new Button(); btnEstado = new Button(); dgvCategorias = new DataGridView();
        tlpPrincipal.SuspendLayout(); tlpNavegacion.SuspendLayout(); flpEdicion.SuspendLayout(); ((System.ComponentModel.ISupportInitialize)dgvCategorias).BeginInit(); SuspendLayout();
        tlpPrincipal.BackColor = Color.FromArgb(241,243,245); tlpPrincipal.ColumnCount=1; tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
        tlpPrincipal.RowCount=3; tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute,78)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute,94)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent,100));
        tlpPrincipal.Padding=new Padding(28,18,28,24); tlpPrincipal.Dock=DockStyle.Fill; tlpPrincipal.Controls.Add(tlpNavegacion,0,0); tlpPrincipal.Controls.Add(flpEdicion,0,1); tlpPrincipal.Controls.Add(dgvCategorias,0,2);
        tlpNavegacion.ColumnCount=4; tlpNavegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100)); tlpNavegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,120)); tlpNavegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,105)); tlpNavegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,115)); tlpNavegacion.Dock=DockStyle.Fill;
        lblTitulo.Text="Categorías"; lblTitulo.Font=new Font("Segoe UI",21,FontStyle.Bold); lblTitulo.ForeColor=Color.FromArgb(45,49,54); lblTitulo.Dock=DockStyle.Fill; lblTitulo.TextAlign=ContentAlignment.MiddleLeft;
        EstiloNavegacion(btnProductos,"Productos"); EstiloNavegacion(btnCategorias,"Categorías"); EstiloNavegacion(btnMarcas,"Marcas");
        tlpNavegacion.Controls.Add(lblTitulo,0,0); tlpNavegacion.Controls.Add(btnProductos,1,0); tlpNavegacion.Controls.Add(btnCategorias,2,0); tlpNavegacion.Controls.Add(btnMarcas,3,0);
        flpEdicion.BackColor=Color.White; flpEdicion.Dock=DockStyle.Fill; flpEdicion.Padding=new Padding(10); flpEdicion.WrapContents=true; flpEdicion.AutoScroll=true;
        txtNombre.MaxLength=100; txtNombre.Width=230; txtNombre.PlaceholderText="Nombre de categoría"; txtNombre.Font=new Font("Segoe UI",9.5F);
        txtDescripcion.MaxLength=200; txtDescripcion.Width=280; txtDescripcion.PlaceholderText="Descripción (opcional)"; txtDescripcion.Font=new Font("Segoe UI",9.5F);
        EstiloAccion(btnNuevo,"+ Nueva",Color.FromArgb(82,88,94)); EstiloAccion(btnGuardar,"Guardar",Color.FromArgb(190,137,45)); EstiloAccion(btnEstado,"Dar de baja",Color.FromArgb(165,55,55));
        flpEdicion.Controls.Add(txtNombre); flpEdicion.Controls.Add(txtDescripcion); flpEdicion.Controls.Add(btnNuevo); flpEdicion.Controls.Add(btnGuardar); flpEdicion.Controls.Add(btnEstado);
        dgvCategorias.Dock=DockStyle.Fill; dgvCategorias.ReadOnly=true; dgvCategorias.AllowUserToAddRows=false; dgvCategorias.AllowUserToDeleteRows=false; dgvCategorias.MultiSelect=false; dgvCategorias.SelectionMode=DataGridViewSelectionMode.FullRowSelect; dgvCategorias.BackgroundColor=Color.White; dgvCategorias.RowHeadersVisible=false; dgvCategorias.EnableHeadersVisualStyles=false; dgvCategorias.ColumnHeadersDefaultCellStyle.BackColor=Color.FromArgb(82,88,94); dgvCategorias.ColumnHeadersDefaultCellStyle.ForeColor=Color.White; dgvCategorias.ColumnHeadersDefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleCenter; dgvCategorias.ColumnHeadersDefaultCellStyle.Font=new Font("Segoe UI",8.5F,FontStyle.Bold); dgvCategorias.DefaultCellStyle.Font=new Font("Segoe UI",9F); dgvCategorias.RowTemplate.Height=38;
        Controls.Add(tlpPrincipal); BackColor=Color.FromArgb(241,243,245); FormBorderStyle=FormBorderStyle.None; Name="FormCategorias"; Text="Categorías"; ClientSize=new Size(1180,700);
        tlpPrincipal.ResumeLayout(false); tlpNavegacion.ResumeLayout(false); flpEdicion.ResumeLayout(false); flpEdicion.PerformLayout(); ((System.ComponentModel.ISupportInitialize)dgvCategorias).EndInit(); ResumeLayout(false);
    }

    private static void EstiloNavegacion(Button b,string texto) { b.Text=texto; b.Dock=DockStyle.Fill; b.Margin=new Padding(4,12,4,12); b.FlatStyle=FlatStyle.Flat; b.BackColor=Color.FromArgb(82,88,94); b.ForeColor=Color.White; b.Font=new Font("Segoe UI",8.5F,FontStyle.Bold); }
    private static void EstiloAccion(Button b,string texto,Color color) { b.Text=texto; b.Size=new Size(120,38); b.Margin=new Padding(6,8,6,0); b.FlatStyle=FlatStyle.Flat; b.FlatAppearance.BorderSize=0; b.BackColor=color; b.ForeColor=Color.White; b.Font=new Font("Segoe UI",8.5F,FontStyle.Bold); }
}
