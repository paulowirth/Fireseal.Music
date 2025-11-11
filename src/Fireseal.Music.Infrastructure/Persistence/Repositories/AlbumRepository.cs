using Fireseal.Music.Domain;
using Fireseal.Music.Infrastructure.Abstractions.Persistence;
using Fireseal.Music.Infrastructure.Persistence.Context;

namespace Fireseal.Music.Infrastructure.Persistence.Repositories;

internal sealed class AlbumRepository(FiresealContext context) : DbContextRepository<Album, Guid>(context);