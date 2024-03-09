using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turboaz.Core.Repositories;

namespace Turboaz.Presentation.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IUserRepository userRepository;

    public UserController(IUserRepository userRepository) => this.userRepository = userRepository;

    [HttpGet]
    [Route("[controller]/Info")]
    public IActionResult UserInfo()
    {
        var claims = base.User.Claims;

        return base.View(model: claims);
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
}
