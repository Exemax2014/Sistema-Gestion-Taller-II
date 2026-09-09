namespace Capa_Logica
{
    public class ProductoListaModelo
    {
        public int IdProducto { get; set; }

        public string CodigoBarra { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string Marca { get; set; } = string.Empty;

        public decimal PrecioCosto { get; set; }

        public decimal PorcentajeGanancia { get; set; }

        public decimal PrecioVenta { get; set; }

        public bool Activo { get; set; }

        public string Estado =>
            Activo
                ? "Activo"
                : "Inactivo";
    }


    public class ProductoDetalleModelo
    {
        public int IdProducto { get; set; }

        public int IdCategoria { get; set; }

        public int? IdMarca { get; set; }

        public string CodigoBarra { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public decimal PrecioCosto { get; set; }

        public decimal PorcentajeGanancia { get; set; }

        public decimal PrecioVenta { get; set; }

        public bool Activo { get; set; }
    }


    public class OpcionProductoModelo
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public override string ToString()
        {
            return Nombre;
        }
    }


    public class ResultadoProducto
    {
        public int Codigo { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        public int IdGenerado { get; set; }

        public bool Exitoso =>
            Codigo == 0;
    }
}