using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Services.Base;
using System.Security.Claims;

namespace Steam.Controllers;

public class UserGamesController : Controller
{
    private readonly IUserRepositoryBase _userRepository;
    private readonly IUserGamesRepository _userGamesRepository;

    public UserGamesController(IUserRepositoryBase _userRepository, IUserGamesRepository _userGamesRepositor)
    {
        this._userRepository = _userRepository;
        this._userGamesRepository = _userGamesRepositor;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToLibary(int gameId)
    {
        try
        {
            var user = await _userRepository.FindByLoginAsync(base.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);
            await _userGamesRepository.AddGameToUser(user.Id,gameId);
        }catch(Exception ex)
        {
            return RedirectToAction("Error", "Error",new { message = ex.Message });
        }
        return RedirectToAction(actionName: "Profile",controllerName:"User");
    }
}
