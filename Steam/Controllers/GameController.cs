using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;
using Steam.Services.Base;
using System.Net;

namespace Steam.Controllers;

public class GameController : Controller
{
    private readonly IGameRepository _gameRepository;
    public GameController(IGameRepository gameRepository)
    {
        this._gameRepository = gameRepository;
    }

    [HttpGet]
    public IActionResult AddGame()
    {
        if (base.HttpContext.Request.Cookies["Authorize"] is null)
        {
            return base.Unauthorized();
        }
        return View();
    }

    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Game> games;
        try
        {
            games = await _gameRepository.GetAll();   
        }
        catch (Exception ex)
        {
            return StatusCode(500,ex.Message);
        }
        return View(games);
    }

    public async Task<IActionResult> GetById(int id)
    {
        var result = new Game();
        try
        {
            result = await _gameRepository.GetById(id);
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return View(result);
    }

    [HttpPost]
    public IActionResult AddGame(GameDto gamedto)
    {
        if (base.HttpContext.Request.Cookies["Authorize"] is null)
        {
            return base.Unauthorized();
        }
        try
        {
            _gameRepository.Post(new Game()
            {
                Title = gamedto.Title,
                Description = gamedto.Description,
                Devoloper = gamedto.Devoloper,
                Publisher = gamedto.Publisher,
                Price = gamedto.Price,
                ReleaseDate = gamedto.ReleaseDate,
                Genre = gamedto.Genre,
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return RedirectToAction("GetAll");
    }

}
