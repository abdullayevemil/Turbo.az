using Microsoft.EntityFrameworkCore;
using Turbo.az.Models;

namespace Turbo.az.Data;

public class MyDbContext : DbContext
{
    public DbSet<Log> Logs { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<User> Users { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
}