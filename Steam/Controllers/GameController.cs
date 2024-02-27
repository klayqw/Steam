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
    private readonly IValidator<GameDto> _validator;
    public GameController(SteamDBContext dBContext, IGameServiceBase gameservices,IUserServiceBase userServiceBase, IValidator<GameDto> _validator)
    {
        this.gameService = gameservices;
        this.userService = userServiceBase;
        this._validator = _validator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await gameService.GetAll();
        return View(result);
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var usergames = await userService.GetUserGames(userid);
        var result = await gameService.GetById(id);
        return View(new BuyViewModel()
        {
            game = result,
            UserGames = usergames,
        });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id)
    {
        var result = await gameService.GetById(id);
        return View(result);    
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody]GameDto dto)
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
            return View("Update");
        }
        await gameService.Update(id, dto);
        return RedirectToAction("GetAll");
      
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromQuery]int id)
    {
        Console.WriteLine(id);
        await gameService.Delete(id);
        return RedirectToAction("GetAll");
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
        var result = _validator.Validate(dto);
        if(result.IsValid == false)
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
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Buy(int id)
    {
        Console.WriteLine(id);
        var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await gameService.Buy(userid, id);
        return RedirectToAction(controllerName: "User", actionName: "Profile");
    }

}
