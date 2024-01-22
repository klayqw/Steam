using Microsoft.AspNetCore.Mvc;
using StackOverflow.Dto;
using StackOverflow.Services;
using StackOverflow.Models;

namespace StackOverflow.Controllers;

public class ForumController : Controller
{
    private readonly ILogger<ForumController> _logger;
    private readonly IForumRepository sqlstorage;

    public ForumController(ILogger<ForumController> logger, IForumRepository forumSqlBase)
    {
        _logger = logger;
        sqlstorage = forumSqlBase;
    }

    [ActionName("Get")]
    public async Task<IActionResult> GetForumAsync(int id)
    {
        var result = await sqlstorage.GetAsync(id);
        return View(result);
    }

    [ActionName("GetAll")]
    public async Task<IActionResult> GetAllForumsAsync()
    {
        var result = await sqlstorage.GetAllAsync();
        Console.WriteLine(result.Count);
        return View(result);
    }

    [ActionName("Add")]
    [HttpPost]
    public async Task<IActionResult> PostForumAsync([FromForm]ForumDto Dto)
    {
        
        await sqlstorage.AddAsync(new Forum()
        {
            Title = Dto.Title,
            Description = Dto.Description,
            Like = Dto.Like,
            Dislike = Dto.Dislike,
        });
        return Redirect("/");
    }

    [ActionName("Add")]
    [HttpGet]
    public async Task<IActionResult> PostForumAsync()
    {
        return View();
    }
    [HttpPut,HttpPost]
    [ActionName("Update")]
    public async Task<IActionResult> UpdateForumAsync([FromForm]ForumDto Dto)
    {
        Console.WriteLine(Dto.Id);
        await sqlstorage.UpdateAsync(new Forum()
        {
            Title = Dto.Title,
            Description = Dto.Description,
            Like = Dto.Like,
            Dislike = Dto.Dislike,
        },Dto.Id);
        return Redirect("/");
    }

    [HttpGet]
    [ActionName("Update")]
    public async Task<IActionResult> UpdateForumAsync()
    {
        return View();
    }

    [ActionName("GetById")]
    public IActionResult GetById()
    {
        return View();
    }
    [HttpDelete,HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteForumAsync(int id)
    {
        await sqlstorage.DeleteAsync(id);
        return Redirect("/");
    }

    [HttpGet]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteForumAsync()
    {
        
        return View();
    }

}
