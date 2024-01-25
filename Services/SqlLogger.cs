using System.Data.SqlClient;
using Dapper;
using Turbo.az.Models;
using Turbo.az.Services.Base;

namespace Turbo.az.Services;
public class SqlLogger : ICustomLogger
{
    private readonly string connectionString;
    public SqlLogger(string connectionString) => this.connectionString = connectionString;
    public async Task Log(Log log)
    {
        using var connection = new SqlConnection(connectionString);
        var logs = await connection.ExecuteAsync(
            sql: "insert into Logs (UserId, Url, MethodType, StatusCode, RequestBody, ResponseBody) values (@UserId, @Url, @MethodType, @StatusCode, @RequestBody, @ResponseBody);",
            param: log);
    }
}