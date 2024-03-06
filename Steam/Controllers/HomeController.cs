using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Steam.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult S()
    {
        return Redirect("https://www.tiktok.com/@akechil0ver/video/7326821723632831787?lang=ru-RU");
    }
    public IActionResult Privacy()
    {
        return View();
    }

}
