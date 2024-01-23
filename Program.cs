
namespace CodingTracker;
internal class Program
{
    private static void Main(string[] args)
    {
        var configReader = new ConfigReader();
        var database = new Database(configReader.GetConnectionString(),configReader.GetFileNameString());

    }
}