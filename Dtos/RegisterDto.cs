using System.ComponentModel.DataAnnotations;

namespace Turbo.az.Dtos;

public class RegisterDto
{
    [EmailAddress]
    public string? Email { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }
 
    [Phone]
    public string? PhoneNumber { get; set; }
}