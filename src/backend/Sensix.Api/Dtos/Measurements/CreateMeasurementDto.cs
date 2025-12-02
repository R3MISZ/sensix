using System.ComponentModel.DataAnnotations;

namespace Sensix.Api.Dtos.Measurements;

public class CreateMeasurementDto
{
    [Required]
    public Guid SensorId { get; set; }

    [Required]
    [Range(-1_000_000, 1_000_000)]
    public double Value { get; set; }

    public DateTime? TimestampUtc { get; set; }

    [Required]
    [StringLength(16, MinimumLength = 1)]
    public string Unit { get; set; } = string.Empty;
}