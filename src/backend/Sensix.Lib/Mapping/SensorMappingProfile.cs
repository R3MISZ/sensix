using AutoMapper;
using Sensix.Lib.Dtos;
using Sensix.Lib.Entities;

public class SensorMappingProfile : Profile
{
    public SensorMappingProfile()
    {
        CreateMap<Sensor, SensorDto>();
        CreateMap<CreateSensorRequest, Sensor>()
            .ConstructUsing(src => new Sensor(src.DeviceId, src.Name, src.Type, src.Unit));

        CreateMap<UpdateSensorRequest, Sensor>()
            .AfterMap((src, dest) => dest.Update(src.Name, src.Type, src.Unit))
            .ForAllMembers(opts => opts.Ignore());
    }
}