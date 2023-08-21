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

    public async Task<TrafficAccident?> UpdateTrafficAccident(TrafficAccident trafficAccident) 
        => await _repository.UpdateTrafficAccidentAsync(trafficAccident);

    public async Task<TrafficAccident?> UpdateTrafficAccidentDescription(Guid trafficAccidentId, string description)
    {
        // TODO: Validate text size?
        return await _repository.UpdateTrafficAccidentDescriptionAsync(trafficAccidentId, description);
    }

    public async Task<IReadOnlyList<TrafficAccident?>> GetTrafficAccidentsByCaseNumber(string caseNumber)
        => await _repository.GetTrafficAccidentsByCaseNumberAsync(caseNumber);

    public async Task<TrafficAccident?> GetTrafficAccidentsById(Guid trafficAccidentId)
        => await _repository.GetTrafficAccidentsByIdAsync(trafficAccidentId);

    public async Task DeleteTrafficAccident(Guid trafficAccidentId)
        => await _repository.DeleteTrafficAccidentAsync(trafficAccidentId);
}