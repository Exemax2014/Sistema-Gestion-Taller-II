using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.IO;

namespace Capa_Datos
{
    public sealed class BackupDestinoDatos
    {
        public string NombreBaseDatos { get; init; } = string.Empty;
        public string DirectorioServidor { get; init; } = string.Empty;
    }

    public sealed class BackupResultadoDatos
    {
        public string NombreArchivo { get; init; } = string.Empty;
        public string DirectorioServidor { get; init; } = string.Empty;
    }

    // Distingue un backup confirmado cuyo asiento de auditoría no pudo persistirse.
    public sealed class BackupAuditoriaException : Exception
    {
        public BackupAuditoriaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    // Consulta la ubicación configurada en SQL Server y escribe allí las copias, nunca en una ruta elegida por el cliente.
    public class BackupDatos
    {
        private const string ConsultaDirectorioPredeterminado = "SELECT NULLIF(LTRIM(RTRIM(CONVERT(NVARCHAR(4000), SERVERPROPERTY('InstanceDefaultBackupPath')))), N'');";
        private const string ConsultaDirectorioRegistro = "SELECT TOP (1) value_data FROM sys.dm_server_registry WHERE value_name = N'BackupDirectory' AND registry_key LIKE N'%\\MSSQLServer' ORDER BY registry_key;";

        // Devuelve solo el catálogo y la ubicación de backup publicados por la instancia conectada.
        public BackupDestinoDatos ObtenerDestino()
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            conexion.Open();
            return ObtenerDestino(conexion);
        }

        // Genera el archivo con ruta y nombre derivados exclusivamente de la instancia y registra solo el nombre del archivo.
        public BackupResultadoDatos Generar(int idUsuario)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            conexion.Open();
            BackupDestinoDatos destino = ObtenerDestino(conexion);
            string nombreBaseDatos = destino.NombreBaseDatos;
            string directorio = destino.DirectorioServidor;

            string nombreArchivo = $"{SanitizarNombreArchivo(nombreBaseDatos)}_{DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture)}.bak";
            string rutaCompleta = CombinarRutaServidor(directorio, nombreArchivo);
            if (rutaCompleta.Length > 4000)
                throw new InvalidOperationException("La ubicación de Back Up configurada por SQL Server supera la longitud admitida.");

            try
            {
                using SqlCommand comandoBackup = new(
                    "BACKUP DATABASE @nombreBaseDatos TO DISK = @rutaCompleta WITH COPY_ONLY, INIT;",
                    conexion);
                comandoBackup.Parameters.Add("@nombreBaseDatos", SqlDbType.NVarChar, 128).Value = nombreBaseDatos;
                comandoBackup.Parameters.Add("@rutaCompleta", SqlDbType.NVarChar, 4000).Value = rutaCompleta;
                comandoBackup.CommandTimeout = 0;
                comandoBackup.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(
                    "No se pudo generar la copia de seguridad en la carpeta configurada por SQL Server. Verificá los permisos del servicio SQL Server.", ex);
            }

            try
            {
                using SqlCommand comandoAuditoria = new("dbo.sp_Auditoria_Registrar", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                comandoAuditoria.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;
                comandoAuditoria.Parameters.Add("@accion", SqlDbType.NVarChar, 50).Value = "BACKUP";
                comandoAuditoria.Parameters.Add("@entidad", SqlDbType.NVarChar, 50).Value = "BASE_DATOS";
                comandoAuditoria.Parameters.Add("@idEntidad", SqlDbType.Int).Value = DBNull.Value;
                comandoAuditoria.Parameters.Add("@detalle", SqlDbType.NVarChar, 300).Value = $"Copia de seguridad generada: {nombreArchivo}";
                comandoAuditoria.Parameters.Add("@idSucursal", SqlDbType.Int).Value = DBNull.Value;
                comandoAuditoria.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new BackupAuditoriaException("El backup se generó, pero falló el registro de auditoría.", ex);
            }

            return new BackupResultadoDatos
            {
                NombreArchivo = nombreArchivo,
                DirectorioServidor = directorio
            };
        }

        private static string? ConsultarDirectorio(SqlConnection conexion, string consulta)
        {
            using SqlCommand comando = new(consulta, conexion);
            return comando.ExecuteScalar() as string;
        }

        private static BackupDestinoDatos ObtenerDestino(SqlConnection conexion)
        {
            string nombreBaseDatos = new SqlConnectionStringBuilder(conexion.ConnectionString).InitialCatalog.Trim();
            if (string.IsNullOrWhiteSpace(nombreBaseDatos))
                throw new InvalidOperationException("No se pudo obtener el nombre de la base de datos configurada.");

            string? directorio = ConsultarDirectorio(conexion, ConsultaDirectorioPredeterminado);
            // SQL Server anterior a 2019 puede no exponer InstanceDefaultBackupPath; el registro sigue perteneciendo al servidor.
            if (string.IsNullOrWhiteSpace(directorio))
            {
                try
                {
                    directorio = ConsultarDirectorio(conexion, ConsultaDirectorioRegistro);
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException(
                        "SQL Server no informó su carpeta de Back Up predeterminada. Verificá que la instancia permita consultar su configuración de servidor.", ex);
                }
            }

            directorio = directorio?.Trim();
            if (string.IsNullOrWhiteSpace(directorio))
                throw new InvalidOperationException("SQL Server no informó una carpeta de Back Up predeterminada válida.");

            return new BackupDestinoDatos { NombreBaseDatos = nombreBaseDatos, DirectorioServidor = directorio };
        }

        // Une el nombre generado respetando el separador que publicó el sistema operativo del servidor.
        private static string CombinarRutaServidor(string directorio, string nombreArchivo)
        {
            char separador = directorio.Contains('\\') ? '\\' : '/';
            return $"{directorio.TrimEnd('\\', '/')}{separador}{nombreArchivo}";
        }

        // Conserva únicamente caracteres seguros para el componente de nombre generado por la aplicación.
        private static string SanitizarNombreArchivo(string nombre)
        {
            HashSet<char> invalidos = Path.GetInvalidFileNameChars().ToHashSet();
            foreach (char caracter in "<>:\"/\\|?*")
                invalidos.Add(caracter);

            string resultado = new(nombre.Select(caracter => invalidos.Contains(caracter) || char.IsControl(caracter) ? '_' : caracter).ToArray());
            resultado = resultado.Trim().TrimEnd('.');
            return string.IsNullOrWhiteSpace(resultado) ? "BaseDatos" : resultado;
        }
    }
}
