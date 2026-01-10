using ValueTech.Data.Models;

namespace ValueTech.Data.Interfaces
{
    public interface IAuthRepository
    {
        Task<Usuario?> ValidateUserAsync(string username, string password);
    }
}
