using Fireseal.Music.Application.DependencyInjection;
using Fireseal.Music.Infrastructure.DependencyInjection;
using Fireseal.Music.Infrastructure.Persistence.Context;
using Fireseal.Music.Presentation.DependencyInjection;
using Fireseal.Music.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

//Resolving Dependencies
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation();

var webApplication = builder.Build();

await using (var scope = webApplication.Services.CreateAsyncScope())
{
    var persistenceContext = scope.ServiceProvider.GetRequiredService<FiresealContext>();
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

namespace Fireseal.Music.Api
{
    public partial class Program {}
}