using AutoMapper;
using Domain.Enums;
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

    public async Task<TrafficAccident?> UpdateTrafficAccidentAsync(TrafficAccident trafficAccident)
    {
        var trafficAccidentEntity = _mapper.Map<TrafficAccidentDataModel>(trafficAccident);

        var entityEntry = _dbContext.Set<TrafficAccidentDataModel>()
            .Update(trafficAccidentEntity);
        
        var result = await _dbContext.SaveChangesAsync();

        return result > 0 ? _mapper.Map<TrafficAccident>(entityEntry.Entity) : default;
    }

    public async Task<TrafficAccident?> UpdateTrafficAccidentDescriptionAsync(Guid trafficAccidentId, string description)
    {
        var trafficAccidentEntity = await ApplySpecification(
                new TrafficAccidentByIdSpecification(trafficAccidentId))
            .FirstOrDefaultAsync();

        if (trafficAccidentEntity is not null) trafficAccidentEntity.Description = description;

        var result = await _dbContext.SaveChangesAsync();

        return result > 0 ? _mapper.Map<TrafficAccident>(trafficAccidentEntity) : default;
    }

    public async Task<IReadOnlyList<TrafficAccident?>> GetTrafficAccidentsByCaseNumberAsync(string caseNumber)
    {
        var trafficAccident = await ApplySpecification(
            new TrafficAccidentByCaseNumberWithIncludesSpecification(caseNumber))
            .ToListAsync();

        return _mapper.Map<IReadOnlyList<TrafficAccident>>(trafficAccident);
    }

    public async Task<TrafficAccident?> GetTrafficAccidentsByIdAsync(Guid trafficAccidentId)
    {
        var trafficAccident = await ApplySpecification(
                new TrafficAccidentByIdWithIncludesSpecification(trafficAccidentId))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return _mapper.Map<TrafficAccident>(trafficAccident);
    }

    public async Task DeleteTrafficAccidentAsync(Guid trafficAccidentId)
    {
        var trafficAccident = await ApplySpecification(
            new TrafficAccidentByIdSpecification(trafficAccidentId))
            .FirstOrDefaultAsync();

        if (trafficAccident is not null) trafficAccident.IsDeleted = true;

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<AccidentType>> GetAccidentTypes()
    {
        var accidentTypes = await _dbContext.Set<TrafficAccidentDataModel>()
            .Select(x => x.AccidentType)
            .Distinct().ToListAsync();

        return _mapper.Map<IReadOnlyList<AccidentType>>(accidentTypes);
    }

    public async Task<IReadOnlyList<ParticipantsStatus>> GetParticipantsStatuses()
    {
        var participantsStatus = await _dbContext.Set<TrafficAccidentDataModel>()
            .Select(x => x.ParticipantsStatus)
            .Distinct().ToListAsync();

        return _mapper.Map<IReadOnlyList<ParticipantsStatus>>(participantsStatus);
    }

    public async Task<IReadOnlyList<ParticipantsNominalCount>> GetParticipantsNominalCounts()
    {
        var participantsNominalCounts = await _dbContext.Set<TrafficAccidentDataModel>()
            .Select(x => x.ParticipantsNominalCount)
            .Distinct().ToListAsync();

        return _mapper.Map<IReadOnlyList<ParticipantsNominalCount>>(participantsNominalCounts);
    }

    private IQueryable<T> ApplySpecification<T>(Specification<T> specification) where T : class 
        => SpecificationEvaluator.GetQuery(_dbContext.Set<T>(), specification);
}