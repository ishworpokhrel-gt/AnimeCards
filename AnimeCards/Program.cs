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
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

try
{
    Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Information()
                 .WriteTo.Console()
                 .WriteTo.File(new CompactJsonFormatter(), "Logs/logs.log", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                 .CreateLogger();
    Log.Information("Starting web host.");

    var builder = WebApplication.CreateBuilder(args);
    var services = builder.Services.BuildServiceProvider();


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

            OnMessageReceived = context =>
            {

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
    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        await SeedRoles.RoleSeeder(roleManager);
        await SeedUser.UserSeeder(context, userManager);
        await PermissionSeeder.SeedAdminPermission(context);

        AutoMigration();
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
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "error occured while migraiton.");
        throw;
    }
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
    app.MapControllers();

    app.Run();



}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
