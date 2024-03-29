using Microsoft.AspNetCore.Authentication.Cookies;
using Turbo.az.Middlewares;
using Turbo.az.Repositories;
using Turbo.az.Repositories.Base;
using Turbo.az.Services;
using Turbo.az.Services.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Login";
        options.ReturnUrlParameter = "returnUrl";
    });

builder.Services.AddAuthorization();

builder.Services.AddTransient<LogMiddleware>();

builder.Services.AddSingleton<IVehicleRepository>(provider =>
{
    string connectionStringName = "TurboazDb";

    string? connectionString = builder.Configuration.GetConnectionString(connectionStringName);

    if (string.IsNullOrEmpty(connectionString) || string.IsNullOrWhiteSpace(connectionString))
    {
        throw new Exception($"connection string {connectionStringName} not found in setting.json");
    }

    return new VehicleSqlRepository(connectionString);
});

builder.Services.AddSingleton<IUserRepository>(provider =>
{
    string connectionStringName = "TurboazDb";

    string? connectionString = builder.Configuration.GetConnectionString(connectionStringName);

    if (string.IsNullOrEmpty(connectionString) || string.IsNullOrWhiteSpace(connectionString))
    {
        throw new Exception($"connection string {connectionStringName} not found in setting.json");
    }

    return new UserSqlRepository(connectionString);
});

builder.Services.AddScoped<ICustomLogger>(provider =>
{
    string connectionStringName = "TurboazDb";

    string? connectionString = builder.Configuration.GetConnectionString(connectionStringName);

    if (string.IsNullOrEmpty(connectionString) || string.IsNullOrWhiteSpace(connectionString))
    {
        throw new Exception($"connection string {connectionStringName} not found in setting.json");
    }

    bool isCustomLoggingEnabled = builder.Configuration.GetSection("isCustomLoggingEnabled").Get<bool>();

    return new SqlLogger(connectionString, isCustomLoggingEnabled);
});

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