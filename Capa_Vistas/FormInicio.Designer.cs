using Font = System.Drawing.Font;

namespace Capa_Vistas
{
    partial class FormInicio
    {
        private System.ComponentModel.IContainer components =
            null;


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
            pnlPrincipal = new Panel();
            pnlActividad = new Panel();
            lblActividadTitulo = new Label();
            lblActividadDescripcion = new Label();
            pnlActividadContenido = new Panel();
            lblActividadPlaceholder = new Label();
            pnlAccesos = new Panel();
            lblAccesosTitulo = new Label();
            lblAccesosDescripcion = new Label();
            btnNuevaVenta = new Button();
            btnProductos = new Button();
            btnClientes = new Button();
            pnlTarjetaProductos = new Panel();
            lblProductosTitulo = new Label();
            lblProductosValor = new Label();
            lblProductosDescripcion = new Label();
            pnlTarjetaStock = new Panel();
            lblStockBajoTitulo = new Label();
            lblStockBajoValor = new Label();
            lblStockBajoDescripcion = new Label();
            pnlTarjetaIngresos = new Panel();
            lblIngresosHoyTitulo = new Label();
            lblIngresosHoyValor = new Label();
            lblIngresosHoyDescripcion = new Label();
            pnlTarjetaVentas = new Panel();
            lblVentasHoyTitulo = new Label();
            lblVentasHoyValor = new Label();
            lblVentasHoyDescripcion = new Label();
            lblBienvenida = new Label();
            lblPerfilSucursal = new Label();
            pnlEncabezado = new Panel();
            lblSubtitulo = new Label();
            pnlLineaDorada = new Panel();
            pnlPrincipal.SuspendLayout();
            pnlActividad.SuspendLayout();
            pnlActividadContenido.SuspendLayout();
            pnlAccesos.SuspendLayout();
            pnlTarjetaProductos.SuspendLayout();
            pnlTarjetaStock.SuspendLayout();
            pnlTarjetaIngresos.SuspendLayout();
            pnlTarjetaVentas.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            SuspendLayout();
            // 
            // pnlPrincipal
            // 
            pnlPrincipal.BackColor = Color.FromArgb(245, 246, 248);
            pnlPrincipal.Controls.Add(pnlActividad);
            pnlPrincipal.Controls.Add(pnlAccesos);
            pnlPrincipal.Controls.Add(pnlTarjetaProductos);
            pnlPrincipal.Controls.Add(pnlTarjetaStock);
            pnlPrincipal.Controls.Add(pnlTarjetaIngresos);
            pnlPrincipal.Controls.Add(pnlTarjetaVentas);
            pnlPrincipal.Controls.Add(pnlEncabezado);
            pnlPrincipal.Dock = DockStyle.Fill;
            pnlPrincipal.Location = new Point(0, 0);
            pnlPrincipal.Name = "pnlPrincipal";
            pnlPrincipal.Padding = new Padding(32);
            pnlPrincipal.Size = new Size(1180, 760);
            pnlPrincipal.TabIndex = 0;
            // 
            // pnlActividad
            // 
            pnlActividad.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlActividad.BackColor = Color.White;
            pnlActividad.BorderStyle = BorderStyle.FixedSingle;
            pnlActividad.Controls.Add(lblActividadTitulo);
            pnlActividad.Controls.Add(lblActividadDescripcion);
            pnlActividad.Controls.Add(pnlActividadContenido);
            pnlActividad.Location = new Point(32, 410);
            pnlActividad.Name = "pnlActividad";
            pnlActividad.Size = new Size(1116, 318);
            pnlActividad.TabIndex = 0;
            // 
            // lblActividadTitulo
            // 
            lblActividadTitulo.AutoSize = true;
            lblActividadTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblActividadTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblActividadTitulo.Location = new Point(18, 13);
            lblActividadTitulo.Name = "lblActividadTitulo";
            lblActividadTitulo.Size = new Size(167, 25);
            lblActividadTitulo.TabIndex = 0;
            lblActividadTitulo.Text = "Actividad reciente";
            // 
            // lblActividadDescripcion
            // 
            lblActividadDescripcion.AutoSize = true;
            lblActividadDescripcion.Font = new Font("Segoe UI", 8F);
            lblActividadDescripcion.ForeColor = Color.FromArgb(105, 110, 116);
            lblActividadDescripcion.Location = new Point(165, 18);
            lblActividadDescripcion.Name = "lblActividadDescripcion";
            lblActividadDescripcion.Size = new Size(206, 19);
            lblActividadDescripcion.TabIndex = 1;
            lblActividadDescripcion.Text = "Últimas operaciones registradas.";
            // 
            // pnlActividadContenido
            // 
            pnlActividadContenido.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlActividadContenido.BackColor = Color.FromArgb(248, 249, 250);
            pnlActividadContenido.Controls.Add(lblActividadPlaceholder);
            pnlActividadContenido.Location = new Point(18, 48);
            pnlActividadContenido.Name = "pnlActividadContenido";
            pnlActividadContenido.Size = new Size(1078, 249);
            pnlActividadContenido.TabIndex = 2;
            // 
            // lblActividadPlaceholder
            // 
            lblActividadPlaceholder.Dock = DockStyle.Fill;
            lblActividadPlaceholder.Font = new Font("Segoe UI", 9F);
            lblActividadPlaceholder.ForeColor = Color.FromArgb(130, 135, 140);
            lblActividadPlaceholder.Location = new Point(0, 0);
            lblActividadPlaceholder.Name = "lblActividadPlaceholder";
            lblActividadPlaceholder.Size = new Size(1078, 249);
            lblActividadPlaceholder.TabIndex = 0;
            lblActividadPlaceholder.Text = "La actividad reciente se mostrará aquí al conectar la capa lógica.";
            lblActividadPlaceholder.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlAccesos
            // 
            pnlAccesos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlAccesos.BackColor = Color.White;
            pnlAccesos.BorderStyle = BorderStyle.FixedSingle;
            pnlAccesos.Controls.Add(lblAccesosTitulo);
            pnlAccesos.Controls.Add(lblAccesosDescripcion);
            pnlAccesos.Controls.Add(btnNuevaVenta);
            pnlAccesos.Controls.Add(btnProductos);
            pnlAccesos.Controls.Add(btnClientes);
            pnlAccesos.Location = new Point(32, 262);
            pnlAccesos.Name = "pnlAccesos";
            pnlAccesos.Size = new Size(1116, 125);
            pnlAccesos.TabIndex = 1;
            // 
            // lblAccesosTitulo
            // 
            lblAccesosTitulo.AutoSize = true;
            lblAccesosTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblAccesosTitulo.ForeColor = Color.FromArgb(55, 59, 64);
            lblAccesosTitulo.Location = new Point(18, 13);
            lblAccesosTitulo.Name = "lblAccesosTitulo";
            lblAccesosTitulo.Size = new Size(148, 25);
            lblAccesosTitulo.TabIndex = 0;
            lblAccesosTitulo.Text = "Accesos rápidos";
            // 
            // lblAccesosDescripcion
            // 
            lblAccesosDescripcion.AutoSize = true;
            lblAccesosDescripcion.Font = new Font("Segoe UI", 8F);
            lblAccesosDescripcion.ForeColor = Color.FromArgb(105, 110, 116);
            lblAccesosDescripcion.Location = new Point(150, 18);
            lblAccesosDescripcion.Name = "lblAccesosDescripcion";
            lblAccesosDescripcion.Size = new Size(204, 19);
            lblAccesosDescripcion.TabIndex = 1;
            lblAccesosDescripcion.Text = "Acciones frecuentes del sistema.";
            // 
            // btnNuevaVenta
            // 
            btnNuevaVenta.BackColor = Color.FromArgb(190, 137, 45);
            btnNuevaVenta.Cursor = Cursors.Hand;
            btnNuevaVenta.FlatAppearance.BorderSize = 0;
            btnNuevaVenta.FlatStyle = FlatStyle.Flat;
            btnNuevaVenta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevaVenta.ForeColor = Color.White;
            btnNuevaVenta.Location = new Point(18, 60);
            btnNuevaVenta.Name = "btnNuevaVenta";
            btnNuevaVenta.Size = new Size(180, 42);
            btnNuevaVenta.TabIndex = 2;
            btnNuevaVenta.Text = "Nueva venta";
            btnNuevaVenta.UseVisualStyleBackColor = false;
            // 
            // btnProductos
            // 
            btnProductos.BackColor = Color.FromArgb(45, 49, 54);
            btnProductos.Cursor = Cursors.Hand;
            btnProductos.FlatAppearance.BorderSize = 0;
            btnProductos.FlatStyle = FlatStyle.Flat;
            btnProductos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnProductos.ForeColor = Color.White;
            btnProductos.Location = new Point(215, 60);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(180, 42);
            btnProductos.TabIndex = 3;
            btnProductos.Text = "Productos";
            btnProductos.UseVisualStyleBackColor = false;
            // 
            // btnClientes
            // 
            btnClientes.BackColor = Color.FromArgb(45, 49, 54);
            btnClientes.Cursor = Cursors.Hand;
            btnClientes.FlatAppearance.BorderSize = 0;
            btnClientes.FlatStyle = FlatStyle.Flat;
            btnClientes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnClientes.ForeColor = Color.White;
            btnClientes.Location = new Point(412, 60);
            btnClientes.Name = "btnClientes";
            btnClientes.Size = new Size(180, 42);
            btnClientes.TabIndex = 4;
            btnClientes.Text = "Clientes";
            btnClientes.UseVisualStyleBackColor = false;
            // 
            // pnlTarjetaProductos
            // 
            pnlTarjetaProductos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlTarjetaProductos.BackColor = Color.White;
            pnlTarjetaProductos.BorderStyle = BorderStyle.FixedSingle;
            pnlTarjetaProductos.Controls.Add(lblProductosTitulo);
            pnlTarjetaProductos.Controls.Add(lblProductosValor);
            pnlTarjetaProductos.Controls.Add(lblProductosDescripcion);
            pnlTarjetaProductos.Location = new Point(887, 128);
            pnlTarjetaProductos.Name = "pnlTarjetaProductos";
            pnlTarjetaProductos.Size = new Size(261, 110);
            pnlTarjetaProductos.TabIndex = 2;
            // 
            // lblProductosTitulo
            // 
            lblProductosTitulo.AutoSize = true;
            lblProductosTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProductosTitulo.ForeColor = Color.FromArgb(80, 85, 90);
            lblProductosTitulo.Location = new Point(16, 13);
            lblProductosTitulo.Name = "lblProductosTitulo";
            lblProductosTitulo.Size = new Size(80, 20);
            lblProductosTitulo.TabIndex = 0;
            lblProductosTitulo.Text = "Productos";
            // 
            // lblProductosValor
            // 
            lblProductosValor.AutoSize = true;
            lblProductosValor.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblProductosValor.ForeColor = Color.FromArgb(45, 49, 54);
            lblProductosValor.Location = new Point(14, 31);
            lblProductosValor.Name = "lblProductosValor";
            lblProductosValor.Size = new Size(44, 52);
            lblProductosValor.TabIndex = 1;
            lblProductosValor.Text = "0";
            // 
            // lblProductosDescripcion
            // 
            lblProductosDescripcion.AutoSize = true;
            lblProductosDescripcion.Font = new Font("Segoe UI", 8F);
            lblProductosDescripcion.ForeColor = Color.FromArgb(115, 120, 125);
            lblProductosDescripcion.Location = new Point(17, 82);
            lblProductosDescripcion.Name = "lblProductosDescripcion";
            lblProductosDescripcion.Size = new Size(117, 19);
            lblProductosDescripcion.TabIndex = 2;
            lblProductosDescripcion.Text = "Productos activos";
            // 
            // pnlTarjetaStock
            // 
            pnlTarjetaStock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlTarjetaStock.BackColor = Color.White;
            pnlTarjetaStock.BorderStyle = BorderStyle.FixedSingle;
            pnlTarjetaStock.Controls.Add(lblStockBajoTitulo);
            pnlTarjetaStock.Controls.Add(lblStockBajoValor);
            pnlTarjetaStock.Controls.Add(lblStockBajoDescripcion);
            pnlTarjetaStock.Location = new Point(603, 128);
            pnlTarjetaStock.Name = "pnlTarjetaStock";
            pnlTarjetaStock.Size = new Size(260, 110);
            pnlTarjetaStock.TabIndex = 3;
            // 
            // lblStockBajoTitulo
            // 
            lblStockBajoTitulo.AutoSize = true;
            lblStockBajoTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStockBajoTitulo.ForeColor = Color.FromArgb(80, 85, 90);
            lblStockBajoTitulo.Location = new Point(16, 13);
            lblStockBajoTitulo.Name = "lblStockBajoTitulo";
            lblStockBajoTitulo.Size = new Size(81, 20);
            lblStockBajoTitulo.TabIndex = 0;
            lblStockBajoTitulo.Text = "Stock bajo";
            // 
            // lblStockBajoValor
            // 
            lblStockBajoValor.AutoSize = true;
            lblStockBajoValor.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblStockBajoValor.ForeColor = Color.FromArgb(175, 65, 65);
            lblStockBajoValor.Location = new Point(14, 31);
            lblStockBajoValor.Name = "lblStockBajoValor";
            lblStockBajoValor.Size = new Size(44, 52);
            lblStockBajoValor.TabIndex = 1;
            lblStockBajoValor.Text = "0";
            // 
            // lblStockBajoDescripcion
            // 
            lblStockBajoDescripcion.AutoSize = true;
            lblStockBajoDescripcion.Font = new Font("Segoe UI", 8F);
            lblStockBajoDescripcion.ForeColor = Color.FromArgb(115, 120, 125);
            lblStockBajoDescripcion.Location = new Point(17, 82);
            lblStockBajoDescripcion.Name = "lblStockBajoDescripcion";
            lblStockBajoDescripcion.Size = new Size(135, 19);
            lblStockBajoDescripcion.TabIndex = 2;
            lblStockBajoDescripcion.Text = "Productos con alerta";
            // 
            // pnlTarjetaIngresos
            // 
            pnlTarjetaIngresos.Anchor = AnchorStyles.Top;
            pnlTarjetaIngresos.BackColor = Color.White;
            pnlTarjetaIngresos.BorderStyle = BorderStyle.FixedSingle;
            pnlTarjetaIngresos.Controls.Add(lblIngresosHoyTitulo);
            pnlTarjetaIngresos.Controls.Add(lblIngresosHoyValor);
            pnlTarjetaIngresos.Controls.Add(lblIngresosHoyDescripcion);
            pnlTarjetaIngresos.Location = new Point(317, 128);
            pnlTarjetaIngresos.Name = "pnlTarjetaIngresos";
            pnlTarjetaIngresos.Size = new Size(260, 110);
            pnlTarjetaIngresos.TabIndex = 4;
            // 
            // lblIngresosHoyTitulo
            // 
            lblIngresosHoyTitulo.AutoSize = true;
            lblIngresosHoyTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblIngresosHoyTitulo.ForeColor = Color.FromArgb(80, 85, 90);
            lblIngresosHoyTitulo.Location = new Point(16, 13);
            lblIngresosHoyTitulo.Name = "lblIngresosHoyTitulo";
            lblIngresosHoyTitulo.Size = new Size(120, 20);
            lblIngresosHoyTitulo.TabIndex = 0;
            lblIngresosHoyTitulo.Text = "Ingresos de hoy";
            // 
            // lblIngresosHoyValor
            // 
            lblIngresosHoyValor.AutoSize = true;
            lblIngresosHoyValor.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblIngresosHoyValor.ForeColor = Color.FromArgb(190, 137, 45);
            lblIngresosHoyValor.Location = new Point(14, 31);
            lblIngresosHoyValor.Name = "lblIngresosHoyValor";
            lblIngresosHoyValor.Size = new Size(132, 52);
            lblIngresosHoyValor.TabIndex = 1;
            lblIngresosHoyValor.Text = "$ 0,00";
            // 
            // lblIngresosHoyDescripcion
            // 
            lblIngresosHoyDescripcion.AutoSize = true;
            lblIngresosHoyDescripcion.Font = new Font("Segoe UI", 8F);
            lblIngresosHoyDescripcion.ForeColor = Color.FromArgb(115, 120, 125);
            lblIngresosHoyDescripcion.Location = new Point(17, 82);
            lblIngresosHoyDescripcion.Name = "lblIngresosHoyDescripcion";
            lblIngresosHoyDescripcion.Size = new Size(91, 19);
            lblIngresosHoyDescripcion.TabIndex = 2;
            lblIngresosHoyDescripcion.Text = "Total vendido";
            // 
            // pnlTarjetaVentas
            // 
            pnlTarjetaVentas.BackColor = Color.White;
            pnlTarjetaVentas.BorderStyle = BorderStyle.FixedSingle;
            pnlTarjetaVentas.Controls.Add(lblVentasHoyTitulo);
            pnlTarjetaVentas.Controls.Add(lblVentasHoyValor);
            pnlTarjetaVentas.Controls.Add(lblVentasHoyDescripcion);
            pnlTarjetaVentas.Location = new Point(32, 128);
            pnlTarjetaVentas.Name = "pnlTarjetaVentas";
            pnlTarjetaVentas.Size = new Size(260, 110);
            pnlTarjetaVentas.TabIndex = 5;
            // 
            // lblVentasHoyTitulo
            // 
            lblVentasHoyTitulo.AutoSize = true;
            lblVentasHoyTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVentasHoyTitulo.ForeColor = Color.FromArgb(80, 85, 90);
            lblVentasHoyTitulo.Location = new Point(16, 13);
            lblVentasHoyTitulo.Name = "lblVentasHoyTitulo";
            lblVentasHoyTitulo.Size = new Size(107, 20);
            lblVentasHoyTitulo.TabIndex = 0;
            lblVentasHoyTitulo.Text = "Ventas de hoy";
            // 
            // lblVentasHoyValor
            // 
            lblVentasHoyValor.AutoSize = true;
            lblVentasHoyValor.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblVentasHoyValor.ForeColor = Color.FromArgb(45, 49, 54);
            lblVentasHoyValor.Location = new Point(14, 31);
            lblVentasHoyValor.Name = "lblVentasHoyValor";
            lblVentasHoyValor.Size = new Size(44, 52);
            lblVentasHoyValor.TabIndex = 1;
            lblVentasHoyValor.Text = "0";
            // 
            // lblVentasHoyDescripcion
            // 
            lblVentasHoyDescripcion.AutoSize = true;
            lblVentasHoyDescripcion.Font = new Font("Segoe UI", 8F);
            lblVentasHoyDescripcion.ForeColor = Color.FromArgb(115, 120, 125);
            lblVentasHoyDescripcion.Location = new Point(17, 82);
            lblVentasHoyDescripcion.Name = "lblVentasHoyDescripcion";
            lblVentasHoyDescripcion.Size = new Size(156, 19);
            lblVentasHoyDescripcion.TabIndex = 2;
            lblVentasHoyDescripcion.Text = "Operaciones registradas";
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblBienvenida.ForeColor = Color.FromArgb(45, 49, 54);
            lblBienvenida.Location = new Point(3, 8);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(172, 40);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Bienvenido";
            // 
            // lblPerfilSucursal
            // 
            lblPerfilSucursal.AutoSize = true;
            lblPerfilSucursal.Font = new Font("Segoe UI", 9F);
            lblPerfilSucursal.ForeColor = Color.FromArgb(105, 110, 116);
            lblPerfilSucursal.Location = new Point(883, 28);
            lblPerfilSucursal.Name = "lblPerfilSucursal";
            lblPerfilSucursal.Size = new Size(107, 20);
            lblPerfilSucursal.TabIndex = 1;
            lblPerfilSucursal.Text = "Perfil · Sucursal";
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlEncabezado.BackColor = Color.Transparent;
            pnlEncabezado.Controls.Add(lblPerfilSucursal);
            pnlEncabezado.Controls.Add(lblBienvenida);
            pnlEncabezado.Controls.Add(lblSubtitulo);
            pnlEncabezado.Controls.Add(pnlLineaDorada);
            pnlEncabezado.Location = new Point(32, 18);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(1116, 82);
            pnlEncabezado.TabIndex = 7;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(105, 110, 116);
            lblSubtitulo.Location = new Point(2, 48);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(205, 20);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Resumen general del sistema.";
            // 
            // pnlLineaDorada
            // 
            pnlLineaDorada.BackColor = Color.FromArgb(190, 137, 45);
            pnlLineaDorada.Location = new Point(2, 74);
            pnlLineaDorada.Name = "pnlLineaDorada";
            pnlLineaDorada.Size = new Size(95, 3);
            pnlLineaDorada.TabIndex = 2;
            // 
            // FormInicio
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1180, 760);
            Controls.Add(pnlPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormInicio";
            Text = "Inicio";
            pnlPrincipal.ResumeLayout(false);
            pnlActividad.ResumeLayout(false);
            pnlActividad.PerformLayout();
            pnlActividadContenido.ResumeLayout(false);
            pnlAccesos.ResumeLayout(false);
            pnlAccesos.PerformLayout();
            pnlTarjetaProductos.ResumeLayout(false);
            pnlTarjetaProductos.PerformLayout();
            pnlTarjetaStock.ResumeLayout(false);
            pnlTarjetaStock.PerformLayout();
            pnlTarjetaIngresos.ResumeLayout(false);
            pnlTarjetaIngresos.PerformLayout();
            pnlTarjetaVentas.ResumeLayout(false);
            pnlTarjetaVentas.PerformLayout();
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            ResumeLayout(false);
        }

