using Microsoft.EntityFrameworkCore;
using Sensix.Lib.Database;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Repository;

public interface ISensorRepository
{
    Task AddAsync(Sensor sensor);
    Task<IReadOnlyList<Sensor>> GetAllAsync();
    Task<Sensor?> GetByIdAsync(Guid id);
    Task<Sensor?> GetByIdNoTrackingAsync(Guid id);
    Task RemoveAsync(Sensor sensor);
}

public class SensorRepository : ISensorRepository
{
    private readonly AppDbContext _dbContext;

    public SensorRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task AddAsync(Sensor sensor)
    {
        await _dbContext.Sensors.AddAsync(sensor);
    }

    public async Task<IReadOnlyList<Sensor>> GetAllAsync()
    {
        return await _dbContext.Sensors
            .AsNoTracking()
            .Include(sensor => sensor.Device)
            .Include(sensor => sensor.Measurements)
            .OrderBy(sensor => sensor.Name)
            .ToListAsync();
    }

    public async Task<Sensor?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Sensors
            .Include(sensor => sensor.Device)
            .Include(sensor => sensor.Measurements)
            .FirstOrDefaultAsync(sensor => sensor.Id == id);
    }

    public async Task<Sensor?> GetByIdNoTrackingAsync(Guid id)
    {
        return await _dbContext.Sensors
            .AsNoTracking()
            .Include(sensor => sensor.Device)
            .Include(sensor => sensor.Measurements)
            .FirstOrDefaultAsync(sensor => sensor.Id == id);
    }

    public Task RemoveAsync(Sensor sensor)
    {
        _dbContext.Sensors.Remove(sensor);
        return Task.CompletedTask;
    }
}