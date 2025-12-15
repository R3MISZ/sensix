using Sensix.Lib.Entities;
using Sensix.Lib.Dtos;
using Sensix.Lib.Mapping;
using Sensix.Lib.Repository;

namespace Sensix.Lib.Service;

public interface IDeviceService
{
    Task<DeviceResponse> CreateDeviceAsync(CreateDeviceRequest request);
    Task<IReadOnlyList<DeviceResponse>> ReadDevicesAsync();
    Task<DeviceResponse?> UpdateDeviceAsync(UpdateDeviceRequest request);
    Task<bool> DeleteDeviceAsync(Guid id);
}

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<DeviceResponse> CreateDeviceAsync(CreateDeviceRequest request)
    {
        var device = new Device();
        device.SetName(request.Name);
        device.SetLocation(request.Location);

        await _deviceRepository.AddAsync(device);
        await _deviceRepository.SaveChangesAsync();

        return DeviceMapping.ToResponse(device);
    }

    public async Task<IReadOnlyList<DeviceResponse>> ReadDevicesAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();

        return DeviceMapping.ToResponseList(devices);
    }

    public async Task<DeviceResponse?> UpdateDeviceAsync(UpdateDeviceRequest request)
    {
        var device = await _deviceRepository.GetByIdAsync(request.Id);
        if (device is null)
            return null;

        device.SetName(request.Name);
        device.SetLocation(request.Location);

        if (request.IsActive)
            device.Activate();
        else
            device.Deactivate();

        await _deviceRepository.SaveChangesAsync();

        return DeviceMapping.ToResponse(device);
    }

    public async Task<bool> DeleteDeviceAsync(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device is null)
            return false;

        await _deviceRepository.RemoveAsync(device);
        await _deviceRepository.SaveChangesAsync();

        return true;
    }
}