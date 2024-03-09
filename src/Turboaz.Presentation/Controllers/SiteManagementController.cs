using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Turboaz.Presentation.Controllers;

[Authorize(Roles = "Admin")]
public class SiteManagementController : Controller
{
    [HttpGet]
    public IActionResult UploadWallpaper() => base.View();

    [HttpPost]
    [OutputCache(NoStore = true, Duration = -1)]
    public async Task<IActionResult> UploadWallpaper(IFormFile file)
    {
        var fileExtension = new FileInfo(file.FileName).Extension;

        var filename = $"Wallpaper{fileExtension}";

        var destinationWallpaperPath = $"wwwroot/Wallpaper/{filename}";

        using var fileStream = System.IO.File.Create(destinationWallpaperPath);

        await file.CopyToAsync(fileStream);

        return base.View("UploadWallpaper");
    }
}
