using Steam.Models;

namespace Steam.Services.Base;

public interface ILogRepository
{
    public Task Add(Log log);
}
