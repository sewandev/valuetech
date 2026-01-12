using ValueTech.Api.Contracts.Responses;
using ValueTech.Api.Contracts.Requests;

namespace ValueTech.Api.Services
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionResponse>> GetAllAsync();
        Task<RegionResponse?> GetByIdAsync(int id);
        Task DeleteAsync(int id, string auditUser, string auditIp);
        Task<int> CreateAsync(CreateRegionRequest request, string auditUser, string auditIp);
    }
}
