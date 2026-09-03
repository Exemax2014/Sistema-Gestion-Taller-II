using Capa_Logica;

namespace Capa_Vistas
{
    // ============================================================
    // Formulario: FormPrincipal
    //
    // Responsabilidad:
    // Es el contenedor general de la aplicación.
    //
    // La estructura visual se encuentra en:
    // FormPrincipal.Designer.cs
    //
    // Este archivo contiene únicamente:
    // - navegación;
    // - datos de sesión;
    // - permisos;
    // - apertura de módulos;
    // - comportamiento del menú.
    // ============================================================
    public partial class FormPrincipal : Form
    {
        // Formulario actualmente abierto dentro de pnlContenido.
        private Form? formularioActivo;

        // Botón actualmente seleccionado del menú.
        private Button? botonActivo;


        public FormPrincipal()
        {
            InitializeComponent();

            ConfigurarEventos();
            CargarDatosSesion();
            AplicarPermisosMenu();
            MostrarInicio();
        }


        // ========================================================
        // Configura los eventos de los controles creados desde
        // el Diseñador de Windows Forms.
        // ========================================================
        private void ConfigurarEventos()
        {
            btnInicio.Click += BtnInicio_Click;
            btnVentas.Click += BtnVentas_Click;
            btnClientes.Click += BtnClientes_Click;
            btnProductos.Click += BtnProductos_Click;
            btnUsuarios.Click += BtnUsuarios_Click;
            btnReportes.Click += BtnReportes_Click;

            btnEditarPerfil.Click += BtnEditarPerfil_Click;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;

            // Efecto visual al pasar el mouse.
            foreach (Button boton in ObtenerBotonesMenu())
            {
                boton.MouseEnter += BotonMenu_MouseEnter;
                boton.MouseLeave += BotonMenu_MouseLeave;
            }
        }


        // ========================================================
        // Carga los datos del usuario autenticado en la interfaz.
        // ========================================================
        private void CargarDatosSesion()
        {
            lblPerfil.Text =
                $"{SesionActual.Perfil} del sistema";

            lblUsuario.Text =
                $"●     {SesionActual.NombreUsuario}";
        }


        // ========================================================
        // Aplica los permisos del usuario al menú principal.
        //
        // IMPORTANTE:
        // Los botones nunca se ocultan.
        //
        // Con permiso:
        // - visible;
        // - habilitado;
        // - apariencia normal.
        //
        // Sin permiso:
        // - visible;
        // - deshabilitado;
        // - apariencia gris.
        // ========================================================
        private void AplicarPermisosMenu()
        {
            // Inicio siempre está disponible.
            ConfigurarPermisoBoton(
                btnInicio,
                true
            );

            ConfigurarPermisoBoton(
                btnVentas,
                SesionActual.TienePermiso("VENTAS_VER")
            );

            ConfigurarPermisoBoton(
                btnClientes,
                SesionActual.TienePermiso("CLIENTES_VER")
            );

            ConfigurarPermisoBoton(
                btnProductos,
                SesionActual.TienePermiso("PRODUCTOS_VER")
            );

            ConfigurarPermisoBoton(
                btnUsuarios,
                SesionActual.TienePermiso("USUARIOS_VER")
            );

            // Cada perfil posee su propio permiso para reportes.
            bool puedeVerReportes =
                SesionActual.TienePermiso("REPORTES_ADMINISTRADOR") ||
                SesionActual.TienePermiso("REPORTES_GERENTE") ||
                SesionActual.TienePermiso("REPORTES_VENDEDOR");

            ConfigurarPermisoBoton(
                btnReportes,
                puedeVerReportes
            );
        }


        // ========================================================
        // Configura visualmente un botón dependiendo de si
        // el usuario posee o no permiso para utilizarlo.
        // ========================================================
        private void ConfigurarPermisoBoton(
            Button boton,
            bool tienePermiso)
        {
            // Nunca ocultamos botones para que el menú conserve
            // siempre la misma estructura.
            boton.Visible = true;

            boton.Enabled = tienePermiso;

            if (tienePermiso)
            {
                boton.BackColor =
                    Color.FromArgb(30, 31, 34);

                boton.ForeColor =
                    Color.White;

                boton.Cursor =
                    Cursors.Hand;
            }
            else
            {
                boton.BackColor =
                    Color.FromArgb(55, 56, 59);

                boton.ForeColor =
                    Color.FromArgb(130, 130, 130);

                boton.Cursor =
                    Cursors.Default;
            }
        }


        // ========================================================
        // Devuelve todos los botones principales del menú.
        // ========================================================
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


        // ========================================================
        // Marca visualmente el módulo actualmente seleccionado.
        //
        // Los botones sin permiso conservan su aspecto gris.
        // ========================================================
        private void SeleccionarBoton(Button boton)
        {
            // Un botón deshabilitado nunca puede seleccionarse.
            if (!boton.Enabled)
            {
                return;
            }

            botonActivo = boton;

            foreach (Button item in ObtenerBotonesMenu())
            {
                if (item.Enabled)
                {
                    item.BackColor =
                        Color.FromArgb(30, 31, 34);

                    item.ForeColor =
                        Color.White;

                    item.Font = new Font(
                        "Segoe UI",
                        9.5F,
                        FontStyle.Regular
                    );
                }
                else
                {
                    item.BackColor =
                        Color.FromArgb(55, 56, 59);

                    item.ForeColor =
                        Color.FromArgb(130, 130, 130);

                    item.Font = new Font(
                        "Segoe UI",
                        9.5F,
                        FontStyle.Regular
                    );
                }
            }

            // Resaltar solamente el botón seleccionado.
            boton.BackColor =
                Color.FromArgb(58, 59, 62);

            boton.ForeColor =
                Color.White;

            boton.Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Bold
            );
        }


