using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Turbo.az.Controllers;

[Authorize]
public class UserController : Controller
{
    [HttpGet]
    [Route("[controller]/Info")]
    public IActionResult UserInfo()
    {
        var claims = base.User.Claims;

        return base.View(model: claims);
    }
}
