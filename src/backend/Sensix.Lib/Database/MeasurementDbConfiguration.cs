using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Database;

public class MeasurementDbConfiguration : IEntityTypeConfiguration<Measurement>
{
    public void Configure(EntityTypeBuilder<Measurement> builder)
    {
        builder.ToTable("Measurements");

        // PK
        builder.HasKey(m => m.Id);

        // Index for faster queries by time
        builder.HasIndex(m => m.TimestampUtc);

        // Properties
        builder.Property(m => m.TimestampUtc)
            .IsRequired();

        builder.Property(m => m.Value)
            .IsRequired();
    }
}