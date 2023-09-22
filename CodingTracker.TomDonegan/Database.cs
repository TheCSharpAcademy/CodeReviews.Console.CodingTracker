using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker.TomDonegan
{
    public class Database
    {
        public static void CreateSQLiteDatabase()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[
                    "CodingTrackerDatabase"
                ].ConnectionString;
                Console.WriteLine(connectionString);

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a table (e.g., Person) in the database
                    using (
                        SQLiteCommand command = new SQLiteCommand(
                            "CREATE TABLE IF NOT EXISTS CodingSessions (Id INTEGER PRIMARY KEY,Date TEXT, StartTime TEXT, EndTime TEXT, Duration TEXT);",
                            connection
                        )
                    )
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The Database could not be created. Error: {e}");
            }
        }

        public static void DeleteSQLiteDatabase() { }

        public static void UpdateSQLiteDatabase() { }

        public static void AddEntrySQLiteDatabase(CodingSession session)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[
                    "CodingTrackerDatabase"
                ].ConnectionString;

            string queryString = ConfigurationManager.ConnectionStrings[
                    "InsertCodingSessionQuery"
                ].ConnectionString;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Create the SQLite command with the insert query and connection
                using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                {
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
    }
}
