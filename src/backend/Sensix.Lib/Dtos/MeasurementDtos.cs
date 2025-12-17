using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensix.Lib.Dtos;

public class CreateMeasurementRequest
{
    public Guid SensorId { get; set; }
    public DateTime TimestampUtc { get; set; }
    public double Value { get; set; }
}

public class UpdateMeasurementRequest
{
    public Guid? SensorId { get; set; }
    public DateTime? TimestampUtc { get; set; }
    public double? Value { get; set; }
}

public class MeasurementResponse
{
    public Guid Id { get; set; }
    public Guid SensorId { get; set; }
    public DateTime TimestampUtc { get; set; }
    public double Value { get; set; }
}
