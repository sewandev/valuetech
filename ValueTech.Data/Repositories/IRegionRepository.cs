using ValueTech.Data.Entities;

namespace ValueTech.Data.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
