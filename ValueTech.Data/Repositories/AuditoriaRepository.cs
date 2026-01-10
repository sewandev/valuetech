using Microsoft.Data.SqlClient;
using ValueTech.Data.Interfaces;
using ValueTech.Data.Models;

namespace ValueTech.Data.Repositories
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly string _connectionString;

        public AuditoriaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddAsync(Auditoria auditoria)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(
                "INSERT INTO Auditoria (Usuario, IpAddress, Accion, Detalle) VALUES (@Usuario, @IpAddress, @Accion, @Detalle)", 
                connection);
            
            command.Parameters.AddWithValue("@Usuario", auditoria.Usuario);
            command.Parameters.AddWithValue("@IpAddress", auditoria.IpAddress);
            command.Parameters.AddWithValue("@Accion", auditoria.Accion);
            command.Parameters.AddWithValue("@Detalle", auditoria.Detalle ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Auditoria>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var logs = new List<Auditoria>();
            using var command = new SqlCommand("SELECT IdAuditoria, Fecha, Usuario, IpAddress, Accion, Detalle FROM Auditoria ORDER BY Fecha DESC", connection);
            
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                logs.Add(new Auditoria
                {
                    IdAuditoria = reader.GetInt32(0),
                    Fecha = reader.GetDateTime(1),
                    Usuario = reader.GetString(2),
                    IpAddress = reader.GetString(3),
                    Accion = reader.GetString(4),
                    Detalle = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                });
            }

            return logs;
        }
    }
}
