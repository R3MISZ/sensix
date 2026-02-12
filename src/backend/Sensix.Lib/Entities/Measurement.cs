namespace Sensix.Lib.Entities;

public class Measurement
{
    public Guid Id { get; private set; }
    public Guid SensorId { get; private set; }
    public double Value { get; private set; }
    public DateTime TimestampUtc { get; private set; }

    // Navigation
    public Sensor Sensor { get; private set; } = null!;

    private Measurement() { }

    public Measurement(Guid sensorId, double value, DateTime? timestamp = null)
    {
        Id = Guid.NewGuid();
        SensorId = sensorId;
        Value = value;
        TimestampUtc = timestamp ?? DateTime.UtcNow;
    }
}