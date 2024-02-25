using Microsoft.AspNetCore.Identity;
using Turbo.az.CustomExceptions;
using Turbo.az.Dtos;
using Turbo.az.Services.Base;

namespace Turbo.az.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public IdentityService(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager)
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

        var result = await this.signInManager.PasswordSignInAsync(user, loginDto.Password!, true, true);

        if (!result.Succeeded)
        {
            throw new ArgumentException("Login process did not succeed, something went wrong!");
        }

        if (loginDto.Login == "Admin" && loginDto.Password == "Admin123!")
        {
            await this.AddRoleToUserAsync(user, "Admin");
        }
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
    {
        var newUser = new IdentityUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Login,
        };

        var result = await this.userManager.CreateAsync(newUser, registerDto.Password!);

        return result;
    }

    public async Task<IdentityUser?> FindByUserNameAsync(string userName)
    {
        var user = await this.userManager.FindByNameAsync(userName);

        return user;
    }

    public async Task AddRoleToUserAsync(IdentityUser user, string roleType)
    {
        var role = new IdentityRole { Name =  roleType};

        await roleManager.CreateAsync(role);

        await userManager.AddToRoleAsync(user, role.Name);
    }

    public async Task LogOutAsync() => await this.signInManager.SignOutAsync();
}