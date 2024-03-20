using FluentValidation;
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
        if (gametodelete == null)
        {
            throw new NullReferenceException($"game was not found under id {id}");
        }
        _dbContext.Games.Remove(gametodelete);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Add(GameDto dto)
    {
        var game = new Game()
        {
            GameImageUrl = dto.GameImageUrl,
            GamePreviewUrl = dto.GamePreviewUrl,
            Title = dto.Title,
            Description = dto.Description,
            Devoloper = dto.Devoloper,
            Publisher = dto.Publisher,
            Price =dto.Price,
            ReleaseDate = dto.ReleaseDate,
            Genre = dto.Genre,
        };      
        await _dbContext.Games.AddAsync(game);
        await _dbContext.SaveChangesAsync();

    }

    public async Task<Game> GetById(int id)
    {
        if(id <= 0)
        {
            throw new ArgumentOutOfRangeException("id");
        }
        var result = await _dbContext.Games.FirstOrDefaultAsync(e => e.Id == id);
        if (result == null)
        {
            throw new NullReferenceException($"game was not found under id {id}");
        }
        return result;
    }

    public async Task<IEnumerable<Game>> GetAll()
    {
        var result = await _dbContext.Games.ToArrayAsync();
        if (result == null)
        {
            throw new NullReferenceException($"problems with database");
        }
        return result;
    }

    public async Task Update(int id, GameDto dto)
    {
        if(id <= 0)
        {
            throw new ArgumentOutOfRangeException("id");
        }
        Console.WriteLine(dto.Price);
        var gameToUpdate = await _dbContext.Games.FindAsync(id);
        if (gameToUpdate == null)
        {
            throw new NullReferenceException($"game was not found under id {id}");
        }
        gameToUpdate.GameImageUrl = dto.GameImageUrl;
        gameToUpdate.GamePreviewUrl = dto.GamePreviewUrl;
        gameToUpdate.Title = dto.Title;
        gameToUpdate.Description = dto.Description;
        gameToUpdate.Devoloper = dto.Devoloper;
        gameToUpdate.Publisher = dto.Publisher;
        gameToUpdate.Price = dto.Price;
        gameToUpdate.ReleaseDate = dto.ReleaseDate;
        gameToUpdate.Genre = dto.Genre;
        _dbContext.Update(gameToUpdate);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Buy(string id, int gameid)
    {
        await _dbContext.userGames.AddAsync(new UserGames()
        {
            UserId = id,
            GameId = gameid,
        });
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteFromLibary(int id,string userid)
    {
        if(id <= 0)
        {
            throw new ArgumentOutOfRangeException("id");
        }
        var userGame = await _dbContext.userGames.FirstOrDefaultAsync(ug => ug.UserId == userid && ug.GameId == id);
        if(userGame == null)
        {
            throw new NullReferenceException("Game or user not found");
        }
        _dbContext.Remove(userGame);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Comment>> GetComments(int gameid)
    {
        var comments = await _dbContext.Comment.Include(x => x.User).Where(y => y.GameId == gameid).ToArrayAsync();
        return comments;    
    }

    public async Task AddComment(CommentDto commentDTO)
    {
        await _dbContext.Comment.AddAsync(new Comment()
        {
            UserId = commentDTO.UserId,
            GameId = commentDTO.GameId,
            Text = commentDTO.Comment,
        });
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Game>> Search(string game)
    {
        if(game == string.Empty)
        {
            return await _dbContext.Games.ToArrayAsync();
        }
        var result = await _dbContext.Games.Where(x => x.Title.Contains(game)).ToArrayAsync();
        return result;
    }
}

