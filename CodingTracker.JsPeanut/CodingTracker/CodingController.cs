using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    public class CodingController
    {
        static string connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];

        static List<CodingSession> CodingSessions = new();

        public static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS coding_sessions(Id INTEGER PRIMARY KEY AUTOINCREMENT, StartTime TEXT, EndTime TEXT, Duration INTEGER)";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            UserInput.GetUserInput();
        }

        
        public static void Insert()
        {
            Console.Clear();
            string startTime = UserInput.GetStartTimeInput();
            string endTime = UserInput.GetEndTimeInput();
            string durationMessage = CalculateDuration(startTime, endTime);
            
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO coding_sessions(StartTime, EndTime, Duration) VALUES('{startTime}', '{endTime}', '{DateDifference.Duration}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            UserInput.GetUserInput();
        }

        public static void StopwatchSession()
        {
            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Press any key to start the stopwatch");
            Console.ReadKey();
            Console.WriteLine("Stopwatch started");

            stopwatch.Start();
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    stopwatch.Stop();
                    Console.WriteLine("Stopwatch stopped");
                    break;
                }

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Elapsed time: {stopwatch.Elapsed}");
                Thread.Sleep(100);
            }

            Console.ReadKey();
        }

        public static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "SELECT * FROM coding_sessions";

                tableCmd.ExecuteNonQuery();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CodingSessions.Add(new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            StartTime = DateTime.ParseExact(reader.GetString(1), "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture),
                            EndTime = DateTime.ParseExact(reader.GetString(2), "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture),
                            Duration = TimeSpan.Parse(reader.GetString(3))
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No coding sessions found.");
                    UserInput.GetUserInput();
                }

                connection.Close();

                TableVisualizationEngine.PrintInTableFormat(CodingSessions);

                CodingSessions.Clear();

            }
            
        }

        public static void Update()
        {
            GetAllRecords();
            int recordId = UserInput.GetNumberInput("Type the ID of the record you want to update. Type M to return to the main menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();

                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_sessions WHERE Id = {recordId})";

                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"Record with ID {recordId} doesn't exist.");
                    connection.Close();
                    Update();
                }

                string newStartTime = UserInput.GetStartTimeInput();
                string newEndTime = UserInput.GetEndTimeInput();
                string newDurationMessage = CalculateDuration(newStartTime, newEndTime);

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"UPDATE coding_sessions SET StartTime = '{newStartTime}', EndTime = '{newEndTime}', Duration = '{DateDifference.Duration}' WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();

                connection.Close();

                UserInput.GetUserInput();
            }
            
        }

        public static void Delete()
        {
            Console.Clear();
            GetAllRecords();
            int recordId = UserInput.GetNumberInput("Type the ID of the record you want to delete. Type M to return to the main menu.");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM coding_sessions WHERE Id = {recordId}";

                var tableCmd2 = connection.CreateCommand();

                tableCmd2.CommandText = $"UPDATE sqlite_sequence SET seq = seq - 1 WHERE Id = {recordId}";

                var deletedRows = tableCmd.ExecuteNonQuery();

                if (deletedRows == 0)
                {
                    Console.WriteLine($"Record with ID {recordId} does not exist.");
                    Delete();
                }

                connection.Close();
            }

            UserInput.GetUserInput();
        }

        public static string CalculateDuration(string initialDate, string finalDate)
        {
            DateTime initialDate_ = DateTime.ParseExact(initialDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime finalDate_ = DateTime.ParseExact(finalDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateDifference dateDifference = new DateDifference(initialDate_, finalDate_);
            Console.WriteLine(dateDifference.PrintCodingSessionTime());

            return dateDifference.PrintCodingSessionTime();
        }

    }
}
