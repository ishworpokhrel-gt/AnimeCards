using AnimeCards.Extension;
using AnimeCards.MiddleWare;
using Business.Anime;
using Business.Business.cms.Account;
using Common_Shared.Accessor;
using Common_Shared.ResponseWrapper;
using Common_Shared.Token;
using Data;
using Data.Seed;
using Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
builder.Services.AddAuthentication(i =>
{
    i.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    i.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    i.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    i.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
    opt.SaveToken = true;
    opt.Events = new JwtBearerEvents
    {
        // This method is first event in authentication pipeline
        // we have chance to wait until TokenValidationParameters
        // is loaded.
        OnMessageReceived = context =>
        {
            //if (context.Request.Path.StartsWithSegments("/hangfire-admin-dashboard"))
            //    return Task.CompletedTask;

            if (context.Request.Cookies.ContainsKey("X-Access-Token"))
            {
                context.Token = context.Request.Cookies["X-Access-Token"];
            }
            else if (context.Request.Cookies.ContainsKey("X-Refresh-Token"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Fail("");
                return Task.CompletedTask;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Fail("");
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            if (context.Response.StatusCode != (int)HttpStatusCode.Forbidden)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.HandleResponse();
                return Task.CompletedTask;
            }
            else
            {
                context.HandleResponse();
                context.Response.ContentType = "application/json";
                var errorObject = ErrorResponseWrapper.ErrorApi("Forbidden", 4031);
                var json = JsonConvert.SerializeObject(errorObject);

                return context.Response.WriteAsync(json);
            }

        }
    };
});
builder.Services.AddSwaggerServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
await SeedRoles.RoleSeeder(roleManager);
await SeedUser.UserSeeder(context, userManager);
await PermissionSeeder.SeedAdminPermission(context);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();

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
