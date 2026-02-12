namespace Sensix.Lib.Dtos;

// Input
public record CreateMeasurementRequest
{
    public Guid SensorId { get; init; }
    public double Value { get; init; }
    public DateTime TimestampUtc { get; init; } = DateTime.UtcNow;
}

public record UpdateMeasurementRequest
{
    public double Value { get; init; }
    public DateTime TimestampUtc { get; init; }
}

// Output
public record MeasurementDto
{
    public Guid Id { get; init; }
    public Guid SensorId { get; init; }
    public double Value { get; init; }
    public DateTime TimestampUtc { get; init; }
}
