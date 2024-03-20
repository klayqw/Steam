using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Steam.Models;
using Steam.Services.Base;
using Steam.ViewModel;

namespace Steam.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminPanel adminPanel;
    private readonly UserManager<User> userManager;
    public AdminController(IAdminPanel adminPanel,UserManager<User> userManager)
    {
        this.adminPanel = adminPanel;
        this.userManager = userManager;
    }

    [HttpGet]
    public IActionResult Panel()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AllUser()
    {
        IEnumerable<User> result;
        var userslist = new List<UserForAdminViewModel>();
        try
        {
            result = await adminPanel.GetAllUser();
           
            foreach (var user in result)
            {
                var rolelist = await userManager.GetRolesAsync(user);
                var role = rolelist.First();
                userslist.Add(new UserForAdminViewModel()
                {
                    user = user,
                    role = role,
                });
            }
        }catch (Exception ex) 
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
        return View(new AdminViewModel()
        {
            users = userslist
        });
    }

    [HttpPut]
    public async Task<IActionResult> Ban(string id)
    {
        try
        {
            await adminPanel.BanUserById(id);
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
        return RedirectToAction("AllUser");
    }

    [HttpPut]
    public async Task<IActionResult> Unban(string id)
    {
        await adminPanel.UnBanUserById(id);
        return Redirect("AllUser");
    }
}
