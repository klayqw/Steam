using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Models;
using Steam.Models.ManyTable;
using Steam.Services.Base;
using System;

namespace Steam.Services;

public class NotificationService : INotificationServiceBase
{
    private readonly SteamDBContext _dbContext;
    private readonly UserManager<IdentityUser> userManager;
    public NotificationService(SteamDBContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        this.userManager = userManager;
    }

    public async Task<IActionResult> AddNotification(Notification notification)
    {
        await _dbContext.notifications.AddAsync(notification);
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<IActionResult> DeleteNotification(int notificationid)
    {
        var todelete = await _dbContext.notifications.FindAsync(notificationid);
        _dbContext.notifications.Remove(todelete);
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }
    public async Task<IActionResult> AddNotificationToUser(string id, int notificationid)
    {
        var userNotification = new UserNotifications
        {
            UserId = id,
            NotificationId = notificationid
        };

        await _dbContext.userNotifications.AddAsync(userNotification);
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<IEnumerable<Notification>> GetAllNotificationUser(string id)
    {
        var notifications = await _dbContext.userNotifications
           .Where(un => un.UserId == id)
           .Select(un => un.Notification)
           .ToArrayAsync();
        return notifications;
    }

    public async Task<IActionResult> RemoveNotificationFromUser(string id, int notificationid)
    {
        var userNotification = await _dbContext.userNotifications
           .FirstOrDefaultAsync(un => un.UserId == id && un.NotificationId == notificationid);
        _dbContext.userNotifications.Remove(userNotification);
        await DeleteNotification(notificationid);
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }

}
