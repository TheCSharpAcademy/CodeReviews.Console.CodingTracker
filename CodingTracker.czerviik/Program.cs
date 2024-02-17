namespace CodingTracker;
internal class Program
{
    private static void Main(string[] args)
    {
        var configReader = new ConfigReader();
        int consoleHeight = 40;
        int consoleWidth = 80;

        try
        {
            string connectionString = configReader.GetConnectionString();
            string fileName = configReader.GetFileNameString();

            var database = new Database(connectionString, fileName);

            Console.SetWindowSize(consoleWidth,consoleHeight);

            var menuManager = new MenuManager(database);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error:{ex.Message}");
            Environment.Exit(1);
        }
    }
}