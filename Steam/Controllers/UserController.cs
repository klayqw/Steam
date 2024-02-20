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
    private readonly IDataProtector dataProtector;
    private readonly ILogger<UserController> _logger;
    private static User user;
    public UserController(IUserRepositoryBase userRepository, ILogger<UserController> logger, IDataProtectionProvider dataProtectionProvider)
    {
        this.dataProtector = dataProtectionProvider.CreateProtector("keytouser");
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
        Console.WriteLine(result);
        if(result == null)
        {
            return BadRequest();
        }
        user = result;
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

    [HttpPost]
    [Authorize]
    [ActionName("Add")]
    public async Task<IActionResult> AddGameToUserLibary(int gameid)
    {
        try
        {
            await _userRepository.AddGameAsync(gameid,user.Id);

        }
        catch(Exception ex)
        {
            return StatusCode(500,ex.Message);
        }
        return RedirectToAction("Profile");
    }

    [HttpGet]
    [Authorize]
    [ActionName("Profile")]
    public IActionResult Profile()
    {
        Console.WriteLine(user);
        return View(user);
    }
}
