using System.Net;
using System.Text.Json;
using Turbo.az.Attributes;
using Turbo.az.Models;
using Dapper;
using System.Data.SqlClient;
using Turbo.az.Extensions;
namespace Turbo.az.Controllers;
public class VehicleController
{
    private const string connectionString = "Server=localhost;Database=TradingAppDb;User Id=HP;Password=WDAGUtilityAccount;";
    [HttpGet("GetAll")]
    public async Task GetVehiclesAsync(HttpListenerContext context)
    {
        using var writer = new StreamWriter(context.Response.OutputStream);
        using var connection = new SqlConnection(connectionString);
        var vehicles = await connection.QueryAsync<Vehicle>("select * from Vehicles");
        var vehiclesHtml = vehicles.GetHtml();
        await writer.WriteLineAsync(vehiclesHtml);
        context.Response.ContentType = "text/html";
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
    [HttpGet("GetById")]
    public async Task GetVehicleByIdAsync(HttpListenerContext context)
    {
        var vehicleIdToGetObj = context.Request.QueryString["id"];
        if (vehicleIdToGetObj == null || int.TryParse(vehicleIdToGetObj, out int vehicleIdToGet) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        using var connection = new SqlConnection(connectionString);
        var vehicle = await connection.QueryFirstOrDefaultAsync<Vehicle>(
            sql: "select top 1 * from Vehicles where Id = @Id",
            param: new { Id = vehicleIdToGet });
        if (vehicle is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }
        using var writer = new StreamWriter(context.Response.OutputStream);
        await writer.WriteLineAsync(JsonSerializer.Serialize(vehicle));
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
    [HttpPost("Create")]
    public async Task PostVehicleAsync(HttpListenerContext context)
    {
        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();
        var newVehicle = JsonSerializer.Deserialize<Vehicle>(json);
        if (newVehicle == null || string.IsNullOrWhiteSpace(newVehicle.Model) || string.IsNullOrWhiteSpace(newVehicle.Brand))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        using var connection = new SqlConnection(connectionString);
        var vehicles = await connection.ExecuteAsync(
            @"insert into Vehicles (Price, Brand, Model, EngineVolume) 
        values(@Price, @Brand, @Model, @EngineVolume)",
            param: new
            {
                newVehicle.Price,
                newVehicle.Brand,
                newVehicle.Model,
                newVehicle.EngineVolume,
            });
        context.Response.StatusCode = (int)HttpStatusCode.Created;
    }
    [HttpDelete]
    public async Task DeleteVehicleAsync(HttpListenerContext context)
    {
        var vehicleIdToDeleteObj = context.Request.QueryString["id"];
        if (vehicleIdToDeleteObj == null || int.TryParse(vehicleIdToDeleteObj, out int vehicleIdToDelete) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        using var connection = new SqlConnection(connectionString);
        var deletedRowsCount = await connection.ExecuteAsync(
            @"delete Vehicles
        where Id = @Id",
            param: new
            {
                Id = vehicleIdToDelete,
            });
        if (deletedRowsCount == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
    [HttpPut]
    public async Task PutVehicleAsync(HttpListenerContext context)
    {
        var vehicleIdToUpdateObj = context.Request.QueryString["id"];
        if (vehicleIdToUpdateObj == null || int.TryParse(vehicleIdToUpdateObj, out int vehicleIdToUpdate) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();
        var vehicleToUpdate = JsonSerializer.Deserialize<Vehicle>(json);
        if (vehicleToUpdate == null || string.IsNullOrWhiteSpace(vehicleToUpdate.Model) || string.IsNullOrWhiteSpace(vehicleToUpdate.Brand))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        using var connection = new SqlConnection(connectionString);
        var affectedRowsCount = await connection.ExecuteAsync(
            @"update Vehicles
        set Price = @Price, Brand = @Brand, Model = @Model, EngineVolume = @EngineVolume
        where Id = @Id",
            param: new
            {
                vehicleToUpdate.Price,
                vehicleToUpdate.Brand,
                vehicleToUpdate.Model,
                vehicleToUpdate.EngineVolume,
                Id = vehicleIdToUpdate
            });
        if (affectedRowsCount == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
}