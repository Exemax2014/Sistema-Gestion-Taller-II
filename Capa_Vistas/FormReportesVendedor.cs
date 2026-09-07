using System;
using System.Windows.Forms;
using Capa_Logica;

namespace Capa_Vistas
{
    // =====================================================================
    // Formulario: FormReportesVendedor
    //
    // Responsabilidad:
    // Muestra el reporte de ventas propio del vendedor autenticado,
    // pensado para armar el cierre de caja del día.
    //
    // La estructura visual se encuentra en:
    // FormReportesVendedor.Designer.cs
    //
    // Este archivo contiene únicamente:
    // - carga de datos;
    // - interacción con Capa_Logica.
    //
    // Regla crítica de seguridad:
    // El filtro por vendedor NUNCA debe depender de un valor editable
    // por el usuario en la pantalla. Siempre debe usarse el id del
    // usuario autenticado, tomado de SesionActual, tanto acá como del
    // lado del procedimiento almacenado. Este vendedor no debe poder
    // ver, bajo ninguna circunstancia, las ventas de otro vendedor.
    //
    // Se carga dentro de FormPrincipal -> pnlContenido y no repite
    // cabecera, menú lateral ni cierre de sesión (ya los pone FormPrincipal).
    // =====================================================================
    public partial class FormReportesVendedor : Form
    {
        public FormReportesVendedor()
        {
            InitializeComponent();

            // Rango de fechas por defecto: el día de hoy.
            dtpDesde.Value = DateTime.Today;
            dtpHasta.Value = DateTime.Today;
        }

        private void BtnMisVentas_Click(object? sender, EventArgs e)
        {
            // TODO: reemplazar este bloque cuando exista VentaLogica.
            //
            // Uso previsto (ajustar el nombre exacto de la propiedad de
            // SesionActual que identifica al usuario logueado, ej. IdUsuario):
            //
            // var ventas = VentaLogica.ObtenerVentasPorVendedor(
            //     SesionActual.IdUsuario,
            //     dtpDesde.Value,
            //     dtpHasta.Value);
            //
            // dgvMisVentas.DataSource = ventas;
            //
            // El procedimiento almacenado detrás de ObtenerVentasPorVendedor
            // debe recibir el id de vendedor como parámetro obligatorio,
            // nunca construir el filtro a partir de texto libre.

            MessageBox.Show(
                "Todavía no existe el módulo de Ventas para traer los datos reales.\n" +
                "Esta pantalla ya está lista para conectarse en cuanto exista VentaLogica.",
                "Mis ventas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
