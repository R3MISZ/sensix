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
}

public class MeasurementRepository : IMeasurementRepository
{
    private readonly SensixDbContext _dbContext;

    public MeasurementRepository(SensixDbContext dbContext) => _dbContext = dbContext;

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
}
