using Fireseal.Music.Infrastructure.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fireseal.Music.Tests.Unit.Fakes;

internal sealed class FakeRepository(DbContext context) : DbContextRepository<FakeEntity, Guid>(context);