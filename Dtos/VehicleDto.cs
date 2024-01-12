using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turbo.az.Dtos;
public class VehicleDto
{
    public double? Price { get; set; }
    public string? BrandName { get; set; }
    public string? ModelName { get; set; }
    public int? EngineVolume { get; set; }
    public string? ImageUrl { get; set; }
}