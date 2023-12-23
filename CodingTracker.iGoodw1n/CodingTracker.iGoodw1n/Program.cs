using static System.Configuration.ConfigurationManager;

namespace CodingTracker.iGoodw1n;

internal static class Program
{
    private static readonly string dbPath = AppSettings["dbPath"]  ?? "CodingTracker.db";
    private static readonly string connectionString = AppSettings["connectionString"]  ?? $"Data Source={dbPath}";
    static void Main(string[] args)
    {
        CodingController controller = new(connectionString);
        controller.Start();
    }
}