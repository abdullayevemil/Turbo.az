using Turbo.az.Dtos;
using Turbo.az.Models;
using Turbo.az.Services.Base;

namespace Turbo.az.Services;

public class VehicleBuilder
{
    public static Vehicle Create(VehicleDto vehicleDto, int id, string userName)
    {
        return new Vehicle
        {
            Id = id,
            UserLogin = userName,
            BrandName = vehicleDto.BrandName,
            ModelName = vehicleDto.ModelName,
            Price = vehicleDto.Price,
            EngineVolume = vehicleDto.EngineVolume,
            ImageUrls = vehicleDto.ImageUrls,
            HorsePowers = vehicleDto.HorsePowers,
            SeatsCount = vehicleDto.SeatsCount,
            Color = vehicleDto.Color,
            TransmissionType = (TransmissionType?)vehicleDto.TransmissionType,
            Drivetrain = (Drivetrain?)vehicleDto.Drivetrain,
        };
    }
}