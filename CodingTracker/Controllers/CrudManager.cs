using CodingTracker.Models;
using CodingTracker.Views;
using Dapper;
using Spectre.Console;

namespace CodingTracker.Controllers;

public static class CrudManager
{
    public static void InsertSqlRecord()
    {
        using var connection = DbBuilder.GetConnection();
        connection.Open();
        CodingSession session = HelpersValidation.GetSessionData();

        if (session.StartTime != "" || session.EndTime != "")
        {
            var sql =
                $"INSERT INTO coding_tracker (startTime, endTime, duration)" +
                $"VALUES ('{session.StartTime}', '{session.EndTime}', '{session.Duration}')";

            var rowsAffected = connection.Execute(sql);
            Console.WriteLine($"\n{rowsAffected} row(s) inserted.");
        }
    }

    public static void DeleteSqlRecord()
    {
        try
        {
            try
            {
                if (CheckForRecords() == 0) throw new HelpersValidation.NoRecords();
                
                TableVisualisationEngine.GenerateFullReport(false);

                using var connection = DbBuilder.GetConnection();
                connection.Open();

                var recordId = UserInput.GetNumberInput("delete");

                var sql =
                    $"DELETE FROM coding_tracker WHERE id = '{recordId}'";

                var rowsAffected = connection.Execute(sql);
                if (rowsAffected == 0)
                {
                    AnsiConsole.Markup(
                        $"[red]Record {recordId} does not exist in the database. Please any key to try another ID.[/]");
                    Console.ReadKey();
                    Console.Clear();
                    DeleteSqlRecord();
                }
                else
                {
                    Console.WriteLine($"\n{rowsAffected} row(s) deleted.");
                }
            }
            catch (HelpersValidation.InputZero)
            {
                Console.WriteLine("Returning to the main menu...");
            }
        }
        catch (HelpersValidation.NoRecords)
        {
            Console.WriteLine("There are no records to delete. Returning to main menu...");
        }
        
    }

    public static void UpdateSqlRecord()
    {
        try
        {
            try
            {
                if (CheckForRecords() == 0) throw new HelpersValidation.NoRecords();
                
                TableVisualisationEngine.GenerateFullReport(false);

                using var connection = DbBuilder.GetConnection();
                connection.Open();

                var recordId = UserInput.GetNumberInput("update");

                CodingSession session = HelpersValidation.GetSessionData();

                if (session.StartTime != "" || session.EndTime != "")
                {
                    var sql =
                        $"""
                            UPDATE coding_tracker
                            SET startTime = '{session.StartTime}', endTime = '{session.EndTime}', duration = '{session.Duration}'
                            WHERE id = '{recordId}';
                         """;

                    var rowsAffected = connection.Execute(sql);
                    if (rowsAffected == 0)
                    {
                        AnsiConsole.Markup(
                            $"[red]Record {recordId} does not exist in the database. Please any key to try another ID.[/]");
                        Console.ReadKey();
                        Console.Clear();
                        UpdateSqlRecord();
                    }
                    else
                    {
                        Console.WriteLine($"\n{rowsAffected} row(s) updated.");
                    }
                }
            }
            catch (HelpersValidation.InputZero)
            {
                Console.WriteLine("Returning to main menu...");
            }
        }
        catch (HelpersValidation.NoRecords)
        {
            Console.WriteLine("There are no records to update. Returning to main menu...");
        }
    }

    public static List<CodingSession> GetAllSessions()
    {
        List<CodingSession> sessionData = new();
        using var connection = DbBuilder.GetConnection();
        connection.Open();
        var reader = connection.ExecuteReader("SELECT id, startTime, endTime FROM coding_tracker");

        while (reader.Read())
        {
            string id = reader.GetString(0);
            string startTime = reader.GetString(1);
            string endTime = reader.GetString(2);

            sessionData.Add(new CodingSession(id, startTime, endTime));
        }

        return sessionData;
    }

    public static List<CodingSession> GetSummarySessions()
    {
        List<CodingSession> sessionData = new();
        using var connection = DbBuilder.GetConnection();
        connection.Open();
        var reader = connection.ExecuteReader(
            @"SELECT * FROM(
                                        SELECT id, startTime, endTime FROM coding_tracker LIMIT 3) a
                                        UNION
                                        SELECT * FROM(
                                        SELECT id, startTime, endTime FROM coding_tracker ORDER BY id DESC LIMIT 3) b");


        while (reader.Read())
        {
            string id = reader.GetString(0);
            string startTime = reader.GetString(1);
            string endTime = reader.GetString(2);

            sessionData.Add(new CodingSession(id, startTime, endTime));
        }

        return sessionData;
    }

    public static List<CodingSession> GetFilteredSessions(string startDate, string endDate)
    {
        List<CodingSession> sessionData = new();
        using var connection = DbBuilder.GetConnection();
        connection.Open();
        var reader = connection.ExecuteReader(@$"SELECT * FROM coding_tracker
                                                        WHERE startTime > '{startDate} 00:00'
                                                        AND endTime < '{endDate} 23:59'");


        while (reader.Read())
        {
            string id = reader.GetString(0);
            string startTime = reader.GetString(1);
            string endTime = reader.GetString(2);

            sessionData.Add(new CodingSession(id, startTime, endTime));
        }

        return sessionData;
    }

    internal static void SeedDatabase()
    {
        for (int i = 0; i < 50; i++)
        {
            using var connection = DbBuilder.GetConnection();
            connection.Open();
            CodingSession session = HelpersValidation.SeedSessionData();

            var sql =
                $"INSERT INTO coding_tracker (startTime, endTime, duration)" +
                $"VALUES ('{session.StartTime}', '{session.EndTime}', '{session.Duration}')";

            connection.Execute(sql);
        }
    }

    internal static int CheckForRecords()
    {
        using var connection = DbBuilder.GetConnection();
        connection.Open();

        var sql = "SELECT COUNT(*) FROM coding_tracker";
        var count = connection.ExecuteScalar<int>(sql);

        return count;
    }
}