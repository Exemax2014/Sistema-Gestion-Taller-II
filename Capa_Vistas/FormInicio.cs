using Capa_Logica;

namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormInicio
    //
    // Vista principal del sistema.
    //
    // El Designer solamente maneja el diseño visual.
    // Los eventos y comportamiento se configuran acá.
    // ============================================================

    public partial class FormInicio : Form
    {
        private readonly FormPrincipal formPrincipal;


        public FormInicio(
            FormPrincipal formPrincipal)
        {
            InitializeComponent();


            this.formPrincipal =
                formPrincipal;


            ConfigurarEventos();

            PrepararVistaInicial();
        }


        // ========================================================
        // EVENTOS
        // ========================================================

        private void ConfigurarEventos()
        {
            btnNuevaVenta.Click +=
                BtnNuevaVenta_Click;


            btnProductos.Click +=
                BtnProductos_Click;


            btnClientes.Click +=
                BtnClientes_Click;
        }


        // ========================================================
        // VISTA INICIAL
        // ========================================================

        private void PrepararVistaInicial()
        {
            lblBienvenida.Text =
                $"Bienvenido, {SesionActual.Nombre}";


            lblPerfilSucursal.Text =
                ObtenerDescripcionSesion();


            // Valores visuales.
            // Más adelante vendrán desde Capa_Logica.

            lblVentasHoyValor.Text =
                "0";


            lblIngresosHoyValor.Text =
                "$ 0,00";


            lblStockBajoValor.Text =
                "0";


            lblProductosValor.Text =
                "0";
        }


        // ========================================================
        // DESCRIPCIÓN DE SESIÓN
        // ========================================================

        private string ObtenerDescripcionSesion()
        {
            string perfil =
                string.IsNullOrWhiteSpace(
                    SesionActual.Perfil
                )
                    ? "Usuario"
                    : SesionActual.Perfil;


            string sucursal =
                SesionActual.IdSucursalOperativa.HasValue
                    ? SesionActual.SucursalOperativa
                    : "Todas las sucursales";


            return $"{perfil} · {sucursal}";
        }


        // ========================================================
        // NUEVA VENTA
        // ========================================================

        private void BtnNuevaVenta_Click(
            object? sender,
            EventArgs e)
        {
            FormVentas ventas =
                new FormVentas();


            formPrincipal.AbrirFormularioEnPanel(
                ventas,
                formPrincipal.BotonVentas
            );
        }


        // ========================================================
        // PRODUCTOS
        // ========================================================

        private void BtnProductos_Click(
            object? sender,
            EventArgs e)
        {
            FormProductos productos =
                new FormProductos(
                    formPrincipal
                );


            formPrincipal.AbrirFormularioEnPanel(
                productos,
                formPrincipal.BotonProductos
            );
        }


        // ========================================================
        // CLIENTES
        // ========================================================

        private void BtnClientes_Click(
            object? sender,
            EventArgs e)
        {
            FormClientes clientes =
                new FormClientes(
                    formPrincipal
                );


            formPrincipal.AbrirFormularioEnPanel(
                clientes,
                formPrincipal.BotonClientes
            );
        }
    }
}