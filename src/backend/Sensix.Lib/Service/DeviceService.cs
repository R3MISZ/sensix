using Sensix.Lib.Entities;
using Sensix.Lib.Dtos;
using Sensix.Lib.Mapping;
using Sensix.Lib.Repository;

namespace Sensix.Lib.Service;

public interface IDeviceService
{
    Task<DeviceResponse> CreateAsync(CreateDeviceRequest request);
    Task<IReadOnlyList<DeviceResponse>> GetAllAsync();
    Task<DeviceResponse?> GetByIdAsync(Guid id);
    Task<DeviceResponse?> UpdateAsync(Guid id, UpdateDeviceRequest request);
    Task<bool> DeleteAsync(Guid id);
}

public class DeviceService : IDeviceService
{
    private readonly IUnitOfWork _dbRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;

    public DeviceService(IUnitOfWork dbRepository, IDeviceRepository deviceRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _deviceRepository = deviceRepository;
        _mapper = mapper;
    }

    public async Task<DeviceResponse> CreateAsync(CreateDeviceRequest request)
    {
        var device = new Device();

        device.SetName(request.Name);
        device.SetLocation(request.Location);

        if (request.IsActive is true) device.Activate();
        else if (request.IsActive is false) device.Deactivate();

        await _deviceRepository.AddAsync(device);
        await _dbRepository.SaveChangesAsync();

        return DeviceMapping.ToResponse(device);
    }

    public async Task<IReadOnlyList<DeviceResponse>> GetAllAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();

        return DeviceMapping.ToResponseList(devices);
    }

    public async Task<DeviceResponse?> GetByIdAsync(Guid id)
    {
        var device = await _deviceRepository.GetByIdNoTrackingAsync(id);
        if (device is null) return null;

        return DeviceMapping.ToResponse(device);
    }

    public async Task<DeviceResponse?> UpdateAsync(Guid id, UpdateDeviceRequest request)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device is null) return null;

        if (request.Name is not null)
            device.SetName(request.Name);

        if (request.Location is not null)
            device.SetLocation(request.Location);

        if (request.IsActive is true) device.Activate();
        else if (request.IsActive is false) device.Deactivate();

        await _dbRepository.SaveChangesAsync();
        return DeviceMapping.ToResponse(device);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device is null)
            return false;

        await _deviceRepository.RemoveAsync(device);
        await _dbRepository.SaveChangesAsync();

        return true;
    }
}