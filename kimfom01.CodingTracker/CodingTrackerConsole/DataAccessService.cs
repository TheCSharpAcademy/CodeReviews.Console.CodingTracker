namespace CodingTrackerConsole;

public class DataAccessService
{
    public static List<CodingTrackerModel> LoadData()
    {
        var databaseManager = new DatabaseManager();

        return databaseManager.ReadFromDb();
    }
}
