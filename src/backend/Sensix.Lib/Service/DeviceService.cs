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
    private readonly IUnitOfWork _uow;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;

    public DeviceService(IUnitOfWork uow, IDeviceRepository deviceRepository, IMapper mapper)
    {
        _uow = uow;
        _deviceRepository = deviceRepository;
        _mapper = mapper;
    }

    public async Task<DeviceDto> CreateAsync(CreateDeviceRequest request)
    {
        // Nutzt jetzt ConstructUsing im MappingProfile (via Device-Konstruktor)
        var device = _mapper.Map<Device>(request);

        await _deviceRepository.AddAsync(device);
        await _uow.SaveChangesAsync();

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
        return device is null ? null : _mapper.Map<DeviceDto>(device);
    }

    public async Task<DeviceDto?> UpdateAsync(Guid id, UpdateDeviceRequest request)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device is null) return null;

        _mapper.Map(request, device);

        await _uow.SaveChangesAsync();
        return _mapper.Map<DeviceDto>(device);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device is null) return false;

        await _deviceRepository.RemoveAsync(device);
        await _uow.SaveChangesAsync();
        return true;
    }
}