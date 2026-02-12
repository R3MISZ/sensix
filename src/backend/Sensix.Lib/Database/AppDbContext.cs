using Microsoft.EntityFrameworkCore;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<Measurement> Measurements => Set<Measurement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Get all classes using IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}