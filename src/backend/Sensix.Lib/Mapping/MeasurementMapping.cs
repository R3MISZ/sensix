using Sensix.Lib.Dtos;
using Sensix.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensix.Lib.Mapping;

public class MeasurementMapping
{
    public static MeasurementResponse ToResponse(Measurement measurement)
    {
        return new MeasurementResponse
        {
            Id = measurement.Id,
            SensorId = measurement.SensorId,
            TimestampUtc = measurement.TimestampUtc,
            Value = measurement.Value
        };
    }
    public static IReadOnlyList<MeasurementResponse> ToResponseList(IEnumerable<Measurement> measurements)
    {
        return measurements
            .Select(ToResponse)
            .ToList();
    }
}
