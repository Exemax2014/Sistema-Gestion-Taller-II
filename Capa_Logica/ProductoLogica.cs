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
        private const decimal PrecioMaximo = 9999999999999999.99m;
        private const decimal PorcentajeGananciaMaximo = 999.99m;

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

        // Busca productos vendibles de la sucursal indicada; una búsqueda vacía
        // equivale a listar todos los que tienen stock disponible.
        public List<ProductoInfo> Buscar(
            string texto,
            int idSucursal)
        {
            return productoDatos.Buscar(
                (texto ?? string.Empty).Trim(),
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


        // Filtra las marcas del producto según la categoría seleccionada.
        public List<OpcionProductoModelo> ObtenerMarcasPorCategoria(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                return new List<OpcionProductoModelo>();
            }

            return productoDatos.ObtenerMarcasPorCategoria(idCategoria)
                .Select(m => new OpcionProductoModelo { Id = m.IdMarca, Nombre = m.Nombre })
                .ToList();
        }


        // ========================================================
        // VALIDAR PRODUCTO
        // ========================================================

        // Centraliza las reglas autoritativas para altas y modificaciones
        // antes de enviar valores que puedan fallar por rango en SQL Server.
        public string? ValidarProducto(
            int? idCategoria,
            string? codigoBarra,
            string? nombre,
            decimal precioCosto,
            decimal porcentajeGanancia)
        {
            if (
                !idCategoria.HasValue
                ||
                idCategoria.Value <= 0)
            {
                return "Elegí una categoría.";
            }


            string nombreNormalizado = NormalizarNombre(nombre);
            string codigoNormalizado = codigoBarra ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                return "El nombre es obligatorio.";
            }

            if (nombreNormalizado.Length > 100)
            {
                return "El nombre supera el máximo de 100 caracteres.";
            }

            if (nombreNormalizado.Any(c =>
                char.IsControl(c) ||
                (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c) && !"-_.()/+'#%&°,:×º".Contains(c))))
            {
                return "El nombre contiene caracteres no válidos.";
            }

            if (string.IsNullOrWhiteSpace(codigoNormalizado))
            {
                codigoNormalizado = string.Empty;
            }

            if (codigoNormalizado.Length > 50)
            {
                return
                    "El código de barras supera el máximo de 50 caracteres.";
            }

            if (codigoNormalizado.Any(c => c < '0' || c > '9'))
            {
                return "El código de barras solo puede contener dígitos.";
            }

            if (precioCosto < 0 || precioCosto > PrecioMaximo ||
                !TieneHastaDosDecimales(precioCosto))
            {
                return
                    "El precio de costo debe ser un valor no negativo de hasta 2 decimales.";
            }

            if (porcentajeGanancia < 0 || porcentajeGanancia > PorcentajeGananciaMaximo ||
                !TieneHastaDosDecimales(porcentajeGanancia))
            {
                return
                    "El porcentaje de ganancia debe estar entre 0 y 999,99 y tener hasta 2 decimales.";
            }

            if (!PrecioVentaEnRango(precioCosto, porcentajeGanancia))
            {
                return "El costo y el porcentaje superan el rango permitido para el precio de venta.";
            }


            return null;
        }


        // ========================================================
        // ALTA
        // ========================================================

        // Registra un producto solo después de aplicar la validación común autoritativa.
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

            string? error = ValidarProducto(
                idCategoria,
                codigoBarra,
                nombre,
                precioCosto,
                porcentajeGanancia
            );

            if (error != null)
            {
                return new ResultadoProducto { Codigo = 3, Mensaje = error };
            }

            ResultadoProductoDatos resultado =
                productoDatos.Alta(
                    idCategoria,
                    idMarca,
                    codigoBarra,
                    NormalizarNombre(nombre),
                    descripcion,
                    precioCosto,
                    porcentajeGanancia,
                    activo,
                    SesionActual.IdUsuario,
                    SesionActual.IdSucursal ?? SesionActual.IdSucursalOperativa
                );

            return ConvertirResultado(resultado);
        }


        // ========================================================
        // MODIFICAR
        // ========================================================

        // Modifica un producto usando la misma validación común que el alta.
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

            ProductoDetalleInfo? productoActual = productoDatos.ObtenerPorId(idProducto);
            if (productoActual == null)
            {
                return new ResultadoProducto { Codigo = 1, Mensaje = "El producto no existe o fue dado de baja." };
            }

            if (productoActual.Activo != activo && !PuedeEliminarProducto())
            {
                return new ResultadoProducto { Codigo = 5, Mensaje = "No tenés permiso para cambiar el estado del producto." };
            }

            string? error = ValidarProducto(
                idCategoria,
                codigoBarra,
                nombre,
                precioCosto,
                porcentajeGanancia
            );

            if (error != null)
            {
                return new ResultadoProducto { Codigo = 3, Mensaje = error };
            }

            return ConvertirResultado(productoDatos.Modificar(
                    idProducto,
                    idCategoria,
                    idMarca,
                    codigoBarra,
                    NormalizarNombre(nombre),
                    descripcion,
                    precioCosto,
                    porcentajeGanancia,
                    activo,
                    SesionActual.IdUsuario,
                    SesionActual.IdSucursal ?? SesionActual.IdSucursalOperativa));
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


            return ConvertirResultado(productoDatos.Baja(idProducto, SesionActual.IdUsuario,
                SesionActual.IdSucursal ?? SesionActual.IdSucursalOperativa));
        }


        // Reactiva un producto inactivo usando sus datos actuales y el permiso específico de baja/estado.
        public ResultadoProducto Reactivar(int idProducto)
        {
            if (!PuedeEliminarProducto())
                return new ResultadoProducto { Codigo = 5, Mensaje = "No tenés permiso para reactivar productos." };
            if (idProducto <= 0)
                return new ResultadoProducto { Codigo = 3, Mensaje = "El producto indicado no es válido." };

            ProductoDetalleInfo? producto = productoDatos.ObtenerPorId(idProducto);
            if (producto == null)
                return new ResultadoProducto { Codigo = 1, Mensaje = "El producto no existe o fue dado de baja." };
            if (producto.Activo)
                return new ResultadoProducto { Codigo = 3, Mensaje = "El producto ya se encuentra activo." };

            return ConvertirResultado(productoDatos.Modificar(
                producto.IdProducto,
                producto.IdCategoria,
                producto.IdMarca,
                producto.CodigoBarra,
                producto.Nombre,
                producto.Descripcion,
                producto.PrecioCosto,
                producto.PorcentajeGanancia,
                true,
                SesionActual.IdUsuario,
                SesionActual.IdSucursal ?? SesionActual.IdSucursalOperativa));
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

        // Comprueba la escala admitida por los campos DECIMAL(18,2) y DECIMAL(5,2).
        private static bool TieneHastaDosDecimales(decimal valor)
        {
            int escala = (decimal.GetBits(valor)[3] >> 16) & 0x7F;
            return escala <= 2;
        }

        // Evita que el precio calculado de SQL exceda DECIMAL(18,2).
        private static bool PrecioVentaEnRango(decimal precioCosto, decimal porcentajeGanancia)
        {
            decimal factor = 1m + porcentajeGanancia / 100m;
            return precioCosto <= PrecioMaximo / factor;
        }

        // Recorta extremos y reduce espacios repetidos sin alterar nombres/modelos legítimos.
        private static string NormalizarNombre(string? nombre)
        {
            return string.Join(" ", (nombre ?? string.Empty)
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
