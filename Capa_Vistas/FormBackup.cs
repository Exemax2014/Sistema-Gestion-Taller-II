using Capa_Logica;

namespace Capa_Vistas
{
    // Muestra el destino configurado por SQL Server y permite iniciar una copia en ese servidor.
    public partial class FormBackup : Form
    {
        private readonly FormPrincipal formPrincipal;
        private readonly BackupLogica backupLogica = new();
        private bool ejecutandoBackup;

        public FormBackup(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;

            btnGenerarBackup.Click += BtnGenerarBackup_Click;
            Resize += (_, _) => AjustarLayout();
            Load += FormBackup_Load;
            AjustarLayout();
        }

        // Revalida autorización y carga los datos de destino publicados por la instancia conectada.
        private void FormBackup_Load(object? sender, EventArgs e)
        {
            if (!backupLogica.PuedeRealizarBackup())
            {
                btnGenerarBackup.Enabled = false;
                MostrarMensaje("Sin permiso", "No tenés permisos para realizar copias de seguridad.");
                return;
            }

            try
            {
                BackupDestino destino = backupLogica.ObtenerDestino();
                txtBaseDatos.Text = destino.NombreBaseDatos;
                txtUbicacionCopia.Text = destino.DirectorioServidor;
            }
            catch (Exception ex)
            {
                btnGenerarBackup.Enabled = false;
                MostrarMensaje("No se pudo leer la configuración", ex.Message);
            }
        }

        // Impide ejecuciones simultáneas y presenta el archivo generado en el servidor SQL.
        private async void BtnGenerarBackup_Click(object? sender, EventArgs e)
        {
            if (ejecutandoBackup)
                return;

            if (!backupLogica.PuedeRealizarBackup())
            {
                MostrarMensaje("Sin permiso", "No tenés permisos para realizar copias de seguridad.");
                return;
            }

            ejecutandoBackup = true;
            btnGenerarBackup.Enabled = false;
            try
            {
                BackupResultado resultado = await Task.Run(backupLogica.GenerarBackup);
                txtUbicacionCopia.Text = resultado.DirectorioServidor;
                MostrarMensaje(
                    "Back Up generado",
                    $"Copia de seguridad generada correctamente.\n\nArchivo: {resultado.NombreArchivo}\n\nUbicación: {resultado.DirectorioServidor}");
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudo generar el Back Up", ex.Message);
            }
            finally
            {
                ejecutandoBackup = false;
                btnGenerarBackup.Enabled = backupLogica.PuedeRealizarBackup();
            }
        }

        // Conserva márgenes proporcionales y reubica la acción principal en anchos reducidos.
        private void AjustarLayout()
        {
            const int margenLateral = 32;
            int ancho = Math.Max(300, ClientSize.Width - margenLateral * 2);
            pnlCabecera.SetBounds(margenLateral, 20, ancho, 100);

            int anchoTarjeta = Math.Min(980, ancho);
            int xTarjeta = (ClientSize.Width - anchoTarjeta) / 2;
            bool anchoGrande = anchoTarjeta >= 620;
            int altoTarjeta = anchoGrande ? 292 : 322;
            pnlTarjeta.SetBounds(xTarjeta, 145, anchoTarjeta, altoTarjeta);

            int interior = Math.Max(240, anchoTarjeta - 48);
            lblBaseDatos.SetBounds(24, 24, interior, 20);
            txtBaseDatos.SetBounds(24, 48, interior, 28);
            lblUbicacionCopia.SetBounds(24, 99, interior, 20);
            txtUbicacionCopia.SetBounds(24, 123, interior, 28);

            if (anchoGrande)
            {
                lblAvisoServidor.SetBounds(24, 166, interior - 190, 42);
                btnGenerarBackup.SetBounds(anchoTarjeta - 194, 164, 170, 40);
            }
            else
            {
                lblAvisoServidor.SetBounds(24, 163, interior, 40);
                btnGenerarBackup.SetBounds(anchoTarjeta - 194, altoTarjeta - 58, 170, 40);
            }
        }

        // Centra los avisos respecto de la ventana principal y no del formulario embebido.
        private void MostrarMensaje(string titulo, string texto)
        {
            using FormMensaje mensaje = new(titulo, texto);
            mensaje.ShowDialog(formPrincipal);
        }
    }
}
