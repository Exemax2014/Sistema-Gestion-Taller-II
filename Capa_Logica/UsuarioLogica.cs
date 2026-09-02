using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // Clase: UsuarioLogica
    //
    // Responsabilidad:
    // Contener la lógica relacionada con la autenticación
    // de usuarios.
    //
    // Coordina:
    // - UsuarioDatos: obtiene el usuario desde SQL Server.
    // - PasswordHelper: verifica la contraseña.
    // - SesionActual: guarda al usuario autenticado.
    //
    // Esta clase no muestra MessageBox ni ejecuta SQL.
    // ============================================================
    public class UsuarioLogica
    {
        private readonly UsuarioDatos usuarioDatos;

        // ========================================================
        // Constructor
        //
        // Crea el objeto encargado del acceso a datos de usuarios.
        // ========================================================
        public UsuarioLogica()
        {
            usuarioDatos = new UsuarioDatos();
        }


        // ========================================================
        // Método: IniciarSesion
        //
        // Valida las credenciales ingresadas por el usuario.
        //
        // Devuelve:
        // true  -> login correcto.
        // false -> datos incompletos, usuario inexistente
        //          o contraseña incorrecta.
        //
        // El parámetro "mensaje" devuelve una explicación para
        // que Capa_Vistas decida cómo mostrarla.
        // ========================================================
        public bool IniciarSesion(
            string nombreUsuario,
            string contrasena,
            out string mensaje)
        {
            // Validar que el usuario haya ingresado ambos datos.
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                mensaje = "Debe ingresar el nombre de usuario.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(contrasena))
            {
                mensaje = "Debe ingresar la contraseña.";
                return false;
            }

            // Buscar al usuario mediante Capa_Datos.
            UsuarioLoginDatos? usuario =
                usuarioDatos.BuscarPorNombreUsuario(nombreUsuario);

            // Si no se encontró, no se puede iniciar sesión.
            if (usuario == null)
            {
                mensaje = "Usuario o contraseña incorrectos.";
                return false;
            }

            // Comparar la contraseña ingresada con el hash
            // almacenado en la base de datos.
            bool contrasenaCorrecta = PasswordHelper.Verificar(
                contrasena,
                usuario.ContrasenaHash
            );

            if (!contrasenaCorrecta)
            {
                mensaje = "Usuario o contraseña incorrectos.";
                return false;
            }

            // Si las credenciales son válidas, almacenar
            // los datos del usuario en la sesión actual.
            SesionActual.Iniciar(
                usuario.IdUsuario,
                usuario.IdPerfil,
                usuario.IdSucursal,
                usuario.Nombre,
                usuario.Apellido,
                usuario.NombreUsuario,
                usuario.Perfil,
                usuario.Sucursal
            );

            mensaje = "Inicio de sesión correcto.";
            return true;
        }
    }
}