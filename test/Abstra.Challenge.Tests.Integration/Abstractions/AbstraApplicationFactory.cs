using Abstra.Challenge.Infrastructure.Persistence.Context;
using Abstra.Challenge.Infrastructure.Persistence.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Abstra.Challenge.Tests.Integration.Abstractions;

public sealed class AbstraApplicationFactory : WebApplicationFactory<Api.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(serviceCollection =>
        {
            var dbName = Guid.NewGuid().ToString();
            var connectionString = $"DataSource=file:{dbName}?mode=memory&cache=shared";

            serviceCollection
                .RemoveAll<IOptions<PersistenceOptions>>()
                .RemoveAll<DbContextOptions<AbstraContext>>()
                .Configure<PersistenceOptions>(options => { options.ConnectionString = connectionString; })
                .AddDbContext<AbstraContext>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AbstraContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        });
    }
}
