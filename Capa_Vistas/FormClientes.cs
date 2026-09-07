using System;
using System.Windows.Forms;
using Capa_Datos;
using Capa_Logica;

namespace Capa_Vistas
{
    public partial class FormClientes : Form
    {
        private readonly ClienteLogica clienteLogica = new ClienteLogica();

        public FormClientes()
        {
            InitializeComponent();
            ConfigurarPermisos();
            CargarGrilla();
        }

        private void ConfigurarPermisos()
        {
            btnAlta.Enabled = SesionActual.TienePermiso("CLIENTES_ALTA");
            btnBaja.Enabled = SesionActual.TienePermiso("CLIENTES_BAJA");
        }

        private void CargarGrilla()
        {
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = clienteLogica.ObtenerActivos();
        }

        private void BtnAlta_Click(object? sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string documento = txtDocumento.Text;
            string correo = txtCorreo.Text;
            string telefono = txtTelefono.Text;

            string? error = clienteLogica.ValidarAlta(nombre, apellido, documento);
            if (error != null)
            {
                MessageBox.Show(error, "Alta de cliente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                clienteLogica.Alta(nombre, apellido, documento, correo, telefono);
                LimpiarFormulario();
                CargarGrilla();

                MessageBox.Show(
                    "Cliente agregado correctamente.",
                    "Alta de cliente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo agregar el cliente: " + ex.Message,
                    "Alta de cliente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnBaja_Click(object? sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Seleccioná un cliente de la grilla antes de dar de baja.",
                    "Baja de cliente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (dgvClientes.SelectedRows[0].DataBoundItem is not ClienteInfo cliente)
            {
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                $"¿Confirmás dar de baja a {cliente.Nombre} {cliente.Apellido}?",
                "Baja de cliente",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                clienteLogica.Baja(cliente.IdCliente);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo dar de baja al cliente: " + ex.Message,
                    "Baja de cliente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDocumento.Clear();
            txtCorreo.Clear();
            txtTelefono.Clear();
        }
    }
}