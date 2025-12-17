namespace Sensix.Lib.Entities;

public class Device
{
    // PK
    public Guid Id { get; private set; } = Guid.NewGuid();

    // FK
    public ICollection<Sensor> Sensors { get; private set; } = new List<Sensor>();

    // Properties
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string? Name { get; private set; }
    public string? Location { get; private set; }
    public bool IsActive { get; private set; } = false;


    public void SetName(string? name)
    {
        Name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
    }

    public void SetLocation(string? location)
    {
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}