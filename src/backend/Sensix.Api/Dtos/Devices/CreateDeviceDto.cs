using System.ComponentModel.DataAnnotations;

namespace Sensix.Api.Dtos.Devices;

public class CreateDeviceDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }

    [MaxLength(150)]
    public string? Location { get; set; }
}