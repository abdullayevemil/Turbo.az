using Microsoft.AspNetCore.Identity;
using Turboaz.Core.Dtos;
using Turboaz.Core.Models;

namespace Turboaz.Core.Services;

public interface IIdentityService
{
    Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
    Task<User?> FindByUserNameAsync(string userName);
    Task ChangePassword(User user, string oldPassword, string newPassword);
}