using Microsoft.AspNetCore.Mvc;
using Steam.Models;

namespace Steam.Controllers;

public class ErrorController : Controller
{
    public IActionResult Error(string message)
    {
        
        return View(new ErrorModel() { Message = message});
    }
}
