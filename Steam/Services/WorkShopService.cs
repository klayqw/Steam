using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Dto;
using Steam.Models;
using Steam.Models.ManyTable;
using Steam.Services.Base;
using System.Security.Claims;

namespace Steam.Services;

public class WorkShopService : IWorkShopServiceBase
{
    private readonly SteamDBContext _dbContext;
    private readonly INotificationServiceBase _notificationService;
    public WorkShopService(SteamDBContext _dbContext, INotificationServiceBase notificationService)
    {
        this._dbContext = _dbContext;
        _notificationService = notificationService;
    }

    public async Task Add(WorkShopDto workShopDto,string creator)
    {
        await _dbContext.workShops.AddAsync(new WorkShop()
        {
            Title = workShopDto.Title,  
            Description = workShopDto.Description,
            Like = 0,
            Dislike = 0,
            Creator = creator,
            Subscribers = 0,
            GameId = workShopDto.GameId,
        });
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddToSub(string id, int workshopid)
    {
        if(_dbContext.userWorkShopSubs.Any(uws => uws.UserId == id && uws.WorkShopItemId == workshopid))
        {
            return;
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
    }

    public async Task Delete(int id,HttpContext context)
    {
        var todelete = await _dbContext.workShops.FindAsync(id);
        if(context.User.Identity.Name == todelete.Creator || context.User.IsInRole("Admin"))
        {
            var userto = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == todelete.Creator);
            if (userto != null)
            {
                await _notificationService.AddNotification(new Notification()
                {
                    Title = $"Delete From {context.User.Identity.Name}",
                    Description = $"Your workshop item {todelete.Title} was delete by {context.User.Identity.Name}",
                    UserTo = userto.Id,
                    UserFrom = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Type = "DeleteWorkShopItem"
                });
                var notification = await _dbContext.notifications
                                    .OrderByDescending(n => n.Id)
                                    .FirstOrDefaultAsync();
                await _notificationService.AddNotificationToUser(userto.Id, notification.Id);
            }
           
            _dbContext.workShops.Remove(todelete);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            return;
        }
    }

    public async Task<IEnumerable<WorkShop>> GetAll()
    {
        var result = await _dbContext.workShops
            .Include(ws => ws.Game)
            .ToArrayAsync();
        return result;
    }

    public async Task<WorkShop> GetById(int id)
    {
        var result = await _dbContext.workShops
            .Include(ws => ws.Game)
            .FirstOrDefaultAsync(w => w.Id == id);
        return result;
    }

    public async Task<IEnumerable<WorkShop>> GetUserWorkShop(string creator)
    {
        var result = await _dbContext.workShops.Include(ws => ws.Game).Where(x => x.Creator == creator).ToArrayAsync();
        return result;
    }

    public async Task<IEnumerable<WorkShop>> ShowSub(string id)
    {
        var result = await _dbContext.userWorkShopSubs
            .Include(uws => uws.WorkShopItem) 
            .Include(w => w.WorkShopItem.Game)
            .Where(uws => uws.UserId == id)
            .Select(uws => uws.WorkShopItem)
            .ToArrayAsync();
        return result;
    }

    public async Task UnFollow(int id, string userid)
    {
        var userWorkShopSubToRemove = await _dbContext.userWorkShopSubs.FirstOrDefaultAsync(uw => uw.UserId == userid && uw.WorkShopItemId == id);
        _dbContext.userWorkShopSubs.Remove(userWorkShopSubToRemove);
        var toedit = await _dbContext.workShops.FindAsync(id);
        toedit.Subscribers -= 1;
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(WorkShopDto workShopDto, int id, HttpContext context)
    {
        
        var gameToUpdate = await _dbContext.workShops.FindAsync(id);
        if(context.User.Identity.Name == gameToUpdate.Creator || context.User.IsInRole("Admin"))
        {
            var userto = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == gameToUpdate.Creator);
            if(userto != null)
            {
                await _notificationService.AddNotification(new Notification()
                {
                    Title = $"Update From {context.User.Identity.Name}",
                    Description = $"Your workshop item {gameToUpdate.Title} was updated by {context.User.Identity.Name}",
                    UserTo = userto.Id,
                    UserFrom = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Type = "UpdateWorkShopItem"
                });
                var notification = await _dbContext.notifications
                                    .OrderByDescending(n => n.Id)
                                    .FirstOrDefaultAsync();
                await _notificationService.AddNotificationToUser(userto.Id, notification.Id);
            }
            gameToUpdate.Title = workShopDto.Title;
            gameToUpdate.Description = workShopDto.Description;
            await _dbContext.SaveChangesAsync();
            return;
        }
        else
        {
            return;
        }
    }
}
