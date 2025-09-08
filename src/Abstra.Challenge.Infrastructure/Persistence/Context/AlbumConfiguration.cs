using Abstra.Challenge.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abstra.Challenge.Infrastructure.Persistence.Context;

internal sealed class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("Albums");
        
        builder.HasKey(album => album.Id);

        builder.HasIndex(album => album.Artist);
        builder.HasIndex(album => album.Title);
        
        builder
            .HasMany(album => album.Tracks)
            .WithOne(track => track.Album)
            .HasForeignKey(track => track.AlbumId);
    }
}