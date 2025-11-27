using Microsoft.EntityFrameworkCore;

using Sensix.Infrastructure.Entities;

namespace Sensix.Infrastructure;

// For db migration
public class SensixDbContext : DbContext
{
    public SensixDbContext(DbContextOptions<SensixDbContext> options) : base(options)
    {
    }

    public DbSet<Measurement> Measurements { get; set; }
}