using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // Clase: ProductoLogica
    //
    // Valida y coordina las operaciones relacionadas
    // con productos.
    //
    // También controla los permisos correspondientes
    // antes de realizar altas, modificaciones o bajas.
    // ============================================================

    public class ProductoLogica
    {
        private readonly ProductoDatos productoDatos =
            new ProductoDatos();


        // ========================================================
        // PERMISOS
        //
        // La Vista puede consultar estos métodos sin necesitar
        // conocer perfiles ni reglas internas.
        // ========================================================

        public bool PuedeVerProductos()
        {
            return SesionActual.TienePermiso(
                "PRODUCTOS_VER"
            );
        }


        public bool PuedeCrearProducto()
        {
            return SesionActual.TienePermiso(
                "PRODUCTOS_ALTA"
            );
        }


        public bool PuedeModificarProducto()
        {
            return SesionActual.TienePermiso(
                "PRODUCTOS_MODIFICAR"
            );
        }


        public bool PuedeEliminarProducto()
        {
            return SesionActual.TienePermiso(
                "PRODUCTOS_BAJA"
            );
        }


        // ========================================================
        // BUSCAR
        // ========================================================

        public List<ProductoInfo> Buscar(
            string texto,
            int idSucursal)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return new List<ProductoInfo>();
            }


            return productoDatos.Buscar(
                texto.Trim(),
                idSucursal
            );
        }


        // ========================================================
        // LISTAR
        // ========================================================

        public List<ProductoListaModelo> ObtenerTodos()
        {
            if (!PuedeVerProductos())
            {
                return new List<ProductoListaModelo>();
            }


            return productoDatos
                .ObtenerTodos()
                .Select(
                    p => new ProductoListaModelo
                    {
                        IdProducto =
                            p.IdProducto,

                        CodigoBarra =
                            p.CodigoBarra,

                        Nombre =
                            p.Nombre,

                        Categoria =
                            p.Categoria,

                        Marca =
                            p.Marca,

                        PrecioCosto =
                            p.PrecioCosto,

                        PorcentajeGanancia =
                            p.PorcentajeGanancia,

                        PrecioVenta =
                            p.PrecioVenta,

                        Activo =
                            p.Activo
                    }
                )
                .ToList();
        }


        // ========================================================
        // OBTENER POR ID
        // ========================================================

        public ProductoDetalleModelo? ObtenerPorId(
            int idProducto)
        {
            if (
                idProducto <= 0
                ||
                !PuedeVerProductos())
            {
                return null;
            }


            ProductoDetalleInfo? producto =
                productoDatos.ObtenerPorId(
                    idProducto
                );


            if (producto == null)
            {
                return null;
            }


            return new ProductoDetalleModelo
            {
                IdProducto =
                    producto.IdProducto,

                IdCategoria =
                    producto.IdCategoria,

                IdMarca =
                    producto.IdMarca,

                CodigoBarra =
                    producto.CodigoBarra,

                Nombre =
                    producto.Nombre,

                Descripcion =
                    producto.Descripcion,

                PrecioCosto =
                    producto.PrecioCosto,

                PorcentajeGanancia =
                    producto.PorcentajeGanancia,

                PrecioVenta =
                    producto.PrecioVenta,

                Activo =
                    producto.Activo
            };
        }


        // ========================================================
        // CATEGORÍAS
        // ========================================================

        public List<OpcionProductoModelo> ObtenerCategorias()
        {
            return productoDatos
                .ObtenerCategorias()
                .Select(
                    c => new OpcionProductoModelo
                    {
                        Id =
                            c.IdCategoria,

                        Nombre =
                            c.Nombre
                    }
                )
                .ToList();
        }


        // ========================================================
        // MARCAS
        // ========================================================

        public List<OpcionProductoModelo> ObtenerMarcas()
        {
            return productoDatos
                .ObtenerMarcas()
                .Select(
                    m => new OpcionProductoModelo
                    {
                        Id =
                            m.IdMarca,

                        Nombre =
                            m.Nombre
                    }
                )
                .ToList();
        }


        // ========================================================
        // VALIDAR PRODUCTO
        // ========================================================

        public string? ValidarProducto(
            int? idCategoria,
            string nombre,
            string precioCostoTexto,
            string porcentajeGananciaTexto)
        {
            if (
                !idCategoria.HasValue
                ||
                idCategoria.Value <= 0)
            {
                return "Elegí una categoría.";
            }


            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El nombre es obligatorio.";
            }


            if (
                !decimal.TryParse(
                    precioCostoTexto,
                    out decimal precioCosto
                )
                ||
                precioCosto < 0)
            {
                return
                    "El precio de costo debe ser un número mayor o igual a 0.";
            }


            if (
                !decimal.TryParse(
                    porcentajeGananciaTexto,
                    out decimal porcentaje
                )
                ||
                porcentaje < 0)
            {
                return
                    "El porcentaje de ganancia debe ser un número mayor o igual a 0.";
            }


            if (porcentaje > 999.99m)
            {
                return
                    "El porcentaje de ganancia no puede superar 999.99.";
            }


            return null;
        }


        // ========================================================
        // ALTA
        // ========================================================

        public ResultadoProducto Alta(
            int idCategoria,
            int? idMarca,
            string? codigoBarra,
            string nombre,
            string? descripcion,
            decimal precioCosto,
            decimal porcentajeGanancia,
            bool activo)
        {
            if (!PuedeCrearProducto())
            {
                return new ResultadoProducto
                {
                    Codigo = 5,

                    Mensaje =
                        "No tenés permiso para registrar productos."
                };
            }


            ResultadoProductoDatos resultado =
                productoDatos.Alta(
                    idCategoria,
                    idMarca,
                    codigoBarra,
                    nombre,
                    descripcion,
                    precioCosto,
                    porcentajeGanancia,
                    activo
                );


            return ConvertirResultado(
                resultado
            );
        }


        // ========================================================
        // MODIFICAR
        // ========================================================

        public ResultadoProducto Modificar(
            int idProducto,
            int idCategoria,
            int? idMarca,
            string? codigoBarra,
            string nombre,
            string? descripcion,
            decimal precioCosto,
            decimal porcentajeGanancia,
            bool activo)
        {
            if (!PuedeModificarProducto())
            {
                return new ResultadoProducto
                {
                    Codigo = 5,

                    Mensaje =
                        "No tenés permiso para modificar productos."
                };
            }


            if (idProducto <= 0)
            {
                return new ResultadoProducto
                {
                    Codigo = 3,

                    Mensaje =
                        "El producto indicado no es válido."
                };
            }


            return ConvertirResultado(
                productoDatos.Modificar(
                    idProducto,
                    idCategoria,
                    idMarca,
                    codigoBarra,
                    nombre,
                    descripcion,
                    precioCosto,
                    porcentajeGanancia,
                    activo
                )
            );
        }


        // ========================================================
        // BAJA
        // ========================================================

        public ResultadoProducto Baja(
            int idProducto)
        {
            if (!PuedeEliminarProducto())
            {
                return new ResultadoProducto
                {
                    Codigo = 5,

                    Mensaje =
                        "No tenés permiso para eliminar productos."
                };
            }


            if (idProducto <= 0)
            {
                return new ResultadoProducto
                {
                    Codigo = 3,

                    Mensaje =
                        "El producto indicado no es válido."
                };
            }


            return ConvertirResultado(
                productoDatos.Baja(
                    idProducto
                )
            );
        }


        // ========================================================
        // CONVERSIÓN RESULTADO DATOS -> LOGICA
        // ========================================================

        private static ResultadoProducto ConvertirResultado(
            ResultadoProductoDatos resultado)
        {
            return new ResultadoProducto
            {
                Codigo =
                    resultado.Codigo,

                Mensaje =
                    resultado.Mensaje,

                IdGenerado =
                    resultado.IdGenerado
            };
        }
    }
}