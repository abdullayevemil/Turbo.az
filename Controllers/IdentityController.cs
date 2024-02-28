using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Turbo.az.CustomExceptions;
using Turbo.az.Dtos;
using Turbo.az.Repositories.Base;
using Turbo.az.Services.Base;

namespace Turbo.az.Controllers;

[AllowAnonymous]
public class IdentityController : Controller
{
    private readonly IIdentityService identityService;
    private readonly IUserRepository userRepository;

    public IdentityController(IIdentityService identityService, IUserRepository userRepository)
    {
        this.userRepository = userRepository;

        this.identityService = identityService;
    }

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
        var result = await this.identityService.RegisterAsync(dto);

        if (!result.Succeeded)
        {
            AddErrorsToModelState(result.Errors);

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

    [HttpGet]
    public IActionResult ChangePassword() => base.View();

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDto changePasswordDto)
    {
        var userName = base.HttpContext.User.Identity.Name;

        var user = await this.userRepository.GetUserByUserNameAsync(userName!);

        await this.identityService.ChangePassword(user, changePasswordDto.OldPassword!, changePasswordDto.NewPassword!);

        return base.RedirectToAction(actionName: "Info", controllerName: "User");
    }

    private void AddErrorsToModelState(IEnumerable<IdentityError> errors)
    {
        foreach (var error in errors)
        {
            base.ModelState.AddModelError(error.Code, error.Description);
        }
    }
}
