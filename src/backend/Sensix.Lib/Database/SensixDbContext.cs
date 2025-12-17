using Microsoft.EntityFrameworkCore;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Database;

public class SensixDbContext : DbContext
{
    public SensixDbContext(DbContextOptions<SensixDbContext> options) : base(options)
    {
    }

    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<Measurement> Measurements => Set<Measurement>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(builder =>
        {
            builder.ToTable("Devices");

            // PK
            builder.HasKey(device => device.Id);

            // FK
            builder.HasMany(device => device.Sensors)
                .WithOne(sensor => sensor.Device)
                .HasForeignKey(sensor => sensor.DeviceId)
                .OnDelete(DeleteBehavior.SetNull);

            // Properties
            builder.Property(device => device.CreatedAtUtc)
                .IsRequired();

            builder.Property(device => device.Name)
                .HasMaxLength(200);

            builder.Property(device => device.Location)
                .HasMaxLength(500);

            builder.Property(device => device.IsActive)
                .IsRequired();
        });

        modelBuilder.Entity<Sensor>(builder =>
        {
            builder.ToTable("Sensors");

            // PK
            builder.HasKey(sensor => sensor.Id);

            // FK
            builder.HasMany(sensor => sensor.Measurements)
                .WithOne(measurement => measurement.Sensor)
                .HasForeignKey(measurement => measurement.SensorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Properties
            builder.Property(sensor => sensor.CreatedAtUtc)
                .IsRequired();

            builder.Property(sensor => sensor.Name)
                .HasMaxLength(200);

            builder.Property(sensor => sensor.Type)
                .HasMaxLength(100);

            builder.Property(sensor => sensor.Unit)
                .HasMaxLength(50);

            builder.Property(sensor => sensor.IsActive)
                .IsRequired();
        });

        modelBuilder.Entity<Measurement>(builder =>
        {
            builder.ToTable("Measurements");

            // PK
            builder.HasKey(m => m.Id);

            // Properties
            builder.Property(measurement => measurement.TimestampUtc)
                .IsRequired();

            builder.Property(m => m.Value)
                .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}