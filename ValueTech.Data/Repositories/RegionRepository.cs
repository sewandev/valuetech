using Microsoft.Data.SqlClient;
using System.Data;
using ValueTech.Data.Models;
using ValueTech.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace ValueTech.Data.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly string _connectionString;
        private readonly Microsoft.Extensions.Logging.ILogger<RegionRepository> _logger;

        public RegionRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<RegionRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
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

                    try
                    {
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
                    catch (SqlException ex)
                    {
                        _logger.LogError(ex, "Error obteniendo todas las regiones.");
                        throw;
                    }
                }
            }

            return result;
        }

        public async Task<Region?> GetByIdAsync(int idRegion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_Region_GetById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdRegion", idRegion);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Region
                            {
                                IdRegion = reader.GetInt32(reader.GetOrdinal("IdRegion")),
                                Nombre = reader.GetString(reader.GetOrdinal("Region"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        public async Task DeleteAsync(int idRegion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_Region_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdRegion", idRegion);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError(ex, "Error eliminando región {Id}.", idRegion);
                        throw;
                    }
                }
            }
        }
    }
}
