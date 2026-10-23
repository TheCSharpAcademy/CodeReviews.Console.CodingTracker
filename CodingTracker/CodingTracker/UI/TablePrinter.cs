using CodingTracker.Enums;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.UI
{
    internal class TablePrinter
    {
        internal static void PrintSessionsTable(List<CodingSession> sessions)
        {
            UserInput.ClearDisplay();

            SortOrderOptions order = UserInput.GetOrderOption();
            List<CodingSession> orderedSessions = SortSessions(sessions, order);

            Table sessionsTable = new Table()
                .Title("[blue bold]Tracked Sessions[/]")
                .AddColumn("[lightgreen bold]ID[/]")
                .AddColumn("[lightgreen bold]StartTime[/]")
                .AddColumn("[lightgreen bold]EndTime[/]")
                .AddColumn("[lightgreen bold]Duration[/]")
                .BorderColor(Color.Grey50);

            foreach (var session in orderedSessions)
            {
                sessionsTable.AddRow(
                    session.ID.ToString(),
                    Markup.Escape(session.StartTime),
                    Markup.Escape(session.EndTime),
                    session.Duration);
            }

            AnsiConsole.Write(sessionsTable);
        }

        internal static List<CodingSession> SortSessions(List<CodingSession> sessions, SortOrderOptions order)
        {

            if (order == SortOrderOptions.Ascending)
            {
                return sessions.OrderBy(session => session.EndTime).ToList();
            }
            else
            {
                return sessions.OrderByDescending(session => session.EndTime).ToList();
            }
        }
    }
}
