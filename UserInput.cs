using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker
{
    public static class UserInput
    {
        public static string TimeInput()
        {
            do
            {
                string? userInput = Console.ReadLine();

                if (userInput != null)
                {
                    try
                    {
                        return DateTime.Parse(userInput).ToString("HH:mm");
                    }
                    catch
                    {
                        Console.WriteLine("Enter a valid time (HH:mm):\n");
                    }
                }
            } while (true);


        }
    }
}
