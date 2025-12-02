using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Sensix.Api.Dtos.Common;
using Sensix.Api.Dtos.Sensors;

using Sensix.Infrastructure;

namespace Sensix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorsController : ControllerBase
{
    private readonly SensixDbContext _db;
    private readonly IMapper _mapper;

    public SensorsController(SensixDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    // GET: /api/sensors
    [HttpGet]
    public async Task<ActionResult<PagedResult<SensorDto>>> GetAll(
        [FromQuery] Guid? deviceId,
        [FromQuery] bool? isActive,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0 || pageSize > 500) pageSize = 50;

        var query = _db.Sensors
            .Include(s => s.Device)
            .AsNoTracking()
            .AsQueryable();

        if (deviceId.HasValue)
            query = query.Where(s => s.DeviceId == deviceId.Value);

        if (isActive.HasValue)
            query = query.Where(s => s.IsActive == isActive.Value);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await query
            .OrderBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<SensorDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(new PagedResult<SensorDto>
        {
            Data = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        });
    }

    // GET: /api/sensors/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SensorDto>> GetById(Guid id)
    {
        var sensor = await _db.Sensors
            .Include(s => s.Device)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sensor == null)
            return NotFound();

        return Ok(_mapper.Map<SensorDto>(sensor));
    }

    // POST: /api/sensors
    [HttpPost]
    public async Task<ActionResult<SensorDto>> Create([FromBody] CreateSensorDto dto)
    {
        // Sicherstellen, dass Device existiert
        var deviceExists = await _db.Devices.AnyAsync(d => d.Id == dto.DeviceId);
        if (!deviceExists)
            return BadRequest($"Device {dto.DeviceId} does not exist.");

        var entity = _mapper.Map<Infrastructure.Entities.Sensor>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAtUtc = DateTime.UtcNow;
        entity.IsActive = true;

        _db.Sensors.Add(entity);
        await _db.SaveChangesAsync();

        await _db.Entry(entity).Reference(s => s.Device).LoadAsync();

        var result = _mapper.Map<SensorDto>(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
    }

    // PUT: /api/sensors/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SensorDto>> Update(Guid id, [FromBody] UpdateSensorDto dto)
    {
        var existing = await _db.Sensors.Include(s => s.Device).FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null)
            return NotFound();

        _mapper.Map(dto, existing);
        await _db.SaveChangesAsync();

        var result = _mapper.Map<SensorDto>(existing);
        return Ok(result);
    }

    // DELETE: /api/sensors/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _db.Sensors.FindAsync(id);
        if (existing == null)
            return NotFound();

        _db.Sensors.Remove(existing);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
