internal class Program
{
    private static void Main(string[] args)
    {
        DatabaseManager databaseManager = new();
        GetUserInput getUserInput = new();

        databaseManager.CreateTable();
        getUserInput.MainMenu();
    }
}