using Microsoft.Extensions.Configuration;

namespace DataSeedingTool.Configuration;

public static class ConfigurationFactory
{
    public static IConfiguration CreateConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{GetEnvironment()}.json", optional: true)
            .Build();

    private static string GetEnvironment() => Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
}