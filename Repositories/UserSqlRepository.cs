using Microsoft.AspNetCore.Identity;
using Turbo.az.Data;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Repositories;

public class UserSqlRepository : IUserRepository
{
    private readonly MyDbContext dbContext;

    public UserSqlRepository(MyDbContext dbContext) => this.dbContext = dbContext;

    public IEnumerable<IdentityUser> GetAllUsers() => this.dbContext.Users.AsEnumerable();
}