using Microsoft.Extensions.Configuration;

namespace CodingTracker.kalsson.Configuration;

public static class ConfigurationHelper
{
    /// <summary>
    /// Builds and returns the application configuration.
    /// </summary>
    public static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        return builder.Build();
    }
}