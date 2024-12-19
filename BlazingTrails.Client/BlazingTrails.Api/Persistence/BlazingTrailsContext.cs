using BlazingTrails.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.Api.Persistence;

public class BlazingTrailsContext(DbContextOptions<BlazingTrailsContext> options) : DbContext(options)
{
    public DbSet<Trail> Trails => Set<Trail>();
    public DbSet<Waypoint> Waypoints => Set<Waypoint>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TrailConfig());
        modelBuilder.ApplyConfiguration(new WaypointConfig());
    }
}