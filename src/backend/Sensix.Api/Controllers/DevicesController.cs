using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Sensix.Api.Dtos.Devices;
using Sensix.Api.Dtos.Common;

using Sensix.Infrastructure;

namespace Sensix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly SensixDbContext _db;
    private readonly IMapper _mapper;

    public DevicesController(SensixDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    // GET: /api/devices
    [HttpGet]
    public async Task<ActionResult<PagedResult<DeviceDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0 || pageSize > 500) pageSize = 50;

        var query = _db.Devices
            .Include(d => d.Sensors)
            .AsNoTracking();

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await query
            .OrderBy(d => d.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<DeviceDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(new PagedResult<DeviceDto>
        {
            Data = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        });
    }

    // GET: /api/devices/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeviceDto>> GetById(Guid id)
    {
        var device = await _db.Devices
            .Include(d => d.Sensors)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);

        if (device == null)
            return NotFound();

        return Ok(_mapper.Map<DeviceDto>(device));
    }

    // POST: /api/devices
    [HttpPost]
    public async Task<ActionResult<DeviceDto>> Create([FromBody] CreateDeviceDto dto)
    {
        var entity = _mapper.Map<Infrastructure.Entities.Device>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAtUtc = DateTime.UtcNow;

        _db.Devices.Add(entity);
        await _db.SaveChangesAsync();

        var result = _mapper.Map<DeviceDto>(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
    }

    // PUT: /api/devices/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DeviceDto>> Update(Guid id, [FromBody] UpdateDeviceDto dto)
    {
        var existing = await _db.Devices.FindAsync(id);
        if (existing == null)
            return NotFound();

        _mapper.Map(dto, existing);
        await _db.SaveChangesAsync();

        return Ok(_mapper.Map<DeviceDto>(existing));
    }

    // DELETE: /api/devices/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _db.Devices.FindAsync(id);
        if (existing == null)
            return NotFound();

        _db.Devices.Remove(existing);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}