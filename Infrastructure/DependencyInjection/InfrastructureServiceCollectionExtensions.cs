using Domain.Interfaces;

using Infrastructure.Database;
using Infrastructure.Database.Interfaces;
using Infrastructure.Database.Repositories;

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

        services.AddDbContext<ITrafficAccidentsDbContext, TrafficAccidentsDbContext>(options =>
            options.UseNpgsql(dataSource, o => o.UseNetTopologySuite()));

        services.AddTransient<ITrafficAccidentsRepository, TrafficAccidentsRepository>();

        return services;
    }
}