using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Controllers;

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
}
