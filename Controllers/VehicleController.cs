using Microsoft.AspNetCore.Mvc;
using Turbo.az.Models;

namespace Turbo.az.Controllers;
public class VehicleController : Controller
{
    public VehicleController() { }

    [ActionName("Index")]
    public IActionResult ShowAllVehicles()
    {
        IEnumerable<Vehicle> vehicles = new List<Vehicle> {
             new Vehicle {
                 Id = 1,
                 Brand = "VAZ",
                 Model = "2106",
                 Price = 4000,
                 EngineVolume = 1600
             },
         };

        return View(model: vehicles);
    }
}