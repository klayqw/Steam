using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Services.Base;
using System.Security.Claims;

namespace Steam.Controllers;

public class WorkShopController : Controller
{
    private readonly IWorkShopServiceBase _workShopService;
    private readonly IValidator<WorkShopDto> _workShopValidator;
    public WorkShopController(IWorkShopServiceBase workShopService, IValidator<WorkShopDto> _workShopValidator)
    {
        _workShopService = workShopService;
        this._workShopValidator = _workShopValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _workShopService.GetAll();
        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _workShopService.GetById(id);
        return View(result);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Add()
    {
        return View(new WorkShopDto());
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(WorkShopDto workShopDto)
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

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        Console.WriteLine(id);
        await _workShopService.Delete(id, HttpContext);
        return RedirectToAction("GetAll");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Update(int id)
    {
        Console.WriteLine(id);
        var result = await _workShopService.GetById(id);
        return View(result);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody]WorkShopDto dto,int id)
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
        await _workShopService.Update(dto, id,HttpContext);
        return RedirectToAction("GetAll");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserWorkShop()
    {
        var result = await _workShopService.GetUserWorkShop(HttpContext.User.Identity.Name);
        return View(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ShowSub()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _workShopService.ShowSub(userId);
        return View(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToSub(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _workShopService.AddToSub(userId, id);
        return RedirectToAction("ShowSub");
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> UnFollow(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _workShopService.UnFollow(id, userId);
        return RedirectToAction("ShowSub");
    }
   
}
