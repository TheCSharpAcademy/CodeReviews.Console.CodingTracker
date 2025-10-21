using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace CodingTracker
{
    internal static class Validation
    {
        internal static bool ValidateTimeSpan(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime)
            {
                AnsiConsole.MarkupLine($"[red]Error: Start Time is later than End Time. Returning to main menu[/]");
                return false;
            }
            return true;
        }
        internal static bool ValidateOverlappingTime(CodingSession session)
        {
            if (session != null)
            {
                AnsiConsole.MarkupLine($"[red]Error: Session is overlapping with existing session(s). Returning to main menu[/]");
                return false;
            }
            return true;
        }
    }
}
