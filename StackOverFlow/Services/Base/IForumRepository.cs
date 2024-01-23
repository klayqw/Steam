using StackOverFlow.Models;
using System.Collections;

namespace StackOverFlow.Services.Base;

public interface IForumRepository
{
    public Task AddAsync(Forum forum);
    public Task<IEnumerable<Forum>> GetAll();
}
