namespace Capa_Vistas
{
    public partial class FormMensaje : Form
    {
        private FormPrincipal? principalActual;
        private FormOverlay? overlayActual;

        public FormMensaje(
            string titulo,
            string mensaje,
            string textoAceptar = "Aceptar",
            bool mostrarCancelar = false)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            lblTitulo.Text =
                titulo;

            lblMensaje.Text =
                mensaje;

            btnAceptar.Text =
                textoAceptar;

            btnCancelar.Visible =
                mostrarCancelar;

            ConfigurarBotones(
                mostrarCancelar
            );

            btnAceptar.Click +=
                BtnAceptar_Click;

            btnCancelar.Click +=
                BtnCancelar_Click;
        }

        // Usa como owner la instancia principal real, oscurece el fondo y garantiza retirar el overlay.
        public new DialogResult ShowDialog(IWin32Window owner)
        {
            principalActual = ResolverFormularioPrincipal(owner);
            if (principalActual is null)
            {
                StartPosition = FormStartPosition.CenterParent;
                return base.ShowDialog(owner);
            }

            StartPosition = FormStartPosition.Manual;
            try
            {
                overlayActual = new FormOverlay(principalActual.Bounds);
                overlayActual.Show(principalActual);
                overlayActual.BringToFront();
                Shown += CentrarDialogoEnPrincipal;
                return base.ShowDialog(principalActual);
            }
            finally
            {
                Shown -= CentrarDialogoEnPrincipal;
                FormOverlay? overlay = overlayActual;
                overlayActual = null;
                principalActual = null;
                if (overlay != null)
                {
                    try
                    {
                        if (!overlay.IsDisposed && overlay.Visible) overlay.Close();
                    }
                    finally
                    {
                        overlay.Dispose();
                    }
                }
            }
        }

        // Centra el diálogo con su tamaño final usando los límites de pantalla completos del principal.
        private void CentrarDialogoEnPrincipal(object? sender, EventArgs e)
        {
            Shown -= CentrarDialogoEnPrincipal;
            FormPrincipal? principal = principalActual;
            if (principal == null || principal.IsDisposed || !principal.IsHandleCreated || !principal.Visible) return;

            Rectangle limites = principal.Bounds;
            int x = limites.Left + (limites.Width - Width) / 2;
            int y = limites.Top + (limites.Height - Height) / 2;
            Location = new Point(x, y);
            BringToFront();
            Activate();
        }

        // Resuelve el FormPrincipal visible desde un owner embebido y deja intacto el flujo anterior a Login.
        private static FormPrincipal? ResolverFormularioPrincipal(IWin32Window owner)
        {
            if (owner is FormPrincipal principalDirecto)
                return EsPrincipalVisible(principalDirecto) ? principalDirecto : null;

            if (owner is Control control)
            {
                for (Control? actual = control; actual is not null; actual = actual.Parent)
                    if (actual is FormPrincipal principal && EsPrincipalVisible(principal)) return principal;

                FormPrincipal? formularioContenedor = control.FindForm() as FormPrincipal;
                if (formularioContenedor != null && EsPrincipalVisible(formularioContenedor)) return formularioContenedor;
            }

            return Application.OpenForms.OfType<FormPrincipal>().FirstOrDefault(EsPrincipalVisible);
        }

        // Evita asociar el overlay o el centrado a formularios principales que estén cerrándose u ocultos.
        private static bool EsPrincipalVisible(FormPrincipal principal) =>
            !principal.IsDisposed && principal.IsHandleCreated && principal.Visible;


        private void ConfigurarBotones(
            bool mostrarCancelar)
        {
            if (mostrarCancelar)
            {
                btnCancelar.Location =
                    new Point(70, 175);

                btnAceptar.Location =
                    new Point(250, 175);

                return;
            }

            btnAceptar.Location =
                new Point(160, 175);
        }


        private void BtnAceptar_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult =
                DialogResult.OK;

            Close();
        }


        private void BtnCancelar_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult =
                DialogResult.Cancel;

            Close();
        }
    }
}
