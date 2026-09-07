namespace Capa_Vistas
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlCabecera = new Panel();
            picLogo = new PictureBox();
            lblMarca = new Label();

            btnMinimizar = new Button();
            btnMaximizar = new Button();
            btnCerrarPrograma = new Button();

            lblPerfil = new Label();
            lblFecha = new Label();
            btnUsuarioMenu = new Button();

            pnlLineaDorada = new Panel();

            pnlCuerpo = new Panel();
            pnlMenu = new Panel();

            btnInicio = new Button();
            btnVentas = new Button();
            btnClientes = new Button();
            btnProductos = new Button();
            btnUsuarios = new Button();
            btnReportes = new Button();

            pnlContenido = new Panel();
            lblTituloInicio = new Label();
            pnlLineaTitulo = new Panel();
            lblBienvenida = new Label();
            lblDescripcion = new Label();

            pnlMenuUsuario = new Panel();
            lblUsuario = new Label();
            pnlSeparadorUsuario = new Panel();
            btnEditarPerfil = new Button();
            btnCerrarSesion = new Button();

            pnlCabecera.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)picLogo)
                .BeginInit();

            pnlCuerpo.SuspendLayout();
            pnlMenu.SuspendLayout();
            pnlContenido.SuspendLayout();
            pnlMenuUsuario.SuspendLayout();

            SuspendLayout();

            // =====================================================
            // FORM PRINCIPAL
            // =====================================================

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;

            BackColor = Color.FromArgb(247, 248, 250);

            ClientSize = new Size(1400, 820);

            Controls.Add(pnlMenuUsuario);
            Controls.Add(pnlCuerpo);
            Controls.Add(pnlLineaDorada);
            Controls.Add(pnlCabecera);

            FormBorderStyle = FormBorderStyle.None;

            MinimumSize = new Size(1050, 650);

            Name = "FormPrincipal";

            StartPosition = FormStartPosition.CenterScreen;

            Text = "Hierro y Forja";

            WindowState = FormWindowState.Maximized;

            // =====================================================
            // CABECERA
            // =====================================================

            pnlCabecera.BackColor =
                Color.FromArgb(17, 21, 26);

            pnlCabecera.Controls.Add(picLogo);
            pnlCabecera.Controls.Add(lblMarca);

            pnlCabecera.Controls.Add(lblPerfil);
            pnlCabecera.Controls.Add(lblFecha);
            pnlCabecera.Controls.Add(btnUsuarioMenu);

            pnlCabecera.Controls.Add(btnMinimizar);
            pnlCabecera.Controls.Add(btnMaximizar);
            pnlCabecera.Controls.Add(btnCerrarPrograma);

            pnlCabecera.Dock = DockStyle.Top;

            pnlCabecera.Name = "pnlCabecera";

            pnlCabecera.Size = new Size(1400, 100);

            pnlCabecera.TabIndex = 0;

            // =====================================================
            // LOGO
            // =====================================================

            picLogo.BackColor =
                Color.Transparent;

            picLogo.Image =
                Properties.Resource.IconoHierroForja;

            picLogo.Location =
                new Point(18, 12);

            picLogo.Name =
                "picLogo";

            picLogo.Size =
                new Size(72, 72);

            picLogo.SizeMode =
                PictureBoxSizeMode.Zoom;

            picLogo.TabIndex =
                0;

            picLogo.TabStop =
                false;

            // =====================================================
            // NOMBRE
            // =====================================================

            lblMarca.AutoSize =
                true;

            lblMarca.Font =
                new Font(
                    "Segoe UI",
                    20F,
                    FontStyle.Bold);

            lblMarca.ForeColor =
                Color.White;

            lblMarca.Location =
                new Point(103, 30);

            lblMarca.Name =
                "lblMarca";

            lblMarca.Text =
                "HIERRO Y FORJA";

            // =====================================================
            // MINIMIZAR
            // =====================================================

            btnMinimizar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnMinimizar.BackColor =
                Color.Transparent;

            btnMinimizar.Cursor =
                Cursors.Hand;

            btnMinimizar.FlatAppearance.BorderSize =
                0;

            btnMinimizar.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(45, 48, 52);

            btnMinimizar.FlatStyle =
                FlatStyle.Flat;

            btnMinimizar.Font =
                new Font(
                    "Segoe UI",
                    10F);

            btnMinimizar.ForeColor =
                Color.White;

            btnMinimizar.Location =
                new Point(1275, 0);

            btnMinimizar.Name =
                "btnMinimizar";

            btnMinimizar.Size =
                new Size(40, 30);

            btnMinimizar.Text =
                "—";

            btnMinimizar.UseVisualStyleBackColor =
                false;

            // =====================================================
            // MAXIMIZAR / RESTAURAR
            // =====================================================

            btnMaximizar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnMaximizar.BackColor =
                Color.Transparent;

            btnMaximizar.Cursor =
                Cursors.Hand;

            btnMaximizar.FlatAppearance.BorderSize =
                0;

            btnMaximizar.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(45, 48, 52);

            btnMaximizar.FlatStyle =
                FlatStyle.Flat;

            btnMaximizar.Font =
                new Font(
                    "Segoe UI",
                    10F);

            btnMaximizar.ForeColor =
                Color.White;

            btnMaximizar.Location =
                new Point(1317, 0);

            btnMaximizar.Name =
                "btnMaximizar";

            btnMaximizar.Size =
                new Size(40, 30);

            btnMaximizar.Text =
                "□";

            btnMaximizar.UseVisualStyleBackColor =
                false;

            // =====================================================
            // CERRAR
            // =====================================================

            btnCerrarPrograma.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnCerrarPrograma.BackColor =
                Color.Transparent;

            btnCerrarPrograma.Cursor =
                Cursors.Hand;

            btnCerrarPrograma.FlatAppearance.BorderSize =
                0;

            btnCerrarPrograma.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(175, 45, 45);

            btnCerrarPrograma.FlatStyle =
                FlatStyle.Flat;

            btnCerrarPrograma.Font =
                new Font(
                    "Segoe UI",
                    10F);

            btnCerrarPrograma.ForeColor =
                Color.White;

            btnCerrarPrograma.Location =
                new Point(1360, 0);

            btnCerrarPrograma.Name =
                "btnCerrarPrograma";

            btnCerrarPrograma.Size =
                new Size(40, 30);

            btnCerrarPrograma.Text =
                "×";

            btnCerrarPrograma.UseVisualStyleBackColor =
                false;

            // =====================================================
            // PERFIL
            // =====================================================

            lblPerfil.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            lblPerfil.Font =
                new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Bold);

            lblPerfil.ForeColor =
                Color.White;

            lblPerfil.Location =
                new Point(920, 34);

            lblPerfil.Name =
                "lblPerfil";

            lblPerfil.Size =
                new Size(215, 24);

            lblPerfil.Text =
                "Administrador";

            lblPerfil.TextAlign =
                ContentAlignment.MiddleRight;

            // =====================================================
            // FECHA
            // =====================================================

            lblFecha.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            lblFecha.Font =
                new Font(
                    "Segoe UI",
                    8.5F);

            lblFecha.ForeColor =
                Color.FromArgb(180, 183, 188);

            lblFecha.Location =
                new Point(870, 59);

            lblFecha.Name =
                "lblFecha";

            lblFecha.Size =
                new Size(265, 22);

            lblFecha.Text =
                "Lunes, 7 de septiembre de 2026";

            lblFecha.TextAlign =
                ContentAlignment.MiddleRight;

            // =====================================================
            // BOTÓN USUARIO
            // =====================================================

            btnUsuarioMenu.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnUsuarioMenu.BackColor =
                Color.FromArgb(23, 27, 32);

            btnUsuarioMenu.Cursor =
                Cursors.Hand;

            btnUsuarioMenu.FlatAppearance.BorderColor =
                Color.FromArgb(190, 137, 45);

            btnUsuarioMenu.FlatAppearance.BorderSize =
                1;

            btnUsuarioMenu.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(35, 39, 44);

            btnUsuarioMenu.FlatStyle =
                FlatStyle.Flat;

            btnUsuarioMenu.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold);

            btnUsuarioMenu.ForeColor =
                Color.White;

            btnUsuarioMenu.Location =
                new Point(1160, 40);

            btnUsuarioMenu.Name =
                "btnUsuarioMenu";

            btnUsuarioMenu.Padding =
                new Padding(12, 0, 8, 0);

            btnUsuarioMenu.Size =
                new Size(215, 44);

            btnUsuarioMenu.Text =
                "●   admin               ▼";

            btnUsuarioMenu.TextAlign =
                ContentAlignment.MiddleLeft;

            btnUsuarioMenu.UseVisualStyleBackColor =
                false;

            // =====================================================
            // LINEA DORADA GLOBAL
            // =====================================================

            pnlLineaDorada.BackColor =
                Color.FromArgb(190, 137, 45);

            pnlLineaDorada.Dock =
                DockStyle.Top;

            pnlLineaDorada.Name =
                "pnlLineaDorada";

            pnlLineaDorada.Size =
                new Size(1400, 3);

            // =====================================================
            // CUERPO
            // =====================================================

            pnlCuerpo.BackColor =
                Color.FromArgb(247, 248, 250);

            pnlCuerpo.Controls.Add(pnlContenido);
            pnlCuerpo.Controls.Add(pnlMenu);

            pnlCuerpo.Dock =
                DockStyle.Fill;

            pnlCuerpo.Name =
                "pnlCuerpo";

            // =====================================================
            // MENU LATERAL
            // =====================================================

            pnlMenu.BackColor =
                Color.FromArgb(17, 21, 26);

            pnlMenu.Controls.Add(btnReportes);
            pnlMenu.Controls.Add(btnUsuarios);
            pnlMenu.Controls.Add(btnProductos);
            pnlMenu.Controls.Add(btnClientes);
            pnlMenu.Controls.Add(btnVentas);
            pnlMenu.Controls.Add(btnInicio);

            pnlMenu.Dock =
                DockStyle.Left;

            pnlMenu.Name =
                "pnlMenu";

            pnlMenu.Size =
                new Size(105, 717);

            // =====================================================
            // INICIO
            // =====================================================

            btnInicio.BackColor =
                Color.FromArgb(72, 53, 24);

            btnInicio.Cursor =
                Cursors.Hand;

            btnInicio.Dock =
                DockStyle.Top;

            btnInicio.FlatAppearance.BorderSize =
                0;

            btnInicio.FlatStyle =
                FlatStyle.Flat;

            btnInicio.Font =
                new Font(
                    "Segoe UI",
                    8.5F,
                    FontStyle.Bold);

            btnInicio.ForeColor =
                Color.White;

            btnInicio.Name =
                "btnInicio";

            btnInicio.Size =
                new Size(105, 94);

            btnInicio.Text =
                "⌂\r\n\r\nINICIO";

            btnInicio.TextAlign =
                ContentAlignment.MiddleCenter;

            btnInicio.UseVisualStyleBackColor =
                false;

            // =====================================================
            // VENTAS
            // =====================================================

            btnVentas.BackColor =
                Color.FromArgb(17, 21, 26);

            btnVentas.Cursor =
                Cursors.Hand;

            btnVentas.Dock =
                DockStyle.Top;

            btnVentas.FlatAppearance.BorderSize =
                0;

            btnVentas.FlatStyle =
                FlatStyle.Flat;

            btnVentas.Font =
                new Font(
                    "Segoe UI",
                    8.5F);

            btnVentas.ForeColor =
                Color.White;

            btnVentas.Name =
                "btnVentas";

            btnVentas.Size =
                new Size(105, 94);

            btnVentas.Tag =
                "VENTAS_VER";

            btnVentas.Text =
                "🛒\r\n\r\nVENTAS";

            btnVentas.TextAlign =
                ContentAlignment.MiddleCenter;

            btnVentas.UseVisualStyleBackColor =
                false;

            // =====================================================
            // CLIENTES
            // =====================================================

            btnClientes.BackColor =
                Color.FromArgb(17, 21, 26);

            btnClientes.Cursor =
                Cursors.Hand;

            btnClientes.Dock =
                DockStyle.Top;

            btnClientes.FlatAppearance.BorderSize =
                0;

            btnClientes.FlatStyle =
                FlatStyle.Flat;

            btnClientes.Font =
                new Font("Segoe UI", 8.5F);

            btnClientes.ForeColor =
                Color.White;

            btnClientes.Name =
                "btnClientes";

            btnClientes.Size =
                new Size(105, 94);

            btnClientes.Tag =
                "CLIENTES_VER";

            btnClientes.Text =
                "♟\r\n\r\nCLIENTES";

            btnClientes.TextAlign =
                ContentAlignment.MiddleCenter;

            btnClientes.UseVisualStyleBackColor =
                false;

            // =====================================================
            // PRODUCTOS
            // =====================================================

            btnProductos.BackColor =
                Color.FromArgb(17, 21, 26);

            btnProductos.Cursor =
                Cursors.Hand;

            btnProductos.Dock =
                DockStyle.Top;

            btnProductos.FlatAppearance.BorderSize =
                0;

            btnProductos.FlatStyle =
                FlatStyle.Flat;

            btnProductos.Font =
                new Font("Segoe UI", 8.5F);

            btnProductos.ForeColor =
                Color.White;

            btnProductos.Name =
                "btnProductos";

            btnProductos.Size =
                new Size(105, 94);

            btnProductos.Tag =
                "PRODUCTOS_VER";

            btnProductos.Text =
                "◇\r\n\r\nPRODUCTOS";

            btnProductos.TextAlign =
                ContentAlignment.MiddleCenter;

            btnProductos.UseVisualStyleBackColor =
                false;

            // =====================================================
            // USUARIOS
            // =====================================================

            btnUsuarios.BackColor =
                Color.FromArgb(17, 21, 26);

            btnUsuarios.Cursor =
                Cursors.Hand;

            btnUsuarios.Dock =
                DockStyle.Top;

            btnUsuarios.FlatAppearance.BorderSize =
                0;

            btnUsuarios.FlatStyle =
                FlatStyle.Flat;

            btnUsuarios.Font =
                new Font("Segoe UI", 8.5F);

            btnUsuarios.ForeColor =
                Color.White;

            btnUsuarios.Name =
                "btnUsuarios";

            btnUsuarios.Size =
                new Size(105, 94);

            btnUsuarios.Tag =
                "USUARIOS_VER";

            btnUsuarios.Text =
                "♙\r\n\r\nUSUARIOS";

            btnUsuarios.TextAlign =
                ContentAlignment.MiddleCenter;

            btnUsuarios.UseVisualStyleBackColor =
                false;

            // =====================================================
            // REPORTES
            // =====================================================

            btnReportes.BackColor =
                Color.FromArgb(17, 21, 26);

            btnReportes.Cursor =
                Cursors.Hand;

            btnReportes.Dock =
                DockStyle.Top;

            btnReportes.FlatAppearance.BorderSize =
                0;

            btnReportes.FlatStyle =
                FlatStyle.Flat;

            btnReportes.Font =
                new Font("Segoe UI", 8.5F);

            btnReportes.ForeColor =
                Color.White;

            btnReportes.Name =
                "btnReportes";

            btnReportes.Size =
                new Size(105, 94);

            btnReportes.Text =
                "▥\r\n\r\nREPORTES";

            btnReportes.TextAlign =
                ContentAlignment.MiddleCenter;

            btnReportes.UseVisualStyleBackColor =
                false;

            // =====================================================
            // CONTENIDO
            // =====================================================

            pnlContenido.BackColor =
                Color.FromArgb(247, 248, 250);

            pnlContenido.Controls.Add(lblDescripcion);
            pnlContenido.Controls.Add(lblBienvenida);
            pnlContenido.Controls.Add(pnlLineaTitulo);
            pnlContenido.Controls.Add(lblTituloInicio);

            pnlContenido.Dock =
                DockStyle.Fill;

            pnlContenido.Name =
                "pnlContenido";

            // =====================================================
            // TITULO INICIO
            // =====================================================

            lblTituloInicio.AutoSize =
                true;

            lblTituloInicio.Font =
                new Font(
                    "Segoe UI",
                    22F,
                    FontStyle.Bold);

            lblTituloInicio.ForeColor =
                Color.FromArgb(27, 34, 42);

            lblTituloInicio.Location =
                new Point(50, 40);

            lblTituloInicio.Name =
                "lblTituloInicio";

            lblTituloInicio.Text =
                "Inicio";

            // =====================================================
            // LINEA TITULO
            // =====================================================

            pnlLineaTitulo.BackColor =
                Color.FromArgb(190, 137, 45);

            pnlLineaTitulo.Location =
                new Point(52, 91);

            pnlLineaTitulo.Name =
                "pnlLineaTitulo";

            pnlLineaTitulo.Size =
                new Size(80, 3);

            // =====================================================
            // BIENVENIDA
            // =====================================================

            lblBienvenida.AutoSize =
                true;

            lblBienvenida.Font =
                new Font(
                    "Segoe UI",
                    27F,
                    FontStyle.Bold);

            lblBienvenida.ForeColor =
                Color.FromArgb(27, 34, 42);

            lblBienvenida.Location =
                new Point(50, 155);

            lblBienvenida.Name =
                "lblBienvenida";

            lblBienvenida.Text =
                "Bienvenido";

            // =====================================================
            // DESCRIPCION
            // =====================================================

            lblDescripcion.AutoSize =
                true;

            lblDescripcion.Font =
                new Font(
                    "Segoe UI",
                    11F);

            lblDescripcion.ForeColor =
                Color.FromArgb(95, 102, 110);

            lblDescripcion.Location =
                new Point(53, 220);

            lblDescripcion.Name =
                "lblDescripcion";

            lblDescripcion.Text =
                "Seleccione una opción del menú para comenzar.";

            // =====================================================
            // MENU USUARIO
            // =====================================================

            pnlMenuUsuario.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            pnlMenuUsuario.BackColor =
                Color.White;

            pnlMenuUsuario.BorderStyle =
                BorderStyle.FixedSingle;

            pnlMenuUsuario.Controls.Add(btnCerrarSesion);
            pnlMenuUsuario.Controls.Add(pnlSeparadorUsuario);
            pnlMenuUsuario.Controls.Add(btnEditarPerfil);
            pnlMenuUsuario.Controls.Add(lblUsuario);

            pnlMenuUsuario.Location =
                new Point(1158, 84);

            pnlMenuUsuario.Name =
                "pnlMenuUsuario";

            pnlMenuUsuario.Size =
                new Size(217, 150);

            pnlMenuUsuario.TabIndex =
                10;

            pnlMenuUsuario.Visible =
                false;

            // =====================================================
            // NOMBRE USUARIO
            // =====================================================

            lblUsuario.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);

            lblUsuario.ForeColor =
                Color.FromArgb(30, 34, 39);

            lblUsuario.Location =
                new Point(15, 8);

            lblUsuario.Name =
                "lblUsuario";

            lblUsuario.Size =
                new Size(185, 34);

            lblUsuario.Text =
                "Administrador Sistema";

            lblUsuario.TextAlign =
                ContentAlignment.MiddleLeft;

            // =====================================================
            // EDITAR PERFIL
            // =====================================================

            btnEditarPerfil.BackColor =
                Color.White;

            btnEditarPerfil.Cursor =
                Cursors.Hand;

            btnEditarPerfil.FlatAppearance.BorderSize =
                0;

            btnEditarPerfil.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(245, 241, 232);

            btnEditarPerfil.FlatStyle =
                FlatStyle.Flat;

            btnEditarPerfil.Font =
                new Font("Segoe UI", 9F);

            btnEditarPerfil.ForeColor =
                Color.FromArgb(45, 45, 45);

            btnEditarPerfil.Location =
                new Point(0, 44);

            btnEditarPerfil.Name =
                "btnEditarPerfil";

            btnEditarPerfil.Padding =
                new Padding(15, 0, 0, 0);

            btnEditarPerfil.Size =
                new Size(215, 43);

            btnEditarPerfil.Text =
                "⚙   Editar perfil";

            btnEditarPerfil.TextAlign =
                ContentAlignment.MiddleLeft;

            // =====================================================
            // SEPARADOR
            // =====================================================

            pnlSeparadorUsuario.BackColor =
                Color.FromArgb(220, 220, 220);

            pnlSeparadorUsuario.Location =
                new Point(14, 89);

            pnlSeparadorUsuario.Name =
                "pnlSeparadorUsuario";

            pnlSeparadorUsuario.Size =
                new Size(187, 1);

            // =====================================================
            // CERRAR SESION
            // =====================================================

            btnCerrarSesion.BackColor =
                Color.White;

            btnCerrarSesion.Cursor =
                Cursors.Hand;

            btnCerrarSesion.FlatAppearance.BorderSize =
                0;

            btnCerrarSesion.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(250, 242, 242);

            btnCerrarSesion.FlatStyle =
                FlatStyle.Flat;

            btnCerrarSesion.Font =
                new Font("Segoe UI", 9F);

            btnCerrarSesion.ForeColor =
                Color.FromArgb(150, 48, 48);

            btnCerrarSesion.Location =
                new Point(0, 96);

            btnCerrarSesion.Name =
                "btnCerrarSesion";

            btnCerrarSesion.Padding =
                new Padding(15, 0, 0, 0);

            btnCerrarSesion.Size =
                new Size(215, 43);

            btnCerrarSesion.Text =
                "↩   Cerrar sesión";

            btnCerrarSesion.TextAlign =
                ContentAlignment.MiddleLeft;

            // =====================================================
            // FINAL
            // =====================================================

            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)picLogo)
                .EndInit();

            pnlCuerpo.ResumeLayout(false);
            pnlMenu.ResumeLayout(false);

            pnlContenido.ResumeLayout(false);
            pnlContenido.PerformLayout();

            pnlMenuUsuario.ResumeLayout(false);

            ResumeLayout(false);
        }

        #endregion

        private Panel pnlCabecera;
        private PictureBox picLogo;
        private Label lblMarca;

        private Button btnMinimizar;
        private Button btnMaximizar;
        private Button btnCerrarPrograma;

        private Label lblPerfil;
        private Label lblFecha;
        private Button btnUsuarioMenu;

        private Panel pnlLineaDorada;

        private Panel pnlCuerpo;
        private Panel pnlMenu;
        private Panel pnlContenido;

        private Button btnInicio;
        private Button btnVentas;
        private Button btnClientes;
        private Button btnProductos;
        private Button btnUsuarios;
        private Button btnReportes;

        private Label lblTituloInicio;
        private Panel pnlLineaTitulo;
        private Label lblBienvenida;
        private Label lblDescripcion;

        private Panel pnlMenuUsuario;
        private Label lblUsuario;
        private Panel pnlSeparadorUsuario;
        private Button btnEditarPerfil;
        private Button btnCerrarSesion;
    }
}