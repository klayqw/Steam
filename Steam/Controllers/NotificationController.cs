using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steam.Models;
using Steam.Services;
using Steam.Services.Base;
using System.Security.Claims;

namespace Steam.Controllers;

[Authorize]
public class NotificationController : Controller
{
    private readonly INotificationServiceBase _notificationService;
    private readonly IFriendService _friendService;
    public NotificationController(INotificationServiceBase _notificationService, IFriendService friendService)
    {
        this._notificationService = _notificationService;
        _friendService = friendService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotification()
    {
        var result = await _notificationService.GetAllNotificationUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return View(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await _notificationService.RemoveNotificationFromUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, id);
        return RedirectToAction("GetAllNotification");
    }

    [HttpPost]
    public async Task<IActionResult> Accept(int id)
    {
        var notification = await _notificationService.GetById(id);
        await _friendService.Accept(notification.UserFrom, notification.UserTo);
        return RedirectToAction("Delete", new { id });
    }
}
