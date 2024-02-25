using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Steam.Data;
using Steam.Services;
using Steam.Services.Base;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();


builder.Services.AddScoped<IGameServiceBase, GameService>();
builder.Services.AddScoped<IWorkShopServiceBase, WorkShopService>();

builder.Services.AddDbContext<SteamDBContext>(dbContextOptionsBuilder => {
    var connectionString = builder.Configuration.GetConnectionString("SteamBase");
    dbContextOptionsBuilder.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<SteamDBContext>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
