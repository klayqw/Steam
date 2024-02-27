using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;

namespace Steam.Services.Base;

public interface IGameServiceBase
{
    public Task<IActionResult> Add(GameDto gameAddDTO);
    public Task<Game> GetById(int id);
    public Task<IEnumerable<Game>> GetAll();
    public Task<IActionResult> Update(int id,GameDto gameAddDTO);
    public Task Delete(int id);
    public Task<IActionResult> Buy(string id, int gameid);
    public Task<IActionResult> DeleteFromLibary(int id,string userid);
}
