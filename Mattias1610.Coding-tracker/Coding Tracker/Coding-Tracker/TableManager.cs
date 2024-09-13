using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace Coding_Tracker
{
    
    public class TableManager
    {
        public string connectionString = @"Data source = coding-tracker.db";
        public void CreateTable(){
            

            using(var connection = new SqliteConnection(connectionString)){
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS coding_tracker(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Duration TEXT)";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        
    }
}