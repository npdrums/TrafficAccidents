using Infrastructure.Database.Mappers;
using Infrastructure.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class IoCContainer
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddAutoMapper(x => x.AddProfile(new DatabaseMappingProfile()));

        services
            .AddDatabaseServices(configuration.GetConnectionString("DefaultConnection"));

        return services;
    }
}