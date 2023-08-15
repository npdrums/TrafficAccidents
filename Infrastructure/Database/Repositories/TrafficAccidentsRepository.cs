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
       
        var municipality = await ApplySpecification(
            new MunicipalityAreaCoversSpecification(trafficAccidentEntity.AccidentLocation))
            .FirstOrDefaultAsync();
        var settlement = await ApplySpecification(
            new SettlementAreaCoversSpecification(trafficAccident.AccidentLocation))
            .FirstOrDefaultAsync();
        var city = await ApplySpecification(
            new CityAreaCoversSpecification(trafficAccident.AccidentLocation))
            .FirstOrDefaultAsync();

        trafficAccidentEntity.Municipality ??= municipality;
        trafficAccidentEntity.Settlement ??= settlement;
        trafficAccidentEntity.City ??= city;

        var entityEntry = await _dbContext.Set<TrafficAccidentDataModel>().AddAsync(trafficAccidentEntity);
        var result = await _dbContext.SaveChangesAsync();

        return result > 0 ? _mapper.Map<TrafficAccident>(entityEntry.Entity) : default;
    }

    public async Task<IReadOnlyList<TrafficAccident?>> GetTrafficAccidentsByCaseNumberAsync(string caseNumber)
    {
        var trafficAccident = await ApplySpecification(
            new TrafficAccidentByCaseNumberWithIncludesSpecification(caseNumber))
            .ToListAsync();

        return _mapper.Map<IReadOnlyList<TrafficAccident>>(trafficAccident);
    }

    public async Task DeleteTrafficAccidentAsync(Guid externalId)
    {
        var trafficAccidents = await ApplySpecification(
            new TrafficAccidentByExternalIdSpecification(externalId))
            .FirstOrDefaultAsync();

        if (trafficAccidents is not null) trafficAccidents.IsDeleted = true;

        await _dbContext.SaveChangesAsync();
    }

    private IQueryable<T> ApplySpecification<T>(Specification<T> specification) where T : class 
        => SpecificationEvaluator.GetQuery(_dbContext.Set<T>(), specification);
}