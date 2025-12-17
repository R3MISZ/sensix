namespace Sensix.Lib.Dtos;

public class CreateSensorRequest
{
    public Guid? DeviceId { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Unit { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateSensorRequest
{
    public Guid? DeviceId { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Unit { get; set; }
    public bool? IsActive { get; set; }
}

public class SensorResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? DeviceId { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Unit { get; set; }
    public bool IsActive { get; set; }
}