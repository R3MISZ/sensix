using AutoMapper;
using Sensix.Lib.Dtos;
using Sensix.Lib.Entities;

public class MeasurementMappingProfile : Profile
{
    public MeasurementMappingProfile()
    {
        CreateMap<Measurement, MeasurementDto>();
        CreateMap<CreateMeasurementRequest, Measurement>()
            .ConstructUsing(src => new Measurement(src.SensorId, src.Value, src.TimestampUtc));
    }
}