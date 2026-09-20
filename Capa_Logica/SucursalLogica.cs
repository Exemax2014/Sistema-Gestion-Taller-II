using Capa_Datos;

namespace Capa_Logica
{
    // Representa una opción de ubicación utilizada por el formulario de sucursales.
    public class OpcionSucursalModelo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    // Expone la cantidad de usuarios activos de un perfil dentro de una sucursal.
    public class PerfilSucursalResumenModelo
    {
        public int IdPerfil { get; set; }
        public string Perfil { get; set; } = string.Empty;
        public int CantidadUsuarios { get; set; }
    }

    // Agrupa la información que necesita la vista para representar una sucursal.
    public class SucursalResumenModelo
    {
        public int IdSucursal { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Calle { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public List<PerfilSucursalResumenModelo> UsuariosPorPerfil { get; set; } = new();
    }

    // Devuelve el resultado de una operación de sucursal a la capa de Vistas.
    public class ResultadoSucursal
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdGenerado { get; set; }
        public bool Exitoso => Codigo == 0;
    }

    // Coordina permisos, validaciones y persistencia de sucursales.
    public class SucursalLogica
    {
        private readonly SucursalDatos sucursalDatos;
        private readonly DireccionDatos direccionDatos;

        // Inicializa los servicios de datos requeridos por la gestión de sucursales.
        public SucursalLogica()
        {
            sucursalDatos = new SucursalDatos();
            direccionDatos = new DireccionDatos();
        }

        // Informa si la sesión puede consultar la pestaña de sucursales.
        public bool PuedeVerSucursales() => SesionActual.TienePermiso("SUCURSALES_VER");

        // Informa si la sesión puede registrar sucursales nuevas.
        public bool PuedeCrearSucursal() => SesionActual.TienePermiso("SUCURSALES_ALTA");

        // Informa si la sesión puede modificar los datos de una sucursal.
        public bool PuedeModificarSucursal() => SesionActual.TienePermiso("SUCURSALES_MODIFICAR");

        // Informa si la sesión puede realizar bajas o reactivaciones de sucursales.
        public bool PuedeEliminarSucursal() => SesionActual.TienePermiso("SUCURSALES_BAJA");

        // Devuelve las sucursales activas que puede seleccionar el usuario autenticado.
        public List<SucursalInfo> ObtenerSucursalesDisponibles()
        {
            List<SucursalInfo> sucursales = sucursalDatos.ObtenerActivas();

            if (!SesionActual.IdSucursal.HasValue)
            {
                return sucursales;
            }

            return sucursales
                .Where(s => s.IdSucursal == SesionActual.IdSucursal.Value)
                .ToList();
        }

        // Devuelve todas las sucursales activas con sus perfiles dinámicos y cantidades.
        public List<SucursalResumenModelo> ObtenerResumenUsuariosPorPerfil()
        {
            if (!PuedeVerSucursales())
            {
                return new List<SucursalResumenModelo>();
            }

            return sucursalDatos.ObtenerResumenUsuariosPorPerfil()
                .Select(s => new SucursalResumenModelo
                {
                    IdSucursal = s.IdSucursal,
                    Nombre = s.Nombre,
                    Calle = s.Calle,
                    Localidad = s.Localidad,
                    Provincia = s.Provincia,
                    UsuariosPorPerfil = s.UsuariosPorPerfil
                        .Select(p => new PerfilSucursalResumenModelo
                        {
                            IdPerfil = p.IdPerfil,
                            Perfil = p.Perfil,
                            CantidadUsuarios = p.CantidadUsuarios
                        })
                        .ToList()
                })
                .ToList();
        }

        // Carga el catálogo cerrado de provincias para evitar altas libres desde la Vista.
        public List<OpcionSucursalModelo> ObtenerProvincias()
        {
            return direccionDatos.ObtenerProvincias()
                .Select(p => new OpcionSucursalModelo { Id = p.IdProvincia, Nombre = p.Nombre })
                .ToList();
        }

