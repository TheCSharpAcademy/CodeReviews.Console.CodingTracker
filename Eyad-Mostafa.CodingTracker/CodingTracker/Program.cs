namespace CodingTracker;
class Program
{
    static void Main(string[] args)
    {
        DatabaseManager.CreataDatabase();
        Menu.ShowMenu();
    }

}