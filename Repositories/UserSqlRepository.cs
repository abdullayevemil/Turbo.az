using Microsoft.EntityFrameworkCore;
using Turbo.az.CustomExceptions;
using Turbo.az.Data;
using Turbo.az.Models;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Repositories;

public class UserSqlRepository : IUserRepository
{
    private readonly MyDbContext dbContext;

    public UserSqlRepository(MyDbContext dbContext) => this.dbContext = dbContext;

    public IEnumerable<User> GetAllUsers()
    {
        var users = this.dbContext.Users.Where(user => user.UserName != "admin").AsEnumerable();

        return users;
    }

    public async Task<User> GetUserByUserNameAsync(string userName)
    {
        var user = await this.dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);

        if (user is null)
        {
            throw new NotFoundException($"User with username: {userName} was not found in the database!");
        }

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