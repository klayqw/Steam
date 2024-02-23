using Microsoft.AspNetCore.Mvc;
using Steam.Middleware;

namespace Steam.Controllers;

public class LogMiddlewareController : Controller
{
    public LogMiddlewareController() { }

    public IActionResult ChangeLog()
    {
        LogsMiddleware.IsOn = LogsMiddleware.IsOn == false ? true : false;
        return Redirect("/");
    }
}
