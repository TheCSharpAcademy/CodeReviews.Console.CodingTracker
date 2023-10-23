using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.FarSM
{
    class DataAccess
    {
        private static string conn = ConfigurationManager.AppSettings.Get("db_connection");
        public DataAccess()
        {
            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding_tracker(
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Start_Time TEXT,
                                        End_Time TEXT,
                                        Duration TEXT
                                        )";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
