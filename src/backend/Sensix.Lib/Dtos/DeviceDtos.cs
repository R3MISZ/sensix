namespace Sensix.Lib.Dtos;

public class CreateDeviceRequest
{
    public string? Name { get; set; }
    public string? Location { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateDeviceRequest
{
    public string? Name { get; set; }
    public string? Location { get; set; }
    public bool? IsActive { get; set; }
}

public class DeviceResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }
}