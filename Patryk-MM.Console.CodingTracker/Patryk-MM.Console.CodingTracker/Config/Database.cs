using System.Data.SQLite;

namespace Patryk_MM.Console.CodingTracker.Config {
    /// <summary>
    /// Represents methods to interact with the SQLite database.
    /// </summary>
    public class Database {
        private static string _connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
        /// <summary>
        /// Gets a SQLite database connection using the connection string specified in the application settings.
        /// </summary>
        public static SQLiteConnection GetConnection() {
            return new SQLiteConnection(_connectionString);
        }
        /// <summary>
        /// Initializes the database by creating required tables if they do not exist.
        /// </summary>
        public static void InitializeDatabase() {
            using (var connection = GetConnection()) {
                connection.Open();

                string createTableQuery = @"CREATE TABLE IF NOT EXISTS CodingSessions(
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            StartDate TEXT NOT NULL,
                                            EndDate TEXT NOT NUll,
                                            Duration TEXT NOT NULL);
                                            CREATE TABLE IF NOT EXISTS CodingGoals(
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            YearAndMonth TEXT NOT NULL,
                                            Hours INTEGET NOT NULL,
                                            HourGoal INTEGER NOT NULL);";

                using (var command = new SQLiteCommand(createTableQuery, connection)) {
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
