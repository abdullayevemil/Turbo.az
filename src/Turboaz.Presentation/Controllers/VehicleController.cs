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
    private readonly IUserRepository userRepository;

    public VehicleController(IVehicleRepository vehicleRepository, IUserRepository userRepository)
    {
        this.vehicleRepository = vehicleRepository;

        this.userRepository = userRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    [ActionName("Index")]
    [Route("[controller]/[action]")]
    [Route("[controller]/[action]/{search}")]
    public IActionResult ShowAllVehicles(string? search = null)
    {
        var vehicles = this.vehicleRepository.GetAllVehicles();

        if (search is null)
        {
            return base.View(model: vehicles);
        }

        var searchedVehicles = vehicles.Where(vehicle => 
        vehicle.BrandName!.ToLower().Contains(search.ToLower())
        || vehicle.ModelName!.ToLower().Contains(search.ToLower())
        || search.ToLower().Contains(vehicle.BrandName.ToLower())
        || search.ToLower().Contains(vehicle.ModelName.ToLower())
        );

        return base.View(model: searchedVehicles);
    }

    [HttpGet]
    [AllowAnonymous]
    [ActionName("Details")]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> ShowVehicleDetails(int id)
    {
        var selectedVehicle = await this.vehicleRepository.GetVehicleByIdAsync(id);

        return base.View(model: selectedVehicle);
    }

    [HttpGet]
    [ActionName("UserVehicles")]
    [Route("[controller]/UserVehicles")]
    public async Task<IActionResult> ShowUserVehicles()
    {
        var userName = base.HttpContext.User.Identity!.Name;

        var user = await this.userRepository.GetUserByUserNameAsync(userName);

        var userVehicles = this.vehicleRepository.GetUserVehicles(user!.Id);

        return base.View(model: userVehicles);
    }

    [HttpGet]
    public IActionResult Create() => base.View();

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] VehicleDto vehicleDto, [FromForm] IFormFileCollection files)
    {
        await this.UploadImages(files);

        var userName = base.HttpContext.User.Identity!.Name;

        var user = await this.userRepository.GetUserByUserNameAsync(userName);

        var vehicle = VehicleBuilder.Create(vehicleDto, default, user!.Id);

        StringBuilder stringBuilder = new StringBuilder();

        bool isFirst = true;

        foreach (var file in files)
        {
            if (isFirst)
            {
                vehicle.FirstImageUrl = "Images/Vehicles/" + userName + '_' + file.FileName;
            }
            stringBuilder.Append("Images/Vehicles/" + userName + '_' + file.FileName + ';');
        }

        vehicle.ImageUrls = stringBuilder.ToString();

        await this.vehicleRepository.InsertVehicleAsync(vehicle);

        return base.RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Filter(int? minimumPrice, int? maximumPrice)
    {
        var vehicles = this.vehicleRepository.GetAllVehicles();

        if (minimumPrice is null || maximumPrice is null)
        {
            return base.View(viewName: "Index", model: vehicles);
        }

        var filteredVehicles = vehicles.Where(vehicle =>
            vehicle.Price >= minimumPrice && vehicle.Price <= maximumPrice
        );

        return base.View(viewName: "Index", model: filteredVehicles);
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
        var vehicle = VehicleBuilder.Create(vehicleDto, id, base.HttpContext.User.Identity!.Name);

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

            destinationVehicleImagePath = $"wwwroot/Images/Vehicles/{filename}";

            using var fileStream = System.IO.File.Create(destinationVehicleImagePath);

            await file.CopyToAsync(fileStream);
        }
    }
}