using Capa_Logica;

namespace Capa_Vistas
{
    // Permite generar una copia .bak en una carpeta elegida por el usuario, sin exponer configuración sensible.
    public partial class FormBackup : Form
    {
        private readonly FormPrincipal formPrincipal;
        private readonly BackupLogica backupLogica = new();
        private bool ejecutandoBackup;

        public FormBackup(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;

            btnExaminar.Click += BtnExaminar_Click;
            btnGenerarBackup.Click += BtnGenerarBackup_Click;
            Resize += (_, _) => AjustarLayout();
            Load += FormBackup_Load;
            AjustarLayout();
        }

        // Revalida el permiso también desde el formulario y obtiene el catálogo de la conexión configurada.
        private void FormBackup_Load(object? sender, EventArgs e)
        {
            if (!backupLogica.PuedeRealizarBackup())
            {
                btnExaminar.Enabled = btnGenerarBackup.Enabled = false;
                MostrarMensaje("Sin permiso", "No tenés permisos para realizar copias de seguridad.");
                return;
            }

            try
            {
                txtBaseDatos.Text = backupLogica.ObtenerNombreBaseDatos();
            }
            catch (Exception ex)
            {
                btnGenerarBackup.Enabled = false;
                MostrarMensaje("No se pudo leer la configuración", ex.Message);
            }
        }

        // Presenta el selector de carpetas sin aceptar una ruta escrita manualmente.
        private void BtnExaminar_Click(object? sender, EventArgs e)
        {
            using FolderBrowserDialog selector = new()
            {
                Description = "Seleccioná la carpeta donde SQL Server guardará el archivo de copia.",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = true
            };

            if (Directory.Exists(txtCarpetaDestino.Text))
                selector.SelectedPath = txtCarpetaDestino.Text;

            if (selector.ShowDialog(formPrincipal) == DialogResult.OK)
                txtCarpetaDestino.Text = selector.SelectedPath;
        }

        // Evita ejecuciones simultáneas y mantiene la pantalla utilizable durante la operación de SQL Server.
        private async void BtnGenerarBackup_Click(object? sender, EventArgs e)
        {
            if (ejecutandoBackup)
                return;

            if (!backupLogica.PuedeRealizarBackup())
            {
                MostrarMensaje("Sin permiso", "No tenés permisos para realizar copias de seguridad.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCarpetaDestino.Text))
            {
                MostrarMensaje("Carpeta requerida", "Seleccioná una carpeta de destino antes de generar el Back Up.");
                return;
            }

            ejecutandoBackup = true;
            btnGenerarBackup.Enabled = false;
            btnExaminar.Enabled = false;
            try
            {
                string nombreArchivo = await Task.Run(() => backupLogica.GenerarBackup(txtCarpetaDestino.Text));
                MostrarMensaje("Back Up generado", $"Copia de seguridad generada correctamente.\n\n{nombreArchivo}");
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudo generar el Back Up", ex.Message);
            }
            finally
            {
                ejecutandoBackup = false;
                btnGenerarBackup.Enabled = backupLogica.PuedeRealizarBackup();
                btnExaminar.Enabled = backupLogica.PuedeRealizarBackup();
            }
        }

        // Centra la tarjeta y reacomoda el selector en una segunda fila cuando el ancho es reducido.
        private void AjustarLayout()
        {
            const int margenLateral = 32;
            int ancho = Math.Max(300, ClientSize.Width - margenLateral * 2);
            pnlCabecera.SetBounds(margenLateral, 20, ancho, 100);

            int anchoTarjeta = Math.Min(980, ancho);
            int xTarjeta = (ClientSize.Width - anchoTarjeta) / 2;
            int altoTarjeta = anchoTarjeta >= 620 ? 270 : 310;
            pnlTarjeta.SetBounds(xTarjeta, 145, anchoTarjeta, altoTarjeta);

            int interior = Math.Max(240, anchoTarjeta - 48);
            lblBaseDatos.SetBounds(24, 24, interior, 20);
            txtBaseDatos.SetBounds(24, 48, interior, 28);
            lblCarpetaDestino.SetBounds(24, 99, interior, 20);

            if (anchoTarjeta >= 620)
            {
                int anchoRuta = Math.Max(180, interior - 142);
                txtCarpetaDestino.SetBounds(24, 123, anchoRuta, 28);
                btnExaminar.SetBounds(24 + anchoRuta + 12, 118, 130, 38);
                btnGenerarBackup.SetBounds(anchoTarjeta - 194, 202, 170, 40);
            }
            else
            {
                txtCarpetaDestino.SetBounds(24, 123, interior, 28);
                btnExaminar.SetBounds(24, 162, 130, 38);
                btnGenerarBackup.SetBounds(anchoTarjeta - 194, altoTarjeta - 58, 170, 40);
            }
        }

        // Centraliza todos los avisos respecto de la ventana principal.
        private void MostrarMensaje(string titulo, string texto)
        {
            using FormMensaje mensaje = new(titulo, texto);
            mensaje.ShowDialog(formPrincipal);
        }
    }
}
