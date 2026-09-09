using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(
            bool disposing)
        {
            if (
                disposing
                &&
                components != null)
            {
                components.Dispose();
            }


            base.Dispose(
                disposing
            );
        }


        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlCabecera = new Panel();
            picLogo = new PictureBox();
            picNombreMarca = new PictureBox();
            lblPerfil = new Label();
            lblFecha = new Label();
            btnMinimizar = new Button();
            btnMaximizar = new Button();
            btnCerrarPrograma = new Button();
            pnlLineaDorada = new Panel();
            pnlCuerpo = new Panel();
            pnlContenido = new Panel();
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
            ((System.ComponentModel.ISupportInitialize)picNombreMarca).BeginInit();
            pnlCuerpo.SuspendLayout();
            pnlMenu.SuspendLayout();
            pnlSucursal.SuspendLayout();
            pnlCuenta.SuspendLayout();
            SuspendLayout();
            // 
            // pnlCabecera
            // 
            pnlCabecera.BackColor = Color.FromArgb(17, 21, 26);
            pnlCabecera.Controls.Add(picLogo);
            pnlCabecera.Controls.Add(picNombreMarca);
            pnlCabecera.Controls.Add(lblPerfil);
            pnlCabecera.Controls.Add(lblFecha);
            pnlCabecera.Controls.Add(btnMinimizar);
            pnlCabecera.Controls.Add(btnMaximizar);
            pnlCabecera.Controls.Add(btnCerrarPrograma);
            pnlCabecera.Dock = DockStyle.Top;
            pnlCabecera.Location = new Point(0, 0);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Size = new Size(1400, 105);
            pnlCabecera.TabIndex = 0;
            // 
            // picLogo
            // 
            picLogo.BackColor = Color.Transparent;
            picLogo.Image = Properties.Resource.IconoHierroForja;
            picLogo.Location = new Point(24, 3);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(108, 96);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // picNombreMarca
            // 
            picNombreMarca.BackColor = Color.Transparent;
            picNombreMarca.Image = Properties.Resource.LogoNombreHierroForja;
            picNombreMarca.Location = new Point(157, -23);
            picNombreMarca.Name = "picNombreMarca";
            picNombreMarca.Size = new Size(494, 143);
            picNombreMarca.SizeMode = PictureBoxSizeMode.Zoom;
            picNombreMarca.TabIndex = 1;
            picNombreMarca.TabStop = false;
            // 
            // lblPerfil
            // 
            lblPerfil.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPerfil.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblPerfil.ForeColor = Color.FromArgb(190, 137, 45);
            lblPerfil.Location = new Point(1178, 36);
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
            lblFecha.Location = new Point(1048, 60);
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
            pnlLineaDorada.Location = new Point(0, 105);
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
            pnlCuerpo.Location = new Point(0, 108);
            pnlCuerpo.Name = "pnlCuerpo";
            pnlCuerpo.Size = new Size(1400, 712);
            pnlCuerpo.TabIndex = 2;
            // 
            // pnlContenido
            // 
            pnlContenido.BackColor = Color.FromArgb(245, 246, 248);
            pnlContenido.Dock = DockStyle.Fill;
            pnlContenido.Location = new Point(165, 0);
            pnlContenido.Name = "pnlContenido";
            pnlContenido.Size = new Size(1235, 712);
            pnlContenido.TabIndex = 1;
            // 
            // pnlMenu
            // 
            pnlMenu.AutoScroll = true;
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
            pnlMenu.Size = new Size(165, 712);
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
            btnInicio.Image = Properties.Resource.IconoInicio;
            btnInicio.ImageAlign = ContentAlignment.TopCenter;
            btnInicio.Location = new Point(0, 76);
            btnInicio.Name = "btnInicio";
            btnInicio.Padding = new Padding(0, 4, 0, 5);
            btnInicio.Size = new Size(165, 92);
            btnInicio.TabIndex = 1;
            btnInicio.Text = "INICIO";
            btnInicio.TextAlign = ContentAlignment.BottomCenter;
            btnInicio.UseVisualStyleBackColor = false;
            // 
            // btnVentas
            // 
            btnVentas.BackColor = Color.FromArgb(17, 21, 26);
            btnVentas.Cursor = Cursors.Hand;
            btnVentas.FlatAppearance.BorderSize = 0;
            btnVentas.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnVentas.FlatStyle = FlatStyle.Flat;
            btnVentas.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnVentas.ForeColor = Color.White;
            btnVentas.Image = Properties.Resource.IconoVentas;
            btnVentas.ImageAlign = ContentAlignment.TopCenter;
            btnVentas.Location = new Point(0, 168);
            btnVentas.Name = "btnVentas";
            btnVentas.Padding = new Padding(0, 4, 0, 5);
            btnVentas.Size = new Size(165, 92);
            btnVentas.TabIndex = 2;
            btnVentas.Tag = "VENTAS_VER";
            btnVentas.Text = "VENTAS";
            btnVentas.TextAlign = ContentAlignment.BottomCenter;
            btnVentas.UseVisualStyleBackColor = false;
            // 
            // btnClientes
            // 
            btnClientes.BackColor = Color.FromArgb(17, 21, 26);
            btnClientes.Cursor = Cursors.Hand;
            btnClientes.FlatAppearance.BorderSize = 0;
            btnClientes.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnClientes.FlatStyle = FlatStyle.Flat;
            btnClientes.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnClientes.ForeColor = Color.White;
            btnClientes.Image = Properties.Resource.IconoClientes;
            btnClientes.ImageAlign = ContentAlignment.TopCenter;
            btnClientes.Location = new Point(0, 260);
            btnClientes.Name = "btnClientes";
            btnClientes.Padding = new Padding(0, 4, 0, 5);
            btnClientes.Size = new Size(165, 92);
            btnClientes.TabIndex = 3;
            btnClientes.Tag = "CLIENTES_VER";
            btnClientes.Text = "CLIENTES";
            btnClientes.TextAlign = ContentAlignment.BottomCenter;
            btnClientes.UseVisualStyleBackColor = false;
            // 
            // btnProductos
            // 
            btnProductos.BackColor = Color.FromArgb(17, 21, 26);
            btnProductos.Cursor = Cursors.Hand;
            btnProductos.FlatAppearance.BorderSize = 0;
            btnProductos.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnProductos.FlatStyle = FlatStyle.Flat;
            btnProductos.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnProductos.ForeColor = Color.White;
            btnProductos.Image = Properties.Resource.IconoProductos;
            btnProductos.ImageAlign = ContentAlignment.TopCenter;
            btnProductos.Location = new Point(0, 352);
            btnProductos.Name = "btnProductos";
            btnProductos.Padding = new Padding(0, 4, 0, 5);
            btnProductos.Size = new Size(165, 92);
            btnProductos.TabIndex = 4;
            btnProductos.Tag = "PRODUCTOS_VER";
            btnProductos.Text = "PRODUCTOS";
            btnProductos.TextAlign = ContentAlignment.BottomCenter;
            btnProductos.UseVisualStyleBackColor = false;
            // 
            // btnUsuarios
            // 
            btnUsuarios.BackColor = Color.FromArgb(17, 21, 26);
            btnUsuarios.Cursor = Cursors.Hand;
            btnUsuarios.FlatAppearance.BorderSize = 0;
            btnUsuarios.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnUsuarios.ForeColor = Color.White;
            btnUsuarios.Image = Properties.Resource.IconoUsuarios;
            btnUsuarios.ImageAlign = ContentAlignment.TopCenter;
            btnUsuarios.Location = new Point(0, 444);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Padding = new Padding(0, 4, 0, 5);
            btnUsuarios.Size = new Size(165, 92);
            btnUsuarios.TabIndex = 5;
            btnUsuarios.Tag = "USUARIOS_VER";
            btnUsuarios.Text = "USUARIOS";
            btnUsuarios.TextAlign = ContentAlignment.BottomCenter;
            btnUsuarios.UseVisualStyleBackColor = false;
            // 
            // btnReportes
            // 
            btnReportes.BackColor = Color.FromArgb(17, 21, 26);
            btnReportes.Cursor = Cursors.Hand;
            btnReportes.FlatAppearance.BorderSize = 0;
            btnReportes.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 35, 40);
            btnReportes.FlatStyle = FlatStyle.Flat;
            btnReportes.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnReportes.ForeColor = Color.White;
            btnReportes.Image = Properties.Resource.IconoReportes;
            btnReportes.ImageAlign = ContentAlignment.TopCenter;
            btnReportes.Location = new Point(0, 536);
            btnReportes.Name = "btnReportes";
            btnReportes.Padding = new Padding(0, 4, 0, 5);
            btnReportes.Size = new Size(165, 92);
            btnReportes.TabIndex = 6;
            btnReportes.Text = "REPORTES";
            btnReportes.TextAlign = ContentAlignment.BottomCenter;
            btnReportes.UseVisualStyleBackColor = false;
            // 
            // pnlCuenta
            // 
            pnlCuenta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCuenta.BackColor = Color.FromArgb(17, 21, 26);
            pnlCuenta.Controls.Add(pnlSeparadorCuenta);
            pnlCuenta.Controls.Add(lblUsuarioActual);
            pnlCuenta.Controls.Add(btnEditarPerfil);
            pnlCuenta.Controls.Add(btnCerrarSesion);
            pnlCuenta.Location = new Point(0, 720);
            pnlCuenta.Name = "pnlCuenta";
            pnlCuenta.Size = new Size(165, 150);
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
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)picNombreMarca).EndInit();
            pnlCuerpo.ResumeLayout(false);
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

        private PictureBox picNombreMarca;

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
    }
}