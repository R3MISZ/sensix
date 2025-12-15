using Microsoft.EntityFrameworkCore;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Database;

public class SensixDbContext : DbContext
{
    public SensixDbContext(DbContextOptions<SensixDbContext> options) : base(options)
    {
    }

    public DbSet<Device> Devices => Set<Device>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(builder =>
        {
            builder.ToTable("Devices");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Location)
                .HasMaxLength(500);

            builder.Property(d => d.IsActive)
                .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}