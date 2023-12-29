using StackOverflow.Models;

namespace StackOverflow.Services;

public interface IForumRepository 
{
    public Task AddAsync(Forum forum) ;
    public Task DeleteAsync(Forum forum) ;
    public Task UpdateAsync(Forum forum);
    public Task<Forum> GetAsync(int id);
   
}
