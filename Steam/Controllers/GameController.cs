using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public IActionResult AddGame()
    {
        return View();
    }
    [Authorize]
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
    [Authorize]
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
    [Authorize]
    public IActionResult AddGame(GameDto gamedto)
    {
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
