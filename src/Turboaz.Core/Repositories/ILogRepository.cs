using Turboaz.Core.Models;

namespace Turboaz.Core.Repositories;

public interface ILogRepository
{
    Task AddLogAsync(Log log);
}