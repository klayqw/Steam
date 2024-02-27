using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;
using Steam.Services.Base;
using Steam.ViewModel;
using System.Security.Claims;

namespace Steam.Controllers;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IUserServiceBase userService;
    public UserController(UserManager<IdentityUser> userManager,
      RoleManager<IdentityRole> roleManager,
      SignInManager<IdentityUser> signInManager,IUserServiceBase userService)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.signInManager = signInManager;
        this.userService = userService;
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Search(string username)
    {
        var result = await userService.Search(username);
        var user = await userService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return View("FindAnotherProfile", new FindUserViewModel()
        {
            users = result,
            currentUser = user,
        });

    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await signInManager.SignOutAsync();
        return Redirect("/");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await userService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var games = await userService.GetUserGames(user.Id);
        var groups = await userService.GetUserGroups(user.Id);

        return View(new UserViewModel()
        {
            user = user,
            games = games,
            groups = groups,
        });

    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ProfileById(string id)
    {
        var user = await userService.GetUser(id);
        var games = await userService.GetUserGames(user.Id);
        var groups = await userService.GetUserGroups(user.Id);

        return View("Profile",new UserViewModel()
        {
            user = user,
            games = games,
            groups = groups,
        });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ProfileByNickname(string nickname)
    {
        var userid = await userManager.FindByNameAsync(nickname);
        var user = await userService.GetUser(userid.Id);
        var games = await userService.GetUserGames(user.Id);
        var groups = await userService.GetUserGroups(user.Id);

        return View(new UserViewModel()
        {
            user = user,
            games = games,
            groups = groups,
        });

    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Libary()
    {
        var user = await userService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var games = await userService.GetUserGames(user.Id);
        return View(games);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Settings()
    {
        return View();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Update()
    {
        var user = await userService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return View(new UpdateDto()
        {
            AvatarUrl = user.AvatarUrl,
        });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UpdateById(string id)
    {
        var user = await userService.GetUser(id);
        return View("Update",new UpdateDto()
        {
            AvatarUrl = user.AvatarUrl,
        });
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody]UpdateDto dto )
    {
        Console.WriteLine(dto.AvatarUrl);
        var user = await userService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        await userService.Update(dto, user);
        return RedirectToAction("Profile");
    }

    [HttpGet]
    [Authorize]

    public async Task<IActionResult> FindAnotherProfile()
    {
        var users = await userService.GetAllUser();
        var user = await userService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        return View(new FindUserViewModel()
        {
            users = users,
            currentUser = user,
        });
    }
}
