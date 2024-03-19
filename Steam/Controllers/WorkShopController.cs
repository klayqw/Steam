using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Services.Base;
using Steam.ViewModel;
using System.Security.Claims;

namespace Steam.Controllers;

public class WorkShopController : Controller
{
    private readonly IWorkShopServiceBase _workShopService;
    private readonly IGameServiceBase _gameService;
    private readonly IValidator<WorkShopDto> _workShopValidator;
    public WorkShopController(IWorkShopServiceBase workShopService, IValidator<WorkShopDto> _workShopValidator,IGameServiceBase gameServiceBase)
    {
        _workShopService = workShopService;
        this._workShopValidator = _workShopValidator;
        this._gameService = gameServiceBase; 
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _workShopService.GetAll();
            return View(result);
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _workShopService.GetById(id);
            var userworkshopitem = await _workShopService.ShowSub(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return View(new WorkShopViewModel()
            {
                item = result,
                workShopUserItems = userworkshopitem,
            });
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Add()
    {
        var games = await _gameService.GetAll();
        return View(new WorkShopToAddViewModel()
        {
            Games = games,
            WorkShopDto = new WorkShopDto()
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(WorkShopDto workShopDto)
    {
        try
        {
            var result = _workShopValidator.Validate(workShopDto);
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
            await _workShopService.Add(workShopDto, base.HttpContext.User.Identity.Name);
            return RedirectToAction("GetAll");
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            Console.WriteLine(id);
            await _workShopService.Delete(id, HttpContext);
            return RedirectToAction("GetAll");
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Update(int id)
    {
        try
        {
            Console.WriteLine(id);
            var result = await _workShopService.GetById(id);
            return View(result);
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] WorkShopDto dto, int id)
    {
        try
        {
            var result = _workShopValidator.Validate(dto);
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
            await _workShopService.Update(dto, id, HttpContext);
            return RedirectToAction("GetAll");
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserWorkShop()
    {
        try
        {
            var result = await _workShopService.GetUserWorkShop(HttpContext.User.Identity.Name);
            return View(result);
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ShowSub()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _workShopService.ShowSub(userId);
            return View(result);
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToSub(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _workShopService.AddToSub(userId, id);
            return RedirectToAction("ShowSub");
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> UnFollow(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _workShopService.UnFollow(id, userId);
            return RedirectToAction("ShowSub");
        }
        catch (Exception ex)
        {
            return RedirectToAction("Error", "ErrorPage", new { message = ex.Message });
        }
    }

}
