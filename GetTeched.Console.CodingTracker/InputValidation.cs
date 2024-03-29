using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker;

public class InputValidation
{
    internal bool DateValidation(string userDateInput, string inputType)
    {
        while (!DateTime.TryParseExact(userDateInput, "dd-MM-yy", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Markup("\n[Red]Invalid date entered. Please try again.[/]\n\n"));
            return false;
        }
        return true;
    }

    internal bool TimeValidation(string userTimeInput, string inputType)
    {
        while (!DateTime.TryParseExact(userTimeInput, "HH:mm", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Markup("\n[Red]Invalid time entered. Please try again.[/]\n\n"));
            return false;
        }
        return true;
    }
}
