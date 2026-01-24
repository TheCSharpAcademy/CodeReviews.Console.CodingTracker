using Microsoft.Extensions.Configuration;

namespace CodingTracker.Utils;

internal static class Config
{
    internal static string? InitializeConfig()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return configuration.GetConnectionString("DefaultConnection");
    }
}