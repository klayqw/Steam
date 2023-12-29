using Microsoft.AspNetCore.Mvc;
using StackOverflow.Services;

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


    public IActionResult GetForum(int id)
    {

        return View();
    }

    public IActionResult GetAllForums()
    {
        return View();
    }



}
