using Spectre.Console;

public class ValidateInput
{
    public int GetValidInt(string prompt, int min, int max)
    {
        int result;
        bool validInput;
        do
        {
            result = AnsiConsole.Ask<int>(prompt);
            validInput = result >= min && result <= max;
            if (!validInput)
            {
                AnsiConsole.Markup($"[red]Invalid input, try again. Value must be between {min} and {max}.[/]\n");
            }
        } while (!validInput);

        return result;
    }

    public TimeSpan GetValidTime(string prompt)
    {
        TimeSpan result;
        bool validInput;
        do
        {
            string input = AnsiConsole.Ask<string>(prompt);
            validInput = TimeSpan.TryParse(input, out result) && result.TotalSeconds > 0 && CountDelimiters(input, ':') == 2 && AreComponentsTwoDigits(input);
            if (!validInput)
            {
                AnsiConsole.Markup($"[red]Invalid input, try again. Please input a valid time[/]\n");
            }
        } while (!validInput);

        return result;
    }

    public bool ValidateSessionDates(DateTime startDate, DateTime endDate)
    {
        return endDate > startDate;
    }

    private int CountDelimiters(string input, char delimiter)
    {
        return input.Count(c => c == delimiter);
    }

    private bool AreComponentsTwoDigits(string input)
    {
        var components = input.Split(':');
        return components.All(c => c.Length == 2);
    }
}