using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    public class DashboardResumenDatos
    {
        public int VentasHoy { get; set; }
        public decimal IngresosHoy { get; set; }
        public int StockBajo { get; set; }
        public int ProductosActivos { get; set; }
    }

    public class DashboardSerieDatos
    {
        public DateTime Fecha { get; set; }
        public int Ventas { get; set; }
        public decimal Ingresos { get; set; }
    }

    public class DashboardProductoDatos
    {
        public string Producto { get; set; } = string.Empty;
        public int UnidadesVendidas { get; set; }
    }

    public class DashboardVentaRecienteDatos
    {
        public DateTime FechaHora { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Vendedor { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }

    public class AvisoDatos
    {
        public int IdAviso { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string Autor { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
    }

    public class AvisoDestinoDatos
    {
        public int IdFuncionalidad { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    // Consulta datos acotados para el inicio y administra sus avisos persistentes.
    public class DashboardDatos
    {
        // Obtiene las tarjetas sin cargar el detalle completo de ventas o inventario.
        public DashboardResumenDatos ObtenerResumen(int? idSucursal)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = CrearComando("dbo.sp_Dashboard_ObtenerResumen", conexion);
            AgregarSucursal(comando, idSucursal);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            return lector.Read() ? new DashboardResumenDatos
            {
                VentasHoy = lector.GetInt32(0), IngresosHoy = lector.GetDecimal(1),
                StockBajo = lector.GetInt32(2), ProductosActivos = lector.GetInt32(3)
            } : new DashboardResumenDatos();
        }

        // Devuelve una serie diaria compacta para los mini informes semanales.
        public List<DashboardSerieDatos> ObtenerSerieSemanal(int? idSucursal)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = CrearComando("dbo.sp_Dashboard_VentasUltimos7Dias", conexion);
            AgregarSucursal(comando, idSucursal);
            return EjecutarLista(comando, lector => new DashboardSerieDatos
            {
                Fecha = lector.GetDateTime(0), Ventas = lector.GetInt32(1), Ingresos = lector.GetDecimal(2)
            });
        }

        // Obtiene sólo los productos necesarios para el ranking breve del inicio.
        public List<DashboardProductoDatos> ObtenerProductosMasVendidos(int? idSucursal)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = CrearComando("dbo.sp_Dashboard_ProductosMasVendidos", conexion);
            AgregarSucursal(comando, idSucursal);
            return EjecutarLista(comando, lector => new DashboardProductoDatos
            {
                Producto = lector.GetString(0), UnidadesVendidas = lector.GetInt32(1)
            });
        }

        // Recupera las cinco últimas ventas visibles en el alcance solicitado.
        public List<DashboardVentaRecienteDatos> ObtenerActividadReciente(int? idSucursal)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = CrearComando("dbo.sp_Dashboard_ActividadReciente", conexion);
            AgregarSucursal(comando, idSucursal);
            return EjecutarLista(comando, lector => new DashboardVentaRecienteDatos
            {
                FechaHora = lector.GetDateTime(0), Cliente = lector.GetString(1),
                Vendedor = lector.GetString(2), Total = lector.GetDecimal(3)
            });
        }

        // Lista exclusivamente los avisos activos destinados al usuario autenticado.
        public List<AvisoDatos> ListarAvisosParaUsuario(int idUsuario)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = CrearComando("dbo.sp_Aviso_ListarParaUsuario", conexion);
            comando.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;
            return EjecutarLista(comando, lector => new AvisoDatos
            {
                IdAviso = lector.GetInt32(0), Titulo = lector.GetString(1), Mensaje = lector.GetString(2),
                FechaCreacion = lector.GetDateTime(3), Autor = lector.GetString(4), Destino = lector.GetString(5)
            });
        }

        // Devuelve los destinos habilitados por SQL para el contexto del autor.
        public List<AvisoDestinoDatos> ListarDestinosAviso(int idUsuario)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = CrearComando("dbo.sp_Aviso_ListarDestinos", conexion);
            comando.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;
            return EjecutarLista(comando, lector => new AvisoDestinoDatos
            {
                IdFuncionalidad = lector.GetInt32(0), Nombre = lector.GetString(1)
            });
        }

        // Publica el aviso y deja la autorización jerárquica como responsabilidad de SQL.
        public void PublicarAviso(int idUsuario, int idDestino, string titulo, string mensaje)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            using SqlCommand comando = CrearComando("dbo.sp_Aviso_Publicar", conexion);
            comando.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;
            comando.Parameters.Add("@idFuncionalidadDestino", SqlDbType.Int).Value = idDestino;
            comando.Parameters.Add("@titulo", SqlDbType.NVarChar, 100).Value = titulo;
            comando.Parameters.Add("@mensaje", SqlDbType.NVarChar, 500).Value = mensaje;
            conexion.Open();
            comando.ExecuteNonQuery();
        }

        // Centraliza la creación de comandos para mantener todas las consultas como procedimientos almacenados.
        private static SqlCommand CrearComando(string procedimiento, SqlConnection conexion)
        {
            return new SqlCommand(procedimiento, conexion) { CommandType = CommandType.StoredProcedure };
        }

        // Envía NULL para el alcance global o una sucursal concreta sin valores hardcodeados.
        private static void AgregarSucursal(SqlCommand comando, int? idSucursal)
        {
            comando.Parameters.Add("@idSucursal", SqlDbType.Int).Value = idSucursal ?? (object)DBNull.Value;
        }

        // Ejecuta lectores homogéneos y evita repetir la apertura y el mapeo de listas.
        private static List<T> EjecutarLista<T>(SqlCommand comando, Func<SqlDataReader, T> mapear)
        {
            List<T> resultado = new List<T>();
            comando.Connection.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read()) resultado.Add(mapear(lector));
            return resultado;
        }
    }
}
