using ValueTech.Data.Models;

namespace ValueTech.Data.Interfaces
{
    public interface IAuditoriaRepository
    {
        Task AddAsync(Auditoria auditoria);
        Task<IEnumerable<Auditoria>> GetAllAsync();
    }
}
