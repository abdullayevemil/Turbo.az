using Turbo.az.Data;
using Turbo.az.Models;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Repositories;

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