using Turboaz.Core.Models;

namespace Turboaz.Core.Repositories;

public interface IVehicleRepository
{
    IEnumerable<Vehicle> GetAllVehicles();
    Task<VehiclesMainInformation> GetVehiclesMainInformation();
    Task<Vehicle?> GetVehicleByIdAsync(int id);
    IEnumerable<Vehicle?> GetUserVehicles(string userId);
    Task InsertVehicleAsync(Vehicle vehicle);
    Task DeleteVehicleAsync(int id);
    Task UpdateVehicleAsync(int id, Vehicle newVehicle);
}