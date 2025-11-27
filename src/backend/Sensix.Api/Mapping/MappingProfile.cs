using AutoMapper;
using Sensix.Api.Dtos;
using Sensix.Infrastructure.Entities;

namespace Sensix.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity -> DTO
        CreateMap<Measurement, MeasurementDto>();

        // CreateDto -> Entity
        CreateMap<CreateMeasurementDto, Measurement>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TimestampUtc,
                opt => opt.MapFrom(src => src.TimestampUtc ?? DateTime.UtcNow));

        // UpdateDto -> Entity (overwrite only non-null fields)
        CreateMap<UpdateMeasurementDto, Measurement>()
           .ForAllMembers(opt =>
               opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}