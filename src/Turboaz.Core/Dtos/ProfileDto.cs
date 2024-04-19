using System.ComponentModel.DataAnnotations;

namespace Turboaz.Core.Dtos;

public class ProfileDto
{
    [EmailAddress]
    public string? Email { get; set; }

    public string? Login { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    public string? Surname { get; set; }
}