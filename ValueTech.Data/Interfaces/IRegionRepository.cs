using ValueTech.Data.Models;

namespace ValueTech.Data.Interfaces
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(int idRegion);
    }
}
