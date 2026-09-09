namespace Capa_Logica
{
    // ============================================================
    // Clase: SesionActual
    //
    // Responsabilidad:
    // Mantener en memoria los datos del usuario autenticado
    // durante toda la ejecución de la aplicación.
    //
    // También almacena las funcionalidades permitidas para
    // poder controlar accesos y elementos del menú.
    // ============================================================
    public static class SesionActual
    {
        // ========================================================
        // IDENTIFICADORES PRINCIPALES
        // ========================================================

        public static int IdUsuario { get; private set; }

        public static int IdPerfil { get; private set; }

        // Sucursal fija asignada al usuario.
        //
        // Administrador:
        //     NULL porque puede trabajar con varias sucursales.
        //
        // Gerente/Vendedor:
        //     ID de su sucursal asignada.
        public static int? IdSucursal { get; private set; }


        // ========================================================
        // SUCURSAL OPERATIVA
        //
        // Representa la sucursal sobre la que actualmente
        // se encuentra trabajando el sistema.
        //
        // NULL:
        //     Todas las sucursales.
        //
        // Un ID:
        //     Una sucursal específica.
        //
        // Para Gerentes y Vendedores se inicializa
        // automáticamente con su sucursal asignada.
        //
        // Para Administradores comienza en NULL,
        // es decir, "Todas las sucursales".
        // ========================================================

        public static int? IdSucursalOperativa { get; private set; }

        public static string SucursalOperativa { get; private set; }
            = string.Empty;


        // ========================================================
        // DATOS DESCRIPTIVOS
        // ========================================================

        public static string Nombre { get; private set; }
            = string.Empty;

        public static string Apellido { get; private set; }
            = string.Empty;

        public static string NombreUsuario { get; private set; }
            = string.Empty;

        public static string Perfil { get; private set; }
            = string.Empty;

        public static string Sucursal { get; private set; }
            = string.Empty;


        // Indica si existe actualmente una sesión iniciada.
        public static bool SesionIniciada { get; private set; }


        // ========================================================
        // FUNCIONALIDADES / PERMISOS
        //
        // HashSet permite consultar rápidamente si un código
        // determinado pertenece al usuario.
        //
        // OrdinalIgnoreCase evita problemas por diferencias
        // entre mayúsculas y minúsculas.
        // ========================================================

        private static HashSet<string> funcionalidades =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase
            );


        // ========================================================
        // Método: Iniciar
        //
        // Guarda los datos del usuario autenticado y las
        // funcionalidades correspondientes a su perfil.
        // ========================================================

        public static void Iniciar(
            int idUsuario,
            int idPerfil,
            int? idSucursal,
            string nombre,
            string apellido,
            string nombreUsuario,
            string perfil,
            string sucursal,
            IEnumerable<string>? funcionalidadesPermitidas = null)
        {
            IdUsuario = idUsuario;
            IdPerfil = idPerfil;
            IdSucursal = idSucursal;

            Nombre = nombre;
            Apellido = apellido;
            NombreUsuario = nombreUsuario;

            Perfil = perfil;
            Sucursal = sucursal;


            // ====================================================
            // INICIALIZAR SUCURSAL OPERATIVA
            //
            // Gerente/Vendedor:
            //     Trabajan automáticamente sobre su sucursal.
            //
            // Administrador:
            //     Como no tiene una sucursal fija, comienza
            //     consultando "Todas las sucursales".
            // ====================================================

            IdSucursalOperativa = idSucursal;

            SucursalOperativa = idSucursal.HasValue
                ? sucursal
                : "Todas las sucursales";


            // Crear una nueva colección de permisos
            // para la sesión actual.
            funcionalidades = funcionalidadesPermitidas != null
                ? new HashSet<string>(
                    funcionalidadesPermitidas,
                    StringComparer.OrdinalIgnoreCase
                )
                : new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase
                );


            SesionIniciada = true;
        }


        // ========================================================
        // Método: CambiarSucursalOperativa
        //
        // Permite modificar la sucursal sobre la que se encuentra
        // trabajando actualmente el sistema.
        //
        // Reglas:
        //
        // Administrador:
        //     Puede seleccionar cualquier sucursal.
        //     También puede seleccionar NULL para consultar todas.
        //
        // Gerente/Vendedor:
        //     Solo pueden trabajar sobre su sucursal asignada.
        //
        // Devuelve:
        // true  -> cambio permitido.
        // false -> cambio no permitido.
        // ========================================================

        public static bool CambiarSucursalOperativa(
            int? idSucursal,
            string nombreSucursal)
        {
            // ----------------------------------------------------
            // Usuario con sucursal fija.
            // Gerente o Vendedor.
            // ----------------------------------------------------

            if (IdSucursal.HasValue)
            {
                // No puede seleccionar "Todas las sucursales".
                if (!idSucursal.HasValue)
                {
                    return false;
                }

                // No puede seleccionar una sucursal diferente
                // de la que tiene asignada.
                if (idSucursal.Value != IdSucursal.Value)
                {
                    return false;
                }
            }


            // ----------------------------------------------------
            // Administrador:
            //
            // IdSucursal = NULL.
            //
            // Puede utilizar:
            // NULL -> Todas las sucursales.
            // ID   -> Una sucursal específica.
            // ----------------------------------------------------

            IdSucursalOperativa = idSucursal;

            SucursalOperativa = idSucursal.HasValue
                ? nombreSucursal
                : "Todas las sucursales";


            return true;
        }


        // ========================================================
        // Método: EstaConsultandoTodasLasSucursales
        //
        // Devuelve true cuando el contexto operativo actual
        // representa todas las sucursales.
        // ========================================================

        public static bool EstaConsultandoTodasLasSucursales()
        {
            return !IdSucursalOperativa.HasValue;
        }


        // ========================================================
        // Método: ObtenerIdSucursalOperativa
        //
        // Devuelve el ID de la sucursal operativa actual.
        //
        // Este método debe utilizarse en operaciones que
        // obligatoriamente necesitan una sucursal específica,
        // por ejemplo:
        //
        // - realizar una venta;
        // - descontar stock;
        // - registrar movimientos de inventario.
        //
        // Si está seleccionada la opción "Todas las sucursales",
        // se genera una excepción controlada.
        // ========================================================

        public static int ObtenerIdSucursalOperativa()
        {
            if (!IdSucursalOperativa.HasValue)
            {
                throw new InvalidOperationException(
                    "Debe seleccionar una sucursal específica para realizar esta operación."
                );
            }

            return IdSucursalOperativa.Value;
        }


        // ========================================================
        // Método: TienePermiso
        //
        // Permite consultar desde Capa_Logica o Capa_Vistas si
        // el usuario autenticado posee una funcionalidad concreta.
        //
        // Ejemplo:
        //
        // SesionActual.TienePermiso("USUARIOS_VER")
        // ========================================================

        public static bool TienePermiso(
            string codigoFuncionalidad)
        {
            if (string.IsNullOrWhiteSpace(
                codigoFuncionalidad))
            {
                return false;
            }

            return funcionalidades.Contains(
                codigoFuncionalidad
            );
        }


        // ========================================================
        // Método: ObtenerFuncionalidades
        //
        // Devuelve una copia de los códigos de funcionalidades
        // cargados en la sesión.
        //
        // Se evita entregar directamente la colección interna
        // para que no pueda modificarse desde otra clase.
        // ========================================================

        public static List<string> ObtenerFuncionalidades()
        {
            return funcionalidades.ToList();
        }


        // ========================================================
        // Método: Cerrar
        //
        // Limpia todos los datos almacenados cuando se
        // finaliza la sesión.
        // ========================================================

        public static void Cerrar()
        {
            IdUsuario = 0;
            IdPerfil = 0;

            IdSucursal = null;
            IdSucursalOperativa = null;

            Nombre = string.Empty;
            Apellido = string.Empty;
            NombreUsuario = string.Empty;

            Perfil = string.Empty;
            Sucursal = string.Empty;

            SucursalOperativa = string.Empty;

            funcionalidades.Clear();

            SesionIniciada = false;
        }
    }
}