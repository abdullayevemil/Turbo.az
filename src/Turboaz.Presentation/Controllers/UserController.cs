using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Turboaz.Core.Dtos;
using Turboaz.Core.Models;
using Turboaz.Core.Repositories;

namespace Turboaz.Presentation.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IUserRepository userRepository;

    public UserController(IUserRepository userRepository) => this.userRepository = userRepository;

    [HttpGet]
    [Route("[controller]/Info")]
    public async Task<IActionResult> UserInfo()
    {
        var username = base.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)!.Value;

        var user = await this.userRepository.GetUserByUserNameAsync(username);

        return base.View(model: user);
    }

    [HttpGet]
    [ActionName("Index")]
    [Authorize(Roles = "Admin")]
    public IActionResult ShowAllUsers()
    {
        var users = this.userRepository.GetAllUsers();

        return base.View(model: users);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Ban(string id)
    {
        await this.userRepository.BanUserAsync(id);

        return base.Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        await this.userRepository.DeleteUserAsync(id);

        return base.Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(string id, [FromBody] ProfileDto profileDto)
    {
        var user = new User
        {
            Email = profileDto.Email,
            UserName = profileDto.Login,
            Surname = profileDto.Surname,
            PhoneNumber = profileDto.PhoneNumber,
        };

        await this.userRepository.UpdateProfileAsync(id, user);

        return base.RedirectToAction(actionName: "Logout", controllerName: "Identity");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ChangeProfilePhoto()
    {
        var userName = base.HttpContext.User.Identity!.Name;

        var user = await this.userRepository.GetUserByUserNameAsync(userName!);

        base.ViewData["oldProfilePhotoUrl"] = user!.ProfilePhotoUrl;

        return base.View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeProfilePhoto([FromForm] IFormFile file)
    {
        var userName = base.HttpContext.User.Identity!.Name;

        var user = await this.userRepository.GetUserByUserNameAsync(userName!);

        System.IO.File.Delete("wwwroot/" + user!.ProfilePhotoUrl!);

        await this.UploadImage(user!.UserName!, file);

        var newPhotoUrl = "Images/Profile/" + user!.UserName + '_' + file.FileName;

        await this.userRepository.ChangeProfilePhotoAsync(user.Id, newPhotoUrl);
    
        return base.RedirectToAction(actionName: "Info", controllerName: "User"); 
    }

    private async Task UploadImage(string username, IFormFile file)
    {
        var filename = $"{username}_{file.FileName}";

        var destinationVehicleImagePath = $"wwwroot/Images/Profile/{filename}";

        using var fileStream = System.IO.File.Create(destinationVehicleImagePath);

        await file.CopyToAsync(fileStream);
    }
}
