using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Steam.Middleware
{
    public class CheckIsBannedMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckIsBannedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, UserManager<IdentityUser> _userManager)
        {
            var user = await _userManager.GetUserAsync(httpContext.User);
            if (user == null)
            {
                await _next.Invoke(httpContext);
                return;
            }
            if (httpContext.Request.Path.StartsWithSegments("/User/Logout"))
            {
                await _next.Invoke(httpContext);
                return;
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Ban") && !httpContext.Request.Path.StartsWithSegments("/ErrorPage/Banned"))
            {
                httpContext.Response.Redirect("/ErrorPage/Banned");
                return;
            }

            await _next.Invoke(httpContext);
        }
    }
}