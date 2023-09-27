using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingTracker.rthring.Models;
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

        // insert into coding_session (StartTime, EndTime, Duration) VALUES ('2023/09/25; 01:17', '2023/09/25; 02:18', 61)

        // SELECT * FROM coding_session

        internal List<CodingSession> GetRecords()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM coding_session";

                List<CodingSession> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("HAS ROWS");
                    while (reader.Read())
                    {
                        tableData.Add(
                            new CodingSession
                            {
                                Id = reader.GetInt32(0),
                                // MONTHS ARE WRONG WHEN OUTPUTTED
                                StartTime = DateTime.ParseExact(reader.GetString(1), "yyyy/MM/dd HH:mm", new CultureInfo("en-US")),
                                EndTime = DateTime.ParseExact(reader.GetString(2), "yyyy/MM/dd HH:mm", new CultureInfo("en-US")),
                                Duration = reader.GetInt32(3)
                            });
                    }
                }

                connection.Close();

                return tableData;
            }
        }
    }
}
