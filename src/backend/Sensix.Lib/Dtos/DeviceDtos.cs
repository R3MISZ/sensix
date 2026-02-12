namespace Sensix.Lib.Dtos;

// Input
public record CreateDeviceRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Location { get; init; }
    public bool IsActive { get; init; } = true;
}

public record UpdateDeviceRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Location { get; init; }
    public bool IsActive { get; init; }
}

// Output
public record DeviceDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Location { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}