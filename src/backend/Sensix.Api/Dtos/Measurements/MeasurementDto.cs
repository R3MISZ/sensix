using Sensix.Api.Dtos.Common;

namespace Sensix.Api.Dtos.Measurements;

// Response obj (only output, no validation needed)
public class MeasurementDto
{
    public Guid Id { get; set; }

    public DateTime TimestampUtc { get; set; }

    public double Value { get; set; }

    public string Unit { get; set; } = string.Empty;

    // Sensor information
    public Guid SensorId { get; set; }
    public string SensorName { get; set; } = string.Empty;

    // Device information (Sensor.Device)
    public Guid DeviceId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
}