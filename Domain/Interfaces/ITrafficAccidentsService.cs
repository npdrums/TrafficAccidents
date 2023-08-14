using Domain.Models;

namespace Domain.Interfaces;

public interface ITrafficAccidentsService
{
    Task<TrafficAccident?> CreateTrafficAccident(TrafficAccident trafficAccident);

    Task<TrafficAccident?> GetTrafficAccidentByExternalId(string externalId);
}