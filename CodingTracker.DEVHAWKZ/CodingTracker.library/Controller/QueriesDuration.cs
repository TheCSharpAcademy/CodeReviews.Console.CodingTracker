using CodingTracker.library.View;
using Microsoft.Data.Sqlite;

namespace CodingTracker.library.Controller;

internal static class QueriesDuration
{
    private static string connectionString = Queries.ConnectionString;
    internal static void MaxDurationQuery()
    {
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @"SELECT * FROM sessions WHERE Duration = (SELECT MAX(Duration) FROM sessions)";
     
            SqliteDataReader reader = tableCommand.ExecuteReader();
               
            if (reader.Read())
            {
                TableVisualizationEngine.PrintQuerieValues(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetDouble(3), "Longest Session");
            }

            connection.Close();      
        }
    }

    internal static void MinDurationQuery()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @"SELECT * FROM sessions WHERE Duration = (SELECT MIN(Duration) FROM sessions)";

            SqliteDataReader reader = tableCommand.ExecuteReader();

            if (reader.Read())
            {
                TableVisualizationEngine.PrintQuerieValues(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDouble(3), "Shortest Session");
            }

            connection.Close();
        }
    }
   
    internal static void AvgDurationQuery()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = @"SELECT Avg(Duration) FROM sessions WHERE Duration > 0";

            SqliteDataReader reader = tableCommand.ExecuteReader();

            if (reader.Read())
            {
                TableVisualizationEngine.PrintSingleValue(Math.Round(reader.GetDouble(0),2), "Average time in minutes");
            }

            connection.Close();
        }
    }


}
