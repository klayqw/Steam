using Microsoft.AspNetCore.Mvc;
using Steam.Models;

namespace Steam.Services.Base;

public interface IAdminPanel
{
    public Task<IEnumerable<User>> GetAllUser();
    public Task BanUserById(string id);
    public Task UnBanUserById(string id);
}
