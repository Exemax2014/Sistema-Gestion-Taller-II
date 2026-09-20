#nullable disable

namespace Capa_Vistas;

partial class FormMarcas
{
    private TableLayoutPanel tlpPrincipal = null!;
    private TableLayoutPanel tlpNavegacion = null!;
    private Label lblTitulo = null!;
    private Button btnProductos = null!;
    private Button btnCategorias = null!;
    private Button btnMarcas = null!;
    private TableLayoutPanel tlpEdicion = null!;
    private TextBox txtNombre = null!;
    private Label lblCategorias = null!;
    private CheckedListBox clbCategorias = null!;
    private FlowLayoutPanel flpAcciones = null!;
    private Button btnNuevo = null!;
    private Button btnGuardar = null!;
    private Button btnEstado = null!;
    private DataGridView dgvMarcas = null!;

    private void InitializeComponent()
    {
        tlpPrincipal=new TableLayoutPanel(); tlpNavegacion=new TableLayoutPanel(); lblTitulo=new Label(); btnProductos=new Button(); btnCategorias=new Button(); btnMarcas=new Button();
        tlpEdicion=new TableLayoutPanel(); txtNombre=new TextBox(); lblCategorias=new Label(); clbCategorias=new CheckedListBox(); flpAcciones=new FlowLayoutPanel(); btnNuevo=new Button(); btnGuardar=new Button(); btnEstado=new Button(); dgvMarcas=new DataGridView();
        tlpPrincipal.SuspendLayout(); tlpNavegacion.SuspendLayout(); tlpEdicion.SuspendLayout(); flpAcciones.SuspendLayout(); ((System.ComponentModel.ISupportInitialize)dgvMarcas).BeginInit(); SuspendLayout();
        tlpPrincipal.BackColor=Color.FromArgb(241,243,245); tlpPrincipal.ColumnCount=1; tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100)); tlpPrincipal.RowCount=3;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute,78)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute,215)); tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent,100)); tlpPrincipal.Padding=new Padding(28,18,28,24); tlpPrincipal.Dock=DockStyle.Fill;
        tlpPrincipal.Controls.Add(tlpNavegacion,0,0); tlpPrincipal.Controls.Add(tlpEdicion,0,1); tlpPrincipal.Controls.Add(dgvMarcas,0,2);
        tlpNavegacion.ColumnCount=4; tlpNavegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100)); tlpNavegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,120)); tlpNavegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,105)); tlpNavegacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,115)); tlpNavegacion.Dock=DockStyle.Fill;
        lblTitulo.Text="Marcas"; lblTitulo.Font=new Font("Segoe UI",21,FontStyle.Bold); lblTitulo.ForeColor=Color.FromArgb(45,49,54); lblTitulo.Dock=DockStyle.Fill; lblTitulo.TextAlign=ContentAlignment.MiddleLeft;
        EstiloNavegacion(btnProductos,"Productos"); EstiloNavegacion(btnCategorias,"Categorías"); EstiloNavegacion(btnMarcas,"Marcas"); tlpNavegacion.Controls.Add(lblTitulo,0,0); tlpNavegacion.Controls.Add(btnProductos,1,0); tlpNavegacion.Controls.Add(btnCategorias,2,0); tlpNavegacion.Controls.Add(btnMarcas,3,0);
        tlpEdicion.BackColor=Color.White; tlpEdicion.ColumnCount=2; tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,35)); tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,65)); tlpEdicion.RowCount=2; tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute,36)); tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Percent,100)); tlpEdicion.Dock=DockStyle.Fill; tlpEdicion.Padding=new Padding(10);
        txtNombre.MaxLength=100; txtNombre.PlaceholderText="Nombre de marca"; txtNombre.Font=new Font("Segoe UI",9.5F); txtNombre.Dock=DockStyle.Fill; txtNombre.Margin=new Padding(4,3,16,3);
        lblCategorias.Text="Categorías activas asociadas"; lblCategorias.Font=new Font("Segoe UI",8.5F,FontStyle.Bold); lblCategorias.ForeColor=Color.FromArgb(70,75,80); lblCategorias.Dock=DockStyle.Fill; lblCategorias.TextAlign=ContentAlignment.MiddleLeft;
        clbCategorias.CheckOnClick=true; clbCategorias.Dock=DockStyle.Fill; clbCategorias.Font=new Font("Segoe UI",9F); clbCategorias.IntegralHeight=false;
        tlpEdicion.Controls.Add(txtNombre,0,0); tlpEdicion.Controls.Add(lblCategorias,1,0); tlpEdicion.Controls.Add(clbCategorias,1,1);
        flpAcciones.Dock=DockStyle.Fill; flpAcciones.FlowDirection=FlowDirection.TopDown; flpAcciones.WrapContents=false;
        EstiloAccion(btnNuevo,"+ Nueva",Color.FromArgb(82,88,94)); EstiloAccion(btnGuardar,"Guardar",Color.FromArgb(190,137,45)); EstiloAccion(btnEstado,"Dar de baja",Color.FromArgb(165,55,55)); flpAcciones.Controls.Add(btnNuevo); flpAcciones.Controls.Add(btnGuardar); flpAcciones.Controls.Add(btnEstado); tlpEdicion.Controls.Add(flpAcciones,0,1);
        dgvMarcas.Dock=DockStyle.Fill; dgvMarcas.ReadOnly=true; dgvMarcas.AllowUserToAddRows=false; dgvMarcas.AllowUserToDeleteRows=false; dgvMarcas.MultiSelect=false; dgvMarcas.SelectionMode=DataGridViewSelectionMode.FullRowSelect; dgvMarcas.BackgroundColor=Color.White; dgvMarcas.RowHeadersVisible=false; dgvMarcas.EnableHeadersVisualStyles=false; dgvMarcas.ColumnHeadersDefaultCellStyle.BackColor=Color.FromArgb(82,88,94); dgvMarcas.ColumnHeadersDefaultCellStyle.ForeColor=Color.White; dgvMarcas.ColumnHeadersDefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleCenter; dgvMarcas.ColumnHeadersDefaultCellStyle.Font=new Font("Segoe UI",8.5F,FontStyle.Bold); dgvMarcas.DefaultCellStyle.Font=new Font("Segoe UI",9F); dgvMarcas.RowTemplate.Height=38;
        Controls.Add(tlpPrincipal); BackColor=Color.FromArgb(241,243,245); FormBorderStyle=FormBorderStyle.None; Name="FormMarcas"; Text="Marcas"; ClientSize=new Size(1180,700);
        tlpPrincipal.ResumeLayout(false); tlpNavegacion.ResumeLayout(false); tlpEdicion.ResumeLayout(false); tlpEdicion.PerformLayout(); flpAcciones.ResumeLayout(false); ((System.ComponentModel.ISupportInitialize)dgvMarcas).EndInit(); ResumeLayout(false);
    }

    private static void EstiloNavegacion(Button b,string texto) { b.Text=texto; b.Dock=DockStyle.Fill; b.Margin=new Padding(4,12,4,12); b.FlatStyle=FlatStyle.Flat; b.BackColor=Color.FromArgb(82,88,94); b.ForeColor=Color.White; b.Font=new Font("Segoe UI",8.5F,FontStyle.Bold); }
    private static void EstiloAccion(Button b,string texto,Color color) { b.Text=texto; b.Size=new Size(120,38); b.Margin=new Padding(4,4,4,4); b.FlatStyle=FlatStyle.Flat; b.FlatAppearance.BorderSize=0; b.BackColor=color; b.ForeColor=Color.White; b.Font=new Font("Segoe UI",8.5F,FontStyle.Bold); }
}
