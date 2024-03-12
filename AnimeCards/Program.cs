using AnimeCards.MiddleWare;
//using Business.Account;
using Business.Anime;
using Business.Business.cms.Account;
using Common_Shared.Accessor;
using Common_Shared.Token;
using Data;
using Data.Seed;
using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAnimeSerivice, AnimeService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<IUserAccessor, UserAccessor>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddDefaultTokenProviders();

//await SeedUser.UserSeeder(AppDbContext context, UserManager<ApplicationUser> usermanager)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
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
