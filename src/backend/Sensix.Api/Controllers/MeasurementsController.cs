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

    // GET /api/measurements
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetAll()
    {
        var items = await _db.Measurements
            .AsNoTracking()
            .OrderBy(m => m.TimestampUtc)
            .ToListAsync();

        return Ok(items);
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
        // Auto-Id erstellen
        if (input.Id == Guid.Empty)
            input.Id = Guid.NewGuid();

        // Timestamp setzen falls nicht angegeben
        if (input.TimestampUtc == default)
            input.TimestampUtc = DateTime.UtcNow;

        // Unit defaulten falls leer
        if (string.IsNullOrWhiteSpace(input.Unit))
            input.Unit = "UNKNOWN";

        _db.Measurements.Add(input);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }
}
