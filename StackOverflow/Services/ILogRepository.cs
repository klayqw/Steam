using StackOverflow.Models;

namespace StackOverflow.Services;

public interface ILogRepository
{
    public Task AddLog(LogInfo logInfo);
}
