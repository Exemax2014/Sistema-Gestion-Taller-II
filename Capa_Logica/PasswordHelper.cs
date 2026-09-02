using System.Security.Cryptography;

namespace Capa_Logica
{
    // ============================================================
    // Clase: PasswordHelper
    //
    // Responsabilidad:
    // Generar y verificar hashes seguros de contraseñas.
    //
    // La contraseña original nunca se guarda directamente
    // en la base de datos. Solo se almacena el hash generado.
    //
    // Algoritmo utilizado:
    // PBKDF2 con SHA-256 y un salt aleatorio.
    // ============================================================
    public static class PasswordHelper
    {
        // Cantidad de bytes utilizados para generar el salt.
        private const int TamanioSalt = 16;

        // Cantidad de bytes que tendrá el hash generado.
        private const int TamanioHash = 32;

        // Cantidad de iteraciones utilizadas por PBKDF2.
        private const int Iteraciones = 100000;


        // ========================================================
        // Método: GenerarHash
        //
        // Recibe una contraseña en texto normal y devuelve
        // un valor preparado para almacenarse en la base de datos.
        //
        // Formato almacenado:
        // iteraciones.salt.hash
        // ========================================================
        public static string GenerarHash(string contrasena)
        {
            // Crear un salt aleatorio para que dos usuarios con
            // la misma contraseña no tengan necesariamente
            // el mismo hash.
            byte[] salt = RandomNumberGenerator.GetBytes(TamanioSalt);

            // Generar el hash utilizando PBKDF2 + SHA-256.
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                contrasena,
                salt,
                Iteraciones,
                HashAlgorithmName.SHA256,
                TamanioHash
            );

            // Convertir salt y hash a Base64 para poder
            // almacenarlos como texto en SQL Server.
            string saltBase64 = Convert.ToBase64String(salt);
            string hashBase64 = Convert.ToBase64String(hash);

            return $"{Iteraciones}.{saltBase64}.{hashBase64}";
        }


        // ========================================================
        // Método: Verificar
        //
        // Compara una contraseña ingresada por el usuario con
        // el hash previamente almacenado en la base de datos.
        //
        // Devuelve true si coinciden o false si son diferentes.
        // ========================================================
        public static bool Verificar(
            string contrasena,
            string hashAlmacenado
        )
        {
            // Separar las tres partes almacenadas:
            // iteraciones, salt y hash.
            string[] partes = hashAlmacenado.Split('.');

            // Si el formato no es el esperado, no se puede validar.
            if (partes.Length != 3)
            {
                return false;
            }

            // Recuperar la cantidad de iteraciones.
            if (!int.TryParse(partes[0], out int iteraciones))
            {
                return false;
            }

            try
            {
                // Recuperar el salt y el hash originales.
                byte[] salt = Convert.FromBase64String(partes[1]);
                byte[] hashEsperado = Convert.FromBase64String(partes[2]);

                // Generar nuevamente el hash utilizando
                // la contraseña que acaba de ingresar el usuario.
                byte[] hashCalculado = Rfc2898DeriveBytes.Pbkdf2(
                    contrasena,
                    salt,
                    iteraciones,
                    HashAlgorithmName.SHA256,
                    hashEsperado.Length
                );

                // Comparar ambos hashes de forma segura.
                return CryptographicOperations.FixedTimeEquals(
                    hashCalculado,
                    hashEsperado
                );
            }
            catch (FormatException)
            {
                // Si el valor almacenado tiene un formato inválido,
                // se considera que la contraseña no coincide.
                return false;
            }
        }
    }
}