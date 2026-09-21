namespace Capa_Vistas
{
    // Ventana oscura temporal, propiedad del formulario principal y situada debajo del diálogo modal.
    internal sealed class FormOverlay : Form
    {
        // Crea una capa translúcida sin borde, controles, entrada de teclado ni presencia en Alt+Tab.
        public FormOverlay(Rectangle bounds)
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.Black;
            Opacity = 0.42;
            Bounds = bounds;
            TabStop = false;
        }

        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parametros = base.CreateParams;
                parametros.ExStyle |= 0x00000080 | 0x08000000; // WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE
                return parametros;
            }
        }
    }
}
