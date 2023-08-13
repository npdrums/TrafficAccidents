using AutoMapper;

using Domain.Interfaces;
using Domain.Models;

using Infrastructure.Database.Entities;
using Infrastructure.Database.Interfaces;

namespace Infrastructure.Database.Repositories;

public class TrafficAccidentsRepository : ITrafficAccidentsRepository
{
    private readonly ITrafficAccidentsDbContext _dbContext;
    private readonly IMapper _mapper;

    public TrafficAccidentsRepository(ITrafficAccidentsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task CreateTrafficAccidentAsync(TrafficAccident trafficAccident)
    {
        var trafficAccidentEntity = _mapper.Map<TrafficAccidentDataModel>(trafficAccident);

        await _dbContext.TrafficAccidents.AddAsync(trafficAccidentEntity);
        await _dbContext.SaveChangesAsync();
    }
}