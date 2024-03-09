#pragma warning disable CS8604

using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turboaz.Core.Dtos;
using Turboaz.Core.Repositories;
using Turboaz.Infrastructure.Services;

namespace Turboaz.Presentation.Controllers;

[Authorize]
public class VehicleController : Controller
{
    private readonly IVehicleRepository vehicleRepository;
    public VehicleController(IVehicleRepository vehicleRepository) => this.vehicleRepository = vehicleRepository;

    [HttpGet]
    [AllowAnonymous]
    [ActionName("Index")]
    [Route("[controller]/[action]")]
    public IActionResult ShowAllVehicles()
    {
        var vehicles = this.vehicleRepository.GetAllVehicles();

        return base.View(model: vehicles);
    }

    [HttpGet]
    [AllowAnonymous]
    [ActionName("Details")]
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
    public IActionResult Create() => base.View();

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] VehicleDto vehicleDto, [FromForm] IFormFileCollection files)
    {
        await UploadImages(files);

        var vehicle = VehicleBuilder.Create(vehicleDto, default, base.HttpContext.User.Identity.Name);

        StringBuilder stringBuilder = new StringBuilder();

        bool isFirst = true;

        foreach (var file in files)
        {
            if (isFirst)
            {
                vehicle.FirstImageUrl = "Images/" + base.HttpContext.User.Identity.Name + '_' + file.FileName;
            }
            stringBuilder.Append("Images/" + base.HttpContext.User.Identity.Name + '_' + file.FileName + ';');
        }

        vehicle.ImageUrls = stringBuilder.ToString();

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
    public async Task<IActionResult> Update(int id, [FromBody] VehicleDto vehicleDto)
    {
        var vehicle = VehicleBuilder.Create(vehicleDto, id, base.HttpContext.User.Identity.Name);

        await this.vehicleRepository.UpdateVehicleAsync(id, vehicle);

        return base.RedirectToAction(actionName: "Index");
    }

    public async Task UploadImages(IFormFileCollection files)
    {
        string destinationVehicleImagePath;

        string filename;

        foreach (var file in files)
        {
            filename = $"{base.HttpContext.User.Identity!.Name}_{file.FileName}";

            destinationVehicleImagePath = $"wwwroot/Images/{filename}";

            using var fileStream = System.IO.File.Create(destinationVehicleImagePath);

            await file.CopyToAsync(fileStream);
        }
    }
}