using Microsoft.Data.Sqlite;
namespace CodeTracker;

class Program
{
    static void Main()
    {
        using (var connection = new SqliteConnection(UserInput.connectionsString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"Create Table if Not Exists TestLast (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT,
                    EndTime TEXT,
                    TimeCoded TEXT
                    )";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        UserInput.GetUserInput();
    }
}