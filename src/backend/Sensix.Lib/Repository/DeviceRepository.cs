using Microsoft.EntityFrameworkCore;
using Sensix.Lib.Database;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Repository;

public interface IDeviceRepository
{
    Task AddAsync(Device device);
    Task<IReadOnlyList<Device>> GetAllAsync();
    Task<Device?> GetByIdAsync(Guid id);
    Task<Device?> GetByIdNoTrackingAsync(Guid id);
    Task RemoveAsync(Device device);
}

public class DeviceRepository : IDeviceRepository
{
    private readonly SensixDbContext _dbContext;

    public DeviceRepository(SensixDbContext dbContext) => _dbContext = dbContext;

    public async Task AddAsync(Device device)
    {
        await _dbContext.Devices.AddAsync(device);
    }

    public async Task<IReadOnlyList<Device>> GetAllAsync()
    {
        return await _dbContext.Devices
            .AsNoTracking()
            .Include(device => device.Sensors)
            .OrderBy(device => device.Name)
            .ToListAsync();
    }

    public async Task<Device?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Devices
            .Include(device => device.Sensors)
            .FirstOrDefaultAsync(device => device.Id == id);
    }

    public async Task<Device?> GetByIdNoTrackingAsync(Guid id)
    {
        return await _dbContext.Devices
            .AsNoTracking()
            .Include(device => device.Sensors)
            .FirstOrDefaultAsync(device => device.Id == id);
    }

    public Task RemoveAsync(Device device)
    {
        _dbContext.Devices.Remove(device);
        return Task.CompletedTask;
    }
}