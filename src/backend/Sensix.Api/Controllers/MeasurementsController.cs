using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sensix.Lib.Dtos;
using Sensix.Lib.Service;

namespace Sensix.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MeasurementsController : ControllerBase
{
    private readonly IMeasurementService _measurementService;

    public MeasurementsController(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
    }

    [HttpPost]
    public async Task<ActionResult<MeasurementDto>> Create([FromBody] CreateMeasurementRequest request)
    {
        var result = await _measurementService.CreateAsync(request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MeasurementDto>>> ReadAll()
    {
        var result = await _measurementService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MeasurementDto>> ReadById([FromRoute] Guid id)
    {
        var result = await _measurementService.GetByIdAsync(id);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var success = await _measurementService.DeleteAsync(id);
        if (success is false)
            return NotFound();
        return NoContent();
    }
}
