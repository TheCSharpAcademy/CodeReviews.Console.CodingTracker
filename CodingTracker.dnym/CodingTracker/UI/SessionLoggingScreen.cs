using CodingTracker.DataAccess;
using CodingTracker.Models;
using TCSAHelper.Console;

namespace CodingTracker.UI;

internal static class SessionLoggingScreen
{
    internal static Screen Get(IDataAccess dataAccess, CodingSession? codingSession = null)
    {
        // If codingSession is null, the screen is for creating a new session. Otherwise, it is for modifying an existing session.
        // The body is made to ask one of four questions, depending on which info has been received through the prompt handler.
        // The footer is made to give state-based hints about what to input.
        // A prompt handling action parses the user's input and calls the data access layer to insert/update the session.

        DateOnly? startDate = null;
        TimeOnly? startTime = null;
        DateOnly? endDate = null;
        TimeOnly? endTime = null;
        DateTime now = DateTime.UtcNow;

        var screen = new Screen(header: (_, _) => {
            if (codingSession == null)
            {
                return "Log Coding Session";
            }
            else
            {
                return "Modifying Coding Session";
            }
        }, body: (_, _) => {
            // The content of the body depends on which input part is currently being asked for. The ones already entered are repeated for convenience.
            var startDatePrompt = "At what date did you start coding? " + (startDate == null ? string.Empty : ((DateOnly)startDate).ToString(Program.mainDateFormat));
            var startTimePrompt = "At what time did you start coding? " + (startTime == null ? string.Empty : ((TimeOnly)startTime).ToString(Program.mainTimeFormat));
            var endDatePrompt = "At what date did you stop coding? " + (endDate == null ? string.Empty : ((DateOnly)endDate).ToString(Program.mainDateFormat));
            var endTimePrompt = "At what time did you stop coding? " + (endTime == null ? string.Empty : ((TimeOnly)endTime).ToString(Program.mainTimeFormat));

            if (startDate == null)
            {
                return $"{startDatePrompt}";
            }
            else if (startTime == null)
            {
                return $"{startDatePrompt}\n{startTimePrompt}";
            }
            else if (endDate == null)
            {
                return $"{startDatePrompt}\n{startTimePrompt}\n\n{endDatePrompt}";
            }
            else
            {
                return $"{startDatePrompt}\n{startTimePrompt}\n\n{endDatePrompt}\n{endTimePrompt}";
            }
        }, footer: (_, _) => {
            // Like the body, the footer depends on what has been input so far. Here, we build a string which has the correct hint information.
            now = DateTime.UtcNow;
            var hintStart = codingSession?.StartTime ?? now;
            var hintEnd = codingSession?.EndTime ?? now;

            string currentInput;
            string currentFormats;
            string currentData;
            void PrepareHint(string type, string[] formats, DateTime hintDateTime)
            {
                currentInput = type;
                currentFormats = string.Join(" or ", formats.Select(f => f.ToUpper()).ToArray());
                currentData = hintDateTime.ToLocalTime().ToString(type == "date" ? Program.mainDateFormat : Program.mainTimeFormat);
            }

            if (startDate == null)
            {
                PrepareHint("date", Program.dateFormats, hintStart);
            }
            else if (startTime == null)
            {
                PrepareHint("time", Program.timeFormats, hintStart);
            }
            else if (endDate == null)
            {
                PrepareHint("date", Program.dateFormats, hintEnd);
            }
            else
            {
                PrepareHint("time", Program.timeFormats, hintEnd);
            }

            if (endTime == null)
            {
                return @$"Input {currentInput} in the format {currentFormats},
or press [Enter] to use the {(codingSession == null ? "current" : "stored")} {currentInput}: {currentData}
Press [Esc] to cancel {(codingSession == null ? "insertion" : "modification")}.";
            }
            else
            {
                return $"Press [Esc] to cancel {(codingSession == null ? "insertion" : "modification")},\nor any other key to confirm.";
            }
        });

        void PromptHandler(string text)
        {
            // This function is called when the user presses [Enter] to submit their input. We can only tell what they were inputting by looking at what has already been input.
            // If the user has input an empty string, we use either the current time or the stored time, depending on whether we are creating or modifying a session.

            if (startDate == null)
            {
                ProcessDate(ref startDate, text, codingSession?.StartTime ?? now);
            }
            else if (startTime == null)
            {
                ProcessTime(ref startTime, text, codingSession?.StartTime ?? now);
            }
            else if (endDate == null)
            {
                ProcessDate(ref endDate, text, codingSession?.EndTime ?? now);
                if (endDate < startDate)
                {
                    Console.Beep();
                    endDate = null;
                }
            }
            else
            {
                ProcessTime(ref endTime, text, codingSession?.EndTime ?? now);
                if (endDate == startDate && endTime < startTime)
                {
                    Console.Beep();
                    endTime = null;
                }

                if (endTime != null)
                {
                    // If all necessary data has been input, insert or update the session.
                    var newSession = new CodingSession()
                    {
                        Id = codingSession?.Id ?? -1,
                        StartTime = CombineDateTime(startDate.Value, startTime.Value),
                        EndTime = CombineDateTime(endDate.Value, endTime.Value)
                    };
                    screen.SetPromptAction(null);
                    screen.SetAnyKeyAction(() =>
                    {
                        var overlappingSessions = dataAccess.CheckForOverlap(newSession);
                        if (codingSession != null)
                        {
                            overlappingSessions = overlappingSessions.Where(cs => cs.Id != codingSession.Id).ToList();
                        }
                        if (overlappingSessions.Any())
                        {
                            Console.Beep();
                            var errorScreen = new Screen(body: (_, _) => $"The session you are trying to {(codingSession == null ? "insert" : "modify")},\n\t{newSession.StartTime.ToLocalTime()} - {newSession.EndTime.ToLocalTime()},\n\n{(codingSession == null ? "" : "now ")}overlaps with the following session{(overlappingSessions.Count > 1 ? "s" : "")}:\n{string.Join("\n", overlappingSessions.Select(s => $"\t{s.StartTime.ToLocalTime()} - {s.EndTime.ToLocalTime()}"))}\n\nPress any key to cancel insertion and return to the main menu.");
                            errorScreen.SetAnyKeyAction(() => errorScreen.ExitScreen());
                            errorScreen.Show();
                            screen.ExitScreen();
                            return;
                        }
                        if (codingSession == null)
                        {
                            dataAccess.Insert(newSession);
                        }
                        else
                        {
                            dataAccess.Update(newSession);
                        }
                        screen.ExitScreen();
                    });
                }
            }
        }
        screen.SetPromptAction(PromptHandler);
        screen.AddAction(ConsoleKey.Escape, () => screen.ExitScreen());
        return screen;
    }

