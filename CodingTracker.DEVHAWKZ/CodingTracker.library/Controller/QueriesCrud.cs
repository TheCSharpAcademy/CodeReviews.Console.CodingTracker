using CodingTracker.library.Model;
using CodingTracker.library.View;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace CodingTracker.library.Controller;

internal static class QueriesCrud 
{
    private static string connectionString = Queries.ConnectionString;

    internal static void CreateTableQuery()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS sessions
                                       (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            StartTime TEXT,
                                            EndTime TEXT,
                                            Duration REAL
                                        )";

            tableCommand.ExecuteNonQuery();
            connection.Close();
        }
    }

    internal static void InsertNewSessionQuery(string startTime, string endTime, double duration)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $"INSERT INTO sessions(StartTime, EndTime, Duration) VALUES ('{startTime}', '{endTime}', '{duration}')";

            tableCommand.ExecuteNonQuery();
            connection.Close();
        }
    }

    internal static int ViewAllSessionsQuery(string operation = "")
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @"SELECT * FROM sessions";
            SqliteDataReader reader = tableCommand.ExecuteReader();

            List<CodingSessions> sessions = new();


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    sessions.Add(new CodingSessions()
                    {
                        SessionId = reader.GetInt32(0),
                        SessionStartTime = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy HH:mm", new CultureInfo("en-US")),
                        SessionEndTime = DateTime.ParseExact(reader.GetString(2), "dd-MM-yyyy HH:mm", new CultureInfo("en-US")),
                        Duration = reader.GetDouble(3)
                    });
                }

                if (operation != "query" && operation != "crud")
                {
                    if (sessions.Count > 1)
                    {
                        Helpers.GetOrderedList(sessions);
                    }

                    else
                    {
                        TableVisualizationEngine.PrintSessions(sessions);
                    }
                }

                else if (operation == "crud")
                {
                    TableVisualizationEngine.PrintSessions(sessions);
                }

            }
            
            else
            {
                Console.Clear();
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("\nNo records found!\n");
                Console.WriteLine("-----------------------------------------------------\n\nPress any key to get back to main menu...");
                Console.ReadKey();

            }
            
            connection.Close();
            return sessions.Count;
        }
    }
    
    internal static void DeleteSessionQuery(int recordId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $"DELETE FROM sessions WHERE Id = {recordId}";

            int rowCount = tableCommand.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\nSession with id {recordId} doesn't exists.\n\nPress any key to continue");
                Console.ReadKey();
                connection.Close();
                CrudController.DeleteSession();
            }

            Console.Clear();
            Console.WriteLine("Session deleted successfully!");
            connection.Close();
        }
    }

    internal static void UpdateSessionQuery(int recordId)
    {
        bool possibleUpdate = PossibleUpdateQuery(recordId);

        if (possibleUpdate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string startTime = Helpers.GetDateTime("start"); 
                string endTime = Helpers.GetDateTime("end", startTime, "endTime"); 
                double duration = Helpers.Duration(startTime, endTime);

                connection.Open();
                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = $"UPDATE sessions SET StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}' WHERE Id = {recordId}";

                tableCommand.ExecuteNonQuery();
                connection.Close();

                Console.Clear();
                Console.WriteLine("Session updated successfully!\n\nPress any key to get back to main menu...");
                Console.ReadKey();
            }
        }

        else
        {
            Console.WriteLine($"Session with id {recordId} doesn't exist.\n\nPress any key to get back to main menu...");
            Console.ReadKey();
        }
    }

    private static bool PossibleUpdateQuery(int recordId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $"SELECT EXISTS (SELECT 1 FROM sessions WHERE Id = {recordId}) ";

            int rowCount = Convert.ToInt32(tableCommand.ExecuteScalar());

            if (rowCount == 0)
            {
                connection.Close();
                return false;
            }

            else
            {
                connection.Close();
                return true;
            }
        }
    }
}
