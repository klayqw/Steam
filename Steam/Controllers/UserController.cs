using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;

namespace Steam.Controllers;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public UserController(UserManager<IdentityUser> userManager,
      RoleManager<IdentityRole> roleManager,
      SignInManager<IdentityUser> signInManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginDto dto)
    {
        var user = await this.userManager.FindByNameAsync(dto.Login);

        if (user == null)
        {
            return BadRequest();
        }
        var result = await this.signInManager.PasswordSignInAsync(user, dto.Password, true, true);

        if (result.Succeeded == false)
            return BadRequest();

        return Redirect("/");
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registration([FromForm] RegistrationDto dto)
    {
        var newUser = new User
        {
            Email = dto.Email,
            UserName = dto.Login,
            AvatarUrl = "https://otvet.imgsmail.ru/download/u_5b13a2ab2b3112095c60260400df34ca_800.jpg",
        };
        var result = await this.userManager.CreateAsync(newUser, dto.Password);

        if (result.Succeeded == false)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return View("Registration");
        }

        if (dto.Login.Contains("admin"))
        {
            var role = new IdentityRole { Name = "Admin" };
            await roleManager.CreateAsync(role);
            await userManager.AddToRoleAsync(newUser, role.Name);
        }
        else
        {
            var role = new IdentityRole { Name = "User" };
            await roleManager.CreateAsync(role);
            await userManager.AddToRoleAsync(newUser, role.Name);
        }

        return RedirectToAction("Login");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await signInManager.SignOutAsync();
        return Redirect("/");
    }

}
