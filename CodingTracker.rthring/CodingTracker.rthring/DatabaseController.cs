using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace CodingTracker.rthring
{
    public class DatabaseController
    {
        private string ConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
        public DatabaseController()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_session (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime Text,
                        Duration INTEGER
                        )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        
    }
}
