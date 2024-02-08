using Turbo.az.Models;

namespace Turbo.az.Repositories.Base;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
    Task<Vehicle?> GetVehicleByIdAsync(int id);
    Task<IEnumerable<Vehicle?>> GetUserVehiclesAsync(string userLogin);
    Task InsertVehicleAsync(Vehicle vehicle);
}