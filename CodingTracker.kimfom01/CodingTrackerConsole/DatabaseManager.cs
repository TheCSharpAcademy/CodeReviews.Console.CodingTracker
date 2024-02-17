using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTrackerConsole;

internal class DatabaseManager
{
    private readonly string? _connectionString = ConfigurationManager.AppSettings.Get("connectionString");

    public void CreateDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = @"CREATE TABLE IF NOT EXISTS codingTracker(
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Date TXT,
                                            StartTime TXT,
                                            EndTime TXT,
                                            Duration TXT)";

                command.ExecuteNonQuery();
            }
        }
    }

    public void InsertRecord(CodingTrackerModel modelRecord)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = @$"INSERT INTO codingTracker (Date, StartTime, EndTime, Duration)
                                            VALUES ('{modelRecord.Date}', '{modelRecord.StartTime}', '{modelRecord.EndTime}', '{modelRecord.Duration}')";

                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteRecord(string date)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = @$"DELETE FROM codingTracker
                                            WHERE Date = '{date}'";

                command.ExecuteNonQuery();
            }
        }

        Console.Clear();
    }

    public void UpdateRecord(string oldDate, CodingTrackerModel newRecord)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                if (newRecord.StartTime is null && newRecord.EndTime is null)
                {
                    command.CommandText = $@"UPDATE codingTracker
                                                SET Date = '{newRecord.Date}'
                                                WHERE Date = '{oldDate}'";
                }
                else if (newRecord.Date is null)
                {
                    command.CommandText = $@"UPDATE codingTracker
                                                SET StartTime = '{newRecord.StartTime}', EndTime = '{newRecord.EndTime}', Duration = '{newRecord.Duration}'
                                                WHERE Date = '{oldDate}'";
                }
                else
                {
                    command.CommandText = $@"UPDATE codingTracker
                                                SET Date = '{newRecord.Date}', StartTime = '{newRecord.StartTime}', EndTime = '{newRecord.EndTime}', Duration = '{newRecord.Duration}'
                                                WHERE Date = '{oldDate}'";
                }

                command.ExecuteNonQuery();
            }
        }
    }

    public List<CodingTrackerModel> ReadFromDb()
    {
        List<CodingTrackerModel> codingTrackerModels = new();

        using (var connection = new SqliteConnection(_connectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = $@"SELECT * FROM codingTracker";

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var models = new CodingTrackerModel
                        {
                            Date = (string)reader["Date"],
                            StartTime = (string)reader["StartTime"],
                            EndTime = (string)reader["EndTime"],
                            Duration = (string)reader["Duration"]
                        };

                        codingTrackerModels.Add(models);
                    }
                }
                else
                {
                    Console.WriteLine("\nYou don't have any records!\n");
                }
            }
        }

        return codingTrackerModels;
    }
}