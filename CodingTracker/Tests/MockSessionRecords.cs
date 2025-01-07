using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

internal class MockSessionRecords
{
    internal static void CreateRandomRecords()
    {
        Console.Clear();
        var numberOfRecords = InputHelper.GetPositiveNumberInput("Enter a number of random records to generate:");

        int numberOfDays;
        if (numberOfRecords >= 3000) numberOfDays = Random.Shared.Next(1000, 2000);
        else if (numberOfRecords >= 2000) numberOfDays = Random.Shared.Next(500, 1000);
        else if (numberOfRecords >= 1000) numberOfDays = Random.Shared.Next(300, 500);
        else if (numberOfRecords >= 300) numberOfDays = Random.Shared.Next(100, 300);
        else numberOfDays = Random.Shared.Next(30, 100);

        var mockList = GenerateRecords(numberOfRecords, numberOfDays);

        PopulateDatabase(mockList);

        string record = (numberOfRecords == 1) ? "record" : "records";
        AnsiConsole.MarkupLine($"[green]New {numberOfRecords} {record} created successfully![/]");

        var setGoalDate = DateTime.Now.AddDays(Random.Shared.Next(-numberOfDays, 0));
        Goals.UpdateGoalData(1000, 4, setGoalDate);
        AnsiConsole.MarkupLine($"[cyan]New goal date {setGoalDate.ToString(Config.DateFormat)} set successfully![/]");

        var showRecords = DisplayInfoHelpers.GetYesNoAnswer("Do you want to see updated list of records?");
        if (showRecords) RecordRead.ShowAllRecords();
        else Console.Clear();
    }

    internal static void PopulateDatabase(List<MockCodingSession> records)
    {
        using var connection = new SqliteConnection(Config.ConnectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            var parameters = new DynamicParameters();
            foreach (var record in records)
            {
                parameters.Add("@start_time", record.StartTime);
                parameters.Add("@end_time", record.EndTime);
                parameters.Add("@duration", record.Duration);
                connection.Execute(@"
                    INSERT INTO sessions (start_time, end_time, duration)
                    VALUES(@start_time, @end_time, @duration)",
                    parameters, transaction);
            }
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            AnsiConsole.MarkupLine("[red]Failed to populate DB with mock records![/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
    }

    internal static List<MockCodingSession> GenerateRecords(int numberOfRecords, int numberOfDays)
    {
        var sessions = new List<MockCodingSession>();
        for (int i = 0; i < numberOfRecords; i++)
        {
            var startTime = DateTimeHelper.ConcatenateDateAndTime(
                DateTime.Now.AddDays(Random.Shared.Next(-numberOfDays, 0)),
                $"{Random.Shared.Next(7, 24):00}:{Random.Shared.Next(0, 60):00}");
            TimeSpan duration = TimeSpan.FromMinutes(Random.Shared.Next(1, 500));
            var endTime = startTime + duration;

            sessions.Add(new MockCodingSession
            {
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration
            });
        }
        sessions = sessions.OrderBy(s => s.StartTime).ToList();
        return sessions;
    }
}
