using Microsoft.AspNetCore.Identity;

namespace Turboaz.Core.Models;

public class User : IdentityUser
{
    public bool IsBanned { get; set; }
}