using Domain.Models;

namespace Domain.Interfaces;

public interface ITrafficAccidentsRepository
{
    Task<TrafficAccident?> CreateTrafficAccidentAsync(TrafficAccident trafficAccident);

    Task<TrafficAccident?> GetTrafficAccidentByExternalIdAsync(string externalId);
}