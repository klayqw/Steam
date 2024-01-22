using Microsoft.AspNetCore.Mvc;
using StackOverflow.Middleware;

namespace StackOverflow.Controllers;

public class MiddlewareSettingController : Controller
{
    [HttpGet]
    [ActionName("Change")]
    public IActionResult Change()
    {
        Console.WriteLine(LogMiddleware.IsOn);
        if (LogMiddleware.IsOn)
        {
            LogMiddleware.IsOn = false;
            Console.WriteLine(LogMiddleware.IsOn);
            return Redirect("/");
        }
        Console.WriteLine(LogMiddleware.IsOn);
        LogMiddleware.IsOn = true;
        return Redirect("/");
    }
}
