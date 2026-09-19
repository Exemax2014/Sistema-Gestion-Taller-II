using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // MODELOS DE LÓGICA - PERFILES Y PERMISOS
    // ============================================================

    public class PerfilGestionModelo
    {
        public int IdPerfil { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public bool AlcanceGlobal { get; set; }
        public bool EsAdministrador { get; set; }
    }


    public class FuncionalidadPerfilModelo
    {
        public int IdFuncionalidad { get; set; }

        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public bool Asignada { get; set; }
        public bool Bloqueada { get; set; }
    }


    public class ResultadoPerfil
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdGenerado { get; set; }

        public bool Exitoso =>
            Codigo == 0;
    }

    // Representa un perfil destino seleccionable dentro de la configuración de avisos.
    public class PerfilAvisoDestinoModelo
    {
        public int IdPerfil { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Asignado { get; set; }
    }


    // ============================================================
    // Clase: PerfilLogica
    //
    // Responsabilidad:
    // Reglas de negocio para administrar tipos de usuario
    // y sus permisos.
    //
    // La Vista consulta esta clase.
    // Esta clase consulta Capa_Datos.
    //
    // No ejecuta SQL ni muestra MessageBox.
    // ============================================================

    public class PerfilLogica
    {
        private readonly PerfilGestionDatos perfilDatos;


        public PerfilLogica()
        {
            perfilDatos =
                new PerfilGestionDatos();
        }


        // ========================================================
        // PERMISO DEL MÓDULO
        // ========================================================

        public bool PuedeGestionarPermisos()
        {
            return SesionActual.TienePermiso(
                "PERMISOS_GESTIONAR"
            );
        }


        // ========================================================
        // LISTADO DE PERFILES
        // ========================================================

        public List<PerfilGestionModelo> ListarPerfiles()
        {
            if (!PuedeGestionarPermisos())
            {
                return new List<PerfilGestionModelo>();
            }


            return perfilDatos
                .Listar()
                .Select(
                    p =>
                        new PerfilGestionModelo
                        {
                            IdPerfil =
                                p.IdPerfil,

                            Nombre =
                                p.Nombre,

                            Descripcion =
                                p.Descripcion,

                            AlcanceGlobal =
                                p.AlcanceGlobal,

                            EsAdministrador =
                                p.AlcanceGlobal
                        }
                )
                .ToList();
        }


        // ========================================================
        // DETALLE DE PERFIL
        // ========================================================

        public PerfilGestionModelo? ObtenerPorId(
            int idPerfil)
        {
            if (
                !PuedeGestionarPermisos()
                ||
                idPerfil <= 0)
            {
                return null;
            }


            PerfilDetalleDatos? perfil =
                perfilDatos.ObtenerPorId(
                    idPerfil
                );


            if (perfil == null)
            {
                return null;
            }


            return new PerfilGestionModelo
            {
                IdPerfil =
                    perfil.IdPerfil,

                Nombre =
                    perfil.Nombre,

                Descripcion =
                    perfil.Descripcion,

                AlcanceGlobal =
                    perfil.AlcanceGlobal,

                EsAdministrador =
                    perfil.AlcanceGlobal
            };
        }


        // ========================================================
        // FUNCIONALIDADES DISPONIBLES
        // ========================================================

        public List<FuncionalidadPerfilModelo>
            ObtenerFuncionalidadesDisponibles()
        {
            if (!PuedeGestionarPermisos())
            {
                return new List<FuncionalidadPerfilModelo>();
            }


            return perfilDatos
                .ListarFuncionalidades()
                .Select(
                    f =>
                        new FuncionalidadPerfilModelo
                        {
                            IdFuncionalidad =
                                f.IdFuncionalidad,

                            Codigo =
                                f.Codigo,

                            Nombre =
                                f.Nombre,

                            Descripcion =
                                f.Descripcion,

                            Asignada =
                                false,

                            Bloqueada =
                                f.Codigo ==
                                "PERMISOS_GESTIONAR"
                        }
                )
                .ToList();
        }


        public List<FuncionalidadPerfilModelo>
            ObtenerFuncionalidadesPerfil(
                int idPerfil)
        {
            if (
                !PuedeGestionarPermisos()
                ||
                idPerfil <= 0)
            {
                return new List<FuncionalidadPerfilModelo>();
            }


            return perfilDatos
                .ListarFuncionalidadesPorPerfil(
                    idPerfil
                )
                .Select(
                    f =>
                        new FuncionalidadPerfilModelo
                        {
                            IdFuncionalidad =
                                f.IdFuncionalidad,

                            Codigo =
                                f.Codigo,

                            Nombre =
                                f.Nombre,

                            Descripcion =
                                f.Descripcion,

                            Asignada =
                                f.Asignada,

                            Bloqueada =
                                f.Bloqueada
                        }
                )
                .ToList();
        }


        // ========================================================
        // ALTA
        // ========================================================

        // Guarda datos y funcionalidades en una sola llamada para no dejar perfiles parciales.
        public ResultadoPerfil Guardar(
            int? idPerfil,
            string nombre,
            string descripcion,
            IEnumerable<int> funcionalidadesSeleccionadas)
        {
            if (!PuedeGestionarPermisos())
            {
                return ResultadoNoPermitido(
                    "No tiene permiso para administrar tipos de usuario."
                );
            }

            if (idPerfil.HasValue)
            {
                if (idPerfil.Value <= 0)
                {
                    return ResultadoInvalido(
                        "El tipo de usuario indicado no es válido."
                    );
                }

                PerfilGestionModelo? perfilActual =
                    ObtenerPorId(idPerfil.Value);

                if (perfilActual == null)
                {
                    return ResultadoInvalido(
                        "El tipo de usuario no existe."
                    );
                }

                if (perfilActual.AlcanceGlobal)
                {
                    return ResultadoNoPermitido(
                        "El perfil global del sistema no puede modificarse."
                    );
                }
            }

            string? errorDatos = ValidarDatosPerfil(
                nombre,
                descripcion
            );

            if (errorDatos != null)
            {
                return ResultadoInvalido(errorDatos);
            }

            List<int> seleccionadas =
                (funcionalidadesSeleccionadas
                    ?? Enumerable.Empty<int>())
                .ToList();

            if (seleccionadas.Any(id => id <= 0))
            {
                return ResultadoInvalido(
                    "La lista de funcionalidades contiene valores no válidos."
                );
            }

            if (seleccionadas.Count != seleccionadas.Distinct().Count())
            {
                return ResultadoInvalido(
                    "La lista de funcionalidades contiene valores duplicados."
                );
            }

            List<FuncionalidadPerfilModelo> disponibles =
                ObtenerFuncionalidadesDisponibles();

            HashSet<int> idsDisponibles =
                disponibles
                    .Select(f => f.IdFuncionalidad)
                    .ToHashSet();

            if (seleccionadas.Any(id => !idsDisponibles.Contains(id)))
            {
                return ResultadoInvalido(
                    "Se seleccionó una funcionalidad que no está disponible."
                );
            }

            FuncionalidadPerfilModelo? permisoGestion =
                disponibles.FirstOrDefault(
                    f => f.Codigo == "PERMISOS_GESTIONAR"
                );

            if (
                permisoGestion != null
                && seleccionadas.Contains(permisoGestion.IdFuncionalidad))
            {
                return ResultadoNoPermitido(
                    "La administración de permisos es exclusiva del perfil global."
                );
            }

            string? errorAlcanceReportes =
                ValidarAlcancesReportes(
                    seleccionadas,
                    disponibles,
                    false
                );

            if (errorAlcanceReportes != null)
            {
                return ResultadoInvalido(
                    errorAlcanceReportes
                );
            }

            ResultadoPerfilDatos resultado =
                perfilDatos.Guardar(
                    idPerfil,
                    NormalizarNombre(nombre),
                    NormalizarDescripcion(descripcion),
                    seleccionadas
                );

            return ConvertirResultado(resultado);
        }

        // Crea un perfil con datos normalizados y validados antes de llegar a Datos.
        public ResultadoPerfil Crear(
            string nombre,
            string descripcion)
        {
            if (!PuedeGestionarPermisos())
            {
                return ResultadoNoPermitido(
                    "No tiene permiso para crear tipos de usuario."
                );
            }


            string? error = ValidarDatosPerfil(nombre, descripcion);

            if (error != null)
            {
                return ResultadoInvalido(error);
            }

            nombre = NormalizarNombre(nombre);
            descripcion = NormalizarDescripcion(descripcion);

            ResultadoPerfilDatos resultado =
                perfilDatos.Alta(
                    nombre,
                    descripcion
                );


            return ConvertirResultado(
                resultado
            );
        }


        // ========================================================
        // MODIFICACIÓN
        // ========================================================

        // Modifica un perfil no global con las mismas reglas autoritativas del alta.
        public ResultadoPerfil Modificar(
            int idPerfil,
            string nombre,
            string descripcion)
        {
            if (!PuedeGestionarPermisos())
            {
                return ResultadoNoPermitido(
                    "No tiene permiso para modificar tipos de usuario."
                );
            }


            if (idPerfil <= 0)
            {
                return ResultadoInvalido(
                    "El tipo de usuario indicado no es válido."
                );
            }


            PerfilGestionModelo? perfilActual =
                ObtenerPorId(
                    idPerfil
                );


            if (perfilActual == null)
            {
                return ResultadoInvalido(
                    "El tipo de usuario no existe."
                );
            }


            if (perfilActual.AlcanceGlobal)
            {
                return ResultadoNoPermitido(
                    "El perfil global del sistema no puede modificarse."
                );
            }


            string? error = ValidarDatosPerfil(nombre, descripcion);

            if (error != null)
            {
                return ResultadoInvalido(error);
            }

            nombre = NormalizarNombre(nombre);
            descripcion = NormalizarDescripcion(descripcion);

            ResultadoPerfilDatos resultado =
                perfilDatos.Modificar(
                    idPerfil,
                    nombre,
                    descripcion
                );


            return ConvertirResultado(
                resultado
            );
        }


        // ========================================================
        // GUARDAR PERMISOS
        //
        // Se compara lo seleccionado en la Vista con lo que existe
        // actualmente en SQL.
        //
        // Cada asignación o retiro se realiza mediante los
        // procedimientos almacenados correspondientes.
        // ========================================================

        public ResultadoPerfil GuardarPermisos(
            int idPerfil,
            IEnumerable<int> funcionalidadesSeleccionadas)
        {
            if (!PuedeGestionarPermisos())
            {
                return ResultadoNoPermitido(
                    "No tiene permiso para administrar permisos."
                );
            }


            if (idPerfil <= 0)
            {
                return ResultadoInvalido(
                    "El tipo de usuario indicado no es válido."
                );
            }


            PerfilGestionModelo? perfil =
                ObtenerPorId(
                    idPerfil
                );


            if (perfil == null)
            {
                return ResultadoInvalido(
                    "El tipo de usuario no existe."
                );
            }


            /*
                El perfil global mantiene siempre todas las
                funcionalidades y no se edita manualmente.
            */
            if (perfil.AlcanceGlobal)
            {
                ResultadoPerfilDatos sincronizacion =
                    perfilDatos
                        .SincronizarAdministrador();


                return ConvertirResultado(
                    sincronizacion
                );
            }


            HashSet<int> seleccionadas =
                new HashSet<int>(
                    funcionalidadesSeleccionadas
                    ?? Enumerable.Empty<int>()
                );


            List<FuncionalidadPerfilModelo> disponibles =
                ObtenerFuncionalidadesDisponibles();


            HashSet<int> idsDisponibles =
                disponibles
                    .Select(
                        f =>
                            f.IdFuncionalidad
                    )
                    .ToHashSet();


            if (
                seleccionadas.Any(
                    id =>
                        !idsDisponibles.Contains(
                            id
                        )
                )
            )
            {
                return ResultadoInvalido(
                    "Se seleccionó una funcionalidad que no está disponible."
                );
            }


            FuncionalidadPerfilModelo? permisoGestion =
                disponibles
                    .FirstOrDefault(
                        f =>
                            f.Codigo ==
                            "PERMISOS_GESTIONAR"
                    );


            if (
                permisoGestion != null
                &&
                seleccionadas.Contains(
                    permisoGestion.IdFuncionalidad
                )
            )
            {
                return ResultadoNoPermitido(
                    "La administración de permisos es exclusiva del perfil global."
                );
            }

            string? errorAlcanceReportes =
                ValidarAlcancesReportes(
                    seleccionadas,
                    disponibles,
                    perfil.AlcanceGlobal
                );

            if (errorAlcanceReportes != null)
            {
                return ResultadoInvalido(
                    errorAlcanceReportes
                );
            }


            ResultadoPerfilDatos resultado =
                perfilDatos
                    .GuardarFuncionalidades(
                        idPerfil,
                        seleccionadas
                    );


            return ConvertirResultado(
                resultado
            );
        }


        // ========================================================
        // BAJA LÓGICA
        // ========================================================

        public ResultadoPerfil Eliminar(
            int idPerfil)
        {
            if (!PuedeGestionarPermisos())
            {
                return ResultadoNoPermitido(
                    "No tiene permiso para dar de baja tipos de usuario."
                );
            }


            if (idPerfil <= 0)
            {
                return ResultadoInvalido(
                    "El tipo de usuario indicado no es válido."
                );
            }


            PerfilGestionModelo? perfil =
                ObtenerPorId(
                    idPerfil
                );


            if (perfil == null)
            {
                return ResultadoInvalido(
                    "El tipo de usuario no existe."
                );
            }


            if (perfil.AlcanceGlobal)
            {
                return ResultadoNoPermitido(
                    "El perfil global del sistema no puede darse de baja."
                );
            }


            ResultadoPerfilDatos resultado =
                perfilDatos.Baja(
                    idPerfil
                );


            return ConvertirResultado(
                resultado
            );
        }


        // ========================================================
        // SINCRONIZACIÓN DEL ADMINISTRADOR
        // ========================================================

        public ResultadoPerfil SincronizarAdministrador()
        {
            if (!PuedeGestionarPermisos())
            {
                return ResultadoNoPermitido(
                    "No tiene permiso para administrar permisos."
                );
            }


            ResultadoPerfilDatos resultado =
                perfilDatos
                    .SincronizarAdministrador();


            return ConvertirResultado(
                resultado
            );
        }


        // ========================================================
        // RESULTADOS
        // ========================================================

        private static ResultadoPerfil ConvertirResultado(
            ResultadoPerfilDatos resultado)
        {
            return new ResultadoPerfil
            {
                Codigo =
                    resultado.Codigo,

                Mensaje =
                    resultado.Mensaje,

                IdGenerado =
                    resultado.IdGenerado
            };
        }


        // Valida los campos editables del perfil sin decidir sus permisos ni funcionalidades.
        public string? ValidarDatosPerfil(string? nombre, string? descripcion)
        {
            string nombreOriginal = nombre ?? string.Empty;
            string descripcionOriginal = descripcion ?? string.Empty;
            string nombreNormalizado = NormalizarNombre(nombreOriginal);
            string descripcionNormalizada = NormalizarDescripcion(descripcionOriginal);

            if (string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                return "El nombre del tipo de usuario es obligatorio.";
            }

            if (nombreOriginal.Any(char.IsControl) ||
                !nombreNormalizado.All(caracter => char.IsLetterOrDigit(caracter) ||
                    caracter == ' ' || caracter == '-' || caracter == '\''))
            {
                return "El nombre solo puede incluir letras, números, espacios, guiones y apóstrofes.";
            }

            if (nombreNormalizado.Length > 50)
            {
                return "El nombre del tipo de usuario no puede superar los 50 caracteres.";
            }

            if (descripcionOriginal.Any(char.IsControl))
            {
                return "La descripción no puede contener caracteres de control.";
            }

            if (descripcionNormalizada.Length > 200)
            {
                return "La descripción no puede superar los 200 caracteres.";
            }

            return null;
        }

        // Obtiene los perfiles activos que pueden configurarse como destino del perfil indicado.
        public List<PerfilAvisoDestinoModelo> ObtenerDestinosAviso(int idPerfil)
        {
            if (!PuedeGestionarPermisos() || idPerfil <= 0) return new List<PerfilAvisoDestinoModelo>();
            return perfilDatos.ListarDestinosAviso(idPerfil)
                .Select(destino => new PerfilAvisoDestinoModelo
                {
                    IdPerfil = destino.IdPerfil,
                    Nombre = destino.Nombre,
                    Asignado = destino.Asignado
                }).ToList();
        }

        // Valida y guarda relaciones emisor-destino sin permitir IDs inválidos o duplicados.
        public ResultadoPerfil GuardarDestinosAviso(int idPerfil, IEnumerable<int> idsDestinos)
        {
            if (!PuedeGestionarPermisos()) return ResultadoNoPermitido("No tiene permiso para administrar destinos de avisos.");
            if (idPerfil <= 0) return ResultadoInvalido("El tipo de usuario indicado no es válido.");
            List<int> destinos = (idsDestinos ?? Enumerable.Empty<int>()).ToList();
            if (destinos.Any(id => id <= 0) || destinos.Count != destinos.Distinct().Count()) return ResultadoInvalido("Los perfiles destino seleccionados no son válidos.");
            if (destinos.Contains(idPerfil)) return ResultadoInvalido("Un perfil no puede enviarse avisos a sí mismo.");
            return ConvertirResultado(perfilDatos.GuardarDestinosAviso(idPerfil, destinos));
        }


        // Garantiza que los permisos de alcance de Reportes no formen combinaciones ambiguas.
        private static string? ValidarAlcancesReportes(
            IEnumerable<int> seleccionadas,
            IEnumerable<FuncionalidadPerfilModelo> disponibles,
            bool perfilGlobal)
        {
            HashSet<int> idsSeleccionados =
                seleccionadas.ToHashSet();

            List<string> alcances =
                disponibles
                    .Where(
                        funcionalidad =>
                            idsSeleccionados.Contains(
                                funcionalidad.IdFuncionalidad
                            )
                            && funcionalidad.Codigo.StartsWith(
                                "REPORTES_ALCANCE_",
                                StringComparison.OrdinalIgnoreCase
                            )
                    )
                    .Select(
                        funcionalidad =>
                            funcionalidad.Codigo
                    )
                    .ToList();

            if (alcances.Count > 1)
            {
                return "Solo puede asignarse un alcance de Reportes por perfil.";
            }

            if (
                alcances.Contains(
                    "REPORTES_ALCANCE_GLOBAL"
                )
                && !perfilGlobal)
            {
                return "El alcance global de Reportes requiere un perfil global.";
            }

            return null;
        }


        // Reduce espacios consecutivos del nombre para mantener la comparación y el guardado consistentes.
        private static string NormalizarNombre(string? valor)
        {
            return string.Join(
                ' ',
                (valor ?? string.Empty)
                    .Trim()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            );
        }


        // Conserva el texto opcional de la descripción, eliminando solo espacios externos.
        private static string NormalizarDescripcion(string? valor)
        {
            return (valor ?? string.Empty).Trim();
        }


        private static ResultadoPerfil ResultadoInvalido(
            string mensaje)
        {
            return new ResultadoPerfil
            {
                Codigo = 3,
                Mensaje =
                    mensaje
            };
        }


        private static ResultadoPerfil ResultadoNoPermitido(
            string mensaje)
        {
            return new ResultadoPerfil
            {
                Codigo = 5,
                Mensaje =
                    mensaje
            };
        }
    }
}
