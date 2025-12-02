using Microsoft.EntityFrameworkCore;

using Sensix.Infrastructure.Entities;

namespace Sensix.Infrastructure;

// For db migration
public class SensixDbContext : DbContext
{
    public SensixDbContext(DbContextOptions<SensixDbContext> options) : base(options) {}

    public DbSet<Device> Devices { get; set; }
    
    public DbSet<Sensor> Sensors { get; set; }

    public DbSet<Measurement> Measurements { get; set; }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        // device
        model.Entity<Device>(e =>
        {
            e.HasKey(d => d.Id);

            e.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(d => d.Location)
                .HasMaxLength(150);

            e.Property(d => d.Description)
                .HasMaxLength(250);

            // Device 1..N Sensors
            e.HasMany(d => d.Sensors)
                .WithOne(s => s.Device)
                .HasForeignKey(s => s.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // sensor
        model.Entity<Sensor>(e =>
        {
            e.HasKey(s => s.Id);

            e.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(s => s.Type)
                .HasMaxLength(50);

            e.Property(s => s.Unit)
                .IsRequired()
                .HasMaxLength(20);

            // sensor 1..N measurements
            e.HasMany(s => s.Measurements)
                .WithOne(m => m.Sensor)
                .HasForeignKey(m => m.SensorId)
                .OnDelete(DeleteBehavior.Cascade);

            // performance: Index for filtering by Device
            e.HasIndex(s => s.DeviceId);
        });

        // measurement
        model.Entity<Measurement>(e =>
        {
            e.HasKey(m => m.Id);

            e.Property(m => m.Unit)
                .IsRequired()
                .HasMaxLength(20);

            e.Property(m => m.Value)
                .IsRequired();

            // Performance: timestamp queries
            e.HasIndex(m => m.TimestampUtc);

            // Performance: filtering by sensor & time range
            e.HasIndex(m => new { m.SensorId, m.TimestampUtc });
        });
    }
}