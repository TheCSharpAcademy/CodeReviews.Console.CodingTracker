
namespace CodingTracker;
internal class Program
{
    private static void Main(string[] args)
    {
        var configReader = new ConfigReader();

        try
        {
            string connectionString = configReader.GetConnectionString();
            string fileName = configReader.GetFileNameString();

            var database = new Database(connectionString, fileName);
            var menuManager = new MenuManager(database);

        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error:{ex.Message}");
            Environment.Exit(1);
        }
    }
}