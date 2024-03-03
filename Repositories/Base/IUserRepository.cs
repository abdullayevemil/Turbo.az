using Turbo.az.Dtos;
using Turbo.az.Models;

namespace Turbo.az.Repositories.Base;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByLoginAndPassword(LoginDto loginDto);
    Task InsertUserAsync(User user);
}