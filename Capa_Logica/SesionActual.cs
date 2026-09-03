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
        // Identificadores principales.
        public static int IdUsuario { get; private set; }
        public static int IdPerfil { get; private set; }
        public static int IdSucursal { get; private set; }

        // Datos descriptivos.
        public static string Nombre { get; private set; } = string.Empty;
        public static string Apellido { get; private set; } = string.Empty;
        public static string NombreUsuario { get; private set; } = string.Empty;

        public static string Perfil { get; private set; } = string.Empty;
        public static string Sucursal { get; private set; } = string.Empty;

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
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);


        // ========================================================
        // Método: Iniciar
        //
        // Guarda los datos del usuario autenticado y las
        // funcionalidades correspondientes a su perfil.
        //
        // El parámetro funcionalidadesPermitidas es opcional
        // temporalmente para no romper otras llamadas existentes
        // mientras terminamos de integrar los permisos.
        // ========================================================
        public static void Iniciar(
            int idUsuario,
            int idPerfil,
            int idSucursal,
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

            // Crear una nueva colección de permisos para la sesión.
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
        // Método: TienePermiso
        //
        // Permite consultar desde Capa_Logica o Capa_Vistas si
        // el usuario autenticado posee una funcionalidad concreta.
        //
        // Ejemplo:
        // SesionActual.TienePermiso("USUARIOS_VER")
        // ========================================================
        public static bool TienePermiso(string codigoFuncionalidad)
        {
            if (string.IsNullOrWhiteSpace(codigoFuncionalidad))
            {
                return false;
            }

            return funcionalidades.Contains(codigoFuncionalidad);
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
        // Limpia los datos personales y permisos almacenados
        // cuando se finaliza la sesión.
        // ========================================================
        public static void Cerrar()
        {
            IdUsuario = 0;
            IdPerfil = 0;
            IdSucursal = 0;

            Nombre = string.Empty;
            Apellido = string.Empty;
            NombreUsuario = string.Empty;

            Perfil = string.Empty;
            Sucursal = string.Empty;

            funcionalidades.Clear();

            SesionIniciada = false;
        }
    }
}