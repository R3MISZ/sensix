using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Database;

public class DeviceDbConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
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
    }
}