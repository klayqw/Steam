using Microsoft.AspNetCore.Mvc;
using StackOverFlow.Dto;
using StackOverFlow.Services.Base;
using StackOverFlow.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Hosting.Server;

namespace StackOverFlow.Controllers;

public class ForumController : Controller
{
    private readonly IForumRepository forumRepository;
    public ForumController(IForumRepository forumRepository)
    {
        this.forumRepository = forumRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(ForumDto dto)
    {
        try
        {
            forumRepository.AddAsync(new Forum()
            {
                Title = dto.Title,
                Description = dto.Description,
                Like = 0,
                Dislike = 0,
            });
        }catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Created(base.Request.GetDisplayUrl(),"Created!");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        List<Forum> list;
        try
        {
            var result = await forumRepository.GetAll();
            list = result.ToList();
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }

        return View(list);
    }
}
