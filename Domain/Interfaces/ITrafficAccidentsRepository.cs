using Domain.Models;

namespace Domain.Interfaces;

public interface ITrafficAccidentsRepository
{
    Task CreateTrafficAccidentAsync(TrafficAccident trafficAccident);
}