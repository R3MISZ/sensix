using System.ComponentModel.DataAnnotations;

namespace Sensix.Api.Dtos.Sensors;

public class CreateSensorDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Type { get; set; }

    [Required]
    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    [Required]
    public Guid DeviceId { get; set; }
}