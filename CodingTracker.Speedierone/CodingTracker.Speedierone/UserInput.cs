using Microsoft.Data.Sqlite;

namespace CodingTracker;
internal class UserInput
{
    private static void ViewRecords()
    {

    }

    public static void AddRecord()
    {
        Console.Clear();
        string date = Helpers.GetDate();
        string timeStart = Helpers.GetTime();
        string timeEnd = Helpers.GetTime();
        string connectionString = "Data Source=Coding-Tracker.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO code_tracker(Date, StartTime, EndTime) VALUES('{date}', '{timeStart}', '{timeEnd}')";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
