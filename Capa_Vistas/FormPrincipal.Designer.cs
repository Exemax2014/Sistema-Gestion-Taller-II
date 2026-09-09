using Font = System.Drawing.Font;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

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
            lblPerfil = new Label();
            lblFecha = new Label();
            btnMinimizar = new Button();
            btnMaximizar = new Button();
            btnCerrarPrograma = new Button();
            pnlLineaDorada = new Panel();
            pnlCuerpo = new Panel();
            pnlContenido = new Panel();
            lblTituloInicio = new Label();
            pnlLineaTitulo = new Panel();
            lblBienvenida = new Label();
            lblDescripcion = new Label();
            pnlMenu = new Panel();
            pnlSucursal = new Panel();
            lblSucursalTitulo = new Label();
            lblSucursalActual = new Label();
            btnInicio = new Button();
            btnVentas = new Button();
            btnClientes = new Button();
            btnProductos = new Button();
            btnUsuarios = new Button();
            btnReportes = new Button();
            pnlCuenta = new Panel();
            pnlSeparadorCuenta = new Panel();
            lblUsuarioActual = new Label();
            btnEditarPerfil = new Button();
            btnCerrarSesion = new Button();
            pnlCabecera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlCuerpo.SuspendLayout();
            pnlContenido.SuspendLayout();
            pnlMenu.SuspendLayout();
            pnlSucursal.SuspendLayout();
            pnlCuenta.SuspendLayout();
            SuspendLayout();
            // 
            // pnlCabecera
            // 
            pnlCabecera.BackColor = Color.FromArgb(17, 21, 26);
            pnlCabecera.Controls.Add(picLogo);
            pnlCabecera.Controls.Add(lblMarca);
            pnlCabecera.Controls.Add(lblPerfil);
            pnlCabecera.Controls.Add(lblFecha);
            pnlCabecera.Controls.Add(btnMinimizar);
            pnlCabecera.Controls.Add(btnMaximizar);
            pnlCabecera.Controls.Add(btnCerrarPrograma);
            pnlCabecera.Dock = DockStyle.Top;
            pnlCabecera.Location = new Point(0, 0);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(1400, 96);
            pnlCabecera.TabIndex = 0;
            // 
            // picLogo
            // 
            picLogo.BackColor = Color.Transparent;
            picLogo.Image = Properties.Resource.IconoHierroForja;
            picLogo.Location = new Point(45, 12);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(66, 66);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // lblMarca
            // 
            lblMarca.AutoSize = true;
            lblMarca.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
            lblMarca.ForeColor = Color.White;
            lblMarca.Location = new Point(159, 15);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(274, 45);
            lblMarca.TabIndex = 1;
            lblMarca.Text = "HIERRO Y FORJA";
            // 
            // lblPerfil
            // 
            lblPerfil.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPerfil.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblPerfil.ForeColor = Color.FromArgb(190, 137, 45);
            lblPerfil.Location = new Point(1178, 33);
            lblPerfil.Name = "lblPerfil";
            lblPerfil.Size = new Size(210, 23);
            lblPerfil.TabIndex = 2;
            lblPerfil.Text = "Administrador";
            lblPerfil.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblFecha
            // 
            lblFecha.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblFecha.Font = new Font("Segoe UI", 8.5F);
            lblFecha.ForeColor = Color.FromArgb(157, 162, 169);
            lblFecha.Location = new Point(1048, 56);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(340, 21);
            lblFecha.TabIndex = 3;
            lblFecha.Text = "Martes, 8 de septiembre de 2026 · 13:31";
            lblFecha.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnMinimizar
            // 
            btnMinimizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimizar.BackColor = Color.Transparent;
            btnMinimizar.Cursor = Cursors.Hand;
            btnMinimizar.FlatAppearance.BorderSize = 0;
            btnMinimizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 44, 49);
            btnMinimizar.FlatStyle = FlatStyle.Flat;
            btnMinimizar.Font = new Font("Segoe UI", 10F);
            btnMinimizar.ForeColor = Color.White;
            btnMinimizar.Location = new Point(1274, 0);
            btnMinimizar.Name = "btnMinimizar";
            btnMinimizar.Size = new Size(42, 30);
            btnMinimizar.TabIndex = 4;
            btnMinimizar.Text = "—";
            btnMinimizar.UseVisualStyleBackColor = false;
            // 
            // btnMaximizar
            // 
            btnMaximizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMaximizar.BackColor = Color.Transparent;
            btnMaximizar.Cursor = Cursors.Hand;
            btnMaximizar.FlatAppearance.BorderSize = 0;
            btnMaximizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 44, 49);
            btnMaximizar.FlatStyle = FlatStyle.Flat;
            btnMaximizar.Font = new Font("Segoe UI", 10F);
            btnMaximizar.ForeColor = Color.White;
            btnMaximizar.Location = new Point(1316, 0);
            btnMaximizar.Name = "btnMaximizar";
            btnMaximizar.Size = new Size(42, 30);
            btnMaximizar.TabIndex = 5;
            btnMaximizar.Text = "□";
            btnMaximizar.UseVisualStyleBackColor = false;
            // 
            // btnCerrarPrograma
            // 
            btnCerrarPrograma.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrarPrograma.BackColor = Color.Transparent;
            btnCerrarPrograma.Cursor = Cursors.Hand;
            btnCerrarPrograma.FlatAppearance.BorderSize = 0;
            btnCerrarPrograma.FlatAppearance.MouseOverBackColor = Color.FromArgb(175, 45, 45);
            btnCerrarPrograma.FlatStyle = FlatStyle.Flat;
            btnCerrarPrograma.Font = new Font("Segoe UI", 10F);
            btnCerrarPrograma.ForeColor = Color.White;
            btnCerrarPrograma.Location = new Point(1358, 0);
            btnCerrarPrograma.Name = "btnCerrarPrograma";
            btnCerrarPrograma.Size = new Size(42, 30);
            btnCerrarPrograma.TabIndex = 6;
            btnCerrarPrograma.Text = "×";
            btnCerrarPrograma.UseVisualStyleBackColor = false;
            // 
            // pnlLineaDorada
            // 
            pnlLineaDorada.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaDorada.Dock = DockStyle.Top;
            pnlLineaDorada.Location = new Point(0, 96);
            pnlLineaDorada.Name = "pnlLineaDorada";
            pnlLineaDorada.Size = new Size(1400, 3);
            pnlLineaDorada.TabIndex = 1;
            // 
            // pnlCuerpo
            // 
            pnlCuerpo.BackColor = Color.FromArgb(245, 246, 248);
            pnlCuerpo.Controls.Add(pnlContenido);
            pnlCuerpo.Controls.Add(pnlMenu);
            pnlCuerpo.Dock = DockStyle.Fill;
            pnlCuerpo.Location = new Point(0, 99);
            pnlCuerpo.Name = "pnlCuerpo";
            pnlCuerpo.Size = new Size(1400, 721);
            pnlCuerpo.TabIndex = 2;
            // 
            // pnlContenido
            // 
            pnlContenido.BackColor = Color.FromArgb(245, 246, 248);
            pnlContenido.Controls.Add(lblTituloInicio);
            pnlContenido.Controls.Add(pnlLineaTitulo);
            pnlContenido.Controls.Add(lblBienvenida);
            pnlContenido.Controls.Add(lblDescripcion);
            pnlContenido.Dock = DockStyle.Fill;
            pnlContenido.Location = new Point(159, 0);
            pnlContenido.Name = "pnlContenido";
            pnlContenido.Size = new Size(1241, 721);
            pnlContenido.TabIndex = 1;
            // 
            // lblTituloInicio
            // 
            lblTituloInicio.AutoSize = true;
            lblTituloInicio.Font = new Font("Segoe UI", 21F, FontStyle.Bold);
            lblTituloInicio.ForeColor = Color.FromArgb(27, 34, 42);
            lblTituloInicio.Location = new Point(48, 38);
            lblTituloInicio.Name = "lblTituloInicio";
            lblTituloInicio.Size = new Size(110, 47);
            lblTituloInicio.TabIndex = 0;
            lblTituloInicio.Text = "Inicio";
            // 
            // pnlLineaTitulo
            // 
            pnlLineaTitulo.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaTitulo.Location = new Point(50, 88);
            pnlLineaTitulo.Name = "pnlLineaTitulo";
            pnlLineaTitulo.Size = new Size(72, 3);
            pnlLineaTitulo.TabIndex = 1;
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblBienvenida.ForeColor = Color.FromArgb(27, 34, 42);
            lblBienvenida.Location = new Point(48, 150);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(259, 60);
            lblBienvenida.TabIndex = 2;
            lblBienvenida.Text = "Bienvenido";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 10.5F);
            lblDescripcion.ForeColor = Color.FromArgb(95, 102, 110);
            lblDescripcion.Location = new Point(51, 213);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(393, 25);
            lblDescripcion.TabIndex = 3;
            lblDescripcion.Text = "Seleccione una opción del menú para comenzar.";
            // 
            // pnlMenu
            // 
            pnlMenu.BackColor = Color.FromArgb(17, 21, 26);
            pnlMenu.Controls.Add(pnlSucursal);
            pnlMenu.Controls.Add(btnInicio);
            pnlMenu.Controls.Add(btnVentas);
            pnlMenu.Controls.Add(btnClientes);
            pnlMenu.Controls.Add(btnProductos);
            pnlMenu.Controls.Add(btnUsuarios);
            pnlMenu.Controls.Add(btnReportes);
            pnlMenu.Controls.Add(pnlCuenta);
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Location = new Point(0, 0);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(159, 721);
            pnlMenu.TabIndex = 0;
            // 
            // pnlSucursal
            // 
            pnlSucursal.BackColor = Color.FromArgb(17, 21, 26);
            pnlSucursal.Controls.Add(lblSucursalTitulo);
            pnlSucursal.Controls.Add(lblSucursalActual);
            pnlSucursal.Location = new Point(0, 0);
            pnlSucursal.Name = "pnlSucursal";
            pnlSucursal.Size = new Size(165, 72);
            pnlSucursal.TabIndex = 0;
            // 
            // lblSucursalTitulo
            // 
            lblSucursalTitulo.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblSucursalTitulo.ForeColor = Color.FromArgb(190, 137, 45);
            lblSucursalTitulo.Location = new Point(14, 12);
            lblSucursalTitulo.Name = "lblSucursalTitulo";
            lblSucursalTitulo.Size = new Size(137, 18);
            lblSucursalTitulo.TabIndex = 0;
            lblSucursalTitulo.Text = "SUCURSAL";
            // 
            // lblSucursalActual
            // 
            lblSucursalActual.Font = new Font("Segoe UI", 8.5F);
            lblSucursalActual.ForeColor = Color.FromArgb(215, 218, 222);
            lblSucursalActual.Location = new Point(14, 33);
            lblSucursalActual.Name = "lblSucursalActual";
            lblSucursalActual.Size = new Size(137, 25);
            lblSucursalActual.TabIndex = 1;
            lblSucursalActual.Text = "Todas las sucursales";
            // 
            // btnInicio
            // 
            btnInicio.BackColor = Color.FromArgb(72, 53, 24);
            btnInicio.Cursor = Cursors.Hand;
            btnInicio.FlatAppearance.BorderSize = 0;
            btnInicio.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnInicio.FlatStyle = FlatStyle.Flat;
            btnInicio.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnInicio.ForeColor = Color.White;
            btnInicio.Location = new Point(0, 72);
            btnInicio.Name = "btnInicio";
            btnInicio.Size = new Size(165, 72);
            btnInicio.TabIndex = 1;
            btnInicio.Text = "⌂\r\nINICIO";
            btnInicio.UseVisualStyleBackColor = false;
            // 
            // btnVentas
            // 
            btnVentas.BackColor = Color.FromArgb(17, 21, 26);
            btnVentas.Cursor = Cursors.Hand;
            btnVentas.FlatAppearance.BorderSize = 0;
            btnVentas.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnVentas.FlatStyle = FlatStyle.Flat;
            btnVentas.Font = new Font("Segoe UI", 8.5F);
            btnVentas.ForeColor = Color.White;
            btnVentas.Location = new Point(0, 144);
            btnVentas.Name = "btnVentas";
            btnVentas.Size = new Size(165, 72);
            btnVentas.TabIndex = 2;
            btnVentas.Tag = "VENTAS_VER";
            btnVentas.Text = "\U0001f6d2\r\nVENTAS";
            btnVentas.UseVisualStyleBackColor = false;
            // 
            // btnClientes
            // 
            btnClientes.BackColor = Color.FromArgb(17, 21, 26);
            btnClientes.Cursor = Cursors.Hand;
            btnClientes.FlatAppearance.BorderSize = 0;
            btnClientes.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnClientes.FlatStyle = FlatStyle.Flat;
            btnClientes.Font = new Font("Segoe UI", 8.5F);
            btnClientes.ForeColor = Color.White;
            btnClientes.Location = new Point(0, 216);
            btnClientes.Name = "btnClientes";
            btnClientes.Size = new Size(165, 72);
            btnClientes.TabIndex = 3;
            btnClientes.Tag = "CLIENTES_VER";
            btnClientes.Text = "♟\r\nCLIENTES";
            btnClientes.UseVisualStyleBackColor = false;
            // 
            // btnProductos
            // 
            btnProductos.BackColor = Color.FromArgb(17, 21, 26);
            btnProductos.Cursor = Cursors.Hand;
            btnProductos.FlatAppearance.BorderSize = 0;
            btnProductos.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnProductos.FlatStyle = FlatStyle.Flat;
            btnProductos.Font = new Font("Segoe UI", 8.5F);
            btnProductos.ForeColor = Color.White;
            btnProductos.Location = new Point(0, 288);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(165, 72);
            btnProductos.TabIndex = 4;
            btnProductos.Tag = "PRODUCTOS_VER";
            btnProductos.Text = "◇\r\nPRODUCTOS";
            btnProductos.UseVisualStyleBackColor = false;
            // 
            // btnUsuarios
            // 
            btnUsuarios.BackColor = Color.FromArgb(17, 21, 26);
            btnUsuarios.Cursor = Cursors.Hand;
            btnUsuarios.FlatAppearance.BorderSize = 0;
            btnUsuarios.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.Font = new Font("Segoe UI", 8.5F);
            btnUsuarios.ForeColor = Color.White;
            btnUsuarios.Location = new Point(0, 360);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(165, 72);
            btnUsuarios.TabIndex = 5;
            btnUsuarios.Tag = "USUARIOS_VER";
            btnUsuarios.Text = "♙\r\nUSUARIOS";
            btnUsuarios.UseVisualStyleBackColor = false;
            // 
            // btnReportes
            // 
            btnReportes.BackColor = Color.FromArgb(17, 21, 26);
            btnReportes.Cursor = Cursors.Hand;
            btnReportes.FlatAppearance.BorderSize = 0;
            btnReportes.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnReportes.FlatStyle = FlatStyle.Flat;
            btnReportes.Font = new Font("Segoe UI", 8.5F);
            btnReportes.ForeColor = Color.White;
            btnReportes.Location = new Point(0, 432);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(165, 72);
            btnReportes.TabIndex = 6;
            btnReportes.Text = "▥\r\nREPORTES";
            btnReportes.UseVisualStyleBackColor = false;
            // 
            // pnlCuenta
            // 
            pnlCuenta.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlCuenta.BackColor = Color.FromArgb(17, 21, 26);
            pnlCuenta.Controls.Add(pnlSeparadorCuenta);
            pnlCuenta.Controls.Add(lblUsuarioActual);
            pnlCuenta.Controls.Add(btnEditarPerfil);
            pnlCuenta.Controls.Add(btnCerrarSesion);
            pnlCuenta.Location = new Point(0, 571);
            pnlCuenta.Name = "pnlCuenta";
            pnlCuenta.Size = new Size(159, 150);
            pnlCuenta.TabIndex = 7;
            // 
            // pnlSeparadorCuenta
            // 
            pnlSeparadorCuenta.BackColor = Color.FromArgb(48, 52, 57);
            pnlSeparadorCuenta.Location = new Point(12, 0);
            pnlSeparadorCuenta.Name = "pnlSeparadorCuenta";
            pnlSeparadorCuenta.Size = new Size(141, 1);
            pnlSeparadorCuenta.TabIndex = 0;
            // 
            // lblUsuarioActual
            // 
            lblUsuarioActual.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUsuarioActual.ForeColor = Color.FromArgb(190, 137, 45);
            lblUsuarioActual.Location = new Point(14, 13);
            lblUsuarioActual.Name = "lblUsuarioActual";
            lblUsuarioActual.Size = new Size(137, 25);
            lblUsuarioActual.TabIndex = 1;
            lblUsuarioActual.Text = "ADMIN";
            // 
            // btnEditarPerfil
            // 
            btnEditarPerfil.BackColor = Color.FromArgb(17, 21, 26);
            btnEditarPerfil.Cursor = Cursors.Hand;
            btnEditarPerfil.FlatAppearance.BorderSize = 0;
            btnEditarPerfil.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnEditarPerfil.FlatStyle = FlatStyle.Flat;
            btnEditarPerfil.Font = new Font("Segoe UI", 8.5F);
            btnEditarPerfil.ForeColor = Color.FromArgb(215, 218, 222);
            btnEditarPerfil.Location = new Point(0, 45);
            btnEditarPerfil.Name = "btnEditarPerfil";
            btnEditarPerfil.Padding = new Padding(14, 0, 0, 0);
            btnEditarPerfil.Size = new Size(165, 40);
            btnEditarPerfil.TabIndex = 2;
            btnEditarPerfil.Text = "Editar perfil";
            btnEditarPerfil.TextAlign = ContentAlignment.MiddleLeft;
            btnEditarPerfil.UseVisualStyleBackColor = false;
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.BackColor = Color.FromArgb(17, 21, 26);
            btnCerrarSesion.Cursor = Cursors.Hand;
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 31, 32);
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnCerrarSesion.ForeColor = Color.FromArgb(210, 95, 95);
            btnCerrarSesion.Location = new Point(0, 86);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Padding = new Padding(14, 0, 0, 0);
            btnCerrarSesion.Size = new Size(165, 40);
            btnCerrarSesion.TabIndex = 3;
            btnCerrarSesion.Text = "Cerrar sesión";
            btnCerrarSesion.TextAlign = ContentAlignment.MiddleLeft;
            btnCerrarSesion.UseVisualStyleBackColor = false;
            // 
            // FormPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1400, 820);
            Controls.Add(pnlCuerpo);
            Controls.Add(pnlLineaDorada);
            Controls.Add(pnlCabecera);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(1050, 650);
            Name = "FormPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hierro y Forja";
            WindowState = FormWindowState.Maximized;
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlCuerpo.ResumeLayout(false);
            pnlContenido.ResumeLayout(false);
            pnlContenido.PerformLayout();
            pnlMenu.ResumeLayout(false);
            pnlSucursal.ResumeLayout(false);
            pnlCuenta.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion


        // =========================================================
        // CONTROLES
        // =========================================================

        private Panel pnlCabecera;

        private PictureBox picLogo;
        private Label lblMarca;

        private Label lblPerfil;
        private Label lblFecha;

        private Button btnMinimizar;
        private Button btnMaximizar;
        private Button btnCerrarPrograma;

        private Panel pnlLineaDorada;

        private Panel pnlCuerpo;

        private Panel pnlMenu;

        private Panel pnlSucursal;
        private Label lblSucursalTitulo;
        private Label lblSucursalActual;

        private Button btnInicio;
        private Button btnVentas;
        private Button btnClientes;
        private Button btnProductos;
        private Button btnUsuarios;
        private Button btnReportes;

        private Panel pnlCuenta;
        private Panel pnlSeparadorCuenta;

        private Label lblUsuarioActual;

        private Button btnEditarPerfil;
        private Button btnCerrarSesion;

        private Panel pnlContenido;

        private Label lblTituloInicio;
        private Panel pnlLineaTitulo;
        private Label lblBienvenida;
        private Label lblDescripcion;
    }
}