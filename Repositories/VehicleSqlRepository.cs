using System.Data.SqlClient;
using Dapper;
using Turbo.az.Models;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Repositories;
public class VehicleSqlRepository : IVehicleRepository
{
    private const string ConnectionString = "Server=localhost;Database=VehiclesDb;User Id=admin;Password=admin;";

    public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
    {
        using var connection = new SqlConnection(ConnectionString);

        var vehicles = await connection.QueryAsync<Vehicle>("select * from Vehicles");

        return vehicles;
    }

    public async Task InsertVehicleAsync(Vehicle vehicle) 
    {
        using var connection = new SqlConnection(ConnectionString);

        var vehicles = await connection.ExecuteAsync(
            sql: "insert into Vehicles (Brand, Model, Price, EngineVolume) values (@Brand, @Model, @Price, @EngineVolume);",
            param: vehicle);
    }
}