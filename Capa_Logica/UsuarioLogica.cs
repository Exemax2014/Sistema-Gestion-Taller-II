using System.Net.Mail;
using System.Text.RegularExpressions;
using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // MODELOS DE LÓGICA - USUARIOS
    // ============================================================

    public enum EstadoUsuarioFiltro
    {
        Todos = 0,
        Activos = 1,
        Inactivos = 2
    }


    public class UsuarioListadoModelo
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;

        public int IdPerfil { get; set; }
        public string Perfil { get; set; } = string.Empty;

        public int? IdSucursal { get; set; }
        public string Sucursal { get; set; } = string.Empty;

        public bool Activo { get; set; }

        public string Estado =>
            Activo
                ? "Activo"
                : "Inactivo";
    }


    public class UsuarioDetalleModelo
    {
        public int IdUsuario { get; set; }

        public int IdPerfil { get; set; }
        public int? IdSucursal { get; set; }
        public int? IdDireccion { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }

        public string Perfil { get; set; } = string.Empty;
        public string Sucursal { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }


    public class UsuarioGuardarModelo
    {
        public int IdPerfil { get; set; }
        public int? IdSucursal { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }

        public int? IdDireccion { get; set; }

        // Se utiliza solamente al registrar un usuario nuevo.
        public string Contrasena { get; set; } = string.Empty;
    }


    public class PerfilUsuarioModelo
    {
        public int IdPerfil { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        // true:
        // el perfil puede trabajar sin una sucursal fija.
        public bool AlcanceGlobal { get; set; }
    }


    public class SucursalUsuarioModelo
    {
        public int IdSucursal { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }


    public class ResultadoUsuario
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdGenerado { get; set; }

        public bool Exitoso =>
            Codigo == 0;
    }


    // ============================================================
    // Clase: UsuarioLogica
    //
    // Responsabilidad:
    // Autenticación y reglas de negocio del módulo Usuarios.
    //
    // La Vista consulta esta clase.
    // Esta clase consulta Capa_Datos.
    //
    // No ejecuta SQL ni muestra MessageBox.
    // ============================================================

    public class UsuarioLogica
    {
        private readonly UsuarioDatos usuarioDatos;
        private readonly SucursalLogica sucursalLogica;


        public UsuarioLogica()
        {
            usuarioDatos =
                new UsuarioDatos();

            sucursalLogica =
                new SucursalLogica();

        }


        // ========================================================
        // AUTENTICACIÓN
        // ========================================================

        public bool IniciarSesion(
            string nombreUsuario,
            string contrasena,
            out string mensaje)
        {
            if (string.IsNullOrWhiteSpace(
                nombreUsuario))
            {
                mensaje =
                    "Debe ingresar el nombre de usuario.";

                return false;
            }


            if (string.IsNullOrWhiteSpace(
                contrasena))
            {
                mensaje =
                    "Debe ingresar la contraseña.";

                return false;
            }


            UsuarioLoginDatos? usuario =
                usuarioDatos.BuscarPorNombreUsuario(
                    nombreUsuario.Trim()
                );


            if (usuario == null)
            {
                mensaje =
                    "Usuario o contraseña incorrectos.";

                return false;
            }


            bool contrasenaCorrecta =
                PasswordHelper.Verificar(
                    contrasena,
                    usuario.ContrasenaHash
                );


            if (!contrasenaCorrecta)
            {
                mensaje =
                    "Usuario o contraseña incorrectos.";

                return false;
            }


            List<string> funcionalidades =
                usuarioDatos.ObtenerFuncionalidadesPerfil(
                    usuario.IdPerfil
                );


            SesionActual.Iniciar(
                usuario.IdUsuario,
                usuario.IdPerfil,
                usuario.IdSucursal,
                usuario.Nombre,
                usuario.Apellido,
                usuario.NombreUsuario,
                usuario.Perfil,
                usuario.Sucursal,
                funcionalidades,
                usuario.AlcanceGlobal
            );


            mensaje =
                "Inicio de sesión correcto.";

            return true;
        }


        // ========================================================
        // PERMISOS DEL MÓDULO
        // ========================================================

        public bool PuedeVerUsuarios()
        {
            return SesionActual.TienePermiso(
                "USUARIOS_VER"
            );
        }


        public bool PuedeCrearUsuario()
        {
            return SesionActual.TienePermiso(
                "USUARIOS_ALTA"
            );
        }


        public bool PuedeModificarUsuario()
        {
            return SesionActual.TienePermiso(
                "USUARIOS_MODIFICAR"
            );
        }


        public bool PuedeEliminarUsuario()
        {
            return SesionActual.TienePermiso(
                "USUARIOS_BAJA"
            );
        }


        public bool PuedeReactivarUsuario()
        {
            return SesionActual.TienePermiso(
                "USUARIOS_ALTA"
            );
        }


        public bool PuedeGestionarPermisos()
        {
            return SesionActual.TienePermiso(
                "PERMISOS_GESTIONAR"
            );
        }


        public bool PerfilTienePermiso(
            int idPerfil,
            string codigoFuncionalidad)
        {
            if (
                idPerfil <= 0
                ||
                string.IsNullOrWhiteSpace(
                    codigoFuncionalidad))
            {
                return false;
            }


            return usuarioDatos
                .ObtenerFuncionalidadesPerfil(
                    idPerfil
                )
                .Any(
                    codigo =>
                        string.Equals(
                            codigo,
                            codigoFuncionalidad.Trim(),
                            StringComparison.OrdinalIgnoreCase
                        )
                );
        }


        // ========================================================
        // LISTADO
        // ========================================================

        public List<UsuarioListadoModelo> Listar(
            string texto,
            int? idPerfil,
            EstadoUsuarioFiltro estado)
        {
            if (!PuedeVerUsuarios())
            {
                return new List<UsuarioListadoModelo>();
            }


            bool? activo =
                estado switch
                {
                    EstadoUsuarioFiltro.Activos => true,
                    EstadoUsuarioFiltro.Inactivos => false,
                    _ => null
                };


            List<UsuarioListadoDatos> usuarios =
                usuarioDatos.Listar(
                    activo
                );


            IEnumerable<UsuarioListadoDatos> consulta =
                usuarios;


            string textoBusqueda =
                (texto ?? string.Empty)
                .Trim();


            if (!string.IsNullOrWhiteSpace(
                textoBusqueda))
            {
                consulta =
                    consulta.Where(
                        u =>
                            Contiene(
                                u.NombreUsuario,
                                textoBusqueda
                            )
                            ||
                            Contiene(
                                u.Nombre,
                                textoBusqueda
                            )
                            ||
                            Contiene(
                                u.Apellido,
                                textoBusqueda
                            )
                            ||
                            Contiene(
                                u.Dni,
                                textoBusqueda
                            )
                            ||
                            Contiene(
                                u.Correo,
                                textoBusqueda
                            )
                    );
            }


            if (idPerfil.HasValue)
            {
                consulta =
                    consulta.Where(
                        u =>
                            u.IdPerfil ==
                            idPerfil.Value
                    );
            }


            return consulta
                .Select(
                    u =>
                        new UsuarioListadoModelo
                        {
                            IdUsuario =
                                u.IdUsuario,

                            NombreUsuario =
                                u.NombreUsuario,

                            Nombre =
                                u.Nombre,

                            Apellido =
                                u.Apellido,

                            IdPerfil =
                                u.IdPerfil,

                            Perfil =
                                u.Perfil,

                            IdSucursal =
                                u.IdSucursal,

                            Sucursal =
                                u.Sucursal,

                            Activo =
                                u.Activo
                        }
                )
                .ToList();
        }


        // ========================================================
        // DETALLE
        // ========================================================

        public UsuarioDetalleModelo? ObtenerPorId(
            int idUsuario)
        {
            if (
                !PuedeVerUsuarios()
                ||
                idUsuario <= 0)
            {
                return null;
            }


            UsuarioDetalleDatos? usuario =
                usuarioDatos.ObtenerPorId(
                    idUsuario
                );


            if (usuario == null)
            {
                return null;
            }


            return new UsuarioDetalleModelo
            {
                IdUsuario =
                    usuario.IdUsuario,

                IdPerfil =
                    usuario.IdPerfil,

                IdSucursal =
                    usuario.IdSucursal,

                IdDireccion =
                    usuario.IdDireccion,

                Nombre =
                    usuario.Nombre,

                Apellido =
                    usuario.Apellido,

                Dni =
                    usuario.Dni,

                Telefono =
                    usuario.Telefono,

                NombreUsuario =
                    usuario.NombreUsuario,

                Correo =
                    usuario.Correo,

                Sexo =
                    usuario.Sexo,

                FechaNacimiento =
                    usuario.FechaNacimiento,

                Perfil =
                    usuario.Perfil,

                Sucursal =
                    usuario.Sucursal,

                Activo =
                    usuario.Activo
            };
        }


        // ========================================================
        // PERFILES
        // ========================================================

        public List<PerfilUsuarioModelo> ObtenerPerfiles()
        {
            if (!PuedeVerUsuarios())
            {
                return new List<PerfilUsuarioModelo>();
            }


            return usuarioDatos
                .ListarPerfiles()
                .Select(
                    p =>
                        new PerfilUsuarioModelo
                        {
                            IdPerfil =
                                p.IdPerfil,

                            Nombre =
                                p.Nombre,

                            Descripcion =
                                p.Descripcion,

                            AlcanceGlobal =
                                p.AlcanceGlobal
                        }
                )
                .ToList();
        }


        public PerfilUsuarioModelo? ObtenerPerfil(
            int idPerfil)
        {
            if (idPerfil <= 0)
            {
                return null;
            }


            PerfilDatos? perfil =
                usuarioDatos
                    .ListarPerfiles()
                    .FirstOrDefault(
                        p =>
                            p.IdPerfil ==
                            idPerfil
                    );


            if (perfil == null)
            {
                return null;
            }


            return new PerfilUsuarioModelo
            {
                IdPerfil =
                    perfil.IdPerfil,

                Nombre =
                    perfil.Nombre,

                Descripcion =
                    perfil.Descripcion,

                AlcanceGlobal =
                    perfil.AlcanceGlobal
            };
        }


        // ========================================================
        // SUCURSALES
        // ========================================================

        public List<SucursalUsuarioModelo> ObtenerSucursalesDisponibles()
        {
            return sucursalLogica
                .ObtenerSucursalesDisponibles()
                .Select(
                    s =>
                        new SucursalUsuarioModelo
                        {
                            IdSucursal =
                                s.IdSucursal,

                            Nombre =
                                s.Nombre
                        }
                )
                .ToList();
        }


        // ========================================================
        // ALTA
        // ========================================================

        public ResultadoUsuario Crear(
            UsuarioGuardarModelo usuario)
        {
            if (!PuedeCrearUsuario())
            {
                return ResultadoNoPermitido(
                    "No tiene permiso para registrar usuarios."
                );
            }


            ResultadoUsuario validacion =
                ValidarUsuario(
                    usuario,
                    esAlta: true
                );


            if (!validacion.Exitoso)
            {
                return validacion;
            }


            PerfilUsuarioModelo? perfil =
                ObtenerPerfil(
                    usuario.IdPerfil
                );


            if (perfil == null)
            {
                return ResultadoInvalido(
                    "El perfil seleccionado no existe."
                );
            }


            int? idSucursal =
                ResolverSucursal(
                    perfil,
                    usuario.IdSucursal,
                    out ResultadoUsuario resultadoSucursal
                );


            if (!resultadoSucursal.Exitoso)
            {
                return resultadoSucursal;
            }


            UsuarioGuardarDatos datos =
                CrearDatosGuardar(
                    usuario,
                    idSucursal
                );


            datos.ContrasenaHash =
                PasswordHelper.GenerarHash(
                    usuario.Contrasena
                );


            ResultadoUsuarioDatos resultado =
                usuarioDatos.Alta(
                    datos,
                    SesionActual.IdUsuario,
                    SesionActual.IdSucursal ?? SesionActual.IdSucursalOperativa
                );


            return ConvertirResultado(resultado);
        }


        // ========================================================
        // MODIFICACIÓN
        // ========================================================

        public ResultadoUsuario Modificar(
            int idUsuario,
            UsuarioGuardarModelo usuario)
        {
            if (!PuedeModificarUsuario())
            {
                return ResultadoNoPermitido(
                    "No tiene permiso para modificar usuarios."
                );
            }


            if (idUsuario <= 0)
            {
                return ResultadoInvalido(
                    "El usuario indicado no es válido."
                );
            }


            ResultadoUsuario validacion =
                ValidarUsuario(
                    usuario,
                    esAlta: false
                );


            if (!validacion.Exitoso)
            {
                return validacion;
            }


            PerfilUsuarioModelo? perfil =
                ObtenerPerfil(
                    usuario.IdPerfil
                );


            if (perfil == null)
            {
                return ResultadoInvalido(
                    "El perfil seleccionado no existe."
                );
            }


            int? idSucursal =
                ResolverSucursal(
                    perfil,
                    usuario.IdSucursal,
                    out ResultadoUsuario resultadoSucursal
                );


            if (!resultadoSucursal.Exitoso)
            {
                return resultadoSucursal;
            }


            UsuarioGuardarDatos datos =
                CrearDatosGuardar(
                    usuario,
                    idSucursal
                );


            ResultadoUsuarioDatos resultado =
                usuarioDatos.Modificar(
                    idUsuario,
                    datos,
                    SesionActual.IdUsuario,
                    SesionActual.IdSucursal ?? SesionActual.IdSucursalOperativa
                );


            return ConvertirResultado(resultado);
        }


        // ========================================================
        // BAJA LÓGICA
        // ========================================================

        public ResultadoUsuario Eliminar(
            int idUsuario)
        {
            if (!PuedeEliminarUsuario())
            {
                return ResultadoNoPermitido(
                    "No tiene permiso para dar de baja usuarios."
                );
            }


            if (idUsuario <= 0)
            {
                return ResultadoInvalido(
                    "El usuario indicado no es válido."
                );
            }


            if (
                SesionActual.SesionIniciada
                &&
                SesionActual.IdUsuario ==
                idUsuario)
            {
                return ResultadoNoPermitido(
                    "No puede dar de baja al usuario con el que tiene la sesión iniciada."
                );
            }


            ResultadoUsuarioDatos resultado =
                usuarioDatos.Baja(
                    idUsuario,
                    SesionActual.IdUsuario,
                    SesionActual.IdSucursal ?? SesionActual.IdSucursalOperativa
                );


            return ConvertirResultado(resultado);
        }


        // ========================================================
        // REACTIVAR USUARIO
        // ========================================================

        public ResultadoUsuario Reactivar(
            int idUsuario)
        {
            if (!PuedeReactivarUsuario())
            {
                return ResultadoNoPermitido(
                    "No tiene permiso para reactivar usuarios."
                );
            }


            if (idUsuario <= 0)
            {
                return ResultadoInvalido(
                    "El usuario indicado no es válido."
                );
            }


            ResultadoUsuarioDatos resultado =
                usuarioDatos.Reactivar(
                    idUsuario,
                    SesionActual.IdUsuario,
                    SesionActual.IdSucursal ?? SesionActual.IdSucursalOperativa
                );


            return ConvertirResultado(resultado);
        }


        // ========================================================
        // VALIDACIONES DE NEGOCIO
        // ========================================================

        private ResultadoUsuario ValidarUsuario(
            UsuarioGuardarModelo usuario,
            bool esAlta)
        {
            if (usuario == null)
            {
                return ResultadoInvalido(
                    "Debe ingresar los datos del usuario."
                );
            }


            usuario.Nombre =
                (usuario.Nombre ?? string.Empty)
                .Trim();

            usuario.Apellido =
                (usuario.Apellido ?? string.Empty)
                .Trim();

            usuario.Dni =
                (usuario.Dni ?? string.Empty)
                .Trim();

            usuario.Telefono =
                (usuario.Telefono ?? string.Empty)
                .Trim();

            usuario.NombreUsuario =
                (usuario.NombreUsuario ?? string.Empty)
                .Trim();

            usuario.Correo =
                (usuario.Correo ?? string.Empty)
                .Trim();

            usuario.Sexo =
                (usuario.Sexo ?? string.Empty)
                .Trim();

            usuario.Contrasena =
                usuario.Contrasena
                ?? string.Empty;


            if (string.IsNullOrWhiteSpace(
                usuario.Nombre))
            {
                return ResultadoInvalido(
                    "El nombre es obligatorio."
                );
            }


            if (usuario.Nombre.Length > 100)
            {
                return ResultadoInvalido(
                    "El nombre no puede superar los 100 caracteres."
                );
            }


            if (!EsNombreValido(
                usuario.Nombre))
            {
                return ResultadoInvalido(
                    "El nombre contiene caracteres no válidos."
                );
            }


            if (string.IsNullOrWhiteSpace(
                usuario.Apellido))
            {
                return ResultadoInvalido(
                    "El apellido es obligatorio."
                );
            }


            if (usuario.Apellido.Length > 100)
            {
                return ResultadoInvalido(
                    "El apellido no puede superar los 100 caracteres."
                );
            }


            if (!EsNombreValido(
                usuario.Apellido))
            {
                return ResultadoInvalido(
                    "El apellido contiene caracteres no válidos."
                );
            }


            if (string.IsNullOrWhiteSpace(
                usuario.Dni))
            {
                return ResultadoInvalido(
                    "El DNI es obligatorio."
                );
            }


            if (
                usuario.Dni.Length > 20
                ||
                !usuario.Dni.All(char.IsDigit))
            {
                return ResultadoInvalido(
                    "El DNI debe contener solamente números y no puede superar los 20 dígitos."
                );
            }


            if (
                !string.IsNullOrWhiteSpace(
                    usuario.Telefono)
                &&
                (
                    usuario.Telefono.Length > 30
                    ||
                    !usuario.Telefono.Any(char.IsDigit)
                    ||
                    !EsTelefonoValido(
                        usuario.Telefono)
                ))
            {
                return ResultadoInvalido(
                    "El teléfono debe contener al menos un número, caracteres válidos y hasta 30 caracteres."
                );
            }


            if (string.IsNullOrWhiteSpace(
                usuario.NombreUsuario))
            {
                return ResultadoInvalido(
                    "El nombre de usuario es obligatorio."
                );
            }


            if (
                usuario.NombreUsuario.Length > 50
                ||
                !Regex.IsMatch(
                    usuario.NombreUsuario,
                    @"^[A-Za-z0-9._-]+$"
                ))
            {
                return ResultadoInvalido(
                    "El nombre de usuario solo puede contener letras, números, punto, guion y guion bajo."
                );
            }


            if (string.IsNullOrWhiteSpace(
                usuario.Correo))
            {
                return ResultadoInvalido(
                    "El correo es obligatorio."
                );
            }


            if (
                usuario.Correo.Length > 150
                ||
                !EsCorreoValido(
                    usuario.Correo)
            )
            {
                return ResultadoInvalido(
                    "El correo ingresado no es válido."
                );
            }


            if (
                !string.IsNullOrWhiteSpace(
                    usuario.Sexo)
                &&
                usuario.Sexo.Length > 20)
            {
                return ResultadoInvalido(
                    "El valor de sexo no puede superar los 20 caracteres."
                );
            }


            if (
                usuario.FechaNacimiento.HasValue
                &&
                usuario.FechaNacimiento.Value.Date >
                DateTime.Today)
            {
                return ResultadoInvalido(
                    "La fecha de nacimiento no puede ser futura."
                );
            }


            if (usuario.IdPerfil <= 0)
            {
                return ResultadoInvalido(
                    "Debe seleccionar un perfil."
                );
            }


            if (esAlta)
            {
                if (string.IsNullOrWhiteSpace(
                    usuario.Contrasena))
                {
                    return ResultadoInvalido(
                        "La contraseña es obligatoria."
                    );
                }


                if (
                    usuario.Contrasena.Length < 8
                    ||
                    usuario.Contrasena.Length > 100)
                {
                    return ResultadoInvalido(
                        "La contraseña debe tener entre 8 y 100 caracteres."
                    );
                }
            }


            return ResultadoCorrecto();
        }


        // ========================================================
        // REGLA PERFIL / SUCURSAL
        // ========================================================

        private int? ResolverSucursal(
            PerfilUsuarioModelo perfil,
            int? idSucursal,
            out ResultadoUsuario resultado)
        {
            if (perfil.AlcanceGlobal)
            {
                resultado =
                    ResultadoCorrecto();

                return null;
            }


            if (!idSucursal.HasValue)
            {
                resultado =
                    ResultadoInvalido(
                        "El perfil seleccionado requiere una sucursal."
                    );

                return null;
            }


            bool sucursalDisponible =
                ObtenerSucursalesDisponibles()
                    .Any(
                        s =>
                            s.IdSucursal ==
                            idSucursal.Value
                    );


            if (!sucursalDisponible)
            {
                resultado =
                    ResultadoInvalido(
                        "La sucursal seleccionada no está disponible."
                    );

                return null;
            }


            resultado =
                ResultadoCorrecto();

            return idSucursal.Value;
        }


        // ========================================================
        // CONVERSIÓN A CAPA_DATOS
        // ========================================================

        private static UsuarioGuardarDatos CrearDatosGuardar(
            UsuarioGuardarModelo usuario,
            int? idSucursal)
        {
            return new UsuarioGuardarDatos
            {
                IdPerfil =
                    usuario.IdPerfil,

                IdSucursal =
                    idSucursal,

                Nombre =
                    usuario.Nombre,

                Apellido =
                    usuario.Apellido,

                Dni =
                    usuario.Dni,

                Telefono =
                    usuario.Telefono,

                NombreUsuario =
                    usuario.NombreUsuario,

                Correo =
                    usuario.Correo,

                Sexo =
                    usuario.Sexo,

                FechaNacimiento =
                    usuario.FechaNacimiento,

                IdDireccion =
                    usuario.IdDireccion
            };
        }


        // ========================================================
        // VALIDADORES AUXILIARES
        // ========================================================

        // Permite nombres Unicode habituales y rechaza controles que puedan llegar por texto pegado.
        private static bool EsNombreValido(
            string valor)
        {
            return !valor.Any(char.IsControl)
                && Regex.IsMatch(
                    valor,
                    @"^[\p{L} '-]+$"
                );
        }


        // Acepta solo el formato de teléfono admitido por la Vista.
        private static bool EsTelefonoValido(
            string telefono)
        {
            return Regex.IsMatch(
                telefono,
                @"^[0-9+\-\s()]+$"
            );
        }


        // Requiere una estructura mínima de correo antes de delegar el análisis a MailAddress.
        private static bool EsCorreoValido(
            string correo)
        {
            try
            {
                if (!Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return false;
                }

                MailAddress direccion =
                    new MailAddress(
                        correo
                    );

                return string.Equals(
                    direccion.Address,
                    correo,
                    StringComparison.OrdinalIgnoreCase
                );
            }
            catch
            {
                return false;
            }
        }


        private static bool Contiene(
            string valor,
            string texto)
        {
            return (
                valor
                ?? string.Empty
            ).Contains(
                texto,
                StringComparison.OrdinalIgnoreCase
            );
        }


        // ========================================================
        // RESULTADOS
        // ========================================================

        private static ResultadoUsuario ConvertirResultado(
            ResultadoUsuarioDatos resultado)
        {
            return new ResultadoUsuario
            {
                Codigo =
                    resultado.Codigo,

                Mensaje =
                    resultado.Mensaje,

                IdGenerado =
                    resultado.IdGenerado
            };
        }


        private static ResultadoUsuario ResultadoCorrecto()
        {
            return new ResultadoUsuario
            {
                Codigo = 0,
                Mensaje =
                    "Operación válida."
            };
        }


        private static ResultadoUsuario ResultadoInvalido(
            string mensaje)
        {
            return new ResultadoUsuario
            {
                Codigo = 3,
                Mensaje =
                    mensaje
            };
        }


        private static ResultadoUsuario ResultadoNoPermitido(
            string mensaje)
        {
            return new ResultadoUsuario
            {
                Codigo = 5,
                Mensaje =
                    mensaje
            };
        }
    }
}
