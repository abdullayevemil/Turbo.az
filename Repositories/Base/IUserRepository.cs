using Turbo.az.Models;

namespace Turbo.az.Repositories.Base;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    Task BanUserAsync(string id);
    Task DeleteUserAsync(string id);
}