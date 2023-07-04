using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.alvaromosconi.ConsoleUI
{
    internal static class UserInput
    {
        internal static string SelectMenuOption(string OPTIONS)
        {
            Console.Write("Select: ");
            var selectedOption = Console.ReadLine();

            if (!OPTIONS.Contains(selectedOption) || selectedOption.Length > 1)
            {
                Console.WriteLine("Invalid option. Please select a valid option.\n");

                selectedOption = SelectMenuOption(OPTIONS);
            }

            return selectedOption;
        }
    }
}
