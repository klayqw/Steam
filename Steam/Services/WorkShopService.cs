using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Dto;
using Steam.Models;
using Steam.Models.ManyTable;
using Steam.Services.Base;

namespace Steam.Services;

public class WorkShopService : IWorkShopServiceBase
{
    private readonly SteamDBContext _dbContext;
    public WorkShopService(SteamDBContext _dbContext)
    {
        this._dbContext = _dbContext;
    }

    public async Task<IActionResult> Add(WorkShopDto workShopDto,string creator)
    {
        await _dbContext.workShops.AddAsync(new WorkShop()
        {
            Title = workShopDto.Title,  
            Description = workShopDto.Description,
            Like = 0,
            Dislike = 0,
            Creator = creator,
            Subscribers = 0,
        });
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<IActionResult> AddToSub(string id, int workshopid)
    {
        if(_dbContext.userWorkShopSubs.Any(uws => uws.UserId == id && uws.WorkShopItemId == workshopid))
        {
            return new BadRequestResult();
        }
        var toedit = await _dbContext.workShops.FindAsync(workshopid);
        toedit.Subscribers += 1;
        var toadd = new UserWorkShopSub()
        {
            WorkShopItemId = workshopid,
            UserId = id,
        };
        await _dbContext.userWorkShopSubs.AddAsync(toadd);
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<IActionResult> Delete(int id,HttpContext context)
    {
        var todelete = await _dbContext.workShops.FindAsync(id);
        if(context.User.Identity.Name == todelete.Creator || context.User.IsInRole("Admin"))
        {
            _dbContext.workShops.Remove(todelete);
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }
        else
        {
            return new BadRequestResult();
        }
    }

    public async Task<IEnumerable<WorkShop>> GetAll()
    {
        var result = await _dbContext.workShops.ToArrayAsync();
        return result;
    }

    public async Task<WorkShop> GetById(int id)
    {
        var result = await _dbContext.workShops.FirstOrDefaultAsync(w => w.Id == id);
        return result;
    }

    public async Task<IEnumerable<WorkShop>> GetUserWorkShop(string creator)
    {
        var result = await _dbContext.workShops.Where(x => x.Creator == creator).ToArrayAsync();
        return result;
    }

    public async Task<IEnumerable<WorkShop>> ShowSub(string id)
    {
        var result = await _dbContext.userWorkShopSubs
            .Include(uws => uws.WorkShopItem) 
            .Where(uws => uws.UserId == id)
            .Select(uws => uws.WorkShopItem)
            .ToArrayAsync();
        return result;
    }

    public async Task<IActionResult> UnFollow(int id, string userid)
    {
        var userWorkShopSubToRemove = await _dbContext.userWorkShopSubs.FirstOrDefaultAsync(uw => uw.UserId == userid && uw.WorkShopItemId == id);
        _dbContext.userWorkShopSubs.Remove(userWorkShopSubToRemove);
        var toedit = await _dbContext.workShops.FindAsync(id);
        toedit.Subscribers -= 1;
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<IActionResult> Update(WorkShopDto workShopDto, int id, HttpContext context)
    {
        
        var gameToUpdate = await _dbContext.workShops.FindAsync(id);
        if(context.User.Identity.Name == gameToUpdate.Creator || context.User.IsInRole("Admin"))
        {
            gameToUpdate.Title = workShopDto.Title;
            gameToUpdate.Description = workShopDto.Description;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }
        else
        {
            return new BadRequestResult();
        }
    }
}
