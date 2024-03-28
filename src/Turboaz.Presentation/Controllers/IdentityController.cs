using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Turboaz.Core.CustomExceptions;
using Turboaz.Core.Dtos;
using Turboaz.Core.Models;
using Turboaz.Core.Repositories;
using Turboaz.Core.Services;

namespace Turboaz.Presentation.Controllers;

[AllowAnonymous]
public class IdentityController : Controller
{
    private readonly IIdentityService identityService;
    private readonly IUserRepository userRepository;
    private readonly SignInManager<User> signInManager;

    public IdentityController(IIdentityService identityService, IUserRepository userRepository, SignInManager<User> signInManager)
    {
        this.userRepository = userRepository;

        this.signInManager = signInManager;

        this.identityService = identityService;
    }

    [HttpGet]
    public IActionResult Login() => base.View();

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginDto dto)
    {
        try
        {
            var user = await userRepository.GetUserByUserNameAsync(dto.Login!);

            var result = await this.signInManager.PasswordSignInAsync(user!, dto.Password!, true, true);

            if (!result.Succeeded)
            {
                base.ModelState.AddModelError("Invalid argument", "Invalid login or password!");

                return base.View("Login");
            }
        }
        catch (Exception exception)
        {
            base.ModelState.AddModelError(exception.GetType().Name, exception.Message);

            return base.View("Login");
        }

        return base.RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    [HttpGet]
    public IActionResult Register() => base.View();

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterDto dto, [FromForm] IFormFile file)
    {
        await this.UploadImage(dto.Login!, file);

        dto.ProfilePhotoUrl = "Images/Profile/" + dto.Login + '_' + file.FileName;

        var result = await this.identityService.RegisterAsync(dto);

        if (!result.Succeeded)
        {
            this.AddErrorsToModelState(result.Errors);

            return base.View("Register");
        }

        return base.RedirectToAction("Login");
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await this.signInManager.SignOutAsync();

        return base.RedirectToAction("Login");
    }

    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword() => base.View();

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDto changePasswordDto)
    {
        var userName = base.HttpContext.User.Identity!.Name;

        var user = await this.userRepository.GetUserByUserNameAsync(userName!);

        await this.identityService.ChangePassword(user!, changePasswordDto.OldPassword!, changePasswordDto.NewPassword!);

        return base.RedirectToAction(actionName: "Info", controllerName: "User");
    }

    private void AddErrorsToModelState(IEnumerable<IdentityError> errors)
    {
        foreach (var error in errors)
        {
            base.ModelState.AddModelError(error.Code, error.Description);
        }
    }

    private async Task UploadImage(string username, IFormFile file)
    {
        var filename = $"{username}_{file.FileName}";

        var destinationVehicleImagePath = $"wwwroot/Images/Profile/{filename}";

        using var fileStream = System.IO.File.Create(destinationVehicleImagePath);

        await file.CopyToAsync(fileStream);
    }
}
