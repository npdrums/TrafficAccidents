using Domain.Models;

namespace Domain.Interfaces;

public interface ITrafficAccidentsRepository
{
    Task<TrafficAccident?> CreateTrafficAccidentAsync(TrafficAccident trafficAccident);

    Task<IReadOnlyList<TrafficAccident?>> GetTrafficAccidentsByCaseNumberAsync(string caseNumber);

    Task DeleteTrafficAccidentAsync(Guid externalId);
}