using Turboaz.Core.Models;

namespace Turboaz.Core.Services;

public interface ICustomLogger
{
    Task Log(Log log);
    bool IsLoggingEnabled();
}