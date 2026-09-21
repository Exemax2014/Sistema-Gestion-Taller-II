using Capa_Logica;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Capa_Vistas
{
    public partial class FormPrincipal : Form
    {
        // =========================================================
        // ESTADO
        // =========================================================

        private Form? formularioActivo;
        private Button? botonActivo;
        private readonly UsuarioLogica usuarioLogicaSucursal = new UsuarioLogica();
        private readonly ReporteLogica reporteLogica = new();
        private readonly BackupLogica backupLogica = new();
        private Bitmap? iconoBackupOriginal;
        private bool cargandoSucursalesOperativas;
        private ToolTip? toolTipSesion;
        private readonly Dictionary<Button, Image?> imagenesMenuOriginales = new();
        private readonly Dictionary<Image, Image> imagenesMenuAtenuadas = new();
        private readonly Dictionary<Button, bool> accesosMenu = new();

        private sealed class OpcionSucursalOperativa
        {
            public int? IdSucursal { get; set; }
            public string Nombre { get; set; } = string.Empty;
        }


        // =========================================================
        // RELOJ
        // =========================================================

        private readonly System.Windows.Forms.Timer reloj =
            new System.Windows.Forms.Timer();


        // =========================================================
        // CONSTANTES VENTANA
        // =========================================================

        private const int WM_NCHITTEST = 0x0084;

        private const int HTCLIENT = 1;

        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTCAPTION = 0x0002;

        private const int TAMANIO_BORDE = 8;


        // =========================================================
        // WINDOWS
        // =========================================================

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();


        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(
            IntPtr hWnd,
            int Msg,
            IntPtr wParam,
            IntPtr lParam
        );


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public FormPrincipal()
        {
            InitializeComponent();
            components ??= new System.ComponentModel.Container();
            toolTipSesion = new ToolTip(components);
            iconoBackupOriginal = CrearIconoBackup();
            btnBackup.Image = iconoBackupOriginal;
            foreach (Button boton in ObtenerBotonesMenu())
                imagenesMenuOriginales[boton] = boton.Image;

            ConfigurarEventos();

            ConfigurarReloj();

            CargarDatosSesion();
            ConfigurarSelectorSucursalOperativa();

            AplicarPermisosMenu();


            ActualizarBotonMaximizar();

            MostrarInicio();

        }


        // =========================================================
        // EVENTOS
        // =========================================================

        private void ConfigurarEventos()
        {
            // Menú principal
            btnInicio.Click +=
                BtnInicio_Click;

            btnVentas.Click +=
                BtnVentas_Click;

            btnClientes.Click +=
                BtnClientes_Click;

            btnProductos.Click +=
                BtnProductos_Click;

            btnUsuarios.Click +=
                BtnUsuarios_Click;

            btnReportes.Click +=
                BtnReportes_Click;

            btnBackup.Click +=
                BtnBackup_Click;


            // Cuenta
            btnEditarPerfil.Click +=
                BtnEditarPerfil_Click;

            btnCerrarSesion.Click +=
                BtnCerrarSesion_Click;


            // Ventana
            btnMinimizar.Click +=
                BtnMinimizar_Click;

            btnMaximizar.Click +=
                BtnMaximizar_Click;

            btnCerrarPrograma.Click +=
                BtnCerrarPrograma_Click;


            // Arrastrar
            pnlCabecera.MouseDown +=
                PnlCabecera_MouseDown;

            picNombreMarca.MouseDown +=
                PnlCabecera_MouseDown;

            picLogo.MouseDown +=
                PnlCabecera_MouseDown;


            // Doble clic
            pnlCabecera.DoubleClick +=
                PnlCabecera_DoubleClick;

            picNombreMarca.DoubleClick +=
                PnlCabecera_DoubleClick;


            // Resize
            Resize +=
                FormPrincipal_Resize;


            // Hover navegación
            foreach (Button boton in ObtenerBotonesMenu())
            {
                boton.MouseEnter +=
                    BotonMenu_MouseEnter;

                boton.MouseLeave +=
                    BotonMenu_MouseLeave;
            }

            cmbSucursalOperativa.SelectedIndexChanged += CmbSucursalOperativa_SelectedIndexChanged;
            FormClosed += (_, _) => LiberarRecursosMenu();
        }

        // Renderiza un símbolo monocromático y deja que el menú aplique su atenuación cacheada habitual.
        private static Bitmap CrearIconoBackup()
        {
            Bitmap imagen = new(42, 42);
            using Graphics graphics = Graphics.FromImage(imagen);
            graphics.Clear(Color.Transparent);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            using Font fuente = new("Segoe UI Symbol", 27F, FontStyle.Regular, GraphicsUnit.Pixel);
            using Brush pincel = new SolidBrush(Color.White);
            using StringFormat formato = new()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            graphics.DrawString("⇩", fuente, pincel, new RectangleF(0, 0, imagen.Width, imagen.Height), formato);
            return imagen;
        }


        // =========================================================
        // DATOS DE SESIÓN
        // =========================================================

        private void CargarDatosSesion()
        {
            lblPerfil.Text =
                SesionActual.Perfil;


            lblUsuarioActual.Text =
                SesionActual.NombreUsuario
                    .ToUpperInvariant();


            // =========================================================
            // SUCURSAL ACTUAL
            // =========================================================

            if (SesionActual.IdSucursalOperativa.HasValue)
            {
                lblSucursalActual.Text =
                    SesionActual.SucursalOperativa;
            }
            else
            {
                lblSucursalActual.Text =
                    "Todas";
            }

            ActualizarDatosSesionVisuales();

            ActualizarFechaHora();
        }

        // Ajusta los datos de sesión al ancho del menú y conserva el texto completo como ayuda contextual.
        private void ActualizarDatosSesionVisuales()
        {
            lblUsuarioActual.AutoEllipsis = true;
            lblUsuarioActual.Width = Math.Max(40, pnlCuenta.ClientSize.Width - lblUsuarioActual.Left - 14);
            lblSucursalActual.AutoEllipsis = true;
            lblSucursalActual.Width = Math.Max(40, pnlSucursal.ClientSize.Width - lblSucursalActual.Left - 14);
            toolTipSesion?.SetToolTip(lblUsuarioActual, lblUsuarioActual.Text);
            toolTipSesion?.SetToolTip(lblPerfil, lblPerfil.Text);
            toolTipSesion?.SetToolTip(lblSucursalActual, lblSucursalActual.Text);
        }


        // =========================================================
        // SUCURSAL OPERATIVA
        // =========================================================

        private void ConfigurarSelectorSucursalOperativa()
        {
            /*
                Usuario con sucursal fija:
                muestra solamente su sucursal y no permite cambiarla.

                Usuario global:
                puede elegir una sucursal específica o "Todas".
            */
            if (!SesionActual.AlcanceGlobal)
            {
                cmbSucursalOperativa.Visible =
                    false;

                lblSucursalActual.Visible =
                    true;

                lblSucursalActual.Text =
                    string.IsNullOrWhiteSpace(
                        SesionActual.SucursalOperativa)
                        ? SesionActual.Sucursal
                        : SesionActual.SucursalOperativa;
                ActualizarDatosSesionVisuales();

                return;
            }


            lblSucursalActual.Visible =
                false;

            cmbSucursalOperativa.Visible =
                true;

            CargarSucursalesOperativas();
        }


        private void CargarSucursalesOperativas()
        {
            try
            {
                cargandoSucursalesOperativas =
                    true;


                List<OpcionSucursalOperativa> opciones =
                    new List<OpcionSucursalOperativa>
                    {
                        new OpcionSucursalOperativa
                        {
                            IdSucursal = null,
                            Nombre = "Todas las sucursales"
                        }
                    };


                List<SucursalUsuarioModelo> sucursales =
                    usuarioLogicaSucursal
                        .ObtenerSucursalesDisponibles();


                opciones.AddRange(
                    sucursales.Select(
                        sucursal =>
                            new OpcionSucursalOperativa
                            {
                                IdSucursal =
                                    sucursal.IdSucursal,

                                Nombre =
                                    sucursal.Nombre
                            }
                    )
                );


                cmbSucursalOperativa.DataSource =
                    opciones;

                cmbSucursalOperativa.DisplayMember =
                    nameof(
                        OpcionSucursalOperativa.Nombre
                    );

                cmbSucursalOperativa.ValueMember =
                    nameof(
                        OpcionSucursalOperativa.IdSucursal
                    );


                OpcionSucursalOperativa? seleccion =
                    opciones.FirstOrDefault(
                        opcion =>
                            opcion.IdSucursal
                            ==
                            SesionActual.IdSucursalOperativa
                    );


                if (seleccion != null)
                {
                    cmbSucursalOperativa.SelectedItem =
                        seleccion;
                }
                else
                {
                    cmbSucursalOperativa.SelectedIndex =
                        0;
                }
            }
            catch (Exception ex)
            {
                cmbSucursalOperativa.DataSource =
                    null;

                MostrarMensajePrincipal(
                    "No se pudieron cargar las sucursales",
                    ex.Message
                );
            }
            finally
            {
                cargandoSucursalesOperativas =
                    false;
            }
        }


        private void CmbSucursalOperativa_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (cargandoSucursalesOperativas)
            {
                return;
            }


            if (!SesionActual.AlcanceGlobal)
            {
                return;
            }


            if (
                cmbSucursalOperativa.SelectedItem
                is not OpcionSucursalOperativa opcion)
            {
                return;
            }


            int? sucursalAnterior =
                SesionActual.IdSucursalOperativa;


            bool cambioPermitido =
                SesionActual.CambiarSucursalOperativa(
                    opcion.IdSucursal,
                    opcion.Nombre
                );


            if (!cambioPermitido)
            {
                SeleccionarSucursalOperativaActual();

                return;
            }


            lblSucursalActual.Text =
                SesionActual.SucursalOperativa;
            ActualizarDatosSesionVisuales();


            /*
                Si Ventas está abierto, se vuelve a crear para que
                tome inmediatamente el nuevo contexto de sucursal.
            */
            // Mantiene Inicio sincronizado con el alcance que se acaba de seleccionar.
            if (
                formularioActivo is FormVentas
                &&
                sucursalAnterior !=
                    SesionActual.IdSucursalOperativa)
            {
                AbrirFormularioEnPanel(
                    new FormVentas(
                        this
                    ),
                    btnVentas
                );
            }

            if (
                formularioActivo is FormInicio inicio
                &&
                sucursalAnterior != SesionActual.IdSucursalOperativa)
            {
                inicio.ActualizarPorSucursalOperativa();
            }
        }


        private void SeleccionarSucursalOperativaActual()
        {
            if (
                cmbSucursalOperativa.DataSource
                is not List<OpcionSucursalOperativa> opciones)
            {
                return;
            }


            cargandoSucursalesOperativas =
                true;


            OpcionSucursalOperativa? seleccion =
                opciones.FirstOrDefault(
                    opcion =>
                        opcion.IdSucursal
                        ==
                        SesionActual.IdSucursalOperativa
                );


            if (seleccion != null)
            {
                cmbSucursalOperativa.SelectedItem =
                    seleccion;
            }


            cargandoSucursalesOperativas =
                false;
        }


        // Crea una copia atenuada única por icono sin alterar el recurso original compartido.
        private Image ObtenerImagenMenuAtenuada(Image imagenOriginal)
        {
            if (imagenesMenuAtenuadas.TryGetValue(imagenOriginal, out Image? imagenAtenuada))
                return imagenAtenuada;

            Bitmap copia = new(imagenOriginal.Width, imagenOriginal.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(copia))
            using (ImageAttributes atributos = new())
            {
                ColorMatrix matriz = new()
                {
                    Matrix00 = 1f,
                    Matrix11 = 1f,
                    Matrix22 = 1f,
                    Matrix33 = 0.4f,
                    Matrix44 = 1f
                };
                atributos.SetColorMatrix(matriz);
                graphics.DrawImage(
                    imagenOriginal,
                    new Rectangle(0, 0, copia.Width, copia.Height),
                    0,
                    0,
                    imagenOriginal.Width,
                    imagenOriginal.Height,
                    GraphicsUnit.Pixel,
                    atributos);
            }

            imagenesMenuAtenuadas.Add(imagenOriginal, copia);
            return copia;
        }


        // Libera las copias de iconos al cerrar el formulario, sin disponer las imágenes originales.
        private void LiberarImagenesMenuAtenuadas()
        {
            foreach (Image imagen in imagenesMenuAtenuadas.Values)
                imagen.Dispose();
            imagenesMenuAtenuadas.Clear();
        }

        // Libera las imágenes propias del botón nuevo sin disponer los recursos compartidos del menú.
        private void LiberarRecursosMenu()
        {
            LiberarImagenesMenuAtenuadas();
            btnBackup.Image = null;
            iconoBackupOriginal?.Dispose();
            iconoBackupOriginal = null;
        }


        private void MostrarMensajePrincipal(
            string titulo,
            string mensaje)
        {
            using FormMensaje formMensaje =
                new FormMensaje(
                    titulo,
                    mensaje
                );


            formMensaje.ShowDialog(
                this
            );
        }


        // =========================================================
        // RELOJ
        // =========================================================

        private void ConfigurarReloj()
        {
            reloj.Interval =
                30000;

            reloj.Tick +=
                Reloj_Tick;

            reloj.Start();
        }


        private void Reloj_Tick(
            object? sender,
            EventArgs e)
        {
            ActualizarFechaHora();
        }


        private void ActualizarFechaHora()
        {
            CultureInfo cultura =
                new CultureInfo("es-AR");


            string fecha =
                DateTime.Now.ToString(
                    "dddd, d 'de' MMMM 'de' yyyy",
                    cultura
                );


            if (!string.IsNullOrWhiteSpace(fecha))
            {
                fecha =
                    char.ToUpper(fecha[0]) +
                    fecha.Substring(1);
            }


            string hora =
                DateTime.Now.ToString(
                    "HH:mm"
                );


            lblFecha.Text =
                $"{fecha} · {hora}";
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
        // MAXIMIZAR
        // =========================================================

        private void BtnMaximizar_Click(
            object? sender,
            EventArgs e)
        {
            CambiarEstadoVentana();
        }


        private void PnlCabecera_DoubleClick(
            object? sender,
            EventArgs e)
        {
            CambiarEstadoVentana();
        }


        private void CambiarEstadoVentana()
        {
            if (
                WindowState ==
                FormWindowState.Maximized)
            {
                WindowState =
                    FormWindowState.Normal;
            }
            else
            {
                WindowState =
                    FormWindowState.Maximized;
            }


            ActualizarBotonMaximizar();
        }


        private void ActualizarBotonMaximizar()
        {
            btnMaximizar.Text =
                WindowState ==
                FormWindowState.Maximized
                    ? "❐"
                    : "□";
        }


        // =========================================================
        // ARRASTRAR
        // =========================================================

        private void PnlCabecera_MouseDown(
            object? sender,
            MouseEventArgs e)
        {
            if (
                e.Button !=
                MouseButtons.Left)
            {
                return;
            }


            if (
                WindowState ==
                FormWindowState.Maximized)
            {
                WindowState =
                    FormWindowState.Normal;

                ActualizarBotonMaximizar();
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
        // REDIMENSIONAR
        // =========================================================

        protected override void WndProc(
            ref Message mensaje)
        {
            if (
                mensaje.Msg ==
                WM_NCHITTEST
                &&
                WindowState !=
                FormWindowState.Maximized)
            {
                base.WndProc(
                    ref mensaje
                );


                if (
                    (int)mensaje.Result !=
                    HTCLIENT)
                {
                    return;
                }


                Point cursor =
                    PointToClient(
                        Cursor.Position
                    );


                bool izquierda =
                    cursor.X <=
                    TAMANIO_BORDE;


                bool derecha =
                    cursor.X >=
                    ClientSize.Width -
                    TAMANIO_BORDE;


                bool arriba =
                    cursor.Y <=
                    TAMANIO_BORDE;


                bool abajo =
                    cursor.Y >=
                    ClientSize.Height -
                    TAMANIO_BORDE;


                if (izquierda && arriba)
                {
                    mensaje.Result =
                        new IntPtr(HTTOPLEFT);

                    return;
                }


                if (derecha && arriba)
                {
                    mensaje.Result =
                        new IntPtr(HTTOPRIGHT);

                    return;
                }


                if (izquierda && abajo)
                {
                    mensaje.Result =
                        new IntPtr(HTBOTTOMLEFT);

                    return;
                }


                if (derecha && abajo)
                {
                    mensaje.Result =
                        new IntPtr(HTBOTTOMRIGHT);

                    return;
                }


                if (izquierda)
                {
                    mensaje.Result =
                        new IntPtr(HTLEFT);

                    return;
                }


                if (derecha)
                {
                    mensaje.Result =
                        new IntPtr(HTRIGHT);

                    return;
                }


                if (arriba)
                {
                    mensaje.Result =
                        new IntPtr(HTTOP);

                    return;
                }


                if (abajo)
                {
                    mensaje.Result =
                        new IntPtr(HTBOTTOM);

                    return;
                }


                return;
            }


            base.WndProc(
                ref mensaje
            );
        }


        // =========================================================
        // RESIZE
        // =========================================================

        private void FormPrincipal_Resize(
            object? sender,
            EventArgs e)
        {
            ActualizarBotonMaximizar();
            ActualizarDatosSesionVisuales();
        }


        // =========================================================
        // CERRAR PROGRAMA
        // =========================================================

        private void BtnCerrarPrograma_Click(
    object? sender,
    EventArgs e)
        {
            if (
                formularioActivo
                is IControlaCambios formularioConCambios
                &&
                !formularioConCambios.PuedeCerrar())
            {
                return;
            }


            using FormMensaje mensaje =
                new FormMensaje(
                    "Estás por salir del sistema",
                    "¿Deseás cerrar la aplicación?",
                    "Sí, salir",
                    true
                );


            if (
                mensaje.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }


            reloj.Stop();

            Application.Exit();
        }

        // =========================================================
        // PERMISOS
        // =========================================================

        // Aplica acceso y estilo sin depender del render disabled de Windows.
        private void AplicarPermisosMenu()
        {
            ConfigurarPermisoBoton(
                btnInicio,
                true
            );


            ConfigurarPermisoBoton(
                btnVentas,
                SesionActual.TienePermiso("VENTAS_VER")
                || SesionActual.TienePermiso("VENTAS_REALIZAR")
            );


            ConfigurarPermisoBoton(
                btnClientes,
                SesionActual.TienePermiso(
                    "CLIENTES_VER"
                )
            );


            ConfigurarPermisoBoton(
                btnProductos,
                SesionActual.TienePermiso("PRODUCTOS_VER")
                || SesionActual.TienePermiso("CATEGORIAS_VER")
                || SesionActual.TienePermiso("MARCAS_VER")
            );


            ConfigurarPermisoBoton(
                btnUsuarios,
                PuedeAccederUsuarios()
            );


            ConfigurarPermisoBoton(
                btnReportes,
                reporteLogica.PuedeAbrirReportes()
            );

            ConfigurarPermisoBoton(
                btnBackup,
                backupLogica.PuedeRealizarBackup()
            );
        }


        private void ConfigurarPermisoBoton(
            Button boton,
            bool tienePermiso)
        {
            boton.Visible =
                true;


            accesosMenu[boton] = tienePermiso;
            boton.Enabled = true;
            boton.TabStop = tienePermiso;

            if (imagenesMenuOriginales.TryGetValue(boton, out Image? imagenOriginal))
                boton.Image = tienePermiso || imagenOriginal is null
                    ? imagenOriginal
                    : ObtenerImagenMenuAtenuada(imagenOriginal);


            if (tienePermiso)
            {
                boton.BackColor =
                    Color.FromArgb(
                        17,
                        21,
                        26
                    );


                boton.ForeColor =
                    Color.White;


                boton.Cursor =
                    Cursors.Hand;
            }
            else
            {
                boton.BackColor =
                    Color.FromArgb(
                        28,
                        31,
                        35
                    );


                boton.ForeColor = Color.FromArgb(150, 154, 160);


                boton.Cursor =
                    Cursors.Default;
            }
        }


        // Consulta el acceso guardado para bloquear también la navegación interna.
        private bool TieneAccesoMenu(Button boton)
        {
            return accesosMenu.TryGetValue(boton, out bool permitido) && permitido;
        }


        private Button[] ObtenerBotonesMenu()
        {
            return new Button[]
            {
                btnInicio,
                btnVentas,
                btnClientes,
                btnProductos,
                btnUsuarios,
                btnReportes,
                btnBackup
            };
        }


        // =========================================================
        // BOTÓN ACTIVO
        // =========================================================

        private void SeleccionarBoton(
            Button boton)
        {
            if (!TieneAccesoMenu(boton))
            {
                return;
            }


            botonActivo =
                boton;


            foreach (
                Button item
                in ObtenerBotonesMenu())
            {
                if (TieneAccesoMenu(item))
                {
                    item.BackColor =
                        Color.FromArgb(
                            17,
                            21,
                            26
                        );


                    item.ForeColor =
                        Color.White;


                    item.Font =
                        new Font(
                            "Segoe UI",
                            8.5F,
                            FontStyle.Regular
                        );
                }
                else
                {
                    item.BackColor =
                        Color.FromArgb(
                            28,
                            31,
                            35
                        );


                    item.ForeColor =
                        Color.FromArgb(
                            150,
                            154,
                            160
                        );
                }
            }


            boton.BackColor =
                Color.FromArgb(
                    72,
                    53,
                    24
                );


            boton.ForeColor =
                Color.White;


            boton.Font =
                new Font(
                    "Segoe UI",
                    8.5F,
                    FontStyle.Bold
                );
        }


        // =========================================================
        // HOVER
        // =========================================================

        private void BotonMenu_MouseEnter(
            object? sender,
            EventArgs e)
        {
            if (
                sender is Button boton
                &&
                TieneAccesoMenu(boton)
                &&
                boton != botonActivo)
            {
                boton.BackColor =
                    Color.FromArgb(
                        31,
                        34,
                        39
                    );
            }
        }


        private void BotonMenu_MouseLeave(
            object? sender,
            EventArgs e)
        {
            if (
                sender is Button boton
                &&
                TieneAccesoMenu(boton)
                &&
                boton != botonActivo)
            {
                boton.BackColor =
                    Color.FromArgb(
                        17,
                        21,
                        26
                    );
            }
        }


        // =========================================================
        // CERRAR FORMULARIO ACTIVO
        //
        // Antes de cerrar pregunta al formulario si tiene
        // cambios pendientes.
        // =========================================================

        private bool CerrarFormularioActivo()
        {
            if (formularioActivo == null)
            {
                return true;
            }


            if (
                formularioActivo
                is IControlaCambios formularioConCambios
                &&
                !formularioConCambios.PuedeCerrar())
            {
                return false;
            }


            formularioActivo.Close();

            formularioActivo.Dispose();

            formularioActivo = null;


            return true;
        }


        // =========================================================
        // ABRIR FORMULARIO
        // =========================================================


        public void AbrirFormularioEnPanel(
            Form formulario,
            Button botonOrigen)
        {
            bool origenEsModulo = accesosMenu.TryGetValue(botonOrigen, out bool tieneAccesoModulo);
            bool detalleStockAutorizado = botonOrigen == btnProductos
                && formulario is FormProductoDetalle detalle
                && detalle.EsGestionStockDesdeReporte
                && SesionActual.TienePermiso("PRODUCTOS_MODIFICAR");

            if (origenEsModulo && !tieneAccesoModulo && !detalleStockAutorizado)
            {
                if (formulario is FormBackup)
                {
                    MostrarMensajePrincipal(
                        "Sin permiso",
                        "No tenés permisos para realizar copias de seguridad.");
                }

                formulario.Dispose();

                return;
            }


            if (!CerrarFormularioActivo())
            {
                formulario.Dispose();

                return;
            }


            SeleccionarBoton(
                botonOrigen
            );


            formularioActivo =
                formulario;


            formulario.TopLevel =
                false;


            formulario.FormBorderStyle =
                FormBorderStyle.None;


            formulario.Dock =
                DockStyle.Fill;


            pnlContenido.Controls.Add(
                formulario
            );


            formulario.Show();

            formulario.BringToFront();
        }




        // =========================================================
        // INICIO
        // =========================================================

        private void MostrarInicio()
        {
            FormInicio inicio =
                new FormInicio(
                    this
                );


            AbrirFormularioEnPanel(
                inicio,
                btnInicio
            );
        }



        // =========================================================
        // ACCESO BOTON CLIENTES
        // =========================================================

        public Button BotonClientes
        {
            get
            {
                return btnClientes;
            }
        }


        // =========================================================
        // ACCESO BOTÓN VENTAS
        // =========================================================

        public Button BotonVentas
        {
            get
            {
                return btnVentas;
            }
        }


        // Expone el botón de origen para que la navegación embebida conserve Usuarios seleccionado.
        public Button BotonUsuarios
        {
            get
            {
                return btnUsuarios;
            }
        }


        // Permite entrar al contenedor Usuarios si se autorizó alguna de sus vistas internas.
        private bool PuedeAccederUsuarios()
        {
            return SesionActual.TienePermiso("USUARIOS_VER")
                || SesionActual.TienePermiso("PERMISOS_GESTIONAR")
                || SesionActual.TienePermiso("SUCURSALES_VER");
        }


        // Permite que el detalle abierto desde Reportes conserve seleccionado su módulo de origen.
        public Button BotonReportes
        {
            get
            {
                return btnReportes;
            }
        }


        // =========================================================
        // INICIO
        // =========================================================

        private void BtnInicio_Click(
            object? sender,
            EventArgs e)
        {
            MostrarInicio();
        }


        // =========================================================
        // VENTAS
        // =========================================================

        // Abre Ventas si la sesión puede consultar el módulo o registrar operaciones.
        private void BtnVentas_Click(
            object? sender,
            EventArgs e)
        {
            if (
                !SesionActual.TienePermiso("VENTAS_VER")
                && !SesionActual.TienePermiso("VENTAS_REALIZAR"))
            {
                return;
            }


            AbrirFormularioEnPanel(
                new FormVentas(),
                btnVentas
            );
        }


        // =========================================================
        // CLIENTES
        // =========================================================

        private void BtnClientes_Click(
            object? sender,
            EventArgs e)
        {
            if (
                !SesionActual.TienePermiso(
                    "CLIENTES_VER"
                ))
            {
                return;
            }


            AbrirFormularioEnPanel(
                new FormClientes(this),
                btnClientes
            );
        }


        // =========================================================
        // PRODUCTOS
        // =========================================================

        // Permite a los formularios internos conservar
        // seleccionado el módulo Productos.
        public Button BotonProductos
        {
            get
            {
                return btnProductos;
            }
        }


        private void BtnProductos_Click(
            object? sender,
            EventArgs e)
        {
            if (!SesionActual.TienePermiso("PRODUCTOS_VER")
                && !SesionActual.TienePermiso("CATEGORIAS_VER")
                && !SesionActual.TienePermiso("MARCAS_VER"))
            {
                return;
            }


            AbrirFormularioEnPanel(
                new FormProductos(this),
                btnProductos
            );
        }

        // =========================================================
        // USUARIOS
        // =========================================================

        private void BtnUsuarios_Click(
            object? sender,
            EventArgs e)
        {
            if (!PuedeAccederUsuarios())
            {
                return;
            }


            AbrirFormularioEnPanel(
                new FormUsuarios(this),
                btnUsuarios
            );
        }


        // =========================================================
        // REPORTES
        // =========================================================

        // Revalida el acceso al módulo aunque el evento se invoque fuera del menú.
        private void BtnReportes_Click(
            object? sender,
            EventArgs e)
        {
            if (!reporteLogica.PuedeAbrirReportes())
            {
                return;
            }

            FormReportesGeneral reportes =
                new FormReportesGeneral(this);


            AbrirFormularioEnPanel(
                reportes,
                btnReportes
            );
        }

        // Revalida el permiso y abre el formulario embebido de generación de copias.
        private void BtnBackup_Click(object? sender, EventArgs e)
        {
            if (!backupLogica.PuedeRealizarBackup())
            {
                MostrarMensajePrincipal(
                    "Sin permiso",
                    "No tenés permisos para realizar copias de seguridad.");
                return;
            }

            AbrirFormularioEnPanel(new FormBackup(this), btnBackup);
        }


        // =========================================================
        // EDITAR PERFIL
        // =========================================================

        private void BtnEditarPerfil_Click(
            object? sender,
            EventArgs e)
        {
            if (!SesionActual.SesionIniciada) return;
            AbrirFormularioEnPanel(new FormMiPerfil(this), btnEditarPerfil);
        }

        // Actualiza inmediatamente el nombre visible tras editar la cuenta propia.
        public void ActualizarSesionVisibleDesdePerfil() => CargarDatosSesion();

        // Regresa al Inicio al cancelar o completar la edición del perfil propio.
        public void VolverAInicioDesdePerfil() => MostrarInicio();


        // =========================================================
        // CERRAR SESION
        // =========================================================

        private void BtnCerrarSesion_Click(
    object? sender,
    EventArgs e)
        {
            if (
                formularioActivo
                is IControlaCambios formularioConCambios
                &&
                !formularioConCambios.PuedeCerrar())
            {
                return;
            }


            using FormMensaje mensaje =
                new FormMensaje(
                    "Cerrar sesión",
                    "¿Deseás cerrar la sesión actual?",
                    "Sí, cerrar sesión",
                    true
                );


            if (
                mensaje.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }


            reloj.Stop();

            SesionActual.Cerrar();

            Close();
        }
    }
}
