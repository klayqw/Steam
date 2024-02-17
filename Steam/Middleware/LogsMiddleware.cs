using Steam.Services.Base;
using Steam.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.DataProtection;

namespace Steam.Middleware;
public class LogsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDataProtector dataProtector;
    public static bool IsOn = true;

    public LogsMiddleware(RequestDelegate next, IDataProtectionProvider dataProtectionProvider)
    {
        this.dataProtector = dataProtectionProvider.CreateProtector("keytouser");
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext, ILogRepository logger)
    {
        if (IsOn == false)
        {
            await _next(httpContext);
            return;
        }
        var id = httpContext.Request.Cookies["Authorize"] == null ? "undefind" : dataProtector.Unprotect(httpContext.Request.Cookies["Authorize"]);

        var log = new Log()
        {
            UserId = id,
            Url = httpContext.Request.GetDisplayUrl(),
            MethodType = httpContext.Request.Method,
            StatusCode = httpContext.Response.StatusCode.ToString(),
        };

        await logger.Add(log);
        await _next(httpContext);

    }

   
}