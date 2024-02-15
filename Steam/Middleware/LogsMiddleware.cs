using static System.Net.Mime.MediaTypeNames;
using System.Net;
using Steam.Services.Base;
using Steam.Services;
using System;
using Steam.Models;
using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection;

namespace Steam.Middleware;
public class LogsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDataProtector dataProtector;

    public LogsMiddleware(RequestDelegate next, IDataProtectionProvider dataProtectionProvider)
    {
        this.dataProtector = dataProtectionProvider.CreateProtector("keytouser");
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext, ILogRepository logger)
    {
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