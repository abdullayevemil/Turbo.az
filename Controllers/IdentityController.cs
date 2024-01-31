using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Turbo.az.Dtos;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Controllers;
public class IdentityController : Controller
{
    private readonly IDataProtector dataProtector;
    private readonly IUserRepository userRepository;
    public IdentityController(IDataProtectionProvider dataProtectionProvider, IUserRepository userRepository)
    {
        this.dataProtector = dataProtectionProvider.CreateProtector("TEST");
        
        this.userRepository = userRepository;
    }
    
    [HttpGet]
    public IActionResult Login() => base.View();

    [HttpGet]
    public IActionResult Register() => base.View();

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] UserDto userDto)
    {
        var foundUser = await userRepository.GetUserByLoginAndPassword(userDto);

        if (foundUser is not null)
        {
            var userHash = this.dataProtector.Protect(foundUser.Id.ToString());

            base.HttpContext.Response.Cookies.Append("Authorize", userHash);

            return RedirectToAction("Index", "Vehicle");
        }
        else
        {
            return BadRequest("Incorrect login or password!");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] UserDto userDto)
    {
        await userRepository.InsertUserAsync(new Models.User 
        {
            Login = userDto.Login,
            Password = userDto.Password
        });
        
        return base.RedirectToAction("Login");
    }
}