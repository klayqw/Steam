using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steam.Models;
using Steam.Services.Base;

namespace Steam.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminPanel adminPanel;
    public AdminController(IAdminPanel adminPanel)
    {
        this.adminPanel = adminPanel;
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
        try
        {
            result = await adminPanel.GetAllUser();
        }catch (Exception ex) 
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
        return View(result);
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
