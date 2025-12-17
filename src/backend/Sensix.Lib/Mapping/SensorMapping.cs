using Sensix.Lib.Dtos;
using Sensix.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensix.Lib.Mapping;

public class SensorMapping
{
    public static SensorResponse ToResponse(Sensor sensor)
    {
        return new SensorResponse
        {
            Id = sensor.Id,
            CreatedAtUtc = sensor.CreatedAtUtc,
            Name = sensor.Name,
            Type = sensor.Type,
            Unit = sensor.Unit,
            IsActive = sensor.IsActive,

            DeviceId = sensor.DeviceId
        };
    }

    public static IReadOnlyList<SensorResponse> ToResponseList(IEnumerable<Sensor> sensors)
    {
        return sensors
            .Select(ToResponse)
            .ToList();
    }
}
