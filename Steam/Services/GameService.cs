using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Steam.Data;
using Steam.Dto;

namespace Steam.Services;

public class GameService
{   
    private readonly SteamDBContext _dbContext;
    public GameService(SteamDBContext _dbContext)
    {
        this._dbContext = _dbContext;
    }

    public async Task<IActionResult> Add(GameAddDTO dto)
    {
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

        return new OkResult();

    }
}

