namespace Sensix.Api.Dtos.Sensors;

public class SensorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string Unit { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
}