using Domain.Models;

namespace Domain.Interfaces;

public interface ITrafficAccidentsService
{
    Task<TrafficAccident?> CreateTrafficAccident(TrafficAccident trafficAccident);

    Task<IReadOnlyList<TrafficAccident?>> GetTrafficAccidentsByCaseNumber(string caseNumber);

    Task DeleteTrafficAccident(Guid externalId);
}