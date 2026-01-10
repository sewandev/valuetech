using Microsoft.Data.SqlClient;
using ValueTech.Data.Interfaces;
using ValueTech.Data.Models;
using System.Data;

namespace ValueTech.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Usuario?> ValidateUserAsync(string username, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT IdUsuario, Username FROM Usuario WHERE Username = @Username AND Password = @Password", connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password); // En prod: Hash verification

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    IdUsuario = reader.GetInt32(0),
                    Username = reader.GetString(1)
                };
            }

            return null;
        }
    }
}
