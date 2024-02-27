using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;

namespace Steam.Services.Base;

public interface IGroupServices
{
    public Task<IEnumerable<Group>> GetAll();
    public Task<Group> GetById(int id);
    public Task<IActionResult> Add(GroupDto dto, string creator);
    public Task<IEnumerable<Group>> GetUserGroup(string creator);
    public Task<IActionResult> JoinInGroup(int id, string userid);
    public Task<IEnumerable<Group>> ShowJoinedGroup(string id);
    public Task<IActionResult> Leave(int id,string userid);
    public Task<IActionResult> Delete(int id, HttpContext context);
    public Task<IActionResult> Update(GroupDto dto,int id, HttpContext context);
    public Task<IEnumerable<User>> GetUsersInGroup(int id);
}
