using Microsoft.AspNetCore.Identity;
using Turbo.az.Dtos;
using Turbo.az.Models;

namespace Turbo.az.Services.Base;

public interface IIdentityService
{
    Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
    Task LoginAsync(LoginDto loginDto);
    Task<User?> FindByUserNameAsync(string userName);
    Task LogOutAsync();
}