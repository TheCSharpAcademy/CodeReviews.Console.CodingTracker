using Spectre.Console;

namespace CodingTracker;

public class Report
{
    public static void ShowSessions(IEnumerable<CodingSession> sessions)
    {
        var table = new Table();
        table.Title("Your latest sessions");
        table.AddColumn("Id");
        table.AddColumn("Start");
        table.AddColumn("End");
        table.AddColumn("Duration");

        foreach (CodingSession session in sessions)
        {
            string end = session.EndTime.Equals(DateTime.MinValue) ? "-" : session.EndTime.ToString();
            string duration = session.Duration < TimeSpan.Zero ? "-" : session.Duration.TotalMinutes.ToString("N0");
            table.AddRow($"{session.Id}", $"{session.StartTime}", $"{end}", $"{duration,3} minutes");
        }
        AnsiConsole.Write(table);
    }

    public static void ShowAnalytics(SessionController sessionController)
    {
        var totalCodingTime = sessionController.GetTotalCodingTime().TotalMinutes.ToString("N0");
        AnsiConsole.Markup($"[blue]Total coding time:[/] [red]{totalCodingTime}[/]\n");
    }
}