namespace Sensix.Lib.Entities;

public class Measurement
{
    // PK
    public Guid Id { get; private set; } = Guid.NewGuid();

    // FK
    public Guid SensorId { get; set; } // FK
    public Sensor Sensor { get; set; } = null!;

    // Properties
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public double Value { get; set; } = 0.0;
}