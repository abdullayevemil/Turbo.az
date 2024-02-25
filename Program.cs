using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Turbo.az.Data;
using Turbo.az.Middlewares;
using Turbo.az.Repositories;
using Turbo.az.Repositories.Base;
using Turbo.az.Services;
using Turbo.az.Services.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDbContext>(dbContextOptionsBuilder =>
{
    var connectionString = builder.Configuration.GetConnectionString("TurboazDb");

    dbContextOptionsBuilder.UseSqlServer(connectionString);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Login";
        options.ReturnUrlParameter = "returnUrl";
    });

builder.Services.AddAuthorization();

builder.Services.AddTransient<LogMiddleware>();

builder.Services.AddScoped<IVehicleRepository, VehicleSqlRepository>();

builder.Services.AddScoped<IUserRepository, UserSqlRepository>();

builder.Services.AddScoped<ILogRepository, LogSqlRepository>();

builder.Services.AddScoped<ICustomLogger, SqlLogger>();

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