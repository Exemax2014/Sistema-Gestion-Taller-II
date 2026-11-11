using Microsoft.Web.WebView2.WinForms;

namespace Capa_Vistas
{
    // Muestra el manual distribuido con la aplicación sin abandonar la ventana principal.
    public sealed class FormManualUsuario : Form
    {
        private readonly FormPrincipal formPrincipal;
        private readonly WebView2 visorPdf = new() { Dock = DockStyle.Fill, Visible = false };
        private readonly Label lblEstado = new()
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(105, 110, 116),
            TextAlign = ContentAlignment.MiddleCenter
        };
        private bool manualInicializado;

        public FormManualUsuario(FormPrincipal formPrincipal)
        {
            this.formPrincipal = formPrincipal;
            ConstruirVista();
            Shown += async (_, _) => await CargarManualAsync();
        }

        private void ConstruirVista()
        {
            BackColor = Color.FromArgb(245, 246, 248);

            Panel contenido = new()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(28, 22, 28, 28),
                BackColor = BackColor
            };

            TableLayoutPanel estructura = new()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            estructura.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            estructura.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            estructura.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel cabecera = new() { Dock = DockStyle.Fill };
            Button btnVolver = new()
            {
                Dock = DockStyle.Left,
                Width = 112,
                Text = "← Volver",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(45, 49, 54),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Click += (_, _) => formPrincipal.VolverAInicioDesdeManual();

            Label titulo = new()
            {
                Dock = DockStyle.Fill,
                Text = "Manual de usuario",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 49, 54),
                Padding = new Padding(16, 0, 0, 0)
            };
            cabecera.Controls.Add(titulo);
            cabecera.Controls.Add(btnVolver);

            Label subtitulo = new()
            {
                Dock = DockStyle.Fill,
                Text = "Consultá la guía completa de uso del sistema.",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(105, 110, 116)
            };

            Panel contenedorVisor = new()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(1)
            };
            contenedorVisor.Controls.Add(visorPdf);
            contenedorVisor.Controls.Add(lblEstado);

            estructura.Controls.Add(cabecera, 0, 0);
            estructura.Controls.Add(subtitulo, 0, 1);
            estructura.Controls.Add(contenedorVisor, 0, 2);
            contenido.Controls.Add(estructura);
            Controls.Add(contenido);
        }

        private async Task CargarManualAsync()
        {
            if (manualInicializado) return;
            manualInicializado = true;

            string rutaManual = Path.Combine(AppContext.BaseDirectory, "Documentacion", "Manual_de_Usuario.pdf");
            if (!File.Exists(rutaManual))
            {
                lblEstado.Text = "El Manual de Usuario no está instalado con la aplicación.";
                MostrarMensaje("Manual no disponible", "No se encontró el Manual de Usuario instalado con la aplicación.");
                return;
            }

            lblEstado.Text = "Abriendo manual…";
            try
            {
                await visorPdf.EnsureCoreWebView2Async();
                visorPdf.Source = new Uri(rutaManual, UriKind.Absolute);
                visorPdf.Visible = true;
                lblEstado.Visible = false;
            }
            catch
            {
                visorPdf.Visible = false;
                lblEstado.Text = "No fue posible inicializar el visor del manual.";
                MostrarMensaje("No se pudo abrir el manual", "No fue posible inicializar el visor del manual. Verificá que Microsoft Edge WebView2 Runtime esté instalado.");
            }
        }

        private void MostrarMensaje(string titulo, string mensaje)
        {
            using FormMensaje dialogo = new(titulo, mensaje);
            dialogo.ShowDialog(formPrincipal);
        }
    }
}
