using Sensix.Lib.Dtos;
using Sensix.Lib.Entities;
using Sensix.Lib.Mapping;
using Sensix.Lib.Repository;

namespace Sensix.Lib.Service;

public interface IMeasurementService
{
    Task<MeasurementResponse> CreateAsync(CreateMeasurementRequest request);
    Task<IReadOnlyList<MeasurementResponse>> GetAllAsync();
    Task<MeasurementResponse?> GetByIdAsync(Guid id);
    Task<MeasurementResponse?> UpdateAsync(Guid id, UpdateMeasurementRequest request);
    Task<bool> DeleteAsync(Guid id);
}

public class MeasurementService : IMeasurementService
{
    private readonly IDbRepository _dbRepository;
    private readonly IMeasurementRepository _measurementRepository;

    public MeasurementService(IDbRepository dbRepository, IMeasurementRepository measurementRepository)
    {
        _dbRepository = dbRepository;
        _measurementRepository = measurementRepository;
    }

    #region CRUD Operations
    public async Task<MeasurementResponse> CreateAsync(CreateMeasurementRequest request)
    {
        var measurement = new Measurement();

        measurement.SensorId = request.SensorId;
        measurement.TimestampUtc = request.TimestampUtc;
        measurement.Value = request.Value;

        await _measurementRepository.AddAsync(measurement);
        await _dbRepository.SaveChangesAsync();

        return MeasurementMapping.ToResponse(measurement);
    }

    public async Task<IReadOnlyList<MeasurementResponse>> GetAllAsync()
    {
        var measurements = await _measurementRepository.GetAllAsync();

        return MeasurementMapping.ToResponseList(measurements);
    }

    public async Task<MeasurementResponse?> GetByIdAsync(Guid id)
    {
        var measurement = await _measurementRepository.GetByIdNoTrackingAsync(id);
        if (measurement is null) return null;

        return MeasurementMapping.ToResponse(measurement);
    }

    public async Task<MeasurementResponse?> UpdateAsync(Guid id, UpdateMeasurementRequest request)
    {
        var measurement = await _measurementRepository.GetByIdAsync(id);
        if (measurement is null) return null;

        if (request.SensorId is not null)
            measurement.SensorId = request.SensorId.Value;

        if (request.TimestampUtc is not null)
            measurement.TimestampUtc = request.TimestampUtc.Value;

        if (request.Value is not null)
            measurement.Value = request.Value.Value;

        await _dbRepository.SaveChangesAsync();
        return MeasurementMapping.ToResponse(measurement);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var measurement = await _measurementRepository.GetByIdAsync(id);
        if (measurement is null)
            return false;

        await _measurementRepository.RemoveAsync(measurement);
        await _dbRepository.SaveChangesAsync();

        return true;
    }
    #endregion

    public async Task<List<Measurement>> GetBySensorIdAsync(
        Guid sensorId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int limit,
        string order)
    {

        int clampedLimit = Math.Clamp(limit, 1, 5000);

        var desc = IsDesc(order);

        return await _measurementRepository.GetBySensorIdAsync(
            sensorId,
            fromUtc,
            toUtc,
            clampedLimit,
            desc);
    }

    private static bool IsDesc(string order)
        => !string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase);
}