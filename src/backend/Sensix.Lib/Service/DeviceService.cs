using Sensix.Lib.Entities;
using Sensix.Lib.Dtos;
using Sensix.Lib.Mapping;
using Sensix.Lib.Repository;
using AutoMapper;

namespace Sensix.Lib.Service;

public interface IDeviceService
{
    Task<DeviceDto> CreateAsync(CreateDeviceRequest request);
    Task<IReadOnlyList<DeviceDto>> GetAllAsync();
    Task<DeviceDto?> GetByIdAsync(Guid id);
    Task<DeviceDto?> UpdateAsync(Guid id, UpdateDeviceRequest request);
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

    public async Task<DeviceDto> CreateAsync(CreateDeviceRequest request)
    {
        var device = _mapper.Map<Device>(request);

        await _deviceRepository.AddAsync(device);
        await _dbRepository.SaveChangesAsync();

        return _mapper.Map<DeviceDto>(device);
    }

    public async Task<IReadOnlyList<DeviceDto>> GetAllAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();

        return _mapper.Map<IReadOnlyList<DeviceDto>>(devices);
    }

    public async Task<DeviceDto?> GetByIdAsync(Guid id)
    {
        var device = await _deviceRepository.GetByIdNoTrackingAsync(id);

        // return -> if (true) ? then : else
        return (device is null) ? null : _mapper.Map<DeviceDto>(device);
    }

    public async Task<DeviceDto?> UpdateAsync(Guid id, UpdateDeviceRequest request)
    {
        var device = await _deviceRepository.GetByIdAsync(id);

        if (device is null) return null;

        _mapper.Map(request, device);

        if (request.Name is not null)
            device.SetName(request.Name);

        if (request.Location is not null)
            device.SetLocation(request.Location);

        await _dbRepository.SaveChangesAsync();
        return _mapper.Map<DeviceDto>(device);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);

        if (device is null) return false;

        await _deviceRepository.RemoveAsync(device);
        await _dbRepository.SaveChangesAsync();

        return true;
    }
}