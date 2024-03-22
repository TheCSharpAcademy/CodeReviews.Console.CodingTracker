using DbHelpers.HopelessCoding;
using Spectre.Console;

namespace ValidationChecks.HopelessCoding;

internal class Validations
{
    internal static int GetValidIdFromUser(string message)
    {
        Console.Write($"Please enter the ID of the record which you want to {message}: ");
        int inputId;
        if (!int.TryParse(Console.ReadLine(), out inputId))
        {
            return -1;
        }
        return inputId;
    }

    internal static bool ValidateId(int id)
    {
        if (id == -1)
        {
            AnsiConsole.Write(new Markup($"[red]\nInvalid input. Input must be a valid integer ID.\n[/]"));
            Console.WriteLine("----------------------------");
            return false;
        }

        if (!DatabaseHelpers.IdExists(id))
        {
            AnsiConsole.Write(new Markup($"[red]\nRecord with the ID={id} does not exists.\n[/]"));
            Console.WriteLine("----------------------------");
            return false;
        }
        return true;
    }

    internal static string ValidateTime(string timing)
    {
        string currentDateTime = DateTime.Now.ToString(@"yyyy-MM-dd HH\:mm");

        DateTime dateTime;

        while (true)
        {
            Console.Write($"Please enter the {timing} date and time (YYYY-MM-DD hh:mm format) or press Enter to use current date and time: ");
            string userInputTime = Console.ReadLine();

            // Use the current date and time if the user doesn't provide one
            string setTime = string.IsNullOrEmpty(userInputTime) ? currentDateTime : userInputTime;
            if (string.IsNullOrEmpty(userInputTime))
            {
                AnsiConsole.Write(new Markup($"[yellow1]Using current time. Current time is: {currentDateTime}[/]\n"));
            }

            if (DateTime.TryParseExact(setTime, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out _))
            {
                return setTime;
            }
            else
            {
                AnsiConsole.Write(new Markup("[red]Invalid input. Please give time in valid format.[/]\n"));
            }
        }
    }

    internal static string ValidateEndTimeNotBeforeStartTime(string startTime)
    {
        string endTime = Validations.ValidateTime("end");

        while (DateTime.Parse(endTime) < DateTime.Parse(startTime))
        {
            AnsiConsole.Write(new Markup($"[red]End time cannot be before start time. Please enter a valid end time.[/]\n"));
            endTime = Validations.ValidateTime("end");
        }
        return endTime;
    }
}

