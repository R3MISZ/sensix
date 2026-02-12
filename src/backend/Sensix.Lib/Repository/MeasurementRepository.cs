using Microsoft.EntityFrameworkCore;
using Sensix.Lib.Database;
using Sensix.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensix.Lib.Repository;

public interface IMeasurementRepository
{
    Task AddAsync(Measurement measurement);
    Task<IReadOnlyList<Measurement>> GetAllAsync();
    Task<Measurement?> GetByIdAsync(Guid id);
    Task<Measurement?> GetByIdNoTrackingAsync(Guid id);
    Task RemoveAsync(Measurement measurement);

    Task<List<Measurement>> GetBySensorIdAsync(
        Guid sensorId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int limit,
        bool desc);
}

public class MeasurementRepository : IMeasurementRepository
{
    private readonly AppDbContext _dbContext;

    public MeasurementRepository(AppDbContext dbContext) => _dbContext = dbContext;

    #region CRUD Operations
    public async Task AddAsync(Measurement measurement)
    {
        await _dbContext.Measurements.AddAsync(measurement);
    }

    public async Task<IReadOnlyList<Measurement>> GetAllAsync()
    {
        return await _dbContext.Measurements
            .AsNoTracking()
            .Include(measurement => measurement.Sensor)
            .ToListAsync();
    }

    public async Task<Measurement?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Measurements
            .Include(measurement => measurement.Sensor)
            .FirstOrDefaultAsync(measurement => measurement.Id == id);
    }

    public async Task<Measurement?> GetByIdNoTrackingAsync(Guid id)
    {
        return await _dbContext.Measurements
            .AsNoTracking()
            .Include(measurement => measurement.Sensor)
            .FirstOrDefaultAsync(measurement => measurement.Id == id);
    }

    public Task RemoveAsync(Measurement measurement)
    {
        _dbContext.Measurements.Remove(measurement);
        return Task.CompletedTask;
    }
    #endregion

    public async Task<List<Measurement>> GetBySensorIdAsync(
        Guid sensorId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int limit,
        bool desc)
    {
        IQueryable<Measurement> query = _dbContext.Measurements.AsNoTracking()
            .Where(measurement => measurement.SensorId == sensorId);

        if (fromUtc.HasValue)
            query = query.Where(measurement => measurement.TimestampUtc >= fromUtc.Value);

        if (toUtc.HasValue)
            query = query.Where(measurement => measurement.TimestampUtc <= toUtc.Value);

        if (desc)
            query = query.OrderByDescending(measurement => measurement.TimestampUtc);
        else
            query = query.OrderBy(measurement => measurement.TimestampUtc);

        return await query.Take(limit).ToListAsync();
    }
}