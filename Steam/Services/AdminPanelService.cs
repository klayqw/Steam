using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Models;
using Steam.Services.Base;

namespace Steam.Services;

public class AdminPanelService : IAdminPanel
{
    private readonly SteamDBContext steamDBContext;
    private readonly UserManager<IdentityUser> userManager;
    public AdminPanelService(SteamDBContext steamDBContext, UserManager<IdentityUser> user) 
    {
        this.steamDBContext = steamDBContext;
        this.userManager = user;
    }

    public async Task<IActionResult> BanUserById(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if(user == null)
        {
            throw new NullReferenceException($"User not found by id {id}");
        }
        await userManager.DeleteAsync(user);
        return new OkResult();
    }

    public async Task<IEnumerable<User>> GetAllUser()
    {
        var users = userManager.Users.ToList();
        if(users == null)
        {
            throw new NullReferenceException("Problems with database");
        }
        var usersWithoutAdminRole = users.Where(u => !userManager.IsInRoleAsync(u, "Admin").Result).OfType<User>().ToList();
        return usersWithoutAdminRole;
    }
}
