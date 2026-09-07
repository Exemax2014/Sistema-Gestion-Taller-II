namespace Capa_Vistas
{
    public partial class FormMensaje : Form
    {
        public FormMensaje(
            string titulo,
            string mensaje,
            string textoAceptar = "Aceptar",
            bool mostrarCancelar = false)
        {
            InitializeComponent();

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