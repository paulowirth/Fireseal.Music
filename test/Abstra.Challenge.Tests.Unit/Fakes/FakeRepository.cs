using Abstra.Challenge.Infrastructure.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Abstra.Challenge.Tests.Unit.Fakes;

internal sealed class FakeRepository(DbContext context) : DbContextRepository<FakeEntity, Guid>(context);