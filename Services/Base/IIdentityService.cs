using Microsoft.AspNetCore.Identity;
using Turbo.az.Dtos;

namespace Turbo.az.Services.Base;

public interface IIdentityService
{
    Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
    Task LoginAsync(LoginDto loginDto);
    Task<IdentityUser> FindByUserNameAsync(string userName);
    Task AddRoleToUserAsync(IdentityUser user, string roleType);
    Task LogOutAsync();
}