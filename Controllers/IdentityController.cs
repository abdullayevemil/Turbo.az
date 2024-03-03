using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turbo.az.Dtos;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Controllers;

[AllowAnonymous]
public class IdentityController : Controller
{
    private readonly IUserRepository userRepository;
    public IdentityController(IUserRepository userRepository) => this.userRepository = userRepository;

    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        base.ViewData["returnUrl"] = returnUrl;

        return base.View();
    }

    [HttpGet]
    public IActionResult Register() => base.View();

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
    {
        var foundUser = await userRepository.GetUserByLoginAndPassword(loginDto);

        if (foundUser is not null)
        {
            var claims = new Claim[]
            {
                new(ClaimTypes.Email, foundUser.Email!),
                new(ClaimTypes.Name, foundUser.Login!),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await base.HttpContext.SignInAsync(
                scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                principal: new ClaimsPrincipal(claimsIdentity)
            );

            if (string.IsNullOrWhiteSpace(loginDto.ReturnUrl))
            {
                return base.RedirectToAction(controllerName: "Home", actionName: "Index");
            }

            return base.RedirectPermanent(loginDto.ReturnUrl);
        }
        else
        {
            return base.BadRequest("Incorrect login or password!");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
    {
        await userRepository.InsertUserAsync(new Models.User
        {
            Email = registerDto.Email,
            Login = registerDto.Login,
            Password = registerDto.Password
        });

        return base.RedirectToAction("Login");
    }

    [HttpDelete]
    [Authorize]
    public async Task LogOut() => await base.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
}
