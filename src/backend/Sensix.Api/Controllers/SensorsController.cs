using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sensix.Lib.Dtos;
using Sensix.Lib.Service;

namespace Sensix.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SensorsController : ControllerBase
{
    private readonly ISensorService _sensorService;

    public SensorsController(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    [HttpPost]
    public async Task<ActionResult<SensorDto>> Create([FromBody] CreateSensorRequest request)
    {
        var result = await _sensorService.CreateAsync(request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SensorDto>>> ReadAll()
    {
        var result = await _sensorService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SensorDto>> ReadById([FromRoute] Guid id)
    {
        var result = await _sensorService.GetByIdAsync(id);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SensorDto>> Update(Guid id, [FromBody] UpdateSensorRequest request)
    {
        var result = await _sensorService.UpdateAsync(id, request);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var success = await _sensorService.DeleteAsync(id);
        if (success is false)
            return NotFound();
        return NoContent();
    }
}
