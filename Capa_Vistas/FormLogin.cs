using Capa_Logica;
using System.Runtime.InteropServices;

namespace Capa_Vistas
{
    public partial class FormLogin : Form
    {
        private readonly UsuarioLogica usuarioLogica;


        private const int WM_NCLBUTTONDOWN =
            0x00A1;

        private const int HTCAPTION =
            0x0002;


        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();


        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(
            IntPtr hWnd,
            int Msg,
            IntPtr wParam,
            IntPtr lParam
        );


        public FormLogin()
        {
            InitializeComponent();


            usuarioLogica =
                new UsuarioLogica();


            ConfigurarEventos();


            AcceptButton =
                btnIngresar;
        }


        // =========================================================
        // EVENTOS
        // =========================================================

        private void ConfigurarEventos()
        {
            btnMinimizar.Click +=
                BtnMinimizar_Click;


            btnCerrar.Click +=
                BtnCerrar_Click;


            pnlIzquierda.MouseDown +=
                MoverVentana;


            pnlDerecha.MouseDown +=
                MoverVentana;


            picLogoCompleto.MouseDown +=
                MoverVentana;
        }


        // =========================================================
        // MINIMIZAR
        // =========================================================

        private void BtnMinimizar_Click(
            object? sender,
            EventArgs e)
        {
            WindowState =
                FormWindowState.Minimized;
        }


        // =========================================================
        // CERRAR / SALIR
        // =========================================================

        private void BtnCerrar_Click(
            object? sender,
            EventArgs e)
        {
            ConfirmarSalida();
        }


        private void ConfirmarSalida()
        {
            using FormMensaje mensaje =
                new FormMensaje(
                    "Estás por salir del sistema",
                    "¿Deseás cerrar la aplicación?",
                    "Sí, salir",
                    true
                );


            DialogResult resultado =
                mensaje.ShowDialog(this);


            if (resultado !=
                DialogResult.OK)
            {
                return;
            }


            Application.Exit();
        }


        // =========================================================
        // MOVER VENTANA
        // =========================================================

        private void MoverVentana(
            object? sender,
            MouseEventArgs e)
        {
            if (e.Button !=
                MouseButtons.Left)
            {
                return;
            }


            ReleaseCapture();


            SendMessage(
                Handle,
                WM_NCLBUTTONDOWN,
                new IntPtr(HTCAPTION),
                IntPtr.Zero
            );
        }


        // =========================================================
        // LOGIN
        // =========================================================

        private void btnIngresar_Click(
            object? sender,
            EventArgs e)
        {
            try
            {
                bool loginCorrecto =
                    usuarioLogica.IniciarSesion(
                        txtUsuario.Text,
                        txtContrasena.Text,
                        out string mensaje
                    );


                if (!loginCorrecto)
                {
                    using FormMensaje aviso =
                        new FormMensaje(
                            "No se pudo iniciar sesión",
                            mensaje
                        );


                    aviso.ShowDialog(this);


                    txtContrasena.Clear();

                    txtContrasena.Focus();

                    return;
                }


                Hide();


                using (
                    FormPrincipal formPrincipal =
                        new FormPrincipal())
                {
                    formPrincipal.ShowDialog();
                }


                if (!SesionActual.SesionIniciada)
                {
                    txtUsuario.Clear();

                    txtContrasena.Clear();


                    Show();


                    txtUsuario.Focus();
                }
                else
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                using FormMensaje aviso =
                    new FormMensaje(
                        "Error al iniciar sesión",
                        ex.Message
                    );


                aviso.ShowDialog(this);
            }
        }
    }
}