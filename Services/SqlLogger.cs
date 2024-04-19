using Turbo.az.Models;
using Turbo.az.Repositories.Base;
using Turbo.az.Services.Base;

namespace Turbo.az.Services;

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

    private void SetIsLoggerEnabled() => isLoggingEnabled = configuration.GetSection("isCustomLoggingEnabled").Get<bool>();
}