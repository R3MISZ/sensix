using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sensix.Infrastructure;          // for SensixDbContext
using Sensix.Infrastructure.Entities; // for Measurement

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

    // GET /api/measurements?fromUtc=...&toUtc=...&unit=...
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetAll(
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
    public async Task<ActionResult<Measurement>> GetLatest([FromQuery] string? unit)
    {
        var query = _db.Measurements.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(unit))
            query = query.Where(m => m.Unit == unit);

        var latest = await query
            .OrderByDescending(m => m.TimestampUtc)
            .FirstOrDefaultAsync();

        if (latest == null)
            return NotFound();

        return Ok(latest);
    }

    // GET /api/measurements/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Measurement>> GetById(Guid id)
    {
        var m = await _db.Measurements.FindAsync(id);
        if (m == null)
            return NotFound();

        return Ok(m);
    }

    // POST /api/measurements
    [HttpPost]
    public async Task<ActionResult<Measurement>> Create([FromBody] Measurement input)
    {
        if (input.Id == Guid.Empty)
            input.Id = Guid.NewGuid();

        if (input.TimestampUtc == default)
            input.TimestampUtc = DateTime.UtcNow;

        if (string.IsNullOrWhiteSpace(input.Unit))
            input.Unit = "UNKNOWN";

        _db.Measurements.Add(input);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    // PUT /api/measurements/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Measurement>> Update(Guid id, [FromBody] Measurement input)
    {
        if (id == Guid.Empty)
            return BadRequest("Id must not be empty.");

        var existing = await _db.Measurements.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Value = input.Value;

        if (input.TimestampUtc != default)
            existing.TimestampUtc = input.TimestampUtc;

        if (!string.IsNullOrWhiteSpace(input.Unit))
            existing.Unit = input.Unit;

        await _db.SaveChangesAsync();

        return Ok(existing);
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
