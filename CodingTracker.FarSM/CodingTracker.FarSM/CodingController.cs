using CodingTracker.FarSM.Models;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;

namespace CodingTracker.FarSM
{
    internal class CodingController
    {
        private static string conn = ConfigurationManager.AppSettings.Get("db_connection");

        public static void UpdateEntry()
        {
            ViewAllEntries();
            int rowToUpdate = Inputs.GetNumberInput("Type the row number that has to be updated.");

            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_tracker WHERE Id={rowToUpdate})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (checkQuery == 0)
                {
                    Console.WriteLine($"\nRecord with Id {rowToUpdate} doesn't exist");
                    connection.Close();
                    UpdateEntry();
                }
                else
                {
                    DateTime Date = Inputs.GetDateInput("\nEnter the date to be inserted: ");
                    string date = Date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                    DateTime STime = Inputs.GetTimeInput("\nEnter the start time: ");
                    string startTime = STime.ToString("hh:mm", CultureInfo.InvariantCulture);

                    DateTime ETime = Inputs.GetTimeInput("\nEnter the end time: ");
                    DateTime validEndTime = Helpers.CompareDates(STime,ETime);
                    string endTime = validEndTime.ToString("hh:mm", CultureInfo.InvariantCulture);

                    TimeSpan Duration = Helpers.CalculateDuration(validEndTime, STime);
                    string duration = Duration.ToString(@"hh\:mm");

                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"UPDATE coding_tracker SET Date='{date}', Start_Time='{startTime}',End_Time='{endTime}',Duration='{duration}' WHERE Id={rowToUpdate}";
                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine($"Row {rowToUpdate} is updated.");
                }
            }
        }

        public static void DeleteEntry()
        {
            ViewAllEntries();
            int rowToDelete = Inputs.GetNumberInput("Type the row number that has to be deleted.");

            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE FROM coding_tracker WHERE Id='{rowToDelete}'";
                int rowCount = tableCmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    Console.Write($"\nRecord with Id {rowToDelete} doesn't exist. Press any key to re-enter");
                    Console.ReadLine();
                    Console.Clear();
                    DeleteEntry();
                }
                else
                {
                    connection.Close();
                    Console.WriteLine($"\nThe record with the Id {rowToDelete} is deleted. Press any key to go to the main menu");
                    Console.ReadLine();
                    Inputs.MainMenu();
                }
            }
        }

        public static void InsertEntry()
        {
            DateTime Date = Inputs.GetDateInput("\nEnter the date: ");
            string date = Date.ToString("dd-MM-yyyy",CultureInfo.InvariantCulture);

            DateTime StartTime = Inputs.GetTimeInput("Enter the start time: ");
            string startTime = StartTime.ToString("hh:mm",CultureInfo.InvariantCulture);

            DateTime EndTime = Inputs.GetTimeInput("Enter the end time: ");
            DateTime validEndTime = Helpers.CompareDates(StartTime, EndTime);
            string endTime = validEndTime.ToString("hh:mm", CultureInfo.InvariantCulture);

            TimeSpan Duration = Helpers.CalculateDuration(validEndTime,StartTime);
            string duration = Duration.ToString(@"hh\:mm");

            Console.WriteLine("Coding duration: " + duration);
            Console.WriteLine("Record entered.\n");

            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO coding_tracker (Date,Start_Time,End_Time,Duration) VALUES('{date}','{startTime}','{endTime}','{duration}')";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void ViewAllEntries()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM coding_tracker";
                List<CodingSession> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            date = DateTime.ParseExact(reader.GetString(1),"dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None), 
                            startTime = DateTime.Parse(reader.GetString(2)),
                            endTime = DateTime.Parse(reader.GetString(3)),
                            duration = TimeSpan.Parse(reader.GetString(4))                       

                        });
                    }
                }
                else Console.WriteLine("No data found");
                connection.Close();

                Helpers.PrintData(tableData,"All entries");
            }
        }

        internal static void RecordCodingTime()
        {
            string start;
            string stop;
            DateTime sTime = new DateTime();
            DateTime eTime = new DateTime();

            Stopwatch sw = Stopwatch.StartNew();

            Console.Clear();
            Console.WriteLine("\nPress any key to START recording your coding time");
            start = Console.ReadLine();
            if (start != null)
            {
                sTime = DateTime.Now; 
                sw.Start();
            }
            Console.WriteLine("\nCODING.......");
            Console.WriteLine("\nPress any key to STOP recording your coding time");
            Console.WriteLine("\n.......CODING.......");

            stop = Console.ReadLine();
            if (stop != null)
            {
                eTime = DateTime.Now;
                sw.Stop();
            }
            TimeSpan timeElapsed = sw.Elapsed;
            Console.WriteLine("\nCoding Time is " + timeElapsed.ToString(@"m\:ss\.ff"));

            string date = DateTime.Today.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            string startTime = sTime.ToString("hh:mm", CultureInfo.InvariantCulture);
            string endTime = eTime.ToString("hh:mm", CultureInfo.InvariantCulture);
            string duration = timeElapsed.ToString(@"hh\:mm");

            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO coding_tracker (Date,Start_Time,End_Time,Duration) VALUES('{date}','{startTime}','{endTime}','{duration}')";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
            Console.WriteLine("\nThe coding session has been entered");
        }

        internal static void LastFourWeeksData()
        {
            using(var connection = new SqliteConnection(conn))
            {
                DateTime currentDate = DateTime.Now;
                int weekNo = 1;
                int dayCount = 7;
                TimeSpan totalCodingHours =new TimeSpan();
                int totalDaysCoded = 0;

                for (int i = 4; i > 0 ; i--)
                {
                    int daysCoded = 0;
                    TimeSpan codingHours = new TimeSpan();
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"SELECT * FROM coding_tracker";
                    List<DateDuration> TableDateDuration = new();
                    SqliteDataReader reader = tableCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")) >=
                               currentDate.AddDays(-(dayCount * i)) &&
                               DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")) <=
                               currentDate.AddDays((-(dayCount * i)) + 7))
                            {
                                TableDateDuration.Add(new DateDuration
                                {
                                    date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                                    duration = TimeSpan.Parse(reader.GetString(4))
                                });
                                daysCoded++;
                                codingHours += TimeSpan.Parse(reader.GetString(4));
                            }
                        }
                    }
                    else Console.WriteLine("No data found");
                    connection.Close();
                    if (TableDateDuration.Count == 0) { Console.WriteLine("\n No data found matching the dates"); }
                    else
                    {
                        string weekTitle = "Week: " + weekNo;
                        Helpers.PrintDuration(TableDateDuration, weekTitle);
                    }
                    weekNo++;
                    totalCodingHours += codingHours;
                    totalDaysCoded += daysCoded;
                    Console.WriteLine("\nNumber of days when coding was done: " + daysCoded);
                    Console.WriteLine("Total number of hours coded: {0:0.##}", codingHours.TotalHours);
                    Console.WriteLine("Average coding done per day of the week: {0:0.##}", codingHours.TotalHours / 7);
                    Console.WriteLine("-----------------------------------------------------------------");
                    Console.WriteLine();
                }

                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Total number of days coded in the last 4 weeks: "+ totalDaysCoded);
                Console.WriteLine("Total number of hours coded in the last 4 weeks: " + totalCodingHours.TotalHours);
                Console.WriteLine("Average number of hours coded per days coding was done: {0:0.##}" , totalCodingHours.TotalHours / totalDaysCoded);
                Console.WriteLine("Average number of hours coded per day in the last 4 weeks: {0:0.##}" , totalCodingHours.TotalHours / 28);
                Console.WriteLine("-----------------------------------------------------------------");
            }
        }

        internal static void SetCodingGoals()
        {
            Console.Clear();
            Console.Write("\nEnter the number of hours you want to code this month: ");
            string GoalTime = Console.ReadLine();
            double goalTime = 0;
            while (!Double.TryParse(GoalTime, out goalTime) || Convert.ToDecimal(GoalTime)<0)
            {
                Console.Write("Invalid input. Please enter a number or decimal: ");
                GoalTime = Console.ReadLine();
            }

            string currentMonth = DateTime.Now.ToString("MM");
            TimeSpan hoursCoded = new TimeSpan();
            DateTime currentDate = DateTime.Now;
            Console.WriteLine("---------------------------------------------------------------------------");

            using (var connection = new SqliteConnection(conn))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM coding_tracker WHERE substr(Date, 4, 2) = '{currentMonth}'";
                List<CodingSession> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None),
                            startTime = DateTime.Parse(reader.GetString(2)),
                            endTime = DateTime.Parse(reader.GetString(3)),
                            duration = TimeSpan.Parse(reader.GetString(4))
                        });
                        hoursCoded += TimeSpan.Parse(reader.GetString(4));
                    }
                }
                else Console.WriteLine("\nNo coding done this month");
                connection.Close();
                if (goalTime > hoursCoded.TotalHours)
                {
                    Double timeLeft = goalTime - hoursCoded.TotalHours;
                    int daysLeft = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day;
                    Console.WriteLine("Coding hours left: {0:0.####}" , timeLeft);
                    Console.WriteLine("Hours to be coded per day for the next of the month: {0:0.####}", timeLeft / daysLeft);
                }
                else
                {
                    Console.WriteLine("Woohoo! You have surpassed your coding time goal. Go breath some fresh air!");
                }
                Console.WriteLine("---------------------------------------------------------------------------");
            }

        }
    }
}