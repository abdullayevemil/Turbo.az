using Turboaz.Core.Dtos;
using Turboaz.Core.Models;

namespace Turboaz.Infrastructure.Services;

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

    public static void InitializeVehicle(Vehicle vehicleToUpdate, Vehicle newVehicle)
    {
        vehicleToUpdate.BrandName = newVehicle.BrandName;

        vehicleToUpdate.ModelName = newVehicle.ModelName;
        
        vehicleToUpdate.Price = newVehicle.Price;
        
        vehicleToUpdate.EngineVolume = newVehicle.EngineVolume;

        vehicleToUpdate.HorsePowers = newVehicle.HorsePowers;

        vehicleToUpdate.TransmissionType = newVehicle.TransmissionType;

        vehicleToUpdate.Drivetrain = newVehicle.Drivetrain;

        vehicleToUpdate.UserLogin = newVehicle.UserLogin;

        vehicleToUpdate.Color = newVehicle.Color;

        vehicleToUpdate.SeatsCount = newVehicle.SeatsCount;
    }
}