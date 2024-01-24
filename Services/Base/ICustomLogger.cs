using Turbo.az.Models;

namespace Turbo.az.Services.Base;
public interface ICustomLogger
{
    Task Log(Log log);
}