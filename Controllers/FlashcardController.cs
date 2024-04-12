using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using toolfox.Models;
using toolfox.Models.ViewModels;

namespace toolfox.Controllers;

public class FlashcardController : Controller
{
    private readonly ILogger<FlashcardController> _logger;

    public FlashcardController(ILogger<FlashcardController> logger)
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
