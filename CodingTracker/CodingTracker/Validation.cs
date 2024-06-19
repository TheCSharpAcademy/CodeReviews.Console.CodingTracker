using Spectre.Console;
using System;
namespace CodingTracker
{
    public class Validation
    {
        int first;
        public int ValidString(string start)
        {
            if (Int32.TryParse(start, out int a))
            {
                first = a;
            }
            else
            {
                AnsiConsole.Markup("[underline red]Try again![/]");
                ValidString(Console.ReadLine());
            }
            return a;
        }
    }
}
