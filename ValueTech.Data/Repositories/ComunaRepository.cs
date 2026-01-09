using Microsoft.Data.SqlClient;
using System.Data;
using ValueTech.Data.Entities;

namespace ValueTech.Data.Repositories
{
    public class ComunaRepository : IComunaRepository
    {
        private readonly string _connectionString;

        public ComunaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Comuna>> GetByRegionIdAsync(int regionId)
        {
            var result = new List<Comuna>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_Comuna_GetByRegion", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdRegion", regionId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(MapToComuna(reader));
                        }
                    }
                }
            }

            return result;
        }

        public async Task<Comuna?> GetByIdAsync(int idComuna)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_Comuna_GetById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdComuna", idComuna);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapToComuna(reader);
                        }
                    }
                }
            }

            return null;
        }

        public async Task UpdateAsync(Comuna comuna)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_Comuna_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdComuna", comuna.IdComuna);
                    command.Parameters.AddWithValue("@IdRegion", comuna.IdRegion);
                    command.Parameters.AddWithValue("@Comuna", comuna.Nombre);

                    var xmlParam = new SqlParameter("@InformacionAdicional", SqlDbType.Xml)
                    {
                        Value = (object?)comuna.InformacionAdicional ?? DBNull.Value
                    };
                    command.Parameters.Add(xmlParam);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private Comuna MapToComuna(SqlDataReader reader)
        {
            var c = new Comuna
            {
                IdComuna = reader.GetInt32(reader.GetOrdinal("IdComuna")),
                IdRegion = reader.GetInt32(reader.GetOrdinal("IdRegion")),
                Nombre = reader.GetString(reader.GetOrdinal("Comuna"))
            };

            int ordinalInfo = reader.GetOrdinal("InformacionAdicional");
            if (!reader.IsDBNull(ordinalInfo))
            {
                c.InformacionAdicional = reader.GetString(ordinalInfo);
            }

            return c;
        }
    }
}
