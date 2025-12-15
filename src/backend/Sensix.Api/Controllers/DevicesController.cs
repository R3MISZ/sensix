using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ActionResult<DeviceResponse>> Create([FromBody] CreateDeviceRequest request)
    {
        var result = await _deviceService.CreateDeviceAsync(request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DeviceResponse>>> GetAll()
    {
        var result = await _deviceService.ReadDevicesAsync();
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<DeviceResponse>> Update([FromBody] UpdateDeviceRequest request)
    {
        var result = await _deviceService.UpdateDeviceAsync(request);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var success = await _deviceService.DeleteDeviceAsync(id);
        if (success is false)
            return NotFound();
        return NoContent();
    }
}