using CodingTracker.Chad1082.Data;

namespace CodingTracker.Chad1082
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database.SetupDB();

            Menu mainMenu = new Menu();

            mainMenu.ShowMainMenu();
        }
    }
}