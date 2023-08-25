

using System.Configuration;

namespace CodingTracker.library.Controller;

internal static class Queries
{
    private static string connectionString = ConfigurationManager.AppSettings.Get("connectionString");

    internal static string ConnectionString { get; }
}
