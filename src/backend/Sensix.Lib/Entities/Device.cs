namespace Sensix.Lib.Entities;

public class Device
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Location { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    // Navigation property
    public ICollection<Sensor> Sensors { get; private set; } = new List<Sensor>();

    // EF Core 
    private Device() { }

    // Logic for new device
    public Device(string name, string? location = null)
    {
        Id = Guid.NewGuid();
        Update(name, location);
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Update(string name, string? location)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name darf nicht leer sein.");
        Name = name.Trim();
        Location = location?.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}