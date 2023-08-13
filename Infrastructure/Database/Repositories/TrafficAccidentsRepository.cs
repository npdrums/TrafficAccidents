using AutoMapper;
using AutoMapper.QueryableExtensions;

using Domain.Interfaces;
using Domain.Models;

using Infrastructure.Database.Entities;
using Infrastructure.Database.Interfaces;
using Infrastructure.Database.Specification;

using Microsoft.EntityFrameworkCore;

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

    public async Task<TrafficAccident?> GetByExternalId(string externalId)
    {
        return await ApplySpecification(new TrafficAccidentByExternalIdSpecification(externalId))
            .ProjectTo<TrafficAccident>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    private IQueryable<TrafficAccidentDataModel> ApplySpecification(
        Specification<TrafficAccidentDataModel> specification)
    {
        return SpecificationEvaluator.GetQuery(_dbContext.TrafficAccidents, specification);
    }
}