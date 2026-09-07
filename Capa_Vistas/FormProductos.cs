using System;
using System.Windows.Forms;
using Capa_Datos;
using Capa_Logica;

namespace Capa_Vistas
{
    // =====================================================================
    // Formulario: FormProductos
    //
    // Responsabilidad:
    // Por ahora solo permite listar el catálogo activo y dar de alta
    // productos nuevos. Baja y modificación no están incluidas todavía
    // (no las pide ningún perfil por ahora salvo Administrador).
    //
    // Permisos:
    // El botón Alta se habilita solo según
    // SesionActual.TienePermiso("PRODUCTOS_ALTA").
    // No se hardcodea ningún nombre de perfil acá.
    //
    // Se carga dentro de FormPrincipal -> pnlContenido y no repite
    // cabecera, menú lateral ni cierre de sesión.
    // =====================================================================
    public partial class FormProductos : Form
    {
        private readonly ProductoLogica productoLogica = new ProductoLogica();

        public FormProductos()
        {
            InitializeComponent();
            ConfigurarPermisos();
            CargarCategorias();
            CargarGrilla();
        }

        private void ConfigurarPermisos()
        {
            bool puedeAlta = SesionActual.TienePermiso("PRODUCTOS_ALTA");

            btnAlta.Enabled = puedeAlta;
            cmbCategoria.Enabled = puedeAlta;
            txtCodigoBarra.Enabled = puedeAlta;
            txtNombre.Enabled = puedeAlta;
            txtDescripcion.Enabled = puedeAlta;
            txtPrecioCosto.Enabled = puedeAlta;
            txtPorcentajeGanancia.Enabled = puedeAlta;
        }

        private void CargarCategorias()
        {
            cmbCategoria.DataSource = productoLogica.ObtenerCategorias();
            cmbCategoria.DisplayMember = nameof(CategoriaInfo.Nombre);
            cmbCategoria.ValueMember = nameof(CategoriaInfo.IdCategoria);
        }

        private void CargarGrilla()
        {
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = productoLogica.ObtenerActivos();
        }

        private void BtnAlta_Click(object? sender, EventArgs e)
        {
            int? idCategoria = cmbCategoria.SelectedValue is int valor ? valor : null;

            string? error = productoLogica.ValidarAlta(
                idCategoria,
                txtNombre.Text,
                txtPrecioCosto.Text,
                txtPorcentajeGanancia.Text);

            if (error != null)
            {
                MessageBox.Show(error, "Alta de producto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                productoLogica.Alta(
                    idCategoria!.Value,
                    txtCodigoBarra.Text,
                    txtNombre.Text,
                    txtDescripcion.Text,
                    decimal.Parse(txtPrecioCosto.Text),
                    decimal.Parse(txtPorcentajeGanancia.Text));

                LimpiarFormulario();
                CargarGrilla();

                MessageBox.Show(
                    "Producto agregado correctamente.",
                    "Alta de producto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo agregar el producto: " + ex.Message,
                    "Alta de producto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            txtCodigoBarra.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecioCosto.Clear();
            txtPorcentajeGanancia.Clear();
        }
    }
}