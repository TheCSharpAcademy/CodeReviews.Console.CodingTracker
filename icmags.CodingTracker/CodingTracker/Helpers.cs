using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace CodingTracker
{
    internal static class Helpers
    {
        internal static DateTime GetDateTimeInput(string message)
        {
            DateTime convertedDateTime = new();
            string dateInput = AnsiConsole.Prompt(new TextPrompt<string>(message));

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out convertedDateTime))
            {
                dateInput = AnsiConsole.Prompt(new TextPrompt<string>("Invalid date and time. [green](Format: dd-mm-yy HH:mm:ss)[/]. Try again:"));
            }

            return convertedDateTime;
        }
        internal static TimeSpan CalculateDuration(DateTime startTime, DateTime endTime)
        {
            TimeSpan duration = endTime - startTime;
            return duration;
        }
        internal static int GetNumberInput(string message)
        {
            string numberInput = AnsiConsole.Prompt(new TextPrompt<string>(message));

            while (!Int32.TryParse(numberInput, out _))
            {
                numberInput = AnsiConsole.Prompt(new TextPrompt<string>("[red]Invalid number. Try again:[/]"));
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }
}
