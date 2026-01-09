using Microsoft.Data.SqlClient;
using System.Data;
using ValueTech.Data.Models;
using ValueTech.Data.Interfaces;

namespace ValueTech.Data.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly string _connectionString;

        public RegionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            var result = new List<Region>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_Region_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Region
                            {
                                IdRegion = reader.GetInt32(reader.GetOrdinal("IdRegion")),
                                Nombre = reader.GetString(reader.GetOrdinal("Region"))
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
