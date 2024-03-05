using Microsoft.AspNetCore.Mvc;
using Steam.Dto;
using Steam.Models;
using Steam.ViewModel;

namespace Steam.Services.Base;

public interface IUserServiceBase
{
    public Task<IEnumerable<Game>> GetUserGames(string id);
    public Task<IEnumerable<Group>> GetUserGroups(string id);
    public Task<IEnumerable<User>> GetAllUser();
    public Task<User> GetUser(string id);
    public Task Update(UpdateDto dto,User user);
    public Task<IEnumerable<User>> Search(string username);

}
