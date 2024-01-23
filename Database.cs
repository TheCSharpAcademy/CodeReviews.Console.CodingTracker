using System.Data.SQLite;

namespace CodingTracker
{
    public class Database
    {
        private string ConnectionString { get; set; }
        private string FileName { get; set; }
        public Database(string connectionString, string fileName)
        {
            ConnectionString = connectionString;
            FileName = fileName;
            CreateDatabase();
        }
        public void CreateDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                if (!File.Exists(FileName))
                {
                    System.Console.WriteLine("Database file does not exist. A new database will be created.");
                    string commandText = "CREATE TABLE IF NOT EXISTS coding_tracker(id INTEGER PRIMARY KEY AUTOINCREMENT, notes TEXT, date_start TEXT, date_end TEXT)";
                    SQLiteCommand command = new(commandText, connection);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        System.Console.WriteLine("New file and table created.");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
    }

}