        #endregion


        // =========================================================
        // CONTROLES VISUALES
        // =========================================================

        private Panel pnlPrincipal;

        private Panel pnlEncabezado;

        private Label lblSubtitulo;

        private Panel pnlLineaDorada;

        private Label lblBienvenida;

        private Label lblPerfilSucursal;


        private Panel pnlTarjetaVentas;

        private Label lblVentasHoyTitulo;

        private Label lblVentasHoyValor;

        private Label lblVentasHoyDescripcion;


        private Panel pnlTarjetaIngresos;

        private Label lblIngresosHoyTitulo;

        private Label lblIngresosHoyValor;

        private Label lblIngresosHoyDescripcion;


        private Panel pnlTarjetaStock;

        private Label lblStockBajoTitulo;

        private Label lblStockBajoValor;

        private Label lblStockBajoDescripcion;


        private Panel pnlTarjetaProductos;

        private Label lblProductosTitulo;

        private Label lblProductosValor;

        private Label lblProductosDescripcion;


        private Panel pnlAccesos;

        private Label lblAccesosTitulo;

        private Label lblAccesosDescripcion;

        private Button btnNuevaVenta;

        private Button btnProductos;

        private Button btnClientes;


        private Panel pnlActividad;

        private Label lblActividadTitulo;

        private Label lblActividadDescripcion;

        private Panel pnlActividadContenido;

        private Label lblActividadPlaceholder;
    }
}