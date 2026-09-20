using System.Text.RegularExpressions;
using Capa_Datos;

namespace Capa_Logica;

public sealed class CategoriaCatalogoModelo
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public bool Activo { get; init; }
    public int ProductosAsociados { get; init; }
}

public sealed class MarcaCatalogoModelo
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public bool Activo { get; init; }
    public int CategoriasAsociadas { get; init; }
}

public sealed class ResultadoCatalogo
{
    public bool Exitoso { get; init; }
    public string Mensaje { get; init; } = string.Empty;
}

// Valida permisos y reglas autoritativas del mantenimiento de categorías y marcas.
public sealed class CatalogoLogica
{
    private readonly CatalogoDatos datos = new();

    // Consulta permisos granulares de categorías sin depender del perfil.
    public bool Puede(string codigo) => SesionActual.TienePermiso(codigo);

    // Lista categorías históricas únicamente para perfiles autorizados.
    public List<CategoriaCatalogoModelo> ListarCategorias() => Puede("CATEGORIAS_VER")
        ? datos.ListarCategorias().Select(x => new CategoriaCatalogoModelo { Id = x.Id, Nombre = x.Nombre, Descripcion = x.Descripcion, Activo = x.Activo, ProductosAsociados = x.ProductosAsociados }).ToList()
        : new List<CategoriaCatalogoModelo>();

    // Lista marcas históricas únicamente para perfiles autorizados.
    public List<MarcaCatalogoModelo> ListarMarcas() => Puede("MARCAS_VER")
        ? datos.ListarMarcas().Select(x => new MarcaCatalogoModelo { Id = x.Id, Nombre = x.Nombre, Activo = x.Activo, CategoriasAsociadas = x.CategoriasAsociadas }).ToList()
        : new List<MarcaCatalogoModelo>();

    // Devuelve las categorías relacionadas a una marca para precargar la edición.
    public List<int> ObtenerCategoriasMarca(int idMarca) => Puede("MARCAS_VER") && idMarca > 0 ? datos.ObtenerCategoriasMarca(idMarca) : new List<int>();

    // Normaliza y valida el nombre antes de persistir categoría y auditoría.
    public ResultadoCatalogo GuardarCategoria(int id, string nombre, string descripcion)
    {
        if (!Puede(id == 0 ? "CATEGORIAS_ALTA" : "CATEGORIAS_MODIFICAR")) return Fallo("No tenés permiso para guardar categorías.");
        string normalizado = Normalizar(nombre);
        string detalle = descripcion?.Trim() ?? string.Empty;
        if (normalizado.Length == 0 || normalizado.Length > 100 || detalle.Length > 200 || normalizado.Any(char.IsControl) || detalle.Any(char.IsControl))
            return Fallo("Ingresá un nombre de categoría válido (hasta 100 caracteres) y una descripción de hasta 200 caracteres.");
        return Convertir(datos.GuardarCategoria(id, normalizado, detalle, SesionActual.IdUsuario, SesionActual.IdSucursalOperativa));
    }

    // Aplica la baja lógica o reactivación según el permiso de categorías.
    public ResultadoCatalogo CambiarEstadoCategoria(int id, bool activar)
    {
        if (!Puede("CATEGORIAS_BAJA")) return Fallo("No tenés permiso para cambiar el estado de categorías.");
        if (id <= 0) return Fallo("La categoría seleccionada no es válida.");
        return Convertir(datos.CambiarEstadoCategoria(id, activar, SesionActual.IdUsuario, SesionActual.IdSucursalOperativa));
    }

    // Normaliza nombre y valida categorías activas antes del guardado atómico de marca.
    public ResultadoCatalogo GuardarMarca(int id, string nombre, IEnumerable<int> categorias)
    {
        if (!Puede(id == 0 ? "MARCAS_ALTA" : "MARCAS_MODIFICAR")) return Fallo("No tenés permiso para guardar marcas.");
        string normalizado = Normalizar(nombre);
        int[] seleccionadas = categorias?.Distinct().ToArray() ?? Array.Empty<int>();
        if (normalizado.Length == 0 || normalizado.Length > 100 || normalizado.Any(char.IsControl)) return Fallo("Ingresá un nombre de marca válido de hasta 100 caracteres.");
        if (seleccionadas.Length == 0 || seleccionadas.Any(x => x <= 0)) return Fallo("Seleccioná al menos una categoría válida.");
        HashSet<int> activas = new(datos.ListarCategorias().Where(x => x.Activo).Select(x => x.Id));
        if (seleccionadas.Any(x => !activas.Contains(x))) return Fallo("Todas las categorías seleccionadas deben estar activas.");
        return Convertir(datos.GuardarMarca(id, normalizado, seleccionadas, SesionActual.IdUsuario, SesionActual.IdSucursalOperativa));
    }

    // Aplica la baja lógica o reactivación según el permiso de marcas.
    public ResultadoCatalogo CambiarEstadoMarca(int id, bool activar)
    {
        if (!Puede("MARCAS_BAJA")) return Fallo("No tenés permiso para cambiar el estado de marcas.");
        if (id <= 0) return Fallo("La marca seleccionada no es válida.");
        return Convertir(datos.CambiarEstadoMarca(id, activar, SesionActual.IdUsuario, SesionActual.IdSucursalOperativa));
    }

    // Reduce espacios consecutivos y recorta los extremos manteniendo Unicode.
    private static string Normalizar(string? valor) => Regex.Replace(valor?.Trim() ?? string.Empty, @"\s+", " ");

    // Convierte la respuesta SQL a un resultado apto para las vistas.
    private static ResultadoCatalogo Convertir(ResultadoCatalogoDatos r) => new() { Exitoso = r.Codigo == 0, Mensaje = r.Mensaje };

    // Construye un error de validación sin consultar la base.
    private static ResultadoCatalogo Fallo(string mensaje) => new() { Exitoso = false, Mensaje = mensaje };
}
