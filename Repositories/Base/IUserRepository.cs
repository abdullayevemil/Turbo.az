using Turbo.az.Models;

namespace Turbo.az.Repositories.Base;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
}