using Abstra.Challenge.Domain;
using Abstra.Challenge.Infrastructure.Abstractions.Persistence;
using Abstra.Challenge.Infrastructure.Persistence.Context;

namespace Abstra.Challenge.Infrastructure.Persistence.Repositories;

internal sealed class TrackRepository(AbstraContext context) : DbContextRepository<Track, Guid>(context);