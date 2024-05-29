using System.Configuration;
using System.Data.SQLite;

namespace Patryk_MM.Console.CodingTracker.Config {
    public class Database {
        private static string _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        public static SQLiteConnection GetConnection() {
            return new SQLiteConnection(_connectionString);
        }

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
                                            HourGoal INTEGER NOT NULL);";

                using (var command = new SQLiteCommand(createTableQuery, connection)) {
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
