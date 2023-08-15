using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Interfaces;

public interface ITrafficAccidentsDbContext
{
    DbSet<T> Set<T>() where T : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}