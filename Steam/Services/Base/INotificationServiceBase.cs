using Microsoft.AspNetCore.Mvc;
using Steam.Models;

namespace Steam.Services.Base;

public interface INotificationServiceBase
{
    public Task<IActionResult> DeleteNotification(int notificationid);
    public Task<IActionResult> AddNotification(Notification notification);
    public Task<IEnumerable<Notification>> GetAllNotificationUser(string id);
    public Task<IActionResult> AddNotificationToUser(string id, int notificationid);
    public Task<IActionResult> RemoveNotificationFromUser(string id, int notificationid);

}
