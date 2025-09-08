using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Abstra.Challenge.Presentation.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Abstra.Challenge.Presentation.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddAuthentication()
            .AddFluentValidation()
            .AddControllers();
        
        return serviceCollection
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(swaggerGenOptions =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swaggerGenOptions.IncludeXmlComments(xmlPath);

                //JWT Bearer config
                swaggerGenOptions.AddSecurityDefinition(
                    "Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token. Example: Bearer eyJhbGci..."
                });
                
                swaggerGenOptions.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            []
                        }
                    });
                });
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = 
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = AuthenticationConstants.JwtIssuer,
                        ValidAudience = AuthenticationConstants.JwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationConstants.JwtKey))
                    };
            });
        return serviceCollection;
    }
    
    private static IServiceCollection AddFluentValidation(this IServiceCollection serviceCollection) =>
        serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
}