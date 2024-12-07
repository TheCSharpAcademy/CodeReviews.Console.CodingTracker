using Spectre.Console;
using Microsoft.Data.Sqlite;
using Dapper;
using CodingTracker.DreamFXX.Models;


namespace CodingTracker.DreamFXX
{
    internal class DatabaseManager
    {
        private readonly string? _connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        public void CreateDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(@"CREATE TABLE IF NOT EXISTS MyCodingTracker (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        StartTime TEXT,
                                        EndTime TEXT,
                                        Duration TEXT)");
            }
        }

        public void InsertRecord(string date, string startTime, string endTime, string duration)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(@$"INSERT INTO MyCodingTracker (Date, StartTime, EndTime, Duration)
                                       VALUES('{date}', '{startTime}', '{endTime}', '{duration}')");
            }
        }

        public void DeleteRecord(string date)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = @$"DELETE FROM MyCodingTracker
                                            WHERE Date = '{date}'";
                connection.Execute(sql);
            }
        }

        public void UpdateRecord(string oldDate, string? newDate, string? startTime, string? endTime, string? duration)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                
                    if (startTime is null && endTime is null)
                    {
                        connection.Execute($@"UPDATE MyCodingTracker
                                                SET Date = '{newDate}'
                                                WHERE Date = '{oldDate}'");
                    }
                    else if (newDate is null)
                    {
                        connection.Execute($@"UPDATE MyCodingTracker
                                                SET StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}'
                                                WHERE Date = '{oldDate}'");
                    }
                    else
                    {
                        connection.Execute($@"UPDATE MyCodingTracker
                                               SET Date = '{newDate}', StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}'
                                               WHERE Date = '{oldDate}'");
                    }
            }
        }

        public List<CodingSession> ReadFromDb()
        {
            List<CodingSession> codingSessions = new();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = @"SELECT * FROM MyCodingTracker";
                using (var command = new SqliteCommand(sqlQuery, connection))
                using (var reader = command.ExecuteReader())

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var model = new CodingSession
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Date = reader["Date"].ToString()!,
                                StartTime = reader["StartTime"].ToString()!,
                                EndTime = reader["EndTime"].ToString()!,
                                Duration = reader["Duration"].ToString()!
                            };

                            codingSessions.Add(model);
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]No records found in the database.[/]");
                    }
            }
            return codingSessions;
        }

        public CodingSession? ReadSingleRecord(string date)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM MyCodingTracker WHERE Date = @Date";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", date);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CodingSession
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Date = reader["Date"].ToString()!,
                                StartTime = reader["StartTime"].ToString()!,
                                EndTime = reader["EndTime"].ToString()!,
                                Duration = reader["Duration"].ToString()!
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
