using AutoMapper;

using Domain.Enums;
using Domain.Models;

using Infrastructure.Database.Entities;
using Infrastructure.Database.Entities.Enums;

namespace Infrastructure.Database.Mappers;

public class DatabaseMappingProfile : Profile
{
    public DatabaseMappingProfile()
    {
        CreateMap<TrafficAccident, TrafficAccidentDataModel>();

        CreateMap<TrafficAccidentDataModel, TrafficAccident>()
            .ForMember(x => x.MunicipalityName, opt => opt.MapFrom(x => x.Municipality!.MunicipalityName))
            .ForMember(x => x.SettlementName, opt => opt.MapFrom(x => x.Settlement!.SettlementName))
            .ForMember(x => x.CityName, opt => opt.MapFrom(x => x.City!.CityName));

        CreateMap<DataAccidentType, AccidentType>();
        CreateMap<DataParticipantsNominalCount, ParticipantsNominalCount>();
        CreateMap<DataParticipantsStatus, ParticipantsStatus>();
    }
}