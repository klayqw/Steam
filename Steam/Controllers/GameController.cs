using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Dto;

namespace Steam.Controllers;

public class GameController : Controller
{
    private readonly SteamDBContext dBContext;

    public GameController(SteamDBContext dBContext)
    {
        this.dBContext = dBContext; 
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await dBContext.Games.ToArrayAsync();
        return View(result);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(GameAddDTO dto)
    {

    }

}
