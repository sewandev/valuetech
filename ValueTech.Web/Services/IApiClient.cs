using ValueTech.Api.Contracts.Responses;
using ValueTech.Api.Contracts.Requests;

namespace ValueTech.Web.Services
{
    public interface IApiClient
    {
        Task<IEnumerable<RegionResponse>> GetRegionsAsync();
        Task<IEnumerable<ComunaResponse>> GetComunasByRegionAsync(int regionId);
        Task<ComunaResponse?> GetComunaByIdAsync(int regionId, int comunaId);
        Task UpdateComunaAsync(int regionId, UpdateComunaRequest request);
        Task<bool> ValidateUserAsync(string username, string password);
        Task<IEnumerable<Data.Models.Auditoria>> GetAuditLogsAsync();
    }
}