        // Carga las localidades activas de la provincia seleccionada.
        public List<OpcionSucursalModelo> ObtenerLocalidades(int idProvincia)
        {
            if (idProvincia <= 0)
            {
                return new List<OpcionSucursalModelo>();
            }

            return direccionDatos.ObtenerLocalidadesPorProvincia(idProvincia)
                .Select(l => new OpcionSucursalModelo { Id = l.IdLocalidad, Nombre = l.Nombre })
                .ToList();
        }

        // Busca una localidad normalizando espacios y mayúsculas para reutilizar registros existentes.
        public OpcionSucursalModelo? BuscarLocalidad(int idProvincia, string? nombre)
        {
            string nombreNormalizado = NormalizarEspacios(nombre);

            if (idProvincia <= 0 || string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                return null;
            }

            return ObtenerLocalidades(idProvincia).FirstOrDefault(localidad =>
                string.Equals(NormalizarEspacios(localidad.Nombre), nombreNormalizado, StringComparison.OrdinalIgnoreCase));
        }

        // Crea o reactiva una localidad de la provincia elegida solo al registrar o editar una sucursal.
        public ResultadoSucursal ObtenerOCrearLocalidad(int idProvincia, string? nombre)
        {
            if (!PuedeCrearSucursal() && !PuedeModificarSucursal())
            {
                return ResultadoSinPermiso("crear localidades para sucursales");
            }

            string nombreNormalizado = NormalizarEspacios(nombre);
            if (idProvincia <= 0)
            {
                return ResultadoInvalido("Seleccioná una provincia válida.");
            }

            if (string.IsNullOrWhiteSpace(nombreNormalizado) || nombreNormalizado.Length > 100)
            {
                return ResultadoInvalido("La localidad es obligatoria y no puede superar los 100 caracteres.");
            }

            ResultadoDireccionDatos resultado = direccionDatos.ObtenerOCrearLocalidad(idProvincia, nombreNormalizado);
            return ConvertirResultado(resultado);
        }

        // Valida los datos comunes para que alta y modificación compartan una sola regla autoritativa.
        public string? ValidarSucursal(string? nombre, int? idProvincia, int? idLocalidad, string? calle)
        {
            string nombreNormalizado = NormalizarEspacios(nombre);
            string calleNormalizada = NormalizarEspacios(calle);

            if (string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                return "El nombre de la sucursal es obligatorio.";
            }

            if (nombreNormalizado.Length > 100)
            {
                return "El nombre de la sucursal no puede superar los 100 caracteres.";
            }

            if (!idProvincia.HasValue || idProvincia.Value <= 0)
            {
                return "Seleccioná una provincia válida.";
            }

            if (!idLocalidad.HasValue || idLocalidad.Value <= 0)
            {
                return "Seleccioná una localidad válida.";
            }

            if (string.IsNullOrWhiteSpace(calleNormalizada) || calleNormalizada.Length > 150)
            {
                return "La dirección es obligatoria y no puede superar los 150 caracteres.";
            }

            bool localidadPerteneceAProvincia = ObtenerLocalidades(idProvincia.Value)
                .Any(l => l.Id == idLocalidad.Value);

            return localidadPerteneceAProvincia
                ? null
                : "La localidad seleccionada no pertenece a la provincia indicada.";
        }

        // Registra una sucursal activa con dirección válida después de controlar el permiso de alta.
        public ResultadoSucursal Alta(string? nombre, int? idProvincia, int? idLocalidad, string? calle)
        {
            if (!PuedeCrearSucursal())
            {
                return ResultadoSinPermiso("registrar sucursales");
            }

            string? error = ValidarSucursal(nombre, idProvincia, idLocalidad, calle);
            if (error != null)
            {
                return ResultadoInvalido(error);
            }

            return ConvertirResultado(sucursalDatos.Alta(
                NormalizarEspacios(nombre), idLocalidad!.Value, NormalizarEspacios(calle), SesionActual.IdUsuario));
        }

