using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Timers;

namespace CodingTracker.Hacker_735
{
    internal class TimeInputController
    {
        static internal (string FinalTime, DateTime StartTime, DateTime EndTime) GetTimeInput()
        {
            DateTime parsedStartTime = AskForTime("start");
            DateTime parsedEndTime = AskForTime("end");

            int totalMinutes = ((parsedEndTime.Hour * 60) + parsedEndTime.Minute) - ((parsedStartTime.Hour * 60) + parsedStartTime.Minute);

            if (totalMinutes < 0)
            {
                totalMinutes += 24 * 60;
            }


            string finalTime = ($"{totalMinutes / 60:D2}:{totalMinutes % 60:D2}");
            

            AnsiConsole.MarkupLine($"Time passed was {finalTime}");

            return (finalTime, parsedStartTime, parsedEndTime);
        }


        internal static string GetDateInput()
        {
            AnsiConsole.MarkupLine("Enter Date (dd-mm-yy format):");
            string dateInput = Console.ReadLine();

            while (true)
            {
                if (dateInput == "td")
                    return DateTime.Today.ToString("dd-MM-yy");

                if (Validation.IsValidDate(dateInput, out string dateOutput))
                    return dateOutput;

                AnsiConsole.MarkupLine("Invalid format. Please use dd-mm-yy");
                dateInput = Console.ReadLine();
            }
        }

        private static DateTime AskForTime(string timeType)
        {
            while (true)
            {
                AnsiConsole.Markup($"Enter {timeType} time (HH:mm, 24-hour format): ");
                string? input = Console.ReadLine();

                if (Validation.IsValidTime(input, out DateTime output))
                    return output;

                AnsiConsole.MarkupLine("Invalid format. Please use 24-hour HH:mm (e.g. 14:30).");
            }
        }

    }
}
