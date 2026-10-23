using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
namespace CodingTracker.Data
{
    internal class CodingSessionRepository
    {
        private static string connectionString = ConfigReader.ReadConfig();
        internal static void AddCodingSession(CodingSession session)
        {
            try
            {
                var sql = @"INSERT INTO CodingSessions (StartTime, EndTime, Duration)
                        VALUES (@StartTime, @EndTime, @Duration)";

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                connection.Execute(sql, session);

            }
            catch (Exception)
            {
                throw;
            }

        }

        internal static List<CodingSession> GetAllSessions()
        {
            List<CodingSession> sessions = new List<CodingSession>();
            try
            {
                var sql = @"SELECT * FROM CodingSessions;";

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                sessions = connection.Query<CodingSession>(sql).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return sessions;
        }

        internal static List<int> GetAllSessionIds()
        {
            List<int> sessionIds = new List<int>();
            try
            {
                var sql = @"SELECT ID FROM CodingSessions;";

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                sessionIds = connection.Query<int>(sql).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return sessionIds;
        }

        internal static CodingSession GetSessionFromId(int id)
        {
            try
            {
                var sql = @"SELECT * FROM CodingSessions
                            WHERE ID = @ID;";

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                return connection.QuerySingleOrDefault<CodingSession>(sql, new { ID = id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void DeleteSessionFromId(int id)
        {
            try
            {
                var sql = @"DELETE FROM CodingSessions
                            WHERE ID = @ID;";

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                connection.Execute(sql, new {ID = id});
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void UpdateSessionFromId(int id, CodingSession session)
        {
            try
            {
                var sql = @"UPDATE CodingSessions
                            SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration
                            WHERE ID = @ID;";

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                connection.Execute(sql, session);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
