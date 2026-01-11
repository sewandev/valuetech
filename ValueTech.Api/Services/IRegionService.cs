using ValueTech.Api.Contracts.Responses;

namespace ValueTech.Api.Services
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionResponse>> GetAllAsync();
        Task<RegionResponse?> GetByIdAsync(int id);
        Task DeleteAsync(int id, string auditUser, string auditIp);
    }
}
