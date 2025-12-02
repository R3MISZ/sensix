using System.ComponentModel.DataAnnotations;

namespace Sensix.Api.Dtos.Sensors;

public class UpdateSensorDto
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(50)]
    public string? Type { get; set; }

    [MaxLength(20)]
    public string? Unit { get; set; }

    public bool? IsActive { get; set; }
}