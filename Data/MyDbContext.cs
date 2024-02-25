using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Turbo.az.Models;

namespace Turbo.az.Data;

public class MyDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Log> Logs { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
}