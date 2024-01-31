using Turbo.az.Dtos;
using Turbo.az.Models;

namespace Turbo.az.Repositories.Base;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByLoginAndPassword(UserDto userDto);
    Task InsertUserAsync(User user);
}