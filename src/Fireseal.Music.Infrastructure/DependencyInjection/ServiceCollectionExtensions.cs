using Fireseal.Music.Domain;
using Fireseal.Music.Domain.Abstractions;
using Fireseal.Music.Infrastructure.Persistence.Context;
using Fireseal.Music.Infrastructure.Persistence.Options;
using Fireseal.Music.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fireseal.Music.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            return serviceCollection.AddPersistence(configuration);
        }

        private IServiceCollection AddPersistence(IConfiguration configuration)
        {
            return serviceCollection
                .Configure<PersistenceOptions>(configuration.GetSection(nameof(PersistenceOptions)))
                .AddDbContext<FiresealContext>()
                .AddScoped<IRepository<Album, Guid>, AlbumRepository>()
                .AddScoped<IRepository<Track, Guid>, TrackRepository>();
        }
    }
}