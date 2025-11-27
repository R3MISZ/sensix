using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Sensix.Infrastructure;          // for SensixDbContext
using Sensix.Infrastructure.Entities; // for Measurement
using Sensix.Api.Dtos;

namespace Sensix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementsController : ControllerBase
{
    private readonly SensixDbContext _db;

    public MeasurementsController(SensixDbContext db)
    {
        _db = db;
    }

    private static MeasurementDto ToDto(Measurement m) => new MeasurementDto(m.Id, m.Value, m.TimestampUtc, m.Unit);

    // GET /api/measurements?fromUtc=...&toUtc=...&unit=...
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeasurementDto>>> GetAll(
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? toUtc,
        [FromQuery] string? unit)
    {
        var query = _db.Measurements.AsNoTracking().AsQueryable();

        if (fromUtc.HasValue)
            query = query.Where(m => m.TimestampUtc >= fromUtc.Value);

        if (toUtc.HasValue)
            query = query.Where(m => m.TimestampUtc <= toUtc.Value);

        if (!string.IsNullOrWhiteSpace(unit))
            query = query.Where(m => m.Unit == unit);

        var items = await query
            .OrderBy(m => m.TimestampUtc)
            .ToListAsync();

        return Ok(items);
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

        return Ok(ToDto(latest));
    }

    // GET /api/measurements/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MeasurementDto>> GetById(Guid id)
    {
        var m = await _db.Measurements.FindAsync(id);
        if (m == null)
            return NotFound();

        return Ok(m);
    }

    // POST /api/measurements
    [HttpPost]
    public async Task<ActionResult<MeasurementDto>> Create([FromBody] CreateMeasurementDto input)
    {
        var entity = new Measurement
        {
            Id = Guid.NewGuid(),
            Value = input.Value,
            TimestampUtc = input.TimestampUtc ?? DateTime.UtcNow,
            Unit = string.IsNullOrWhiteSpace(input.Unit) ? "UNKNOWN" : input.Unit
        };

        _db.Measurements.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, ToDto(entity));
    }

    // PUT /api/measurements/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MeasurementDto>> Update(Guid id, [FromBody] UpdateMeasurementDto dto)
    {
        if (id == Guid.Empty)
            return BadRequest("Id must not be empty.");

        var existing = await _db.Measurements.FindAsync(id);
        if (existing == null)
            return NotFound();

        if (dto.Value.HasValue)
            existing.Value = dto.Value.Value;

        if (dto.TimestampUtc.HasValue)
            existing.TimestampUtc = dto.TimestampUtc.Value;

        if (!string.IsNullOrWhiteSpace(dto.Unit))
            existing.Unit = dto.Unit;

        await _db.SaveChangesAsync();

        return Ok(ToDto(existing));
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
