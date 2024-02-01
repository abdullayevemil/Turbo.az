using System.Data.SqlClient;
using Dapper;
using Turbo.az.Dtos;
using Turbo.az.Models;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Repositories;
public class UserSqlRepository : IUserRepository
{
    private readonly string connectionString;

    public UserSqlRepository(string connectionString) => this.connectionString = connectionString;
    
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        using var connection = new SqlConnection(connectionString);

        var users = await connection.QueryAsync<User>("select * from Users");
        
        return users;
    }

    public async Task<User?> GetUserByLoginAndPassword(LoginDto loginDto)
    {
        using var connection = new SqlConnection(connectionString);

        var user = await connection.QueryFirstOrDefaultAsync<User>(
            sql: @"select *
                from Users
                where Login = @login and Password = @password",
            param: new
            {
                login = loginDto.Login,
                password = loginDto.Password,
            }
        );

        return user;
    }

    public async Task InsertUserAsync(User user)
    {
        using var connection = new SqlConnection(connectionString);
        
        var users = await connection.ExecuteAsync(
            sql: "insert into Users (Email, Login, Password) values (@Email, @Login, @Password);",
            param: user);
    }
}