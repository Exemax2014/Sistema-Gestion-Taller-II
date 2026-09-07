using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // Clase: ProductoLogica
    //
    // Responsabilidad:
    // Contener la lógica relacionada con la búsqueda de productos
    // y su stock disponible en una sucursal puntual.
    //
    // Coordina:
    // - ProductoDatos: obtiene los productos desde SQL Server.
    //
    // Esta clase no muestra MessageBox ni ejecuta SQL.
    // ============================================================
    public class ProductoLogica
    {
        private readonly ProductoDatos productoDatos;

        // ========================================================
        // Constructor
        //
        // Crea el objeto encargado del acceso a datos de productos.
        // ========================================================
        public ProductoLogica()
        {
            productoDatos = new ProductoDatos();
        }

        // ========================================================
        // Método: Buscar
        //
        // Busca productos activos a partir de un texto libre
        // (código de barra o nombre), junto con el stock
        // disponible en la sucursal indicada.
        //
        // Devuelve una lista vacía si el texto está vacío,
        // sin necesidad de consultar la base de datos.
        // ========================================================
        public List<ProductoInfo> Buscar(string texto, int idSucursal)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return new List<ProductoInfo>();
            }

            return productoDatos.Buscar(texto.Trim(), idSucursal);
        }
    }
}
