using System.Data.SQLite;
using DbHelpers.HopelessCoding;
using Dapper;
using ValidationChecks.HopelessCoding;
using Helpers.HopelessCoding;
using CodingModel.HopelessCoding;

namespace User.HopelessCoding;

internal class UserCommands
{
    internal static void AddNewRecord()
    {
        Console.Clear();
        Console.WriteLine("Adding a new record to the database\n");
        var newCodingSession = GeneralHelpers.CreateCodingSession(null);

        using (var connection = new SQLiteConnection(DatabaseHelpers.connectionString))
        {
            connection.Open();

            string addNewQuery = @"INSERT INTO coding_sessions (StartTime, EndTime, Duration)
                                 VALUES (@StartTime, @EndTime, @Duration);";

            GeneralHelpers.PrintExecutionResult(connection.Execute(addNewQuery, newCodingSession));
        }
    }

    internal static void UpdateRecord()
    {
        Console.Clear();
        Console.WriteLine("Updating record from the database\n");

        ViewRecords();

        int idToUpdate = Validations.GetValidIdFromUser("update");
        if (!Validations.ValidateId(idToUpdate))
        {
            return;
        }

        var codingSession = GeneralHelpers.CreateCodingSession(idToUpdate);

        using (var connection = new SQLiteConnection(DatabaseHelpers.connectionString))
        {
            string updateQuery = @"UPDATE coding_sessions
                                 SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration
                                 WHERE Id = @Id";

            GeneralHelpers.PrintExecutionResult(connection.Execute(updateQuery, codingSession));
        }
    }

    internal static void DeleteRecord()
    {
        Console.Clear();
        Console.WriteLine("Deleting record from the database\n");

        ViewRecords();

        int idToDelete = Validations.GetValidIdFromUser("delete");
        if (!Validations.ValidateId(idToDelete))
        {
            return;
        }

        using (var connection = new SQLiteConnection(DatabaseHelpers.connectionString))
        {
            string deleteQuery = "DELETE FROM coding_sessions WHERE Id = @Id";

            GeneralHelpers.PrintExecutionResult(connection.Execute(deleteQuery, new CodingSession
            {
                Id = idToDelete
            }));
        }
    }

    internal static void ViewRecords()
    {
        Console.Clear();
        string listQuery = @"SELECT * FROM coding_sessions ORDER BY StartTime DESC";
        DatabaseHelpers.ListRecords(listQuery, null, "All records sorted by date");

        Console.WriteLine("----------------------------");
    }
}