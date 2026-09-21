using Microsoft.Data.SqlClient;
using System.Data;

namespace Capa_Datos
{
    // Distingue un backup confirmado cuyo asiento de auditoría no pudo persistirse.
    public sealed class BackupAuditoriaException : Exception
    {
        public BackupAuditoriaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    // Ejecuta el backup real y registra su resultado confirmado en la auditoría existente.
    public class BackupDatos
    {
        // Devuelve el catálogo inicial configurado sin exponer credenciales ni otros datos de conexión.
        public string ObtenerNombreBaseDatos() => Conexion.ObtenerNombreBaseDatos();

        // Ejecuta BACKUP con parámetros y registra el evento solo después de que SQL Server confirma el archivo.
        public void Generar(string nombreBaseDatos, string rutaCompleta, int idUsuario, string nombreArchivo)
        {
            using SqlConnection conexion = Conexion.CrearConexion();
            conexion.Open();

            using (SqlCommand comandoBackup = new(
                "BACKUP DATABASE @nombreBaseDatos TO DISK = @rutaCompleta WITH COPY_ONLY, INIT;",
                conexion))
            {
                comandoBackup.Parameters.Add("@nombreBaseDatos", SqlDbType.NVarChar, 128).Value = nombreBaseDatos;
                comandoBackup.Parameters.Add("@rutaCompleta", SqlDbType.NVarChar, 4000).Value = rutaCompleta;
                comandoBackup.CommandTimeout = 0;
                comandoBackup.ExecuteNonQuery();
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
        }
    }
}
