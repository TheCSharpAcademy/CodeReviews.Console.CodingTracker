using Microsoft.Extensions.Configuration;

namespace CodingTracker;

internal static class AppConfig
{
    private static IConfiguration _configuration;

    static AppConfig()
    {
        BuildConfiguration();
    }

    private static void BuildConfiguration()
    {
        string configPath = Path.Combine(Directory.GetCurrentDirectory(), "config");
        var builder = new ConfigurationBuilder()
            .SetBasePath(configPath)
            .AddJsonFile( "appsettings.json", optional: true, reloadOnChange: true);
        
        _configuration = builder.Build();
    }
    
    internal static string GetConnectionString()
    {
        return _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }
}