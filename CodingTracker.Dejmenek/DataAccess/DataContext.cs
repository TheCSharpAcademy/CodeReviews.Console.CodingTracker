using CodingTracker.Dejmenek.Helpers;
using CodingTracker.Dejmenek.Models;
using Dapper;
using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker.Dejmenek.DataAccess
{
    public static class DataContext
    {
        private readonly static string _connectionString = ConfigurationManager.ConnectionStrings["CodingTracker"].ConnectionString;

        public static void InitDatabase()
        {
            CreateTables();

            if (IsEmptyCodingSessionsTable())
            {
                SeedCodingSessionsData();
            }

            if (IsEmptyGoalsTable())
            {
                SeedGoalsData();
            }
        }

        private static void CreateTables()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                     CREATE TABLE IF NOT EXISTS Goals (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartDate TEXT NOT NULL,
                        EndDate TEXT NOT NULL,
                        TargetDuration INTEGER NOT NULL
                     );

                    CREATE TABLE IF NOT EXISTS CodingSessions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartDateTime TEXT NOT NULL,
                        EndDateTime TEXT NOT NULL,
                        Duration INTEGER NOT NULL
                    );
                ";

                connection.Execute(sql);
            }
        }

        private static void SeedGoalsData()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    INSERT INTO Goals (StartDate, EndDate, TargetDuration)
                    VALUES (@StartDate, @EndDate, @TargetDuration);
                ";

                List<Goal> randomGoals = RandomGoals.GenerateRandomGoals();

                foreach (var goal in randomGoals)
                {
                    connection.Execute(sql, new
                    {
                        goal.StartDate,
                        goal.EndDate,
                        goal.TargetDuration
                    });
                }
            }
        }

        private static void SeedCodingSessionsData()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    INSERT INTO CodingSessions (StartDateTime, EndDateTime, Duration)
                    VALUES (@StartDateTime, @EndDateTime, @Duration);
                ";

                List<CodingSession> randomSessions = RandomCodingSessions.GenerateRandomCodingSessions();

                foreach (var session in randomSessions)
                {
                    connection.Execute(sql, new
                    {
                        session.StartDateTime,
                        session.EndDateTime,
                        session.Duration
                    });
                }
            }
        }

        private static bool IsEmptyGoalsTable()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"SELECT EXISTS (SELECT 1 FROM Goals);";

                long count = connection.ExecuteScalar<long>(sql);

                if (count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static bool IsEmptyCodingSessionsTable()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"SELECT EXISTS (SELECT 1 FROM CodingSessions);";

                long count = connection.ExecuteScalar<long>(sql);

                if (count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
