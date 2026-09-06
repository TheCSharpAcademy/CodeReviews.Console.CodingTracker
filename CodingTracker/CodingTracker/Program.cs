using CodingTracker.Controllers;
using CodingTracker.View;

namespace CodingTracker
{
    internal class Program
    {
        static void Main()
        {
            var codingController = new CodingController();
            var userInterface = new UserInterface(codingController);

            userInterface.Menu();
        }
    }
}
