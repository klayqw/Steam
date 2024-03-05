using Steam.Models;

namespace Steam.Services.Base;

public interface IFriendService
{
    public Task<IEnumerable<User>> GetUserFriend(string id);
    public Task<bool> IsAlreadyRequest(string id,string userto);
    public Task RequestToAdd(string username, string id, string toid);
    public Task Accept(string id, string friendid);
    public Task Delete(string id,string userid);
}
