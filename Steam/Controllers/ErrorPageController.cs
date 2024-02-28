using Microsoft.AspNetCore.Mvc;
using Steam.ViewModel;

namespace Steam.Controllers;

public class ErrorPageController : Controller
{
    public ErrorPageController() { }

    [HttpGet]
    public IActionResult Error(string message)
    {
        return View(new ErrorViewModel() { Message = message});
    }
}
