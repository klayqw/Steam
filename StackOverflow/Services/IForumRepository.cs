using StackOverflow.Models;

namespace StackOverflow.Services;

public interface IForumRepository 
{
    public Task AddAsync(Forum forum) ;
    public Task DeleteAsync(int id) ;
    public Task UpdateAsync(Forum forum, int id);
    public Task<Forum> GetAsync(int id);
    public Task<List<Forum>> GetAllAsync();
   
}
