using Spectre.Console;

namespace CodingTracker;

public class UserInput
{
    internal static string? GetDate()
    {
        Views.PromptUser("date", "(dd/mm/yy)", "yellow");
        bool isValid;

        string? input = Console.ReadLine();

        if (input.ToLower() == "q")
        {
            Controller.Run();
        }

        try
        {
            do
            {
                isValid = Validate.ValidateDate(input);

                if (!isValid)
                {
                    input = Console.ReadLine();
                }
                else
                {
                    isValid = true;
                }
            } while (!isValid);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return input;
    }

    internal static string? GetTime(string message)
    {
        Views.PromptUser(message, "(h:mm tt)", "yellow");
        bool isValid;

        string? input = Console.ReadLine();

        if (input.ToLower() == "q")
        {
            Controller.Run();
        }

        try
        {
            do
            {
                isValid = Validate.ValidateTime(input);

                if (!isValid)
                {
                    input = Console.ReadLine();
                }
                else
                {
                    isValid = true;
                }
            } while (!isValid);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return input;
    }

    internal static string? GetId()
    {
        AnsiConsole.MarkupInterpolated($"Input [yellow]Id[/]. Press [yellow]Q[/] to cancel\n");

        bool isValid;

        string? input = Console.ReadLine();

        if (input.ToLower() == "q")
        {
            Controller.Run();
        }

        try
        {
            do
            {
                isValid = Validate.ValidateNumber(input);

                if (!isValid)
                {
                    input = Console.ReadLine();
                }
                else
                {
                    isValid = true;
                }
            } while (!isValid);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return input;
    }

    internal static string? GetNumber()
    {
        bool isValid;

        string? input = Console.ReadLine();

        if (input.ToLower() == "q")
        {
            Controller.Run();
        }

        try
        {
            do
            {
                isValid = Validate.ValidateNumber(input);

                if (!isValid)
                {
                    input = Console.ReadLine();
                }
                else
                {
                    isValid = true;
                }
            } while (!isValid);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return input;
    }

    internal static string? GetUpdatedValue(string column)
    {
        string? value = "";

        switch (column)
        {
            case "Id":
                value = GetId();
                break;
            case "Date":
                value = GetDate();
                break;
            case "StartTime":
                value = GetTime("start time");
                break;
            case "EndTime":
                value = GetTime("end time");
                break;
            default:
                break;
        }

        return value;
    }
}
