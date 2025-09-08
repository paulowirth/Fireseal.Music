using Abstra.Challenge.Domain;
using Abstra.Challenge.Domain.Abstractions;
using Abstra.Challenge.Infrastructure.Persistence.Context;
using Abstra.Challenge.Infrastructure.Persistence.Options;
using Abstra.Challenge.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Abstra.Challenge.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection.AddPersistence(configuration);
    }
    
    private static IServiceCollection AddPersistence(
        this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .Configure<PersistenceOptions>(configuration.GetSection(nameof(PersistenceOptions)))
            .AddDbContext<AbstraContext>()
            .AddScoped<IRepository<Album, Guid>, AlbumRepository>()
            .AddScoped<IRepository<Track, Guid>, TrackRepository>();
    }
}