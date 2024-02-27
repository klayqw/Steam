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

public class GroupService : IGroupServices
{
    private readonly SteamDBContext _dbContext;
    private readonly INotificationServiceBase _notificationService;
    public GroupService(SteamDBContext _dbContext, INotificationServiceBase notificationService)
    {
        this._dbContext = _dbContext;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Add(GroupDto dto,string creator)
    {
        await _dbContext.Groups.AddAsync(new Group()
        {
            Name = dto.Name,
            GroupImageUrl = dto.GroupImageUrl,
            Description = dto.Description,
            Creator = creator,
            MemberCount = 0,
        });
        await _dbContext.SaveChangesAsync();
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == creator);
        var item = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Name == dto.Name);
        await JoinInGroup(item.Id, user.Id);
        return new OkResult();
    }

    public async Task<IActionResult> Delete(int id, HttpContext context)
    {
        var todelete = await _dbContext.Groups.FindAsync(id);
        if(todelete == null)
        {
            throw new NullReferenceException($"Group was not found under id {id}");
        }
        if(todelete.Creator == context.User.Identity.Name || context.User.IsInRole("Admin"))
        {
            var userto = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == todelete.Creator);
            if(userto != null)
            {
                await _notificationService.AddNotification(new Notification()
                {
                    Title = $"Delete From {context.User.Identity.Name}",
                    Description = $"Your group {todelete.Name} was delete by {context.User.Identity.Name}",
                    UserTo = userto.Id,
                    UserFrom = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Type = "DeleteGroup"
                });
                var notification = await _dbContext.notifications
                                    .OrderByDescending(n => n.Id)
                                    .FirstOrDefaultAsync();
                await _notificationService.AddNotificationToUser(userto.Id, notification.Id);
            }
            _dbContext.Remove(todelete);
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }
        return new BadRequestResult();
    }

    public async Task<IEnumerable<Group>> GetAll()
    {
        var result = await _dbContext.Groups.ToArrayAsync();
        if(result == null)
        {
            throw new NullReferenceException("Problems in database");
        }
        return result;
    }

    public async Task<Group> GetById(int id)
    {
        var result = await _dbContext.Groups.FindAsync(id);
        if(result == null)
        {
            throw new NullReferenceException($"Group wasn found by id {id}");
        }
        return result;
    }

    public async Task<IEnumerable<Group>> GetUserGroup(string creator)
    {
        var result = await _dbContext.Groups.Where(x => x.Creator == creator).ToArrayAsync();
        return result;
    }

    public async Task<IEnumerable<User>> GetUsersInGroup(int id)
    {
        var usersInGroup = await _dbContext.userGroups
                    .Where(ug => ug.GroupId == id)                  
                    .Select(ug => ug.User)
                    .ToArrayAsync();
        return usersInGroup;
    }

    public async Task<IActionResult> JoinInGroup(int id, string userid)
    {
        if (_dbContext.userGroups.Any(uws => uws.UserId == userid && uws.GroupId == id))
        {
            return new BadRequestResult();
        }
        var toedit = await _dbContext.Groups.FindAsync(id);
        if (toedit == null)
        {
            throw new NullReferenceException("Something went wrong");
        }
        toedit.MemberCount += 1;
        var toadd = new UserGroups()
        {
            UserId = userid,
            GroupId = id,
        };
        await _dbContext.userGroups.AddAsync(toadd);
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<IActionResult> Leave(int id, string userid)
    {
        var userWorkShopSubToRemove = await _dbContext.userGroups.FirstOrDefaultAsync(uw => uw.UserId == userid && uw.GroupId == id);
        var toedit = await _dbContext.Groups.FindAsync(id);
        var user = await _dbContext.Users.FindAsync(userid);
        if(toedit ==  null || user == null || userWorkShopSubToRemove == null)
        {
            throw new NullReferenceException("Gorup wasnt found or User");
        } 
        if(toedit.Creator == user.UserName)
        {
            return new BadRequestResult();
        }
        _dbContext.userGroups.Remove(userWorkShopSubToRemove);
        toedit.MemberCount -= 1;
        if(toedit.MemberCount <= 0)
        {
            _dbContext.Groups.Remove(toedit);
        }
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<IEnumerable<Group>> ShowJoinedGroup(string id)
    {
        var result = await _dbContext.userGroups
            .Include(x => x.Group)
            .Where(x => x.UserId == id)
            .Select(x => x.Group)
            .ToArrayAsync();
        return result;
    }

    

    public async Task<IActionResult> Update(GroupDto dto, int id, HttpContext context)
    {
        var toedit = await _dbContext.Groups.FindAsync(id);
        if(toedit == null)
        {
            throw new NullReferenceException("Group wasnt found in update");
        }
        if(toedit.Creator == context.User.Identity.Name || context.User.IsInRole("Admin"))
        {
            var userto = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == toedit.Creator);
            if(userto != null)
            {
                await _notificationService.AddNotification(new Notification()
                {
                    Title = $"Update From {context.User.Identity.Name}",
                    Description = $"Your group {toedit.Name} was updated by {context.User.Identity.Name}",
                    UserTo = userto.Id,
                    UserFrom = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Type = "UpdateGroup"
                });
                var notification = await _dbContext.notifications
                                    .OrderByDescending(n => n.Id)
                                    .FirstOrDefaultAsync();
                await _notificationService.AddNotificationToUser(userto.Id, notification.Id);
            }
            toedit.Name = dto.Name;
            toedit.Description = dto.Description;
            toedit.GroupImageUrl = dto.GroupImageUrl;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }
        return new BadRequestResult();
    }
}
