using CodingTracker.SamGannon.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CodingTracker.SamGannon
{
    internal class CodingController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;

        internal void Post(Coding coding)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO coding (date, duration) VALUES ('{coding.Date}', '{coding.Duration }')";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal void GetCodingData()
        {
            List<Coding> tableData = new List<Coding>();
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM coding";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tableData.Add(
                                    new Coding
                                    {
                                        Id = reader.GetInt32(0),
                                        Date = reader.GetString(1),
                                        Duration = reader.GetString(2)
                                    });
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n\nNo rows found");
                        }

                        TableVisualisation.ShowTable(tableData);
                    }
                }
            }
        }

        internal Coding GetById(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM coding WHERE Id = {id}";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        Coding coding = new();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            coding.Id = reader.GetInt32(0);
                            coding.Date = reader.GetString(1);
                            coding.Duration = reader.GetString(2);

                        }

                        Console.WriteLine("\n\n");

                        return coding;
                    }
                }
            }
        }

        internal void Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"DELETE FROM coding WHERE Id = {id}";
                    tableCmd.ExecuteNonQuery();

                    Console.WriteLine($"\nRecord with id {id} was deleted");
                }
            }
        }

        internal void Update(Coding coding)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $@"UPDATE coding SET
                        Date = '{coding.Date}',
                        Duration = '{coding.Duration}'
                    WHERE Id = {coding.Id}";

                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal void GetSleepData()
        {
            List<Sleep> tableData = new List<Sleep>();
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM sleep";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tableData.Add(
                                    new Sleep
                                    {
                                        Id = reader.GetInt32(0),
                                        Duration = reader.GetString(1),
                                        SleepType = reader.GetString(2)
                                    });
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n\nNo rows found");
                        }

                        TableVisualisation.ShowTable(tableData);
                    }
                }
            }
        }

        internal Sleep GetBySleepId(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM sleep WHERE Id = {id}";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        Sleep sleep = new();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            sleep.Id = reader.GetInt32(0);
                            sleep.Duration = reader.GetString(1);
                            sleep.SleepType = reader.GetString(2);

                        }

                        Console.WriteLine("\n\n");

                        return sleep;
                    }
                }
            }
        }

        internal void UpdateSleep(Sleep sleep)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $@"UPDATE sleep SET
                        Duration = '{sleep.Duration}',
                        SleepType = '{sleep.SleepType}'
                    WHERE Id = {sleep.Id}";

                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal void PostSleep(Sleep sleep)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO sleep (Duration, SleepType) VALUES ('{sleep.Duration}', '{sleep.SleepType}')";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal void DeleteSleep(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"DELETE FROM sleep WHERE Id = {id}";
                    tableCmd.ExecuteNonQuery();

                    Console.WriteLine($"\nRecord with id {id} was deleted");
                }
            }
        }
    }
}