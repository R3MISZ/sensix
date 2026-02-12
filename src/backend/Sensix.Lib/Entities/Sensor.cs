namespace Sensix.Lib.Entities;

public class Sensor
{
    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string? Unit { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    // Navigation
    public Device Device { get; private set; } = null!;
    public ICollection<Measurement> Measurements { get; private set; } = new List<Measurement>();

    private Sensor() { }

    public Sensor(Guid deviceId, string name, string type, string? unit = null)
    {
        Id = Guid.NewGuid();
        DeviceId = deviceId;
        Update(name, type, unit);
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Update(string name, string type, string? unit)
    {
        Name = name.Trim();
        Type = type.Trim();
        Unit = unit?.Trim();
    }
}