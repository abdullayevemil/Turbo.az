namespace Turboaz.Core.Models;

public class Vehicle
{
    public int Id { get; set; }
    
    public string? UserLogin { get; set; }
    
    public double? Price { get; set; }
    
    public string? BrandName { get; set; }
    
    public string? ModelName { get; set; }
    
    public int? EngineVolume { get; set; }
    
    public string? ImageUrls { get; set; }
    
    public string? FirstImageUrl { get; set; }
    
    public int? HorsePowers { get; set; }
    
    public int? SeatsCount { get; set; }
    
    public string? Color { get; set; }
    
    public TransmissionType? TransmissionType { get; set; }
    
    public Drivetrain? Drivetrain { get; set; }
}