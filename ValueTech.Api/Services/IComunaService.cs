using ValueTech.Api.Contracts.Requests;
using ValueTech.Api.Contracts.Responses;

namespace ValueTech.Api.Services
{
    public interface IComunaService
    {
        Task<IEnumerable<ComunaResponse>> GetByRegionIdAsync(int regionId);
        Task<ComunaResponse?> GetByIdAsync(int idComuna);
        Task UpdateAsync(int idComuna, UpdateComunaRequest request, string auditUser, string auditIp);
    }
}
