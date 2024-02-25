using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turbo.az.CustomExceptions;
using Turbo.az.Dtos;
using Turbo.az.Services.Base;

namespace Turbo.az.Controllers;

[AllowAnonymous]
public class IdentityController : Controller
{
    private readonly IIdentityService identityService;

    public IdentityController(IIdentityService identityService) => this.identityService = identityService;

    [HttpGet]
    public IActionResult Login() => base.View();

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginDto dto)
    {
        try
        {
            await this.identityService.LoginAsync(dto);
        }
        catch (Exception exception) when (exception is NotFoundException || exception is ArgumentException)
        {
            return base.BadRequest(exception.Message);
        }

        return base.RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    [HttpGet]
    public IActionResult Register() => base.View();

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterDto dto)
    {
        var result = await this.identityService.RegisterAsync(dto, "admin", "Admin");

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                base.ModelState.AddModelError(error.Code, error.Description);
            }

            return base.View("Register");
        }

        return base.RedirectToAction("Login");
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await this.identityService.LogOutAsync();

        return base.Ok();
    }
}
