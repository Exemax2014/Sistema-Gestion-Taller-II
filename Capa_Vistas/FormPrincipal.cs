using Capa_Logica;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Capa_Vistas
{
    public partial class FormPrincipal : Form
    {
        // =========================================================
        // ESTADO DEL FORMULARIO
        // =========================================================

        private Form? formularioActivo;
        private Button? botonActivo;


        // =========================================================
        // CONSTANTES PARA VENTANA SIN BORDE
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
        // FUNCIONES DE WINDOWS
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

            ConfigurarEventos();

            CargarDatosSesion();

            AplicarPermisosMenu();

            MostrarInicio();

            ActualizarBotonMaximizar();
        }


        // =========================================================
        // CONFIGURACIÓN DE EVENTOS
        // =========================================================

        private void ConfigurarEventos()
        {
            // Menú principal
            btnInicio.Click += BtnInicio_Click;
            btnVentas.Click += BtnVentas_Click;
            btnClientes.Click += BtnClientes_Click;
            btnProductos.Click += BtnProductos_Click;
            btnUsuarios.Click += BtnUsuarios_Click;
            btnReportes.Click += BtnReportes_Click;

            // Usuario
            btnUsuarioMenu.Click += BtnUsuarioMenu_Click;
            btnEditarPerfil.Click += BtnEditarPerfil_Click;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;

            // Ventana
            btnMinimizar.Click += BtnMinimizar_Click;
            btnMaximizar.Click += BtnMaximizar_Click;
            btnCerrarPrograma.Click += BtnCerrarPrograma_Click;

            // Arrastrar ventana desde la cabecera
            pnlCabecera.MouseDown += PnlCabecera_MouseDown;
            lblMarca.MouseDown += PnlCabecera_MouseDown;
            picLogo.MouseDown += PnlCabecera_MouseDown;

            // Doble clic para maximizar/restaurar
            pnlCabecera.DoubleClick += PnlCabecera_DoubleClick;
            lblMarca.DoubleClick += PnlCabecera_DoubleClick;

            // Reubicar menú del usuario
            Resize += FormPrincipal_Resize;

            // Hover menú lateral
            foreach (Button boton in ObtenerBotonesMenu())
            {
                boton.MouseEnter += BotonMenu_MouseEnter;
                boton.MouseLeave += BotonMenu_MouseLeave;
            }
        }


        // =========================================================
        // DATOS DE SESIÓN
        // =========================================================

        private void CargarDatosSesion()
        {
            lblPerfil.Text =
                SesionActual.Perfil;


            lblUsuario.Text =
                $"{SesionActual.Nombre} {SesionActual.Apellido}";


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


            lblFecha.Text =
                fecha;


            ActualizarBotonUsuario(false);
        }


        // =========================================================
        // BOTÓN DEL USUARIO
        // =========================================================

        private void ActualizarBotonUsuario(
            bool menuAbierto)
        {
            string flecha =
                menuAbierto
                    ? "▲"
                    : "▼";


            btnUsuarioMenu.Text =
                $"●   {SesionActual.NombreUsuario}               {flecha}";
        }


        // =========================================================
        // MENÚ DESPLEGABLE USUARIO
        // =========================================================

        private void BtnUsuarioMenu_Click(
            object? sender,
            EventArgs e)
        {
            bool mostrar =
                !pnlMenuUsuario.Visible;


            if (mostrar)
            {
                PosicionarMenuUsuario();

                pnlMenuUsuario.Visible =
                    true;

                pnlMenuUsuario.BringToFront();
            }
            else
            {
                pnlMenuUsuario.Visible =
                    false;
            }


            ActualizarBotonUsuario(
                mostrar
            );
        }


        private void PosicionarMenuUsuario()
        {
            Point posicionPantalla =
                btnUsuarioMenu.PointToScreen(
                    new Point(
                        0,
                        btnUsuarioMenu.Height
                    )
                );

            Point posicionFormulario =
                PointToClient(
                    posicionPantalla
                );

            int xCentrado =
                posicionFormulario.X
                +
                (btnUsuarioMenu.Width / 2)
                -
                (pnlMenuUsuario.Width / 2);

            pnlMenuUsuario.Location =
                new Point(
                    xCentrado,
                    posicionFormulario.Y + 4
                );
        }


        private void OcultarMenuUsuario()
        {
            pnlMenuUsuario.Visible =
                false;

            ActualizarBotonUsuario(
                false
            );
        }


        // =========================================================
        // MINIMIZAR
        // =========================================================

        private void BtnMinimizar_Click(
            object? sender,
            EventArgs e)
        {
            OcultarMenuUsuario();

            WindowState =
                FormWindowState.Minimized;
        }


        // =========================================================
        // MAXIMIZAR / RESTAURAR
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
            OcultarMenuUsuario();


            if (WindowState ==
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
            if (WindowState ==
                FormWindowState.Maximized)
            {
                btnMaximizar.Text = "❐";
            }
            else
            {
                btnMaximizar.Text = "□";
            }
        }


        // =========================================================
        // ARRASTRAR VENTANA
        // =========================================================

        private void PnlCabecera_MouseDown(
            object? sender,
            MouseEventArgs e)
        {
            if (e.Button !=
                MouseButtons.Left)
            {
                return;
            }


            // Si está maximizada y se intenta arrastrar,
            // primero restauramos.
            if (WindowState ==
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
        // REDIMENSIONAR VENTANA SIN BORDE
        // =========================================================

        protected override void WndProc(
            ref Message mensaje)
        {
            if (mensaje.Msg ==
                WM_NCHITTEST
                &&
                WindowState !=
                FormWindowState.Maximized)
            {
                base.WndProc(
                    ref mensaje
                );


                if ((int)mensaje.Result !=
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
                        new IntPtr(
                            HTTOPLEFT
                        );

                    return;
                }


                if (derecha && arriba)
                {
                    mensaje.Result =
                        new IntPtr(
                            HTTOPRIGHT
                        );

                    return;
                }


                if (izquierda && abajo)
                {
                    mensaje.Result =
                        new IntPtr(
                            HTBOTTOMLEFT
                        );

                    return;
                }


                if (derecha && abajo)
                {
                    mensaje.Result =
                        new IntPtr(
                            HTBOTTOMRIGHT
                        );

                    return;
                }


                if (izquierda)
                {
                    mensaje.Result =
                        new IntPtr(
                            HTLEFT
                        );

                    return;
                }


                if (derecha)
                {
                    mensaje.Result =
                        new IntPtr(
                            HTRIGHT
                        );

                    return;
                }


                if (arriba)
                {
                    mensaje.Result =
                        new IntPtr(
                            HTTOP
                        );

                    return;
                }


                if (abajo)
                {
                    mensaje.Result =
                        new IntPtr(
                            HTBOTTOM
                        );

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


            if (pnlMenuUsuario.Visible)
            {
                PosicionarMenuUsuario();

                pnlMenuUsuario.BringToFront();
            }
        }


        // =========================================================
        // CERRAR PROGRAMA
        // =========================================================

        private void BtnCerrarPrograma_Click(
            object? sender,
            EventArgs e)
        {
            OcultarMenuUsuario();


            using FormMensaje mensaje =
                new FormMensaje(
                    "Estás por salir del sistema",
                    "¿Deseás cerrar la aplicación?",
                    "Sí, salir",
                    true
                );


            if (mensaje.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }


            Application.Exit();
        }


        // =========================================================
        // PERMISOS
        // =========================================================

        private void AplicarPermisosMenu()
        {
            ConfigurarPermisoBoton(
                btnInicio,
                true
            );


            ConfigurarPermisoBoton(
                btnVentas,
                SesionActual.TienePermiso(
                    "VENTAS_VER"
                )
            );


            ConfigurarPermisoBoton(
                btnClientes,
                SesionActual.TienePermiso(
                    "CLIENTES_VER"
                )
            );


            ConfigurarPermisoBoton(
                btnProductos,
                SesionActual.TienePermiso(
                    "PRODUCTOS_VER"
                )
            );


            ConfigurarPermisoBoton(
                btnUsuarios,
                SesionActual.TienePermiso(
                    "USUARIOS_VER"
                )
            );


            bool puedeVerReportes =
                SesionActual.TienePermiso(
                    "REPORTES_ADMINISTRADOR"
                )
                ||
                SesionActual.TienePermiso(
                    "REPORTES_GERENTE"
                )
                ||
                SesionActual.TienePermiso(
                    "REPORTES_VENDEDOR"
                );


            ConfigurarPermisoBoton(
                btnReportes,
                puedeVerReportes
            );
        }


        private void ConfigurarPermisoBoton(
            Button boton,
            bool tienePermiso)
        {
            boton.Visible =
                true;


            boton.Enabled =
                tienePermiso;


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


                boton.ForeColor =
                    Color.FromArgb(
                        95,
                        98,
                        103
                    );


                boton.Cursor =
                    Cursors.Default;
            }
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
                btnReportes
            };
        }


        // =========================================================
        // SELECCIÓN DEL MENÚ
        // =========================================================

        private void SeleccionarBoton(
            Button boton)
        {
            if (!boton.Enabled)
            {
                return;
            }


            botonActivo =
                boton;


            foreach (
                Button item
                in ObtenerBotonesMenu())
            {
                if (item.Enabled)
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
                            95,
                            98,
                            103
                        );
                }
            }


            // Color dorado oscuro para el módulo activo.
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
        // HOVER MENÚ
        // =========================================================

        private void BotonMenu_MouseEnter(
            object? sender,
            EventArgs e)
        {
            if (
                sender is Button boton
                &&
                boton.Enabled
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
                boton.Enabled
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
        // FORMULARIO ACTIVO
        // =========================================================

        private void CerrarFormularioActivo()
        {
            if (formularioActivo == null)
            {
                return;
            }


            formularioActivo.Close();

            formularioActivo.Dispose();

            formularioActivo =
                null;
        }


        // =========================================================
        // ABRIR MÓDULO
        // =========================================================

        public void AbrirFormularioEnPanel(
            Form formulario,
            Button botonOrigen)
        {
            OcultarMenuUsuario();


            if (!botonOrigen.Enabled)
            {
                formulario.Dispose();

                return;
            }


            CerrarFormularioActivo();


            SeleccionarBoton(
                botonOrigen
            );


            lblTituloInicio.Visible =
                false;

            pnlLineaTitulo.Visible =
                false;

            lblBienvenida.Visible =
                false;

            lblDescripcion.Visible =
                false;


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
            OcultarMenuUsuario();


            CerrarFormularioActivo();


            SeleccionarBoton(
                btnInicio
            );


            lblTituloInicio.Text =
                "Inicio";


            lblBienvenida.Text =
                $"Bienvenido, {SesionActual.Nombre}";


            lblDescripcion.Text =
                "Seleccione una opción del menú para comenzar.";


            lblTituloInicio.Visible =
                true;

            pnlLineaTitulo.Visible =
                true;

            lblBienvenida.Visible =
                true;

            lblDescripcion.Visible =
                true;


            lblTituloInicio.BringToFront();

            pnlLineaTitulo.BringToFront();

            lblBienvenida.BringToFront();

            lblDescripcion.BringToFront();
        }


        // =========================================================
        // MODULO TEMPORAL
        // =========================================================

        private void MostrarModuloTemporal(
            string titulo,
            string descripcion,
            Button boton)
        {
            OcultarMenuUsuario();


            if (!boton.Enabled)
            {
                return;
            }


            CerrarFormularioActivo();


            SeleccionarBoton(
                boton
            );


            lblTituloInicio.Text =
                titulo;


            lblBienvenida.Text =
                titulo;


            lblDescripcion.Text =
                descripcion;


            lblTituloInicio.Visible =
                true;

            pnlLineaTitulo.Visible =
                true;

            lblBienvenida.Visible =
                true;

            lblDescripcion.Visible =
                true;


            lblTituloInicio.BringToFront();

            pnlLineaTitulo.BringToFront();

            lblBienvenida.BringToFront();

            lblDescripcion.BringToFront();
        }


        // =========================================================
        // EVENTOS MÓDULOS
        // =========================================================

        public Button BotonClientes
        {
            get
            {
                return btnClientes;
            }
        }

        private void BtnInicio_Click(
            object? sender,
            EventArgs e)
        {
            MostrarInicio();
        }


        private void BtnVentas_Click(
            object? sender,
            EventArgs e)
        {
            if (
                SesionActual.TienePermiso(
                    "VENTAS_VER"
                ))
            {
                AbrirFormularioEnPanel(
                    new FormVentas(),
                    btnVentas
                );

                return;
            }
        }


        private void BtnClientes_Click(
             object? sender,
             EventArgs e)
        {
            AbrirFormularioEnPanel(
                new FormClientes(this),
                btnClientes
            );
        }


        private void BtnProductos_Click(
             object? sender,
             EventArgs e)
        {
            AbrirFormularioEnPanel(new FormProductos(), btnProductos);
        }

        private void BtnUsuarios_Click(
            object? sender,
            EventArgs e)
        {
            MostrarModuloTemporal(
                "Usuarios",
                "Usuarios, perfiles y permisos.",
                btnUsuarios
            );
        }


        private void BtnReportes_Click(
            object? sender,
            EventArgs e)
        {
            if (
                SesionActual.Perfil ==
                "Vendedor")
            {
                AbrirFormularioEnPanel(
                    new FormReportesVendedor(),
                    btnReportes
                );

                return;
            }


            MostrarModuloTemporal(
                "Reportes",
                "Reportes y estadísticas del sistema.",
                btnReportes
            );
        }


        // =========================================================
        // EDITAR PERFIL
        // =========================================================

        private void BtnEditarPerfil_Click(
            object? sender,
            EventArgs e)
        {
            OcultarMenuUsuario();


            using FormMensaje mensaje =
                new FormMensaje(
                    "Editar perfil",
                    "Esta funcionalidad se implementará posteriormente."
                );


            mensaje.ShowDialog(this);
        }


        // =========================================================
        // CERRAR SESIÓN
        // =========================================================

        private void BtnCerrarSesion_Click(
    object? sender,
    EventArgs e)
        {
            OcultarMenuUsuario();


            using FormMensaje mensaje =
                new FormMensaje(
                    "Cerrar sesión",
                    "¿Deseás cerrar la sesión actual?",
                    "Sí, cerrar sesión",
                    true
                );


            if (mensaje.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }


            SesionActual.Cerrar();


            Close();
        }
    }
}