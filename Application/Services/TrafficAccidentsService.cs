using Domain.Interfaces;
using Domain.Models;

namespace Application.Services;

public class TrafficAccidentsService : ITrafficAccidentsService
{
    private readonly ITrafficAccidentsRepository _repository;

    public TrafficAccidentsService(ITrafficAccidentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<TrafficAccident?> CreateTrafficAccident(TrafficAccident trafficAccident) 
        => await _repository.CreateTrafficAccidentAsync(trafficAccident);

    public async Task<IReadOnlyList<TrafficAccident?>> GetTrafficAccidentsByCaseNumber(string caseNumber)
        => await _repository.GetTrafficAccidentsByCaseNumberAsync(caseNumber);

    public async Task DeleteTrafficAccident(Guid externalId)
        => await _repository.DeleteTrafficAccidentAsync(externalId);
}