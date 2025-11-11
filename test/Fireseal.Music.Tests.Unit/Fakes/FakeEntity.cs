using System.Diagnostics.CodeAnalysis;
using Fireseal.Music.Domain.Abstractions;

namespace Fireseal.Music.Tests.Unit.Fakes;

[ExcludeFromCodeCoverage]
internal sealed class FakeEntity : Entity<Guid>
{
    public required string Name { get; set; }
    
    public Guid? ParentFakeEntityId { get; set; }

    public FakeEntity? ParentFakeEntity { get; set; }

    public List<FakeEntity> Children { get; set; } = [];
}