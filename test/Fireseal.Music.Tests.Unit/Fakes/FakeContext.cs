using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Fireseal.Music.Tests.Unit.Fakes;

internal sealed class FakeContext(DbContextOptions<FakeContext> options) : DbContext(options)
{
    internal DbSet<FakeEntity> FakeEntities => Set<FakeEntity>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
        optionsBuilder.LogTo(message => Debug.WriteLine(message));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FakeEntity>()
            .HasMany(fakeEntity => fakeEntity.Children)
            .WithOne(fakeEntity => fakeEntity.ParentFakeEntity)
            .HasForeignKey(fakeEntity => fakeEntity.ParentFakeEntityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}