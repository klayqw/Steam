using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;
using Steam.Services.Base;
using Steam.ViewModel;
using System.Security.Claims;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Steam.Controllers;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IUserServiceBase userService;
    private readonly IFriendService friendService;

    public UserController(UserManager<IdentityUser> userManager,
      RoleManager<IdentityRole> roleManager,
      SignInManager<IdentityUser> signInManager,IUserServiceBase userService, IFriendService friendService)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.signInManager = signInManager;
        this.userService = userService;
        this.friendService = friendService;
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
            ModelState.AddModelError("Wrong Data", "Wrong login or password");
            

            return View("Login");
        }
        var result = await this.signInManager.PasswordSignInAsync(user, dto.Password, true, true);

        if (result.Succeeded == false)
        {
            ModelState.AddModelError("Wrong Data", "Wrong login or password");
            return View("Login");
        }

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
            AvatarUrl = "/uploads/default.jpg",
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
        var friends = await friendService.GetUserFriend(user.Id);
        var groups = await userService.GetUserGroups(user.Id);

        Console.WriteLine(user.IsOnline);
        return View(new UserViewModel()
        {
            user = user,
            games = games,
            groups = groups,
            Friends = friends,
            IsAnotherUser = false,
            IsRequested = false
        });

    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Join()
    {
        await userService.UpdateUserOnlineStatus(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, true);
        return Ok();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Leave()
    {
        await userService.UpdateUserOnlineStatus(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, false);
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ProfileById(string id)
    {
        var user = await userService.GetUser(id);
        var games = await userService.GetUserGames(user.Id);
        var groups = await userService.GetUserGroups(user.Id);
        var friends = await friendService.GetUserFriend(user.Id);

        return View("Profile",new UserViewModel()
        {
            user = user,
            games = games,
            groups = groups,
            Friends = friends,
            IsAnotherUser = id != User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            IsRequested = await friendService.IsAlreadyRequest(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, user.Id),
            IsFriend = friendService.GetUserFriend(User.FindFirst(ClaimTypes.NameIdentifier)?.Value).Result.Any(x => x.Id == id)
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
        var friends = await friendService.GetUserFriend(user.Id);

        return View("Profile",new UserViewModel()
        {
            user = user,
            games = games,
            groups = groups,
            Friends = friends,
            IsAnotherUser = nickname != User.Identity.Name,
            IsRequested = await friendService.IsAlreadyRequest(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, user.Id)
        });;

    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Libary()
    {
        var user = await userService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var games = await userService.GetUserGames(user.Id);
        var friends = await friendService.GetUserFriend(user.Id);
        Console.WriteLine(friends);
        IEnumerable<FriendsGame> friendsgames = new List<FriendsGame>();
        foreach (var friend in friends)
        {
            var gamesoffriend = await userService.GetUserGames(friend.Id);
            friendsgames = friendsgames.Append(new FriendsGame()
            {
                user = friend,
                games = gamesoffriend,
            });
        }
        return View(new LibaryViewModel()
        {
            games = games,
            friends = friendsgames,
        });
       
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
        });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UpdateById(string id)
    {
        var user = await userService.GetUser(id);
        return View("Update",new UpdateDto()
        {
        });
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody]UpdateDto dto )
    {
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RequestToFriend(string friendId)
    {
        Console.WriteLine("1");
        await friendService.RequestToAdd(User.Identity.Name, User.FindFirst(ClaimTypes.NameIdentifier)?.Value, friendId);
        return RedirectToAction("ProfileById", new { id = friendId});
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Friends()
    {
        var friend = await friendService.GetUserFriend(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return View(friend);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteFromFriends(string id)
    {
        await friendService.Delete(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, id);
        return RedirectToAction("ProfileById", new { id });
    }

    [HttpGet]
    [Authorize]
    public IActionResult ChangeAvatar()
    {
        return View();
    }


    [HttpPut]
    [Authorize]
    public async Task<IActionResult> ChangeAvatar(IFormFile avatar)
    {
        using (var image = Image.FromStream(avatar.OpenReadStream()))
        {
            if (image.Width > 800 || image.Height > 800)
            {
                ModelState.AddModelError(string.Empty, "Photo cannot be more than 600 x 600 pixels.");
                return View("ChangeAvatar");
            }
        }

        var userName = User.Identity.Name;
        var fileName = $"{userName}{Path.GetExtension(avatar.FileName)}";

        var destinationFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        var destinationPath = Path.Combine(destinationFolder, fileName); 

        if (System.IO.File.Exists(destinationPath))
        {
            System.IO.File.Delete(destinationPath);
        }

        Directory.CreateDirectory(destinationFolder);

        using (var stream = new FileStream(destinationPath, FileMode.Create))
        {
            await avatar.CopyToAsync(stream);
        }

        var relativePath = "/uploads/" + fileName;

        await userService.UpdateAvatar(relativePath, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        return RedirectToAction("Profile");
    }

}
