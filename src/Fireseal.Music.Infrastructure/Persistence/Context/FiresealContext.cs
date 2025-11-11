using System.Reflection;
using Fireseal.Music.Domain;
using Fireseal.Music.Infrastructure.Persistence.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fireseal.Music.Infrastructure.Persistence.Context;

public sealed class FiresealContext : DbContext
{
    private readonly PersistenceOptions _options;
    
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Track> Tracks => Set<Track>();

    public FiresealContext(IOptions<PersistenceOptions> options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Value.ConnectionString);
        _options = options.Value;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(_options.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}