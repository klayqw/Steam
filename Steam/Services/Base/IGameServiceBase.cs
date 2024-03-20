using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;

namespace Steam.Services.Base;

public interface IGameServiceBase
{
    public Task Add(GameDto gameAddDTO);
    public Task<Game> GetById(int id);
    public Task<IEnumerable<Game>> GetAll();
    public Task Update(int id,GameDto gameAddDTO);
    public Task Delete(int id);
    public Task Buy(string id, int gameid);
    public Task DeleteFromLibary(int id,string userid);
    public Task<IEnumerable<Comment>> GetComments(int gameid);
    public Task AddComment(CommentDto commentDTO);
    public Task<IEnumerable<Game>> Search(string game);
}
