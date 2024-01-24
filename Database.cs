using System.Data.SQLite;

namespace CodingTracker
{
    public class Database
    {
        private string _connectionString;
        private string _fileName;
        public Database(string connectionString, string fileName)
        {
            _connectionString = connectionString;
            _fileName = fileName;
            InitializeDatabase();
        }
        public void InitializeDatabase()
        {
            if (!File.Exists(_fileName))
            {
                System.Console.WriteLine("Database file does not exist. A new database will be created.");
            }

            using (var connection = new SQLiteConnection(_connectionString))
            {

                string commandText = "CREATE TABLE IF NOT EXISTS coding_tracker(id INTEGER PRIMARY KEY AUTOINCREMENT, notes TEXT, date_start TEXT, date_end TEXT)";
                SQLiteCommand command = new(commandText, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    System.Console.WriteLine("Database initialized.");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error: {ex.Message}");
                }

            }
        }
    }

}