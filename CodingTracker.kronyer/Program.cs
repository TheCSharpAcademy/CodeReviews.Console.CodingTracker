namespace CodingTracker;

public class CodingTracker
{
    public static void Main(string[] args)
    {
        var dataAcess = new DataAccess();
        dataAcess.CreateDatabase();


        //SeedData.SeedRecords(10);


        UInterface.MainMenu();
    }
}