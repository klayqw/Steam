using StackOverFlow.Models;

namespace StackOverFlow.Services.Base;

public interface IForumRepository
{
    public Task AddAsync(Forum forum);
}
