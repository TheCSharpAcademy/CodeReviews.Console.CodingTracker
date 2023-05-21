using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    public class CodingController
    {
        static string connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];

        public static List<CodingSession> CodingSessions = new();

        static List<CodingSession> DeletedCodingSessions = new();

        public static List<Goal> Goals = new();

        public static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS coding_sessions(Id INTEGER PRIMARY KEY AUTOINCREMENT, StartTime TEXT, EndTime TEXT, Duration INTEGER)";

                var tableCmd2 = connection.CreateCommand();

                tableCmd2.CommandText = "CREATE TABLE IF NOT EXISTS goals(Id INTEGER PRIMARY KEY AUTOINCREMENT, Goal TEXT, AddedDate TEXT, ProgressRemaining TEXT)";

                tableCmd.ExecuteNonQuery();

                tableCmd2.ExecuteNonQuery();

                connection.Close();
            }
            UserInput.GetUserInput();

        }

        public static void Insert()
        {
            Console.Clear();
            string startTime = UserInput.GetStartTimeInput();
            string endTime = UserInput.GetEndTimeInput();
            Validation.ValidateDuration(startTime, endTime, "dd/MM/yyyy HH:mm", UserInput.GetStartTimeInput, UserInput.GetEndTimeInput);
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO coding_sessions(StartTime, EndTime, Duration) VALUES('{startTime}', '{endTime}', '{DateDifference.Duration}')";
                tableCmd.ExecuteNonQuery();

                DateTime parsedStartTime = DateTime.ParseExact(startTime, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
                DateTime parsedEndTime = DateTime.ParseExact(endTime, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);

                var firstGoal = Goals.FirstOrDefault();

                var tableCmd2 = connection.CreateCommand();

                tableCmd.CommandText = "SELECT ProgressRemaining FROM goals";
                SqliteDataReader reader = tableCmd.ExecuteReader();

                TimeSpan progressRemaining = TimeSpan.Zero;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        progressRemaining = TimeSpan.Parse(reader.GetString(0));
                    }
                }

                    if (DateTime.ParseExact(startTime, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture) >= firstGoal.AddedDate)
                {
                    DateDifference dateDifference = new DateDifference(parsedStartTime, parsedEndTime);
                    firstGoal.ProgressRemaining = progressRemaining;
                    firstGoal.ProgressRemaining -= DateDifference.Duration;
                    var tableCmd3 = connection.CreateCommand();

                    tableCmd3.CommandText = $"UPDATE goals SET ProgressRemaining = '{firstGoal.ProgressRemaining}'";

                    tableCmd3.ExecuteNonQuery();
                }

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
                DateTime DateTimeNow1 = DateTime.Now;
                var stopwatchElapsed = stopwatch.Elapsed;
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    stopwatch.Stop();
                    var DateTimeNow2 = DateTime.Now;
                    Console.WriteLine("Stopwatch stopped");
                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();

                        var tableCmd = connection.CreateCommand();

                        var parsed1 = DateTimeNow1.ToString("dd/MM/yyyy HH:mm");
                        var parsed2 = DateTimeNow2.ToString("dd/MM/yyyy HH:mm");

                        var dur = DateTime.Now.Subtract(stopwatchElapsed).ToString("dd/MM/yyyy HH:mm");

                        tableCmd.CommandText = $"INSERT INTO coding_sessions(StartTime, EndTime, Duration) VALUES('{dur}', '{parsed2}', '{stopwatchElapsed}')";

                        tableCmd.ExecuteNonQuery();

                        connection.Close();
                    }
                    break;
                }
                Console.SetCursorPosition(0, Console.CursorTop);

                
                Console.Write($"Elapsed time: {stopwatchElapsed}");
                Thread.Sleep(100);
            }

            Console.ReadKey();
        }

        public static void DisplayAllRecords()
        {
            Console.Clear();
            var codingSessionsCopy = CodingSessions.ToList();
            TableVisualizationEngine.DisplayInTableFormatCodingSessions(codingSessionsCopy);
            codingSessionsCopy.Clear();
            Filter();
        }


        public static void Update()
        {
            DisplayAllRecords();
            int recordId = UserInput.GetNumberInput("Type the ID of the record you want to update. Type M to return to the main menu.");
            foreach (var dcs in DeletedCodingSessions)
            {
                if (recordId > dcs.Id)
                {
                    recordId--;
                }
            }

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

        public static void Filter()
        {
            Console.WriteLine("If you want to filter your records by time period, type: \n\n J to see your coding sessions in the last year \n L to see your coding sessions in the last 30 days \n W to see your coding sessions in the last week \n D to see your coding sessions in the last day \n\nIf you want to get back to the main menu, type M.");
            void FilterBy(int amountOfDays)
            {
                var FiltBy = CodingSessions.Where(cs => (cs.StartTime - CodingSessions.LastOrDefault().StartTime).Days <= amountOfDays).ToList();
                TableVisualizationEngine.DisplayInTableFormatCodingSessions(FiltBy);
                
            }
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "J":
                        Console.Clear();
                        Console.WriteLine("Your coding sessions in the last 365 days:\n");
                        FilterBy(365);
                        Filter();
                        break;
                    case "L":
                        Console.Clear();
                        Console.WriteLine("Your coding sessions in the last 30 days:\n");
                        FilterBy(30);
                        Filter();
                        break;
                    case "W":
                        Console.Clear();
                        Console.WriteLine("Your coding sessions in the last 7 days:\n");
                        FilterBy(7);
                        Filter();
                        break;
                    case "D":
                        Console.Clear();
                        Console.WriteLine("Your coding sessions in the last day:\n");
                        FilterBy(1);
                        Filter();
                        break;
                    case "M":
                        UserInput.GetUserInput();
                        break;
                }
            }
        }



        public static void SetGoal()
        {
            if (Goals.Count == 1)
            {
                Console.WriteLine("You have to finish your first goal before adding another one!");
                UserInput.GetUserInput();
            }
            var timeUnit = UserInput.GetGoalMeasureInput();
            var goalValue = UserInput.GetGoalInput();
            switch (timeUnit)
            {
                case "mins":
                    ParseGoalIntoTimeSpan(TimeSpan.FromMinutes, goalValue, "hours", 1);
                    break;
                case "h":
                    ParseGoalIntoTimeSpan(TimeSpan.FromHours, goalValue, "hours", 1);
                    break;
                case "d":
                    ParseGoalIntoTimeSpan(TimeSpan.FromDays, goalValue, "hours", 1);
                    break;
                case "w":
                    ParseGoalIntoTimeSpan(TimeSpan.FromDays, goalValue, "hours", 7);
                    break;
                case "mo":
                    ParseGoalIntoTimeSpan(TimeSpan.FromDays, goalValue, "hours", 30);
                    break;
                case "y":
                    ParseGoalIntoTimeSpan(TimeSpan.FromDays, goalValue, "hours", 365);
                    break;
            }
            TimeSpan ParseGoalIntoTimeSpan(Func<double, TimeSpan> FromTimeUnit, string goal, string timeUnitInPrintedMsg, int number)
            {
                double timeUnit = double.Parse(goal);
                TimeSpan goalTimeSpan = FromTimeUnit(timeUnit * number);
                Console.WriteLine(goalTimeSpan);
                Console.WriteLine($"Your goal is of {timeUnit} {timeUnitInPrintedMsg}!");

                if (timeUnit <= 1 && timeUnitInPrintedMsg == "hours")
                {
                    timeUnitInPrintedMsg = "hour";
                }

                
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var tableCmd = connection.CreateCommand();

                    string formattedDateNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                    DateTime dateTime;

                    bool success = DateTime.TryParseExact(formattedDateNow, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

                    tableCmd.CommandText = $"INSERT INTO goals(Goal, AddedDate, ProgressRemaining) VALUES('{goalTimeSpan}', '{formattedDateNow}', '{goalTimeSpan}')";

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }

                Console.WriteLine(goalTimeSpan);
                Console.WriteLine($"Your goal is of {timeUnit} {timeUnitInPrintedMsg}!");

                if (timeUnit <= 1 && timeUnitInPrintedMsg == "hours")
                {
                    timeUnitInPrintedMsg = "hour";
                }

                Console.Clear();
                UserInput.GetUserInput();
                return goalTimeSpan;

            }

        }

        public static void LoadAllRecords()
        {
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

                var codingSessionsCopy = CodingSessions.ToList();

                foreach (var cs in codingSessionsCopy)
                {
                    foreach (var ds in DeletedCodingSessions)
                    {
                        if (cs.Id > ds.Id)
                        {
                            cs.Id--;
                        }
                    }
                }
                connection.Close();
                codingSessionsCopy.Clear();
            }
        }

        public static void GetAllGoalRecords()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd2 = connection.CreateCommand();

                tableCmd2.CommandText = "SELECT * FROM goals";

                tableCmd2.ExecuteNonQuery();

                SqliteDataReader reader = tableCmd2.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Goals.Add(new Goal
                        {
                            Id = reader.GetInt32(0),
                            GoalValue = TimeSpan.Parse(reader.GetString(1)),
                            AddedDate = DateTime.Parse(reader.GetString(2), CultureInfo.CurrentCulture),
                            ProgressRemaining = TimeSpan.Parse(reader.GetString(1))
                        });
                    }
                }
                connection.Close();
            }
        }

        public static void Delete()
        {
            Console.Clear();
            DisplayAllRecords();
            int recordId = UserInput.GetNumberInput("Type the ID of the record you want to delete. Type M to return to the main menu.");
            foreach (var dcs in DeletedCodingSessions)
            {
                if (recordId > dcs.Id)
                {
                    recordId--;
                }
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM coding_sessions WHERE Id = {recordId}";

                var deletedRows = tableCmd.ExecuteNonQuery();

                if (deletedRows == 0)
                {
                    Console.WriteLine($"Record with ID {recordId} does not exist.");
                    Delete();
                }

                DeletedCodingSessions.Add(new CodingSession
                {
                    Id = recordId,
                    StartTime = CodingSessions.Where(i => i.Id == recordId).Select(i => i.StartTime).FirstOrDefault(),
                    EndTime = CodingSessions.Where(i => i.Id == recordId).Select(i => i.EndTime).FirstOrDefault(),
                    Duration = CodingSessions.Where(i => i.Id == recordId).Select(i => i.Duration).FirstOrDefault()
                });

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
