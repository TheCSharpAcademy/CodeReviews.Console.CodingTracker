using Microsoft.Data.Sqlite;
using System;
using System.Configuration;
using System.Globalization;
using Spectre.Console;

namespace CodeTrackerConfigFirst
{
    public static class Program
    {
        static readonly string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        public static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_session (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,                        
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration INTEGER
                        )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            MainMenu.GetUserInput();
        }

        
        
    }

 
}

