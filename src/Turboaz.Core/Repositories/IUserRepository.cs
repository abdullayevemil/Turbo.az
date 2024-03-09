using Turboaz.Core.Models;

namespace Turboaz.Core.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    Task BanUserAsync(string id);
    Task DeleteUserAsync(string id);
    Task<User?> GetUserByUserNameAsync(string userName);
}