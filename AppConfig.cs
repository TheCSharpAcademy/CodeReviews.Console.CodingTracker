using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace CodingTracker;

public class AppConfig
{
    private static IConfiguration _configuration;

    static AppConfig()
    {
        BuildConfiguration();
    }

    private static void BuildConfiguration()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "configs");
        var builder = new ConfigurationBuilder()
            .SetBasePath(configPath)
            .AddJsonFile( "appsettings.json", optional: true, reloadOnChange: true);
        
        _configuration = builder.Build();
    }
    
    public static string GetConnectionString(string name = "DefaultConnection")
    {
        return _configuration.GetConnectionString(name) ?? string.Empty;
    }
}