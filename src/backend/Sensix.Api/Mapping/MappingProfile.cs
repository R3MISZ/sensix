using AutoMapper;
using Sensix.Infrastructure.Entities;

using Sensix.Api.Dtos.Devices;
using Sensix.Api.Dtos.Measurements;
using Sensix.Api.Dtos.Sensors;

namespace Sensix.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Device
        CreateMap<Device, DeviceDto>()
            .ForMember(d => d.SensorCount,
                opt => opt.MapFrom(src => src.Sensors.Count));

        CreateMap<CreateDeviceDto, Device>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAtUtc, opt => opt.Ignore());

        CreateMap<UpdateDeviceDto, Device>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));

        // Sensor
        CreateMap<Sensor, SensorDto>()
            .ForMember(d => d.DeviceName,
                opt => opt.MapFrom(src => src.Device.Name));

        CreateMap<CreateSensorDto, Sensor>()
            .ForMember(s => s.Id, opt => opt.Ignore())
            .ForMember(s => s.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(s => s.IsActive, opt => opt.MapFrom(_ => true));

        CreateMap<UpdateSensorDto, Sensor>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));

        // Measurement
        CreateMap<Measurement, MeasurementDto>()
            .ForMember(d => d.SensorName,
                opt => opt.MapFrom(src => src.Sensor.Name))
            .ForMember(d => d.DeviceId,
                opt => opt.MapFrom(src => src.Sensor.DeviceId))
            .ForMember(d => d.DeviceName,
                opt => opt.MapFrom(src => src.Sensor.Device.Name));

        CreateMap<CreateMeasurementDto, Measurement>()
            .ForMember(m => m.Id, opt => opt.Ignore())
            .ForMember(m => m.TimestampUtc,
                opt => opt.MapFrom(src => src.TimestampUtc ?? DateTime.UtcNow))
            .ForMember(m => m.Unit,
                opt => opt.MapFrom(src => src.Unit ?? string.Empty));

        CreateMap<UpdateMeasurementDto, Measurement>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
