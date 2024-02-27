using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        var result = await adminPanel.GetAllUser();
        return View(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Ban(string id)
    {
        Console.WriteLine(id);
        await adminPanel.BanUserById(id);
        return RedirectToAction("AllUser");
    }
}
