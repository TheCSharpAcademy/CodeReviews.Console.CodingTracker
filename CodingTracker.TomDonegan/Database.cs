using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker.TomDonegan
{
    public class Database
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings[
            "CodingTrackerDatabase"
        ].ConnectionString;

        public static void CreateSQLiteDatabase()
        {
            try
            {
                using SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();

                // Create a table (e.g., Person) in the database
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

        public static List<CodingSession> ViewAllSQLiteDatabase()
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

            // Now you have a list of YourModel objects with the retrieved data
            return session;
        }

        public static void DeleteEntrySQLiteDatabase(string sessionId)
        {
            string queryString = ConfigurationManager.ConnectionStrings[
                "DeleteSession"
            ].ConnectionString;

            using SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
            {
                // Replace @ValueToDelete with the actual value you want to delete
                command.Parameters.AddWithValue("@SessionToDelete", sessionId);

                command.ExecuteNonQuery();
            }
        }

        public static void UpdateEntrySQLiteDatabase() { }

        public static void AddEntrySQLiteDatabase(CodingSession session)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[
                "CodingTrackerDatabase"
            ].ConnectionString;

            string queryString = ConfigurationManager.ConnectionStrings[
                "InsertCodingSessionQuery"
            ].ConnectionString;

            using SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            // Create the SQLite command with the insert query and connection
            using SQLiteCommand command = new SQLiteCommand(queryString, connection);
            // Add parameters with values
            command.Parameters.AddWithValue("@Date", session.date);
            command.Parameters.AddWithValue("@StartTime", session.startTime);
            command.Parameters.AddWithValue("@EndTime", session.endTime);
            command.Parameters.AddWithValue("@Duration", session.duration);

            // Execute the SQLite command to insert the data
            command.ExecuteNonQuery();
        }
    }
}
