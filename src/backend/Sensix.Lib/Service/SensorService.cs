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
    private readonly IUnitOfWork _dbRepository;
    private readonly ISensorRepository _sensorRepository;
    private readonly ILogger<ISensorService> _logger;

    public SensorService(IUnitOfWork dbRepository, ISensorRepository sensorRepository, ILogger<ISensorService> logger)
    {
        _dbRepository = dbRepository;
        _sensorRepository = sensorRepository;
        _logger = logger;
    }

    public async Task<SensorDto> CreateAsync(CreateSensorRequest request)
    {
        var sensor = new Sensor();

        sensor.SetDeviceId(request.DeviceId);

        sensor.SetName(request.Name);
        sensor.SetType(request.Type);
        sensor.SetUnit(request.Unit);

        if (request.IsActive is true) sensor.Activate();
        else if (request.IsActive is false) sensor.Deactivate();

        await _sensorRepository.AddAsync(sensor);
        await _dbRepository.SaveChangesAsync();

        return SensorMapping.ToResponse(sensor);
    }

    public async Task<IReadOnlyList<SensorDto>> GetAllAsync()
    {
        var sensors = await _sensorRepository.GetAllAsync();

        return SensorMapping.ToResponseList(sensors);
    }

    public async Task<SensorDto?> GetByIdAsync(Guid id)
    {
        var sensor = await _sensorRepository.GetByIdNoTrackingAsync(id);

        if (sensor is null)
        { 
            _logger.LogWarning("Sensor with id {SensorId} not found", id);
            return null;
        } 

        return SensorMapping.ToResponse(sensor);
    }

    public async Task<SensorDto?> UpdateAsync(Guid id, UpdateSensorRequest request)
    {
        var sensor = await _sensorRepository.GetByIdAsync(id);
        if (sensor is null) return null;

        if (request.DeviceId is not null)
            sensor.SetDeviceId(request.DeviceId.Value);

        if (request.Name is not null)
            sensor.SetName(request.Name);

        if (request.Type is not null)
            sensor.SetType(request.Type);

        if (request.Unit is not null)
            sensor.SetUnit(request.Unit);

        if (request.IsActive is true) sensor.Activate();
        else if (request.IsActive is false) sensor.Deactivate();

        await _dbRepository.SaveChangesAsync();
        return SensorMapping.ToResponse(sensor);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sensor = await _sensorRepository.GetByIdAsync(id);
        if (sensor is null)
            return false;

        await _sensorRepository.RemoveAsync(sensor);
        await _dbRepository.SaveChangesAsync();

        return true;
    }
}