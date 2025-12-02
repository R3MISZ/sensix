namespace Sensix.Infrastructure.Entities;

public class Sensor
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; } // temperature, humidity, rpm, ...
    public string Unit { get; set; } = string.Empty; // °C, %, ...
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    
    public Guid DeviceId { get; set; } // foreign key -> device
    public Device Device { get; set; } = null!;

    public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>(); // Navigation → Measurements
}