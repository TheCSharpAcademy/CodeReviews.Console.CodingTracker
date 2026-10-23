using Microsoft.Data.Sqlite;
using Dapper;
using CodingTracker.Models;

namespace CodingTracker.Data
{
    internal class DatabaseInitialiser
    {
        internal static void InitialiseDatabase()
        {
            try
            {
                string connectionString = ConfigReader.ReadConfig();

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                connection.Execute(@"
                CREATE TABLE IF NOT EXISTS CodingSessions (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL,
                    Duration TEXT NOT NULL);");

                SeedData(connection);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void SeedData(SqliteConnection connection)
        {
            int recordCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM CodingSessions;");

            if (recordCount > 0)
                return;

            List<CodingSession> seedSessions = new List<CodingSession>
            {
                new() { ID = 0, StartTime = "2026-10-15 09:00", EndTime = "2026-10-15 11:30", Duration = "02:30" },
                new() { ID = 0, StartTime = "2026-10-16 14:15", EndTime = "2026-10-16 15:45", Duration = "01:30" },
                new() { ID = 0, StartTime = "2026-10-17 19:00", EndTime = "2026-10-17 22:15", Duration = "03:15" },
                new() { ID = 0, StartTime = "2026-10-18 08:30", EndTime = "2026-10-18 10:00", Duration = "01:30" },
                new() { ID = 0, StartTime = "2026-10-19 11:00", EndTime = "2026-10-19 16:45", Duration = "05:45" }
            };

            string sql = @"INSERT INTO CodingSessions (StartTime, EndTime, Duration)
                            VALUES (@StartTime, @EndTime, @Duration);";

            connection.Execute(sql, seedSessions);
        }
    }
}