        // Actualiza una sucursal aplicando las mismas validaciones que el alta.
        public ResultadoSucursal Modificar(int idSucursal, string? nombre, int? idProvincia, int? idLocalidad, string? calle)
        {
            if (!PuedeModificarSucursal())
            {
                return ResultadoSinPermiso("modificar sucursales");
            }

            if (idSucursal <= 0)
            {
                return ResultadoInvalido("La sucursal indicada no es válida.");
            }

            string? error = ValidarSucursal(nombre, idProvincia, idLocalidad, calle);
            if (error != null)
            {
                return ResultadoInvalido(error);
            }

            return ConvertirResultado(sucursalDatos.Modificar(
                idSucursal, NormalizarEspacios(nombre), idLocalidad!.Value, NormalizarEspacios(calle), SesionActual.IdUsuario));
        }

        // Realiza una baja lógica sin permitir que una sucursal inválida llegue a Datos.
        public ResultadoSucursal Baja(int idSucursal)
        {
            if (!PuedeEliminarSucursal())
            {
                return ResultadoSinPermiso("dar de baja sucursales");
            }

            return idSucursal > 0
                ? ConvertirResultado(sucursalDatos.Baja(idSucursal, SesionActual.IdUsuario))
                : ResultadoInvalido("La sucursal indicada no es válida.");
        }

        // Reactiva una sucursal inactiva con el mismo permiso que controla su baja.
        public ResultadoSucursal Reactivar(int idSucursal)
        {
            if (!PuedeEliminarSucursal())
            {
                return ResultadoSinPermiso("reactivar sucursales");
            }

            return idSucursal > 0
                ? ConvertirResultado(sucursalDatos.Reactivar(idSucursal, SesionActual.IdUsuario))
                : ResultadoInvalido("La sucursal indicada no es válida.");
        }

        // Cambia la sucursal operativa solo si pertenece al universo permitido para la sesión.
        public bool CambiarSucursalOperativa(int? idSucursal, string nombreSucursal)
        {
            if (idSucursal.HasValue && !ObtenerSucursalesDisponibles().Any(s => s.IdSucursal == idSucursal.Value))
            {
                return false;
            }

            return SesionActual.CambiarSucursalOperativa(idSucursal, nombreSucursal);
        }

        // Informa si la sesión puede seleccionar el contexto global de sucursales.
        public bool PuedeConsultarTodasLasSucursales()
        {
            return !SesionActual.IdSucursal.HasValue;
        }

        // Normaliza espacios repetidos para que Vistas, Lógica y SQL comparen el mismo valor.
        private static string NormalizarEspacios(string? valor)
        {
            return string.Join(' ', (valor ?? string.Empty).Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        // Convierte el contrato de Direcciones al resultado expuesto por la lógica de sucursales.
        private static ResultadoSucursal ConvertirResultado(ResultadoDireccionDatos resultado)
        {
            return new ResultadoSucursal { Codigo = resultado.Codigo, Mensaje = resultado.Mensaje, IdGenerado = resultado.IdGenerado };
        }

        // Convierte el contrato de Datos al resultado expuesto por la lógica de sucursales.
        private static ResultadoSucursal ConvertirResultado(ResultadoSucursalDatos resultado)
        {
            return new ResultadoSucursal { Codigo = resultado.Codigo, Mensaje = resultado.Mensaje, IdGenerado = resultado.IdGenerado };
        }

        // Construye una respuesta uniforme cuando la sesión no posee la funcionalidad requerida.
        private static ResultadoSucursal ResultadoSinPermiso(string accion)
        {
            return new ResultadoSucursal { Codigo = 5, Mensaje = $"No tenés permiso para {accion}." };
        }

        // Construye una respuesta uniforme para datos que no superan la validación autoritativa.
        private static ResultadoSucursal ResultadoInvalido(string mensaje)
        {
            return new ResultadoSucursal { Codigo = 3, Mensaje = mensaje };
        }
    }
}
