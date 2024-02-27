#pragma warning disable CS8604

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turbo.az.Dtos;
using Turbo.az.Repositories.Base;
using Turbo.az.Services;

namespace Turbo.az.Controllers;

[Authorize]
public class VehicleController : Controller
{
    private readonly IVehicleRepository vehicleRepository;
    public VehicleController(IVehicleRepository vehicleRepository) => this.vehicleRepository = vehicleRepository;

    [HttpGet]
    [AllowAnonymous]
    [ActionName("Index")]
    [Route("[controller]")]
    [Route("[controller]/[action]")]
    public IActionResult ShowAllVehicles()
    {
        var vehicles = this.vehicleRepository.GetAllVehicles();

        return base.View(model: vehicles);
    }

    [HttpGet]
    [AllowAnonymous]
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
        var vehicle = VehicleBuilder.Create(vehicleDto, default, base.HttpContext.User.Identity.Name);

        await this.vehicleRepository.InsertVehicleAsync(vehicle);

        return base.RedirectToAction("Index");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await this.vehicleRepository.DeleteVehicleAsync(id);

        return base.Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var vehicle = await this.vehicleRepository.GetVehicleByIdAsync(id);

        return base.View(model: vehicle);
    }

    [HttpPut]
    [Consumes("application/json")]
    public async Task<IActionResult> Update(int id, [FromBody]VehicleDto vehicleDto)
    {
        var vehicle = VehicleBuilder.Create(vehicleDto, id, base.HttpContext.User.Identity.Name);

        await this.vehicleRepository.UpdateVehicleAsync(id, vehicle);

        return base.RedirectToAction(actionName: "Index");
    }
}