using System.ComponentModel.DataAnnotations;

namespace Turboaz.Core.Dtos;

[Serializable]
public class VehicleDto
{
    public string? BrandName { get; set; }

    public string? ModelName { get; set; }

    [Range(0, double.MaxValue)]
    public double? Price { get; set; }

    [Range(0, int.MaxValue)]
    public int? EngineVolume { get; set; }

    public string? ImageUrls { get; set; }

    public string? FirstImageUrl { get; set; }

    [Range(0, int.MaxValue)]
    public int? HorsePowers { get; set; }

    [Range(0, int.MaxValue)]
    public int? SeatsCount { get; set; }

    public string? Color { get; set; }

    public int TransmissionType { get; set; }
    
    public int Drivetrain { get; set; }
}