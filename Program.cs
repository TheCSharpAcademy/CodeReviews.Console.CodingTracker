
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
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error:{ex.Message}");
            Environment.Exit(1);   
        }

        MenuManager menuManager = new MenuManager();

    }
}