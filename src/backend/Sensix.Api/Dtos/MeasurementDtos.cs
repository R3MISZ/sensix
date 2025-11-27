namespace Sensix.Api.Dtos;

// Response obj
public record MeasurementDto(
    Guid Id,
    double Value,
    DateTime TimestampUtc,
    string Unit);

// for POST /api/measurements
public record CreateMeasurementDto(
    double Value,
    DateTime? TimestampUtc,
    string Unit);

// for PUT /api/measurements/{id}
public record UpdateMeasurementDto(
    double? Value,
    DateTime? TimestampUtc,
    string? Unit);