
namespace CodingTracker.TomDonegan
{
    class Program
    {
        internal static void Main()
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