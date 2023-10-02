using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker.TomDonegan
{
    internal class Database
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings[
            "CodingTrackerDatabase"
        ].ConnectionString;

        internal static void CreateSQLiteDatabase()
        {
            try
            {
                using SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();

                using (
                    SQLiteCommand command = new SQLiteCommand(
                        ConfigurationManager.ConnectionStrings["CreationQuery"].ConnectionString,
                        connection
                    )
                )
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"The Database could not be created. Error: {e}");
            }
        }

        internal static List<CodingSession> ViewAllSQLiteDatabase()
        {
            string queryString = ConfigurationManager.ConnectionStrings[
                "QueryAllCodingSessions"
            ].ConnectionString;

            using SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(queryString, connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            List<CodingSession> session = new List<CodingSession>();

            while (reader.Read())
            {
                CodingSession model = new CodingSession
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    date = reader.GetString(reader.GetOrdinal("Date")),
                    startTime = reader.GetString(reader.GetOrdinal("StartTime")),
                    endTime = reader.GetString(reader.GetOrdinal("EndTime")),
                    duration = reader.GetString(reader.GetOrdinal("Duration")),
                };

                session.Add(model);
            }

            return session;
        }

        internal static void DeleteEntrySQLiteDatabase(string sessionId)
        {
            string queryString = ConfigurationManager.ConnectionStrings[
                "DeleteSession"
            ].ConnectionString;

            using SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@SessionToDelete", sessionId);

                command.ExecuteNonQuery();
            }
        }

        internal static void UpdateEntrySQLiteDatabase(CodingSession session, string id)
        {
            string queryString = ConfigurationManager.ConnectionStrings[
                "UpdateSession"
            ].ConnectionString;

            using SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@date", session.date);
                command.Parameters.AddWithValue("@startTime", session.startTime);
                command.Parameters.AddWithValue("@endTime", session.endTime);
                command.Parameters.AddWithValue("@duration", session.duration);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();
            }

            connection.Close();
        }

        internal static void AddEntrySQLiteDatabase(CodingSession session)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[
                "CodingTrackerDatabase"
            ].ConnectionString;

            string queryString = ConfigurationManager.ConnectionStrings[
                "InsertCodingSessionQuery"
            ].ConnectionString;

            using SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(queryString, connection);

            command.Parameters.AddWithValue("@Date", session.date);
            command.Parameters.AddWithValue("@StartTime", session.startTime);
            command.Parameters.AddWithValue("@EndTime", session.endTime);
            command.Parameters.AddWithValue("@Duration", session.duration);

            command.ExecuteNonQuery();
        }
    }
}
