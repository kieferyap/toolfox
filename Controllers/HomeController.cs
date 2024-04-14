using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using toolfox.Models;
using toolfox.Models.ViewModels;

namespace toolfox.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // View() matches the name of the function, that is, "Index"...
        // ...to the filename within Views, that is, "Index.cshtml"
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult History()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
