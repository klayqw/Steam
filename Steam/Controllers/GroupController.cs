using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Services.Base;
using System.Security.Claims;

namespace Steam.Controllers;

public class GroupController : Controller
{
    private readonly IGroupServices groupService;
    private readonly IValidator<GroupDto> validator;
    public GroupController(IGroupServices groupService, IValidator<GroupDto> validator)
    {
        this.groupService = groupService;
        this.validator = validator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await groupService.GetAll();   
        return View(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await groupService.GetById(id);
        return View(result);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(GroupDto dto)
    {
        var result = validator.Validate(dto);
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
        await groupService.Add(dto,User.Identity.Name);
        return RedirectToAction("GetAll");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserGroup()
    {
        var result = await groupService.GetUserGroup(User.Identity.Name);
        return View(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> JoinIn(int id)
    {
        Console.WriteLine(id);
        await groupService.JoinInGroup(id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return RedirectToAction("ShowJoinedGroup");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ShowJoinedGroup()
    {
        var result = await groupService.ShowJoinedGroup(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return View(result);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Leave(int id)
    {
        await groupService.Leave(id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return RedirectToAction("ShowJoinedGroup");
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        await groupService.Delete(id, HttpContext);
        return RedirectToAction("GetAll");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Update(int id)
    {
        
        var toedit = await groupService.GetById(id);
        return View(toedit);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update(int id,[FromBody]GroupDto dto)
    {
        var result = validator.Validate(dto);
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
        await groupService.Update(dto, id, HttpContext);
        return RedirectToAction("GetAll");
    }

}
