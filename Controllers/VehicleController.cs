#pragma warning disable CS8604

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
    public IActionResult ShowAllVehicles()
    {
        var vehicles = this.vehicleRepository.GetAllVehicles();

        return base.View(model: vehicles);
    }

    [HttpGet]
    [ActionName("Details")]
    [Route("[controller]/{id}")]
    [Route("[controller]/Index/{id}")]
    public async Task<IActionResult> ShowVehicleDetails(int id)
    {
        var selectedVehicle = await vehicleRepository.GetVehicleByIdAsync(id);

        return base.View(model: selectedVehicle);
    }

    [HttpGet]
    [ActionName("UserVehicles")]
    [Route("[controller]/UserVehicles")]
    public IActionResult ShowUserVehicles()
    {
        var userLogin = base.HttpContext.User.Identity!.Name;

        var userVehicles = vehicleRepository.GetUserVehicles(userLogin);

        return base.View(model: userVehicles);
    }

    [HttpGet]
    [Route("[controller]/[action]")]
    public IActionResult Create() => base.View();

    [HttpPost]
    [Route("[controller]/[action]")]
    public async Task<IActionResult> Create([FromForm] VehicleDto vehicleDto)
    {
        await this.vehicleRepository.InsertVehicleAsync(new Vehicle
        {
            UserLogin = base.HttpContext.User.Identity!.Name,
            BrandName = vehicleDto.BrandName,
            ModelName = vehicleDto.ModelName,
            Price = vehicleDto.Price,
            EngineVolume = vehicleDto.EngineVolume,
            ImageUrl = vehicleDto.ImageUrl,
            HorsePowers = vehicleDto.HorsePowers,
            SeatsCount = vehicleDto.SeatsCount,
            Color = vehicleDto.Color,
            TransmissionType = vehicleDto.TransmissionType,
            Drivetrain = vehicleDto.Drivetrain,
        });

        return base.RedirectToAction("Index");
    }
}