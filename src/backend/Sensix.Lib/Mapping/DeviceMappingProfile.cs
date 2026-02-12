using AutoMapper;
using Sensix.Lib.Dtos;
using Sensix.Lib.Entities;

namespace Sensix.Lib.Mapping;

public class DeviceMappingProfile : Profile
{
    public DeviceMappingProfile()
    {
        CreateMap<Device, DeviceDto>();
        CreateMap<CreateDeviceRequest, Device>()
            .ConstructUsing(src => new Device(src.Name, src.Location));

        CreateMap<UpdateDeviceRequest, Device>()
            .AfterMap((src, dest) => dest.Update(src.Name, src.Location))
            .ForAllMembers(opts => opts.Ignore());
    }
}