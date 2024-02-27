using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
}