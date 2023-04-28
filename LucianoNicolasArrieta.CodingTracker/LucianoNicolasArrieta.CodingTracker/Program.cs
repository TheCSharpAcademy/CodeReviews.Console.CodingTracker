using System.Data.SQLite;

namespace coding_tracker
{
    internal class Program
    {
        static string connectionString;
        static string path;
        static string name;
        static Menu menu = new Menu();

        private static void Main(string[] args)
        {
            connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
            path = System.Configuration.ConfigurationManager.AppSettings.Get("DBPath");
            name = System.Configuration.ConfigurationManager.AppSettings.Get("DBName");

            if (!File.Exists(path))
            {
                SQLiteConnection.CreateFile(name);
            }

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                string createTableQuery =
                    @"CREATE TABLE IF NOT EXISTS coding_tracker (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                            Start_Time TEXT,
                            End_Time TEXT,
                            Duration TEXT
                            )";

                SQLiteCommand command = new SQLiteCommand(createTableQuery, myConnection);
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Welcome to Coding Tracker App");
            menu.GetUserOption();
        }
    }
}