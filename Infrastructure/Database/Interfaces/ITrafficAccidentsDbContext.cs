using Infrastructure.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Interfaces;

public interface ITrafficAccidentsDbContext
{
    DbSet<TrafficAccidentDataModel> TrafficAccidents { get; }
}