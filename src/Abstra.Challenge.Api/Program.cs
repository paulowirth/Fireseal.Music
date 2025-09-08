using Abstra.Challenge.Application.DependencyInjection;
using Abstra.Challenge.Infrastructure.DependencyInjection;
using Abstra.Challenge.Infrastructure.Persistence.Context;
using Abstra.Challenge.Presentation.DependencyInjection;
using Abstra.Challenge.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

//Resolving Dependencies
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation();

var webApplication = builder.Build();

await using (var scope = webApplication.Services.CreateAsyncScope())
{
    var persistenceContext = scope.ServiceProvider.GetRequiredService<AbstraContext>();
    await persistenceContext.Database.EnsureCreatedAsync();
}

if (webApplication.Environment.IsDevelopment())
{
    webApplication
        .UseSwagger()
        .UseSwaggerUI();
}

webApplication
    .UseHttpsRedirection()
    .UseAuthorization()
    .UseMiddleware<HttpContextExceptionMiddleware>();

webApplication.MapControllers();

webApplication.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    
    await next();
});

webApplication.Run();

namespace Abstra.Challenge.Api
{
    public partial class Program { }
}