namespace Capa_Vistas
{
    partial class FormPrincipal
    {
        /// <summary>
        /// Variable requerida por el diseñador.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberar los recursos utilizados.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Inicializa todos los controles visuales de FormPrincipal.
        /// La lógica de navegación se mantiene en FormPrincipal.cs.
        /// </summary>
        private void InitializeComponent()
        {
            pnlCabecera = new Panel();
            lblPerfil = new Label();
            lblMarca = new Label();

            pnlCuerpo = new Panel();

            pnlMenu = new Panel();

            btnInicio = new Button();
            btnVentas = new Button();
            btnClientes = new Button();
            btnProductos = new Button();
            btnUsuarios = new Button();
            btnReportes = new Button();

            pnlUsuario = new Panel();
            lblUsuario = new Label();
            btnEditarPerfil = new Button();
            btnCerrarSesion = new Button();

            pnlContenido = new Panel();
            lblBienvenida = new Label();
            lblDescripcion = new Label();

            pnlCabecera.SuspendLayout();
            pnlCuerpo.SuspendLayout();
            pnlMenu.SuspendLayout();
            pnlUsuario.SuspendLayout();
            pnlContenido.SuspendLayout();

            SuspendLayout();

            // ====================================================
            // pnlCabecera
            // ====================================================
            pnlCabecera.BackColor = Color.FromArgb(20, 21, 23);
            pnlCabecera.Controls.Add(lblPerfil);
            pnlCabecera.Controls.Add(lblMarca);
            pnlCabecera.Dock = DockStyle.Top;
            pnlCabecera.Location = new Point(0, 0);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(1200, 70);
            pnlCabecera.TabIndex = 0;

            // ====================================================
            // lblMarca
            // ====================================================
            lblMarca.AutoSize = true;
            lblMarca.Font = new Font(
                "Segoe UI",
                17F,
                FontStyle.Bold,
                GraphicsUnit.Point
            );
            lblMarca.ForeColor = Color.White;
            lblMarca.Location = new Point(25, 18);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(214, 40);
            lblMarca.TabIndex = 0;
            lblMarca.Text = "HIERRO Y FORJA";

            // ====================================================
            // lblPerfil
            // ====================================================
            lblPerfil.Dock = DockStyle.Right;
            lblPerfil.Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Regular,
                GraphicsUnit.Point
            );
            lblPerfil.ForeColor = Color.White;
            lblPerfil.Location = new Point(900, 0);
            lblPerfil.Name = "lblPerfil";
            lblPerfil.Padding = new Padding(0, 0, 25, 0);
            lblPerfil.Size = new Size(300, 70);
            lblPerfil.TabIndex = 1;
            lblPerfil.Text = "Administrador del sistema";
            lblPerfil.TextAlign = ContentAlignment.MiddleRight;


            // ====================================================
            // pnlCuerpo
            //
            // Contiene menú lateral y panel central.
            // ====================================================
            pnlCuerpo.BackColor = Color.FromArgb(245, 245, 245);
            pnlCuerpo.Controls.Add(pnlContenido);
            pnlCuerpo.Controls.Add(pnlMenu);
            pnlCuerpo.Dock = DockStyle.Fill;
            pnlCuerpo.Location = new Point(0, 70);
            pnlCuerpo.Name = "pnlCuerpo";
            pnlCuerpo.Size = new Size(1200, 680);
            pnlCuerpo.TabIndex = 1;


            // ====================================================
            // pnlMenu
            // ====================================================
            pnlMenu.BackColor = Color.FromArgb(30, 31, 34);

            pnlMenu.Controls.Add(btnReportes);
            pnlMenu.Controls.Add(btnUsuarios);
            pnlMenu.Controls.Add(btnProductos);
            pnlMenu.Controls.Add(btnClientes);
            pnlMenu.Controls.Add(btnVentas);
            pnlMenu.Controls.Add(btnInicio);

            pnlMenu.Controls.Add(pnlUsuario);

            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Location = new Point(0, 0);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(185, 680);
            pnlMenu.TabIndex = 0;


            // ====================================================
            // btnInicio
            // ====================================================
            btnInicio.BackColor = Color.FromArgb(58, 59, 62);
            btnInicio.Cursor = Cursors.Hand;
            btnInicio.Dock = DockStyle.Top;
            btnInicio.FlatAppearance.BorderSize = 0;
            btnInicio.FlatStyle = FlatStyle.Flat;

            btnInicio.Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Bold,
                GraphicsUnit.Point
            );

            btnInicio.ForeColor = Color.White;
            btnInicio.Location = new Point(0, 0);
            btnInicio.Name = "btnInicio";
            btnInicio.Size = new Size(185, 54);
            btnInicio.TabIndex = 0;
            btnInicio.Tag = "";
            btnInicio.Text = "⌂     INICIO";
            btnInicio.UseVisualStyleBackColor = false;


