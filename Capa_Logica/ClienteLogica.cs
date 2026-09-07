using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // Clase: ClienteLogica
    //
    // Responsabilidad:
    // Contener la lógica relacionada con clientes: búsqueda,
    // listado, alta y baja lógica.
    //
    // Coordina:
    // - ClienteDatos: obtiene y modifica clientes en SQL Server.
    //
    // Esta clase no muestra MessageBox ni ejecuta SQL.
    // ============================================================
    public class ClienteLogica
    {
        private readonly ClienteDatos clienteDatos;

        public ClienteLogica()
        {
            clienteDatos = new ClienteDatos();
        }

        // ========================================================
        // Método: Buscar
        //
        // Busca clientes activos a partir de un texto libre
        // (nombre, apellido o documento). Se usa en el buscador
        // rápido, por ejemplo en Ventas.
        // ========================================================
        public List<ClienteInfo> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return new List<ClienteInfo>();
            }

            return clienteDatos.Buscar(texto.Trim());
        }

        // ========================================================
        // Método: ObtenerActivos
        //
        // Trae todos los clientes activos, para la grilla del
        // módulo de Clientes.
        // ========================================================
        public List<ClienteInfo> ObtenerActivos()
        {
            return clienteDatos.ObtenerActivos();
        }

        // ========================================================
        // Método: ValidarAlta
        //
        // Valida los datos mínimos antes de dar de alta un cliente.
        // Devuelve null si está todo correcto, o un mensaje de
        // error legible para mostrar al usuario.
        // ========================================================
        public string? ValidarAlta(string nombre, string apellido, string documento)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(apellido))
            {
                return "El apellido es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(documento))
            {
                return "El documento es obligatorio.";
            }

            return null;
        }

        // ========================================================
        // Método: Alta
        //
        // Da de alta un cliente nuevo. Se asume que ValidarAlta
        // ya fue invocado antes desde Capa_Vistas.
        // ========================================================
        public int Alta(
            string nombre,
            string apellido,
            string documento,
            string? correo,
            string? telefono)
        {
            return clienteDatos.Alta(
                nombre.Trim(),
                apellido.Trim(),
                documento.Trim(),
                string.IsNullOrWhiteSpace(correo) ? null : correo.Trim(),
                string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim()
            );
        }

        // ========================================================
        // Método: Baja
        //
        // Baja lógica de un cliente existente.
        // ========================================================
        public void Baja(int idCliente)
        {
            clienteDatos.Baja(idCliente);
        }
    }
}