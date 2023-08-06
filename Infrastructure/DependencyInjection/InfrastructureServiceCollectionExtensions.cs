using Infrastructure.Database;
using Infrastructure.Database.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

namespace Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNetTopologySuite();
        var dataSource = dataSourceBuilder.Build();

        return services.AddDbContext<ITrafficAccidentsDbContext, TrafficAccidentsDbContext>(options =>
            options.UseNpgsql(dataSource, o => o.UseNetTopologySuite()));
    }
}