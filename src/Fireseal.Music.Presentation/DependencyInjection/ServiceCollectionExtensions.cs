using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Fireseal.Music.Presentation.Authentication;
using Microsoft.OpenApi;

namespace Fireseal.Music.Presentation.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public void AddPresentation()
        {
            serviceCollection
                .AddAuthentication()
                .AddFluentValidation()
                .AddControllers();

            serviceCollection
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(swaggerGenOptions =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    swaggerGenOptions.IncludeXmlComments(xmlPath);

                    //JWT Bearer config
                    swaggerGenOptions.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme."
                    });
                
                    swaggerGenOptions.AddSecurityRequirement(document => 
                        new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("bearer", document)] = []
                        });
                });
        }

        private IServiceCollection AddAuthentication()
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

        private IServiceCollection AddFluentValidation() =>
            serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}