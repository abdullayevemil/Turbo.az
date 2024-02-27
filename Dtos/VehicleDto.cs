using System.ComponentModel.DataAnnotations;

namespace Turbo.az.Dtos;

[Serializable]
public class VehicleDto
{
    [Required]
    public string? BrandName { get; set; }

    [Required]
    public string? ModelName { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double? Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int? EngineVolume { get; set; }

    [Required]
    public string? ImageUrl { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int? HorsePowers { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int? SeatsCount { get; set; }

    [Required]
    public string? Color { get; set; }

    [Required]
    public int TransmissionType { get; set; }
    
    [Required]
    public int Drivetrain { get; set; }
}