using Abstra.Challenge.Application.Albums;
using Abstra.Challenge.Application.Tracks;
using Microsoft.Extensions.DependencyInjection;

namespace Abstra.Challenge.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IAlbumService, AlbumService>()
            .AddScoped<ITrackService, TrackService>();
    }
}