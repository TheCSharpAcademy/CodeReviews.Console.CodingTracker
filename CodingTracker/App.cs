using System.Configuration;

public class App
{
    private DatabaseManager _databaseManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["CodingTrackerDB"].ConnectionString);

    public void Run()
    {
        _databaseManager.InitializeDatabase();

        Console.ReadKey();
    }
}