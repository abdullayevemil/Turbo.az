using Microsoft.AspNetCore.Identity;
using Turbo.az.CustomExceptions;
using Turbo.az.Dtos;
using Turbo.az.Models;
using Turbo.az.Services.Base;

namespace Turbo.az.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly SignInManager<User> signInManager;

    public IdentityService(UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<User> signInManager)
    {
        this.userManager = userManager;

        this.roleManager = roleManager;

        this.signInManager = signInManager;
    }

    public async Task LoginAsync(LoginDto loginDto)
    {
        var user = await this.FindByUserNameAsync(loginDto.Login!);

        if (user is null)
        {
            throw new NotFoundException($"{loginDto.Login} was not found in the database!");
        }

        if (user.IsBanned)
        {
            throw new ArgumentException($"{loginDto.Login} was banned!");
        }

        var result = await this.signInManager.PasswordSignInAsync(user, loginDto.Password!, true, true);

        if (!result.Succeeded)
        {
            throw new ArgumentException("Login process did not succeed, something went wrong!");
        }

        if (loginDto.Login!.ToLower() == "admin")
        {
            await this.AddRoleToUserAsync(user, "Admin");
        }
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
    {
        var newUser = new User
        {
            Email = registerDto.Email,
            UserName = registerDto.Login,
            IsBanned = false
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

    public async Task LogOutAsync() => await this.signInManager.SignOutAsync();
}