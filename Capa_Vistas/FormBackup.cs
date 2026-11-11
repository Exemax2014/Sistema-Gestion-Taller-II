using Capa_Logica;

namespace Capa_Vistas
{
    // Muestra el destino configurado por SQL Server y permite iniciar una copia en ese servidor.
    public partial class FormBackup : Form
    {
        private readonly FormPrincipal formPrincipal;
        private readonly BackupLogica backupLogica = new();
        private readonly Panel pnlHistorial = new();
        private readonly DataGridView dgvHistorial = new();
        private readonly Label lblHistorialVacio = new();
        private readonly TableLayoutPanel tlpPagina = new() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, AutoScroll = true, GrowStyle = TableLayoutPanelGrowStyle.FixedSize, BackColor = Color.FromArgb(245, 246, 248) };
        private readonly TableLayoutPanel tlpTarjetaBackup = new() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, GrowStyle = TableLayoutPanelGrowStyle.FixedSize, BackColor = Color.White };
        private bool ejecutandoBackup;

        public FormBackup(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;

            btnGenerarBackup.Click += BtnGenerarBackup_Click;
            ConfigurarHistorial();
            ConfigurarEstructuraVisual();
            Load += FormBackup_Load;
        }

        private void ConfigurarEstructuraVisual()
        {
            foreach (Control control in new Control[] { pnlCabecera, pnlTarjeta })
                Controls.Remove(control);

            pnlCabecera.Dock = DockStyle.Fill;
            pnlTarjeta.Dock = DockStyle.Fill;
            pnlTarjeta.Padding = new Padding(24);
            pnlHistorial.Dock = DockStyle.Fill;
            pnlHistorial.Padding = new Padding(14);

            foreach (Control control in new Control[] { lblBaseDatos, txtBaseDatos, lblUbicacionCopia, txtUbicacionCopia, lblAvisoServidor, btnGenerarBackup })
                pnlTarjeta.Controls.Remove(control);

            tlpTarjetaBackup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTarjetaBackup.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpTarjetaBackup.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpTarjetaBackup.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpTarjetaBackup.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpTarjetaBackup.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            foreach (Control control in new Control[] { lblBaseDatos, txtBaseDatos, lblUbicacionCopia, txtUbicacionCopia })
                control.Dock = DockStyle.Fill;
            lblAvisoServidor.Dock = DockStyle.Fill;
            btnGenerarBackup.Dock = DockStyle.Right;
            btnGenerarBackup.Width = 180;

            Panel acciones = new() { Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0), BackColor = Color.White };
            acciones.Controls.Add(lblAvisoServidor);
            acciones.Controls.Add(btnGenerarBackup);
            tlpTarjetaBackup.Controls.Add(lblBaseDatos, 0, 0);
            tlpTarjetaBackup.Controls.Add(txtBaseDatos, 0, 1);
            tlpTarjetaBackup.Controls.Add(lblUbicacionCopia, 0, 2);
            tlpTarjetaBackup.Controls.Add(txtUbicacionCopia, 0, 3);
            tlpTarjetaBackup.Controls.Add(acciones, 0, 4);
            pnlTarjeta.Controls.Add(tlpTarjetaBackup);

            tlpPagina.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPagina.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tlpPagina.RowStyles.Add(new RowStyle(SizeType.Absolute, 270F));
            tlpPagina.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlCabecera.Margin = new Padding(0, 0, 0, 16);
            pnlTarjeta.Margin = new Padding(0, 0, 0, 18);
            pnlHistorial.Margin = Padding.Empty;
            tlpPagina.Padding = new Padding(24);
            tlpPagina.Controls.Add(pnlCabecera, 0, 0);
            tlpPagina.Controls.Add(pnlTarjeta, 0, 1);
            tlpPagina.Controls.Add(pnlHistorial, 0, 2);
            Controls.Add(tlpPagina);
            tlpPagina.BringToFront();
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
                CargarHistorial();
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
                CargarHistorial();
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

        private void ConfigurarHistorial()
        {
            pnlHistorial.BackColor = Color.White;
            pnlHistorial.BorderStyle = BorderStyle.FixedSingle;
            pnlHistorial.Controls.Add(dgvHistorial);
            pnlHistorial.Controls.Add(lblHistorialVacio);

            dgvHistorial.Dock = DockStyle.Fill;
            dgvHistorial.ReadOnly = true;
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AllowUserToDeleteRows = false;
            dgvHistorial.AllowUserToResizeRows = false;
            dgvHistorial.RowHeadersVisible = false;
            dgvHistorial.AutoGenerateColumns = false;
            dgvHistorial.ScrollBars = ScrollBars.Both;
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = nameof(BackupHistorialItem.Fecha), DataPropertyName = nameof(BackupHistorialItem.Fecha), HeaderText = "Fecha", Width = 145, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = nameof(BackupHistorialItem.Usuario), DataPropertyName = nameof(BackupHistorialItem.Usuario), HeaderText = "Usuario", Width = 170 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = nameof(BackupHistorialItem.Detalle), DataPropertyName = nameof(BackupHistorialItem.Detalle), HeaderText = "Archivo / detalle" });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = nameof(BackupHistorialItem.Sucursal), DataPropertyName = nameof(BackupHistorialItem.Sucursal), HeaderText = "Sucursal", Width = 170 });
            DataGridViewLayoutHelper.Normalizar(dgvHistorial, nameof(BackupHistorialItem.Detalle));
            lblHistorialVacio.Dock = DockStyle.Fill;
            lblHistorialVacio.Text = "Todavía no hay copias de seguridad registradas.";
            lblHistorialVacio.TextAlign = ContentAlignment.MiddleCenter;
            lblHistorialVacio.ForeColor = Color.FromArgb(105, 110, 116);
        }

        private void CargarHistorial()
        {
            try
            {
                List<BackupHistorialItem> historial = backupLogica.ObtenerHistorial();
                dgvHistorial.DataSource = historial;
                lblHistorialVacio.Visible = historial.Count == 0;
            }
            catch (Exception ex)
            {
                dgvHistorial.DataSource = null;
                lblHistorialVacio.Text = $"No se pudo cargar el historial: {ex.Message}";
                lblHistorialVacio.Visible = true;
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
