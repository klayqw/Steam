using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;

namespace Steam.Services.Base;

public interface IWorkShopServiceBase
{
    public Task<IEnumerable<WorkShop>> GetAll();
    public Task<WorkShop> GetById(int id);
    public Task Delete(int id,HttpContext creator);
    public Task Add(WorkShopDto workShopDto, string creator);
    public Task Update(WorkShopDto workShopDto, int id,HttpContext context);
    public Task<IEnumerable<WorkShop>> GetUserWorkShop(string creator);
    public Task AddToSub(string id, int workshopid);
    public Task<IEnumerable<WorkShop>> ShowSub(string id);
    public Task UnFollow(int id, string userid);
}
