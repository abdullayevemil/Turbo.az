using Microsoft.EntityFrameworkCore;
using Turboaz.Core.CustomExceptions;
using Turboaz.Infrastructure.Data;
using Turboaz.Core.Models;
using Turboaz.Core.Repositories;

namespace Turboaz.Infrastructure.Repositories;

public class UserSqlRepository : IUserRepository
{
    private readonly MyDbContext dbContext;

    public UserSqlRepository(MyDbContext dbContext) => this.dbContext = dbContext;

    public IEnumerable<User> GetAllUsers()
    {
        var users = this.dbContext.Users.Where(user => user.UserName != "admin").AsEnumerable();

        return users;
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        var user = await this.dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);

        return user;
    }

    public async Task BanUserAsync(string id)
    {
        var user = await this.dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

        user!.IsBanned = !user!.IsBanned;

        await this.dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await this.dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

        this.dbContext.Remove<User>(user!);

        await this.dbContext.SaveChangesAsync();
    }
}