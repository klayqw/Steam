using Steam.Models;

namespace Steam.Services.Base;

public interface IUserServiceBase
{
    public Task<IEnumerable<Game>> GetUserGames(string id);
    public Task<IEnumerable<Group>> GetUserGroups(string id);
    public Task<User> GetUser(string id);

}
