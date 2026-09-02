namespace Capa_Logica
{
    // ============================================================
    // Clase: SesionActual
    //
    // Responsabilidad:
    // Mantener en memoria los datos del usuario que inició sesión.
    //
    // Estos datos podrán ser utilizados por los formularios y
    // por la lógica del sistema mientras la aplicación esté abierta.
    //
    // No realiza consultas SQL ni contiene lógica de acceso a datos.
    // ============================================================
    public static class SesionActual
    {
        // Identificadores principales del usuario autenticado.
        public static int IdUsuario { get; private set; }
        public static int IdPerfil { get; private set; }
        public static int IdSucursal { get; private set; }

        // Datos descriptivos del usuario.
        public static string Nombre { get; private set; } = string.Empty;
        public static string Apellido { get; private set; } = string.Empty;
        public static string NombreUsuario { get; private set; } = string.Empty;

        // Datos del perfil y sucursal asociados.
        public static string Perfil { get; private set; } = string.Empty;
        public static string Sucursal { get; private set; } = string.Empty;

        // Indica si actualmente existe un usuario autenticado.
        public static bool SesionIniciada { get; private set; }


        // ========================================================
        // Método: Iniciar
        //
        // Guarda en memoria los datos del usuario que fue
        // autenticado correctamente por UsuarioLogica.
        // ========================================================
        public static void Iniciar(
            int idUsuario,
            int idPerfil,
            int idSucursal,
            string nombre,
            string apellido,
            string nombreUsuario,
            string perfil,
            string sucursal)
        {
            IdUsuario = idUsuario;
            IdPerfil = idPerfil;
            IdSucursal = idSucursal;

            Nombre = nombre;
            Apellido = apellido;
            NombreUsuario = nombreUsuario;

            Perfil = perfil;
            Sucursal = sucursal;

            SesionIniciada = true;
        }


        // ========================================================
        // Método: Cerrar
        //
        // Limpia todos los datos almacenados cuando el usuario
        // cierra sesión o finaliza su acceso al sistema.
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

            SesionIniciada = false;
        }
    }
}