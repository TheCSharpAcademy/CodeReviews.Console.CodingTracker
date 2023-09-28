using CodingTracker.rthring;

namespace CodingTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseController database = new DatabaseController();
            UserHandler handler = new UserHandler(database);
            Boolean closeApp = false;
            while (!closeApp)
            {
                closeApp = handler.GetUserInput();
            }
        }
    }
}