namespace Capa_Logica
{
    public class StockSucursalModelo
    {
        public int IdSucursal { get; set; }

        public string Sucursal { get; set; } = string.Empty;

        public int Stock { get; set; }

        public int StockMinimo { get; set; }

        public bool PuedeModificar { get; set; }


        public string Estado
        {
            get
            {
                if (Stock <= StockMinimo)
                {
                    return "Stock bajo";
                }

                return "Normal";
            }
        }
    }


    public class ResultadoInventario
    {
        public int Codigo { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        public bool Exitoso =>
            Codigo == 0;
    }
}