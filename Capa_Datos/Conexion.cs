using System;
using System.IO;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    internal class ConexionConfig
    {
        public string Servidor { get; set; } = "localhost";
        public string BaseDatos { get; set; } = "SistemaGestion";
        public bool AutenticacionWindows { get; set; } = true;
        public string Usuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public bool TrustServerCertificate { get; set; } = true;
    }

    public static class Conexion
    {
        private static ConexionConfig CargarConfiguracion()
        {
            // Ruta determinista en tiempo de ejecución: <AppContext.BaseDirectory>/Configuracion/configuracion.json
            var ruta = Path.Combine(AppContext.BaseDirectory, "Configuracion", "configuracion.json");

            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException($"Archivo de configuración no encontrado: '{ruta}'. Copie 'configuracion.example.json' a esta ubicación y renómbreelo a 'configuracion.json'.");
            }

            string json;
            try
            {
                json = File.ReadAllText(ruta);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"No se pudo leer el archivo de configuración '{ruta}': {ex.Message}", ex);
            }

            ConexionConfig? cfg;
            try
            {
                cfg = JsonSerializer.Deserialize<ConexionConfig>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"El archivo de configuración '{ruta}' no es un JSON válido: {ex.Message}", ex);
            }

            if (cfg == null)
            {
                throw new InvalidOperationException($"El archivo de configuración '{ruta}' está vacío o no contiene los valores esperados.");
            }

            // Validar valores esenciales para evitar usar defaults silenciosos
            if (string.IsNullOrWhiteSpace(cfg.Servidor))
                throw new InvalidOperationException($"El campo 'Servidor' no puede estar vacío en '{ruta}'.");
            if (string.IsNullOrWhiteSpace(cfg.BaseDatos))
                throw new InvalidOperationException($"El campo 'BaseDatos' no puede estar vacío en '{ruta}'.");
            if (!cfg.AutenticacionWindows && string.IsNullOrWhiteSpace(cfg.Usuario))
                throw new InvalidOperationException($"AutenticacionWindows está deshabilitada pero 'Usuario' no está definido en '{ruta}'.");

            return cfg;
        }

        public static SqlConnection CrearConexion()
        {
            var cfg = CargarConfiguracion();

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = cfg.Servidor,
                InitialCatalog = cfg.BaseDatos,
                TrustServerCertificate = cfg.TrustServerCertificate
            };

            if (cfg.AutenticacionWindows)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = cfg.Usuario ?? string.Empty;
                builder.Password = cfg.Contrasena ?? string.Empty;
            }

            return new SqlConnection(builder.ConnectionString);
        }
    }
}
