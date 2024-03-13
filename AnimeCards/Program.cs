using AnimeCards.MiddleWare;
using Business.Anime;
using Business.Business.cms.Account;
using Common_Shared.Accessor;
using Common_Shared.Token;
using Data;
using Data.Seed;
using Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddDefaultTokenProviders();
builder.Services.AddScoped<IAnimeSerivice, AnimeService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<IUserAccessor, UserAccessor>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
await SeedRoles.RoleSeeder(roleManager);
await SeedUser.UserSeeder(context, userManager);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
AutoMigration();
app.MapControllers();

app.Run();

void AutoMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var data = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (data.Database.GetPendingMigrations().Any())
        {
            data.Database.MigrateAsync();
        }
    }
}
