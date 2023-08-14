using AutoMapper;

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

    public async Task<TrafficAccident?> CreateTrafficAccidentAsync(TrafficAccident trafficAccident)
    {
        var trafficAccidentEntity = _mapper.Map<TrafficAccidentDataModel>(trafficAccident);

        // TODO: Try to evaluate will using IDbContextFactory with multiple DbContext instances be faster?

        //await using var municipalitiesUnitOfWork = await _dbContextFactory.CreateDbContextAsync();
        //await using var settlementsUnitOfWork = await _dbContextFactory.CreateDbContextAsync();
        //await using var citiesUnitOfWork = await _dbContextFactory.CreateDbContextAsync();

        var municipality = await _dbContext.Municipalities.Where(x =>
                x.MunicipalityArea.Intersects(trafficAccidentEntity.AccidentLocation))
            .OrderBy(x => x.MunicipalityId)
            .FirstOrDefaultAsync();
        var settlement = await
            _dbContext.Settlements.Where(x =>
                x.SettlementArea.Covers(trafficAccidentEntity.AccidentLocation)).FirstOrDefaultAsync();
        var city = await _dbContext.Cities.Where(x =>
            x.CityArea.Covers(trafficAccidentEntity.AccidentLocation)).FirstOrDefaultAsync();

        trafficAccidentEntity.Municipality ??= municipality;
        trafficAccidentEntity.Settlement ??= settlement;
        trafficAccidentEntity.City ??= city;

        var entityEntry = await _dbContext.TrafficAccidents.AddAsync(trafficAccidentEntity);
        var result = await _dbContext.SaveChangesAsync();

        return result > 0 ? _mapper.Map<TrafficAccident>(entityEntry.Entity) : default;
    }

    public async Task<TrafficAccident?> GetTrafficAccidentByExternalIdAsync(string externalId)
    {
        var trafficAccident = 
            await ApplySpecification(new TrafficAccidentByExternalIdSpecification(externalId))
            .FirstOrDefaultAsync();

        return _mapper.Map<TrafficAccident>(trafficAccident);
    }

    private IQueryable<TrafficAccidentDataModel> ApplySpecification(Specification<TrafficAccidentDataModel> specification)
    {
        return SpecificationEvaluator.GetQuery(_dbContext.TrafficAccidents, specification);
    }
}