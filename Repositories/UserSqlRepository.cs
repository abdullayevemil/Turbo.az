using Microsoft.EntityFrameworkCore;
using Turbo.az.Data;
using Turbo.az.Dtos;
using Turbo.az.Models;
using Turbo.az.Repositories.Base;

namespace Turbo.az.Repositories;

public class UserSqlRepository : IUserRepository
{
    private readonly MyDbContext dbContext;

    public UserSqlRepository(MyDbContext dbContext) => this.dbContext = dbContext;

    public async Task<User?> GetUserByLoginAndPassword(LoginDto loginDto)
    {
        var user = await this.dbContext.Users.FirstOrDefaultAsync(user => user.Login == loginDto.Login && user.Password == loginDto.Password);

        return user;
    }

    public async Task InsertUserAsync(User user)
    {
        await this.dbContext.Users.AddAsync(user);

        await this.dbContext.SaveChangesAsync();
    }
}