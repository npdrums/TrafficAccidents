using Domain.Models;

namespace Domain.Interfaces;

public interface ITrafficAccidentsRepository
{
    Task<TrafficAccident?> CreateTrafficAccidentAsync(TrafficAccident trafficAccident);

    Task<TrafficAccident?> UpdateTrafficAccidentAsync(TrafficAccident trafficAccident);

    Task<TrafficAccident?> UpdateTrafficAccidentDescriptionAsync(Guid trafficAccidentId, string description);

    Task<IReadOnlyList<TrafficAccident?>> GetTrafficAccidentsByCaseNumberAsync(string caseNumber);

    Task<TrafficAccident?> GetTrafficAccidentsByIdAsync(Guid trafficAccidentId);

    Task DeleteTrafficAccidentAsync(Guid trafficAccidentId);
}