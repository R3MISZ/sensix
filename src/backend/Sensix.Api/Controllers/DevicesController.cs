using Microsoft.AspNetCore.Mvc;
using Sensix.Lib.Dtos;
using Sensix.Lib.Service;

namespace Sensix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    public async Task<ActionResult<DeviceDto>> Create([FromBody] CreateDeviceRequest request)
    {
        var result = await _deviceService.CreateAsync(request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DeviceDto>>> ReadAll()
    {
        var result = await _deviceService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeviceDto>> ReadById([FromRoute] Guid id)
    {
        var result = await _deviceService.GetByIdAsync(id);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DeviceDto>> Update(Guid id, [FromBody] UpdateDeviceRequest request)
    {
        var result = await _deviceService.UpdateAsync(id, request);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var success = await _deviceService.DeleteAsync(id);
        if (success is false)
            return NotFound();
        return NoContent();
    }
}