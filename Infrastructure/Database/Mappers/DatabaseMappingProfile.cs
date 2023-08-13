using AutoMapper;
using Domain.Models;
using Infrastructure.Database.Entities;

namespace Infrastructure.Database.Mappers;

public class DatabaseMappingProfile : Profile
{
    public DatabaseMappingProfile()
    {
        CreateMap<TrafficAccident, TrafficAccidentDataModel>();
    }
}