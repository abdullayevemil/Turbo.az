using System.Data.SqlClient;
using Dapper;
using Turbo.az.Models;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Repositories;

public class VehicleSqlRepository : IVehicleRepository
{
    private readonly string connectionString;

    public VehicleSqlRepository(string connectionString) => this.connectionString = connectionString;

    public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
    {
        using var connection = new SqlConnection(connectionString);

        var vehicles = await connection.QueryAsync<Vehicle>("select * from Vehicles");

        return vehicles;
    }

    public async Task<Vehicle?> GetVehicleByIdAsync(int id)
    {
        using var connection = new SqlConnection(connectionString);

        var vehicles = await connection.QueryAsync<Vehicle>(
                sql: "select * from Vehicles where Id=@Id",
                param: new { Id = id });

        var vehicle = vehicles.FirstOrDefault();

        return vehicle;
    }

    public async Task<IEnumerable<Vehicle?>> GetUserVehiclesAsync(string userLogin)
    {
        using var connection = new SqlConnection(connectionString);

        var userVehicles = await connection.QueryAsync<Vehicle>(
            sql: "select * from Vehicles where UserLogin=@UserLogin",
            param: new { UserLogin = userLogin });

        return userVehicles;
    }

    public async Task InsertVehicleAsync(Vehicle vehicle)
    {
        using var connection = new SqlConnection(connectionString);

        var vehicles = await connection.ExecuteAsync(
            sql: "insert into Vehicles (UserLogin, BrandName, ModelName, Price, EngineVolume, ImageUrl, HorsePowers, SeatsCount, Color, TransmissionType, Drivetrain) values (@UserLogin, @BrandName, @ModelName, @Price, @EngineVolume, @ImageUrl, @HorsePowers, @SeatsCount, @Color, @TransmissionType, @Drivetrain);",
            param: vehicle);
    }
}