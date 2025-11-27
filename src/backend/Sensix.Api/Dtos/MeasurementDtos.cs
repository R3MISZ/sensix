using System.ComponentModel.DataAnnotations;

namespace Sensix.Api.Dtos;

// Response obj (only output, no validation needed)
public record MeasurementDto(
    Guid Id,
    double Value,
    DateTime TimestampUtc,
    string Unit);

public class CreateMeasurementDto
{
    [Required]
    [Range(-1_000_000, 1_000_000)]
    public double Value { get; set; }

    public DateTime? TimestampUtc { get; set; }

    [Required]
    [StringLength(16, MinimumLength = 1)]
    public string Unit { get; set; } = string.Empty;
}

public class UpdateMeasurementDto
{
    [Range(-1_000_000, 1_000_000)]
    public double? Value { get; set; }

    public DateTime? TimestampUtc { get; set; }

    [StringLength(16, MinimumLength = 1)]
    public string? Unit { get; set; }
}

public class MeasurementFilterDto : PagingQuery
{
    public DateTime? FromUtc { get; set; }
    public DateTime? ToUtc { get; set; }

    public string? Unit { get; set; }
}