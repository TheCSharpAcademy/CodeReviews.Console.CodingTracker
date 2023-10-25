using CodingTracker.DataAccess;
using CodingTracker.Models;
using ConsoleTableExt;
using TCSAHelper.Console;

namespace CodingTracker.UI;

internal static class LogManagementScreen
{
    internal static Screen Get(IDataAccess dataAccess)
    {
        // Session list screen. Since the screen header function is called first, it calculates the console height available for the list, and this value is used by the footer and body functions.
        const int headerHeight = 3;
        const int footerHeight = 5;// Actual height varies between 3-5, but we'll use a constant for regularity.
        const int promptHeight = 2;
        int[] listNumbersToIds = Array.Empty<int>();
        int lastListUsableHeight = 0;
        int listUsableHeight = 0;
        int skip = 0;

        var screen = new Screen(header: (_, usableHeight) =>
        {
            listUsableHeight = usableHeight - headerHeight - footerHeight - promptHeight;
            if (listUsableHeight != lastListUsableHeight)
            {
                // If the list height changes, we reset the skip (i.e. return to page 1) to keep it simple. What should otherwise happen is a bit unclear.
                skip = 0;
            }
            lastListUsableHeight = listUsableHeight;

            var currentPage = (skip / ListItemsPerPage(listUsableHeight)) + 1;
            var pages = (int)Math.Ceiling(dataAccess.Count() / (double)ListItemsPerPage(listUsableHeight));
            if (pages > 1)
            {
                return $"Coding Sessions (page {currentPage}/{pages})";
            }
            else
            {
                return "Coding Sessions";
            }
        }, body: (_, _) =>
        {
            const string prompt = "\nSelect a session to manage: ";
            if (dataAccess.Count() == 0)
            {
                return "There are no logged coding sessions yet.";
            }
            // TODO: Caching? Currently, we're getting all sessions every time we refresh the screen, which is suboptimal (but no problem with SQLite). But if we cache, we need to invalidate the cache when the user adds, modifies, or deletes a session.
            var sessions = dataAccess.GetAll(skip: skip, limit: ListItemsPerPage(listUsableHeight)).ToList();
            listNumbersToIds = sessions.ConvertAll(cs => cs.Id).ToArray();
            return MakeListString(sessions) + prompt;
        }, footer: (_, _) =>
        {
            const string escHint = "[Esc] to go back to the main menu.";
            var output = "Press ";
            if (skip > 0)
            {
                output += "[PgUp] to go to the previous page,\n";
            }
            if (dataAccess.Count() - skip > ListItemsPerPage(listUsableHeight))
            {
                output += "[PgDown] to go to the next page,\n";
            }
            if (skip > 0 || dataAccess.Count() - skip > ListItemsPerPage(listUsableHeight))
            {
                output += "or ";
            }
            output += escHint;
            return output;
        });

        void HandleUserInput(string text)
        {
            if (int.TryParse(text, out int result) && result >= 1 && result <= listNumbersToIds.Length)
            {
                var i = result - 1;
                var id = listNumbersToIds[i];
                var editScreen = SingleLogViewScreen.Get(dataAccess, id);
                editScreen.Show();
                if (dataAccess.Count() - skip <= 0)
                {
                    skip = Math.Max(0, skip - ListItemsPerPage(listUsableHeight));
                    if (dataAccess.Count() == 0)
                    {
                        screen.SetPromptAction(null);
                    }
                }
            }
            else
            {
                Console.Beep();
            }
        }
        screen.AddAction(ConsoleKey.Escape, () => screen.ExitScreen());
        screen.AddAction(ConsoleKey.PageUp, () =>
        {
            if (skip > 0)
            {
                skip = Math.Max(0, skip - ListItemsPerPage(listUsableHeight));
            }
        });
        screen.AddAction(ConsoleKey.PageDown, () =>
        {
            if (dataAccess.Count() - skip > ListItemsPerPage(listUsableHeight))
            {
                skip += ListItemsPerPage(listUsableHeight);
            }
        });
        if (dataAccess.Count() > 0)
        {
            screen.SetPromptAction(HandleUserInput);
        }
        return screen;
    }

    private static int ListItemsPerPage(int pageHeight)
    {
        // There's a top and a bottom and a divider between each item, and a header row.
        return (int)Math.Floor((pageHeight - 3d) / 2d);
    }

    private static string MakeListString(List<CodingSession> sessions)
    {
        var tableData = new List<List<object>>();
        for (int i = 0; i < sessions.Count; i++)
        {
            var session = sessions[i];
            tableData.Add(new List<object>
            {
                i + 1,
                session.StartTime.ToLocalTime().ToString(Program.mainFullFormat),
                session.EndTime.ToLocalTime().ToString(Program.mainFullFormat),
                DurationString(session.Duration),
            });
        }
        return ConsoleTableBuilder
            .From(tableData)
            .WithColumn("No.", "Start", "End", "Duration")
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .Export().ToString();
    }

    private static string DurationString(TimeSpan duration)
    {
        duration = TimeSpan.FromMinutes(Math.Round(duration.TotalMinutes));
        if (duration.TotalMinutes < 60)
        {
            return $"{duration.Minutes}m";
        }
        else
        {
            return $"{duration.Hours}h{duration.Minutes}m";
        }
    }
}
