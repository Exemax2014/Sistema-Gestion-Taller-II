using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // Clase: ClienteLogica
    //
    // Responsabilidad:
    // Contener la lógica relacionada con la búsqueda de clientes.
    //
    // Coordina:
    // - ClienteDatos: obtiene los clientes desde SQL Server.
    //
    // Esta clase no muestra MessageBox ni ejecuta SQL.
    // ============================================================
    public class ClienteLogica
    {
        private readonly ClienteDatos clienteDatos;

        // ========================================================
        // Constructor
        //
        // Crea el objeto encargado del acceso a datos de clientes.
        // ========================================================
        public ClienteLogica()
        {
            clienteDatos = new ClienteDatos();
        }

        // ========================================================
        // Método: Buscar
        //
        // Busca clientes activos a partir de un texto libre
        // (nombre, apellido o documento).
        //
        // Devuelve una lista vacía si el texto está vacío,
        // sin necesidad de consultar la base de datos.
        // ========================================================
        public List<ClienteInfo> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return new List<ClienteInfo>();
            }

            return clienteDatos.Buscar(texto.Trim());
        }
    }
}
