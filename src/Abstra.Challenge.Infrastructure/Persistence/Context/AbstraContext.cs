using System.Reflection;
using Abstra.Challenge.Domain;
using Abstra.Challenge.Infrastructure.Persistence.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Abstra.Challenge.Infrastructure.Persistence.Context;

public sealed class AbstraContext : DbContext
{
    private readonly PersistenceOptions _options;
    
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Track> Tracks => Set<Track>();

    public AbstraContext(IOptions<PersistenceOptions> options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Value.ConnectionString);
        _options = options.Value;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(_options.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}