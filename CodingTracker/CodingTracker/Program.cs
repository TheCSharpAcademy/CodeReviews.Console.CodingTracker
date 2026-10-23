using CodingTracker.Controllers;
using CodingTracker.Data;
using CodingTracker.UI;

namespace CodingTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseInitialiser.InitialiseDatabase();
            var app = new CodingTrackerController();
            app.Run();
            UserInput.Goodbye();
        }
    }
}
