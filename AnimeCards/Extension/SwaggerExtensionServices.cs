using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AnimeCards.Extension
{
    public static class SwaggerExtensionServices
    {
            public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration config)
            {
                services.AddApiVersioning(setup =>
                {
                    setup.DefaultApiVersion = new ApiVersion(1, 0);
                    setup.AssumeDefaultVersionWhenUnspecified = true;
                    setup.ReportApiVersions = true;
                });
                services.AddVersionedApiExplorer(setup =>
                {
                    setup.GroupNameFormat = "'v'VVV";
                    setup.SubstituteApiVersionInUrl = true;
                });
            


                services.AddSwaggerGen(opt =>
                {
                    var securityScheme = new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Cookie,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "cookie"
                    };

                    var securityRequirement = new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "CookieAuth"
                                    }
                                },
                                new string[]{}
                            }
                         };
                    opt.AddSecurityDefinition("CookieAuth", securityScheme);
                    opt.AddSecurityRequirement(securityRequirement);
                });

                services.ConfigureOptions<ConfigureSwaggerOptions>();
                return services;
            }
        

        public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
        {
            private readonly IApiVersionDescriptionProvider provider;

            public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
            {
                this.provider = provider;
            }

            public void Configure(SwaggerGenOptions options)
            {
                // add swagger document for every API version discovered
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        description.GroupName,
                        CreateVersionInfo(description));
                }
            }

            public void Configure(string name, SwaggerGenOptions options)
            {
                Configure(options);
            }

            private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
            {
                var info = new OpenApiInfo()
                {
                    Title = "AnimeCard API",
                    Version = description.ApiVersion.ToString()
                };

                if (description.IsDeprecated)
                {
                    info.Description += "This API version has been deprecated.";
                }

                return info;
            }
        }
    }
}

