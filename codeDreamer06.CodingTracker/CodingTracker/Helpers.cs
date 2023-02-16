using System.Globalization;
using ConsoleTableExt;

namespace CodingTracker;

public static class Extensions
{
    private static readonly Dictionary<HeaderCharMapPositions, char> HeaderCharacterMap = new()
    {
        { HeaderCharMapPositions.TopLeft, '╒' },
        { HeaderCharMapPositions.TopCenter, '╤' },
        { HeaderCharMapPositions.TopRight, '╕' },
        { HeaderCharMapPositions.BottomLeft, '╞' },
        { HeaderCharMapPositions.BottomCenter, '╪' },
        { HeaderCharMapPositions.BottomRight, '╡' },
        { HeaderCharMapPositions.BorderTop, '═' },
        { HeaderCharMapPositions.BorderRight, '│' },
        { HeaderCharMapPositions.BorderBottom, '═' },
        { HeaderCharMapPositions.BorderLeft, '│' },
        { HeaderCharMapPositions.Divider, '│' },
    };

    public static void DisplayTable<T>(this List<T> records, string emptyMessage) where T : class
    {
        if (records.Count == 0)
        {
            Console.WriteLine(emptyMessage);
            return;
        }

        ConsoleTableBuilder.From(records)
            .WithCharMapDefinition(CharMapDefinition.FramePipDefinition, HeaderCharacterMap)
            .ExportAndWriteLine();
    }

    private static string FormatErrorMessages(string command) => command switch
    {
        "add" => "Add commands should be in this format: 'add [duration]'. \nFor example: 'add 5:30' means 5 hours and 30 minutes." ,
        "remove" => "Remove commands should be in this format: 'remove [id]'. \nFor example: 'remove 3' deletes the third log.",
        "update" => "Update commands should be in this format: 'update [log id] [hours]''. \nFor example: 'update 3 8' changes the number of hours in row 3 to 8 hours.",
        _ => "An unknown error occurred while parsing your command."
    };

    public static int GetNumber(this string str, string keyword)
    {
        _ = int.TryParse(
              str.RemoveKeyword(keyword),
              NumberStyles.Any,
              NumberFormatInfo.InvariantInfo,
              out int number);

        return number;
    }

    public static string? RemoveKeyword(this string str, string keyword)
    {
        var errorMessage = FormatErrorMessages(keyword);

        try
        {
            return str.Replace(keyword + " ", "");
        }

        catch (FormatException)
        {
            if (!string.IsNullOrEmpty(errorMessage))
                Console.WriteLine(errorMessage);

            return string.Empty;
        }
    }

    public static TimeSpan? SplitTime(this string command, string keyword = "")
    {
        var errorMessage = FormatErrorMessages(keyword);

        try
        {
            var time = command.RemoveKeyword(keyword);
            if (string.IsNullOrEmpty(time)) return null;

            if (time.Contains(':'))
            {
                var splitTime = time.Split(":");
                return new TimeSpan(Convert.ToInt32(splitTime[0]), Convert.ToInt32(splitTime[1]), 0);
            }

            else return new TimeSpan(Convert.ToInt32(time), 0, 0);
        }

        catch (FormatException)
        {
            Console.WriteLine(errorMessage);
            return null;
        }
    }

    public static bool IsInvalidForUpdate(this string command)
    {
        if (command.Split().Length < 3)
        {
            Console.WriteLine(FormatErrorMessages("update"));
            return true;
        }

        return false;
    }

    public static bool IsDurationValid(this TimeSpan duration) => duration.TotalHours < 24;
}
