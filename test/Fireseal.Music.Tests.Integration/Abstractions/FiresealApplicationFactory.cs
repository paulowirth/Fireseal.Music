using Fireseal.Music.Infrastructure.Persistence.Context;
using Fireseal.Music.Infrastructure.Persistence.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Fireseal.Music.Tests.Integration.Abstractions;

public sealed class FiresealApplicationFactory : WebApplicationFactory<Fireseal.Music.Api.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(serviceCollection =>
        {
            var dbName = Guid.NewGuid().ToString();
            var connectionString = $"DataSource=file:{dbName}?mode=memory&cache=shared";

            serviceCollection
                .RemoveAll<IOptions<PersistenceOptions>>()
                .RemoveAll<DbContextOptions<FiresealContext>>()
                .Configure<PersistenceOptions>(options => { options.ConnectionString = connectionString; })
                .AddDbContext<FiresealContext>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FiresealContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        });
    }
}
