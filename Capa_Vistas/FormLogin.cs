using Capa_Logica;

namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormLogin
    //
    // Responsabilidad:
    // Recibir las credenciales ingresadas por el usuario y enviarlas
    // a Capa_Logica para realizar la autenticación.
    //
    // Este formulario no consulta directamente SQL Server.
    // ============================================================
    public partial class FormLogin : Form
    {
        // Objeto encargado de manejar la lógica de autenticación.
        private readonly UsuarioLogica usuarioLogica;

        public FormLogin()
        {
            InitializeComponent();

            // Crear la instancia de la lógica de usuarios.
            usuarioLogica = new UsuarioLogica();
        }


        // ========================================================
        // Evento: btnIngresar_Click
        //
        // Se ejecuta cuando el usuario presiona INICIAR SESIÓN.
        // Envía usuario y contraseña a Capa_Logica.
        // ========================================================
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar a la capa lógica que valide
                // las credenciales ingresadas.
                bool loginCorrecto = usuarioLogica.IniciarSesion(
                    txtUsuario.Text,
                    txtContrasena.Text,
                    out string mensaje
                );

                // Si las credenciales no son válidas,
                // informar el motivo y permitir un nuevo intento.
                if (!loginCorrecto)
                {
                    MessageBox.Show(
                        mensaje,
                        "Inicio de sesión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    txtContrasena.Clear();
                    txtContrasena.Focus();

                    return;
                }

                // ============================================================
                // LOGIN CORRECTO
                //
                // Ocultar el formulario de login y abrir FormPrincipal.
                // FormPrincipal utilizará los datos guardados en SesionActual.
                //
                // Si el usuario utiliza "Cerrar sesión", FormPrincipal cerrará
                // la sesión y se volverá a mostrar este formulario.
                //
                // Si FormPrincipal se cierra directamente con la X, se cierra
                // también el login y finaliza la aplicación.
                // ============================================================

                // Ocultar el login mientras el sistema principal está abierto.
                Hide();

                // Crear y abrir el formulario principal de manera modal.
                // Cuando FormPrincipal se cierre, la ejecución continúa aquí.
                using (FormPrincipal formPrincipal = new FormPrincipal())
                {
                    formPrincipal.ShowDialog();
                }


                // ------------------------------------------------------------
                // Si FormPrincipal cerró la sesión mediante el botón
                // "Cerrar sesión", volvemos a mostrar el login.
                // ------------------------------------------------------------
                if (!SesionActual.SesionIniciada)
                {
                    // Limpiar las credenciales anteriores.
                    txtUsuario.Clear();
                    txtContrasena.Clear();

                    // Mostrar nuevamente la ventana de login.
                    Show();

                    txtUsuario.Focus();
                }
                else
                {
                    // Si FormPrincipal se cerró sin cerrar sesión
                    // (por ejemplo utilizando la X), finalizar la aplicación.
                    Close();
                }
            }
            catch (Exception ex)
            {
                // Mostrar errores técnicos relacionados con
                // conexión, configuración o acceso a SQL Server.
                MessageBox.Show(
                    $"No se pudo iniciar sesión.\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // Eventos actualmente asociados a los labels desde
        // el diseñador de Windows Forms.
        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }
    }
}