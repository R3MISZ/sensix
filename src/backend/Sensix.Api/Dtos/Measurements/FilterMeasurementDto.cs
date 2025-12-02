using Sensix.Api.Dtos.Common;

namespace Sensix.Api.Dtos.Measurements;

public class FilterMeasurementDto : PagingQuery
{
    // Filter nach Device
    public Guid? DeviceId { get; set; }

    // Filter nach Sensor
    public Guid? SensorId { get; set; }

    // Zeitbereich
    public DateTime? FromUtc { get; set; }
    public DateTime? ToUtc { get; set; }
}
