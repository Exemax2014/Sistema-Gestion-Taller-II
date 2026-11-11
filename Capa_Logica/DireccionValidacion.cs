using System.Text.RegularExpressions;

namespace Capa_Logica
{
    // Centraliza las reglas autoritativas para los componentes reutilizables de una dirección.
    public static class DireccionValidacion
    {
        private static readonly Regex CallePermitida = new(@"^[\p{L}\p{M}\p{N}\s.,'’/#º°-]+$", RegexOptions.Compiled);
        private static readonly Regex ContieneLetra = new(@"\p{L}", RegexOptions.Compiled);
        private static readonly Regex SeparadoresDeTermino = new(@"[\s.,'’/#º°-]+", RegexOptions.Compiled);
        private static readonly Regex ContieneNumero = new(@"\p{N}", RegexOptions.Compiled);

        public static string NormalizarCalle(string? valor) => NormalizarEspacios(valor);

        public static string NormalizarEspacios(string? valor)
        {
            return string.Join(' ', (valor ?? string.Empty)
                .Trim()
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string? ValidarCalle(string? calle)
        {
            string normalizada = NormalizarCalle(calle);
            if (string.IsNullOrWhiteSpace(normalizada) || normalizada.Length > 150 || !CallePermitida.IsMatch(normalizada))
                return "Ingresá una calle válida.";

            if (!ContieneLetra.IsMatch(normalizada))
                return "La calle debe contener al menos una letra.";

            bool mezclaLetrasYNumeros = SeparadoresDeTermino
                .Split(normalizada)
                .Any(termino => ContieneLetra.IsMatch(termino) && ContieneNumero.IsMatch(termino));

            return mezclaLetrasYNumeros
                ? "La calle no puede mezclar letras y números dentro del mismo término."
                : null;
        }

        public static string? ValidarAltura(string? altura)
        {
            return string.IsNullOrWhiteSpace(altura) || altura.Length > 20 ||
                   !altura.All(char.IsAsciiDigit) || !int.TryParse(altura, out int numero) || numero <= 0
                ? "La altura debe ser un número entero mayor que 0."
                : null;
        }

        public static string? ValidarPiso(string? piso)
        {
            return string.IsNullOrWhiteSpace(piso)
                ? null
                : piso.Length > 2 || !piso.All(char.IsAsciiDigit) ||
                  !int.TryParse(piso, out int numero) || numero > 99
                    ? "El piso debe estar entre 0 y 99."
                    : null;
        }
    }
}
