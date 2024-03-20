using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Steam.Data;
using Steam.Dto;
using Steam.Models;
using Steam.Services.Base;
using Steam.ViewModel;

namespace Steam.Services;

public class UserService : IUserServiceBase
{

    private readonly SteamDBContext _dbContext;
    private readonly UserManager<User> userManager;
    public UserService(SteamDBContext _dbContext,UserManager<User> user)
    {
        this._dbContext = _dbContext;
        this.userManager = user;
    }

    public async Task<User> GetUser(string id)
    {
        var user = await _dbContext.Users.OfType<User>().FirstOrDefaultAsync(x => x.Id == id);
        Console.WriteLine(user.IsOnline);
        return user;
    }

    public async Task<IEnumerable<Game>> GetUserGames(string id)
    {
        var userGames = await _dbContext.userGames.Where(x => x.UserId == id).Select(x => x.Game).ToArrayAsync();
        return userGames;
    }

    public async Task<IEnumerable<Group>> GetUserGroups(string id)
    {
        var userGroups = await _dbContext.userGroups.Where(x => x.UserId == id).Select(x => x.Group).ToArrayAsync();
        return userGroups;
    }

    public async Task<IEnumerable<User>> GetAllUser()
    {
        var users = userManager.Users.ToList();
        var usersWithoutAdminRole = users.Where(u => !userManager.IsInRoleAsync(u, "Admin").Result).OfType<User>().ToList();
        return usersWithoutAdminRole;
    }

    public async Task Update(UpdateDto dto, User user)
    {
        await userManager.UpdateAsync(user);
        if (dto.Password is null || dto.Password.IsNullOrEmpty() || dto.OldPassword is null || dto.OldPassword.IsNullOrEmpty())
        {
            return;
        }
        else
        {
            await userManager.ChangePasswordAsync(user,dto.OldPassword,dto.Password);
            return;
        }
    }

    public async Task<IEnumerable<User>> Search(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            var alluser = _dbContext.Users.OfType<User>();
            return alluser;
        }
        var users = _dbContext.Users.OfType<User>().Where(x => x.UserName.Contains(username));
        return users;
    }

    public async Task UpdateUserOnlineStatus(string userid, bool status)
    {
        var user = await _dbContext.Users.OfType<User>().FirstOrDefaultAsync(x => x.Id == userid);
        if(user == null)
        {
            return;
        }
        if (user.IsOnline == status)
        {
            return;
        }
        user.IsOnline = status;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAvatar(string url,string id)
    {
        var user = await _dbContext.Users.OfType<User>().FirstOrDefaultAsync(x => x.Id == id);
        user.AvatarUrl = url;
        await _dbContext.SaveChangesAsync();
    }
}
