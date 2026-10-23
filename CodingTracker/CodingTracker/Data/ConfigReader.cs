using Microsoft.Extensions.Configuration;

namespace CodingTracker.Data
{
    internal class ConfigReader
    {
        internal static string ReadConfig()
        {
            string? connectionString = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Key 'DefaultConnection' was not found.");

            return connectionString;
        }
    }
}
