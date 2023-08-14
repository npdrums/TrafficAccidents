using AutoMapper;

using Domain.Models;

using Infrastructure.Database.Entities;

namespace Infrastructure.Database.Mappers;

public class DatabaseMappingProfile : Profile
{
    public DatabaseMappingProfile()
    {
        CreateMap<TrafficAccident, TrafficAccidentDataModel>();

        CreateMap<TrafficAccidentDataModel, TrafficAccident>()
            .ForCtorParam("municipalityName", opt => opt.MapFrom(x => x.Municipality!.MunicipalityName))
            .ForCtorParam("settlementName", opt => opt.MapFrom(x => x.Settlement!.SettlementName))
            .ForCtorParam("cityName", opt => opt.MapFrom(x => x.City!.CityName));


    }
}