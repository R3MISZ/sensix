using Sensix.Lib.Entities;
using Sensix.Lib.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensix.Lib.Mapping;

public static class DeviceMapping
{
    public static DeviceResponse ToResponse(Device device)
    {
        return new DeviceResponse
        {
            Id = device.Id,
            Name = device.Name,
            Location = device.Location,
            IsActive = device.IsActive
        };
    }

    public static IReadOnlyList<DeviceResponse> ToResponseList(IEnumerable<Device> devices)
    {
        return devices
            .Select(ToResponse)
            .ToList();
    }
}

