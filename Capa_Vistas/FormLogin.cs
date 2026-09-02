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

                // Por ahora mostramos los datos recuperados
                // para comprobar que todo el login funciona.
                //
                // Más adelante este bloque abrirá FormPrincipal.
                MessageBox.Show(
                    $"Inicio de sesión correcto.\n\n" +
                    $"Usuario: {SesionActual.Nombre} {SesionActual.Apellido}\n" +
                    $"Perfil: {SesionActual.Perfil}\n" +
                    $"Sucursal: {SesionActual.Sucursal}",
                    "Hierro y Forja",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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