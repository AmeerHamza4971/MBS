using Microsoft.AspNetCore.Mvc;

namespace MBS.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
