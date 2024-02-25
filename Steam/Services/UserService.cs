using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Models;
using Steam.Services.Base;

namespace Steam.Services;

public class UserService : IUserServiceBase
{

    private readonly SteamDBContext _dbContext;
    public UserService(SteamDBContext _dbContext)
    {
        this._dbContext = _dbContext;
    }

    public async Task<User> GetUser(string id)
    {
        var user = await _dbContext.Users.OfType<User>().FirstOrDefaultAsync(x => x.Id == id);
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
}
