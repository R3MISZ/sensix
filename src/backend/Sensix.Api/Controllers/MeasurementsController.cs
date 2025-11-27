using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sensix.Api.Dtos;
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

        var result = _mapper.Map<IEnumerable<MeasurementDto>>(items);
        return Ok(result);
    }

    // GET /api/measurements/latest?unit=...
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
        // [ApiController] kümmert sich um Model Validation (Required, Range, etc.)

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
