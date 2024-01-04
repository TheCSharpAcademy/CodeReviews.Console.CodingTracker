namespace Doc415.CodingTracker;

internal class Program
{
    static void Main(string[] args)
    {
        var DataAccess = new DataAccess();

        DataAccess.CreateDatabase();
        SeedData.SeedRecords(20);
        UserInterface.MainMenu();
    }
}
