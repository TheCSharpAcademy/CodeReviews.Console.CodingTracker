using Microsoft.Data.Sqlite;

namespace CodingTrackerProgram.Database
{
    public class Connection
    {
        private static SqliteConnection? _connection;

        public static void Init()
        {
            try
            {
                string dbName = System.Configuration.ConfigurationManager.AppSettings["DbPath"] ??
                    throw new System.Configuration.ConfigurationErrorsException("DbPath configuration must be defined in App.config");

                _connection = new SqliteConnection($"Data Source={dbName}");

                CodingSessionRepository.CreateTable();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"ERROR CODE X001: Could not initialise application. {ex.Message}");
                _connection = null;
                Environment.Exit(1);
            }
        }

        public static SqliteConnection GetConnection()
        {
            if (_connection == null)
            {
                throw new Exception("Database connection missing.");
            }

            return _connection;
        }
    }
}