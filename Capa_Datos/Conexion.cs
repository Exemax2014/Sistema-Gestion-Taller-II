using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    internal class ConexionConfig
    {
        public string? Servidor { get; set; }
        public string? BaseDatos { get; set; }
        public bool? AutenticacionWindows { get; set; }
        public string? Usuario { get; set; }
        public string? Contrasena { get; set; }
        public bool? TrustServerCertificate { get; set; }
    }

    public static class Conexion
    {
        private static ConexionConfig CargarConfiguracion()
        {
            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Configuracion",
                "configuracion.json"
            );

            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException(
                    $"No se encontró el archivo de configuración: '{ruta}'."
                );
            }

            string json;

            try
            {
                json = File.ReadAllText(ruta);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"No se pudo leer el archivo de configuración '{ruta}'.",
                    ex
                );
            }

            ConexionConfig? configuracion;

            try
            {
                configuracion = JsonSerializer.Deserialize<ConexionConfig>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"El archivo de configuración '{ruta}' no contiene un JSON válido.",
                    ex
                );
            }

            if (configuracion == null)
            {
                throw new InvalidOperationException(
                    $"El archivo de configuración '{ruta}' está vacío."
                );
            }

            if (string.IsNullOrWhiteSpace(configuracion.Servidor))
            {
                throw new InvalidOperationException(
                    "Debe especificarse el servidor de SQL Server."
                );
            }

            if (string.IsNullOrWhiteSpace(configuracion.BaseDatos))
            {
                throw new InvalidOperationException(
                    "Debe especificarse el nombre de la base de datos."
                );
            }

            if (configuracion.AutenticacionWindows == null)
            {
                throw new InvalidOperationException(
                    "Debe especificarse el tipo de autenticación."
                );
            }

            if (configuracion.TrustServerCertificate == null)
            {
                throw new InvalidOperationException(
                    "Debe especificarse TrustServerCertificate."
                );
            }

            if (!configuracion.AutenticacionWindows.Value)
            {
                if (string.IsNullOrWhiteSpace(configuracion.Usuario))
                {
                    throw new InvalidOperationException(
                        "Debe especificarse el usuario de SQL Server."
                    );
                }

                if (string.IsNullOrWhiteSpace(configuracion.Contrasena))
                {
                    throw new InvalidOperationException(
                        "Debe especificarse la contraseña de SQL Server."
                    );
                }
            }

            return configuracion;
        }

        public static SqlConnection CrearConexion()
        {
            ConexionConfig configuracion = CargarConfiguracion();

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = configuracion.Servidor,
                InitialCatalog = configuracion.BaseDatos,
                TrustServerCertificate =
                    configuracion.TrustServerCertificate!.Value
            };

            if (configuracion.AutenticacionWindows!.Value)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = configuracion.Usuario;
                builder.Password = configuracion.Contrasena;
            }

            return new SqlConnection(builder.ConnectionString);
        }
    }
}