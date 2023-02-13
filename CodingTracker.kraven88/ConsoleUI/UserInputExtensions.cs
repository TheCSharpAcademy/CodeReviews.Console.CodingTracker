namespace CodingTracker.kraven88.ConsoleUI;

public static class UserInputExtensions
{
    public static string ValidateDate(this string input, string format)
    {
        var output = "";
        while (String.IsNullOrWhiteSpace(output))
        {
            if (DateOnly.TryParseExact(input, format, out var date))
            {
                output = date.ToString(format);
            }
            else
            {
                Console.Write("Invalid input. Please try again: ");
                input = Console.ReadLine()!.Trim().ValidateDate(format);
            } 
        }
        return output;
    }

    public static string ValidateTime(this string input, string format)
    {
        var output = "";
        while (String.IsNullOrWhiteSpace(output))
        {
            if (TimeOnly.TryParseExact(input, format, out var time))
            {
                output = time.ToString(format);
            }
            else
            {
                Console.Write("Invalid input. Please try again: ");
                input = Console.ReadLine()!.Trim().ValidateTime(format);
            } 
        }

        return output;
    }
}
