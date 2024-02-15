using Steam.Middleware;
using Steam.Services;
using Steam.Services.Base;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDataProtection();

var connection = builder.Configuration.GetConnectionString("SteamBase");

builder.Services.AddScoped<ILogRepository>(e => new LogSqlRepository(new SqlConnection(connection)));
builder.Services.AddScoped<IUserRepositoryBase>(e => new UserSqlRepository(new SqlConnection(connection)));
builder.Services.AddScoped<IGameRepository>(e => new GameSqlRepository(new SqlConnection(connection)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<LogsMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
