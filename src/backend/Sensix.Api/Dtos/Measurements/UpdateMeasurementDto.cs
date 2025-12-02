using System.ComponentModel.DataAnnotations;

namespace Sensix.Api.Dtos.Measurements;

public class UpdateMeasurementDto
{
    [Range(-1_000_000, 1_000_000)]
    public double? Value { get; set; }

    public DateTime? TimestampUtc { get; set; }

    [StringLength(16, MinimumLength = 1)]
    public string? Unit { get; set; }
}