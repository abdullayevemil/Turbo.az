using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turbo.az.Dtos;
public class VehicleDto
{
    public double? Price { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? EngineVolume { get; set; }
}