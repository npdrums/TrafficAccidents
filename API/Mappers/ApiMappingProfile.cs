using API.Contracts;

using AutoMapper;

using Domain.Models;

namespace API.Mappers;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<TrafficAccidentRequest, TrafficAccident>()
            .ForCtorParam("longitude", opt => opt.MapFrom(src => src.Longitude))
            .ForCtorParam("latitude", opt => opt.MapFrom(src => src.Latitude));

        CreateMap<TrafficAccident, TrafficAccidentResponse>()
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.AccidentLocation.X))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.AccidentLocation.Y));
    }
}