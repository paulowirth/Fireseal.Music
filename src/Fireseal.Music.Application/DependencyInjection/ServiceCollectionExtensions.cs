using Fireseal.Music.Application.Albums;
using Fireseal.Music.Application.Tracks;
using Microsoft.Extensions.DependencyInjection;

namespace Fireseal.Music.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IAlbumService, AlbumService>()
            .AddScoped<ITrackService, TrackService>();
    }
}