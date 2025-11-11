using Fireseal.Music.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fireseal.Music.Infrastructure.Persistence.Context;

internal sealed class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.ToTable("Tracks");
        
        builder.HasKey(track => track.Id);

        builder.HasIndex(track => track.Isrc).IsUnique();
        builder.HasIndex(track => track.Title);
    }
}