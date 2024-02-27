using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;

namespace Steam.Services.Base;

public interface IUserServiceBase
{
    public Task<IEnumerable<Game>> GetUserGames(string id);
    public Task<IEnumerable<Group>> GetUserGroups(string id);
    public Task<IEnumerable<User>> GetAllUser();
    public Task<User> GetUser(string id);
    public Task<IActionResult> Update(UpdateDto dto,User user);
    public Task<IEnumerable<User>> Search(string username);
}
