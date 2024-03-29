using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Turboaz.Presentation.Controllers;

public class SiteManagementController : Controller
{
    public IActionResult ChangeTheme(IOptionsMonitor<string> theme)
    {
        TempData["Theme"] = theme.CurrentValue;

        return base.RedirectToAction(actionName: "Main", controllerName: "Home");
    }
}
