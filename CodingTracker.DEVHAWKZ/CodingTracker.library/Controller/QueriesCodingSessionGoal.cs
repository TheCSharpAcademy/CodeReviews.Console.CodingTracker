using System.Configuration;
using Microsoft.Data.Sqlite;

namespace CodingTracker.library.Controller;

internal static class QueriesCodingSessionGoal
{
    private static string connectionString = ConfigurationManager.AppSettings.Get("connectionString");

    internal static double TotalLastWeekCodingHoursQuery(string startDate, string endDate)
    {
        double totalHours = 0;

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            
            command.CommandText = $@"
                SELECT Duration
                FROM sessions
                WHERE datetime(StartTime) >= datetime('{startDate}')
                AND datetime(EndTime) <= datetime('{endDate}');";

            SqliteDataReader reader = command.ExecuteReader();
            

            while (reader.Read())
            {
                totalHours = reader.GetDouble(0);
            }   
            
        }

        return totalHours;
    }
}
