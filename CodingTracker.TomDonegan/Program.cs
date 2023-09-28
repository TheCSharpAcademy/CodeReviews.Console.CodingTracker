
namespace CodingTracker.TomDonegan
{
    class Program
    {
        static void Main()
        {

            bool exit = false;

            Database.CreateSQLiteDatabase();

            while (!exit)
            {
                Console.Clear();
                UserInterface.MainMenu();
            }
        }
    }
}