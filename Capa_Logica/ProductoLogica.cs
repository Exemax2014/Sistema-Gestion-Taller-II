using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // Clase: ProductoLogica
    //
    // Responsabilidad:
    // Contener la lógica relacionada con productos: búsqueda
    // (con stock por sucursal), listado del catálogo, categorías
    // y alta de nuevos productos.
    //
    // Coordina:
    // - ProductoDatos: obtiene y modifica productos en SQL Server.
    //
    // Esta clase no muestra MessageBox ni ejecuta SQL.
    // ============================================================
    public class ProductoLogica
    {
        private readonly ProductoDatos productoDatos;

        public ProductoLogica()
        {
            productoDatos = new ProductoDatos();
        }

        // ========================================================
        // Método: Buscar
        //
        // Busca productos activos a partir de un texto libre
        // (código de barra o nombre), junto con el stock
        // disponible en la sucursal indicada. Se usa en Ventas.
        // ========================================================
        public List<ProductoInfo> Buscar(string texto, int idSucursal)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return new List<ProductoInfo>();
            }

            return productoDatos.Buscar(texto.Trim(), idSucursal);
        }

        // ========================================================
        // Método: ObtenerActivos
        //
        // Trae todos los productos activos, para la grilla del
        // módulo de Productos.
        // ========================================================
        public List<ProductoListaInfo> ObtenerActivos()
        {
            return productoDatos.ObtenerActivos();
        }

        // ========================================================
        // Método: ObtenerCategorias
        //
        // Trae las categorías activas para el combo del Alta.
        // ========================================================
        public List<CategoriaInfo> ObtenerCategorias()
        {
            return productoDatos.ObtenerCategorias();
        }

        // ========================================================
        // Método: ValidarAlta
        //
        // Valida los datos mínimos antes de dar de alta un
        // producto. Devuelve null si está todo correcto, o un
        // mensaje de error legible para mostrar al usuario.
        // ========================================================
        public string? ValidarAlta(
         int? idCategoria,
         string nombre,
         string precioCostoTexto,
         string porcentajeGananciaTexto)
        {
            if (idCategoria is null or 0)
            {
                return "Elegí una categoría.";
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (!decimal.TryParse(precioCostoTexto, out decimal precioCosto) || precioCosto < 0)
            {
                return "El precio de costo tiene que ser un número mayor o igual a 0.";
            }

            if (!decimal.TryParse(porcentajeGananciaTexto, out decimal porcentaje) || porcentaje < 0)
            {
                return "El porcentaje de ganancia tiene que ser un número mayor o igual a 0.";
            }

            if (porcentaje > 999.99m)
            {
                return "El porcentaje de ganancia no puede ser mayor a 999.99.";
            }

            return null;
        }
        // ========================================================
        // Método: Alta
        //
        // Da de alta un producto nuevo. Se asume que ValidarAlta
        // ya fue invocado antes desde Capa_Vistas.
        // ========================================================
        public int Alta(
            int idCategoria,
            string? codigoBarra,
            string nombre,
            string? descripcion,
            decimal precioCosto,
            decimal porcentajeGanancia)
        {
            return productoDatos.Alta(
                idCategoria,
                string.IsNullOrWhiteSpace(codigoBarra) ? null : codigoBarra.Trim(),
                nombre.Trim(),
                string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim(),
                precioCosto,
                porcentajeGanancia
            );
        }
    }
}