using API.Contracts;

using AutoMapper;

using Domain.Models;
using NetTopologySuite.Geometries;

namespace API.Mappers;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<TrafficAccidentRequest, TrafficAccident>()
            .ForMember(dest => dest.AccidentLocation, opt => opt.MapFrom(x => new Point(x.Longitude, x.Latitude)));

        CreateMap<TrafficAccidentUpdateRequest, TrafficAccident>()
            .ForMember(dest => dest.AccidentLocation, opt => opt.MapFrom(x => new Point(x.Longitude, x.Latitude)));

        CreateMap<TrafficAccident, TrafficAccidentResponse>()
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.AccidentLocation.X))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.AccidentLocation.Y));
    }
}