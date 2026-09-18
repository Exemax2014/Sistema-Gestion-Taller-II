using Capa_Datos;

using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Capa_Logica
{
    // ============================================================
    // Modelos utilizados por ClienteLogica.
    // ============================================================

    public class ClienteListaModelo
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string DireccionCompleta { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class ClienteDetalleModelo
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int? IdDireccion { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Altura { get; set; } = string.Empty;
        public int? IdLocalidad { get; set; }
        public int? IdProvincia { get; set; }
    }

    public class OpcionClienteModelo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class ResultadoCliente
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdGenerado { get; set; }
        public bool Exitoso => Codigo == 0;
    }

    // ============================================================
    // Clase: ClienteLogica
    //
    // Valida y coordina las operaciones relacionadas con clientes,
    // incluida su dirección y su estado (activo / dado de baja).
    //
    // También controla los permisos correspondientes antes de
    // realizar altas, modificaciones, bajas o reactivaciones.
    // ============================================================

    public class ClienteLogica
    {
        private readonly ClienteDatos clienteDatos = new ClienteDatos();
        private readonly DireccionDatos direccionDatos = new DireccionDatos();

        // ========================================================
        // PERMISOS
        // ========================================================

        public bool PuedeVerClientes() => SesionActual.TienePermiso("CLIENTES_VER");
        public bool PuedeCrearCliente() => SesionActual.TienePermiso("CLIENTES_ALTA");
        public bool PuedeModificarCliente() => SesionActual.TienePermiso("CLIENTES_MODIFICAR");
        public bool PuedeEliminarCliente() => SesionActual.TienePermiso("CLIENTES_BAJA");
        // El historial combina datos de Clientes y Ventas, por lo que exige
        // ambas funcionalidades antes de habilitarlo en la vista.
        public bool PuedeVerHistorialCompras() =>
            SesionActual.TienePermiso("CLIENTES_VER") &&
            SesionActual.TienePermiso("VENTAS_VER");

        // Reactivar usa el mismo permiso que dar de baja: quien
        // puede sacar a un cliente de la lista de activos, también
        // puede devolverlo.
        public bool PuedeReactivarCliente() => SesionActual.TienePermiso("CLIENTES_BAJA");

        // ========================================================
        // BUSCAR (usado por el buscador del listado)
        //
        // estado: "ACTIVOS" | "BAJA" | "TODOS"
        // ========================================================

        public List<ClienteListaModelo> Buscar(string texto, string estado = "ACTIVOS")
        {
            if (!PuedeVerClientes() || string.IsNullOrWhiteSpace(texto))
            {
                return new List<ClienteListaModelo>();
            }

            return clienteDatos
                .Buscar(texto.Trim(), NormalizarEstado(estado))
                .Select(c => new ClienteListaModelo
                {
                    IdCliente = c.IdCliente,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Documento = c.Documento,
                    Correo = c.Correo,
                    Telefono = c.Telefono,
                    Localidad = c.Localidad,
                    Provincia = c.Provincia,
                    DireccionCompleta = ArmarDireccion(c.Calle, c.Altura, c.Localidad, c.Provincia),
                    Activo = c.Activo
                })
                .ToList();
        }

        // ========================================================
        // LISTAR
        // ========================================================

        public List<ClienteListaModelo> ObtenerTodos(string estado = "ACTIVOS")
        {
            if (!PuedeVerClientes())
            {
                return new List<ClienteListaModelo>();
            }

            return clienteDatos
                .ObtenerTodos(NormalizarEstado(estado))
                .Select(c => new ClienteListaModelo
                {
                    IdCliente = c.IdCliente,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Documento = c.Documento,
                    Correo = c.Correo,
                    Telefono = c.Telefono,
                    Localidad = c.Localidad,
                    Provincia = c.Provincia,
                    DireccionCompleta = ArmarDireccion(c.Calle, c.Altura, c.Localidad, c.Provincia),
                    Activo = c.Activo
                })
                .ToList();
        }

        // ========================================================
        // OBTENER POR ID
        // ========================================================

        public ClienteDetalleModelo? ObtenerPorId(int idCliente)
        {
            if (idCliente <= 0 || !PuedeVerClientes())
            {
                return null;
            }

            ClienteDetalleInfo? cliente = clienteDatos.ObtenerPorId(idCliente);

            if (cliente == null)
            {
                return null;
            }

            return new ClienteDetalleModelo
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Documento = cliente.Documento,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                IdDireccion = cliente.IdDireccion,
                Calle = cliente.Calle,
                Altura = cliente.Altura,
                IdLocalidad = cliente.IdLocalidad,
                IdProvincia = cliente.IdProvincia
            };
        }

        // ========================================================
        // PROVINCIAS Y LOCALIDADES
        // ========================================================

        public List<OpcionClienteModelo> ObtenerProvincias()
        {
            return direccionDatos
                .ObtenerProvincias()
                .Select(p => new OpcionClienteModelo { Id = p.IdProvincia, Nombre = p.Nombre })
                .ToList();
        }

        public List<OpcionClienteModelo> ObtenerLocalidades(int idProvincia)
        {
            if (idProvincia <= 0)
            {
                return new List<OpcionClienteModelo>();
            }

            return direccionDatos
                .ObtenerLocalidadesPorProvincia(idProvincia)
                .Select(l => new OpcionClienteModelo { Id = l.IdLocalidad, Nombre = l.Nombre })
                .ToList();
        }

        // Busca una localidad de la provincia seleccionada normalizando el texto
        // para reutilizar la existente aunque cambien mayúsculas o espacios.
        public OpcionClienteModelo? BuscarLocalidad(int idProvincia, string? nombre)
        {
            string nombreNormalizado = NormalizarNombre(nombre);

            if (idProvincia <= 0 || string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                return null;
            }

            return ObtenerLocalidades(idProvincia)
                .FirstOrDefault(l => string.Equals(
                    NormalizarNombre(l.Nombre),
                    nombreNormalizado,
                    StringComparison.OrdinalIgnoreCase));
        }

        // Valida la creación desde Clientes y delega en Datos la operación
        // idempotente que resuelve conflictos de nombre en SQL Server.
        public ResultadoCliente ObtenerOCrearLocalidad(int idProvincia, string? nombre)
        {
            string nombreNormalizado = NormalizarNombre(nombre);

            if (!PuedeCrearCliente() && !PuedeModificarCliente())
            {
                return new ResultadoCliente { Codigo = 5, Mensaje = "No tenés permiso para crear localidades." };
            }

            if (idProvincia <= 0)
            {
                return new ResultadoCliente { Codigo = 3, Mensaje = "Seleccioná una provincia válida." };
            }

            if (string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                return new ResultadoCliente { Codigo = 3, Mensaje = "La localidad es obligatoria." };
            }

            if (nombreNormalizado.Length > 100)
            {
                return new ResultadoCliente { Codigo = 3, Mensaje = "La localidad supera el máximo de 100 caracteres." };
            }

            ResultadoDireccionDatos resultado =
                direccionDatos.ObtenerOCrearLocalidad(idProvincia, nombreNormalizado);

            return new ResultadoCliente
            {
                Codigo = resultado.Codigo,
                Mensaje = resultado.Mensaje,
                IdGenerado = resultado.IdGenerado
            };
        }

        // ========================================================
        // VALIDAR CLIENTE
        // ========================================================

        public string? ValidarCliente(
            string nombre, string apellido, string documento, string? correo, string? telefono,
            int? idProvincia, int? idLocalidad, string? calle)
        {
            string? errorBasico = ValidarDatosBasicos(nombre, apellido, documento, correo, telefono);

            if (errorBasico != null)
            {
                return errorBasico;
            }

            return ValidarDireccionObligatoria(idProvincia, idLocalidad, calle);
        }

        // ========================================================
        // ALTA
        // ========================================================

        // Registra un cliente solo después de validar datos y dirección completa.
        public ResultadoCliente Alta(
            string nombre, string apellido, string documento, string? correo, string? telefono,
            int? idProvincia, int? idLocalidad, string? calle, string? altura)
        {
            if (!PuedeCrearCliente())
            {
                return new ResultadoCliente { Codigo = 5, Mensaje = "No tenés permiso para registrar clientes." };
            }

            string? error = ValidarCliente(nombre, apellido, documento, correo, telefono,
                idProvincia, idLocalidad, calle);
            if (error != null)
            {
                return new ResultadoCliente { Codigo = 3, Mensaje = error };
            }

            ResultadoDireccionDatos direccion = direccionDatos.Alta(idLocalidad!.Value, calle!, altura);

            if (!direccion.Exitoso)
            {
                return new ResultadoCliente { Codigo = direccion.Codigo, Mensaje = direccion.Mensaje };
            }

            return ConvertirResultado(
                clienteDatos.Alta(nombre, apellido, documento, correo, telefono, direccion.IdGenerado));
        }

        // ========================================================
        // MODIFICAR
        // ========================================================

        // Modifica datos y dirección mediante las mismas reglas autoritativas del alta.
        public ResultadoCliente Modificar(
            int idCliente, int? idDireccionActual, string nombre, string apellido, string documento,
            string? correo, string? telefono, int? idProvincia, int? idLocalidad, string? calle, string? altura)
        {
            if (!PuedeModificarCliente())
            {
                return new ResultadoCliente { Codigo = 5, Mensaje = "No tenés permiso para modificar clientes." };
            }

            if (idCliente <= 0)
            {
                return new ResultadoCliente { Codigo = 3, Mensaje = "El cliente indicado no es válido." };
            }

            string? error = ValidarCliente(nombre, apellido, documento, correo, telefono,
                idProvincia, idLocalidad, calle);
            if (error != null)
            {
                return new ResultadoCliente { Codigo = 3, Mensaje = error };
            }

            int? idDireccion = idDireccionActual;

            if (idDireccionActual.HasValue)
            {
                ResultadoDireccionDatos direccion =
                    direccionDatos.Modificar(idDireccionActual.Value, idLocalidad!.Value, calle!, altura);

                if (!direccion.Exitoso)
                {
                    return new ResultadoCliente { Codigo = direccion.Codigo, Mensaje = direccion.Mensaje };
                }
            }
            else
            {
                ResultadoDireccionDatos direccion = direccionDatos.Alta(idLocalidad!.Value, calle!, altura);

                if (!direccion.Exitoso)
                {
                    return new ResultadoCliente { Codigo = direccion.Codigo, Mensaje = direccion.Mensaje };
                }

                idDireccion = direccion.IdGenerado;
            }

            return ConvertirResultado(
                clienteDatos.Modificar(idCliente, nombre, apellido, documento, correo, telefono, idDireccion));
        }

        // ========================================================
        // BAJA
        // ========================================================

        public ResultadoCliente Baja(int idCliente)
        {
            if (!PuedeEliminarCliente())
            {
                return new ResultadoCliente { Codigo = 5, Mensaje = "No tenés permiso para eliminar clientes." };
            }

            if (idCliente <= 0)
            {
                return new ResultadoCliente { Codigo = 3, Mensaje = "El cliente indicado no es válido." };
            }

            return ConvertirResultado(clienteDatos.Baja(idCliente));
        }

        // ========================================================
        // REACTIVAR
        // ========================================================

        public ResultadoCliente Reactivar(int idCliente)
        {
            if (!PuedeReactivarCliente())
            {
                return new ResultadoCliente { Codigo = 5, Mensaje = "No tenés permiso para reactivar clientes." };
            }

            if (idCliente <= 0)
            {
                return new ResultadoCliente { Codigo = 3, Mensaje = "El cliente indicado no es válido." };
            }

            return ConvertirResultado(clienteDatos.Reactivar(idCliente));
        }

        // ========================================================
        // AUXILIARES
        // ========================================================

        private static string ArmarDireccion(string calle, string altura, string localidad, string provincia)
        {
            if (string.IsNullOrWhiteSpace(calle))
            {
                return string.Empty;
            }

            string direccion = string.IsNullOrWhiteSpace(altura) ? calle : $"{calle} {altura}";

            if (!string.IsNullOrWhiteSpace(localidad)) direccion += $", {localidad}";
            if (!string.IsNullOrWhiteSpace(provincia)) direccion += $" ({provincia})";

            return direccion;
        }

        private static ResultadoCliente ConvertirResultado(ResultadoClienteDatos resultado)
        {
            return new ResultadoCliente
            {
                Codigo = resultado.Codigo,
                Mensaje = resultado.Mensaje,
                IdGenerado = resultado.IdGenerado
            };
        }

        // Limita el filtro a los estados admitidos por los procedimientos.
        private static string NormalizarEstado(string? estado)
        {
            return estado?.Trim().ToUpperInvariant() switch
            {
                "BAJA" => "BAJA",
                "TODOS" => "TODOS",
                _ => "ACTIVOS"
            };
        }

        // Unifica espacios internos para comparar nombres de localidad de forma consistente.
        private static string NormalizarNombre(string? valor)
        {
            return string.Join(
                ' ',
                (valor ?? string.Empty)
                    .Trim()
                    .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        }

        // Exige una dirección completa y comprueba que la localidad pertenezca
        // a la provincia seleccionada antes de crear o modificar el cliente.
        private string? ValidarDireccionObligatoria(
            int? idProvincia,
            int? idLocalidad,
            string? calle)
        {
            if (!idProvincia.HasValue || idProvincia.Value <= 0 ||
                !ObtenerProvincias().Any(provincia => provincia.Id == idProvincia.Value))
            {
                return "Seleccioná una provincia válida.";
            }

            if (!idLocalidad.HasValue || idLocalidad.Value <= 0 ||
                !ObtenerLocalidades(idProvincia.Value).Any(localidad => localidad.Id == idLocalidad.Value))
            {
                return "Seleccioná o ingresá una localidad válida para la provincia.";
            }

            if (string.IsNullOrWhiteSpace(calle))
            {
                return "Ingresá la calle de la dirección.";
            }

            if (calle.Trim().Length > 150)
            {
                return "La calle supera el máximo de 150 caracteres.";
            }

            return null;
        }

        // Centraliza las reglas reutilizadas por alta y modificación antes de Datos.
        private static string? ValidarDatosBasicos(
            string nombre, string apellido, string documento, string? correo, string? telefono)
        {
            nombre = (nombre ?? string.Empty).Trim();
            apellido = (apellido ?? string.Empty).Trim();
            documento = (documento ?? string.Empty).Trim();
            correo = (correo ?? string.Empty).Trim();
            telefono = (telefono ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre es obligatorio.";
            if (nombre.Length > 100) return "El nombre supera el máximo de 100 caracteres.";
            if (!EsNombreValido(nombre)) return "El nombre contiene caracteres no válidos.";
            if (string.IsNullOrWhiteSpace(apellido)) return "El apellido es obligatorio.";
            if (apellido.Length > 100) return "El apellido supera el máximo de 100 caracteres.";
            if (!EsNombreValido(apellido)) return "El apellido contiene caracteres no válidos.";
            if (string.IsNullOrWhiteSpace(documento)) return "El documento es obligatorio.";
            if (documento.Length > 20 || !documento.All(char.IsDigit))
                return "El documento debe contener solo números y hasta 20 caracteres.";
            if (!string.IsNullOrWhiteSpace(correo) &&
                (correo.Length > 150 || !EsCorreoValido(correo)))
                return "El correo ingresado no es válido.";
            if (!string.IsNullOrWhiteSpace(telefono) &&
                (telefono.Length > 30 || !telefono.Any(char.IsDigit) || !EsTelefonoValido(telefono)))
                return "El teléfono debe contener al menos un dígito y solo caracteres válidos.";

            return null;
        }

        // Permite letras Unicode, espacios, apóstrofes y guiones en nombres.
        private static bool EsNombreValido(string valor)
        {
            return Regex.IsMatch(valor, @"^[\p{L}\s'-]+$");
        }

        // Restringe el teléfono a los caracteres admitidos por el formulario.
        private static bool EsTelefonoValido(string telefono)
        {
            return Regex.IsMatch(telefono, @"^[0-9+\-\s()]+$");
        }

        // Verifica el formato del correo opcional sin aceptar valores incompletos.
        private static bool EsCorreoValido(string correo)
        {
            try
            {
                if (!Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return false;
                }

                MailAddress direccion = new MailAddress(correo);
                return string.Equals(direccion.Address, correo, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
