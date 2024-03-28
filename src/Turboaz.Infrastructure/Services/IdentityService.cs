using Microsoft.AspNetCore.Identity;
using Turboaz.Core.Dtos;
using Turboaz.Core.Models;
using Turboaz.Core.Services;

namespace Turboaz.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public IdentityService(UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;

        this.roleManager = roleManager;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
    {
        var newUser = new User
        {
            Email = registerDto.Email,
            UserName = registerDto.Login,
            PhoneNumber = registerDto.PhoneNumber,
            IsBanned = false,
            Surname = registerDto.Surname,
            ProfilePhotoUrl = registerDto.ProfilePhotoUrl
        };

        var result = await this.userManager.CreateAsync(newUser, registerDto.Password!);

        return result;
    }

    public async Task<User?> FindByUserNameAsync(string userName)
    {
        var user = await this.userManager.FindByNameAsync(userName);

        return user;
    }

    public async Task AddRoleToUserAsync(User user, string roleType)
    {
        var role = new IdentityRole { Name = roleType };

        await roleManager.CreateAsync(role);

        await userManager.AddToRoleAsync(user, role.Name);
    }

    public async Task ChangePassword(User user, string oldPassword, string newPassword)
    {
        if (user is null)
        {
            throw new ArgumentNullException("Username is null!");
        }

        var succeed = await userManager.CheckPasswordAsync(user, oldPassword);

        if (!succeed)
        {
            throw new ArgumentException("Password is invalid!");
        }

        await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
    }
}