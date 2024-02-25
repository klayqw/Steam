using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Dto;
using Steam.Models;
using Steam.Models.ManyTable;
using Steam.Services.Base;

namespace Steam.Services;

public class GroupService : IGroupServices
{
    private readonly SteamDBContext _dbContext;
    public GroupService(SteamDBContext _dbContext)
    {
        this._dbContext = _dbContext;
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
        if(todelete.Creator == context.User.Identity.Name || context.User.IsInRole("Admin"))
        {
            _dbContext.Remove(todelete);
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }
        return new BadRequestResult();
    }

    public async Task<IEnumerable<Group>> GetAll()
    {
        var result = await _dbContext.Groups.ToArrayAsync();
        return result;
    }

    public async Task<Group> GetById(int id)
    {
        var result = await _dbContext.Groups.FindAsync(id);
        return result;
    }

    public async Task<IEnumerable<Group>> GetUserGroup(string creator)
    {
        var result = await _dbContext.Groups.Where(x => x.Creator == creator).ToArrayAsync();
        return result;
    }

    public async Task<IActionResult> JoinInGroup(int id, string userid)
    {
        if (_dbContext.userGroups.Any(uws => uws.UserId == userid && uws.GroupId == id))
        {
            return new BadRequestResult();
        }
        var toedit = await _dbContext.Groups.FindAsync(id);
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
        if(toedit.Creator == context.User.Identity.Name || context.User.IsInRole("Admin"))
        {
            toedit.Name = dto.Name;
            toedit.Description = dto.Description;
            toedit.GroupImageUrl = dto.GroupImageUrl;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }
        return new BadRequestResult();
    }
}
