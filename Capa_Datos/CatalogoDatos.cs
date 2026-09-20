using System.Data;
using Microsoft.Data.SqlClient;

namespace Capa_Datos;

public sealed class CategoriaCatalogoInfo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public int ProductosAsociados { get; set; }
}

public sealed class MarcaCatalogoInfo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public int CategoriasAsociadas { get; set; }
}

public sealed class ResultadoCatalogoDatos
{
    public int Codigo { get; init; }
    public int IdGenerado { get; init; }
    public string Mensaje { get; init; } = string.Empty;
}

// Acceso a los procedimientos almacenados de administración de catálogos.
public sealed class CatalogoDatos
{
    // Devuelve categorías activas e históricas junto con sus productos asociados.
    public List<CategoriaCatalogoInfo> ListarCategorias()
    {
        List<CategoriaCatalogoInfo> items = new();
        using SqlConnection cn = Conexion.CrearConexion();
        using SqlCommand cmd = new("dbo.sp_Categoria_ListarGestion", cn) { CommandType = CommandType.StoredProcedure };
        cn.Open();
        using SqlDataReader rd = cmd.ExecuteReader();
        while (rd.Read()) items.Add(new CategoriaCatalogoInfo
        {
            Id = Convert.ToInt32(rd["id_categoria"]), Nombre = rd["nombre"].ToString() ?? string.Empty,
            Descripcion = rd["descripcion"] == DBNull.Value ? string.Empty : rd["descripcion"].ToString() ?? string.Empty,
            Activo = Convert.ToBoolean(rd["activo"]), ProductosAsociados = Convert.ToInt32(rd["cantidad_productos"])
        });
        return items;
    }

    // Devuelve las marcas con estado y cantidad de categorías vinculadas.
    public List<MarcaCatalogoInfo> ListarMarcas()
    {
        List<MarcaCatalogoInfo> items = new();
        using SqlConnection cn = Conexion.CrearConexion();
        using SqlCommand cmd = new("dbo.sp_Marca_ListarGestion", cn) { CommandType = CommandType.StoredProcedure };
        cn.Open();
        using SqlDataReader rd = cmd.ExecuteReader();
        while (rd.Read()) items.Add(new MarcaCatalogoInfo
        {
            Id = Convert.ToInt32(rd["id_marca"]), Nombre = rd["nombre"].ToString() ?? string.Empty,
            Activo = Convert.ToBoolean(rd["activo"]), CategoriasAsociadas = Convert.ToInt32(rd["cantidad_categorias"])
        });
        return items;
    }

    // Lee los identificadores de categorías vinculados a una marca para editarla.
    public List<int> ObtenerCategoriasMarca(int idMarca)
    {
        List<int> ids = new();
        using SqlConnection cn = Conexion.CrearConexion();
        using SqlCommand cmd = new("dbo.sp_Marca_CategoriasObtener", cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@idMarca", SqlDbType.Int).Value = idMarca;
        cn.Open();
        using SqlDataReader rd = cmd.ExecuteReader();
        while (rd.Read()) ids.Add(Convert.ToInt32(rd["id_categoria"]));
        return ids;
    }

    // Guarda una categoría y su auditoría en la transacción del procedimiento.
    public ResultadoCatalogoDatos GuardarCategoria(int id, string nombre, string descripcion, int idUsuario, int? idSucursal)
    {
        using SqlConnection cn = Conexion.CrearConexion();
        using SqlCommand cmd = new("dbo.sp_Categoria_Guardar", cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@idCategoria", SqlDbType.Int).Value = id;
        cmd.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
        cmd.Parameters.Add("@descripcion", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(descripcion) ? DBNull.Value : descripcion;
        cmd.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = idUsuario;
        cmd.Parameters.Add("@idSucursalAuditoria", SqlDbType.Int).Value = (object?)idSucursal ?? DBNull.Value;
        return EjecutarResultado(cmd, true);
    }

    // Cambia el estado lógico de una categoría y registra la acción atómicamente.
    public ResultadoCatalogoDatos CambiarEstadoCategoria(int id, bool activar, int usuario, int? sucursal) =>
        CambiarEstado(activar ? "dbo.sp_Categoria_Reactivar" : "dbo.sp_Categoria_Baja", "@idCategoria", id, usuario, sucursal);

    // Guarda una marca y reemplaza sus asociaciones dentro de una sola transacción.
    public ResultadoCatalogoDatos GuardarMarca(int id, string nombre, IEnumerable<int> categorias, int usuario, int? sucursal)
    {
        using SqlConnection cn = Conexion.CrearConexion();
        using SqlCommand cmd = new("dbo.sp_Marca_Guardar", cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@idMarca", SqlDbType.Int).Value = id;
        cmd.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre;
        cmd.Parameters.Add("@categorias", SqlDbType.NVarChar, -1).Value = string.Join(",", categorias);
        cmd.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = usuario;
        cmd.Parameters.Add("@idSucursalAuditoria", SqlDbType.Int).Value = (object?)sucursal ?? DBNull.Value;
        return EjecutarResultado(cmd, true);
    }

    // Cambia el estado lógico de una marca y registra la acción atómicamente.
    public ResultadoCatalogoDatos CambiarEstadoMarca(int id, bool activar, int usuario, int? sucursal) =>
        CambiarEstado(activar ? "dbo.sp_Marca_Reactivar" : "dbo.sp_Marca_Baja", "@idMarca", id, usuario, sucursal);

    // Ejecuta el SP de estado con parámetros comunes de identidad y auditoría.
    private static ResultadoCatalogoDatos CambiarEstado(string sp, string idParam, int id, int usuario, int? sucursal)
    {
        using SqlConnection cn = Conexion.CrearConexion();
        using SqlCommand cmd = new(sp, cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add(idParam, SqlDbType.Int).Value = id;
        cmd.Parameters.Add("@idUsuarioEjecutor", SqlDbType.Int).Value = usuario;
        cmd.Parameters.Add("@idSucursalAuditoria", SqlDbType.Int).Value = (object?)sucursal ?? DBNull.Value;
        return EjecutarResultado(cmd, false);
    }

    // Lee el resultado común de guardado o cambio de estado.
    private static ResultadoCatalogoDatos EjecutarResultado(SqlCommand cmd, bool incluyeId)
    {
        SqlParameter? id = null;
        if (incluyeId) id = cmd.Parameters.Add("@IdGenerado", SqlDbType.Int);
        SqlParameter codigo = cmd.Parameters.Add("@CodigoResultado", SqlDbType.Int);
        codigo.Direction = ParameterDirection.Output;
        SqlParameter mensaje = cmd.Parameters.Add("@MensajeResultado", SqlDbType.NVarChar, 250);
        mensaje.Direction = ParameterDirection.Output;
        if (id != null) id.Direction = ParameterDirection.Output;
        cmd.Connection!.Open();
        cmd.ExecuteNonQuery();
        return new ResultadoCatalogoDatos
        {
            Codigo = Convert.ToInt32(codigo.Value), IdGenerado = id == null ? 0 : Convert.ToInt32(id.Value),
            Mensaje = mensaje.Value?.ToString() ?? string.Empty
        };
    }
}
