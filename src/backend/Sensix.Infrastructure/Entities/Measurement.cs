namespace Sensix.Infrastructure.Entities;

public class Measurement
{
    public Guid Id { get; set; }

    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

    public double Value { get; set; }

    public string Unit { get; set; } = string.Empty;
}
