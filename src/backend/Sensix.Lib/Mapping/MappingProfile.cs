using AutoMapper;
using Sensix.Lib.Dtos;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Device Mappings
        CreateMap<Device, DeviceDto>();
        CreateMap<CreateDeviceRequest, Device>();
        // Prevents overwriting existing data with NULL
        CreateMap<UpdateDeviceRequest, Device>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Sensor Mappings
        CreateMap<Sensor, SensorDto>();
        CreateMap<CreateSensorRequest, Sensor>();

        // Measurement Mappings
        CreateMap<Measurement, MeasurementDto>();
        CreateMap<CreateMeasurementRequest, Measurement>();
    }
}