using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;

namespace CodingTracker.jkjones98
{
    internal class CodingController
    {
        static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        internal void GetRecords()
        {
            List<CodingSession> tableData = new List<CodingSession>();
            using(var connection = new SqliteConnection(connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM coding";

                    using(var reader = tableCmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                tableData.Add(new CodingSession
                                    {
                                        Id = reader.GetInt32(0),
                                        Date = reader.GetString(1),
                                        StartTime = reader.GetString(2),
                                        EndTime = reader.GetString(3),
                                        Duration = reader.GetString(4)
                                    });
                            }
                        }
                        else
                            Console.WriteLine("\nNo rows found.\n");
                    }
                }
                Console.WriteLine("\n\n");
            }
            ConsoleTableCreator.ShowTable(tableData);
        }

        internal void GetGoalRecords()
        {
            List<GoalTracking> goalData = new();
            using(var connection = new SqliteConnection(connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM goals";

                    using(var reader = tableCmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                goalData.Add(new GoalTracking
                                    {
                                        Id = reader.GetInt32(0),
                                        GoalName = reader.GetString(1),
                                        GoalHours = reader.GetFloat(2),
                                        HoursDone = reader.GetFloat(3),
                                        HoursLeft = reader.GetFloat(4)
                                    });
                            }
                        }
                        else
                            Console.WriteLine("\nNo rows found");
                    }
                }
                Console.WriteLine("\n\n");
            }
            ConsoleTableCreator.ShowGoalTable(goalData);
        }

        internal void InsertRecordDb(CodingSession coding)
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO coding (Date, StartTime, EndTime, Duration) VALUES ('{coding.Date}', '{coding.StartTime}', '{coding.EndTime}', '{coding.Duration}')";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal void InsertRecordDb(GoalTracking goals)
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO goals (GoalName, GoalHours, HoursDone, HoursLeft) VALUES ('{goals.GoalName}', '{goals.GoalHours}', '{goals.HoursDone}', '{goals.HoursLeft}')";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal CodingSession CheckIdExists(int id)
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM coding WHERE Id = '{id}' ";

                    using(var reader = tableCmd.ExecuteReader())
                    {
                        CodingSession coding = new();
                        if(reader.HasRows)
                        {
                            reader.Read();
                            coding.Id = reader.GetInt32(0);
                            coding.Date = reader.GetString(1);
                            coding.StartTime = reader.GetString(2);
                            coding.EndTime = reader.GetString(3);
                            coding.Duration = reader.GetString(4);
                        }
                        Console.WriteLine("\n\n");

                        return coding;
                    }
                }
            }
            
        }

        internal void DeleteRecordDb(int id)
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"DELETE FROM coding WHERE Id = '{id}' ";
                    tableCmd.ExecuteNonQuery();

                    Console.WriteLine($"\nRow with Id {id} has been removed");
                }
            }
        }

    
        internal string GetStartOrEndTime(string startOrEnd, int id)
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    CodingSession coding = new();

                    tableCmd.CommandText = $"SELECT * FROM coding WHERE Id = '{id}' ";

                    using(var reader = tableCmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            if(startOrEnd == "EndTime")
                            {
                                reader.Read();
                                coding.EndTime = reader.GetString(3);
                                return coding.EndTime;
                            }
                            else
                            {
                                reader.Read();
                                coding.StartTime = reader.GetString(2);
                                return coding.StartTime;
                            }  
                        }
                    }
                }
                return default;
            }
        }

        internal void UpdateRecordDb(CodingSession updateCoding)
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = 
                        $@"UPDATE coding SET
                            Date = '{updateCoding.Date}',
                            StartTime = '{updateCoding.StartTime}',
                            EndTime = '{updateCoding.EndTime}',
                            Duration = '{updateCoding.Duration}'
                        WHERE 
                            Id = '{updateCoding.Id}'";
                    tableCmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine($"\nRecord with Id {updateCoding.Id} has been updated successfully");
        }

        internal float GetDurationSum()
        {

            using(var connection = new SqliteConnection(connectionString))
            {
                List<string> totalDurationQuery = new();

                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT SUM(Duration) FROM coding";

                    SqliteDataReader reader = tableCmd.ExecuteReader();
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            totalDurationQuery.Add(reader.GetString(0));                                
                        }
                    }
                    else
                        Console.WriteLine("No rows found");
                }

            float parsedDuration = float.Parse(totalDurationQuery.ElementAt(0));

            return parsedDuration;
            }
        }

        internal void GetFilterRecords(string likeDate)
        {
            List<CodingSession> tableData = new List<CodingSession>();
            using(var connection = new SqliteConnection(connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM coding WHERE Date like '%{likeDate}%' ";

                    using(var reader = tableCmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                tableData.Add(new CodingSession
                                    {
                                        Id = reader.GetInt32(0),
                                        Date = reader.GetString(1),
                                        StartTime = reader.GetString(2),
                                        EndTime = reader.GetString(3),
                                        Duration = reader.GetString(4)
                                    });
                            }
                        }
                        else
                            Console.WriteLine("\nNo rows found.\n");
                    }
                }
                Console.WriteLine("\n\n");
            }
            ConsoleTableCreator.ShowTable(tableData);
        }
    }
}