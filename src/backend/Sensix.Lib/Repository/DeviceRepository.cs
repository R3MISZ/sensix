using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sensix.Lib.Database;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Repository;

public interface IDeviceRepository
{
    Task AddAsync(Device device);
    Task<IReadOnlyList<Device>> GetAllAsync();
    Task<Device?> GetByIdAsync(Guid id);
    Task RemoveAsync(Device device);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class DeviceRepository : IDeviceRepository
{
    private readonly SensixDbContext _dbContext;

    public DeviceRepository(SensixDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Device device)
    {
        await _dbContext.Devices.AddAsync(device);
    }

    public async Task<IReadOnlyList<Device>> GetAllAsync()
    {
        return await _dbContext.Devices
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<Device?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Devices
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public Task RemoveAsync(Device device)
    {
        _dbContext.Devices.Remove(device);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}
