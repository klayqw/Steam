using StackOverflow.Services;
using StackOverflow.Models;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using System.Reflection.PortableExecutable;

namespace StackOverflow.Middleware;

public class LogMiddleware
{
    private readonly RequestDelegate next;
    public static bool IsOn = true;
    public LogMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext,ILogRepository logRepository)
    {

        if(IsOn == false)
        {
             await next.Invoke(httpContext);
             return;
        }
        var requestbody = string.Empty;
        var responsebody = string.Empty;
        if (httpContext.Request.Body.CanRead)
        {
            using (StreamReader reader = new StreamReader(httpContext.Request.Body))
            {
                requestbody = await reader.ReadToEndAsync();
            }
        }
        if (httpContext.Response.Body.CanRead)
        {
            using (StreamReader reader = new StreamReader(httpContext.Response.Body))
            {
                responsebody = await reader.ReadToEndAsync();
            }
        }

        logRepository.AddLog(new LogInfo()
        {
            userid = httpContext.User.GetHashCode(),
            methodtype = httpContext.Request.Method,
            statuscode = httpContext.Response.StatusCode,
            url = httpContext.Request.GetDisplayUrl(),
            request_body = requestbody,
            response_body = responsebody,
        });
        await next.Invoke(httpContext);
        return;
    }
}
