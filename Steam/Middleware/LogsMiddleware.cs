using static System.Net.Mime.MediaTypeNames;
using System.Net;
using Steam.Services.Base;
using Steam.Services;
using System;
using Steam.Models;
using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Claims;

namespace Steam.Middleware;
public class LogsMiddleware
{
    private readonly RequestDelegate _next;

    public LogsMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
  
    public async Task<string> GetRequestBodyAsync(HttpContext context)
    {
        using (StreamReader reader = new StreamReader(context.Request.Body))
        {
            string requestBody = await reader.ReadToEndAsync();

            return requestBody;
        }
    }

    public async Task<string> GetResponseBodyAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;

            await _next(context);

            context.Response.Body = originalBodyStream;

            memoryStream.Seek(0, SeekOrigin.Begin);

            return await new StreamReader(memoryStream).ReadToEndAsync();
        }
    }

    public async Task InvokeAsync(HttpContext httpContext, ILogRepository logger)
    {
        var log = new Log()
        {
            UserId = httpContext.User.GetHashCode().ToString(),
            Url = httpContext.Request.GetDisplayUrl(),
            MethodType = httpContext.Request.Method,
            StatusCode = httpContext.Response.StatusCode.ToString(),
            RequestBody = await GetRequestBodyAsync(httpContext),
            ResponseBody = await GetResponseBodyAsync(httpContext),
        };

        await logger.Add(log);

        await _next.Invoke(httpContext);
    }

}
