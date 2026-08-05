namespace CodingTracker;

using Microsoft.Data.Sqlite;
using Dapper;
using System;
using System.Diagnostics;
using Spectre.Console;

public class DatabaseConnector(string connectionString, string defaultDate, string defaultTime, string tableName)
{
    UserInput userInput = new UserInput(defaultDate, defaultTime);
    public void ShowAllRecords()
    {

        List<CodingSession> sessions = CreateList();

        PrintList(sessions);

        return;
    }

    public void AddRecord()
    {
        TimeOnly startTime_timeOnly = TimeOnly.FromDateTime(DateTime.Now);
        string startTime = startTime_timeOnly.ToString($"{defaultTime}");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine("Session started, the date is taken automatically, press ENTER to end it at any moment.");
        string? input = Console.ReadLine();

        stopwatch.Stop();

        TimeOnly endTime_timeOnly = startTime_timeOnly.Add(stopwatch.Elapsed);
        string endTime = endTime_timeOnly.ToString($"{defaultTime}");

        TimeSpan duration_timeSpan = endTime_timeOnly - startTime_timeOnly;
        string duration = duration_timeSpan.ToString(@"hh\:mm\:ss");

        insertRecord(DateTime.Now.ToString($"{defaultDate}"), startTime, endTime, duration);

        return;
    }

    public void AddRecordManually()
    {
        Console.WriteLine($"Enter the date with format -> \"{defaultDate}\":");
        string date = userInput.DateInput();


        Console.WriteLine($"\nEnter when you started the coding session with format -> \"{defaultTime}\":");
        TimeOnly startTime_timeOnly = userInput.HourInput();


        Console.WriteLine($"\nEnter when you finished the coding session with format -> \"{defaultTime}\" (on smaller hour input it's assumed you finished the next day):");
        TimeOnly endTime_timeOnly = userInput.HourInput();

        TimeSpan duration_timeSpan = endTime_timeOnly - startTime_timeOnly;

        string startTime = startTime_timeOnly.ToString($"{defaultTime}");
        string endTime = endTime_timeOnly.ToString($"{defaultTime}");
        string duration = duration_timeSpan.ToString(@"hh\:mm\:ss");

        insertRecord(date, startTime, endTime, duration);

        return;
    }

    public void DeleteSession()
    {
        ShowAllRecords();
        int id = CheckIDExists("\nEnter the ID number of the record you want to delete (enter -1 to go back to the menu):");
        if (id == -1) return;

        using var connection = new SqliteConnection($"{connectionString}");

        connection.Execute(
            $"DELETE FROM {tableName} WHERE id = @Id",
            new
            {
                Id = id
            }
        );

        Console.WriteLine("\nRecord deleted successfully.");
    }

    public void UpdateSession()
    {
        ShowAllRecords();
        int id = CheckIDExists("\nEnter the ID number of the record you want to update (enter -1 to go back to the menu):");

        if (id == -1) return;

        Console.WriteLine($"\nUpdate when you started the coding session with format -> \"{defaultTime}\":");
        TimeOnly startTime = userInput.HourInput();


        Console.WriteLine($"Update when you finished the coding session with format -> \"{defaultTime}\" (on smaller hour input it's assumed you finished the next day):");
        TimeOnly finishTime = userInput.HourInput();

        TimeSpan duration = finishTime - startTime;

        using var connection = new SqliteConnection(connectionString);

        connection.Execute(
            $"UPDATE {tableName} SET StartTime = @startTime, EndTime = @finishTime, Duration = @duration WHERE id = @Id",
            new
            {
                Id = id,
                startTime = startTime.ToString($"{defaultTime}"),
                finishTime = finishTime.ToString($"{defaultTime}"),
                duration = duration.ToString(@"hh\:mm\:ss")
            }
        );

        Console.WriteLine("\nRecord updated successfully.");
    }

    public int CheckIDExists(string message)
    {

        string idString = "";

        bool validNumber = false;

        using var connection = new SqliteConnection($"{connectionString}");

        do
        {
            Console.WriteLine(message);
            idString = userInput.NumberInput().Trim();
            if (idString == "-1")
            {
                return -1;
            }

            int exists = connection.ExecuteScalar<int>(
                $"SELECT EXISTS(SELECT 1 FROM {tableName} WHERE id = @Id)",
                new
                {
                    Id = idString
                }
            );

            if (exists == 0)
            {
                Console.WriteLine("Record does not exist. try again: ");
            }
            else
            {
                validNumber = true;
            }

        } while (!validNumber);

        int id = Convert.ToInt32(idString);

        return id;
    }

    public void insertRecord(string date, string startTime, string endTime, string duration)
    {
        using var connection = new SqliteConnection($"{connectionString}");
        connection.Execute(
            $"INSERT INTO {tableName}(Date, StartTime, EndTime, Duration) VALUES(@Date, @StartTime, @FinishTime, @Duration)",
            new
            {
                Date = date,
                StartTime = startTime,
                FinishTime = endTime,
                Duration = duration
            }
        );

        Console.WriteLine("\nRecord added successfully.");
        Console.WriteLine($"Date: {date}, StartTime: {startTime}, EndTime: {endTime}, Duration: {duration}");


        return;
    }

    public void FilterEntries(int filterOption)
    {
        List<CodingSession> sessions = CreateList();

        switch (filterOption)
        {
            case 1:
                List<CodingSession> filteredByDate = sessions.OrderBy(s => DateTime.ParseExact(s.Date!, defaultDate, null)).ToList();

                PrintList(filteredByDate);

                break;

            case 2:

                int yearCase2 = userInput.GetYearByUser();

                List<CodingSession> filteredByYearCase2 = sessions.Where(s => DateTime.ParseExact(s.Date!, defaultDate, null).Year == yearCase2).ToList();

                Console.WriteLine();

                PrintList(filteredByYearCase2);

                break;

            case 3:

                int yearCase3 = userInput.GetYearByUser();

                List<CodingSession> filteredByYearCase3 = sessions.Where(s => DateTime.ParseExact(s.Date!, defaultDate, null).Year == yearCase3).ToList();

                int month = userInput.GetMonthByUser();

                List<CodingSession> filteredByMonth = filteredByYearCase3.Where(s => DateTime.ParseExact(s.Date!, defaultDate, null).Month == month).ToList();

                Console.WriteLine();

                PrintList(filteredByMonth);

                break;

        }
    }

    public List<CodingSession> CreateList()
    {
        List<CodingSession> sessions = new List<CodingSession>();

        using var connection = new SqliteConnection(connectionString);
        sessions = connection.Query<CodingSession>($"SELECT * FROM {tableName}").ToList();

        return sessions;
    }

    public void PrintList(List<CodingSession> sessions)
    {

        if (sessions.Count == 0)
        {
            Console.WriteLine("No result.");
            return;
        }
        else
        {
            AnsiConsole.MarkupLine("[Green]Filtered Sessions:[/]");

            var table = new Table()
                .AddColumn("[blue]ID[/]")
                .AddColumn("[blue]Date[/]")
                .AddColumn("[blue]Start Time[/]")
                .AddColumn("[blue]End Time[/]")
                .AddColumn("[blue]Duration[/]");
            foreach (var session in sessions)
            {
                table.AddRow(session.id.ToString(), session.Date!, session.StartTime!, session.EndTime!, session.Duration!);
            }
            AnsiConsole.Write(table);
        }
    }
}
