using Microsoft.Extensions.Configuration;
using Turboaz.Core.Models;
using Turboaz.Core.Repositories;
using Turboaz.Core.Services;

namespace Turboaz.Infrastructure.Services;

public class SqlLogger : ICustomLogger
{
    private bool isLoggingEnabled;
    private readonly IConfiguration configuration;
    private readonly ILogRepository logRepository;

    public SqlLogger(IConfiguration configuration, ILogRepository logRepository)
    {
        this.configuration = configuration;
        
        this.logRepository = logRepository;

        SetIsLoggerEnabled();
    }

    public async Task Log(Log log) => await logRepository.AddLogAsync(log);

    public bool IsLoggingEnabled() => isLoggingEnabled;

    private void SetIsLoggerEnabled() => isLoggingEnabled = configuration.GetSection("isCustomLoggingEnabled").Value == "true";
}