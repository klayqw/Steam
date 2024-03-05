using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Models;
using Steam.Services.Base;
using System.Security.Claims;

namespace Steam.Services;

public class FriendService : IFriendService
{
    private readonly SteamDBContext _dbContext;
    private readonly INotificationServiceBase _notificationService;
    public FriendService(SteamDBContext _dbContext, INotificationServiceBase notificationService)
    {
        this._dbContext = _dbContext;
        _notificationService = notificationService;
    }

    public async Task Accept(string id, string friendid)
    {
        await _dbContext.Friendships.AddAsync(new UserFriendship()
        {
            FriendId = friendid,
            UserId = id,
        });
        await _dbContext.Friendships.AddAsync(new UserFriendship()
        {
            FriendId = id,
            UserId = friendid,
        });
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(string id, string userid)
    {
        Console.WriteLine(id + " " + userid);
        var friendshipToRemove = await _dbContext.Friendships.FirstAsync(x => x.UserId == id && x.FriendId == userid);
        var friendshiptoremoveuser = await _dbContext.Friendships.FirstAsync(x => x.UserId == userid && x.FriendId == id);
        if (friendshipToRemove != null)
        {
            _dbContext.Friendships.Remove(friendshipToRemove);
            _dbContext.Friendships.Remove(friendshiptoremoveuser);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<User>> GetUserFriend(string id)
    {
        var user = await _dbContext.Friendships.Where(x => x.UserId == id).Select(u => u.Friend).ToArrayAsync();
        return user;
    }

    public async Task<bool> IsAlreadyRequest(string id, string userto)
    {
        Console.WriteLine(id + " " + userto);
        var notifications = await _dbContext.userNotifications
           .Where(un => un.UserId == userto)
           .Select(un => un.Notification)
           .ToArrayAsync();

        var isRequest = notifications.Any(x =>
        {
            if (x.UserFrom == id && x.UserTo == userto && x.Type == "RequestId")
            {
                return true;
            }
            return false;
        });
        Console.WriteLine(isRequest);
        return isRequest;
    }

    public async Task RequestToAdd(string username, string id,string toid)
    {
        await _notificationService.AddNotification(new Notification()
        {
            Title = $"User {username} want add you to friend!",
            Description = $"User {username} wants to add you as a friend, click on the button below to accept the request",
            UserTo = toid,
            UserFrom = id,
            Type = "RequestId"
        });
        var notification = await _dbContext.notifications
        .OrderByDescending(n => n.Id)
                            .FirstOrDefaultAsync();
        await _notificationService.AddNotificationToUser(toid, notification.Id);

    }
}
