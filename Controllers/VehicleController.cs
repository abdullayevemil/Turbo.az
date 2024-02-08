using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turbo.az.Dtos;
using Turbo.az.Models;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Controllers;

[Authorize]
public class VehicleController : Controller
{
    private readonly IVehicleRepository vehicleRepository;
    public VehicleController(IVehicleRepository vehicleRepository) => this.vehicleRepository = vehicleRepository;

    [HttpGet]
    [ActionName("Index")]
    [Route("[controller]")]
    [Route("[controller]/[action]")]
    public async Task<IActionResult> ShowAllVehicles()
    {
        var vehicles = await this.vehicleRepository.GetAllVehiclesAsync();

        return base.View(model: vehicles);
    }

    [HttpGet]
    [ActionName("Details")]
    [Route("[controller]/{id}")]
    [Route("[controller]/Index/{id}")]
    public async Task<IActionResult> ShowVehicleDetails(int id)
    {
        var selectedVehicle = await vehicleRepository.GetVehicleById(id);

        return base.View(model: selectedVehicle);
    }

    [HttpGet]
    public IActionResult Create() => base.View();

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] VehicleDto vehicleDto)
    {
        await this.vehicleRepository.InsertVehicleAsync(new Vehicle
        {
            BrandName = vehicleDto.BrandName,
            ModelName = vehicleDto.ModelName,
            Price = vehicleDto.Price,
            EngineVolume = vehicleDto.EngineVolume,
            ImageUrl = vehicleDto.ImageUrl,
            HorsePowers = vehicleDto.HorsePowers,
            SeatsCount = vehicleDto.SeatsCount,
            Color = vehicleDto.Color,
            TransmissionType = vehicleDto.TransmissionType,
            Drivetrain = vehicleDto.Drivetrain
        });

        return base.RedirectToAction("Index");
    }
}