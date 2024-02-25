using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;

namespace Steam.Services.Base;

public interface IWorkShopServiceBase
{
    public Task<IEnumerable<WorkShop>> GetAll();
    public Task<WorkShop> GetById(int id);
    public Task<IActionResult> Delete(int id,HttpContext creator);
    public Task<IActionResult> Add(WorkShopDto workShopDto, string creator);
    public Task<IActionResult> Update(WorkShopDto workShopDto, int id,HttpContext context);
    public Task<IEnumerable<WorkShop>> GetUserWorkShop(string creator);
    public Task<IActionResult> AddToSub(string id, int workshopid);
    public Task<IEnumerable<WorkShop>> ShowSub(string id);
    public Task<IActionResult> UnFollow(int id, string userid);
}
