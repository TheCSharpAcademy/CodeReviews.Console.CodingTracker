using Spectre.Console;
using System.Globalization;

namespace coding_tracker;

public class InputValidation
{
    internal bool TimeValidation(string userTimeInput)
    {
        if (!DateTime.TryParseExact(userTimeInput, "HH:mm:ss", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
        {
            AnsiConsole.Write(new Markup("\n[Red]Invalid time entered. Please try again.[/]\n\n"));
            return false;
        }
        return true;
    }

    internal bool DateValidation(string userDateInput)
    {
        if (!DateTime.TryParseExact(userDateInput, "dd-MM-yy", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
        {
            AnsiConsole.Write(new Markup("\n[Red]Invalid date entered. Please try again.[/]\n\n"));
            return false;
        }
        return true;
    }
    //internal bool DateTimeValidation(string userTimeInput)
    //{
    //    List<string> userInput = userTimeInput.Split(' ').ToList();
    //    while (!DateTime.TryParseExact(userTimeInput, "dd-MM-yy HH:mm", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
    //    {
    //        AnsiConsole.Clear();
    //        if (userInput.Count > 1)
    //        {
    //            if (!DateTime.TryParseExact(userInput[0], "dd-MM-yy", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
    //            {
    //                AnsiConsole.Write(new Markup("\n[Red]Invalid date entered. Please try again.[/]\n\n"));
    //                return false;
    //            }
    //            else if (!DateTime.TryParseExact(userInput[1], "HH:mm", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
    //            {
    //                AnsiConsole.Write(new Markup("\n[Red]Invalid time entered. Please try again.[/]\n\n"));
    //                return false;
    //            }
    //        }
    //        else
    //        {
    //            AnsiConsole.Write(new Markup("\n[red]Invalid entry made. Please try again.[/]\n\n"));
    //            return false;
    //        }

    //    }
    //    return true;
    //}

    internal string Duration(string sessionStart, string sessionEnd)
    {
        DateTime startTime = DateTime.ParseExact(sessionStart, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-GB"));
        DateTime endTime = DateTime.ParseExact(sessionEnd, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-GB"));

        TimeSpan duration = endTime.Subtract(startTime);
        return duration.TotalSeconds.ToString();
    }

    internal bool SessionIdInRange(int[] sessionIds, int idSelection)
    {
        if (sessionIds.Contains(idSelection))
        {
            AnsiConsole.Write(new Markup($"\n[red]ID: {idSelection} was found.[/]\n"));
            return true;
        }
        AnsiConsole.Write(new Markup($"[red]ID:{idSelection} was not found. Press any key to try again.[/]\n\n"));
        Console.ReadLine();
        return false;

    }

    internal static string DateTimeParse(string userInput, bool showDate = false, bool showTime = false)
    {
        DateTime parseInput = DateTime.Parse(userInput);

        if (showDate)
        {
            return parseInput.ToString("yyyy-MM-dd");
        }
        else if (showTime)
        {
            return parseInput.ToString("HH:mm:ss");
        }

        return parseInput.ToString("yyyy-MM-dd HH:mm:ss");
    }

    internal string GetTimeStamp()
    {
        DateTime timeStam = DateTime.Now;
        return timeStam.ToString("yyyy-MM-dd HH:mm:ss");
    }

    internal static string SecondsConversion(string secondsDuration)
    {
        int totalseconds = Convert.ToInt32(secondsDuration);
        int seconds = totalseconds % 60;
        int minutes = (totalseconds % 3600) / 60;
        int hours = totalseconds / 3600;
        string totalDuration = "";

        if (hours > 0)
        {
            return totalDuration = String.Format("{0:00}h {1:00}m {2:00}s", hours, minutes, seconds);
        }
        else if (minutes > 0)
        {
            return totalDuration = String.Format("00h {0:00}m {1:00}s", minutes, seconds);
        }
        else
        {
            return totalDuration = String.Format("00h 00m {0:00}s", seconds);
        }
    }
}
