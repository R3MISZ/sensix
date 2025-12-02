namespace Sensix.Api.Dtos.Devices;

public class DeviceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public int SensorCount { get; set; } // for UI how many sensors
}