        // ========================================================
        // Efecto visual al pasar el mouse sobre botones habilitados.
        // ========================================================
        private void BotonMenu_MouseEnter(
            object? sender,
            EventArgs e)
        {
            if (sender is Button boton &&
                boton.Enabled &&
                boton != botonActivo)
            {
                boton.BackColor =
                    Color.FromArgb(46, 47, 50);
            }
        }


        // ========================================================
        // Restaura el color al retirar el mouse.
        // ========================================================
        private void BotonMenu_MouseLeave(
            object? sender,
            EventArgs e)
        {
            if (sender is Button boton &&
                boton.Enabled &&
                boton != botonActivo)
            {
                boton.BackColor =
                    Color.FromArgb(30, 31, 34);
            }
        }


        // ========================================================
        // Cierra el formulario interno actualmente abierto.
        // ========================================================
        private void CerrarFormularioActivo()
        {
            if (formularioActivo == null)
            {
                return;
            }

            formularioActivo.Close();
            formularioActivo.Dispose();

            formularioActivo = null;
        }


        // ========================================================
        // Carga un formulario de módulo dentro de pnlContenido.
        //
        // Este método será utilizado posteriormente por:
        // - FormVentas
        // - FormClientes
        // - FormProductos
        // - FormUsuarios
        // - FormReportes
        // ========================================================
        public void AbrirFormularioEnPanel(
            Form formulario,
            Button botonOrigen)
        {
            // Como protección adicional, no abrir un módulo
            // correspondiente a un botón sin permisos.
            if (!botonOrigen.Enabled)
            {
                formulario.Dispose();
                return;
            }

            CerrarFormularioActivo();

            SeleccionarBoton(botonOrigen);

            // Ocultar la bienvenida mientras existe
            // un módulo abierto.
            lblBienvenida.Visible = false;
            lblDescripcion.Visible = false;

            formularioActivo = formulario;

            formulario.TopLevel = false;

            formulario.FormBorderStyle =
                FormBorderStyle.None;

            formulario.Dock =
                DockStyle.Fill;

            pnlContenido.Controls.Add(formulario);

            formulario.Show();
            formulario.BringToFront();
        }


        // ========================================================
        // Muestra la pantalla inicial del sistema.
        // ========================================================
        private void MostrarInicio()
        {
            CerrarFormularioActivo();

            SeleccionarBoton(btnInicio);

            lblBienvenida.Text =
                $"Bienvenido, {SesionActual.Nombre}";

            lblDescripcion.Text =
                "Seleccione una opción del menú para comenzar.";

            lblBienvenida.Visible = true;
            lblDescripcion.Visible = true;

            lblBienvenida.BringToFront();
            lblDescripcion.BringToFront();
        }


        // ========================================================
        // Muestra temporalmente el nombre de un módulo mientras
        // todavía no existe su formulario definitivo.
        //
        // Más adelante será reemplazado por FormVentas,
        // FormClientes, etc.
        // ========================================================
        private void MostrarModuloTemporal(
            string titulo,
            string descripcion,
            Button boton)
        {
            // Protección adicional de permisos.
            if (!boton.Enabled)
            {
                return;
            }

            CerrarFormularioActivo();

            SeleccionarBoton(boton);

            lblBienvenida.Text = titulo;
            lblDescripcion.Text = descripcion;

            lblBienvenida.Visible = true;
            lblDescripcion.Visible = true;

            lblBienvenida.BringToFront();
            lblDescripcion.BringToFront();
        }


        // ========================================================
        // EVENTOS DEL MENÚ
        // ========================================================

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
            MostrarModuloTemporal(
                "Ventas",
                "Gestión de ventas y operaciones comerciales.",
                btnVentas
            );
        }


        private void BtnClientes_Click(
            object? sender,
            EventArgs e)
        {
            MostrarModuloTemporal(
                "Clientes",
                "Gestión y consulta de clientes.",
                btnClientes
            );
        }


        private void BtnProductos_Click(
            object? sender,
            EventArgs e)
        {
            MostrarModuloTemporal(
                "Productos",
                "Catálogo, productos e inventario.",
                btnProductos
            );
        }


        private void BtnUsuarios_Click(
            object? sender,
            EventArgs e)
        {
            MostrarModuloTemporal(
                "Usuarios",
                "Administración de usuarios y accesos.",
                btnUsuarios
            );
        }


        private void BtnReportes_Click(
            object? sender,
            EventArgs e)
        {
            MostrarModuloTemporal(
                "Reportes",
                "Reportes y estadísticas del sistema.",
                btnReportes
            );
        }


        // ========================================================
        // Edición futura del perfil del usuario.
        // ========================================================
        private void BtnEditarPerfil_Click(
            object? sender,
            EventArgs e)
        {
            MessageBox.Show(
                "La edición del perfil se implementará posteriormente.",
                "Editar perfil",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        // ========================================================
        // Cierra la sesión actual.
        //
        // FormLogin detectará que SesionActual fue cerrada
        // y volverá a mostrarse.
        // ========================================================
        private void BtnCerrarSesion_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult respuesta =
                MessageBox.Show(
                    "¿Desea cerrar la sesión actual?",
                    "Cerrar sesión",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            SesionActual.Cerrar();

            Close();
        }
    }
}