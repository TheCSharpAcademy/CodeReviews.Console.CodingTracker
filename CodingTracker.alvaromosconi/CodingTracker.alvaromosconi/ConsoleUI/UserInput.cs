using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.alvaromosconi.ConsoleUI
{
    internal static class UserInput
    {
        internal static char SelectMenuOption(string OPTIONS)
        {
            Console.Write("Select: ");
            var selectedOption = Console.ReadKey().KeyChar;

            if (!OPTIONS.Contains(selectedOption))
            {
                Console.WriteLine("Invalid option. Please select a valid option.\n");

                selectedOption = SelectMenuOption(OPTIONS);
            }

            return selectedOption;
        }
    }
}
