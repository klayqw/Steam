using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;
using Steam.Services.Base;

namespace Steam.Controllers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;
using System.Xml;

public class UserController : Controller
{
    private readonly IUserRepositoryBase _userRepository;
    private readonly ILogger<UserController> _logger;
    public UserController(IUserRepositoryBase userRepository, ILogger<UserController> logger)
    {
        this._userRepository = userRepository;
        _logger = logger;
    }


    [HttpGet]
    [Authorize]
    public IActionResult Error(string? returnUrl)
    {
        ViewData["returnUrl"] = returnUrl;

        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl)
    {
        base.ViewData["returnUrl"] = returnUrl;

        return base.View();
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserDto dto)
    {

        var result = await _userRepository.FindAsync(dto.Login,dto.Password);
        if(result == null)
        {
            return BadRequest();
        }
        var claims = new List<Claim> {
                new(ClaimTypes.Name, result.Login),
                new(ClaimTypes.Email,result.Email),
        };

        var claimsidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
               scheme: CookieAuthenticationDefaults.AuthenticationScheme,
               principal: new ClaimsPrincipal(claimsidentity)
        );

        return RedirectToAction("GetAll", "Game");

    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registration([FromForm]UserDto user)
    {
        try
        {
            await _userRepository.AddAsync(new User()
            {
                Login = user.Login,
                Password = user.Password,
                Email = user.Email,
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return RedirectToAction("Login");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> LogOut()
    {
        await base.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect("/");

    }
    [HttpGet]
    [Authorize]
    [ActionName("Profile")]
    public async Task<IActionResult> Profile()
    {
        var user = await _userRepository.FindByLoginAsync(base.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);
        return View(user);
    }
   
}
