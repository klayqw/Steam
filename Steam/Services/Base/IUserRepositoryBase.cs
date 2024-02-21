using Steam.Models;

namespace Steam.Services.Base;

public interface IUserRepositoryBase
{
    public Task AddAsync(User user);
    public Task<User> GetAsync(int id);
    public Task<User> FindAsync(string login,string password);
    public Task<User> FindByLoginAsync(string login);
}
