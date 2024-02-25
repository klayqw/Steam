using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Dto;
using Steam.Models;
using Steam.Models.ManyTable;
using Steam.Services.Base;

namespace Steam.Services;

public class GameService : IGameServiceBase
{   
    private readonly SteamDBContext _dbContext;
    public GameService(SteamDBContext _dbContext)
    {
        this._dbContext = _dbContext;
    }

    public async Task Delete(int id)
    {
        var gametodelete = await _dbContext.Games.FirstOrDefaultAsync(x => x.Id == id);
        _dbContext.Games.Remove(gametodelete);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IActionResult> Add(GameDto dto)
    {
        Console.WriteLine(dto.Title);
        await _dbContext.Games.AddAsync(new Models.Game()
        {
            GameImageUrl = dto.GameImageUrl,
            Title = dto.Title,
            Description = dto.Description,
            Devoloper = dto.Devoloper,
            Publisher = dto.Publisher,
            Price = dto.Price,
            ReleaseDate = dto.ReleaseDate,
            Genre = dto.Genre,
        });
        await _dbContext.SaveChangesAsync();
        return new OkResult();

    }

    public async Task<Game> GetById(int id)
    {
        var result = await _dbContext.Games.FirstOrDefaultAsync(e => e.Id == id);
        return result;
    }

    public async Task<IEnumerable<Game>> GetAll()
    {
        var result = await _dbContext.Games.ToArrayAsync();
        return result;
    }

    public async Task<IActionResult> Update(int id, GameDto dto)
    {
        var gameToUpdate = await _dbContext.Games.FindAsync(id);
        gameToUpdate.Title = dto.Title;
        gameToUpdate.Description = dto.Description;
        gameToUpdate.Devoloper = dto.Devoloper;
        gameToUpdate.Publisher = dto.Publisher;
        gameToUpdate.Price = dto.Price;
        gameToUpdate.ReleaseDate = dto.ReleaseDate;
        gameToUpdate.Genre = dto.Genre;

        _dbContext.Update(gameToUpdate);

        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }
}

