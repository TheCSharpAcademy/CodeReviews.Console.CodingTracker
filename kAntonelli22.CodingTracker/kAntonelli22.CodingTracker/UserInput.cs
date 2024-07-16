using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker;
internal class UserInput()
{
    private static string CleanResponse(string? input)
    {
        while (true)
        {
            if (input == null)
                input = "";
            input = input.Trim();
            if (input.Length < 0)
            {
                Console.WriteLine("Please provide a valid response");
            }
            else if (input == "0")
            {
                Environment.Exit(0);
            }
            else
            {
                break;
            }
        }
        return input;
    }
    public static string CleanString(string? input)
    {
        input = UserInput.CleanResponse(input);
        return input;
    } // end of CleanResponse Method
    public static int CleanInt(string? input)
    {
        input = UserInput.CleanResponse(input);
        Int32.TryParse(input, out int output);
        return output;
    } // end of CleanResponse Method
} // end of UserInput Class
