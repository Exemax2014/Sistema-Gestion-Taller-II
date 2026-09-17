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


            nombre =
                (nombre ?? string.Empty)
                .Trim();

            descripcion =
                (descripcion ?? string.Empty)
                .Trim();


            if (string.IsNullOrWhiteSpace(
                nombre))
            {
                return ResultadoInvalido(
                    "El nombre del tipo de usuario es obligatorio."
                );
            }


            if (nombre.Length > 50)
            {
                return ResultadoInvalido(
                    "El nombre del tipo de usuario no puede superar los 50 caracteres."
                );
            }


            if (descripcion.Length > 200)
            {
                return ResultadoInvalido(
                    "La descripción no puede superar los 200 caracteres."
                );
            }


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


            nombre =
                (nombre ?? string.Empty)
                .Trim();

            descripcion =
                (descripcion ?? string.Empty)
                .Trim();


            if (perfilActual.AlcanceGlobal)
            {
                return ResultadoNoPermitido(
                    "El perfil global del sistema no puede modificarse."
                );
            }


            if (string.IsNullOrWhiteSpace(
                nombre))
            {
                return ResultadoInvalido(
                    "El nombre del tipo de usuario es obligatorio."
                );
            }


            if (nombre.Length > 50)
            {
                return ResultadoInvalido(
                    "El nombre del tipo de usuario no puede superar los 50 caracteres."
                );
            }


            if (descripcion.Length > 200)
            {
                return ResultadoInvalido(
                    "La descripción no puede superar los 200 caracteres."
                );
            }


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
