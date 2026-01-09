using ValueTech.Data.Entities;

namespace ValueTech.Data.Repositories
{
    public interface IComunaRepository
    {
        Task<IEnumerable<Comuna>> GetByRegionIdAsync(int regionId);
        Task<Comuna?> GetByIdAsync(int idComuna);
        Task UpdateAsync(Comuna comuna);
    }
}
