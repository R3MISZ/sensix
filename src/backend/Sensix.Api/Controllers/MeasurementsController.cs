using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Sensix.Api.Dtos.Measurements;
using Sensix.Api.Dtos.Common;

using Sensix.Infrastructure;          // for SensixDbContext
using Sensix.Infrastructure.Entities; // for Measurement

namespace Sensix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementsController : ControllerBase
{
    private readonly SensixDbContext _db;
    private readonly IMapper _mapper;

    public MeasurementsController(SensixDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    // GET /api/measurements
    [HttpGet]
    public async Task<ActionResult<PagedResult<MeasurementDto>>> GetAll([FromQuery] FilterMeasurementDto filter)
    {
        var query = _db.Measurements
            .AsNoTracking()
            .Include(m => m.Sensor)
                .ThenInclude(s => s.Device)
            .AsQueryable();

        // Filters
        if (filter.DeviceId.HasValue)
            query = query.Where(m => m.Sensor.DeviceId == filter.DeviceId.Value);

        if (filter.SensorId.HasValue)
            query = query.Where(m => m.SensorId == filter.SensorId.Value);

        if (filter.FromUtc.HasValue)
            query = query.Where(m => m.TimestampUtc >= filter.FromUtc.Value);


        // Count before paging
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = query.OrderBy(m => m.TimestampUtc);

        // Paging
        var skip = (filter.Page - 1) * filter.PageSize;

        var items = await query
            .Skip(skip)
            .Take(filter.PageSize)
            .ToListAsync();

        var mapped = _mapper.Map<IEnumerable<MeasurementDto>>(items);

        // Build response
        var result = new PagedResult<MeasurementDto>
        {
            Data = mapped,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        };

        return Ok(result);
    }

    // GET /api/measurements/latest
    [HttpGet("latest")]
    public async Task<ActionResult<MeasurementDto>> GetLatest([FromQuery] string? unit)
    {
        var query = _db.Measurements.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(unit))
            query = query.Where(m => m.Unit == unit);

        var latest = await query
            .OrderByDescending(m => m.TimestampUtc)
            .FirstOrDefaultAsync();

        if (latest == null)
            return NotFound();

        return Ok(_mapper.Map<MeasurementDto>(latest));
    }

    // GET /api/measurements/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MeasurementDto>> GetById(Guid id)
    {
        var m = await _db.Measurements.FindAsync(id);
        if (m == null)
            return NotFound();

        return Ok(_mapper.Map<MeasurementDto>(m));
    }

    // POST /api/measurements
    [HttpPost]
    public async Task<ActionResult<MeasurementDto>> Create([FromBody] CreateMeasurementDto dto)
    {
        // Existiert der Sensor überhaupt?
        var sensorExists = await _db.Sensors.AnyAsync(s => s.Id == dto.SensorId);
        //if (!sensorExists)
        //    return BadRequest($"Sensor with id {dto.SensorId} does not exist.");

        var entity = _mapper.Map<Measurement>(dto);
        entity.Id = Guid.NewGuid();

        _db.Measurements.Add(entity);
        await _db.SaveChangesAsync();

        var result = _mapper.Map<MeasurementDto>(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
    }

    // PUT /api/measurements/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MeasurementDto>> Update(
        Guid id,
        [FromBody] UpdateMeasurementDto dto)
    {
        if (id == Guid.Empty)
            return BadRequest("Id must not be empty.");

        var existing = await _db.Measurements.FindAsync(id);
        if (existing == null)
            return NotFound();

        // Nur nicht-null Felder aus dto werden gemappt (per AutoMapper-Config)
        _mapper.Map(dto, existing);

        await _db.SaveChangesAsync();

        var result = _mapper.Map<MeasurementDto>(existing);
        return Ok(result);
    }

    // DELETE /api/measurements/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Id must not be empty.");

        var existing = await _db.Measurements.FindAsync(id);
        if (existing == null)
            return NotFound();

        _db.Measurements.Remove(existing);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
