using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;
using Steam.Services.Base;

namespace Steam.Controllers;
using Microsoft.AspNetCore.DataProtection;

public class UserController : Controller
{
    private readonly IUserRepositoryBase _userRepository;
    private readonly IDataProtector dataProtector;
    private readonly ILogger<UserController> _logger;
    public UserController(IUserRepositoryBase userRepository, ILogger<UserController> logger, IDataProtectionProvider dataProtectionProvider)
    {
        this.dataProtector = dataProtectionProvider.CreateProtector("keytouser");
        this._userRepository = userRepository;
        _logger = logger;
    }


    [HttpGet]
    public IActionResult Login()
    {
        if (base.HttpContext.Request.Cookies["Authorize"] is not null)
        {
            return Redirect("/");
        }
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(UserDto dto)
    {
        try
        {
            var user = await _userRepository.FindAsync(dto.Login, dto.Password);
            if (user is null)
            {
                return BadRequest("Incorrect Login or Password");
            }
            else
            {
                var protected_data = dataProtector.Protect(user.Id.ToString());
                base.HttpContext.Response.Cookies.Append("Authorize", $"{protected_data}");
                return Redirect("/");
            }
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
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


}
