using System.Data.SqlClient;
using Dapper;
using Turbo.az.Models;
using Turbo.az.Services.Base;

namespace Turbo.az.Services;
public class SqlLogger : ICustomLogger
{
    private readonly string connectionString;
    private readonly bool isLoggingEnabled;
    public SqlLogger(string connectionString, bool isLoggingEnabled)
    {
        this.connectionString = connectionString;

        this.isLoggingEnabled = isLoggingEnabled;
    }

    public async Task Log(Log log)
    {
        using var connection = new SqlConnection(connectionString);

        var logs = await connection.ExecuteAsync(
            sql: "insert into Logs (UserId, Url, MethodType, StatusCode, RequestBody, ResponseBody) values (@UserId, @Url, @MethodType, @StatusCode, @RequestBody, @ResponseBody);",
            param: log);
    }

    public bool IsLoggingEnabled() => isLoggingEnabled;
}