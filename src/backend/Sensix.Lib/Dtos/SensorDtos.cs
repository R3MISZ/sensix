namespace Sensix.Lib.Dtos;

// Input
public record CreateSensorRequest
{
    public Guid DeviceId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? Unit { get; init; }
    public bool IsActive { get; init; } = true;
}

public record UpdateSensorRequest
{
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? Unit { get; init; }
    public bool IsActive { get; init; }
}

// Output
public record SensorDto
{
    public Guid Id { get; init; }
    public Guid DeviceId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? Unit { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}