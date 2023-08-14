using Infrastructure.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Interfaces;

public interface ITrafficAccidentsDbContext
{
    DbSet<TrafficAccidentDataModel> TrafficAccidents { get; }

    DbSet<MunicipalityDataModel> Municipalities { get; }

    DbSet<SettlementDataModel> Settlements { get; }

    DbSet<CityDataModel> Cities { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}