using Turboaz.Infrastructure.Data;
using Turboaz.Core.Models;
using Turboaz.Core.Repositories;

namespace Turboaz.Infrastructure.Repositories;

public class LogSqlRepository : ILogRepository
{
    private readonly MyDbContext dbContext;
    
    public LogSqlRepository(MyDbContext dbContext) => this.dbContext = dbContext;

    public async Task AddLogAsync(Log log)
    {
        await this.dbContext.Logs.AddAsync(log);

        await this.dbContext.SaveChangesAsync();
    }
}