using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Turbo.az.Data;
using Turbo.az.Middlewares;
using Turbo.az.Models;
using Turbo.az.Repositories;
using Turbo.az.Repositories.Base;
using Turbo.az.Services;
using Turbo.az.Services.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization();

builder.Services.AddTransient<LogMiddleware>();

builder.Services.AddScoped<IVehicleRepository, VehicleSqlRepository>();

builder.Services.AddScoped<ILogRepository, LogSqlRepository>();

builder.Services.AddScoped<IUserRepository, UserSqlRepository>();

builder.Services.AddScoped<ICustomLogger, SqlLogger>();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddDbContext<MyDbContext>(dbContextOptionsBuilder =>
{
    var connectionString = builder.Configuration.GetConnectionString("TurboazDb");
    dbContextOptionsBuilder.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
})
    .AddEntityFrameworkStores<MyDbContext>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<LogMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Main}/{id?}");

app.Run();