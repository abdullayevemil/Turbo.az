using Microsoft.AspNetCore.Identity;

namespace Turbo.az.Models;

public class User : IdentityUser
{
    public bool IsBanned { get; set; }
    public DateTime BanExpirationDate { get; set; }
}