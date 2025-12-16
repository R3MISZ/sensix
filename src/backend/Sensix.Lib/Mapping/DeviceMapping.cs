using Sensix.Lib.Dtos;
using Sensix.Lib.Entities;

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

