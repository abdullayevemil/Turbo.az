using Turbo.az.Repositories;
using Turbo.az.Repositories.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();