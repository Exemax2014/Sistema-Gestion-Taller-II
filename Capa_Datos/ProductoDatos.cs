using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    // ============================================================
    // Modelos utilizados por ProductoDatos.
    // ============================================================

    public class ProductoInfo
    {
        public int IdProducto { get; set; }

        public string CodigoBarra { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public decimal PrecioVenta { get; set; }

        public int Stock { get; set; }
    }


    public class ProductoListaInfo
    {
        public int IdProducto { get; set; }

        public string CodigoBarra { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public decimal PrecioCosto { get; set; }

        public decimal PorcentajeGanancia { get; set; }

        public decimal PrecioVenta { get; set; }

        public string Categoria { get; set; } = string.Empty;

        public string Marca { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }


    public class ProductoDetalleInfo
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


    public class CategoriaInfo
    {
        public int IdCategoria { get; set; }

        public string Nombre { get; set; } = string.Empty;
    }


    public class MarcaInfo
    {
        public int IdMarca { get; set; }

        public string Nombre { get; set; } = string.Empty;
    }


    public class ResultadoProductoDatos
    {
        public int Codigo { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        public int IdGenerado { get; set; }

        public bool Exitoso => Codigo == 0;
    }


    // ============================================================
    // Clase: ProductoDatos
    //
    // Ejecuta los procedimientos almacenados de productos.
    // ============================================================

    public class ProductoDatos
    {
        // Busca productos para el módulo de ventas.
        public List<ProductoInfo> Buscar(
            string texto,
            int idSucursal)
        {
            List<ProductoInfo> productos =
                new List<ProductoInfo>();


            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Producto_Buscar",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            comando.Parameters.Add(
                "@texto",
                SqlDbType.NVarChar,
                100
            ).Value = texto.Trim();


            comando.Parameters.Add(
                "@idSucursal",
                SqlDbType.Int
            ).Value = idSucursal;


            conexion.Open();


            using SqlDataReader lector =
                comando.ExecuteReader();


            while (lector.Read())
            {
                productos.Add(
                    new ProductoInfo
                    {
                        IdProducto =
                            Convert.ToInt32(
                                lector["id_producto"]
                            ),

                        CodigoBarra =
                            LeerTexto(
                                lector,
                                "codigo_barra"
                            ),

                        Nombre =
                            LeerTexto(
                                lector,
                                "nombre"
                            ),

                        Descripcion =
                            LeerTexto(
                                lector,
                                "descripcion"
                            ),

                        PrecioVenta =
                            Convert.ToDecimal(
                                lector["precio_venta"]
                            ),

                        Stock =
                            Convert.ToInt32(
                                lector["stock"]
                            )
                    }
                );
            }


            return productos;
        }


        // Trae todo el catálogo no eliminado.
        public List<ProductoListaInfo> ObtenerTodos()
        {
            List<ProductoListaInfo> productos =
                new List<ProductoListaInfo>();


            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Producto_Listar",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            conexion.Open();


            using SqlDataReader lector =
                comando.ExecuteReader();


            while (lector.Read())
            {
                productos.Add(
                    new ProductoListaInfo
                    {
                        IdProducto =
                            Convert.ToInt32(
                                lector["id_producto"]
                            ),

                        CodigoBarra =
                            LeerTexto(
                                lector,
                                "codigo_barra"
                            ),

                        Nombre =
                            LeerTexto(
                                lector,
                                "nombre"
                            ),

                        Descripcion =
                            LeerTexto(
                                lector,
                                "descripcion"
                            ),

                        PrecioCosto =
                            Convert.ToDecimal(
                                lector["precio_costo"]
                            ),

                        PorcentajeGanancia =
                            Convert.ToDecimal(
                                lector["porcentaje_ganancia"]
                            ),

                        PrecioVenta =
                            Convert.ToDecimal(
                                lector["precio_venta"]
                            ),

                        Categoria =
                            LeerTexto(
                                lector,
                                "categoria"
                            ),

                        Marca =
                            LeerTexto(
                                lector,
                                "marca"
                            ),

                        Activo =
                            Convert.ToBoolean(
                                lector["activo"]
                            )
                    }
                );
            }


            return productos;
        }


        // Trae el detalle completo de un producto.
        public ProductoDetalleInfo? ObtenerPorId(
            int idProducto)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Producto_ObtenerPorId",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            comando.Parameters.Add(
                "@idProducto",
                SqlDbType.Int
            ).Value = idProducto;


            conexion.Open();


            using SqlDataReader lector =
                comando.ExecuteReader();


            if (!lector.Read())
            {
                return null;
            }


            return new ProductoDetalleInfo
            {
                IdProducto =
                    Convert.ToInt32(
                        lector["id_producto"]
                    ),

                IdCategoria =
                    Convert.ToInt32(
                        lector["id_categoria"]
                    ),

                IdMarca =
                    lector["id_marca"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(
                            lector["id_marca"]
                        ),

                CodigoBarra =
                    LeerTexto(
                        lector,
                        "codigo_barra"
                    ),

                Nombre =
                    LeerTexto(
                        lector,
                        "nombre"
                    ),

                Descripcion =
                    LeerTexto(
                        lector,
                        "descripcion"
                    ),

                PrecioCosto =
                    Convert.ToDecimal(
                        lector["precio_costo"]
                    ),

                PorcentajeGanancia =
                    Convert.ToDecimal(
                        lector["porcentaje_ganancia"]
                    ),

                PrecioVenta =
                    Convert.ToDecimal(
                        lector["precio_venta"]
                    ),

                Activo =
                    Convert.ToBoolean(
                        lector["activo"]
                    )
            };
        }


        public List<CategoriaInfo> ObtenerCategorias()
        {
            List<CategoriaInfo> categorias =
                new List<CategoriaInfo>();


            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Categoria_Listar",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            conexion.Open();


            using SqlDataReader lector =
                comando.ExecuteReader();


            while (lector.Read())
            {
                categorias.Add(
                    new CategoriaInfo
                    {
                        IdCategoria =
                            Convert.ToInt32(
                                lector["id_categoria"]
                            ),

                        Nombre =
                            LeerTexto(
                                lector,
                                "nombre"
                            )
                    }
                );
            }


            return categorias;
        }


        public List<MarcaInfo> ObtenerMarcas()
        {
            List<MarcaInfo> marcas =
                new List<MarcaInfo>();


            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Marca_Listar",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            conexion.Open();


            using SqlDataReader lector =
                comando.ExecuteReader();


            while (lector.Read())
            {
                marcas.Add(
                    new MarcaInfo
                    {
                        IdMarca =
                            Convert.ToInt32(
                                lector["id_marca"]
                            ),

                        Nombre =
                            LeerTexto(
                                lector,
                                "nombre"
                            )
                    }
                );
            }


            return marcas;
        }


        public ResultadoProductoDatos Alta(
            int idCategoria,
            int? idMarca,
            string? codigoBarra,
            string nombre,
            string? descripcion,
            decimal precioCosto,
            decimal porcentajeGanancia,
            bool activo)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Producto_Alta",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            CargarParametros(
                comando,
                idCategoria,
                idMarca,
                codigoBarra,
                nombre,
                descripcion,
                precioCosto,
                porcentajeGanancia,
                activo
            );


            SqlParameter idGenerado =
                CrearSalida(
                    comando,
                    "@IdGenerado",
                    SqlDbType.Int
                );


            SqlParameter codigoResultado =
                CrearSalida(
                    comando,
                    "@CodigoResultado",
                    SqlDbType.Int
                );


            SqlParameter mensajeResultado =
                CrearSalida(
                    comando,
                    "@MensajeResultado",
                    SqlDbType.NVarChar,
                    250
                );


            conexion.Open();

            comando.ExecuteNonQuery();


            return new ResultadoProductoDatos
            {
                Codigo =
                    Convert.ToInt32(
                        codigoResultado.Value
                    ),

                Mensaje =
                    mensajeResultado.Value
                        ?.ToString()
                    ?? string.Empty,

                IdGenerado =
                    idGenerado.Value == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            idGenerado.Value
                        )
            };
        }


        public ResultadoProductoDatos Modificar(
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
            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Producto_Modificar",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            comando.Parameters.Add(
                "@idProducto",
                SqlDbType.Int
            ).Value = idProducto;


            CargarParametros(
                comando,
                idCategoria,
                idMarca,
                codigoBarra,
                nombre,
                descripcion,
                precioCosto,
                porcentajeGanancia,
                activo
            );


            SqlParameter codigoResultado =
                CrearSalida(
                    comando,
                    "@CodigoResultado",
                    SqlDbType.Int
                );


            SqlParameter mensajeResultado =
                CrearSalida(
                    comando,
                    "@MensajeResultado",
                    SqlDbType.NVarChar,
                    250
                );


            conexion.Open();

            comando.ExecuteNonQuery();


            return new ResultadoProductoDatos
            {
                Codigo =
                    Convert.ToInt32(
                        codigoResultado.Value
                    ),

                Mensaje =
                    mensajeResultado.Value
                        ?.ToString()
                    ?? string.Empty
            };
        }


        public ResultadoProductoDatos Baja(
            int idProducto)
        {
            using SqlConnection conexion =
                Conexion.CrearConexion();


            using SqlCommand comando =
                new SqlCommand(
                    "dbo.sp_Producto_Baja",
                    conexion
                );


            comando.CommandType =
                CommandType.StoredProcedure;


            comando.Parameters.Add(
                "@idProducto",
                SqlDbType.Int
            ).Value = idProducto;


            SqlParameter codigoResultado =
                CrearSalida(
                    comando,
                    "@CodigoResultado",
                    SqlDbType.Int
                );


            SqlParameter mensajeResultado =
                CrearSalida(
                    comando,
                    "@MensajeResultado",
                    SqlDbType.NVarChar,
                    250
                );


            conexion.Open();

            comando.ExecuteNonQuery();


            return new ResultadoProductoDatos
            {
                Codigo =
                    Convert.ToInt32(
                        codigoResultado.Value
                    ),

                Mensaje =
                    mensajeResultado.Value
                        ?.ToString()
                    ?? string.Empty
            };
        }


        private static void CargarParametros(
            SqlCommand comando,
            int idCategoria,
            int? idMarca,
            string? codigoBarra,
            string nombre,
            string? descripcion,
            decimal precioCosto,
            decimal porcentajeGanancia,
            bool activo)
        {
            comando.Parameters.Add(
                "@idCategoria",
                SqlDbType.Int
            ).Value = idCategoria;


            comando.Parameters.Add(
                "@idMarca",
                SqlDbType.Int
            ).Value =
                (object?)idMarca
                ?? DBNull.Value;


            comando.Parameters.Add(
                "@codigoBarra",
                SqlDbType.NVarChar,
                50
            ).Value =
                string.IsNullOrWhiteSpace(
                    codigoBarra
                )
                    ? DBNull.Value
                    : codigoBarra.Trim();


            comando.Parameters.Add(
                "@nombre",
                SqlDbType.NVarChar,
                100
            ).Value = nombre.Trim();


            comando.Parameters.Add(
                "@descripcion",
                SqlDbType.NVarChar,
                -1
            ).Value =
                string.IsNullOrWhiteSpace(
                    descripcion
                )
                    ? DBNull.Value
                    : descripcion.Trim();


            SqlParameter costo =
                comando.Parameters.Add(
                    "@precioCosto",
                    SqlDbType.Decimal
                );

            costo.Precision = 18;
            costo.Scale = 2;
            costo.Value = precioCosto;


            SqlParameter ganancia =
                comando.Parameters.Add(
                    "@porcentajeGanancia",
                    SqlDbType.Decimal
                );

            ganancia.Precision = 5;
            ganancia.Scale = 2;
            ganancia.Value =
                porcentajeGanancia;


            comando.Parameters.Add(
                "@activo",
                SqlDbType.Bit
            ).Value = activo;
        }


        private static SqlParameter CrearSalida(
            SqlCommand comando,
            string nombre,
            SqlDbType tipo,
            int tamano = 0)
        {
            SqlParameter parametro =
                tamano > 0
                    ? comando.Parameters.Add(
                        nombre,
                        tipo,
                        tamano
                    )
                    : comando.Parameters.Add(
                        nombre,
                        tipo
                    );


            parametro.Direction =
                ParameterDirection.Output;


            return parametro;
        }


        private static string LeerTexto(
            SqlDataReader lector,
            string columna)
        {
            if (lector[columna] == DBNull.Value)
            {
                return string.Empty;
            }


            return lector[columna]
                .ToString()
                ?? string.Empty;
        }
    }
}