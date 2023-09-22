
namespace CodingTracker.TomDonegan
{
    class Program
    {
        static void Main()
        {
            bool exit = false;

            Database.CreateSQLiteDatabase();
            UserInterface.WelcomeScreen();

            while (!exit)
            {
                Console.Clear();
                UserInterface.MainMenu();
            }
        }
    }
}