            // ====================================================
            // btnVentas
            // ====================================================
            btnVentas.BackColor = Color.FromArgb(30, 31, 34);
            btnVentas.Cursor = Cursors.Hand;
            btnVentas.Dock = DockStyle.Top;
            btnVentas.FlatAppearance.BorderSize = 0;
            btnVentas.FlatStyle = FlatStyle.Flat;

            btnVentas.Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Regular,
                GraphicsUnit.Point
            );

            btnVentas.ForeColor = Color.White;
            btnVentas.Location = new Point(0, 54);
            btnVentas.Name = "btnVentas";
            btnVentas.Size = new Size(185, 54);
            btnVentas.TabIndex = 1;
            btnVentas.Tag = "VENTAS_VER";
            btnVentas.Text = "$     VENTAS";
            btnVentas.UseVisualStyleBackColor = false;


            // ====================================================
            // btnClientes
            // ====================================================
            btnClientes.BackColor = Color.FromArgb(30, 31, 34);
            btnClientes.Cursor = Cursors.Hand;
            btnClientes.Dock = DockStyle.Top;
            btnClientes.FlatAppearance.BorderSize = 0;
            btnClientes.FlatStyle = FlatStyle.Flat;

            btnClientes.Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Regular,
                GraphicsUnit.Point
            );

            btnClientes.ForeColor = Color.White;
            btnClientes.Location = new Point(0, 108);
            btnClientes.Name = "btnClientes";
            btnClientes.Size = new Size(185, 54);
            btnClientes.TabIndex = 2;
            btnClientes.Tag = "CLIENTES_VER";
            btnClientes.Text = "◎     CLIENTES";
            btnClientes.UseVisualStyleBackColor = false;


            // ====================================================
            // btnProductos
            // ====================================================
            btnProductos.BackColor = Color.FromArgb(30, 31, 34);
            btnProductos.Cursor = Cursors.Hand;
            btnProductos.Dock = DockStyle.Top;
            btnProductos.FlatAppearance.BorderSize = 0;
            btnProductos.FlatStyle = FlatStyle.Flat;

            btnProductos.Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Regular,
                GraphicsUnit.Point
            );

            btnProductos.ForeColor = Color.White;
            btnProductos.Location = new Point(0, 162);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(185, 54);
            btnProductos.TabIndex = 3;
            btnProductos.Tag = "PRODUCTOS_VER";
            btnProductos.Text = "▦     PRODUCTOS";
            btnProductos.UseVisualStyleBackColor = false;


            // ====================================================
            // btnUsuarios
            // ====================================================
            btnUsuarios.BackColor = Color.FromArgb(30, 31, 34);
            btnUsuarios.Cursor = Cursors.Hand;
            btnUsuarios.Dock = DockStyle.Top;
            btnUsuarios.FlatAppearance.BorderSize = 0;
            btnUsuarios.FlatStyle = FlatStyle.Flat;

            btnUsuarios.Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Regular,
                GraphicsUnit.Point
            );

            btnUsuarios.ForeColor = Color.White;
            btnUsuarios.Location = new Point(0, 216);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(185, 54);
            btnUsuarios.TabIndex = 4;
            btnUsuarios.Tag = "USUARIOS_VER";
            btnUsuarios.Text = "♙     USUARIOS";
            btnUsuarios.UseVisualStyleBackColor = false;


            // ====================================================
            // btnReportes
            // ====================================================
            btnReportes.BackColor = Color.FromArgb(30, 31, 34);
            btnReportes.Cursor = Cursors.Hand;
            btnReportes.Dock = DockStyle.Top;
            btnReportes.FlatAppearance.BorderSize = 0;
            btnReportes.FlatStyle = FlatStyle.Flat;

            btnReportes.Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Regular,
                GraphicsUnit.Point
            );

            btnReportes.ForeColor = Color.White;
            btnReportes.Location = new Point(0, 270);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(185, 54);
            btnReportes.TabIndex = 5;
            btnReportes.Text = "▤     REPORTES";
            btnReportes.UseVisualStyleBackColor = false;


            // ====================================================
            // pnlUsuario
            //
            // Información del usuario en la zona inferior.
            // ====================================================
            pnlUsuario.BackColor = Color.FromArgb(24, 25, 27);
            pnlUsuario.Controls.Add(btnCerrarSesion);
            pnlUsuario.Controls.Add(btnEditarPerfil);
            pnlUsuario.Controls.Add(lblUsuario);
            pnlUsuario.Dock = DockStyle.Bottom;
            pnlUsuario.Location = new Point(0, 495);
            pnlUsuario.Name = "pnlUsuario";
            pnlUsuario.Size = new Size(185, 185);
            pnlUsuario.TabIndex = 6;


            // ====================================================
            // lblUsuario
            // ====================================================
            lblUsuario.Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Bold,
                GraphicsUnit.Point
            );

            lblUsuario.ForeColor = Color.White;
            lblUsuario.Location = new Point(0, 10);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(185, 50);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "●     admin";
            lblUsuario.TextAlign = ContentAlignment.MiddleCenter;


            // ====================================================
            // btnEditarPerfil
            // ====================================================
            btnEditarPerfil.BackColor = Color.FromArgb(45, 46, 49);
            btnEditarPerfil.Cursor = Cursors.Hand;
            btnEditarPerfil.FlatAppearance.BorderSize = 0;
            btnEditarPerfil.FlatStyle = FlatStyle.Flat;

            btnEditarPerfil.Font = new Font(
                "Segoe UI",
                9F,
                FontStyle.Regular,
                GraphicsUnit.Point
            );

            btnEditarPerfil.ForeColor = Color.FromArgb(220, 220, 220);
            btnEditarPerfil.Location = new Point(20, 65);
            btnEditarPerfil.Name = "btnEditarPerfil";
            btnEditarPerfil.Size = new Size(145, 34);
            btnEditarPerfil.TabIndex = 1;
            btnEditarPerfil.Text = "Editar perfil";
            btnEditarPerfil.UseVisualStyleBackColor = false;


            // ====================================================
            // btnCerrarSesion
            // ====================================================
            btnCerrarSesion.BackColor = Color.FromArgb(24, 25, 27);
            btnCerrarSesion.Cursor = Cursors.Hand;
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;

            btnCerrarSesion.Font = new Font(
                "Segoe UI",
                9.5F,
                FontStyle.Regular,
                GraphicsUnit.Point
            );

            btnCerrarSesion.ForeColor = Color.White;
            btnCerrarSesion.Location = new Point(20, 115);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(145, 40);
            btnCerrarSesion.TabIndex = 2;
            btnCerrarSesion.Text = "↩     Cerrar sesión";
            btnCerrarSesion.UseVisualStyleBackColor = false;


            // ====================================================
            // pnlContenido
            //
            // Única zona del programa que cambia según el módulo.
            // ====================================================
            pnlContenido.BackColor = Color.FromArgb(245, 245, 245);
            pnlContenido.Controls.Add(lblDescripcion);
            pnlContenido.Controls.Add(lblBienvenida);
            pnlContenido.Dock = DockStyle.Fill;
            pnlContenido.Location = new Point(185, 0);
            pnlContenido.Name = "pnlContenido";
            pnlContenido.Size = new Size(1015, 680);
            pnlContenido.TabIndex = 1;


            // ====================================================
            // lblBienvenida
            //
            // Contenido temporal de Inicio.
            // ====================================================
            lblBienvenida.Anchor = AnchorStyles.None;

            lblBienvenida.AutoSize = true;

            lblBienvenida.Font = new Font(
                "Segoe UI",
                25F,
                FontStyle.Bold,
                GraphicsUnit.Point
            );

            lblBienvenida.ForeColor = Color.FromArgb(45, 45, 45);
            lblBienvenida.Location = new Point(338, 275);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(339, 57);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Bienvenido, Usuario";


            // ====================================================
            // lblDescripcion
            // ====================================================
            lblDescripcion.Anchor = AnchorStyles.None;

            lblDescripcion.AutoSize = true;

            lblDescripcion.Font = new Font(
                "Segoe UI",
                11F,
                FontStyle.Regular,
                GraphicsUnit.Point
            );

            lblDescripcion.ForeColor = Color.FromArgb(100, 100, 100);
            lblDescripcion.Location = new Point(337, 346);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(341, 25);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text =
                "Seleccione una opción del menú para comenzar.";


            // ====================================================
            // FormPrincipal
            // ====================================================
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;

            BackColor = Color.FromArgb(245, 245, 245);

            ClientSize = new Size(1200, 750);

            Controls.Add(pnlCuerpo);
            Controls.Add(pnlCabecera);

            MinimumSize = new Size(1100, 700);

            Name = "FormPrincipal";

            StartPosition = FormStartPosition.CenterScreen;

            Text = "Hierro y Forja - Sistema de Gestión";

            WindowState = FormWindowState.Maximized;


            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();

            pnlCuerpo.ResumeLayout(false);

            pnlMenu.ResumeLayout(false);

            pnlUsuario.ResumeLayout(false);

            pnlContenido.ResumeLayout(false);
            pnlContenido.PerformLayout();

            ResumeLayout(false);
        }

        #endregion


        // ========================================================
        // CONTROLES DE LA CABECERA
        // ========================================================
        private Panel pnlCabecera;
        private Label lblMarca;
        private Label lblPerfil;


        // ========================================================
        // ESTRUCTURA GENERAL
        // ========================================================
        private Panel pnlCuerpo;
        private Panel pnlMenu;
        private Panel pnlContenido;


        // ========================================================
        // MENÚ PRINCIPAL
        // ========================================================
        private Button btnInicio;
        private Button btnVentas;
        private Button btnClientes;
        private Button btnProductos;
        private Button btnUsuarios;
        private Button btnReportes;


        // ========================================================
        // ZONA DE USUARIO
        // ========================================================
        private Panel pnlUsuario;
        private Label lblUsuario;
        private Button btnEditarPerfil;
        private Button btnCerrarSesion;


        // ========================================================
        // CONTENIDO INICIAL
        // ========================================================
        private Label lblBienvenida;
        private Label lblDescripcion;
    }
}