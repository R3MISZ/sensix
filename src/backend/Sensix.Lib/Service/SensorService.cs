using AutoMapper;
using Microsoft.Extensions.Logging;
using Sensix.Lib.Dtos;
using Sensix.Lib.Entities;
using Sensix.Lib.Mapping;
using Sensix.Lib.Repository;

namespace Sensix.Lib.Service;

public interface ISensorService
{
    Task<SensorDto> CreateAsync(CreateSensorRequest request);
    Task<IReadOnlyList<SensorDto>> GetAllAsync();
    Task<SensorDto?> GetByIdAsync(Guid id);
    Task<SensorDto?> UpdateAsync(Guid id, UpdateSensorRequest request);
    Task<bool> DeleteAsync(Guid id);
}

public class SensorService : ISensorService
{
    private readonly IUnitOfWork _uow;
    private readonly ISensorRepository _sensorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SensorService> _logger;

    public SensorService(IUnitOfWork uow, ISensorRepository sensorRepository, IMapper mapper, ILogger<SensorService> logger)
    {
        _uow = uow;
        _sensorRepository = sensorRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SensorDto> CreateAsync(CreateSensorRequest request)
    {
        var sensor = _mapper.Map<Sensor>(request);
        await _sensorRepository.AddAsync(sensor);
        await _uow.SaveChangesAsync();
        return _mapper.Map<SensorDto>(sensor);
    }

    public async Task<IReadOnlyList<SensorDto>> GetAllAsync()
    {
        var sensors = await _sensorRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyList<SensorDto>>(sensors);
    }

    public async Task<SensorDto?> GetByIdAsync(Guid id)
    {
        var sensor = await _sensorRepository.GetByIdNoTrackingAsync(id);
        return sensor is null ? null : _mapper.Map<SensorDto>(sensor);
    }

    public async Task<SensorDto?> UpdateAsync(Guid id, UpdateSensorRequest request)
    {
        var sensor = await _sensorRepository.GetByIdAsync(id);
        if (sensor is null) return null;

        // AutoMapper calls dest.Update(...) 
        _mapper.Map(request, sensor);

        await _uow.SaveChangesAsync();
        return _mapper.Map<SensorDto>(sensor);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sensor = await _sensorRepository.GetByIdAsync(id);
        if (sensor is null) return false;

        await _sensorRepository.RemoveAsync(sensor);
        await _uow.SaveChangesAsync();
        return true;
    }
}