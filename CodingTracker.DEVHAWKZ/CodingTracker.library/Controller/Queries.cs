namespace CodingTracker.library.Controller;

internal static class Queries
{
    private static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");

    internal static string ConnectionString { get { return connectionString; } }  
}
