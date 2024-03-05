using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Dto;
using Steam.Models;
using Steam.Services;
using Steam.Services.Base;
using Steam.ViewModel;
using System.Security.Claims;

namespace Steam.Controllers;

public class GameController : Controller
{
    private readonly IGameServiceBase gameService;
    private readonly IUserServiceBase userService;
    private readonly IFriendService friendService;
    private readonly IUserServiceBase UserService;
    private readonly IValidator<GameDto> _validator;
    public GameController(SteamDBContext dBContext, IGameServiceBase gameservices, IUserServiceBase userServiceBase, IValidator<GameDto> _validator, IFriendService friendService, IUserServiceBase userService)
    {
        this.gameService = gameservices;
        this.userService = userServiceBase;
        this._validator = _validator;
        this.friendService = friendService;
        this.userService = userService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Game> result;
        try
        {
            result = await gameService.GetAll();
        } catch (Exception ex)
        {
            return RedirectToAction(actionName: "Error", controllerName: "ErrorPage",routeValues: new { message = ex.Message });
        }
        return View(result);
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var usergames = await userService.GetUserGames(userid);
            var result = await gameService.GetById(id);
            var friends = await friendService.GetUserFriend(userid);
            return View(new BuyViewModel()
            {
                game = result,
                UserGames = usergames,
                Friends = friends,
            });
        }catch(Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
        
    }   

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id)
    {
        try
        {
            var result = await gameService.GetById(id);
            return View(result);    
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody]GameDto dto)
    {
        Console.WriteLine(dto.Title);
        try
        {
            var game = await gameService.GetById(id);
            var result = _validator.Validate(dto);
            if (result.IsValid == false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        key: error.PropertyName,
                        errorMessage: error.ErrorMessage
                    );
                }
                return View("Update", game);
            }
            await gameService.Update(id, dto);
            return RedirectToAction("GetAll");
        }catch(Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
       
      
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteFromLibary(int id)
    {
        await gameService.DeleteFromLibary(id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return RedirectToAction("Libary");
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromQuery]int id)
    {
        try
        {
            await gameService.Delete(id);
            return RedirectToAction("GetAll");
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }

    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(GameDto dto)
    {
        try
        {
            var result = _validator.Validate(dto);
            if (result.IsValid == false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        key: error.PropertyName,
                        errorMessage: error.ErrorMessage
                    );
                }
                return View("Add");
            }
            await gameService.Add(dto);
            return RedirectToAction("GetAll");
        }catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
        
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Buy(int id)
    {
        try
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await gameService.Buy(userid, id);
            return RedirectToAction(controllerName: "User", actionName: "Profile");
        }catch(Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }

    }

}
