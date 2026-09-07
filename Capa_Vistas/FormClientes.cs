using System;
using System.Windows.Forms;
using Capa_Datos;
using Capa_Logica;

namespace Capa_Vistas
{
    public partial class FormClientes : Form
    {
        private readonly FormPrincipal formPrincipal;


        public FormClientes(
            FormPrincipal formPrincipal)
        {
            InitializeComponent();
            ConfigurarPermisos();
            CargarGrilla();
        }

            this.formPrincipal =
                formPrincipal;

            ConfigurarEventos();
        }

        private void BtnAlta_Click(object? sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string documento = txtDocumento.Text;
            string correo = txtCorreo.Text;
            string telefono = txtTelefono.Text;

        private void ConfigurarEventos()
            {
            btnNuevoCliente.Click +=
                BtnNuevoCliente_Click;

            dgvClientes.CellContentClick +=
                DgvClientes_CellContentClick;
            }


        // =========================================================
        // NUEVO CLIENTE
        // =========================================================

        private void BtnNuevoCliente_Click(
            object? sender,
            EventArgs e)
            {
            FormClienteDetalle detalle =
                new FormClienteDetalle(
                    formPrincipal
                );


            formPrincipal.AbrirFormularioEnPanel(
                detalle,
                formPrincipal.BotonClientes
            );
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


        // =========================================================
        // EDITAR CLIENTE
        // =========================================================

        private void DgvClientes_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                MessageBox.Show(
                    "Seleccioná un cliente de la grilla antes de dar de baja.",
                    "Baja de cliente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }


            if (dgvClientes.Columns[e.ColumnIndex]
                .Name != "colEditar")
            {
                return;
            }


            DataGridViewRow fila =
                dgvClientes.Rows[e.RowIndex];


            string dni =
                fila.Cells["colDni"]
                    .Value?.ToString() ?? "";

            string nombre =
                fila.Cells["colNombre"]
                    .Value?.ToString() ?? "";

            string apellido =
                fila.Cells["colApellido"]
                    .Value?.ToString() ?? "";

            string telefono =
                fila.Cells["colTelefono"]
                    .Value?.ToString() ?? "";

            string email =
                fila.Cells["colEmail"]
                    .Value?.ToString() ?? "";

            string localidad =
                fila.Cells["colLocalidad"]
                    .Value?.ToString() ?? "";


            FormClienteDetalle detalle =
                new FormClienteDetalle(
                    formPrincipal,
                    dni,
                    nombre,
                    apellido,
                    telefono,
                    email,
                    localidad
                );

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

            formPrincipal.AbrirFormularioEnPanel(
                detalle,
                formPrincipal.BotonClientes
            );
        }
    }
}