    private static DateTime CombineDateTime(DateOnly date, TimeOnly time)
    {
        return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second).ToUniversalTime();
    }

    private static void ProcessDate(ref DateOnly? output, string userInput, DateTime fallback)
    {
        if (string.IsNullOrEmpty(userInput))
        {
            output = DateOnly.FromDateTime(fallback.ToLocalTime());
        }
        else
        {
            output = ParseDateOnly(userInput, Program.dateFormats);
        }
    }

    private static void ProcessTime(ref TimeOnly? output, string userInput, DateTime fallback)
    {
        if (string.IsNullOrEmpty(userInput))
        {
            output = TimeOnly.FromDateTime(fallback.ToLocalTime());
        }
        else
        {
            output = ParseTimeOnly(userInput, Program.timeFormats);
        }
    }

    private static DateOnly? ParseDateOnly(string text, string[] formats)
    {
        foreach (var format in formats)
        {
            if (DateOnly.TryParseExact(text, format, out var date))
            {
                return date;
            }
        }
        Console.Beep();
        return null;
    }

    private static TimeOnly? ParseTimeOnly(string text, string[] formats)
    {
        foreach (var format in formats)
        {
            if (TimeOnly.TryParseExact(text, format, out var time))
            {
                return time;
            }
        }
        Console.Beep();
        return null;
    }
}
