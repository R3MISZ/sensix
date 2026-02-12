using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Database;

public class SensorDbConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
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
    }
}