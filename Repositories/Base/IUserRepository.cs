using Microsoft.AspNetCore.Identity;

namespace Turbo.az.Repositories.Base;

public interface IUserRepository
{
    IEnumerable<IdentityUser> GetAllUsers();
}