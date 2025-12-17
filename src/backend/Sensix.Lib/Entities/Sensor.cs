namespace Sensix.Lib.Entities;

public class Sensor
{
    // PK
    public Guid Id { get; private set; } = Guid.NewGuid();

    // FK
    public Guid? DeviceId { get; private set; }
    public Device? Device { get; private set; }

    // Measurements
    public ICollection<Measurement> Measurements { get; private set; } = new List<Measurement>();

    // Properties
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public string? Name { get; private set; }
    public string? Type { get; private set; } // temperature, humidity, rpm, ...
    public string? Unit { get; private set; } // °C, %, ...
    public bool IsActive { get; private set; } = false;

    public void SetName(string? name)
    {
        Name = name?.Trim();
    }
    public void SetType(string? type)
    {
        Type = type?.Trim();
    }

    public void SetUnit(string? unit)
    {
        Unit = unit?.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;


    public void SetDeviceId(Guid? deviceId)
    {
        DeviceId = deviceId;
    }
}