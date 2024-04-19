using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Turboaz.Core.Models;

namespace Turboaz.Infrastructure.Data;

public class MyDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Log> Logs { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
}