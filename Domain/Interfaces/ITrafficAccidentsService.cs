using Domain.Models;

namespace Domain.Interfaces;

public interface ITrafficAccidentsService
{
    Task<TrafficAccident?> CreateTrafficAccident(TrafficAccident trafficAccident);

    Task<TrafficAccident?> UpdateTrafficAccident(TrafficAccident trafficAccident);
    
    Task<TrafficAccident?> UpdateTrafficAccidentDescription(Guid trafficAccidentId, string description);

    Task<IReadOnlyList<TrafficAccident?>> GetTrafficAccidentsByCaseNumber(string caseNumber);

    Task<TrafficAccident?> GetTrafficAccidentsById(Guid trafficAccidentId);

    Task DeleteTrafficAccident(Guid trafficAccidentId);
}