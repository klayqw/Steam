using Microsoft.AspNetCore.Mvc;
using Steam.Data;

namespace Steam.Controllers;

public class GameController : Controller
{
    private readonly SteamDBContext dBContext;

    public GameController(SteamDBContext dBContext)
    {
        this.dBContext = dBContext; 
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = dBContext.Games.ToList();
    }

}
