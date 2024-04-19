using Turbo.az.Models;

namespace Turbo.az.Repositories.Base;

public interface ILogRepository
{
    Task AddLogAsync(Log log);
}