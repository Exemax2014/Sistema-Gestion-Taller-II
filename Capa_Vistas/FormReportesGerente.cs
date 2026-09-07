using System;
using System.Windows.Forms;
using Capa_Logica;

namespace Capa_Vistas
{
    // =====================================================================
    // Formulario: FormReportesGerente
    //
    // Responsabilidad:
    // Muestra los reportes de gestión pensados para toma de decisiones
    // del Gerente: recaudación, productos más vendidos, ventas totales
    // y ventas filtradas por un vendedor puntual.
    //
    // A diferencia de FormReportesVendedor, acá SÍ se puede consultar
    // por cualquier vendedor (el Gerente ve todo el negocio), por eso
    // el id de vendedor sale de txtVendedorId y no de SesionActual.
    //
    // La estructura visual está en FormReportesGerente.Designer.cs.
    // Este archivo contiene únicamente carga de datos e interacción
    // con Capa_Logica.
    //
    // Se carga dentro de FormPrincipal -> pnlContenido y no repite
    // cabecera, menú lateral ni cierre de sesión.
    // =====================================================================
    public partial class FormReportesGerente : Form
    {
        public FormReportesGerente()
        {
            InitializeComponent();

            // Rango de fechas por defecto: el día de hoy.
            dtpDesde.Value = DateTime.Today;
            dtpHasta.Value = DateTime.Today;
        }

        private void BtnRecaudacion_Click(object? sender, EventArgs e)
        {
            // TODO: reemplazar cuando exista VentaLogica.
            // var datos = VentaLogica.ObtenerRecaudacion(dtpDesde.Value, dtpHasta.Value);
            // dgvResultado.DataSource = datos;

            MessageBox.Show(
                "Todavía no existe el módulo de Ventas para calcular la recaudación.",
                "Recaudación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnProductosMasVendidos_Click(object? sender, EventArgs e)
        {
            // TODO: reemplazar cuando exista VentaLogica (o el método
            // correspondiente en ProductoLogica, a definir).
            // var datos = VentaLogica.ObtenerProductosMasVendidos(dtpDesde.Value, dtpHasta.Value);
            // dgvResultado.DataSource = datos;

            MessageBox.Show(
                "Todavía no existe el módulo de Ventas para calcular productos más vendidos.",
                "Productos más vendidos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnVentas_Click(object? sender, EventArgs e)
        {
            // TODO: reemplazar cuando exista VentaLogica.
            // Trae TODAS las ventas del rango, de todos los vendedores.
            // var datos = VentaLogica.ObtenerVentas(dtpDesde.Value, dtpHasta.Value);
            // dgvResultado.DataSource = datos;

            MessageBox.Show(
                "Todavía no existe el módulo de Ventas para traer los datos reales.",
                "Ventas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnBuscarVendedor_Click(object? sender, EventArgs e)
        {
            // TODO: reemplazar cuando exista un método en UsuarioLogica
            // para buscar un usuario/vendedor por id, ej.:
            // var vendedor = UsuarioLogica.BuscarPorId(int.Parse(txtVendedorId.Text));
            // txtVendedorNombre.Text = vendedor?.NombreCompleto ?? "No encontrado";

            MessageBox.Show(
                "Todavía no existe el método de búsqueda de vendedor por id en UsuarioLogica.",
                "Buscar vendedor",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnVentasPorVendedor_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtVendedorId.Text))
            {
                MessageBox.Show(
                    "Buscá primero un vendedor antes de pedir sus ventas.",
                    "Ventas por vendedor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // TODO: reemplazar cuando exista VentaLogica.
            // var idVendedor = int.Parse(txtVendedorId.Text);
            // var datos = VentaLogica.ObtenerVentasPorVendedor(idVendedor, dtpDesde.Value, dtpHasta.Value);
            // dgvResultado.DataSource = datos;

            MessageBox.Show(
                "Todavía no existe el módulo de Ventas para traer los datos reales.",
                "Ventas por vendedor",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
