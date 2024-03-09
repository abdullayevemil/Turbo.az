using System.ComponentModel.DataAnnotations;

namespace Turboaz.Core.Dtos;

public class RegisterDto
{
    [EmailAddress]
    public string? Email { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }
 
    [Phone]
    public string? PhoneNumber { get; set; }
}