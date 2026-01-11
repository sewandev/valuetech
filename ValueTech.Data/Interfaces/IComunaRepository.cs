using ValueTech.Data.Models;

namespace ValueTech.Data.Interfaces
{
    public interface IComunaRepository
    {
        Task<IEnumerable<Comuna>> GetByRegionIdAsync(int regionId);
        Task<Comuna?> GetByIdAsync(int idComuna);
        Task UpdateAsync(Comuna comuna);
        Task DeleteAsync(int idComuna);
    }
}
