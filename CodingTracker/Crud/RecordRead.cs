using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

internal class RecordRead
{
    public static List<CodingSession> GetListOfAllSessions(string orderBy = "DESC")
    {
        using var connection = new SqliteConnection(Config.ConnectionString);
        orderBy = orderBy.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? "ASC" : "DESC";
        var query = @$"
        SELECT
            id AS Id,
            start_time AS StartTime,
            end_time AS EndTime,
            duration AS Duration
        FROM sessions
        ORDER BY start_time {orderBy}";
        return connection.Query<CodingSession>(query).ToList();
    }

    internal static bool CheckIfNoRecordsAvailable<T>(IEnumerable<T> collection)
    {
        bool ifNoRecords = false;
        if (!collection.Any())
        {
            ifNoRecords = true;
            AnsiConsole.MarkupLine("[red]No records found.[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
        return ifNoRecords;
    }

    internal static void ShowAllRecords()
    {
        Console.Clear();
        if (!DisplayRecordsByOrder("ASC")) return;
        if (!ViewOrder.ChooseOrder()) return;
    }

    internal static bool DisplayRecordsByOrder(string order)
    {
        var sessions = GetListOfAllSessions(orderBy: order);
        if (CheckIfNoRecordsAvailable(sessions)) return false;
        var records = MakeListOfAllRecords(sessions);

        foreach (var record in records)
        {
            AnsiConsole.MarkupLine(record);
        }

        AnsiConsole.MarkupLine($"\nTotal number of records: [green]{records.Count}[/]\n");
        return true;
    }

    internal static Dictionary<string, CodingSession> MakeRecordsMap()
    {
        var sessions = GetListOfAllSessions();
        var records = MakeListOfAllRecords(sessions);
        var recordsMap = new Dictionary<string, CodingSession>();

        for (int i = 0; i < records.Count; i++)
        {
            recordsMap.Add(records[i], sessions[i]);
        }
        return recordsMap;
    }

    internal static List<string> MakeListOfAllRecords(List<CodingSession> sessions)
    {
        var tableData = new List<string>();
        foreach (var session in sessions)
        {
            tableData.Add($"" +
                $"[lightseagreen]{DateTime.Parse(s: session.StartTime ?? "").ToString(Config.DateFormat)}[/]: " +
                $"[indianred1_1]{DateTime.Parse(s: session.StartTime ?? "").ToString(Config.TimeFormat)}[/]-" +
                $"[indianred1]{DateTime.Parse(s: session.EndTime ?? "").ToString(Config.TimeFormat)}[/] => " +
                $"[chartreuse3_1]{SessionDuration.GetTotalSessionDurationInfo(session.Duration)}[/]" +
                $"[{Console.BackgroundColor}] =>id:{session.Id}[/]");
        }
        return tableData;
    }
}
