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

    public Task<TrafficAccident?> GetTrafficAccidentByExternalId(string externalId)
        => _repository.GetTrafficAccidentByExternalIdAsync(externalId);
}